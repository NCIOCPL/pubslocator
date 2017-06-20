using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using PubEntAdmin.BLL;
using PubEntAdmin.DAL;
using System.Web.SessionState;

namespace PubEntAdmin
{
    public partial class holdorderreport : System.Web.UI.Page
    {
        private string _startdt
        {
            get
            {
                object o = ViewState["_startdt"];
                if (o == null)
                {
                    return String.Empty;
                }
                return (o.ToString());
            }

            set
            {
                ViewState["_startdt"] = value;
            }
        }
        private string _enddt
        {
            get
            {
                object o = ViewState["_enddt"];
                if (o == null)
                {
                    return String.Empty;
                }
                return (o.ToString());
            }

            set
            {
                ViewState["_enddt"] = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session.IsNewSession)
            {
                Response.Redirect("~/Home.aspx");
            }
            System.Web.UI.UserControl userControl = (System.Web.UI.UserControl)this.LoadControl("~/UserControl/AdminMenu.ascx");
            this.plcHldMenu.Controls.Add(userControl);
            this.PageTitle = "Held Order Report";
            if (!IsPostBack)
            {
                _startdt  = _enddt = "";
                if (!((CustomPrincipal)Context.User).IsInRole(PubEntAdminManager.AdminRole))
                {
                    PubEntAdminManager.UnathorizedAccess();
                }

            }
        }
        void Bindgrid(string s)
        {
            //grdUsers.DataSource = PubEntAdmin.BLL.CISUser.GetGuamUsers(s);
            //grdUsers.DataBind();
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            int del;
            int rel;
            int tot;
            _startdt = txtStartDt.Text;
            _enddt = txtEndDt.Text;

            if (_startdt == "") _startdt = "1/1/1980";
            if (_enddt == "") _enddt = "1/1/2080";
            tot = PE_DAL.GetHoldOrderCount(_startdt, _enddt);
            lblHeld.Text = tot.ToString();
            del = PE_DAL.GetHoldDeletedCount(_startdt, _enddt);
            lblDeleted.Text = del.ToString();
            rel = PE_DAL.GetHoldReleasedCount(_startdt, _enddt);
            lblReleased.Text = rel.ToString();
            lblDeletedPercent.Text = (tot > 0 ? 100.0 * del / tot : 0).ToString(".#") + "%";

            lblPubsNotDist.Text = PE_DAL.GetHoldDeletedPubCount(_startdt, _enddt).ToString();
        }


        public string PageTitle
        {
            set
            {
                ((Label)this.FindControl("lblPageTitle")).Text = value;
            }
            get
            {
                return ((Label)this.FindControl("lblPageTitle")).Text;
            }
        }
    }
}
