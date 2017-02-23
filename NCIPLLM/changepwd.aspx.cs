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
            if (!IsPostBack)
            {

                //NCIPL_LM
                if (GlobalUtils.UserRoles.getLoggedInUserId().Length == 0 || GlobalUtils.UserRoles.getLoggedInUserRole() < 1)
                {
                    string currASPXfilename = System.IO.Path.GetFileName(Request.Path).ToString();
                    Session["NCIPL_REGISTERREFERRER"] = currASPXfilename;
                    Response.Redirect("~/login.aspx?msg=invaliduser&redir=" + currASPXfilename, true);
                }
                
                divChangePwdConfirmation.Visible = false;
                lblGuamMsg.Visible = false;

                if (Session["NCIPL_User"] != null)
                {
                    if (Session["NCIPL_User"].ToString() != "")
                    {
                        lblUser.Text = Session["NCIPL_User"].ToString();
                    }
                }

                //**** Retrieve Security Questions List
                //new UserServiceClient().Using(client =>
                //{
                ClientUtils client = new ClientUtils();
                int n = 0;
                // var application_info = client.GetApplicationInformation();
                // ddlQuestions.DataSource = application_info.Questions;
                ddlQuestions.DataValueField = "QuestionID";
                ddlQuestions.DataTextField = "QuestionText";
                //ddlQuestions.DataBind();
                ddlQuestions.Items.Insert(0, new ListItem("[Select from the list]", ""));
                Dictionary<String, String> test = client.GetAppInfoQuestions();
                foreach (KeyValuePair<string, string> q in client.GetAppInfoQuestions())
                {
                    ++n;
                    ddlQuestions.Items.Insert(n, new ListItem(q.Value, q.Key));
                }

                    //Get questions and answers
                    string strSecQuestionID = "";
                    string strSecQuestion = "";
                    string strSecAnswer = "";
                    if (Session["NCIPL_User"] != null)
                    {
                        if (Session["NCIPL_User"].ToString() != "")
                        {
                            KeyValuePair<string,string> questions = client.GetUserQuestions(Session["NCIPL_User"].ToString());
                            string answer = client.GetUserAnswers(Session["NCIPL_User"].ToString());
                            if (!string.IsNullOrEmpty(questions.ToString()) && !string.IsNullOrEmpty(answer))
                            {
                                strSecQuestionID = questions.Key;
                                strSecQuestion = questions.Value;
                                strSecAnswer = answer;
                            }

                            if (strSecQuestion != "")
                            {
                                for (int i = 0; i < ddlQuestions.Items.Count; i++)
                                {
                                    if (ddlQuestions.Items[i].Text.ToString() != "")
                                    {
                                        if (strSecQuestion == ddlQuestions.Items[i].Text.ToString())
                                        {
                                            ddlQuestions.Items[i].Selected = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                //}); // end using

                //***EAC Store in a session var the page that called login.aspx (REFERER)
                /*if (Request.QueryString["redir"] != null && Request.QueryString["redir"].ToString().Length > 0)
                    Session["NCIPL_REGISTERREFERRER"] = Request.QueryString["redir"].ToString();
                else
                {
                    //NCIPL_LM Session["NCIPL_REGISTERREFERRER"] = Request.UrlReferrer != null ? Request.UrlReferrer.ToString() : "";
                    Session["NCIPL_REGISTERREFERRER"] = Request.UrlReferrer != null ? Request.UrlReferrer.ToString() : "home.aspx";
                    if (Request.QueryString["js"] != null)
                        if (string.Compare(Request.QueryString["js"].ToString(), "2") == 0)
                            Session["JSTurnedOn"] = "False";
                }*/

                //Display the master page tabs 
                GlobalUtils.Utils UtilMethod = new GlobalUtils.Utils();
                if (Session["NCIPL_Pubs"] != null)
                    Master.LiteralText = UtilMethod.GetTabHtmlMarkUp(Session["NCIPL_Qtys"].ToString(), "");
                else
                    Master.LiteralText = UtilMethod.GetTabHtmlMarkUp("", "");
                UtilMethod = null;
            }

            string sPreviousPage = "home.aspx";

            if (Session["NCIPL_REGISTERREFERRER"] != null)
            {
                if (Session["NCIPL_REGISTERREFERRER"].ToString() != "")
                {
                    sPreviousPage = Session["NCIPL_REGISTERREFERRER"].ToString();
                }
            }
            btnCancel.Attributes.Add("onclick", "window.location='" + sPreviousPage + "'; return(false);");

            //if (!GlobalUtils.Utils.isLoggedin())
            //{
            //    this.Master.LogoImage1Visible = false;
            //    this.Master.LogoImage2Visible = true;
            //}
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string password = txtPassword.Text.Trim();
            string strSecQuestion = ddlQuestions.SelectedItem.Text;
            string strSecAnswer = txtAnswer.Text.Trim();
            
            if (password != "")
            {
                if (strSecQuestion != "" && strSecAnswer != "")
                {
                    try
                    {
                        ClientUtils client = new ClientUtils();
                        //new UserServiceClient().Using(client =>
                        //{

                        /*                   {
                       KeyValuePair<string,string> questions = client.GetUserQuestions(Session["NCIPL_User"].ToString());
                       string answer = client.GetUserAnswers(Session["NCIPL_User"].ToString());
                       if (!string.IsNullOrEmpty(questions.ToString()) && !string.IsNullOrEmpty(answer))
                       {
                           strSecQuestionID = questions.Key;
                           strSecQuestion = questions.Value;
                           strSecAnswer = answer;
                       }*/

                        // ReturnObject ro;

                        bool bIsUserValid = client.ValidateUser(Session["NCIPL_User"].ToString(), password);
                        if (bIsUserValid)
                        {
                            //Set questions and answer
                            //*** Set questions and answer
                            KeyValuePair<string, string> questions_answer = new KeyValuePair<string, string>(strSecQuestion, strSecAnswer);
                            client.SetUserQuestionsAndAnswers(Session["NCIPL_User"].ToString(), questions_answer);

                            /* Old set logic
                            UserQuestion[] questions_answer = new UserQuestion[1];
                            questions_answer[0] = new UserQuestion();
                            questions_answer[0].QuestionText = strSecQuestion;
                            questions_answer[0].Answer = strSecAnswer;
                            ro = client.SetUserQuestionsAndAnswers(Session["NCIPL_User"].ToString(), questions_answer);
                            */

                            divChangePwd.Visible = false;
                            lblGuamMsg.Visible = false;

                            lblConfirmation.Text = "Your account has been updated successfully.";
                            divChangePwdConfirmation.Visible = true;
                        }
                        else
                        {
                            //do not give auth ticket
                            //do not give auth ticket
                            //ReturnObject ro = client.GetValidationFailureReason(username);
                            int ro = client.GetValidationFailureReason(Session["NCIPL_User"].ToString(), password);

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
                    lblGuamMsg.Text = "Security Question and Answer are required fields.";
                    lblGuamMsg.Visible = true;
                }
            }
            else
            {
                lblGuamMsg.Text = "Please enter Current Password in order to Update Account Information";
                lblGuamMsg.Visible = true;
            }
        }

        protected void txtAnswer_TextChanged(object sender, EventArgs e)
        {

        }

        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            string password = txtPassword.Text.Trim();
            string newpassword = txtNewPassword.Text.Trim();
            string confirmpassword = txtConfirmPassword.Text.Trim();

            if (password != "")
            {
                if (newpassword != "" && confirmpassword != "")
                {
                    if (txtNewPassword.Text != txtConfirmPassword.Text)
                    {
                        lblGuamMsg.Text = "New Password and Confirm Password do not match.";
                        lblGuamMsg.Visible = true;
                    }
                    else if (!PubEnt.GlobalUtils.Utils.ValidatePassword(newpassword)) //yma change the rule
                    {
                        lblGuamMsg.Text = "New Password does not meet complexity requirement.";
                        lblGuamMsg.Visible = true;
                    }
                    else
                    {
                        try
                        {
                            string Username = Session["NCIPL_User"].ToString();
                            /*new UserServiceClient().Using(client =>
                            {*/
                            ClientUtils client = new ClientUtils();
                            int returnCode = client.ChangePassword(Username, txtPassword.Text, txtNewPassword.Text);
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

                                lblConfirmation.Text = "Your account password has been changed successfully.";
                                divChangePwdConfirmation.Visible = true;
                            }
                            //});
                        }
                        catch
                        {
                            if (GuamErrorMsg == "Password exists too recently in password history.")
                            {
                                lblGuamMsg.Text = GuamErrorMsg + " None of the last ten(10) passwords may be reused." ;
                            }
                            else 
                            {
                                lblGuamMsg.Text = GuamErrorMsg;
                            }
                            lblGuamMsg.Visible = true;
                        }
                    }
                }
                else
                {
                    lblGuamMsg.Text = "New Password and Confirm New Password are required fields.";
                    lblGuamMsg.Visible = true;
                }
            }
            else
            {
                lblGuamMsg.Text = "Please enter Current Password in order to Change Account Password.";
                lblGuamMsg.Visible = true;
            }
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect(Session["NCIPL_REGISTERREFERRER"].ToString(), true);
        }
    }
}
