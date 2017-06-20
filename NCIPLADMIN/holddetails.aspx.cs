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
    public partial class holddetails : System.Web.UI.Page
    {
        private int _orderid
        {
            get
            {
                object o = ViewState["_orderid"];
                if (o == null)
                {
                    return 0;
                }
                return (int)o;
            }

            set
            {
                ViewState["_orderid"] = value;
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
                if (Request.QueryString["orderid"] != null)
                {
                    _orderid = Int32.Parse(Request.QueryString["orderid"]);
                    legend1.InnerText = "Order " + _orderid;
                    BindPage();
                }
                btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this order?');");
                btnRelease.Attributes.Add("onclick", "return confirm('Release this order?');");

            }

        }
        protected void BindPage()
        {
            ProductCollection col;
            Order o = PubEntAdmin.BLL.Order.GetOrderByOrderID(_orderid);
            if (o != null)
            {
                //***EAC lets bind the detail grid first
                col = PE_DAL.GetOrderDetails(_orderid);
                bGrid.DataSource = col;
                bGrid.DataBind();

                if (o.TermCode == "P") HyperLink1.NavigateUrl = "~/orderprank.aspx";
                if (o.TermCode == "R") HyperLink1.NavigateUrl = "~/orderrepeat.aspx";

                bOrderID.Text = o.OrderId.ToString();
                bCreated.Text = o.DateCreated.ToString("MM/dd/yyy hh:mm");
                bName.Text = o.ShipTo.Fullname;
                bOrg.Text = o.ShipTo.Organization;
                bAddr1.Text = o.ShipTo.Addr1;
                bAddr2.Text = o.ShipTo.Addr2;
                bZip.Text = o.ShipTo.Zip5;
                bCity.Text = o.ShipTo.City;
                bState.Text = o.ShipTo.State;
                bPhone.Text = o.ShipTo.Phone;
                bEmail.Text = o.ShipTo.Email;
                bShipmethod.Text = o.ShipMethod;
                bComment.Text = o.OrderComment;

                if (o.RepeatID > 0)
                {
                    //***EAC lets bind the detail grid first
                    col = PE_DAL.GetOrderDetails(o.RepeatID);
                    aGrid.DataSource = col;
                    aGrid.DataBind();

                    o = PubEntAdmin.BLL.Order.GetOrderByOrderID(o.RepeatID);
                    aOrderID.Text = o.OrderId.ToString();
                    aCreated.Text = o.DateCreated.ToString("MM/dd/yyy hh:mm");
                    aName.Text = o.ShipTo.Fullname;
                    aOrg.Text = o.ShipTo.Organization;
                    aAddr1.Text = o.ShipTo.Addr1;
                    aAddr2.Text = o.ShipTo.Addr2;
                    aZip.Text = o.ShipTo.Zip5;
                    aCity.Text = o.ShipTo.City;
                    aState.Text = o.ShipTo.State;
                    aPhone.Text = o.ShipTo.Phone;
                    aEmail.Text = o.ShipTo.Email;
                    aShipmethod.Text = o.ShipMethod;
                    aComment.Text = o.OrderComment;
                }
                else
                {
                    aOrderID.Text = "-";
                    aCreated.Text = "-";
                    aName.Text = "-";
                    aOrg.Text = "-";
                    aAddr1.Text = "-";
                    aAddr2.Text = "-";
                    aZip.Text = "-";
                    aCity.Text = "-";
                    aState.Text = "-";
                    aPhone.Text = "-";
                    aEmail.Text = "-";
                    aShipmethod.Text = "-";
                    aComment.Text = "-";
                }
            }
        }
        protected void btnSave1_Click(object sender, EventArgs e)
        {
            //***EAC SAVE CURRENT RECORD
            if (Page.IsValid && _orderid > 0)
            {
                ////***EAC we only save ROLE for now ...
                //CISUser c = new CISUser(this._userid, "dummy", "dummy", drpRoles.SelectedValue, "dummy", "dummy", "dummy", "dummy");

                //if (PubEntAdmin.DAL.PE_DAL.SaveGuamUser(c))
                //    lblMessage.Text = "Your changes have been saved";
                //else
                //    lblMessage.Text = "Error! There was a problem saving";
            }

        }

        protected void btnSave2_Click(object sender, EventArgs e)
        {
            btnSave1_Click(sender, e);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string returnmsg;
            Order.DeleteOrder(_orderid, Context.User.Identity.Name, out returnmsg);
            lblMessage.Text = returnmsg;
            btnRelease.Enabled = btnDelete.Enabled = false;
        }

        protected void btnRelease_Click(object sender, EventArgs e)
        {
            string returnmsg;
            Order.ReleaseOrder(_orderid, Context.User.Identity.Name , out returnmsg);
            lblMessage.Text = returnmsg;
            btnRelease.Enabled = btnDelete.Enabled = false;
        }


    }
}
