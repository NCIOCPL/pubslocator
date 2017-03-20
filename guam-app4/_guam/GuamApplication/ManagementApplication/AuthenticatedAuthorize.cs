using System;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace ManagementApplication
{
	public class AuthenticatedAuthorize : AuthorizeAttribute
	{
		public AuthenticatedAuthorize()
		{
		}

		public override void OnAuthorization(AuthorizationContext filterContext)
		{
			base.OnAuthorization(filterContext);
			if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
			{
				filterContext.Result = new HttpUnauthorizedResult();
			}
		}
	}
}