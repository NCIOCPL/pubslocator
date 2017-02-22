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
            //Reset Session for using existing CUSTID
            Session["SEARCHORDER_CUSTID"] = null;       //destroy

            if (!IsPostBack)
            {
                lblGuamMsg.Visible = false;

                #region CommentedFor_HITT12013
                ////***EAC Store in a session var the page that called login.aspx (REFERER)
                //if (Request.QueryString["redir"] != null && Request.QueryString["redir"].ToString().Length > 0)
                //    Session["NCIPL_REGISTERREFERRER"] = Request.QueryString["redir"].ToString();
                //else
                //{
                //    //NCIPL_CC Session["NCIPL_REGISTERREFERRER"] = Request.UrlReferrer != null ? Request.UrlReferrer.ToString() : "";

                //    //NCIPL_CC Firefox and IE is behaving differently for the Referrer, in FF UrlReferrer is default.aspx, in IE it is null when the application is loaded
                //    //NCIPL_CC Session["NCIPL_REGISTERREFERRER"] = Request.UrlReferrer != null ? Request.UrlReferrer.ToString() : "home.aspx";
                //    Session["NCIPL_REGISTERREFERRER"] = (Request.UrlReferrer != null && !(Request.UrlReferrer.ToString().Contains("default.aspx"))) ? Request.UrlReferrer.ToString() : "home.aspx";
                //    if (Request.QueryString["js"] != null)
                //        if (string.Compare(Request.QueryString["js"].ToString(), "2") == 0)
                //            Session["JSTurnedOn"] = "False";
                //}
                #endregion

                if (Request.QueryString["js"] != null)
                    if (string.Compare(Request.QueryString["js"].ToString(), "2") == 0)
                        Session["JSTurnedOn"] = "False";

                //Show user a message if the rol for the log on did not work properly
                if (Request.QueryString["msg"] != null && lblGuamMsg.Visible == false)
                {
                    if (string.Compare(Request.QueryString["msg"].ToString(), "invaliduser", 0) == 0)
                    {
                        lblGuamMsg.Text = "Invalid User or Role.";
                        lblGuamMsg.Visible = true;
                    }
                }

                //this.Master.TopRightPanelVisible = false;
                
                //this.Master.LogoImage1Visible = false;
                //this.Master.LogoImage2Visible = true;

                //Display the master page tabs 
                GlobalUtils.Utils UtilMethod = new GlobalUtils.Utils();
                if (Session["NCIPL_Pubs"] != null)
                    Master.LiteralText = UtilMethod.GetTabHtmlMarkUp(Session["NCIPL_Qtys"].ToString(), "");
                else
                    Master.LiteralText = UtilMethod.GetTabHtmlMarkUp("", "");
                UtilMethod = null;

            }
           

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUserName.Text;
            string password = txtPassword.Text;

            if (username != "" && password != "")
            {
                try
                {
                    /*
                    new UserServiceClient().Using(client =>
                    {*/
                    ClientUtils client = new ClientUtils();
                    bool bIsUserValid = client.ValidateUser(username, password);
                        if (bIsUserValid)
                        {
                            Session["NCIPL_User"] = txtUserName.Text;

                            //Get User Role
                            var user_roles = client.GetRolesForUser(txtUserName.Text.ToString());
                            if (user_roles != null && user_roles.Count() != 0)
                            {
                                //Session["NCIPL_Role"] = user_roles[0];
                                //Session["NCIPL_Role"] = "NCIPL_CC"; //JPJ hard coded role for now
                                foreach(string role in user_roles)
                                {
                                    if(role.ToUpper() == "NCIPL_CC")
                                    {
                                        Session["NCIPL_Role"] = role;
                                        break;
                                    }
                                }
                            }

                            if (GlobalUtils.UserRoles.getLoggedInUserId().Length == 0 || GlobalUtils.UserRoles.getLoggedInUserRole() < 1)
                            {
                                lblGuamMsg.Text = "Invalid User or Role.";
                                lblGuamMsg.Visible = true;
                            }

                            //yma add this maximum password age 60days rule
                            if (client.GetMustChangePasswordFlag(username) == true)
                            {
                                Response.Redirect("changepwd.aspx");
                            }
                            else if (client.IsPasswordExpired(username))
                            {
                                Response.Redirect("changepwd.aspx");
                            }

                            if (!PubEnt.GlobalUtils.Utils.ValidatePassword(password)) //yma change here due to new password rule demand
                            {
                                Response.Redirect("changepwd.aspx");
                            }
                            else
                            {
                                RedirectPreviousPage();
                            }
                            
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
            //if (Session["NCIPL_REGISTERREFERRER"] != null)
            //{
                //Response.Redirect(Session["NCIPL_REGISTERREFERRER"].ToString(),true);
            //}
            //else
            //{
                //Response.Redirect("login.aspx");
            //}

            Response.Redirect("home.aspx");

        }

        //protected void btnForgotPassword_Click(object sender, EventArgs e)
        //{
        //    string username = txtUserName.Text;
        //    string temp = "forgotpwd.aspx?Username=" + username;
        //    Response.Redirect(temp);
        //}

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
