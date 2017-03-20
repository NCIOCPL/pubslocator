using Aspensys.GlobalUsers.WebServiceClient;
using Aspensys.GlobalUsers.WebServiceClient.UserService;
using System;
using System.Runtime.CompilerServices;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace ManagementApplication
{
	public class Login : System.Web.UI.Page
	{
		protected HtmlHead Head1;

		protected HtmlForm Form1;

		protected GuamControl GuamControl;

		protected Literal VersionStatement;

		public Login()
		{
		}

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			if (string.IsNullOrEmpty(this.VersionStatement.Text))
			{
				UsingExtension.Using<UserServiceClient>(new UserServiceClient(), (UserServiceClient client) => {
					VersionInformation vi = (VersionInformation)client.GetVersionInformation().ReturnValue;
					this.VersionStatement.Text = string.Concat(new object[] { "Database Schema Version: ", vi.DatabaseSchemaVersion, " | Expected GUAM Version: ", vi.DatabaseExpectedWebServicesVersion, " | Detected GUAM Version: ", vi.ActualWebServicesVersion});
				});
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!base.IsPostBack)
			{
				UsingExtension.Using<UserServiceClient>(new UserServiceClient(), (UserServiceClient client) => {
					ReturnObject ro = client.AssignPassword("administrator", "123456Ab");
					ro = client.EnableUser("administrator");
				});
			}
		}
	}
}