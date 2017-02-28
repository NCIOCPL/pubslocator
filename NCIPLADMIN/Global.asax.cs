using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging;
using PubEntAdmin.DAL;
using PubEntAdmin.BLL;

namespace PubEntAdmin
{
    public class Global : System.Web.HttpApplication
    {
        protected const string strPubEntAdminAppId = "PubEntAdmin_Application_Id";

        protected void Application_Start(object sender, EventArgs e)
        {
        }

        protected void Session_Start(object sender, EventArgs e)
        {
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        protected void Application_EndRequest(Object sender, EventArgs e)
        {
            if (Response.StatusCode == 404)
            {
                Response.Redirect("~/404.aspx");
            }
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            /// We may need to completely redo authentication - commenting out for now.
            /*
            string userInformation = String.Empty;
            if (Request.IsAuthenticated)
            {
                string usr = User.Identity.Name.ToString();
                usr = usr.Substring(usr.LastIndexOf("\\") + 1);
                CISUser colUsers = CISUser.GetCisUser(usr, System.Convert.ToInt32(ConfigurationSettings.AppSettings[strPubEntAdminAppId]));
                //Begin CR-11-001-36
                if (colUsers == null)
                    Server.Transfer("~/UnauthorizedAccess.aspx", true);
                //End CR-36
                userInformation = colUsers.ID + ";" + colUsers.Role + ";" + colUsers.Login + ";" + colUsers.Name + ";" +
                    colUsers.Email + ";" + colUsers.RegionNo;
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                    1,                              // version
                    Context.User.Identity.Name,             // user name
                    DateTime.Now,                   // issue time
                    DateTime.Now.AddHours(1),       // expires every hour
                    false,                          // don't persist cookie
                    userInformation
                    );

                // Send the cookie to the client
                // Response.Cookies["rpd_cookie"].Value = FormsAuthentication.Encrypt(ticket);
                // Response.Cookies["rpd_cookie"].Path = "/";
                // Response.Cookies["rpd_cookie"].Expires = DateTime.Now.AddMinutes(1);

                Context.User = new CustomPrincipal(User.Identity, colUsers.ID,
                    colUsers.Role, colUsers.Login, colUsers.Name, colUsers.Email,
                    colUsers.RegionNo, colUsers.LastName);
                //Response.Write("<h2>" + colUsers[0].Role + "</h2>");

                if (!User.IsInRole(PubEntAdminManager.AdminRole) &&
                    !User.IsInRole(PubEntAdminManager.DWHStaffRole) &&
                    !User.IsInRole(PubEntAdminManager.RURole))
                {
                    Server.Transfer("~/UnauthorizedAccess.aspx", true);
                }

            }
            else
            {
                Server.Transfer("~/UnauthorizedAccess.aspx", true);
            }
            */
        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception Ex = Server.GetLastError().GetBaseException();
            ExceptionPolicy.HandleException(Ex, "Exception Policy");
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}