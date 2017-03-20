using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PubEnt
{
    public partial class pubmaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {


        }
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            LinkButton3.Visible = false;
            LinkButton1.Visible = false;
            LinkButton2.Visible = false;
            if (GlobalUtils.Utils.isLoggedin())
            {
                LinkButton3.Visible = true;
                LinkButton2.Visible = true;
                LinkButton3.Text = GlobalUtils.Utils.LoggedinUser();
            }
            else
            {
                LinkButton1.Visible = true;
            }

            //if (Request.ServerVariables["SCRIPT_NAME"].Contains("login.aspx")
            //    || Request.ServerVariables["SCRIPT_NAME"].Contains("forgotpwd.aspx")
            //    || Request.ServerVariables["SCRIPT_NAME"].Contains("edit_register.aspx")
            //    || Request.ServerVariables["SCRIPT_NAME"].Contains("changepwd.aspx")
            //    || Request.ServerVariables["SCRIPT_NAME"].Contains("register.aspx")
            //    )
            //    dvtopright.Visible = false;

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

        protected void LinkButton1_Click(object sender, EventArgs e)
        {

        }

        protected void LinkButton2_Click(object sender, EventArgs e)
        {            
            PubEnt.GlobalUtils.Utils.LogoutCurrentUser();
            Response.Redirect("~/default.aspx",true);
        }

        protected void LinkButton3_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/edit_registerinfo.aspx", true);
        }
    }
}
