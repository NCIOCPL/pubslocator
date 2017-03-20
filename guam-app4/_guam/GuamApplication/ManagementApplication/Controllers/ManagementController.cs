using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ManagementApplication.Controllers
{
    public class ManagementController : Controller
    {
        public ManagementController()
        {
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            string item = filterContext.RouteData.Values["controller"] as string;
            string str = filterContext.RouteData.Values["action"] as string;
            string item1 = filterContext.RouteData.Values["id"] as string;
            if ((!(item == "Users") || !(str == "Index") ? !(item == "Application") : false))
            {
                base.ViewData["ApplicationName"] = filterContext.RequestContext.HttpContext.Request.QueryString["application"];
            }
            else
            {
                base.ViewData["ApplicationName"] = item1;
            }
            base.HttpContext.Response.CacheControl = "no-cache";
            base.HttpContext.Response.AddHeader("Pragma", "no-cache");
            base.HttpContext.Response.Expires = -1;
        }
    }
}