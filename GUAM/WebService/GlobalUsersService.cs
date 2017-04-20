using EmailHelper;
using GlobalUsers.DataHelper;
using GlobalUsers.Entities;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Text.RegularExpressions;

namespace WebService
{
    /*
     * Note - this file has been decompiled from the WebService dll provided by Leidos.
     * Most of the changes in this file involve setting the correct object types, i.e.
     *** GlobalUsers.Entities.EventType vs WebService.GlobalUsersService.EventType and
     *** GlobalUsers.Entities.User vs WebService.User
     *** NHibernate.ISession vs System.ServiceModel.Channels.ISession
     */
    [ApplicationAuthorization]
    public class GlobalUsersService : IGlobalUsersService
    {
        private const string SYSTEM_EMAIL = "globalusermanagement-noreply@lmbps.com";

        private static bool UseEventLog;

        private NHibernate.ISession Database = NhibernateBootStrapper.GetSession();
        public Application CurrentApplication
        {
            get;
            set;
        }

        public string CurrentClientIPAddress
        {
            get
            {
                return OperationContext.Current.IncomingMessageHeaders.GetHeader<string>("client-ip-address", "http://aspensys.com/");
            }
        }

        public bool IsCurrentApplicationManager
        {
            get
            {
                // auth hack - daquinohd
                // ServiceSecurityContext.PrimaryIdentity.Name is not returning a value, so I'm 
                // using System.Security.Principal.WindowsIdentity.GetCurrent().Name to get the AppPool Identity for now.
                string name = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;
                name = System.Security.Principal.WindowsIdentity.GetCurrent().Name;

                Account account = this.Database.Query<Account>().FirstOrDefault<Account>((Account acc) => acc.Username.ToLower() == name.ToLower());
                return (account == null ? false : account.Admin);
            }
        }

        static GlobalUsersService()
        {
            GlobalUsersService.UseEventLog = (string.IsNullOrEmpty(ConfigurationManager.AppSettings["EnableEventLog"]) ? false : Convert.ToBoolean(ConfigurationManager.AppSettings["EnableEventLog"]));
        }

        public GlobalUsersService()
        {
            OperationContext.Current.OperationCompleted += new EventHandler((object sender, EventArgs e) =>
            {
                if (this.Database != null)
                {
                    this.Database.Flush();
                    this.Database.Close();
                    this.Database.Dispose();
                }
            });
        }

        private void _AddPassword(int ApplicationID, int UserID, string Password)
        {
            this._AddPassword(ApplicationID, UserID, Password, null);
        }

        private void _AddPassword(int ApplicationID, int UserID, string Password, DateTime? expiration)
        {
            byte[] original_salt = WebService.Password.CreateRandomSalt();
            byte[] original_pass = WebService.Password.CreateSHA512Hash(Password, original_salt);
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GC"].ConnectionString);
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("INSERT INTO PasswordHistory(UserID, PasswordDate, PasswordSalt, PasswordHash, Expiration) VALUES(@UserID, @PasswordDate, @PasswordSalt, @PasswordHash, @Expiration)", connection);
                try
                {
                    command.Parameters.AddWithValue("@UserID", UserID);
                    command.Parameters.AddWithValue("@PasswordDate", DateTime.Now);
                    command.Parameters.AddWithValue("@PasswordSalt", original_salt);
                    command.Parameters.AddWithValue("@PasswordHash", original_pass);
                    SqlParameterCollection parameters = command.Parameters;
                    object value = expiration;
                    if (value == null)
                    {
                        value = DBNull.Value;
                    }
                    parameters.AddWithValue("@Expiration", value);
                    command.ExecuteNonQuery();
                }
                finally
                {
                    if (command != null)
                    {
                        ((IDisposable)command).Dispose();
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    ((IDisposable)connection).Dispose();
                }
            }
        }

        private bool _ExistsUsername(int ApplicationID, string Username)
        {
            int? ret;
            return this._ExistsUsername(ApplicationID, Username, out ret);
        }

        private bool _ExistsUsername(int ApplicationID, string Username, out int? UserID)
        {
            GlobalUsers.Entities.User User;
            bool flag;
            if (!this._ExistsUsername(ApplicationID, Username, out User))
            {
                UserID = null;
                flag = false;
            }
            else
            {
                UserID = new int?(User.UserID);
                flag = true;
            }
            return flag;
        }

        private bool _ExistsUsername(int ApplicationID, string Username, out GlobalUsers.Entities.User User)
        {
            IQueryable<int> nums =
                from ta in this.Database.Query<ApplicationTrust>()
                where ta.Application.ApplicationID == ApplicationID
                select ta.TrustedApplication.ApplicationID;
            IQueryable<GlobalUsers.Entities.User> user_query =
                from u in this.Database.Query<GlobalUsers.Entities.User>()
                where (u.Application.ApplicationID == ApplicationID || nums.Contains<int>(u.Application.ApplicationID)) && (u.Username.ToLower() == Username.ToLower())
                select u;
            User = user_query.FirstOrDefault<GlobalUsers.Entities.User>();
            if (User == null)
            {
                User = (
                    from at in this.Database.Query<ApplicationTrust>()
                    join u in this.Database.Query<GlobalUsers.Entities.User>() on at.TrustedApplication.ApplicationID equals u.Application.ApplicationID
                    where at.Application.ApplicationID == ApplicationID && (u.Username.ToLower() == Username.ToLower())
                    select u).FirstOrDefault<GlobalUsers.Entities.User>();
            }
            return User != null;
        }

        private int _GetApplicationID(string ApplicationName)
        {
            Application application = this.Database.Query<Application>().FirstOrDefault<Application>((Application a) => a.ApplicationName.ToLower() == ApplicationName.ToLower());
            return (application != null ? application.ApplicationID : -1);
        }

        private int? _GetCallerIP()
        {
            int? nullable;
            if (OperationContext.Current.IncomingMessageHeaders.FindHeader("client-ip-address", "http://aspensys.com/") != -1)
            {
                string strIP = OperationContext.Current.IncomingMessageHeaders.GetHeader<string>("client-ip-address", "http://aspensys.com/");
                nullable = new int?(this._IPToInt(strIP));
            }
            else
            {
                nullable = null;
            }
            return nullable;
        }

        private int _GetPasswordExpirationDaysLeft(int ApplicationID, int UserID)
        {
            int num;
            ApplicationInformation ai = this.GetApplicationInformation();
            if (ai.PasswordExpirationDays != 0)
            {
                if (ai.PasswordExpirationWarningWindow > ai.PasswordExpirationDays)
                {
                    throw new Exception("Password expiration warning window cannot be longer than password expiration length.");
                }
                PasswordHistory pwhistory = (
                    from t in this.Database.Query<PasswordHistory>()
                    where t.UserID == UserID
                    orderby t.PasswordDate descending
                    select t).FirstOrDefault<PasswordHistory>();
                if (pwhistory != null)
                {
                    int nDaysOld = this._MidnightsBetweenDates(pwhistory.PasswordDate, DateTime.Now);
                    int nDaysLeft = ai.PasswordExpirationDays - nDaysOld;
                    num = (nDaysLeft >= 0 ? nDaysLeft : 0);
                }
                else
                {
                    num = -1;
                }
            }
            else
            {
                num = -1;
            }
            return num;
        }

        private string _IntToIP(int nIP)
        {
            return (new IPAddress(BitConverter.GetBytes(nIP))).ToString();
        }

        private int _IPToInt(string strIP)
        {
            int num = BitConverter.ToInt32(IPAddress.Parse(strIP).GetAddressBytes(), 0);
            return num;
        }

        private bool _IsEnabled(int ApplicationID, string Username)
        {
            GlobalUsers.Entities.User uu = (
                from t in this.Database.Query<GlobalUsers.Entities.User>()
                where t.Application.ApplicationID == ApplicationID && (t.Username.ToLower() == Username.ToLower())
                select t).FirstOrDefault<GlobalUsers.Entities.User>();
            return (uu != null ? uu.IsEnabled : false);
        }

        private bool _IsIPLockedOut()
        {
            bool flag;
            ApplicationInformation applicationInformation = this.GetApplicationInformation();
            if (applicationInformation.IPAttemptsBeforeLockout != 0)
            {
                int? nullable = this._GetCallerIP();
                if (nullable.HasValue)
                {
                    IQueryable<AuditLog> logEvents =
                        from t in this.Database.Query<AuditLog>()
                        where t.UserIP == (int)nullable && (t.EventType.EventTypeID == 1 || t.EventType.EventTypeID == 2 || t.EventType.EventTypeID == 4) && (t.EventDate > DateTime.Now.AddMinutes((double)(-1 * applicationInformation.IPLockoutWindow)))
                        select t;
                    flag = logEvents.Count<AuditLog>() >= applicationInformation.IPAttemptsBeforeLockout;
                }
                else
                {
                    flag = false;
                }
            }
            else
            {
                flag = false;
            }
            return flag;
        }

        private bool _IsPasswordExpired(int ApplicationID, string Username)
        {
            bool flag;
            int? nullable;
            this._ExistsUsername(this.CurrentApplication.ApplicationID, Username, out nullable);
            ApplicationInformation ai = this.GetApplicationInformation();
            if (ai.PasswordExpirationDays != 0)
            {
                PasswordHistory pwhistory = (
                    from t in this.Database.Query<PasswordHistory>()
                    where (int?)t.UserID == nullable
                    orderby t.PasswordDate descending
                    select t).FirstOrDefault<PasswordHistory>();
                if (pwhistory != null)
                {
                    flag = (this._MidnightsBetweenDates(pwhistory.PasswordDate, DateTime.Now) < ai.PasswordExpirationDays ? false : true);
                }
                else
                {
                    flag = false;
                }
            }
            else
            {
                flag = false;
            }
            return flag;
        }

        private bool _IsValidUsername(string Username)
        {
            bool flag;
            if (!string.IsNullOrEmpty(this.GetApplicationInformation().ValidUsernameRegex))
            {
                Regex reUsername = new Regex(this.GetApplicationInformation().ValidUsernameRegex);
                flag = reUsername.IsMatch(Username);
            }
            else
            {
                flag = true;
            }
            return flag;
        }

        private int _MidnightsBetweenDates(DateTime dtEarlier, DateTime dtLater)
        {
            TimeSpan date = dtLater.Date - dtEarlier.Date;
            return Convert.ToInt32(date.TotalDays);
        }

        private void _ParseAndAddRecipients(MailMessage mm, string RecipientList)
        {
            char[] chrArray = new char[] { ',', ';' };
            string[] strArrays = RecipientList.Split(chrArray);
            for (int i = 0; i < (int)strArrays.Length; i++)
            {
                string recipient = strArrays[i];
                mm.To.Add(new MailAddress(recipient.Trim()));
            }
        }

        private void _RecordAuditEvent(int ApplicationID, string Username, GlobalUsersService.EventType evt)
        {
            this._WriteToAuditLog(ApplicationID, Username, evt, @"N/A");
            // this._RecordAuditEvent(ApplicationID, Username, evt, null);
        }

