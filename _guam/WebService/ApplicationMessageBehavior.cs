using GlobalUsers.Entities;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace WebService
{
    public class ApplicationMessageBehavior : IDispatchMessageInspector
    {
        public ApplicationMessageBehavior()
        {
        }

        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            NHibernate.ISession Database = NhibernateBootStrapper.GetSession();
            try
            {
                string header = OperationContext.Current.IncomingMessageHeaders.GetHeader<string>("application-name", "http://aspensys.com/");

                // auth hack - daquinohd
                // ServiceSecurityContext.PrimaryIdentity.Name is not returning a value, so I'm 
                // using System.Security.Principal.WindowsIdentity.GetCurrent().Name to get the AppPool Identity for now.
                string name = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;
                name = System.Security.Principal.WindowsIdentity.GetCurrent().Name;

                UnauthorizedAccessException uae = new UnauthorizedAccessException(string.Format("{0} cannot access the application {1}", name, header));
                if (!OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.IsAuthenticated)
                {
                    FaultException faultException = new FaultException(string.Format("{0} cannot access the application {1}, it is unauthenticated", name, header));
                }
                Account account = Database.Query<Account>().FirstOrDefault<Account>((Account acc) => acc.Username.ToLower() == name.ToLower());
                if (account == null)
                {
                    throw new FaultException(string.Format("{0} cannot access the application {1}, it does not have acces to GUAM", name, header));
                }
                IQueryable<Application> app_auth =
                    from app in Database.Query<Application>()
                    join acc in Database.Query<ApplicationAccount>() on app.ApplicationID equals acc.Application.ApplicationID
                    where acc.Account.AccountID == account.AccountID
                    select app;
                if ((account.Admin ? false : !app_auth.Any<Application>()))
                {
                    throw uae;
                }
                GlobalUsersService instance = instanceContext.GetServiceInstance() as GlobalUsersService;
                if (instance != null)
                {
                    instance.CurrentApplication = (
                        from a in Database.Query<Application>()
                        where a.ApplicationName.ToLower() == header.ToLower()
                        select a).FirstOrDefault<Application>();
                    if (instance.CurrentApplication == null)
                    {
                        throw new Exception(string.Concat("Application '", header, "' not found in database."));
                    }
                }
            }
            finally
            {
                if (Database != null)
                {
                    Database.Dispose();
                }
            }
            return null;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
        }
    }
}