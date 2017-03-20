using Aspensys.GlobalUsers.WebServiceClient.UserService;
using GlobalUsers.DataHelper;
using GuamApplication.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace ManagementApplication.Models
{
	[Serializable]
	public class Application
	{
		public string[] Accounts
		{
			get;
			set;
		}

		[DisplayName("Application ID")]
		public int ApplicationID
		{
			get;
			set;
		}

		[Description("The name of the application. This name will be specified in the code of the application. Changes to the names of existing applications should be coordinated with the application programmers.")]
		[DisplayName("Application Name")]
		public string ApplicationName
		{
			get;
			set;
		}

		[DisplayName("Application Password")]
		public string ApplicationPassword
		{
			get;
			set;
		}

		[Description("When this is enabled, passwords will be checked against a list of common passwords. Recommended value: <b>unchecked</b>.")]
		[DisplayName("Check against wordlist")]
		public bool CheckPasswordAgainstWordlist
		{
			get;
			set;
		}

		[Description("[todo]")]
		[DisplayName("Daily Expiration Email Subject")]
		public string DailyExpirationEmailSubject
		{
			get;
			set;
		}

		[Description("[todo]")]
		[DisplayName("Daily Expiration Email Template")]
		public string DailyExpirationEmailTemplate
		{
			get;
			set;
		}

		[Description("[todo]")]
		[DisplayName("Daily Inactivity Email Subject")]
		public string DailyInactivityEmailSubject
		{
			get;
			set;
		}

		[Description("[todo]")]
		[DisplayName("Daily Inactivity Email Template")]
		public string DailyInactivityEmailTemplate
		{
			get;
			set;
		}

		[Description("Indicates the number of days a user can be inactive before going into \"inactive\" status and being forbidden to log in. Set to 0 to not use this feature.")]
		[DisplayName("Inactivity Days")]
		[Options(new int[] { 0, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 60, 90, 180, 270, 365 })]
		[Range(0, 365)]
		public int InactivityDays
		{
			get;
			set;
		}

		[Description("Indicates the number of days prior to inactivity status during which emails should be sent. For example, if this is 10, users will begin receiving emails 10 days before they enter inactive status (and stop once they enter inactive status). This value is ignored if Activity Days is set to 0.")]
		[DisplayName("Inactivity Warning Window")]
		[Options(new int[] { 0, 5, 10, 15, 20 })]
		[Range(0, 128)]
		public byte InactivityWarningWindow
		{
			get;
			set;
		}

		[Description("The number of failed login attempts that may be made to the application from a single IP address within a given window before that IP address is prohibited from making additional login attempts.<br />Recommended value: <b>5</b>.<br />Enter 0 to not use this feature.")]
		[DisplayName("IP Attempts Before Lockout")]
		[Options(new int[] { 0, 100, 1000, 10000 })]
		public int IPAttemptsBeforeLockout
		{
			get;
			set;
		}

		[Description("A comma- or semi-colon-delimited list of email addresses to which an administrative message should be sent when a user in this application becomes locked out. Leave this field blank to not use this feature.")]
		[DisplayName("IP Lockout Email Recipients")]
		public string IPLockoutEmailRecipients
		{
			get;
			set;
		}

		[Description("The window in which multiple failed login attempts from a particular IP address must be made in order to trigger a restriction against that IP address. For example, if this value is set to 5, and and the number attempts before lockout is set to 5, then when a failed attempt is made, the system will see if 4 other failed login attempts from this IP have been made in the past 5 minutes. If so, the IP address will be subject to a cool-off period before being allowed to make further login attempts.<br />Recommended value: <b>5</b>.")]
		[DisplayName("IP Lockout Window (mins)")]
		[Options(new int[] { 0, 10, 60, 1440 })]
		public int IPLockoutWindow
		{
			get;
			set;
		}

		[Description("When set to false, no users belonging to this application can authenticate.")]
		[DisplayName("Active")]
		public bool IsActive
		{
			get;
			set;
		}

		[Description("The number of questions to which each user is required to provide answers.  This number should not exceed the number of defined questions.")]
		[DisplayName("Number Of Password Reset Questions")]
		[Range(0, 10)]
		public byte NumberOfPasswordResetQuestions
		{
			get;
			set;
		}

		[Description("A list of special characters that may be used in passwords in addition to alphanumeric characters. If left blank, no character restrictions are applied to passwords. <br />Recommended value: <b>!@#$%^&*</b>.")]
		[DisplayName("Allowed special characters")]
		public string PasswordAllowedSpecialCharacters
		{
			get;
			set;
		}

		[Description("When this is enabled, users are permitted to create passwords that contain their username. When this is disabled, passwords may not contain the user's username (regardless of the way it is capitalized within the password).<br />Recommended value: <b>checked</b>.")]
		[DisplayName("Allow username in password")]
		public bool PasswordAllowUsername
		{
			get;
			set;
		}

		[Description("The number of days a user may have a particular password. If a user logs in after this many days have elapsed since the creation of the password, they will be prompted to change their password immediately. Set to 0 in order to not use password expiration.<br />Recommended value: <b>180</b>.")]
		[DisplayName("Days before password expiration")]
		[Options(new int[] { 0, 30, 60, 90, 120, 150, 180 })]
		[Range(0, 999)]
		public int PasswordExpirationDays
		{
			get;
			set;
		}

		[Description("The number of days prior to password expiration at which the user should be alerted to the upcoming expiration. For example, if this value is 10, the user may see a message after successful login that says, \"Your password will expire in 10 days, would you like to change it now?\" If password expiration is not being used, this value has no meaning.<br />Recommended value: <b>10</b>.")]
		[DisplayName("Password Expiration Warning Window (days)")]
		[Options(new int[] { 0, 5, 10, 15 })]
		[Range(0, 128)]
		public byte PasswordExpirationWarningWindow
		{
			get;
			set;
		}

		[Description("If the user had the same password previously, it must be this many days old or older in order for the user to be able to have it again. Enter 0 to not enforce a password age requirement.")]
		[DisplayName("Minimum Password Age (Days)")]
		[Options(new int[] { 0, 1, 2, 3, 4, 5 })]
		[Range(0, 999)]
		public int PasswordHistoryMinimumAgeDays
		{
			get;
			set;
		}

		[Description("If the user had the same password previously, they must have used this many passwords afterwards in order for them to be permitted to have this password again. Enter 0 to not enforce a password iteration requirement.")]
		[DisplayName("Password History Minimum Iterations")]
		[Options(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 })]
		[Range(0, 99)]
		public int PasswordHistoryMinimumIterations
		{
			get;
			set;
		}

		[Description("The maximum number of characters allowed in passwords in this application.<br />Recommended value: <b>20</b>.")]
		[DisplayName("Maximum Password Length")]
		[Options(new int[] { 10, 15, 20, 25, 30 })]
		[Required]
		public byte PasswordMaxLength
		{
			get;
			set;
		}

		[Description("The minimum number of characters allowed in passwords in this application.<br />Recommended value: <b>8</b>.")]
		[DisplayName("Minimum Password Length")]
		[Options(new int[] { 5, 6, 7, 8, 9, 10 })]
		[Range(5, 99, ErrorMessage="Minimum password length must be between 1 and 99 characters")]
		[Required]
		public byte PasswordMinLength
		{
			get;
			set;
		}

		[Description("The name and/or email address of the account manager point of contact associated with this project.")]
		[DisplayName("Project Account Manager Point Of Contact")]
		public string ProjectAccountManagerPointOfContact
		{
			get;
			set;
		}

		[Description("The name and/or email address of the project manager associated with this application.")]
		[DisplayName("Project Manager")]
		public string ProjectManager
		{
			get;
			set;
		}

		[Description("The name and/or email address of the primary point of contact or client liaison associated with this application.")]
		[DisplayName("Project Point Of Contact")]
		public string ProjectPointOfContact
		{
			get;
			set;
		}

		public ApplicationQuestion[] Questions
		{
			get;
			set;
		}

		[XmlIgnore]
		public Dictionary<int, string> Roles
		{
			get;
			set;
		}

		[Description("Indicates whether users whose passwords are about to expire should receive a daily message when they are within the expiration warning window. For example, if the warning window for the application is 10 days, they will begin receiving daily emails 10 days prior to expiration. They will not receive emails after their password expires.")]
		[DisplayName("Send Daily Expiration Emails")]
		public bool SendDailyExpirationEmails
		{
			get;
			set;
		}

		[DisplayName("Send Daily Inactivity Emails")]
		public bool SendDailyInactivityEmails
		{
			get;
			set;
		}

		[Description("This email address will be BCC'd on all emails sent from this application.")]
		[DisplayName("Email BCC Address")]
		public string SendEmailsBCC
		{
			get;
			set;
		}

		[Description("This is the email address that will appear to have sent emails from this application.")]
		[DisplayName("Email From Address")]
		public string SendEmailsFrom
		{
			get;
			set;
		}

		public List<SerializeKeyValuePair<int, string>> SerializeRoles
		{
			get
			{
				List<SerializeKeyValuePair<int, string>> list;
				if (this.Roles == null)
				{
					list = null;
				}
				else
				{
					list = (
						from r in this.Roles.ToList<KeyValuePair<int, string>>()
						select new SerializeKeyValuePair<int, string>()
						{
							Key = r.Key,
							Value = r.Value
						}).ToList<SerializeKeyValuePair<int, string>>();
				}
				return list;
			}
			set
			{
				if (this.Roles == null)
				{
					this.Roles = new Dictionary<int, string>();
				}
				foreach (SerializeKeyValuePair<int, string> kv in value)
				{
					this.Roles.Add(kv.Key, kv.Value);
				}
			}
		}

		[Description("This is the length of time, in hours, a temporary password is valid.")]
		[DisplayName("Temporary Password Lifespan")]
		[Options(new int[] { 0, 4, 24, 48, 72 })]
		[Range(0, 240)]
		public int TemporaryPasswordExpirationHours
		{
			get;
			set;
		}

		[Description("The number of seconds for which a given single sign on ticket is valid.  A value of 0 indicates no SSO is being used.  Recommended Value: 10")]
		[DisplayName("SSO Ticket Lifespan")]
		[Range(0, 180)]
		public int TicketLifespan
		{
			get;
			set;
		}

		[Description("When this is enabled, the system will not perform any password policy checks, regardless of what is entered into the fields. All passwords will be considered valid, and the application programmer is expected to write custom password checking code inside the application.<br />Recommended value: <b>unchecked</b>.")]
		[DisplayName("Use Custom Password Validation")]
		public bool UseCustomPasswordValidation
		{
			get;
			set;
		}

		[Description("The number of failed login attempts that may be made against the same username in within a given window before that username is locked out.<br />Recommended value: <b>5</b>.<br />Enter 0 to not use this feature.")]
		[DisplayName("User Attempts Before Lockout")]
		[Options(new int[] { 0, 3, 4, 5, 6, 7, 8, 9, 10 })]
		[Range(0, 99)]
		public byte UserAttemptsBeforeLockout
		{
			get;
			set;
		}

		public int UserCount
		{
			get;
			set;
		}

		[Description("A comma- or semi-colon-delimited list of email addresses to which an administrative message should be sent when a user in this application is created, disabed, or deactivated. Leave this field blank to not use this feature.")]
		[DisplayName("User Event Email Recipients")]
		public string UserCreatedEmailRecipients
		{
			get;
			set;
		}

		[Description("The email subject link to use to send emails to the owner of a new account. ")]
		[DisplayName("Create User Subject")]
		public string UserCreateEmailSubject
		{
			get;
			set;
		}

		[Description("The email template to use to send emails to the owner of a new account.  This email will only be fired if an e-mail address is given for the new account.")]
		[DisplayName("Create User Template")]
		public string UserCreateEmailTemplate
		{
			get;
			set;
		}

		[Description("A comma- or semi-colon-delimited list of email addresses to which an administrative message should be sent when a user in this application becomes locked out. Leave this field blank to not use this feature.")]
		[DisplayName("User Lockout Email Recipients")]
		public string UserLockoutEmailRecipients
		{
			get;
			set;
		}

		[Description("This is the subject of the email sent to users, when their account is locked out.")]
		[DisplayName("User Lockout Subject")]
		public string UserLockoutEmailSubject
		{
			get;
			set;
		}

		[Description("This is the email sent to users, when their account becomes locked out.")]
		[DisplayName("User Lockout Template")]
		public string UserLockoutEmailTemplate
		{
			get;
			set;
		}

		[Description("The window in which multiple failed login attempts to a particular username must be made in order to trigger lockout of that username. For example, if this value is set to 5, and and the number user attempts before lockout is set to 5, then when a failed attempt is made, the system will see if 4 other failed login attempts against this username have been made in the past 5 minutes. If so, the account will be locked out.<br />Recommended value: <b>5</b>.")]
		[DisplayName("User Lockout Window (mins)")]
		[Options(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 15 })]
		[Range(0, 999)]
		public int UserLockoutWindow
		{
			get;
			set;
		}

		[Description("This is the subject of the email sent to users, when they reset their password.")]
		[DisplayName("Password Reset Subject")]
		public string UserPasswordResetEmailSubject
		{
			get;
			set;
		}

		[Description("The email template to use to send emails to the owner of an account, when the password is reset.")]
		[DisplayName("Password Reset Template")]
		public string UserPasswordResetEmailTemplate
		{
			get;
			set;
		}

		[Description("A comma- or semi-colon-delimited list of email addresses to which an administrative message should be sent when a user in this application has their password reset. Leave this field blank to not use this feature.")]
		[DisplayName("User Password Reset Recipients")]
		public string UserPasswordResetRecipients
		{
			get;
			set;
		}

		[Description("A message that explains in natural language the requirements for creating a valid username. This message may be displayed to the user when they are asked to create a username, or unsuccessfully create a username. It may also appear as a guide to administrators who are creating new users. For example, the message might read, \"Valid usernames are between 3 and 25 characters in length, and may contain uppercase letters, lowercase letters, and numbers.\"")]
		[DisplayName("Valid Username Explanation")]
		public string ValidUsernameExplanation
		{
			get;
			set;
		}

		[Description("A regular expression that will be used to validate usernames that are created in the application.<br />Recommended value: ^[a-zA-Z0-9\\.@\\-_+]{3,25}$<br />Applications that allow email addresses as usernames should use a less restrictive regular expression.")]
		[DisplayName("Valid Username Regular Expression")]
		public string ValidUsernameRegex
		{
			get;
			set;
		}

		public Application()
		{
		}

		public ApplicationInformation ToApplicationInformation()
		{
			if (this.Questions == null)
			{
				this.Questions = new ApplicationQuestion[0];
			}
			ApplicationInformation ai = new ApplicationInformation();
			ai.CopyFrom(this);
			return ai;
		}
	}
}