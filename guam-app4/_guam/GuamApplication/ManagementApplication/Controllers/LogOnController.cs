using Aspensys.GlobalUsers.WebServiceClient;
using Aspensys.GlobalUsers.WebServiceClient.UserService;
using System;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ManagementApplication.Controllers
{
    public class LogOnController : Controller
    {
        public LogOnController()
        {
        }

        public ActionResult ChangePassword()
        {
            return base.View();
        }

        [HttpPost]
        public ActionResult ChangePassword(string currentPassword, string newPassword)
        {
            ActionResult action;
            bool flag = false;
            UsingExtension.Using<UserServiceClient>(new UserServiceClient(), (UserServiceClient client) =>
            {
                ReturnObject returnObject = client.ChangePassword(base.User.Identity.Name, currentPassword, newPassword);
                if (!(returnObject is ErrorReturnObject))
                {
                    flag = true;
                }
                else
                {
                    base.ModelState.AddModelError("newPassword", returnObject.DefaultErrorMessage);//returnObject.DefaultErrorMessage());
                }
            });
            if (!flag)
            {
                action = base.View();
            }
            else
            {
                action = this.RedirectToAction("Index", "Application", null);
            }
            return action;
        }

        public ActionResult Index()
        {
            return this.Redirect("~/default.aspx");
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            base.Response.Redirect("~/LogOn");
            return null;
        }
    }
}