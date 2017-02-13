using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using PubEnt.DAL;
using PubEnt.BLL;
using Aspensys.GlobalUsers.WebServiceClient;
using Aspensys.GlobalUsers.WebServiceClient.UserService;

namespace PubEnt
{
    public partial class login : System.Web.UI.Page
    {
        public string GuamErrorMsg = "There is an issue with GUAM System.";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblGuamMsg.Visible = false;

                //***EAC Store in a session var the page that called login.aspx (REFERER)
                if (Request.QueryString["redir"] != null && Request.QueryString["redir"].ToString().Length > 0)
                    Session["NCIPL_REGISTERREFERRER"] = Request.QueryString["redir"].ToString();
                else
                    Session["NCIPL_REGISTERREFERRER"] = Request.UrlReferrer != null ? Request.UrlReferrer.ToString() : "";
            }
            //Display the master page tabs 
            GlobalUtils.Utils UtilMethod = new GlobalUtils.Utils();
            if (Session["NCIPL_Pubs"] != null)
                Master.LiteralText = UtilMethod.GetTabHtmlMarkUp(Session["NCIPL_Qtys"].ToString(), "");
            else
                Master.LiteralText = UtilMethod.GetTabHtmlMarkUp("", "");
            UtilMethod = null;
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUserName.Text;
            string password = txtPassword.Text;
            
            if (username != "" && password != "")
            {
                try
                {

                    ClientUtils client = new ClientUtils();
                    //new UserServiceClient().Using(client =>
                    //{
                    //bool bIsUserValid = (bool)client.ValidateUser(username, password).ReturnValue;
                    bool bIsUserValid = client.ValidateUser(username, password);

                    if (bIsUserValid)
                    {
                        Session["NCIPL_User"] = txtUserName.Text;

                        //Get User Role
                        //var user_roles = client.GetRolesForUser(txtUserName.Text.ToString()).ReturnValue as string[];
                        var user_roles = client.GetRolesForUser(username);
                        if (user_roles != null && user_roles.Count() != 0)
                        {
                            Session["NCIPL_Role"] = user_roles[0];
                            //Session["NCIPL_Role"] = "NCIPL_CC"; //JPJ hard coded role for now
                        }

                        //yma add this maximum password age 60days rule
                        /*
                        if ((bool)client.GetMustChangePasswordFlag(username).ReturnValue)
                        {
                            Response.Redirect("changepwd.aspx");
                        }
                        else if ((bool)client.IsPasswordExpired(username).ReturnValue)
                        {
                            Response.Redirect("changepwd.aspx");
                        }
                         */

                        if (!PubEnt.GlobalUtils.Utils.ValidatePassword(password)) //yma change here due to new password rule demand
                        {
                            Response.Redirect("changepwd.aspx");
                        }
                        RedirectPreviousPage();

                    }
                    else
                    {
                        //do not give auth ticket
                        //ReturnObject ro = client.GetValidationFailureReason(username);
                        int ro = client.GetValidationFailureReason(username, password);

                        //yma add this to display customized msg
                        if (ro == 106)
                        {
                            lblGuamMsg.Text = "This account is disabled. Please email testuser1@pubs.cancer.gov for help.";
                        }
                        else
                        {
                            //display failure code on login screen
                            lblGuamMsg.Text = "Incorrect username and/or password. Please try again.";
                        }
                        lblGuamMsg.Visible = true;
                    }
                    //});
                }
                catch
                {
                    lblGuamMsg.Text = GuamErrorMsg;
                    lblGuamMsg.Visible = true;
                }
            }
            else
            {
                lblGuamMsg.Text = "Please enter User Name and Password";
                lblGuamMsg.Visible = true;
            }
        }

        protected void RedirectPreviousPage()
        {
            if (Session["NCIPL_REGISTERREFERRER"] != null)
            {
                if (Session["NCIPL_REGISTERREFERRER"].ToString().Contains("changepwd.aspx") 
                    || Session["NCIPL_REGISTERREFERRER"].ToString().Contains("forgotpwd.aspx")
                    || Session["NCIPL_REGISTERREFERRER"].ToString().Contains("register.aspx")
                    || Session["NCIPL_REGISTERREFERRER"].ToString().Contains("edit_registerinfo.aspx"))
                {
                    Response.Redirect("home.aspx",false);
                }
                else
                {
                    Response.Redirect(Session["NCIPL_REGISTERREFERRER"].ToString(), false);
                }
            }
            else
            {
                Response.Redirect("home.aspx", false);
            }
            
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RedirectPreviousPage();
        }

        protected void btnForgotPassword_Click(object sender, EventArgs e)
        {
            string username = txtUserName.Text;

            Session["Username_Forgot"] = username != "" ? username : Session["Username_Forgot"];//yma add


            //string temp = "forgotpwd.aspx?Username=" + username;
            string temp = "forgotpwd.aspx"; //yma remove Username in qstr for pii
            Response.Redirect(temp);
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            string temp = "register.aspx";
            Response.Redirect(temp);
        }

        protected void txtUserName_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
