using GlobalUsers.DataHelper;
using GlobalUsers.Entities;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace WebService
{
    public static class DataEntityExtension
    {
        public static ApplicationInformation ToApplicationInformation(this Application aa)
        {
            ApplicationInformation applicationInformation;
            ISession Database = NhibernateBootStrapper.GetSession();
            try
            {
                ApplicationInformation ai = aa.Copy<ApplicationInformation>();
                ai.UserCount = (
                    from u in Database.Query<GlobalUsers.Entities.User>()
                    where u.Application.ApplicationID == aa.ApplicationID
                    select u).Count<GlobalUsers.Entities.User>();
                ai.Roles = aa.Roles.ToDictionary<Role, int, string>((Role r) => r.RoleID, (Role r) => r.Name);
                ai.Questions = (
                    from q in aa.Questions.ToList<Question>()
                    select q.Copy<ApplicationQuestion>()).ToArray<ApplicationQuestion>();
                ai.TrustedApplications = aa.ApplicationTrusts.ToDictionary<ApplicationTrust, int, string>((ApplicationTrust t) => t.TrustedApplication.ApplicationID, (ApplicationTrust t) => t.TrustedApplication.ApplicationName);
                if (!(aa.ApplicationName == "ApplicationManagement"))
                {
                    ai.Accounts = (
                        from app_acc in aa.ApplicationAccounts
                        where app_acc.Application.ApplicationID == aa.ApplicationID
                        select app_acc.Account.Username).ToArray<string>();
                }
                else
                {
                    ai.Accounts = (
                        from a in Database.Query<Account>()
                        where a.Admin
                        select a.Username).ToArray<string>();
                }
                applicationInformation = ai;
            }
            finally
            {
                if (Database != null)
                {
                    Database.Dispose();
                }
            }
            return applicationInformation;
        }

        public static User ToWebServiceUser(this GlobalUsers.Entities.User db_user)
        {
            User ret_user = db_user.Copy<User>();
            ret_user.Application = db_user.Application.ApplicationName;
            ret_user.Attributes = db_user.UserMetaDatas.ToList<UserMetaDatum>().ConvertAll<UserAttribute>((UserMetaDatum kv) => new UserAttribute()
            {
                Key = kv.DataKey,
                Value = kv.DataValue
            }).ToArray();
            ret_user.Email = (
                from at in (IEnumerable<UserAttribute>)ret_user.Attributes
                where at.Key == "Email"
                select at into um
                select um.Value).FirstOrDefault<string>();
            ret_user.Roles = (
                from ur in db_user.UserRoles
                select ur.Role.Name).ToArray<string>();
            ret_user.IsEnabled = db_user.IsEnabled;
            ret_user.IsActive = db_user.IsActive;
            ret_user.IsLockedOut = db_user.IsLockedOut;
            ret_user.MustChangePassword = db_user.MustChangePassword;
            ret_user.IsPasswordExpired = db_user.IsPasswordExpired;
            return ret_user;
        }
    }
}