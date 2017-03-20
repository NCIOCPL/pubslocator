using Aspensys.GlobalUsers.WebServiceClient;
using Aspensys.GlobalUsers.WebServiceClient.UserService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Threading;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public class GuamControl : UserControl
{
	protected Label lblFailureMessage;

	protected PlaceHolder phLogin;

	protected TextBox txtUsername;

	protected TextBox txtPassword;

	protected HtmlGenericControl liPersistent;

	protected Label Label1;

	protected CheckBox chkPersistent;

	protected LinkButton lbForgot;

	protected Button btnLogin;

	protected PlaceHolder phExpirationWarning;

	protected Label lblExpirationDays;

	protected Button btnChangePasswordNow;

	protected Button btnChangePasswordLater;

	protected PlaceHolder phChangePassword;

	protected TextBox txtCurrentPassword;

	protected TextBox txtNewPassword;

	protected TextBox txtConfirmPassword;

	protected Button btnChangePassword;

	protected PlaceHolder phSetQuestions;

	protected Repeater repQuestions;

	protected Button btnSetQuestions;

	protected PlaceHolder phForgotPassword;

	protected PlaceHolder phSetUsername;

	protected TextBox txtUsernameForgot;

	protected Button btnSetUsername;

	protected PlaceHolder phAnswerQuestions;

	protected PlaceHolder phQuestions;

	protected Repeater repAnswers;

	protected Button btnResetPassword;

	protected PlaceHolder phResetSuccessful;

	protected LinkButton lbReturnLogin;

	protected PlaceHolder phNoQuestions;

	public bool AllowPersistentLogin
	{
		get
		{
			return this.liPersistent.Visible;
		}
		set
		{
			this.liPersistent.Visible = value;
		}
	}

	public bool AutoRedirect
	{
		get
		{
			return (this.ViewState["AutoRedirect"] == null ? true : Convert.ToBoolean(this.ViewState["AutoRedirect"]));
		}
		set
		{
			this.ViewState["AutoRedirect"] = value;
		}
	}

	public GuamControl.Mode CurrentMode
	{
		get
		{
			if (this.ViewState["CurrentMode"] == null)
			{
				this.ViewState["CurrentMode"] = GuamControl.Mode.LOGIN;
			}
			return (GuamControl.Mode)this.ViewState["CurrentMode"];
		}
		set
		{
			this.ViewState["CurrentMode"] = value;
		}
	}

	protected int QuestionCount
	{
		get
		{
			if (this.ViewState["QuestionCount"] == null)
			{
				UsingExtension.Using<UserServiceClient>(new UserServiceClient(), (UserServiceClient client) => {
					ApplicationInformation application_info = client.GetApplicationInformation();
					this.ViewState["QuestionCount"] = application_info.NumberOfPasswordResetQuestions;
				});
			}
			return Convert.ToInt32(this.ViewState["QuestionCount"]);
		}
		set
		{
			this.ViewState["QuestionCount"] = value;
		}
	}

	protected ApplicationQuestion[] Questions
	{
		get
		{
			if (this.ViewState["Questions"] == null)
			{
				UsingExtension.Using<UserServiceClient>(new UserServiceClient(), (UserServiceClient client) => {
					ApplicationInformation application_info = client.GetApplicationInformation();
					this.ViewState["Questions"] = application_info.Questions;
				});
			}
			return this.ViewState["Questions"] as ApplicationQuestion[];
		}
		set
		{
			this.ViewState["Questions"] = value;
		}
	}

	public string Username
	{
		get
		{
			string text;
			if (this.CurrentMode == GuamControl.Mode.LOGIN)
			{
				text = this.txtUsername.Text;
			}
			else
			{
				text = (string.IsNullOrEmpty(this.ViewState["Username"] as string) ? this.Page.User.Identity.Name : this.ViewState["Username"] as string);
			}
			return text;
		}
		set
		{
			if (this.CurrentMode != GuamControl.Mode.LOGIN)
			{
				this.ViewState["Username"] = value;
			}
			else
			{
				this.txtUsername.Text = value;
			}
		}
	}

	public GuamControl()
	{
	}

	public void btnChangePassword_Click(object sender, EventArgs e)
	{
		UsingExtension.Using<UserServiceClient>(new UserServiceClient(), (UserServiceClient client) => {
			ReturnObject ro = client.ChangePassword(this.Username, this.txtCurrentPassword.Text, this.txtNewPassword.Text);
			if (ro.ReturnCode == 0)
			{
				if (this.PasswordSuccessfullyChanged != null)
				{
					this.PasswordSuccessfullyChanged(this, EventArgs.Empty);
				}
				this.ShowNextRegion(this.Username);
			}
			else
			{
				this.lblFailureMessage.Text = ro.DefaultErrorMessage;
			}
		});
	}

	public void btnLogin_Click(object sender, EventArgs e)
	{
		string text = this.txtUsername.Text;
		string str = this.txtPassword.Text;
		UsingExtension.Using<UserServiceClient>(new UserServiceClient(), (UserServiceClient client) => {
			if (!(bool)client.ValidateUser(text, str).ReturnValue)
			{
				ReturnObject ro = client.GetValidationFailureReason(text);
				this.lblFailureMessage.Text = ro.DefaultErrorMessage;
			}
			else
			{
				this.ShowNextRegion(this.txtUsername.Text);
			}
		});
	}

	public void btnResetPassword_Click(object sender, EventArgs e)
	{
		UserQuestion[] array = this.repAnswers.Items.OfType<RepeaterItem>().Select<RepeaterItem, UserQuestion>((RepeaterItem ri) => {
			UserQuestion userQuestion = new UserQuestion();
			userQuestion.UserQuestionID = Convert.ToInt32(((HiddenField)ri.FindControl("hidUserQuestionID")).Value);
			userQuestion.Answer = ((TextBox)ri.FindControl("txtQuestionAnswer")).Text;
			return userQuestion;
		}).ToArray<UserQuestion>();
		UsingExtension.Using<UserServiceClient>(new UserServiceClient(), (UserServiceClient client) => {
			ReturnObject ro = client.ResetPassword(this.Username, array);
			if (!(ro is ErrorReturnObject))
			{
				this.phResetSuccessful.Visible = true;
				this.phQuestions.Visible = false;
			}
			else
			{
				this.lblFailureMessage.Text = ro.DefaultErrorMessage;
			}
		});
	}

	public void btnSetQuestions_Click(object sender, EventArgs e)
	{
		UserQuestion[] array = this.repQuestions.Items.OfType<RepeaterItem>().Select<RepeaterItem, UserQuestion>((RepeaterItem ri) => {
			UserQuestion userQuestion = new UserQuestion();
			userQuestion.QuestionText = ((DropDownList)ri.FindControl("ddlQuestions")).SelectedItem.Text;
			userQuestion.Answer = ((TextBox)ri.FindControl("txtQuestionAnswer")).Text;
			return userQuestion;
		}).ToArray<UserQuestion>();
		UsingExtension.Using<UserServiceClient>(new UserServiceClient(), (UserServiceClient client) => {
			ReturnObject ro = client.SetUserQuestionsAndAnswers(this.Username, array);
			if (ro.ReturnCode == 0)
			{
				if (this.QuestionsSuccessfullyChanged != null)
				{
					this.QuestionsSuccessfullyChanged(this, EventArgs.Empty);
				}
				this.ShowNextRegion(this.Username);
			}
			else
			{
				this.lblFailureMessage.Text = ro.DefaultErrorMessage;
			}
		});
		this.ShowNextRegion(this.Username);
	}

	public void btnSetUsername_Click(object sender, EventArgs e)
	{
		this.Username = this.txtUsernameForgot.Text;
		this.ShowRegion(this.phForgotPassword);
	}

	public void ChangePasswordNow(object sender, CommandEventArgs cea)
	{
		if (!(cea.CommandArgument.ToString() == "1"))
		{
			this.ShowNextRegion(this.Username);
		}
		else
		{
			this.ShowRegion(this.phChangePassword);
		}
	}

	public void lbForgot_Click(object sender, EventArgs e)
	{
		this.ShowRegion(this.phForgotPassword);
	}

	public void lbReturnLogin_Click(object sender, EventArgs e)
	{
		this.phResetSuccessful.Visible = false;
		this.phQuestions.Visible = true;
		this.ShowRegion(this.phLogin);
	}

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		if (!base.IsPostBack)
		{
			this.ShowRegion(this.phLogin);
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		this.lblFailureMessage.Text = "";
		if (this.CurrentMode == GuamControl.Mode.CHANGE_PASSWORD)
		{
			this.ShowRegion(this.phChangePassword);
		}
		else if (this.CurrentMode == GuamControl.Mode.SET_QUESTIONS)
		{
			this.ShowRegion(this.phSetQuestions);
		}
	}

	protected void SetAuthCookie(string username)
	{
		if (this.UserSuccessfullyAuthenticates != null)
		{
			this.UserSuccessfullyAuthenticates(this, username);
		}
		FormsAuthentication.SetAuthCookie(username, this.chkPersistent.Checked);
	}

	private void ShowNextRegion(string username)
	{
		if (this.CurrentMode == GuamControl.Mode.LOGIN)
		{
			UsingExtension.Using<UserServiceClient>(new UserServiceClient(), (UserServiceClient client) => {
				ApplicationInformation info = client.GetApplicationInformation();
				UserQuestion[] questions = (UserQuestion[])client.GetUserQuestions(username).ReturnValue;
				if ((bool)client.GetMustChangePasswordFlag(username).ReturnValue)
				{
					this.ShowRegion(this.phChangePassword);
				}
				else if ((bool)client.IsPasswordExpired(username).ReturnValue)
				{
					this.ShowRegion(this.phChangePassword);
				}
				else if ((bool)client.ShouldShowPasswordExpirationWarning(username).ReturnValue)
				{
					this.SetAuthCookie(username);
					this.ShowRegion(this.phExpirationWarning);
				}
				else if ((info.NumberOfPasswordResetQuestions <= 0 ? true : questions.Count<UserQuestion>() >= info.NumberOfPasswordResetQuestions))
				{
					this.SetAuthCookie(username);
					if (this.AutoRedirect)
					{
						FormsAuthentication.RedirectFromLoginPage(username, this.chkPersistent.Checked);
					}
				}
				else
				{
					this.SetAuthCookie(username);
					this.ShowRegion(this.phSetQuestions);
				}
			});
		}
	}

	protected void ShowRegion(PlaceHolder region)
	{
		this.Controls.OfType<PlaceHolder>().ToList<PlaceHolder>().ForEach((PlaceHolder ph) => ph.Visible = region == ph);
		if (region == this.phSetQuestions)
		{
			UsingExtension.Using<UserServiceClient>(new UserServiceClient(), (UserServiceClient client) => {
				ApplicationInformation application_info = client.GetApplicationInformation();
				int[] questions = new int[application_info.NumberOfPasswordResetQuestions];
				for (int x = 0; x < (int)questions.Length; x++)
				{
					questions[x] = x + 1;
				}
				this.repQuestions.DataSource = questions;
				this.Questions = application_info.Questions;
				this.QuestionCount = application_info.NumberOfPasswordResetQuestions;
			});
		}
		else if ((region != this.phForgotPassword ? false : !string.IsNullOrEmpty(this.Username)))
		{
			UsingExtension.Using<UserServiceClient>(new UserServiceClient(), (UserServiceClient client) => {
				UserQuestion[] questions = client.GetUserQuestions(this.Username).ReturnValue as UserQuestion[];
				this.phNoQuestions.Visible = (questions == null ? true : questions.Count<UserQuestion>() == 0);
				this.phQuestions.Visible = !this.phNoQuestions.Visible;
				this.repAnswers.DataSource = questions;
			});
		}
		region.DataBind();
	}

	public event EventHandler PasswordSuccessfullyChanged;

	public event EventHandler QuestionsSuccessfullyChanged;

	public event EventHandler<string> UserSuccessfullyAuthenticates;

	public enum Mode
	{
		LOGIN,
		CHANGE_PASSWORD,
		SET_QUESTIONS
	}
}