using System;
using System.Collections.Specialized;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ManagementApplication
{
	public class ApplicationAuthorize : AuthenticatedAuthorize
	{
		protected string controller;

		protected string action;

		protected string id;

		protected string application;

		public ApplicationAuthorize()
		{
		}

		public override void OnAuthorization(AuthorizationContext filterContext)
		{
			base.OnAuthorization(filterContext);
			this.controller = filterContext.RouteData.Values["controller"] as string;
			this.action = filterContext.RouteData.Values["action"] as string;
			this.id = filterContext.RouteData.Values["id"] as string;
			this.application = ((!(this.controller == "Users") || !(this.action == "Index")) && !(this.controller == "Application") ? filterContext.RequestContext.HttpContext.Request.QueryString["application"] : this.id);
			string application_role = string.Concat(this.application, " Administrator");
			if ((filterContext.HttpContext.User.IsInRole(application_role) ? false : !filterContext.HttpContext.User.IsInRole("Full Administrator")))
			{
				filterContext.Result = new HttpUnauthorizedResult();
			}
		}
	}
}