        /// <summary>
        /// Record audit events to the auditing log in the DB.
        /// </summary>
        /// <param name="ApplicationID">int application id</params>
        /// <param name="Username">string user name</params>
        /// <param name="evt">GlobalUsersService.EventType ;</params>
        private void _WriteToAuditLog(int ApplicationID, String Username, GlobalUsersService.EventType evt, String comment)
        {
            int userIP = this._IPToInt(this.CurrentClientIPAddress);
            int eventTypeID = -1;
            string eventTypeValue = evt.ToString();

            switch (eventTypeValue)
            {
                case "LOGIN_SUCCESS":
                    eventTypeID = 0;
                    break;
                case "LOGIN_FAILURE_BADPASSWORD":
                    eventTypeID = 1;
                    break;
                case "LOGIN_FAILURE_BADUSERNAME":
                    eventTypeID = 2;
                    break;
                case "LOGIN_FAILURE_LOCKEDOUT":
                    eventTypeID = 3;
                    break;
                case "LOGIN_FAILURE_LOCKEDOUTIP":
                    eventTypeID = 4;
                    break;
                case "LOGIN_FAILURE_DISABLED":
                    eventTypeID = 5;
                    break;
                case "PASSWORD_CHANGED":
                    eventTypeID = 6;
                    break;
                case "PASSWORD_ASSIGNED":
                    eventTypeID = 7;
                    break;
                case "PASSWORD_RESET":
                    eventTypeID = 8;
                    break;
                case "USER_ADDED_TO_ROLE":
                    eventTypeID = 9;
                    break;
                case "USER_REMOVED_FROM_ROLE":
                    eventTypeID = 10;
                    break;
                case "USER_CREATED":
                    eventTypeID = 11;
                    break;
                case "LOGIN_FAILURE_INACTIVE":
                    eventTypeID = 12;
                    break;
                case "USERNAME_CHANGED":
                    eventTypeID = 13;
                    break;
                case "USER_INACTIVED":
                    eventTypeID = 14;
                    break;
                case "USER_ACTIVED":
                    eventTypeID = 15;
                    break;
                case "USER_DISABLED":
                    eventTypeID = 16;
                    break;
                case "USER_ENABLED":
                    eventTypeID = 17;
                    break;
                default:
                    eventTypeID = -1;
                    break;
            }

            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GC"].ConnectionString);
            try
            {
                connection.Open();
                string cmd = @"INSERT INTO AuditLog(Username, ApplicationID, EventDate, EventTypeID, EventStatusID, UserIP, Comment)
                             VALUES(@Username, @ApplicationID, @EventDate, @EventTypeID, @EventStatusID, @UserIP, @Comment)";
                SqlCommand command = new SqlCommand(cmd, connection);
                try
                {
                    // Add new audit entry to the database
                    command.Parameters.AddWithValue("@Username", Username);
                    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                    command.Parameters.AddWithValue("@EventDate", DateTime.Now);
                    command.Parameters.AddWithValue("@EventTypeID", eventTypeID);
                    command.Parameters.AddWithValue("@EventStatusID", 0);
                    command.Parameters.AddWithValue("@UserIP", userIP);
                    command.Parameters.AddWithValue("@Comment", comment);
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    if (command != null)
                    {
                        ((IDisposable)command).Dispose();
                    }
                }
                finally
                {
                    if (command != null)
                    {
                        ((IDisposable)command).Dispose();
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    ((IDisposable)connection).Dispose();
                }
            }
        }

        // This is not being hit for now 
        // TODO: fix event logging & logic for binaryExpressions -- daquinohd
        private void _RecordAuditEvent(int ApplicationID, string Username, GlobalUsersService.EventType evt, string Comment)
        {
            GlobalUsersService variable = null;
            ParameterExpression parameterExpression;
            ParameterExpression[] parameterExpressionArray;
            if (GlobalUsersService.UseEventLog)
            {
                if (!EventLog.SourceExists("GUAM"))
                {
                    EventLog.CreateEventSource("GUAM", "Application");
                }
                object[] username = new object[] { Username, null, null, null };
                IQueryable<Application> applications = this.Database.Query<Application>();
                parameterExpression = Expression.Parameter(typeof(Application), "a");
                //BinaryExpression binaryExpression = Expression.Equal(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(Application).GetMethod("get_ApplicationID").MethodHandle)), Expression.Field(Expression.Constant(variable), FieldInfo.GetFieldFromHandle(typeof(GlobalUsersService.<>c__DisplayClass41).GetField("ApplicationID").FieldHandle)));
                BinaryExpression binaryExpression = Expression.Equal(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(Application).GetMethod("get_ApplicationID").MethodHandle)), Expression.Field(Expression.Constant(variable), FieldInfo.GetFieldFromHandle(typeof(GlobalUsersService).GetField("ApplicationID").FieldHandle)));
                parameterExpressionArray = new ParameterExpression[] { parameterExpression };
                username[1] = applications.First<Application>(Expression.Lambda<Func<Application, bool>>(binaryExpression, parameterExpressionArray)).ApplicationName;
                username[2] = evt.ToString();
                username[3] = Comment;
                EventLog.WriteEntry("GUAM", string.Format("{0}:{1}:{2}:{3}", username));
            }
            AuditLog al = new AuditLog()
            {
                Application = this.Database.Get<Application>(ApplicationID),
                Username = Username,
                EventDate = DateTime.Now
            };
            if (OperationContext.Current.IncomingMessageHeaders.FindHeader("client-ip-address", "http://aspensys.com/") != -1)
            {
                string strIP = OperationContext.Current.IncomingMessageHeaders.GetHeader<string>("client-ip-address", "http://aspensys.com/");
                al.UserIP = this._IPToInt(strIP);
            }
            else
            {
                al.UserIP = 0;
            }
            IQueryable<GlobalUsers.Entities.EventType> eventTypes = this.Database.Query<GlobalUsers.Entities.EventType>();
            parameterExpression = Expression.Parameter(typeof(GlobalUsers.Entities.EventType), "et");
            BinaryExpression binaryExpression1 = Expression.Equal(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(GlobalUsers.Entities.EventType).GetMethod("get_EventTypeID").MethodHandle)), Expression.Convert(Expression.Field(Expression.Constant(variable), FieldInfo.GetFieldFromHandle(typeof(GlobalUsersService).GetField("evt").FieldHandle)), typeof(int)));
            parameterExpressionArray = new ParameterExpression[] { parameterExpression };
            GlobalUsers.Entities.EventType type = eventTypes.FirstOrDefault<GlobalUsers.Entities.EventType>(Expression.Lambda<Func<GlobalUsers.Entities.EventType, bool>>(binaryExpression1, parameterExpressionArray));
            if (type == null)
            {
                type = new GlobalUsers.Entities.EventType()
                {
                    EventTypeID = (int)evt,
                    Name = Enum.GetName(typeof(GlobalUsersService.EventType), evt)
                };
                this.Database.SaveOrUpdate(type);
            }
            al.EventType = type;
            al.EventStatus = this.Database.Get<EventStatus>(0);
            al.Comment = Comment;
            this.Database.SaveOrUpdate(al);
        }

        private List<GlobalUsers.Entities.User> _SearchUserByMetaData(int ApplicationID, List<KeyValuePair<string, string>> MetaData, bool set_union)
        {
            int mdv;
            List<GlobalUsers.Entities.User> list;
            List<GlobalUsers.Entities.User> results = null;
            foreach (KeyValuePair<string, string> metaDatum in MetaData)
            {
                List<GlobalUsers.Entities.User> r = new List<GlobalUsers.Entities.User>();
                r = (!int.TryParse(metaDatum.Key, out mdv) ? (
                    from u in this.Database.Query<GlobalUsers.Entities.User>()
                    from md in this.Database.Query<UserMetaDatum>()
                    where u.Application.ApplicationID == ApplicationID && md.User.UserID == u.UserID && (md.DataKey == metaDatum.Key) && md.DataValue.ToLower().Contains(metaDatum.Value.ToLower())
                    select u).ToList<GlobalUsers.Entities.User>() : (
                    from u in this.Database.Query<GlobalUsers.Entities.User>()
                    from md in this.Database.Query<UserMetaDatum>()
                    where u.Application.ApplicationID == ApplicationID && md.User.UserID == u.UserID && (md.DataKey == metaDatum.Key) && (md.DataValue == metaDatum.Value)
                    select u).ToList<GlobalUsers.Entities.User>());
                if (results == null)
                {
                    list = r.ToList<GlobalUsers.Entities.User>();
                }
                else
                {
                    list = (set_union ? results.Union<GlobalUsers.Entities.User>(r, new StrictKeyEqualityComparer<GlobalUsers.Entities.User, int>((GlobalUsers.Entities.User x) => x.UserID)).ToList<GlobalUsers.Entities.User>() : results.Intersect<GlobalUsers.Entities.User>(r, new StrictKeyEqualityComparer<GlobalUsers.Entities.User, int>((GlobalUsers.Entities.User x) => x.UserID)).ToList<GlobalUsers.Entities.User>());
                }
                results = list;
            }
            return results;
        }

        private void _SendAdminEmail(string To, string Template, string Subject, string Username, Dictionary<string, string> custom_tokens)
        {
            this._SendEmailInternal(To, Template, Subject, Username, custom_tokens);
        }

        private void _SendAdminUserLockoutEmail(int ApplicationID, string Username)
        {
            ApplicationInformation ai = this.GetApplicationInformation();
            if (!string.IsNullOrEmpty(ai.UserLockoutEmailRecipients))
            {
                MailMessage mm = new MailMessage()
                {
                    From = new MailAddress("globalusermanagement-noreply@lmbps.com")
                };
                this._ParseAndAddRecipients(mm, ai.UserLockoutEmailRecipients);
                mm.Subject = string.Concat("[GUAM] Lockout alert for application ", ai.ApplicationName, ", user ", Username);
                string applicationName = ai.ApplicationName;
                DateTime now = DateTime.Now;
                mm.Body = string.Format("User {0} was locked out of application {1} at {2}. This email address has been configured to receive user lockout alerts in the application settings for {1}.", Username, applicationName, now.ToString("g"));
                (new SmtpClient()).Send(mm);
            }
        }

        private void _SendEmail(string Template, string Subject, string Username, Dictionary<string, string> custom_tokens)
        {
            this._SendEmailInternal(null, Template, Subject, Username, custom_tokens);
        }

        private void _SendEmailInternal(string To, string Template, string Subject, string Username, Dictionary<string, string> custom_tokens)
        {
            ApplicationInformation applicationInformation = this.GetApplicationInformation();
            GlobalUsers.Entities.User user = (
                from u in this.Database.Query<GlobalUsers.Entities.User>()
                where (u.Username.ToLower() == Username.ToLower()) && u.Application.ApplicationID == applicationInformation.ApplicationID
                select u).First<GlobalUsers.Entities.User>();
            ReplacementEmail mm = new ReplacementEmail()
            {
                Tokens = custom_tokens ?? new Dictionary<string, string>(),
                To = user
            };
            if (!string.IsNullOrEmpty(To))
            {
                mm.OverrideTo = To;
            }
            mm.From = new MailAddress((string.IsNullOrEmpty(applicationInformation.SendEmailsFrom) ? "no-reply@lmbps.com" : applicationInformation.SendEmailsFrom));
            mm.Bcc = applicationInformation.SendEmailsBCC;
            mm.SubjectTemplate = Subject.Replace('\r', ' ').Replace('\n', ' ');
            mm.BodyTemplate = Template;
            SmtpClient smtp = new SmtpClient();
            try
            {
                smtp.Send(mm);
            }
            finally
            {
                if (smtp != null)
                {
                    ((IDisposable)smtp).Dispose();
                }
            }
        }

        private void _SendIPLockoutEmail(int ApplicationID, int IP)
        {
            ApplicationInformation ai = this.GetApplicationInformation();
            if (!string.IsNullOrEmpty(ai.IPLockoutEmailRecipients))
            {
                MailMessage mm = new MailMessage()
                {
                    From = new MailAddress("globalusermanagement-noreply@lmbps.com")
                };
                this._ParseAndAddRecipients(mm, ai.IPLockoutEmailRecipients);
                mm.Subject = string.Concat("[GUAM] Lockout alert for application ", ai.ApplicationName, ", IP ", this._IntToIP(IP));
                object[] p = new object[] { this._IntToIP(IP), ai.ApplicationName, null, null, null };
                p[2] = DateTime.Now.ToString("g");
                p[3] = ai.IPAttemptsBeforeLockout;
                p[4] = ai.IPLockoutWindow;
                mm.Body = string.Format("The IP {0} was locked out of application {1} at {2}. It made more than {3} failed logins within a window of {4} minutes. This email address has been configured to receive user lockout alerts in the application settings for {1}.", p);
                (new SmtpClient()).Send(mm);
            }
        }

        private void _SendUserLockoutEmail(int ApplicationID, string Username)
        {
            ApplicationInformation ai = this.GetApplicationInformation();
            if ((string.IsNullOrEmpty(ai.UserLockoutEmailTemplate) ? false : !string.IsNullOrEmpty(ai.UserLockoutEmailSubject)))
            {
                this._SendEmail(ai.UserLockoutEmailTemplate, ai.UserLockoutEmailSubject, Username, null);
            }
        }

        private void _SendUserResetPasswordEmail(string Username)
        {
            ApplicationInformation ai = this.GetApplicationInformation();
            if (!string.IsNullOrEmpty(ai.UserPasswordResetRecipients))
            {
                MailMessage mm = new MailMessage()
                {
                    From = new MailAddress("globalusermanagement-noreply@lmbps.com")
                };
                this._ParseAndAddRecipients(mm, ai.UserPasswordResetRecipients);
                mm.Subject = string.Concat("[GUAM] Password reset for ", ai.ApplicationName, ", user ", Username);
                string applicationName = ai.ApplicationName;
                DateTime now = DateTime.Now;
                mm.Body = string.Format("User {0} reset their password for {1} at {2}. This email address has been configured to receive user password reset alerts in the application settings for {1}.", Username, applicationName, now.ToString("g"));
                (new SmtpClient()).Send(mm);
            }
        }

        private ReturnObject _SetUserPassword(string Username, string NewPassword, bool? MustChangePassword)
        {
            ReturnObject successReturnObject;
            int? nullable;
            if (this._ExistsUsername(this.CurrentApplication.ApplicationID, Username, out nullable))
            {
                ReturnObject ro = this.IsValidPassword(Username, NewPassword);
                if (ro.ReturnCode == 0)
                {
                    this._AddPassword(this.CurrentApplication.ApplicationID, nullable.Value, NewPassword);
                    if (MustChangePassword.HasValue)
                    {
                        GlobalUsers.Entities.User uu = (
                            from t in this.Database.Query<GlobalUsers.Entities.User>()
                            where (int?)t.UserID == nullable
                            select t).FirstOrDefault<GlobalUsers.Entities.User>();
                        uu.MustChangePassword = MustChangePassword.Value;
                        this.Database.SaveOrUpdate(uu);
                    }
                    this._RecordAuditEvent(this.CurrentApplication.ApplicationID, Username, GlobalUsersService.EventType.PASSWORD_ASSIGNED);
                    successReturnObject = new SuccessReturnObject();
                }
                else
                {
                    successReturnObject = new ErrorReturnObject(101, ro.DefaultErrorMessage);
                }
            }
            else
            {
                successReturnObject = new ErrorReturnObject(ErrorReturnObject.Error.UsernameNotFound);
            }
            return successReturnObject;
        }

        private bool _ShouldShowPasswordExpirationWarning(int ApplicationID, string Username)
        {
            bool flag;
            int? nullable;
            this._ExistsUsername(this.CurrentApplication.ApplicationID, Username, out nullable);
            ApplicationInformation ai = this.GetApplicationInformation();
            if (ai.PasswordExpirationDays == 0)
            {
                flag = false;
            }
            else if (ai.PasswordExpirationWarningWindow != 0)
            {
                if (ai.PasswordExpirationWarningWindow > ai.PasswordExpirationDays)
                {
                    throw new Exception("Password expiration warning window cannot be longer than password expiration length.");
                }
                PasswordHistory pwhistory = (
                    from t in this.Database.Query<PasswordHistory>()
                    where (int?)t.UserID == nullable
                    orderby t.PasswordDate descending
                    select t).FirstOrDefault<PasswordHistory>();
                if (pwhistory != null)
                {
                    int nDaysOld = this._MidnightsBetweenDates(pwhistory.PasswordDate, DateTime.Now);
                    flag = ((nDaysOld + 1 < ai.PasswordExpirationDays - ai.PasswordExpirationWarningWindow ? true : nDaysOld >= ai.PasswordExpirationDays) ? false : true);
                }
                else
                {
                    flag = false;
                }
            }
            else
            {
                flag = false;
            }
            return flag;
        }

        public ReturnObject AddRole(string RoleName)
        {
            ReturnObject successReturnObject;
            if ((
                from t in this.Database.Query<Role>()
                where t.Name.ToLower() == RoleName.ToLower()
                select t).FirstOrDefault<Role>() == null)
            {
                Role rrNew = new Role();
                Application application = new Application()
                {
                    ApplicationID = this.CurrentApplication.ApplicationID
                };
                rrNew.Application = application;
                rrNew.Name = RoleName;
                this.Database.SaveOrUpdate(rrNew);
                successReturnObject = new SuccessReturnObject();
            }
            else
            {
                successReturnObject = new ErrorReturnObject(101, "Role already exists.");
            }
            return successReturnObject;
        }

        public ReturnObject AddUser(string Username)
        {
            ReturnObject errorReturnObject;
            if (this._ExistsUsername(this.CurrentApplication.ApplicationID, Username))
            {
                errorReturnObject = new ErrorReturnObject(101, "Username exists.");
            }
            else if (this._IsValidUsername(Username))
            {
                GlobalUsers.Entities.User cc = new GlobalUsers.Entities.User();
                Application application = new Application()
                {
                    ApplicationID = this.CurrentApplication.ApplicationID
                };
                cc.Application = application;
                cc.Username = Username;
                cc.IsEnabled = true;
                cc.IsLockedOut = false;
                cc.IsActive = true;
                cc.IsPasswordExpired = false;
                cc.MustChangePassword = true;
                this.Database.SaveOrUpdate(cc);
                this._RecordAuditEvent(this.CurrentApplication.ApplicationID, Username, GlobalUsersService.EventType.USER_CREATED);
                errorReturnObject = new SuccessReturnObject((object)cc.UserID);
            }
            else
            {
                errorReturnObject = new ErrorReturnObject(102, "Username is invalid.");
            }
            return errorReturnObject;
        }

        public ReturnObject AddUsersToRoles(string[] Usernames, string[] RoleNames)
        {
            ReturnObject errorReturnObject;
            int? nullable;
            string[] usernames = Usernames;
            int num = 0;
            while (true)
            {
                if (num < (int)usernames.Length)
                {
                    string username = usernames[num];
                    if (this._ExistsUsername(this.CurrentApplication.ApplicationID, username, out nullable))
                    {
                        string[] roleNames = RoleNames;
                        int num1 = 0;
                        while (num1 < (int)roleNames.Length)
                        {
                            string str = roleNames[num1];
                            Role role = (
                                from t in this.Database.Query<Role>()
                                where t.Name.ToLower() == str.ToLower()
                                select t).FirstOrDefault<Role>();
                            if (role != null)
                            {
                                if ((
                                    from t in this.Database.Query<UserRole>()
                                    where (int?)t.User.UserID == nullable && t.Role.RoleID == role.RoleID
                                    select t).FirstOrDefault<UserRole>() == null)
                                {
                                    UserRole ur = new UserRole()
                                    {
                                        User = this.Database.Get<GlobalUsers.Entities.User>(nullable.Value),
                                        Role = this.Database.Get<Role>(role.RoleID)
                                    };
                                    this.Database.SaveOrUpdate(ur);
                                    // this._RecordAuditEvent(this.CurrentApplication.ApplicationID, username, GlobalUsersService.EventType.USER_ADDED_TO_ROLE, role.Name);
                                    this._WriteToAuditLog(this.CurrentApplication.ApplicationID, username, GlobalUsersService.EventType.USER_ADDED_TO_ROLE, role.Name);
                                }
                                num1++;
                            }
                            else
                            {
                                errorReturnObject = new ErrorReturnObject(ErrorReturnObject.Error.RoleNameNotFound);
                                return errorReturnObject;
                            }
                        }
                        num++;
                    }
                    else
                    {
                        errorReturnObject = new ErrorReturnObject(ErrorReturnObject.Error.UsernameNotFound);
                        break;
                    }
                }
                else
                {
                    errorReturnObject = new SuccessReturnObject();
                    break;
                }
            }
            return errorReturnObject;
        }

        public ReturnObject AddUserToRole(string Username, string RoleName)
        {
            string[] username = new string[] { Username };
            string[] strArrays = username;
            username = new string[] { RoleName };
            return this.AddUsersToRoles(strArrays, username);
        }

        public ReturnObject AssignPassword(string Username, string NewPassword)
        {
            return this._SetUserPassword(Username, NewPassword, null);
        }

        public ReturnObject ChangePassword(string Username, string CurrentPassword, string NewPassword)
        {
            ReturnObject errorReturnObject;
            int? nullable;
            int ApplicationID = this._GetApplicationID(this.CurrentApplication.ApplicationName);
            if (ApplicationID == -1)
            {
                errorReturnObject = new ErrorReturnObject(ErrorReturnObject.Error.ApplicationDoesNotExist);
            }
            else if (this._ExistsUsername(ApplicationID, Username, out nullable))
            {
                GlobalUsers.Entities.User user = (
                    from t in this.Database.Query<GlobalUsers.Entities.User>()
                    where (int?)t.UserID == nullable
                    select t).FirstOrDefault<GlobalUsers.Entities.User>();
                PasswordHistory pwhistory = (
                    from t in this.Database.Query<PasswordHistory>()
                    where t.UserID == user.UserID
                    orderby t.PasswordDate descending
                    select t).FirstOrDefault<PasswordHistory>();
                if (pwhistory != null)
                {
                    byte[] hashFromSubmission = Password.CreateSHA512Hash(CurrentPassword, pwhistory.PasswordSalt);
                    if (pwhistory.PasswordHash.SequenceEqual<byte>(hashFromSubmission))
                    {
                        ReturnObject ro = this.IsValidPassword(Username, NewPassword);
                        if (ro.ReturnCode == 0)
                        {
                            this._AddPassword(ApplicationID, user.UserID, NewPassword);
                            user.MustChangePassword = false;
                            user.IsPasswordExpired = false;
                            this.Database.SaveOrUpdate(user);
                            this._RecordAuditEvent(this.CurrentApplication.ApplicationID, Username, GlobalUsersService.EventType.PASSWORD_CHANGED);
                            errorReturnObject = new SuccessReturnObject();
                        }
                        else
                        {
                            errorReturnObject = new ErrorReturnObject(101, ro.DefaultErrorMessage);
                        }
                    }
                    else
                    {
                        errorReturnObject = new ErrorReturnObject(102, "Current password was incorrect.");
                    }
                }
                else
                {
                    errorReturnObject = new ErrorReturnObject(102, "Current password was incorrect.");
                }
            }
            else
            {
                errorReturnObject = new ErrorReturnObject(ErrorReturnObject.Error.UsernameNotFound);
            }
            return errorReturnObject;
        }

        public ReturnObject CreateApplication(ApplicationInformation ai)
        {
            ReturnObject errorReturnObject;
            if (!this.IsCurrentApplicationManager)
            {
                errorReturnObject = new ErrorReturnObject(101, "This function is only available to the management application");
            }
            else if (string.IsNullOrEmpty(ai.ApplicationName))
            {
                errorReturnObject = new ErrorReturnObject(102, "You must specify a name.");
            }
            else if (string.IsNullOrEmpty(ai.ApplicationPassword))
            {
                errorReturnObject = new ErrorReturnObject(103, "You must specify a password.");
            }
            else if (this.Database.Query<Application>().Count<Application>((Application a) => ai.ApplicationName.ToLower() == a.ApplicationName.ToLower()) <= 0)
            {
                Application aa = new Application();
                aa.CopyFrom(ai, new string[] { "ApplicationID" });
                errorReturnObject = this.CreateOrUpdateApplication(aa, ai);
            }
            else
            {
                errorReturnObject = new ErrorReturnObject(104, "Another application with that name already exists.");
            }
            return errorReturnObject;
        }

        private ReturnObject CreateOrUpdateApplication(Application aa, ApplicationInformation ai)
        {
            GlobalUsers.Entities.Account account;
            string[] strArrays;
            int i;
            Func<ApplicationQuestion, int> questionID = null;
            this.Database.SaveOrUpdate(aa);
            if (aa.ApplicationID != 0)
            {
                if (ai.Questions == null)
                {
                    ai.Questions = new ApplicationQuestion[0];
                }
                List<GlobalUsers.Entities.Question> Question = (
                    from q in this.Database.Query<GlobalUsers.Entities.Question>()
                    where q.Application.ApplicationID == aa.ApplicationID
                    select q).ToList<GlobalUsers.Entities.Question>();
                foreach (ApplicationQuestion applicationQuestion in
                    from q in (IEnumerable<ApplicationQuestion>)ai.Questions
                    where q.QuestionID > 0
                    select q)
                {
                    GlobalUsers.Entities.Question question = this.Database.Query<GlobalUsers.Entities.Question>().First<GlobalUsers.Entities.Question>((GlobalUsers.Entities.Question q) => q.QuestionID == applicationQuestion.QuestionID && q.Application.ApplicationID == aa.ApplicationID);
                    question.QuestionText = applicationQuestion.QuestionText;
                    this.Database.SaveOrUpdate(question);
                }
                foreach (GlobalUsers.Entities.Question question1 in Question.Where<GlobalUsers.Entities.Question>((GlobalUsers.Entities.Question q) =>
                {
                    ApplicationQuestion[] questions = ai.Questions;
                    if (questionID == null)
                    {
                        questionID = (ApplicationQuestion aq) => aq.QuestionID;
                    }
                    return !((IEnumerable<ApplicationQuestion>)questions).Select<ApplicationQuestion, int>(questionID).Contains<int>(q.QuestionID);
                }))
                {
                    this.Database.Delete(question1);
                }
                foreach (ApplicationTrust trust in
                    from q in this.Database.Query<ApplicationTrust>()
                    where q.Application.ApplicationID == aa.ApplicationID
                    select q)
                {
                    this.Database.Delete(trust);
                }
            }
            foreach (ApplicationQuestion applicationQuestion1 in
                from q in (IEnumerable<ApplicationQuestion>)ai.Questions
                where (q.QuestionID > 0 ? false : !string.IsNullOrWhiteSpace(q.QuestionText))
                select q)
            {
                GlobalUsers.Entities.Question new_q = new GlobalUsers.Entities.Question()
                {
                    Application = this.Database.Get<Application>(aa.ApplicationID),
                    QuestionText = applicationQuestion1.QuestionText
                };
                this.Database.SaveOrUpdate(new_q);
            }
            if (ai.TrustedApplications == null)
            {
                ai.TrustedApplications = new Dictionary<int, string>();
            }
            foreach (int trust in ai.TrustedApplications.Keys)
            {
                NHibernate.ISession database = this.Database;
                ApplicationTrust applicationTrust = new ApplicationTrust()
                {
                    Application = aa,
                    TrustedApplication = this.Database.Get<Application>(trust)
                };
                database.SaveOrUpdate(applicationTrust);
            }
            string[] new_Account = ai.Accounts ?? new string[0];
            if (!(aa.ApplicationName == "ApplicationManagement"))
            {
                List<ApplicationAccount> Account = (
                    from q in this.Database.Query<ApplicationAccount>()
                    where q.Application.ApplicationID == aa.ApplicationID
                    select q).ToList<ApplicationAccount>();
                foreach (ApplicationAccount acc in Account)
                {
                    this.Database.Delete(acc);
                }
                strArrays = new_Account;
                for (i = 0; i < (int)strArrays.Length; i++)
                {
                    string str = strArrays[i];
                    GlobalUsers.Entities.Account account1 = this.Database.Query<GlobalUsers.Entities.Account>().FirstOrDefault<GlobalUsers.Entities.Account>((GlobalUsers.Entities.Account a) => a.Username.ToLower() == str.ToLower());
                    if (account1 == null)
                    {
                        GlobalUsers.Entities.Account account2 = new GlobalUsers.Entities.Account()
                        {
                            Username = str,
                            Admin = false
                        };
                        account1 = account2;
                    }
                    account = account1;
                    this.Database.SaveOrUpdate(account);
                    ApplicationAccount applicationAccount = new ApplicationAccount()
                    {
                        Account = new GlobalUsers.Entities.Account()
                        {
                            AccountID = account.AccountID
                        },
                        Application = new Application()
                        {
                            ApplicationID = aa.ApplicationID
                        }
                    };
                    this.Database.SaveOrUpdate(applicationAccount);
                }
            }
            else
            {
                List<GlobalUsers.Entities.Account> current_Account = (
                    from a in this.Database.Query<GlobalUsers.Entities.Account>()
                    where a.Admin
                    select a).ToList<GlobalUsers.Entities.Account>();
                foreach (GlobalUsers.Entities.Account account3 in current_Account)
                {
                    if (!new_Account.Any<string>((string a) => account3.Username.Equals(a, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        account3.Admin = false;
                        this.Database.SaveOrUpdate(account3);
                    }
                }
                strArrays = new_Account;
                for (i = 0; i < (int)strArrays.Length; i++)
                {
                    string str1 = strArrays[i];
                    if (!current_Account.Any<GlobalUsers.Entities.Account>((GlobalUsers.Entities.Account a) => a.Username.Equals(str1, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        GlobalUsers.Entities.Account account4 = new GlobalUsers.Entities.Account()
                        {
                            Username = str1,
                            Admin = true
                        };
                        account = account4;
                        this.Database.SaveOrUpdate(account);
                    }
                }
            }
            return new SuccessReturnObject();
        }

        public ReturnObject CreateUserAndPassword(string Username, string password)
        {
            ReturnObject errorReturnObject;
            ReturnObject ro = this.AddUser(Username);
            if (!(ro is ErrorReturnObject))
            {
                if (string.IsNullOrEmpty(password))
                {
                    ro = this.GeneratePassword(Username);
                    password = ro.ReturnValue as string;
                    if (ro is ErrorReturnObject)
                    {
                        errorReturnObject = ro;
                        return errorReturnObject;
                    }
                }
                ro = this._SetUserPassword(Username, password, null);
                if ((!(ro is ErrorReturnObject) ? true : ro.ReturnCode != 101))
                {
                    errorReturnObject = ro;
                }
                else
                {
                    errorReturnObject = new ErrorReturnObject(103, "Password failed complexity check.");
                }
            }
            else
            {
                errorReturnObject = ro;
            }
            return errorReturnObject;
        }

        public ReturnObject CreateUserTicket(string username)
        {
            ReturnObject successReturnObject;
            GlobalUsers.Entities.User user = (
                from u in this.Database.Query<GlobalUsers.Entities.User>()
                join a in this.Database.Query<Application>() on u.Application.ApplicationID equals a.ApplicationID
                where (a.ApplicationName.ToLower() == this.CurrentApplication.ApplicationName.ToLower()) && (u.Username.ToLower() == username.ToLower())
                select u).FirstOrDefault<GlobalUsers.Entities.User>();
            if (user != null)
            {
                foreach (UserTicket ut in
                    from ut in this.Database.Query<UserTicket>()
                    where ut.User.UserID == user.UserID
                    select ut)
                {
                    this.Database.Delete(ut);
                }
                string ticket_value = Guid.NewGuid().ToString();
                DateTime ticket_created = DateTime.Now;
                UserTicket new_ticket = new UserTicket();
                new_ticket.User.UserID = user.UserID;
                new_ticket.TicketValue = Password.CreateSHA512Hash(ticket_value, Encoding.ASCII.GetBytes(ticket_created.ToString()));
                new_ticket.TimeCreated = ticket_created;
                this.Database.SaveOrUpdate(new_ticket);
                successReturnObject = new SuccessReturnObject(ticket_value);
            }
            else
            {
                successReturnObject = new ErrorReturnObject(101, "The user does not exist.");
            }
            return successReturnObject;
        }

        public ReturnObject DeleteApplication(string id)
        {
            ReturnObject errorReturnObject;
            if (this.IsCurrentApplicationManager)
            {
                Application aa = this.Database.Query<Application>().FirstOrDefault<Application>((Application a) => a.ApplicationName.ToLower() == id.ToLower());
                if (aa != null)
                {
                    try
                    {
                        this.Database.Delete(aa);
                    }
                    catch (Exception exception)
                    {
                        errorReturnObject = new ErrorReturnObject(103, "The application failed to delete." + exception.ToString());
                        return errorReturnObject;
                    }
                    errorReturnObject = new SuccessReturnObject();
                }
                else
                {
                    errorReturnObject = new ErrorReturnObject(102, "No application with the specified ID could be found.");
                }
            }
            else
            {
                errorReturnObject = new ErrorReturnObject(101, "This function is only available to the management application.");
            }
            return errorReturnObject;
        }

        public ReturnObject DeleteRole(string RoleName, bool HaltIfUser)
        {
            ReturnObject errorReturnObject;
            Role role = (
                from t in this.Database.Query<Role>()
                where t.Name.ToLower() == RoleName.ToLower()
                select t).FirstOrDefault<Role>();
            if (role == null)
            {
                errorReturnObject = new ErrorReturnObject(ErrorReturnObject.Error.RoleNameNotFound);
            }
            else if (((
                from t in this.Database.Query<UserRole>()
                where t.Role.RoleID == role.RoleID
                select t).Count<UserRole>() <= 0 ? true : !HaltIfUser))
            {
                foreach (UserRole ur in
                    from ur in this.Database.Query<UserRole>()
                    where ur.Role.RoleID == role.RoleID
                    select ur)
                {
                    this.Database.Delete(ur);
                    this.Database.Flush();
                }
                this.Database.Delete(role);
                errorReturnObject = new SuccessReturnObject();
            }
            else
            {
                errorReturnObject = new ErrorReturnObject(101, "User exist in role.");
            }
            return errorReturnObject;
        }

        public ReturnObject DeleteUser(string Username)
        {
            ReturnObject successReturnObject;
            int? nullable;
            if (this._ExistsUsername(this.CurrentApplication.ApplicationID, Username, out nullable))
            {
                (
                    from ut in this.Database.Query<GlobalUsers.Entities.User>()
                    where (int?)ut.UserID == nullable
                    select ut).ForEach<GlobalUsers.Entities.User>((GlobalUsers.Entities.User ut) => this.Database.Delete(ut));
                this.Database.Flush();
                successReturnObject = new SuccessReturnObject();
            }
            else
            {
                successReturnObject = new ErrorReturnObject(ErrorReturnObject.Error.UsernameNotFound);
            }
            return successReturnObject;
        }

        public ReturnObject EnableUser(string Username)
        {
            ReturnObject successReturnObject;
            int? nullable;
            if (this._ExistsUsername(this.CurrentApplication.ApplicationID, Username, out nullable))
            {
                GlobalUsers.Entities.User db_user = this.Database.Query<GlobalUsers.Entities.User>().First<GlobalUsers.Entities.User>((GlobalUsers.Entities.User u) => (int?)u.UserID == nullable);
                db_user.IsEnabled = true;
                this.Database.SaveOrUpdate(db_user);
                successReturnObject = new SuccessReturnObject();
            }
            else
            {
                successReturnObject = new ErrorReturnObject(ErrorReturnObject.Error.UsernameNotFound);
            }
            return successReturnObject;
        }

        public ReturnObject ExistsRole(string RoleName)
        {
            ReturnObject errorReturnObject;
            if (this._GetApplicationID(this.CurrentApplication.ApplicationName) != -1)
            {
                errorReturnObject = ((
                    from t in this.Database.Query<Role>()
                    where t.Name.ToLower() == RoleName.ToLower()
                    select t).FirstOrDefault<Role>() != null ? new SuccessReturnObject(true) : new SuccessReturnObject(false));
            }
            else
            {
                errorReturnObject = new ErrorReturnObject(ErrorReturnObject.Error.ApplicationDoesNotExist);
            }
            return errorReturnObject;
        }

        public ReturnObject ExistsUsername(string Username)
        {
            ReturnObject successReturnObject = new SuccessReturnObject((object)this._ExistsUsername(this.CurrentApplication.ApplicationID, Username));
            return successReturnObject;
        }

        public ReturnObject FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            totalRecords = 0;
            IQueryable<GlobalUsers.Entities.User> User =
                from u in this.Database.Query<GlobalUsers.Entities.User>()
                from md in this.Database.Query<UserMetaDatum>()
                where u.Application.ApplicationID == this.CurrentApplication.ApplicationID && md.User.UserID == u.UserID && (md.DataKey == "Email") && md.DataValue.Contains(emailToMatch)
                select u;
            totalRecords = User.Count<GlobalUsers.Entities.User>();
            ReturnObject successReturnObject = new SuccessReturnObject((
                from ujd in User.ToList<GlobalUsers.Entities.User>()
                select ujd.ToWebServiceUser()).Skip<WebService.User>(pageIndex * pageSize).Take<WebService.User>(pageSize).ToArray<WebService.User>());
            return successReturnObject;
        }

        public ReturnObject FindUsersByMetaData(string MetaDataKey, string MetaDataValue)
        {
            IQueryable<GlobalUsers.Entities.User> User =
                from u in this.Database.Query<GlobalUsers.Entities.User>()
                from md in this.Database.Query<UserMetaDatum>()
                where u.Application.ApplicationID == this.CurrentApplication.ApplicationID && md.User.UserID == u.UserID && (md.DataKey == MetaDataKey) && md.DataValue.ToLower().Contains(MetaDataValue.ToLower())
                select u;
            ReturnObject successReturnObject = new SuccessReturnObject((
                from ujd in User.ToList<GlobalUsers.Entities.User>()
                select ujd.ToWebServiceUser()).ToArray<WebService.User>());
            return successReturnObject;
        }

        public ReturnObject FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            totalRecords = 0;
            List<GlobalUsers.Entities.User> User = (
                from t in this.Database.Query<GlobalUsers.Entities.User>()
                where t.Application.ApplicationID == this.CurrentApplication.ApplicationID && t.Username.ToLower().Contains(usernameToMatch.ToLower())
                select t).ToList<GlobalUsers.Entities.User>();
            totalRecords = User.Count<GlobalUsers.Entities.User>();
            ReturnObject successReturnObject = new SuccessReturnObject((
                from u in User
                select u.ToWebServiceUser()).Skip<WebService.User>(pageIndex * pageSize).Take<WebService.User>(pageSize).ToArray<WebService.User>());
            return successReturnObject;
        }

        public ReturnObject FindUsersInRole(string RoleName, string UsernameToMatch)
        {
            ReturnObject successReturnObject;
            Role role = (
                from t in this.Database.Query<Role>()
                where t.Name.ToLower() == RoleName.ToLower()
                select t).FirstOrDefault<Role>();
            if (role != null)
            {
                List<string> lstUsernames = (
                    from u in this.Database.Query<GlobalUsers.Entities.User>()
                    join o in this.Database.Query<UserRole>() on u.UserID equals o.User.UserID
                    where o.Role.RoleID == role.RoleID && u.Username.ToLower().Contains(UsernameToMatch.ToLower())
                    select u.Username).ToList<string>();
                successReturnObject = new SuccessReturnObject(lstUsernames.ToArray());
            }
            else
            {
                successReturnObject = new ErrorReturnObject(ErrorReturnObject.Error.RoleNameNotFound);
            }
            return successReturnObject;
        }

        public ReturnObject GeneratePassword(string Username)
        {
            ReturnObject errorReturnObject;
            Application aa = (
                from t in this.Database.Query<Application>()
                where t.ApplicationID == this.CurrentApplication.ApplicationID
                select t).First<Application>();
            char[] cValid = string.Concat("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", (!string.IsNullOrEmpty(aa.PasswordAllowedSpecialCharacters) ? aa.PasswordAllowedSpecialCharacters : "!@#$%#$")).ToCharArray();
            int attempts = 0;
            while (true)
            {
                int nPasswordLength = aa.PasswordMinLength;
                StringBuilder sbPassword = new StringBuilder(nPasswordLength);
                for (int nCharPos = 0; nCharPos < nPasswordLength; nCharPos++)
                {
                    sbPassword.Append(cValid[Password.RandomInt(0, (int)cValid.Length - 1)]);
                }
                if (this.IsValidPassword(Username, sbPassword.ToString()).ReturnCode != 0)
                {
                    attempts++;
                    if (attempts > 100)
                    {
                        errorReturnObject = new ErrorReturnObject(101, "Unable to generate password. Application may have password parameters set such that no valid password is possible.");
                        break;
                    }
                }
                else
                {
                    errorReturnObject = new SuccessReturnObject(sbPassword.ToString());
                    break;
                }
            }
            return errorReturnObject;
        }

        public ReturnObject GetAllRoles()
        {
            string[] roleNames = (
                from t in this.Database.Query<Role>()
                where t.Application.ApplicationID == this.CurrentApplication.ApplicationID
                select t.Name).ToArray<string>();
            return new SuccessReturnObject(roleNames);
        }

        public ReturnObject GetAllUsers(out int totalRecords, int pageIndex, int pageSize)
        {
            totalRecords = this.Database.Query<GlobalUsers.Entities.User>().Count<GlobalUsers.Entities.User>((GlobalUsers.Entities.User u) => u.Application.ApplicationID == this.CurrentApplication.ApplicationID);
            GlobalUsers.Entities.User[] User = (
                from u in this.Database.Query<GlobalUsers.Entities.User>()
                where u.Application.ApplicationID == this.CurrentApplication.ApplicationID
                orderby u.Username
                select u).Skip<GlobalUsers.Entities.User>(pageIndex * pageSize).Take<GlobalUsers.Entities.User>(pageSize).ToArray<GlobalUsers.Entities.User>();
            ReturnObject successReturnObject = new SuccessReturnObject((
                from u in (IEnumerable<GlobalUsers.Entities.User>)User
                select u.ToWebServiceUser()).ToArray<WebService.User>());
            return successReturnObject;
        }

        public ApplicationInformation GetApplicationInformation()
        {
            ApplicationInformation applicationInformation = this.Database.Query<Application>().First<Application>((Application a) => a.ApplicationName.ToLower() == this.CurrentApplication.ApplicationName.ToLower()).ToApplicationInformation();
            return applicationInformation;
        }

        public List<ApplicationInformation> GetApplicationInformationList()
        {
            if (!this.IsCurrentApplicationManager)
            {
                throw new UnauthorizedAccessException("Only the management application can access this function");
            }
            List<ApplicationInformation> list = (
                from a in this.Database.Query<Application>().ToList<Application>()
                select a.ToApplicationInformation()).ToList<ApplicationInformation>();
            return list;
        }

        public ReturnObject GetApplicationLogs(out int total_logs, DateTime start, DateTime end, int? page, int? size)
        {
            if (!this.IsCurrentApplicationManager)
            {
                throw new UnauthorizedAccessException("Only the management application can access this function");
            }
            IQueryable<AuditLog> list =
                from audit in this.Database.Query<AuditLog>()
                from app in this.Database.Query<Application>()
                from type in this.Database.Query<GlobalUsers.Entities.EventType>()
                where app.ApplicationID == audit.Application.ApplicationID && (app.ApplicationName.ToLower() == this.CurrentApplication.ApplicationName.ToLower()) && type.EventTypeID == audit.EventType.EventTypeID && (audit.EventDate > start) && (audit.EventDate < end)
                orderby audit.EventDate descending
                select audit;
            total_logs = list.Count<AuditLog>();
            if ((!page.HasValue ? false : size.HasValue))
            {
                list = list.Skip<AuditLog>(page.Value * size.Value).Take<AuditLog>(size.Value);
            }
            ReturnObject successReturnObject = new SuccessReturnObject((
                from audit in list.ToList<AuditLog>()
                select new EventAudit()
                {
                    Username = audit.Username,
                    EventType = audit.EventType.Name,
                    Timestamp = audit.EventDate,
                    IP = this._IntToIP(audit.UserIP),
                    Comment = audit.Comment
                }).ToArray<EventAudit>());
            return successReturnObject;
        }

        public ReturnObject GetApplicationLogsForUser(out int total_logs, string username, int? page, int? size)
        {
            if (!this.IsCurrentApplicationManager)
            {
                throw new UnauthorizedAccessException("Only the management application can access this function");
            }
            IQueryable<AuditLog> list =
                from audit in this.Database.Query<AuditLog>()
                from app in this.Database.Query<Application>()
                from type in this.Database.Query<GlobalUsers.Entities.EventType>()
                where app.ApplicationID == audit.Application.ApplicationID && (app.ApplicationName.ToLower() == this.CurrentApplication.ApplicationName.ToLower()) && type.EventTypeID == audit.EventType.EventTypeID && (audit.Username == username)
                orderby audit.EventDate descending
                select audit;
            total_logs = list.Count<AuditLog>();
            if ((!page.HasValue ? false : size.HasValue))
            {
                list = list.Skip<AuditLog>(page.Value * size.Value).Take<AuditLog>(size.Value);
            }
            ReturnObject successReturnObject = new SuccessReturnObject((
                from audit in list.ToList<AuditLog>()
                select new EventAudit()
                {
                    Username = audit.Username,
                    EventType = audit.EventType.Name,
                    Timestamp = audit.EventDate,
                    IP = this._IntToIP(audit.UserIP),
                    Comment = audit.Comment
                }).ToArray<EventAudit>());
            return successReturnObject;
        }

        public ReturnObject GetAvailableQuestions()
        {
            string[] Question = (
                from t in this.Database.Query<GlobalUsers.Entities.Question>()
                where t.Application.ApplicationID == this.CurrentApplication.ApplicationID
                select t.QuestionText).ToArray<string>();
            return new SuccessReturnObject(Question);
        }

        public ReturnObject GetInactivityDaysLeft(string Username)
        {
            int? UserID;
            DateTime dtLater;
            ReturnObject successReturnObject;
            if (this._ExistsUsername(this.CurrentApplication.ApplicationID, Username, out UserID))
            {
                ApplicationInformation ai = this.GetApplicationInformation();
                if (ai.InactivityDays != 0)
                {
                    DateTime? dtLastLogin = new DateTime?((
                        from t in this.Database.Query<AuditLog>()
                        where (t.Username.ToLower() == Username.ToLower()) && t.EventType.EventTypeID == 0
                        orderby t.EventDate descending
                        select t.EventDate).FirstOrDefault<DateTime>());
                    DateTime? dtCreation = new DateTime?((
                        from t in this.Database.Query<AuditLog>()
                        where (t.Username.ToLower() == Username.ToLower()) && t.EventType.EventTypeID == 11
                        orderby t.EventDate descending
                        select t.EventDate).FirstOrDefault<DateTime>());
                    if ((dtLastLogin.HasValue ? true : dtCreation.HasValue))
                    {
                        if (!dtLastLogin.HasValue)
                        {
                            dtLater = dtCreation.Value;
                        }
                        else if (dtLastLogin.HasValue)
                        {
                            DateTime? nullable = dtLastLogin;
                            DateTime? nullable1 = dtCreation;
                            //dtLater = ((nullable.HasValue & nullable1.HasValue ? (int)(nullable.GetValueOrDefault() > nullable1.GetValueOrDefault()) : 0) == 0 ? dtCreation.Value : dtLastLogin.Value);
                            if (nullable.HasValue && nullable1.HasValue)
                            {
                                if (nullable > nullable1)
                                {
                                    dtLater = dtCreation.Value;
                                }
                                else
                                {
                                    dtLater = dtLastLogin.Value;
                                }
                            }
                            else
                            {
                                dtLater = dtCreation.Value;
                            }
                        }
                        else
                        {
                            dtLater = dtLastLogin.Value;
                        }
                        int nAge = this._MidnightsBetweenDates(dtLater, DateTime.Now);
                        int nDaysLeft = ai.InactivityDays - nAge;
                        successReturnObject = (nDaysLeft >= 0 ? new SuccessReturnObject((object)nDaysLeft) : new SuccessReturnObject((object)0));
                    }
                    else
                    {
                        successReturnObject = new SuccessReturnObject((object)-1);
                    }
                }
                else
                {
                    successReturnObject = new ErrorReturnObject(101, "Application does not use inactivity feature.");
                }
            }
            else
            {
                successReturnObject = new ErrorReturnObject(ErrorReturnObject.Error.UsernameNotFound);
            }
            return successReturnObject;
        }

        public ReturnObject GetMustChangePasswordFlag(string Username)
        {
            ReturnObject successReturnObject;
            int? nullable;
            if (this._ExistsUsername(this.CurrentApplication.ApplicationID, Username, out nullable))
            {
                GlobalUsers.Entities.User uu = (
                    from t in this.Database.Query<GlobalUsers.Entities.User>()
                    where (int?)t.UserID == nullable
                    select t).FirstOrDefault<GlobalUsers.Entities.User>();
                successReturnObject = new SuccessReturnObject((object)uu.MustChangePassword);
            }
            else
            {
                successReturnObject = new ErrorReturnObject(ErrorReturnObject.Error.UsernameNotFound);
            }
            return successReturnObject;
        }

        public ReturnObject GetPasswordExpirationDaysLeft(string Username)
        {
            int? UserID;
            ReturnObject successReturnObject;
            if (this._ExistsUsername(this.CurrentApplication.ApplicationID, Username, out UserID))
            {
                successReturnObject = new SuccessReturnObject((object)this._GetPasswordExpirationDaysLeft(this.CurrentApplication.ApplicationID, UserID.Value));
            }
            else
            {
                successReturnObject = new ErrorReturnObject(ErrorReturnObject.Error.UsernameNotFound);
            }
            return successReturnObject;
        }

        public ReturnObject GetPasswordPolicyStatement()
        {
            ReturnObject successReturnObject;
            Application aa = (
                from t in this.Database.Query<Application>()
                where t.ApplicationID == this.CurrentApplication.ApplicationID
                select t).First<Application>();
            StringBuilder sb = new StringBuilder();
            if (!aa.UseCustomPasswordValidation)
            {
                sb.Append(string.Format("Valid passwords are between {0} and {1} characters in length and contain at least 3 out of 4 of the following elements: uppercase letters A-Z, lowercase letters a-z, numbers 0-9, and special characters{2}.", aa.PasswordMinLength, aa.PasswordMaxLength, (string.IsNullOrEmpty(aa.PasswordAllowedSpecialCharacters) ? "" : string.Concat(" ", aa.PasswordAllowedSpecialCharacters))));
                if (!aa.PasswordAllowUsername)
                {
                    sb.Append(" Your username may not appear anywhere in your password.");
                }
                if (aa.CheckPasswordAgainstWordlist)
                {
                    sb.Append(" Passwords may not contain words or phrases found in a dictionary.");
                }
                if (aa.PasswordHistoryMinimumAgeDays > 0)
                {
                    sb.Append(string.Format(" A password cannot be reused within {0} days since it was previously assigned.", aa.PasswordHistoryMinimumAgeDays));
                }
                if (aa.PasswordHistoryMinimumIterations > 0)
                {
                    sb.Append(string.Format(" A password cannot be reused until {0} other new passwords have been assigned.", aa.PasswordHistoryMinimumIterations));
                }
                successReturnObject = new SuccessReturnObject(sb.ToString());
            }
            else
            {
                successReturnObject = new ErrorReturnObject(101, "This application uses custom password validation.");
            }
            return successReturnObject;
        }

        public ReturnObject GetRolesForUser(string Username)
        {
            ReturnObject successReturnObject;
            int? nullable;
            if (this._ExistsUsername(this.CurrentApplication.ApplicationID, Username, out nullable))
            {
                string[] roleNames = (
                    from r in this.Database.Query<Role>()
                    join o in this.Database.Query<UserRole>() on r.RoleID equals o.Role.RoleID
                    where (int?)o.User.UserID == nullable
                    select r.Name).ToArray<string>();
                successReturnObject = new SuccessReturnObject(roleNames);
            }
            else
            {
                successReturnObject = new ErrorReturnObject(ErrorReturnObject.Error.UsernameNotFound);
            }
            return successReturnObject;
        }

        public ReturnObject GetUser(string Username)
        {
            ReturnObject successReturnObject;
            int? nullable;
            if (this._ExistsUsername(this.CurrentApplication.ApplicationID, Username, out nullable))
            {
                GlobalUsers.Entities.User uu = (
                    from t in this.Database.Query<GlobalUsers.Entities.User>()
                    where (int?)t.UserID == nullable
                    select t).First<GlobalUsers.Entities.User>();
                successReturnObject = new SuccessReturnObject(uu.ToWebServiceUser());
            }
            else
            {
                successReturnObject = new ErrorReturnObject(ErrorReturnObject.Error.UsernameNotFound);
            }
            return successReturnObject;
        }

        public ReturnObject GetUserMetaData(string Username, string MetaDataKey)
        {
            ReturnObject errorReturnObject;
            int? nullable;
            if (this._ExistsUsername(this.CurrentApplication.ApplicationID, Username, out nullable))
            {
                UserMetaDatum md = (
                    from t in this.Database.Query<UserMetaDatum>()
                    where (int?)t.User.UserID == nullable && (t.DataKey.ToLower() == MetaDataKey.ToLower())
                    select t).FirstOrDefault<UserMetaDatum>();
                errorReturnObject = (md != null ? new SuccessReturnObject(md.DataValue) : new SuccessReturnObject(string.Empty));
            }
            else
            {
                errorReturnObject = new ErrorReturnObject(ErrorReturnObject.Error.UsernameNotFound);
            }
            return errorReturnObject;
        }

        public ReturnObject GetUserMetaDataAll(string Username)
        {
            ReturnObject successReturnObject;
            int? nullable;
            if (this._ExistsUsername(this.CurrentApplication.ApplicationID, Username, out nullable))
            {
                IQueryable<UserMetaDatum> allMetaData =
                    from t in this.Database.Query<UserMetaDatum>()
                    where t.User.UserID == (int)nullable
                    select t;
                Dictionary<string, string> dMetaData = new Dictionary<string, string>(allMetaData.Count<UserMetaDatum>());
                foreach (UserMetaDatum md in allMetaData)
                {
                    dMetaData.Add(md.DataKey, md.DataValue);
                }
                successReturnObject = new SuccessReturnObject(dMetaData);
            }
            else
            {
                successReturnObject = new ErrorReturnObject(ErrorReturnObject.Error.UsernameNotFound);
            }
            return successReturnObject;
        }

        public ReturnObject GetUserQuestions(string Username)
        {
            ReturnObject successReturnObject;
            int? nullable;
            if (this._ExistsUsername(this.CurrentApplication.ApplicationID, Username, out nullable))
            {
                UserQuestion[] Question = (
                    from qa in
                        (
                            from qa in this.Database.Query<QuestionAnswer>()
                            where (int?)qa.User.UserID == nullable
                            select qa).ToList<QuestionAnswer>()
                    select qa.Copy<UserQuestion>()).ToArray<UserQuestion>();
                successReturnObject = new SuccessReturnObject(Question);
            }
            else
            {
                successReturnObject = new ErrorReturnObject(ErrorReturnObject.Error.UsernameNotFound);
            }
            return successReturnObject;
        }

        public ReturnObject GetUsersByMetaData(string key, List<string> values)
        {
            List<KeyValuePair<string, string>> metadata = new List<KeyValuePair<string, string>>();
            foreach (string val in values)
            {
                metadata.Add(new KeyValuePair<string, string>(key, val));
            }
            List<GlobalUsers.Entities.User> results = this._SearchUserByMetaData(this.CurrentApplication.ApplicationID, metadata, true);
            return new SuccessReturnObject((results == null ? new User[0] : (
                from ujd in results.ToList<GlobalUsers.Entities.User>()
                select ujd.ToWebServiceUser()).ToArray<User>()));
        }

        public ReturnObject GetUsersInRole(string RoleName)
        {
            ReturnObject successReturnObject;
            Role role = (
                from t in this.Database.Query<Role>()
                where t.Name.ToLower() == RoleName.ToLower()
                select t).FirstOrDefault<Role>();
            if (role != null)
            {
                GlobalUsers.Entities.User[] User = (
                    from u in this.Database.Query<GlobalUsers.Entities.User>()
                    join o in this.Database.Query<UserRole>() on u.UserID equals o.User.UserID
                    where o.Role.RoleID == role.RoleID
                    select u).ToArray<GlobalUsers.Entities.User>();
                successReturnObject = new SuccessReturnObject((
                    from u in (IEnumerable<GlobalUsers.Entities.User>)User
                    select u.ToWebServiceUser()).ToArray<WebService.User>());
            }
            else
            {
                successReturnObject = new ErrorReturnObject(ErrorReturnObject.Error.RoleNameNotFound);
            }
            return successReturnObject;
        }

        public ReturnObject GetValidationFailureReason(string Username)
        {
            ReturnObject errorReturnObject;
            AuditLog al = (
                from t in this.Database.Query<AuditLog>()
                where t.Application.ApplicationID == this.CurrentApplication.ApplicationID && (t.Username == Username)
                orderby t.EventDate descending
                select t).FirstOrDefault<AuditLog>();
            if (al == null)
            {
                errorReturnObject = new ErrorReturnObject(101, "Could not authenticate.");
            }
            else if (al.EventType.EventTypeID == 1)
            {
                errorReturnObject = new ErrorReturnObject(102, "Username or password was incorrect.");
            }
            else if (al.EventType.EventTypeID == 2)
            {
                errorReturnObject = new ErrorReturnObject(103, "Username or password was incorrect.");
            }
            else if (al.EventType.EventTypeID == 3)
            {
                errorReturnObject = new ErrorReturnObject(104, "This account is locked out.");
            }
            else if (al.EventType.EventTypeID == 4)
            {
                errorReturnObject = new ErrorReturnObject(105, "The system is currently not accepting requests from this IP.");
            }
            else if (al.EventType.EventTypeID != 5)
            {
                errorReturnObject = (al.EventType.EventTypeID != 12 ? new ErrorReturnObject(101, "Could not authenticate.") : new ErrorReturnObject(107, "This account is inactive."));
            }
            else
            {
                errorReturnObject = new ErrorReturnObject(106, "This account is disabled.");
            }
            return errorReturnObject;
        }

        public ReturnObject GetVersionInformation()
        {
            VersionInformation vi = new VersionInformation()
            {
                ActualWebServicesVersion = string.Concat(Assembly.GetExecutingAssembly().GetName().Version.Major, ".", Assembly.GetExecutingAssembly().GetName().Version.MajorRevision)
            };
            return new SuccessReturnObject(vi);
        }

        public ReturnObject IsIPLockedOut()
        {
            return new SuccessReturnObject((object)this._IsIPLockedOut());
        }

        public ReturnObject IsPasswordExpired(string Username)
        {
            int? UserID;
            ReturnObject successReturnObject;
            if (this._ExistsUsername(this.CurrentApplication.ApplicationID, Username, out UserID))
            {
                successReturnObject = new SuccessReturnObject((object)this._IsPasswordExpired(this.CurrentApplication.ApplicationID, Username));
            }
            else
            {
                successReturnObject = new ErrorReturnObject(ErrorReturnObject.Error.UsernameNotFound);
            }
            return successReturnObject;
        }

        public ReturnObject IsUserInRole(string Username, string RoleName)
        {
            ReturnObject errorReturnObject;
            int? nullable;
            int ApplicationID = this._GetApplicationID(this.CurrentApplication.ApplicationName);
            if (ApplicationID == -1)
            {
                errorReturnObject = new ErrorReturnObject(ErrorReturnObject.Error.ApplicationDoesNotExist);
            }
            else if (this._ExistsUsername(ApplicationID, Username, out nullable))
            {
                Role role = (
                    from t in this.Database.Query<Role>()
                    where t.Name.ToLower() == RoleName.ToLower()
                    select t).FirstOrDefault<Role>();
                if (role != null)
                {
                    UserRole ur = (
                        from t in this.Database.Query<UserRole>()
                        where (int?)t.User.UserID == nullable && t.Role.RoleID == role.RoleID
                        select t).FirstOrDefault<UserRole>();
                    errorReturnObject = new SuccessReturnObject((object)(ur != null));
                }
                else
                {
                    errorReturnObject = new ErrorReturnObject(ErrorReturnObject.Error.RoleNameNotFound);
                }
            }
            else
            {
                errorReturnObject = new ErrorReturnObject(ErrorReturnObject.Error.UsernameNotFound);
            }
            return errorReturnObject;
        }

        public ReturnObject IsValidPassword(string Username, string Password)
        {
            // PasswordHistory ph = null;
            ReturnObject errorReturnObject;
            string password;
            int num;
            Application application = (
                from t in this.Database.Query<Application>()
                where t.ApplicationID == this.CurrentApplication.ApplicationID
                select t).First<Application>();
            if (!application.UseCustomPasswordValidation)
            {
                if (application.PasswordHistoryMinimumAgeDays > 0)
                {
                    GlobalUsers.Entities.User user = (
                        from t in this.Database.Query<GlobalUsers.Entities.User>()
                        where t.Application.ApplicationID == this.CurrentApplication.ApplicationID && (t.Username.ToLower() == Username.ToLower())
                        select t).FirstOrDefault<GlobalUsers.Entities.User>();
                    if (user != null)
                    {
                        foreach (PasswordHistory pwh in
                            from pwh in this.Database.Query<PasswordHistory>()
                            where pwh.UserID == user.UserID && (pwh.PasswordDate > DateTime.Now.AddDays((double)(-1 * application.PasswordHistoryMinimumAgeDays)))
                            orderby pwh.PasswordDate
                            select pwh)
                        {
                            if (WebService.Password.CreateSHA512Hash(Password, pwh.PasswordSalt).SequenceEqual<byte>(pwh.PasswordHash))
                            {
                                errorReturnObject = new ErrorReturnObject(107, "Password is not old enough to be reused.");
                                return errorReturnObject;
                            }
                        }
                    }
                }
                if (application.PasswordHistoryMinimumIterations > 0)
                {
                    GlobalUsers.Entities.User user1 = (
                        from t in this.Database.Query<GlobalUsers.Entities.User>()
                        where t.Application.ApplicationID == this.CurrentApplication.ApplicationID && (t.Username.ToLower() == Username.ToLower())
                        select t).FirstOrDefault<GlobalUsers.Entities.User>();
                    if (user1 != null)
                    {
                        IOrderedQueryable<PasswordHistory> pwhistory =
                            from t in this.Database.Query<PasswordHistory>()
                            where t.UserID == user1.UserID
                            orderby t.PasswordDate descending
                            select t;
                        int nPasswordsAgo = 0;
                        foreach (PasswordHistory passwordHistory in pwhistory)
                        {
                            if (!WebService.Password.CreateSHA512Hash(Password, passwordHistory.PasswordSalt).SequenceEqual<byte>(passwordHistory.PasswordHash))
                            {
                                nPasswordsAgo++;
                                if (nPasswordsAgo >= application.PasswordHistoryMinimumIterations)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                errorReturnObject = new ErrorReturnObject(101, "Password exists too recently in password history.");
                                return errorReturnObject;
                            }
                        }
                    }
                }
                if ((application.PasswordAllowUsername ? false : !string.IsNullOrEmpty(Username)))
                {
                    if (Password.ToLower().Contains(Username.ToLower()))
                    {
                        errorReturnObject = new ErrorReturnObject(102, "Username cannot be part of password.");
                        return errorReturnObject;
                    }
                }
                if (Password.Length < application.PasswordMinLength)
                {
                    errorReturnObject = new ErrorReturnObject(103, "Password is too short.");
                }
                else if (Password.Length <= application.PasswordMaxLength)
                {
                    if (!string.IsNullOrEmpty(application.PasswordAllowedSpecialCharacters))
                    {
                        string strValid = string.Concat("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", application.PasswordAllowedSpecialCharacters);
                        password = Password;
                        num = 0;
                        while (num < password.Length)
                        {
                            if (strValid.Contains<char>(password[num]))
                            {
                                num++;
                            }
                            else
                            {
                                errorReturnObject = new ErrorReturnObject(106, "Password contains invalid characters.");
                                return errorReturnObject;
                            }
                        }
                    }
                    int nCharClasses = 0;
                    if ((new Regex("[A-Z]")).IsMatch(Password))
                    {
                        nCharClasses++;
                    }
                    if ((new Regex("[a-z]")).IsMatch(Password))
                    {
                        nCharClasses++;
                    }
                    if ((new Regex("[0-9]")).IsMatch(Password))
                    {
                        nCharClasses++;
                    }
                    password = application.PasswordAllowedSpecialCharacters ?? string.Empty;
                    num = 0;
                    while (num < password.Length)
                    {
                        if (!Password.Contains<char>(password[num]))
                        {
                            num++;
                        }
                        else
                        {
                            nCharClasses++;
                            break;
                        }
                    }
                    if (nCharClasses >= 3)
                    {
                        errorReturnObject = new SuccessReturnObject();
                    }
                    else
                    {
                        errorReturnObject = new ErrorReturnObject(105, "Password does not contain at least three of the four character classes.");
                    }
                }
                else
                {
                    errorReturnObject = new ErrorReturnObject(104, "Password is too long.");
                }
            }
            else
            {
                errorReturnObject = new SuccessReturnObject();
            }
            return errorReturnObject;
        }

        public ReturnObject IsValidUsername(string Username)
        {
            return new SuccessReturnObject((object)this._IsValidUsername(Username));
        }

        public ReturnObject RemoveUserFromRole(string Username, string RoleName)
        {
            string[] username = new string[] { Username };
            string[] strArrays = username;
            username = new string[] { RoleName };
            return this.RemoveUsersFromRoles(strArrays, username);
        }

        public ReturnObject RemoveUsersFromRoles(string[] Usernames, string[] RoleNames)
        {
            int? UserID;
            ReturnObject errorReturnObject;
            string[] usernames = Usernames;
            int num = 0;
            while (true)
            {
                if (num < (int)usernames.Length)
                {
                    string str = usernames[num];
                    if (this._ExistsUsername(this.CurrentApplication.ApplicationID, str, out UserID))
                    {
                        string[] roleNames = RoleNames;
                        for (int i = 0; i < (int)roleNames.Length; i++)
                        {
                            string str1 = roleNames[i];
                            UserRole user_role = (
                                from ur in this.Database.Query<UserRole>()
                                from u in this.Database.Query<GlobalUsers.Entities.User>()
                                from r in this.Database.Query<Role>()
                                where r.RoleID == ur.Role.RoleID && u.UserID == ur.User.UserID && (u.Username.ToLower() == str.ToLower()) && (r.Name.ToLower() == str1.ToLower())
                                select ur).FirstOrDefault<UserRole>();
                            if (user_role != null)
                            {
                                this.Database.Delete(user_role);
                                //this._RecordAuditEvent(this.CurrentApplication.ApplicationID, str, GlobalUsersService.EventType.USER_REMOVED_FROM_ROLE, str1);
                                this._WriteToAuditLog(this.CurrentApplication.ApplicationID, str, GlobalUsersService.EventType.USER_REMOVED_FROM_ROLE, str1);
                            }
                        }
                        num++;
                    }
                    else
                    {
                        errorReturnObject = new ErrorReturnObject(ErrorReturnObject.Error.UsernameNotFound);
                        break;
                    }
                }
                else
                {
                    errorReturnObject = new SuccessReturnObject();
                    break;
                }
            }
            return errorReturnObject;
        }

        public ReturnObject ResetPassword(string Username, UserQuestion[] answers)
        {
            int? UserID;
            ReturnObject errorReturnObject;
            DateTime? nullable;
            if (!this._ExistsUsername(this.CurrentApplication.ApplicationID, Username, out UserID))
            {
                errorReturnObject = new ErrorReturnObject(ErrorReturnObject.Error.UsernameNotFound);
            }
            else if (!string.IsNullOrEmpty((string)this.GetUserMetaData(Username, "Email").ReturnValue))
            {
                ReturnObject ro = this.ValidateUserAnswers(Username, answers);
                if ((ro is ErrorReturnObject ? false : (bool)ro.ReturnValue))
                {
                    Application ai = this.Database.Query<Application>().First<Application>((Application a) => a.ApplicationID == this.CurrentApplication.ApplicationID);
                    ro = this.GeneratePassword(Username);
                    if (ro.ReturnCode == 0)
                    {
                        string NewPassword = (string)ro.ReturnValue;
                        int applicationID = this.CurrentApplication.ApplicationID;
                        int value = UserID.Value;
                        string str = NewPassword;
                        if (this.CurrentApplication.TemporaryPasswordExpirationHours == 0)
                        {
                            nullable = null;
                        }
                        else
                        {
                            DateTime now = DateTime.Now;
                            nullable = new DateTime?(now.AddHours((double)this.CurrentApplication.TemporaryPasswordExpirationHours));
                        }
                        this._AddPassword(applicationID, value, str, nullable);
                        this.SetMustChangePasswordFlag(Username, true);
                        string template = (string.IsNullOrEmpty(ai.UserPasswordResetEmailTemplate) ? string.Concat("Your password for ", ai.ApplicationName, " has been reset to: {password}") : ai.UserPasswordResetEmailTemplate);
                        string subject = (string.IsNullOrEmpty(ai.UserPasswordResetEmailSubject) ? string.Concat(ai.ApplicationName, " Password Reset") : ai.UserPasswordResetEmailSubject);
                        Dictionary<string, string> strs = new Dictionary<string, string>()
                        {
                            { "password", NewPassword }
                        };
                        this._SendEmail(template, subject, Username, strs);
                        this._RecordAuditEvent(this.CurrentApplication.ApplicationID, Username, GlobalUsersService.EventType.PASSWORD_RESET);
                        this._SendUserResetPasswordEmail(Username);
                        errorReturnObject = new SuccessReturnObject(NewPassword);
                    }
                    else
                    {
                        errorReturnObject = new ErrorReturnObject(102, "Error generating password.");
                    }
                }
                else
                {
                    errorReturnObject = new ErrorReturnObject(103, "At least one answer could not be validated.");
                }
            }
            else
            {
                errorReturnObject = new ErrorReturnObject(101, "User has no email address defined.");
            }
            return errorReturnObject;
        }

        public ReturnObject ResetPasswordAndSendEmail(string Username, string FromEmail, string Subject, string EmailTemplate, string[] BCC)
        {
            ReturnObject errorReturnObject;
            int? nullable;
            if (!this._ExistsUsername(this.CurrentApplication.ApplicationID, Username, out nullable))
            {
                errorReturnObject = new ErrorReturnObject(ErrorReturnObject.Error.UsernameNotFound);
            }
            else if (this.Database.Query<UserMetaDatum>().Any<UserMetaDatum>((UserMetaDatum m) => (int?)m.User.UserID == nullable && (m.DataKey == "Email")))
            {
                ReturnObject ro = this.GeneratePassword(Username);
                if (!(ro is ErrorReturnObject))
                {
                    string password = ro.ReturnValue as string;
                    ro = this.AssignPassword(Username, password);
                    if (!(ro is ErrorReturnObject))
                    {
                        this._RecordAuditEvent(this.CurrentApplication.ApplicationID, Username, GlobalUsersService.EventType.PASSWORD_RESET);
                        this.SetMustChangePasswordFlag(Username, true);
                        Dictionary<string, string> strs = new Dictionary<string, string>()
                        {
                            { "password", password }
                        };
                        this._SendEmail(EmailTemplate, Subject, Username, strs);
                        errorReturnObject = new SuccessReturnObject(password);
                    }
                    else
                    {
                        errorReturnObject = ro;
                    }
                }
                else
                {
                    errorReturnObject = ro;
                }
            }
            else
            {
                errorReturnObject = new ErrorReturnObject(101, "Specified user does not have an email address.");
            }
            return errorReturnObject;
        }

        public ReturnObject SearchByMetaData(Dictionary<string, string> MetaData, int pageIndex, int pageSize, out int totalRecords)
        {
            totalRecords = 0;
            List<GlobalUsers.Entities.User> results = this._SearchUserByMetaData(this.CurrentApplication.ApplicationID, MetaData.ToList<KeyValuePair<string, string>>(), false);
            totalRecords = (results == null ? 0 : results.Count<GlobalUsers.Entities.User>());
            return new SuccessReturnObject((results == null ? new User[0] : (
                from ujd in results.ToList<GlobalUsers.Entities.User>()
                select ujd.ToWebServiceUser()).Skip<User>(pageIndex * pageSize).Take<User>(pageSize).ToArray<User>()));
        }

        public ReturnObject SendNewUserEmail(string Username)
        {
            ReturnObject errorReturnObject;
            int? nullable;
            if (!this._ExistsUsername(this.CurrentApplication.ApplicationID, Username, out nullable))
            {
                errorReturnObject = new ErrorReturnObject(ErrorReturnObject.Error.UsernameNotFound);
            }
            else if (this.Database.Query<UserMetaDatum>().Any<UserMetaDatum>((UserMetaDatum m) => (int?)m.User.UserID == nullable && (m.DataKey == "Email")))
            {
                ApplicationInformation ai = this.GetApplicationInformation();
                if (!string.IsNullOrEmpty(ai.UserCreateEmailTemplate))
                {
                    ReturnObject ro = this.GeneratePassword(Username);
                    if (!(ro is ErrorReturnObject))
                    {
                        string password = ro.ReturnValue as string;
                        ro = this.AssignPassword(Username, password);
                        if (!(ro is ErrorReturnObject))
                        {
                            string userCreateEmailTemplate = ai.UserCreateEmailTemplate;
                            string userCreateEmailSubject = ai.UserCreateEmailSubject;
                            Dictionary<string, string> strs = new Dictionary<string, string>()
                            {
                                { "password", password }
                            };
                            this._SendEmail(userCreateEmailTemplate, userCreateEmailSubject, Username, strs);
                            errorReturnObject = new SuccessReturnObject(password);
                        }
                        else
                        {
                            errorReturnObject = ro;
                        }
                    }
                    else
                    {
                        errorReturnObject = ro;
                    }
                }
                else
                {
                    errorReturnObject = new ErrorReturnObject(102, "New user email template not defined.");
                }
            }
            else
            {
                errorReturnObject = new ErrorReturnObject(101, "Specified user does not have an email address.");
            }
            return errorReturnObject;
        }

        public ReturnObject SetAvailableQuestions(ApplicationQuestion[] Question)
        {
            foreach (GlobalUsers.Entities.Question q in
                from qu in this.Database.Query<GlobalUsers.Entities.Question>()
                where qu.Application.ApplicationID == this.CurrentApplication.ApplicationID
                select qu)
            {
                this.Database.Delete(q);
            }
            ApplicationQuestion[] applicationQuestionArray = Question;
            for (int i = 0; i < (int)applicationQuestionArray.Length; i++)
            {
                ApplicationQuestion question = applicationQuestionArray[i];
                GlobalUsers.Entities.Question qq = new GlobalUsers.Entities.Question();
                Application application = new Application()
                {
                    ApplicationID = this.CurrentApplication.ApplicationID
                };
                qq.Application = application;
                qq.QuestionText = question.QuestionText;
                this.Database.SaveOrUpdate(qq);
            }
            return new SuccessReturnObject();
        }

        public ReturnObject SetMustChangePasswordFlag(string Username, bool Flag)
        {
            ReturnObject successReturnObject;
            int? nullable;
            if (this._ExistsUsername(this.CurrentApplication.ApplicationID, Username, out nullable))
            {
                GlobalUsers.Entities.User uu = (
                    from t in this.Database.Query<GlobalUsers.Entities.User>()
                    where (int?)t.UserID == nullable
                    select t).FirstOrDefault<GlobalUsers.Entities.User>();
                uu.MustChangePassword = Flag;
                this.Database.SaveOrUpdate(uu);
                successReturnObject = new SuccessReturnObject();
            }
            else
            {
                successReturnObject = new ErrorReturnObject(ErrorReturnObject.Error.UsernameNotFound);
            }
            return successReturnObject;
        }

        public ReturnObject SetUserMetaData(string Username, string MetaDataKey, string MetaDataValue)
        {
            ReturnObject successReturnObject;
            int? nullable;
            if (this._ExistsUsername(this.CurrentApplication.ApplicationID, Username, out nullable))
            {
                UserMetaDatum md = (
                    from t in this.Database.Query<UserMetaDatum>()
                    where (int?)t.User.UserID == nullable && (t.DataKey.ToLower() == MetaDataKey.ToLower())
                    select t).FirstOrDefault<UserMetaDatum>();
                if (md != null)
                {
                    if (!(MetaDataValue == string.Empty))
                    {
                        md.DataKey = MetaDataKey;
                        md.DataValue = MetaDataValue;
                        this.Database.SaveOrUpdate(md);
                    }
                    else
                    {
                        this.Database.Delete(md);
                    }
                }
                else if (!string.IsNullOrEmpty(MetaDataValue))
                {
                    UserMetaDatum mdNew = new UserMetaDatum()
                    {
                        User = this.Database.Get<GlobalUsers.Entities.User>(nullable.Value),
                        DataKey = MetaDataKey,
                        DataValue = MetaDataValue
                    };
                    this.Database.SaveOrUpdate(mdNew);
                }
                successReturnObject = new SuccessReturnObject();
            }
            else
            {
                successReturnObject = new ErrorReturnObject(ErrorReturnObject.Error.UsernameNotFound);
            }
            return successReturnObject;
        }

        public ReturnObject SetUserMetaDataAll(string Username, Dictionary<string, string> MetaData)
        {
            UserMetaDatum md = null;
            ReturnObject successReturnObject;
            int? nullable;
            if (this._ExistsUsername(this.CurrentApplication.ApplicationID, Username, out nullable))
            {
                foreach (UserMetaDatum umd in
                    from umd in this.Database.Query<UserMetaDatum>()
                    where (int?)umd.User.UserID == nullable
                    select umd)
                {
                    this.Database.Delete(umd);
                }
                foreach (string key in MetaData.Keys)
                {
                    md = new UserMetaDatum()
                    {
                        User = this.Database.Get<GlobalUsers.Entities.User>(nullable.Value),
                        DataKey = key,
                        DataValue = MetaData[key]
                    };
                    this.Database.SaveOrUpdate(md);
                }
                successReturnObject = new SuccessReturnObject();
            }
            else
            {
                successReturnObject = new ErrorReturnObject(ErrorReturnObject.Error.UsernameNotFound);
            }
            return successReturnObject;
        }

        public ReturnObject SetUserQuestionsAndAnswers(string Username, UserQuestion[] QuestionAndAnswers)
        {
            QuestionAnswer qa = null;
            ReturnObject successReturnObject;
            int? nullable;
            if (this._ExistsUsername(this.CurrentApplication.ApplicationID, Username, out nullable))
            {
                foreach (QuestionAnswer qna in
                    from qna in this.Database.Query<QuestionAnswer>()
                    where qna.User.UserID == (int)nullable
                    select qna)
                {
                    this.Database.Delete(qna);
                }
                UserQuestion[] questionAndAnswers = QuestionAndAnswers;
                for (int i = 0; i < (int)questionAndAnswers.Length; i++)
                {
                    UserQuestion question = questionAndAnswers[i];
                    qa = question.Copy<QuestionAnswer>();
                    qa.User = this.Database.Get<GlobalUsers.Entities.User>(nullable.Value);
                    qa.AnswerSalt = Password.CreateRandomSalt();
                    qa.Answer = Password.CreateSHA512Hash(question.Answer.ToLower(), qa.AnswerSalt);
                    this.Database.SaveOrUpdate(qa);
                }
                successReturnObject = new SuccessReturnObject();
            }
            else
            {
                successReturnObject = new ErrorReturnObject(ErrorReturnObject.Error.UsernameNotFound);
            }
            return successReturnObject;
        }

        public ReturnObject ShouldShowPasswordExpirationWarning(string Username)
        {
            int? UserID;
            ReturnObject successReturnObject;
            if (this._ExistsUsername(this.CurrentApplication.ApplicationID, Username, out UserID))
            {
                successReturnObject = new SuccessReturnObject((object)this._ShouldShowPasswordExpirationWarning(this.CurrentApplication.ApplicationID, Username));
            }
            else
            {
                successReturnObject = new ErrorReturnObject(ErrorReturnObject.Error.UsernameNotFound);
            }
            return successReturnObject;
        }

        public ReturnObject UpdateApplication(ApplicationInformation ai)
        {
            ReturnObject errorReturnObject;
            Application aa = this.Database.Query<Application>().FirstOrDefault<Application>((Application t) => t.ApplicationName.ToLower() == ai.ApplicationName.ToLower());
            if (aa == null)
            {
                errorReturnObject = new ErrorReturnObject(101, string.Format("The requested application {0} does not exist.", ai.ApplicationName));
            }
            else if ((aa.ApplicationName == this.CurrentApplication.ApplicationName ? true : this.IsCurrentApplicationManager))
            {
                aa.CopyFrom(ai, new string[] { "ApplicationID" });
                errorReturnObject = this.CreateOrUpdateApplication(aa, ai);
            }
            else
            {
                errorReturnObject = new ErrorReturnObject(102, "Only the management application can execute this function for another application.");
            }
            return errorReturnObject;
        }

        public ReturnObject UpdateUser(User user)
        {
            UserRole ur = null;
            // GlobalUsersService variable;
            ReturnObject errorReturnObject;
            int? nullable;
            if (!this._ExistsUsername(this.CurrentApplication.ApplicationID, user.Username, out nullable))
            {
                errorReturnObject = new ErrorReturnObject(ErrorReturnObject.Error.UsernameNotFound);
            }
            else if ((user.Attributes == null ? true : (
                from u in (IEnumerable<UserAttribute>)user.Attributes
                group u by u.Key).Count<IGrouping<string, UserAttribute>>((IGrouping<string, UserAttribute> u) => u.Count<UserAttribute>() > 1) <= 1))
            {
                GlobalUsers.Entities.User db_user = this.Database.Query<GlobalUsers.Entities.User>().First<GlobalUsers.Entities.User>((GlobalUsers.Entities.User u) => u.UserID == nullable.Value);
                bool new_username = (string.IsNullOrEmpty(user.NewUsername) ? false : !user.Username.Equals(user.NewUsername));
                if (!new_username)
                {
                    db_user.Username = user.Username;
                }
                else
                {
                    if (this.Database.Query<GlobalUsers.Entities.User>().Any<GlobalUsers.Entities.User>((GlobalUsers.Entities.User u) => (u.Username == user.NewUsername) && (int?)u.UserID != nullable && u.Application.ApplicationID == this.CurrentApplication.ApplicationID))
                    {
                        errorReturnObject = new ErrorReturnObject(101, "Username exists.");
                        return errorReturnObject;
                    }
                    db_user.Username = user.NewUsername;
                    //this._RecordAuditEvent(this.CurrentApplication.ApplicationID, user.Username, GlobalUsersService.EventType.USERNAME_CHANGED, string.Format("{0} changed to {1}", user.Username, user.NewUsername));
                    this._WriteToAuditLog(this.CurrentApplication.ApplicationID, user.Username, GlobalUsersService.EventType.USERNAME_CHANGED, string.Format("{0} changed to {1}", user.Username, user.NewUsername));
                }
                if (user.IsActive ^ db_user.IsActive)
                {
                    if (!user.IsActive)
                    {
                        this._RecordAuditEvent(this.CurrentApplication.ApplicationID, user.Username, GlobalUsersService.EventType.USER_INACTIVED);
                    }
                    else
                    {
                        this._RecordAuditEvent(this.CurrentApplication.ApplicationID, user.Username, GlobalUsersService.EventType.USER_ACTIVED);
                    }
                }
                if (user.IsEnabled ^ db_user.IsEnabled)
                {
                    if (!user.IsEnabled)
                    {
                        this._RecordAuditEvent(this.CurrentApplication.ApplicationID, user.Username, GlobalUsersService.EventType.USER_DISABLED);
                    }
                    else
                    {
                        this._RecordAuditEvent(this.CurrentApplication.ApplicationID, user.Username, GlobalUsersService.EventType.USER_ENABLED);
                    }
                }
                db_user.IsEnabled = user.IsEnabled;
                db_user.IsActive = user.IsActive;
                db_user.IsLockedOut = user.IsLockedOut;
                db_user.MustChangePassword = user.MustChangePassword;
                db_user.IsPasswordExpired = user.IsPasswordExpired;
                this.Database.SaveOrUpdate(db_user);
                if (new_username)
                {
                    foreach (AuditLog newUsername in
                        from u in this.Database.Query<AuditLog>()
                        where (u.Username == user.Username) && u.Application.ApplicationID == this.CurrentApplication.ApplicationID
                        select u)
                    {
                        newUsername.Username = user.NewUsername;
                        this.Database.SaveOrUpdate(newUsername);
                    }
                }
                if (!string.IsNullOrEmpty(user.Email))
                {
                    List<UserAttribute> attr = (user.Attributes != null ? user.Attributes.ToList<UserAttribute>() : new List<UserAttribute>());
                    if (attr.Any<UserAttribute>((UserAttribute a) => a.Key == "Email"))
                    {
                        attr.Remove(attr.FirstOrDefault<UserAttribute>((UserAttribute a) => a.Key == "Email"));
                    }
                    UserAttribute userAttribute = new UserAttribute()
                    {
                        Key = "Email",
                        Value = user.Email
                    };
                    attr.Add(userAttribute);
                    user.Attributes = attr.ToArray();
                }
                if (user.Attributes != null)
                {
                    IQueryable<UserMetaDatum> existing =
                        from m in this.Database.Query<UserMetaDatum>()
                        where m.User.UserID == nullable.Value
                        select m;
                    foreach (UserMetaDatum userMetaDatum in existing)
                    {
                        if (!((IEnumerable<UserAttribute>)user.Attributes).Any<UserAttribute>((UserAttribute a) => a.Key == userMetaDatum.DataKey))
                        {
                            this.Database.Delete(userMetaDatum);
                        }
                    }
                    this.Database.Flush();
                    UserAttribute[] attributes = user.Attributes;
                    for (int i = 0; i < (int)attributes.Length; i++)
                    {
                        UserAttribute userAttribute1 = attributes[i];
                        UserMetaDatum userMetaDatum1 = existing.FirstOrDefault<UserMetaDatum>((UserMetaDatum e) => e.DataKey == userAttribute1.Key);
                        if (userMetaDatum1 == null)
                        {
                            UserMetaDatum userMetaDatum2 = new UserMetaDatum()
                            {
                                User = this.Database.Get<GlobalUsers.Entities.User>(nullable.Value)
                            };
                            userMetaDatum1 = userMetaDatum2;
                        }
                        UserMetaDatum md = userMetaDatum1;
                        md.DataKey = userAttribute1.Key;
                        md.DataValue = userAttribute1.Value;
                        this.Database.SaveOrUpdate(md);
                    }
                }
                if (user.Roles != null)
                {
                    IQueryable<UserRole> userRoles =
                        from r in this.Database.Query<UserRole>()
                        where r.User.UserID == nullable.Value
                        select r;
                    foreach (UserRole urr in userRoles)
                    {
                        if (!user.Roles.Contains<string>(urr.Role.Name))
                        {
                            this.Database.Delete(urr);
                        }
                    }
                    this.Database.Flush();
                    foreach (string str in user.Roles.Where<string>((string ro) => !userRoles.Any<UserRole>((UserRole e) => e.Role.Name.ToLower() == ro.ToLower())))
                    {
                        Role role = this.Database.Query<Role>().FirstOrDefault<Role>((Role rl) => rl.Name.ToLower() == str.ToLower());
                        ur = new UserRole()
                        {
                            User = db_user,
                            Role = role
                        };
                        this.Database.SaveOrUpdate(ur);
                    }
                }
                this.Database.Flush();
                errorReturnObject = new SuccessReturnObject();
            }
            else
            {
                errorReturnObject = new ErrorReturnObject(101, "User attribute key appears more than once.");
            }
            return errorReturnObject;
        }

        public ReturnObject ValidateUser(string Username, string Password)
        {
            ReturnObject successReturnObject;
            DateTime now;
            bool flag;
            GlobalUsers.Entities.User user;
            if (!this._IsIPLockedOut())
            {
                this._ExistsUsername(this.CurrentApplication.ApplicationID, Username, out user);
                if (user == null)
                {
                    this._RecordAuditEvent(this.CurrentApplication.ApplicationID, Username, GlobalUsersService.EventType.LOGIN_FAILURE_BADUSERNAME);
                    successReturnObject = new SuccessReturnObject(false);
                }
                else if (user.IsLockedOut)
                {
                    this._RecordAuditEvent(this.CurrentApplication.ApplicationID, Username, GlobalUsersService.EventType.LOGIN_FAILURE_LOCKEDOUT);
                    successReturnObject = new SuccessReturnObject(false);
                }
                else if (!user.IsEnabled)
                {
                    this._RecordAuditEvent(this.CurrentApplication.ApplicationID, Username, GlobalUsersService.EventType.LOGIN_FAILURE_DISABLED);
                    successReturnObject = new SuccessReturnObject(false);
                }
                else if (user.IsActive)
                {
                    PasswordHistory pwhistory = (
                        from t in this.Database.Query<PasswordHistory>()
                        where t.UserID == user.UserID
                        orderby t.PasswordDate descending
                        select t).FirstOrDefault<PasswordHistory>();
                    if (pwhistory == null)
                    {
                        flag = false;
                    }
                    else
                    {
                        DateTime? expiration = pwhistory.Expiration;
                        if (!expiration.HasValue)
                        {
                            flag = true;
                        }
                        else
                        {
                            expiration = pwhistory.Expiration;
                            now = DateTime.Now;
                            //flag = (expiration.HasValue ? (int)(expiration.GetValueOrDefault() < now) : 0) == 0;
                            if (expiration < now)
                            {
                                flag = true;
                            }
                            else
                            {
                                flag = false;
                            }
                        }
                    }
                    if (flag)
                    {
                        byte[] hashFromSubmission = WebService.Password.CreateSHA512Hash(Password, pwhistory.PasswordSalt);
                        if (!pwhistory.PasswordHash.SequenceEqual<byte>(hashFromSubmission))
                        {
                            Application aa = (
                                from t in this.Database.Query<Application>()
                                where t.ApplicationID == this.CurrentApplication.ApplicationID
                                select t).First<Application>();
                            if (aa.UserAttemptsBeforeLockout > 0)
                            {
                                now = DateTime.Now;
                                DateTime dateTime = now.AddMinutes((double)(-1 * aa.UserLockoutWindow));
                                if ((
                                    from t in this.Database.Query<AuditLog>()
                                    where (t.Username.ToLower() == Username.ToLower()) && t.EventType.EventTypeID == 1 && (t.EventDate > dateTime)
                                    select t).Count<AuditLog>() + 1 >= aa.UserAttemptsBeforeLockout)
                                {
                                    user.IsLockedOut = true;
                                    this.Database.SaveOrUpdate(user);
                                    this._SendAdminUserLockoutEmail(this.CurrentApplication.ApplicationID, Username);
                                    this._SendUserLockoutEmail(this.CurrentApplication.ApplicationID, Username);
                                }
                            }
                            this._RecordAuditEvent(this.CurrentApplication.ApplicationID, Username, GlobalUsersService.EventType.LOGIN_FAILURE_BADPASSWORD);
                            successReturnObject = new SuccessReturnObject(false);
                        }
                        else
                        {
                            if (user.IsLockedOut)
                            {
                                user.IsLockedOut = false;
                                this.Database.SaveOrUpdate(user);
                            }
                            this._RecordAuditEvent(this.CurrentApplication.ApplicationID, Username, GlobalUsersService.EventType.LOGIN_SUCCESS);
                            successReturnObject = new SuccessReturnObject(true);
                        }
                    }
                    else
                    {
                        this._RecordAuditEvent(this.CurrentApplication.ApplicationID, Username, GlobalUsersService.EventType.LOGIN_FAILURE_BADPASSWORD);
                        successReturnObject = new SuccessReturnObject(false);
                    }
                }
                else
                {
                    this._RecordAuditEvent(this.CurrentApplication.ApplicationID, Username, GlobalUsersService.EventType.LOGIN_FAILURE_INACTIVE);
                    successReturnObject = new SuccessReturnObject(false);
                }
            }
            else
            {
                int? nullable = this._GetCallerIP();
                ApplicationInformation applicationInformation = this.GetApplicationInformation();
                if (nullable.HasValue)
                {
                    if ((
                        from t in this.Database.Query<AuditLog>()
                        where (int?)t.UserIP == nullable && t.EventType.EventTypeID == 4 && (t.EventDate > DateTime.Now.AddMinutes((double)(-1 * applicationInformation.IPLockoutWindow)))
                        orderby t.EventDate descending
                        select t).Count<AuditLog>() == 0)
                    {
                        this._SendIPLockoutEmail(this.CurrentApplication.ApplicationID, nullable.Value);
                    }
                }
                this._RecordAuditEvent(this.CurrentApplication.ApplicationID, Username, GlobalUsersService.EventType.LOGIN_FAILURE_LOCKEDOUTIP);
                successReturnObject = new SuccessReturnObject(false);
            }
            return successReturnObject;
        }

        public ReturnObject ValidateUserAnswers(string Username, UserQuestion[] Question)
        {
            int? UserID;
            ReturnObject errorReturnObject;
            if (!this._ExistsUsername(this.CurrentApplication.ApplicationID, Username, out UserID))
            {
                errorReturnObject = new ErrorReturnObject(ErrorReturnObject.Error.UsernameNotFound);
            }
            else if (Question.Count<UserQuestion>() != 0)
            {
                UserQuestion[] question = Question;
                int num = 0;
                while (num < (int)question.Length)
                {
                    UserQuestion userQuestion = question[num];
                    QuestionAnswer qa = this.Database.Query<QuestionAnswer>().FirstOrDefault<QuestionAnswer>((QuestionAnswer q) => q.UserQuestionID == userQuestion.UserQuestionID);
                    if (qa == null)
                    {
                        errorReturnObject = new ErrorReturnObject(101, "Question is not defined for this user.");
                        return errorReturnObject;
                    }
                    else if (Password.CreateSHA512Hash(userQuestion.Answer.ToLower(), qa.AnswerSalt).SequenceEqual<byte>(qa.Answer))
                    {
                        num++;
                    }
                    else
                    {
                        errorReturnObject = new SuccessReturnObject(false);
                        return errorReturnObject;
                    }
                }
                errorReturnObject = new SuccessReturnObject(true);
            }
            else
            {
                errorReturnObject = new ErrorReturnObject(102, "No answers were provided.");
            }
            return errorReturnObject;
        }

        public ReturnObject ValidateUserTicket(string username, string ticket)
        {
            ReturnObject errorReturnObject;
            DateTime time_received = DateTime.Now;
            string applicationName = this.CurrentApplication.ApplicationName;
            var q =
                from t in this.Database.Query<UserTicket>()
                join u in this.Database.Query<GlobalUsers.Entities.User>() on t.User.UserID equals u.UserID
                join a in this.Database.Query<Application>() on u.Application.ApplicationID equals a.ApplicationID
                where (u.Username.ToLower() == username.ToLower()) && (a.ApplicationName.ToLower() == applicationName.ToLower())
                select new { Ticket = t, Application = a, User = u };
            if (q.Count() > 1)
            {
                errorReturnObject = new ErrorReturnObject(101, "The user has more than one ticket.");
            }
            else if (q.Count() != 0)
            {
                var user_ticket = q.First();
                if ((time_received - user_ticket.Ticket.TimeCreated).TotalSeconds > (double)user_ticket.Application.TicketLifespan)
                {
                    errorReturnObject = new ErrorReturnObject(103, "The supplied ticket has expired.");
                }
                else if (Password.CreateSHA512Hash(ticket, Encoding.ASCII.GetBytes(user_ticket.Ticket.TimeCreated.ToString())).SequenceEqual<byte>(user_ticket.Ticket.TicketValue))
                {
                    errorReturnObject = new SuccessReturnObject();
                }
                else
                {
                    errorReturnObject = new ErrorReturnObject(104, "The supplied ticket value is invalid.");
                }
            }
            else
            {
                errorReturnObject = new ErrorReturnObject(102, "The user does not have any tickets.");
            }
            return errorReturnObject;
        }

        public enum EventType
        {
            LOGIN_SUCCESS = 0,
            LOGIN_FAILURE_BADPASSWORD = 1,
            LOGIN_FAILURE_BADUSERNAME = 2,
            LOGIN_FAILURE_LOCKEDOUT = 3,
            LOGIN_FAILURE_LOCKEDOUTIP = 4,
            LOGIN_FAILURE_DISABLED = 5,
            PASSWORD_CHANGED = 6,
            PASSWORD_ASSIGNED = 7,
            PASSWORD_RESET = 8,
            USER_ADDED_TO_ROLE = 9,
            USER_REMOVED_FROM_ROLE = 10,
            USER_CREATED = 11,
            LOGIN_FAILURE_INACTIVE = 12,
            USERNAME_CHANGED = 13,
            USER_INACTIVED = 14,
            USER_ACTIVED = 15,
            USER_DISABLED = 16,
            USER_ENABLED = 17
        }
    }
}