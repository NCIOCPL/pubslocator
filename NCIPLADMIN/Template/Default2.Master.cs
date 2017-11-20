using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Web.Configuration;

namespace PubEntAdmin.Template
{
    public partial class Default2 : System.Web.UI.MasterPage
    {
        PubEntAdmin.UserControl.AdminMenu globalWebMenu1;

        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterSessionTimeOutScript();
            globalWebMenu1 =
                    (PubEntAdmin.UserControl.AdminMenu)this.LoadControl("~/UserControl/AdminMenu.ascx");
            globalWebMenu1.ID = "globalMenu1";
            this.topContent.Controls.Add(globalWebMenu1);
        }

        protected void ScriptManager_Default2Master_AsyncPostBackError(object sender, AsyncPostBackErrorEventArgs e)
        {
            //Set the error message that is sent to the client
            if (e.Exception.Data["ExtraInfo"] != null)
            {
                ScriptManager_Default2Master.AsyncPostBackErrorMessage =
                    e.Exception.Message +
                    e.Exception.Data["ExtraInfo"].ToString();
            }
            else
            {
                ScriptManager_Default2Master.AsyncPostBackErrorMessage =
                    "An unspecified error occurred.";
            }
        }
        protected virtual void RegisterSessionTimeOutScript()
        {
            var authenticationSection = WebConfigurationManager.GetSection("system.web/authentication") as AuthenticationSection;
            var timeout = authenticationSection.Forms.Timeout;
            string path = Request.Url.AbsolutePath.Remove(0, Request.ApplicationPath.Length).ToLower();
            string timeoutScript = @"
            <script language='javascript'>               
                var hdlTimeoutAlert;
                SetTimeout(); 
                function SetTimeout(){
                    var sPath = window.location.pathname;
                    var sPage = sPath.substring(sPath.lastIndexOf('/') + 1);
                    if(sPage == 'login.aspx') return;
                    ClearTimeout();                 
                    hdlTimeoutAlert = self.setTimeout('timeoutAlert()'," + timeout.TotalMilliseconds + @");
                }
                function ClearTimeout(){              
                    clearTimeout(hdlTimeoutAlert);
                }
                function timeoutAlert(){
                    if(confirm(""To maintain the security of your personal information, we have cleared your current session.\r\n Please click 'Ok' to start a new session."")) {} 
                    document.execCommand('ClearAuthenticationCache');    
                    window.location.href = window.location.href;
                }
            </script>";
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "SessionTimeout", timeoutScript, false);
        }
    }
}
