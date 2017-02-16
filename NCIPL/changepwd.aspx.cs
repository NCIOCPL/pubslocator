using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using PubEnt.DAL;
using PubEnt.BLL;

namespace PubEnt
{
    public partial class changepwd : System.Web.UI.Page
    {
        public string GuamErrorMsg = "There is an issue with GUAM System.";

        protected void Page_Load(object sender, EventArgs e)
        {
            string sPreviousPage = "home.aspx";
            string sForgotPwdPage = "forgotpwd.aspx";

            if (Session["NCIPL_REGISTERREFERRER"] != null)
            {
                if (Session["NCIPL_REGISTERREFERRER"].ToString() != "")
                {
                    sPreviousPage = Session["NCIPL_REGISTERREFERRER"].ToString();
                }
            }
            btnCancel.Attributes.Add("onclick", "window.location='" + sPreviousPage + "'; return(false);");

            btnForgotPassword.Attributes.Add("onclick", "window.location='" + sForgotPwdPage + "'; return(false);");

            if (!IsPostBack)
            {
                divChangePwdConfirmation.Visible = false;
                lblGuamMsg.Visible = false;

                if (Session["NCIPL_User"] != null)
                {
                    if (Session["NCIPL_User"].ToString() != "")
                    {
                        lblUser.Text = Session["NCIPL_User"].ToString();
                    }
                }
            }
            //Display the master page tabs 
            GlobalUtils.Utils UtilMethod = new GlobalUtils.Utils();
            if (Session["NCIPL_Pubs"] != null)
                Master.LiteralText = UtilMethod.GetTabHtmlMarkUp(Session["NCIPL_Qtys"].ToString(), "");
            else
                Master.LiteralText = UtilMethod.GetTabHtmlMarkUp("", "");
            UtilMethod = null;
        }

        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            if (txtNewPassword.Text != txtConfirmPassword.Text)
            {
                lblGuamMsg.Text = "New Password and Confirm Password do not match.";
                lblGuamMsg.Visible = true;
            }
            else if (!PubEnt.GlobalUtils.Utils.ValidatePassword(txtNewPassword.Text)) //yma change the rule
            {
                lblGuamMsg.Text = "New Password does not meet complexity requirement.";
                lblGuamMsg.Visible = true;
            }
            else
            {

                try
                {
                    string Username = Session["NCIPL_User"].ToString();

                    ClientUtils client = new ClientUtils();
                    /*new UserServiceClient().Using(client =>
                    {*/
                        int returnCode = client.ChangePassword(Username, txtCurrentPassword.Text, txtNewPassword.Text);
                        if (returnCode == 1)
                        {
                            lblGuamMsg.Text = "Old password cannot match new password.";
                            lblGuamMsg.Visible = true;
                        }
                        else if (returnCode == 2)
                        {
                            lblGuamMsg.Text = "Invalid password. Please retry.";
                            lblGuamMsg.Visible = true;
                        }
                        else if (returnCode == 3)
                        {
                            lblGuamMsg.Text = "Error creating new password. Please retry.";
                            lblGuamMsg.Visible = true;
                        }
                        else
                        {
                            divChangePwd.Visible = false;
                            lblGuamMsg.Visible = false;
                            divChangePwdConfirmation.Visible = true;
                        }
                    //});
                }
                catch
                {
                    lblGuamMsg.Text = GuamErrorMsg;
                    lblGuamMsg.Visible = true;
                }
            }
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect(Session["NCIPL_REGISTERREFERRER"].ToString());
        }
    }
}
