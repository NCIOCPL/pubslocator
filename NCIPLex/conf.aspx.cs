using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//added
using NCIPLex.BLL;
using NCIPLex.DAL;
using System.Configuration;

namespace NCIPLex
{
    public partial class conf : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            /*Begin JavaScript check*/
            if (Request.QueryString["js"] != null)
            {
                if (string.Compare(Request.QueryString["js"].ToString(), "2", true) == 0)
                {
                    Response.Write("This web site requires JavaScript to function properly. Please turn on JavaScript in your browser.");
                    Response.End();
                }
                else
                {
                    Session["JSTurnedOn"] = "True";
                }
            }
            else
            {
                //Assume JavaScript is on, since the user came in directly to this page
                Session["JSTurnedOn"] = "True";
            }
            /*End JavaScript check*/

            if (!IsPostBack)
            {
                //string idletimeout = SQLDataAccess.GetTimeout(1, 2).ToString();
                //labelPageTimeOut.Text = idletimeout;
                ////idletimeout = SQLDataAccess.GetTimeout(1, 2).ToString() + "00";
                //string idle2timeout = SQLDataAccess.GetTimeout(1, 3).ToString();
                //labelSessionTimeOut.Text = idle2timeout;
                this.BindDropdown();
            }
        }

        protected void btnContinue_Click(object sender, EventArgs e)
        {
            //Begin - checks
            if (string.Compare(ddlConfName.SelectedValue, "0", true) == 0)
            {
                lblConfAvailability.Visible = true;
                return;
            }
            if (ddlConfName.SelectedValue.Length > 4) //Possibly Hailstorm
            {
                lblConfAvailability.Text = "Invalid Conference.";
                lblConfAvailability.Visible = true;
                return;
            }
            //End checks

            Session["NCIPLEX_ConfId"] = ddlConfName.SelectedValue;
            Session["NCIPLEX_MaxIntlLimit"] = SQLDataAccess.GetIntl_MaxOrder(Int32.Parse(ddlConfName.SelectedValue));
            Session["NCIPLEX_MaxDomLimit"] = ConfigurationManager.AppSettings["DomesticOrderLimit"];
            
            Response.Redirect("location.aspx", true);
        }

        protected void BindDropdown()
        {
            this.ddlConfName.DataSource = Conf.GetAllConf();
            this.ddlConfName.DataTextField = "ConfName";
            this.ddlConfName.DataValueField = "ConfID";
            this.ddlConfName.DataBind();
        }
    }
}
