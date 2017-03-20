using System;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace ManagementApplication
{
	public class MvcApplication : HttpApplication
	{
		public MvcApplication()
		{
		}

		protected void Application_AuthenticateRequest(object sender, EventArgs e)
		{
			HttpCookie authCookie = base.Request.Cookies[FormsAuthentication.FormsCookieName];
			if (authCookie != null)
			{
				FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
				GenericIdentity userIdentity = new GenericIdentity(ticket.Name);
				GenericPrincipal userPrincipal = new GenericPrincipal(userIdentity, new string[0]);
				base.Context.User = userPrincipal;
			}
		}

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			ModelMetadataProviders.Current = new MetadataProvider();
			MvcApplication.RegisterRoutes(RouteTable.Routes);
		}

		public static void RegisterRoutes(RouteCollection routes)
		{
			RouteCollectionExtensions.IgnoreRoute(routes, "{resource}.axd/{*pathInfo}");
			RouteCollectionExtensions.MapRoute(routes, "Default", "{controller}/{action}/{id}", new { controller = "LogOn", action = "Index", id = UrlParameter.Optional });
		}
	}
}