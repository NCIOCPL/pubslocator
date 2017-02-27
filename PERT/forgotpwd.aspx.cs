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
    public partial class forgotpwd : System.Web.UI.Page
    {
        private String securityQuestion = "";
        public String SecurityQuestion
        {
            get { return securityQuestion; }
            set { securityQuestion = value; }
        }

        private String securityQuestionID = "";
        public String SecurityQuestionID
        {
            get { return securityQuestionID; }
            set { securityQuestionID = value; }
        }

        public string GuamErrorMsg = "There is an issue with GUAM System.";
        public string UserNotFoundErrorMsg = "User Name does not exist.  Please verify and re-enter correct User Name";

        protected void Page_Load(object sender, EventArgs e)
        {
            string Username = "";
            string sPreviousPage = "login.aspx";

            divConfirmation.Visible = false;
            lblGuamMsg.Visible = false;

            /*if (Session["NCIPL_REGISTERREFERRER"] != null)
            {
                if (Session["NCIPL_REGISTERREFERRER"].ToString() != "")
                {
                    sPreviousPage = Session["NCIPL_REGISTERREFERRER"].ToString();
                }
            }*/
            btnCancel.Attributes.Add("onclick", "window.location='" + sPreviousPage + "'; return(false);");

            if (!IsPostBack)
            {
                
                //NCIPL_CC
                //if (GlobalUtils.UserRoles.getLoggedInUserId().Length == 0 || GlobalUtils.UserRoles.getLoggedInUserRole() < 1)
                //{
                    //string currASPXfilename = System.IO.Path.GetFileName(Request.Path).ToString();
                    //Session["NCIPL_REGISTERREFERRER"] = currASPXfilename;
                    //Response.Redirect("~/login.aspx?msg=invaliduser&redir=" + currASPXfilename, true);
                //}

                //--- Get data from query string 
                if (Request.QueryString["Username"] != null)
                {
                    Username = Request.QueryString["Username"].ToString();
                }

                if (Username.Trim() != "")
                {
                    getSecurityQuestion(Username);
                    if (SecurityQuestion != "")
                    {
                        lblUser.Text = Username;
                        lblSecurityQuestion.Text = SecurityQuestion;
                        HidSecurityQuestionID.Value = SecurityQuestionID;
                        divUserName.Visible = false;
                        divSecurityQuestion.Visible = true;
                    }
                    else
                    {
                        HidSecurityQuestionID.Value = "";
                        divSecurityQuestion.Visible = false;
                        divUserName.Visible = true;

                        txtUserName.Text = Username;
                        lblGuamMsg.Text = UserNotFoundErrorMsg;
                        lblGuamMsg.Visible = true;
                        divChangePwd.Visible = true;
                    }
                }
                else
                {
                    HidSecurityQuestionID.Value = "";
                    divSecurityQuestion.Visible = false;
                    divUserName.Visible = true;
                }

                //***EAC Store in a session var the page that called login.aspx (REFERER)
                //if (Request.QueryString["redir"] != null && Request.QueryString["redir"].ToString().Length > 0)
                    //Session["NCIPL_REGISTERREFERRER"] = Request.QueryString["redir"].ToString();
                //else
                    //Session["NCIPL_REGISTERREFERRER"] = Request.UrlReferrer != null ? Request.UrlReferrer.ToString() : "";

                //Display the master page tabs 
                GlobalUtils.Utils UtilMethod = new GlobalUtils.Utils();
                if (Session["NCIPL_Pubs"] != null)
                    Master.LiteralText = UtilMethod.GetTabHtmlMarkUp(Session["NCIPL_Qtys"].ToString(), "");
                else
                    Master.LiteralText = UtilMethod.GetTabHtmlMarkUp("", "");
                UtilMethod = null;

            }

            //this.Master.TopRightPanelVisible = false;
            this.Master.LogoImage1Visible = false;
            this.Master.LogoImage2Visible = true;
        }

        protected void txtUserName_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txtAnswer_TextChanged(object sender, EventArgs e)
        {

        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            //Response.Redirect(Session["NCIPL_REGISTERREFERRER"].ToString());
            Response.Redirect("login.aspx");
        }

        protected void btnResetPassword_Click(object sender, EventArgs e)
        {
            if (divUserName.Visible)
            {
                getSecurityQuestion(txtUserName.Text);
                if (SecurityQuestion != "")
                {
                    lblUser.Text = txtUserName.Text;
                    lblSecurityQuestion.Text = SecurityQuestion;
                    HidSecurityQuestionID.Value = SecurityQuestionID;
                    divUserName.Visible = false;
                    divSecurityQuestion.Visible = true;

                }
                else
                {
                    HidSecurityQuestionID.Value = "";
                    divSecurityQuestion.Visible = false;
                    divUserName.Visible = true;

                    lblGuamMsg.Text = UserNotFoundErrorMsg;
                    lblGuamMsg.Visible = true;
                    divChangePwd.Visible = true;
                }
            }

            if (divSecurityQuestion.Visible)
            {
                if (lblUser.Text != "" && HidSecurityQuestionID.Value != "" && txtAnswer.Text != "")
                {
                    try
                    {
                        //new UserServiceClient().Using(client =>
                        //{
                        //ReturnObject ro;

                        //Reset Password
                        /* TODO: figure out if we need the UserQuestion object 
                        UserQuestion[] questions_answer = new UserQuestion[1];
                        questions_answer[0] = new UserQuestion();
                        questions_answer[0].UserQuestionID = Convert.ToInt32(HidSecurityQuestionID.Value);
                        questions_answer[0].Answer = txtAnswer.Text.ToString();
                        ro = client.ResetPassword(lblUser.Text.ToString(), questions_answer);
                        */
                        ClientUtils client = new ClientUtils();
                        int userQuestionID = Convert.ToInt32(HidSecurityQuestionID.Value);
                        string userAnswer = txtAnswer.Text.ToString();
                        int returnCode = client.ResetPassword(lblUser.Text.ToString(), userAnswer, userQuestionID);

                        if (returnCode == 0)
                        {
                            divChangePwd.Visible = false;
                            divConfirmation.Visible = true;
                        }
                        else
                        {
                            lblGuamMsg.Text = "Security question answer is incorrect. Please retry.";
                            lblGuamMsg.Visible = true;
                            divChangePwd.Visible = true;
                            divConfirmation.Visible = false;
                        }
                        //});
                    }
                    catch
                    {
                        lblGuamMsg.Text = GuamErrorMsg;
                        lblGuamMsg.Visible = true;
                        divChangePwd.Visible = true;
                        divConfirmation.Visible = false;
                    }
                }
            }
        }

        protected void getSecurityQuestion(string sUser)
        {
            try
            {
                /*
                new UserServiceClient().Using(client =>
                {
                    //Get questions and answers
                    var questions = client.GetUserQuestions(sUser).ReturnValue as UserQuestion[];
                    if (questions != null && questions.Count() != 0)
                    {
                        SecurityQuestion = questions[0].QuestionText;
                        SecurityQuestionID = questions[0].UserQuestionID.ToString();
                    }
                });
                */
                //Get questions and answers
                ClientUtils client = new ClientUtils();
                KeyValuePair<string, string> question = client.GetUserQuestions(sUser);
                if (!string.IsNullOrEmpty(question.Value))
                {
                    SecurityQuestion = question.Value;
                }
                if (!string.IsNullOrEmpty(question.Key))
                {
                    SecurityQuestionID = question.Key;
                }
            }
            catch
            {

            }
        }
    }
}
