using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Web.Configuration;
using System.Web.UI.HtmlControls;

namespace PubEnt
{
    public partial class pubmaster : System.Web.UI.MasterPage 
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterSessionTimeOutScript();
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            //Label1.Visible = false;
            LinkButton1.Visible = false;
            LinkButton2.Visible = false;
            lnkEditAcc.Visible = false;
            if (GlobalUtils.Utils.isLoggedin() & (Session["NCIPL_Role"].ToString() == "NCIPL_LM" | Session["NCIPL_Role"].ToString() == "NCIPL_PO"))
            {
                //Label1.Visible = true;
                LinkButton2.Visible = true;
                //Label1.Text = GlobalUtils.Utils.LoggedinUser();
                lnkEditAcc.Visible = true;
                lnkEditAcc.Text = GlobalUtils.Utils.LoggedinUser();
                this.LogoImage1Visible = true; //NCIPL_CC
                this.LogoImage2Visible = false; //NCIPL_CC
            }
            else
            {
                LinkButton1.Visible = true;
                this.LogoImage1Visible = false; //NCIPL_CC
                this.LogoImage2Visible = true; //NCIPL_CC
                this.lnkHome.NavigateUrl = "~/login.aspx"; //NCIPL_CC
                this.lnkHome.Text = "Login"; //NCIPL_CC
            }
            base.Render(writer);
        }
        public string LiteralText
        {
            get
            {
                return LiteralListItem.Text;
            }
            set
            {
                LiteralListItem.Text = value;
            }
        }

        public bool TopRightPanelVisible
        {
            get { return this.pnlTopRight.Visible; }
            set { this.pnlTopRight.Visible = value; }
        }

        //public bool LogoImageVisible
        //{
        //    get { return this.divLogo.Visible; }
        //    set { this.divLogo.Visible = value; }
        //}

        //public bool LoginLinkBtnVisible
        //{
        //    get { return this.LinkButton1.Visible;}
        //    set {this.LinkButton1.Visible = value;}
        //}

        public bool LogoImage1Visible
        {
            get { return this.logoImage.Visible; }
            set { this.logoImage.Visible = value; }
        }
        public bool LogoImage2Visible
        {
            get { return this.logoImage2.Visible; }
            set { this.logoImage2.Visible = value; }
        }
       
        protected void LinkButton1_Click(object sender, EventArgs e)
        {
           
        }

        protected void LinkButton2_Click(object sender, EventArgs e)
        {            
            PubEnt.GlobalUtils.Utils.LogoutCurrentUser();
            Response.Redirect("~/default.aspx",true);
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
                    if(confirm(""To maintain the security of your personal information, we have cleared your current session.\r\n Please click 'Ok' to start a new session.""))  
                        window.location.href ='login.aspx';                     
                    else
                        window.location.href ='login.aspx';
                }
            </script>";             
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "SessionTimeout", timeoutScript, false);
        }
    }
}
