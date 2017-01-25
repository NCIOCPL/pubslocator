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
    public partial class orderrepeat : System.Web.UI.Page
    {
        private string _txtfind
        {
            get
            {
                object o = ViewState["_txtfind"];
                if (o == null)
                {
                    return String.Empty;
                }
                return ((string)o).ToUpper();
            }

            set
            {
                ViewState["_txtfind"] = value;
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

            if (!IsPostBack)
            {
                _txtfind = "";
                if (!((CustomPrincipal)Context.User).IsInRole(PubEntAdminManager.AdminRole))
                {
                    PubEntAdminManager.UnathorizedAccess();
                }
                Bindgrid("");

            }
        }
        void Bindgrid(string s)
        {
            grdUsers.DataSource = PubEntAdmin.BLL.OrderCollection.GetRepeatOrders(s);
            grdUsers.DataBind();
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            _txtfind = txtFind.Text;
            Bindgrid(_txtfind);
        }

        protected void PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdUsers.PageIndex = e.NewPageIndex;
            Bindgrid(_txtfind);
        }

        protected void RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow row = e.Row;
            if (row.DataItem == null)       //**EAC make sure its a datarow
                return;

            Order o = (Order)row.DataItem;
            LinkButton lnkEdit = (LinkButton)row.FindControl("lnkDetails");

            lnkEdit.PostBackUrl = "~/holddetails.aspx?orderid=" + o.OrderId;
        }

    }
}
