using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aspensys.GlobalUsers.WebServiceClient;
using Aspensys.GlobalUsers.WebServiceClient.UserService;

public partial class GuamControl : System.Web.UI.UserControl
{
    public event EventHandler PasswordSuccessfullyChanged;
    public event EventHandler QuestionsSuccessfullyChanged;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        if (!IsPostBack)
        {
            ShowRegion(phLogin);
        }
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        lblFailureMessage.Text = "";
        if (CurrentMode == Mode.CHANGE_PASSWORD)
        {
            ShowRegion(phChangePassword);
        }
        else if (CurrentMode == Mode.SET_QUESTIONS)
        {
            ShowRegion(phSetQuestions);
        }
    }
    
    public bool AllowPersistentLogin
    {
        get
        {
            return liPersistent.Visible;
        }
        set
        {
            liPersistent.Visible = value;
        }
    }

    public Mode CurrentMode
    {
        get
        {
            if (ViewState["CurrentMode"] == null)
                ViewState["CurrentMode"] = Mode.LOGIN;
            return (Mode)ViewState["CurrentMode"];
        }
        set
        {
            ViewState["CurrentMode"] = value;
        }
    }

    #region Login
    public void btnLogin_Click(object sender, EventArgs e)
    {
        string username = txtUsername.Text;
        string password = txtPassword.Text;

       new UserServiceClient().Using(client =>
       {
           bool bIsUserValid = (bool)client.ValidateUser(username, password).ReturnValue;
           if (bIsUserValid)
           {
               Session["NCIPL_User"] = username;
               ReturnObject ro = client.GetRolesForUser(username);
               try
               {
                   Session["NCIPL_Role"] = ((string[])ro.ReturnValue)[0]; 
               }
               catch (Exception)
               {
               }


               ShowNextRegion(txtUsername.Text);
           }
           else
           {
               //do not give auth ticket
               ReturnObject ro = client.GetValidationFailureReason(username);
               //display failure code on login screen
               lblFailureMessage.Text = ro.DefaultErrorMessage;
           }
       });
    }

    public void lbForgot_Click(object sender, EventArgs e)
    {
        ShowRegion(phForgotPassword);
    }
    public void lnkRegister_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/register.aspx",true);
    }
    #endregion

    #region Expiration Warning
    public void ChangePasswordNow(object sender, CommandEventArgs cea)
    {
        if (cea.CommandArgument.ToString() == "1")
        {
            ShowRegion(phChangePassword);
        }
        else
        {
            ShowNextRegion(Username);
        }
    }
    #endregion

    #region Change Password
    public void btnChangePassword_Click(object sender, EventArgs e)
    {
       new UserServiceClient().Using(client =>
       {
           ReturnObject ro = client.ChangePassword(Username, txtCurrentPassword.Text, txtNewPassword.Text);
           if (ro.ReturnCode != 0)
           {
               lblFailureMessage.Text = ro.DefaultErrorMessage;
           }
           else
           {
               if (PasswordSuccessfullyChanged != null)
                   PasswordSuccessfullyChanged(this, EventArgs.Empty);
               ShowNextRegion(Username);
           }
       });
    }
    #endregion

    #region Forgot Password
    public void btnResetPassword_Click(object sender, EventArgs e)
    {
        var questions_answer = repAnswers.Items.OfType<RepeaterItem>().Select
        (
            ri => new UserQuestion()
            {
                UserQuestionID = Convert.ToInt32(((HiddenField)ri.FindControl("hidUserQuestionID")).Value),
                Answer = ((TextBox)ri.FindControl("txtQuestionAnswer")).Text
            }
        ).ToArray();

        new UserServiceClient().Using(client =>
        {
            ReturnObject ro = client.ResetPassword(Username, questions_answer);
            if (ro is ErrorReturnObject)
            {
                lblFailureMessage.Text = ro.DefaultErrorMessage;
            }
            else
            {
                phResetSuccessful.Visible = true;
                phQuestions.Visible = false;
            }
        });
    }

    public void btnSetUsername_Click(object sender, EventArgs e)
    {
        Username = txtUsernameForgot.Text;
        ShowRegion(phForgotPassword);
    }

    public void lbReturnLogin_Click(object sender, EventArgs e)
    {
        phResetSuccessful.Visible = false;
        phQuestions.Visible = true;
        ShowRegion(phLogin);
    }

    #endregion

    #region Set Questions
    public void btnSetQuestions_Click(object sender, EventArgs e)
    {

        var questions_answer = repQuestions.Items.OfType<RepeaterItem>().Select
        (
            ri => new UserQuestion()
            {
                QuestionText = ((DropDownList)ri.FindControl("ddlQuestions")).SelectedItem.Text,
                Answer = ((TextBox)ri.FindControl("txtQuestionAnswer")).Text
            }
        ).ToArray();
        new UserServiceClient().Using(client =>
        {
            ReturnObject ro = client.SetUserQuestionsAndAnswers(Username, questions_answer);
            if (ro.ReturnCode != 0)
            {
                lblFailureMessage.Text = ro.DefaultErrorMessage;
            }
            else
            {
                if (QuestionsSuccessfullyChanged != null)
                    QuestionsSuccessfullyChanged(this, EventArgs.Empty);
                ShowNextRegion(Username);
            }
        });

        ShowNextRegion(Username);
    }

    #endregion

    public string Username
    {
        get
        {
            return CurrentMode == Mode.LOGIN ? txtUsername.Text : String.IsNullOrEmpty(ViewState["Username"] as string) ? Page.User.Identity.Name : ViewState["Username"] as string;
        }
        set
        {
            if (CurrentMode == Mode.LOGIN)
            {
                txtUsername.Text = value;
            }
            else
            {
                ViewState["Username"] = value;
            }
        }
    }

    private void ShowNextRegion(string username)
    {
        if (CurrentMode == Mode.LOGIN)
        {
            new UserServiceClient().Using(client =>
            {
                ApplicationInformation info = client.GetApplicationInformation();
                ReturnObject ro = client.GetUserQuestions(username);
                var questions = (UserQuestion[])ro.ReturnValue;
                if ((bool)client.GetMustChangePasswordFlag(username).ReturnValue)
                {
                    //must change password now before getting access to the site
                    //show MustChangePassword screen
                    //if change successful, give auth ticket
                    ShowRegion(phChangePassword);
                }
                //you don't have to do any explicit checks to see if the current application uses
                //the password expiration feature. if that's the case, IsPasswordExpired and
                //ShouldShowPasswordExpirationWarning will always return false
                else if ((bool)client.IsPasswordExpired(username).ReturnValue)
                {
                    //must change password now before getting access to the site
                    //show password expired screen
                    //if change successful, give auth ticket
                    ShowRegion(phChangePassword);
                }
                else if ((bool)client.ShouldShowPasswordExpirationWarning(username).ReturnValue)
                {
                    //show warning screen, give opportunity to change password immediately
                    SetAuthCookie(username);
                    ShowRegion(phExpirationWarning);
                }
                else if (info.NumberOfPasswordResetQuestions > 0 && questions.Count() < info.NumberOfPasswordResetQuestions)
                {
                    //The user needs to provide password reset questions
                    SetAuthCookie(username);
                    ShowRegion(phSetQuestions);
                }
                else
                {
                    //All clear, send them in
                    SetAuthCookie(username);
                    if (Session["NCIPL_LOGINREFERRER"] != null && Session["NCIPL_LOGINREFERRER"].ToString().Length >0)
                        Response.Redirect(Session["NCIPL_LOGINREFERRER"].ToString());
                    else 
                        FormsAuthentication.RedirectFromLoginPage(username, chkPersistent.Checked);
                }
            });
        }
    }

    protected void ShowRegion(PlaceHolder region)
    {
        Controls.OfType<PlaceHolder>().ToList().ForEach(ph => ph.Visible = region == ph);
        
        if (region == phSetQuestions)
        {
            new UserServiceClient().Using(client =>
            {
                var application_info = client.GetApplicationInformation();
                var questions = new int[application_info.NumberOfPasswordResetQuestions];
                for (int x = 0; x < questions.Length; x++)
                    questions[x] = x + 1;
                repQuestions.DataSource = questions;
                Questions = application_info.Questions;
                QuestionCount = application_info.NumberOfPasswordResetQuestions;
            });
        }
        else if (region == phForgotPassword && !String.IsNullOrEmpty(Username))
        {
            new UserServiceClient().Using(client =>
            {
                var questions = client.GetUserQuestions(Username).ReturnValue as UserQuestion[];
                phNoQuestions.Visible = questions == null || questions.Count() == 0;
                phQuestions.Visible = !phNoQuestions.Visible;
                repAnswers.DataSource = questions;
            });
        }
        region.DataBind();
    }

    protected ApplicationQuestion[] Questions
    {
        get
        {
            if (ViewState["Questions"] == null)
            {
                new UserServiceClient().Using(client =>
                {
                    var application_info = client.GetApplicationInformation();
                    ViewState["Questions"] = application_info.Questions;
                });
            }
            return ViewState["Questions"] as ApplicationQuestion[];
        }
        set
        {
            ViewState["Questions"] = value;
        }
    }

    protected int QuestionCount
    {
        get
        {
            if (ViewState["QuestionCount"] == null)
            { 
                new UserServiceClient().Using(client =>
                {
                    var application_info = client.GetApplicationInformation();
                    ViewState["QuestionCount"] = application_info.NumberOfPasswordResetQuestions;
                });
            }
            return Convert.ToInt32(ViewState["QuestionCount"]);
        }
        set
        {
            ViewState["QuestionCount"] = value;
        }
    }

    protected void SetAuthCookie(string username)
    {
        FormsAuthentication.SetAuthCookie(username, chkPersistent.Checked);
    }

    public enum Mode : int
    {
        LOGIN = 0, CHANGE_PASSWORD = 1, SET_QUESTIONS = 2
    }
}