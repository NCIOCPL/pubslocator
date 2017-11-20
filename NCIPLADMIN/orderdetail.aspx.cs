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
    public partial class orderdetail : System.Web.UI.Page
    {
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
                btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this order?');");
                btnDelete.Enabled = false;
            }
        }



        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string returnmsg;
            Button btn = (Button)sender;

            Order.MarkOrderBad(Int32.Parse(btn.CommandArgument), Context.User.Identity.Name, txtReason.Text, out returnmsg);
            lblMessage.Text = returnmsg;
            if (returnmsg.Contains("Success"))
            {
                btnDelete.Enabled = txtReason.Enabled = false;
            }
            else
            {
                btnDelete.Enabled = txtReason.Enabled = true;
            }

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            int _orderid = Int32.Parse(txtOrderNum.Text);
            ProductCollection col;

            Order o = PubEntAdmin.BLL.Order.GetOrderByOrderID(_orderid);
            o = PubEntAdmin.BLL.Order.GetOrderByOrderID(_orderid);

            if (o != null)
            {
                lblSearchMsg.Text = "";
                legend1.InnerText = "Order " + _orderid;
                btnDelete.CommandArgument = _orderid.ToString();

                //***EAC lets bind the detail grid first
                col = PE_DAL.GetOrderDetails(_orderid);
                bGrid.DataSource = col;
                bGrid.DataBind();

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

                if (o.TermCode == "")
                {
                    btnDelete.Enabled = txtReason.Enabled = true;
                    lblMessage.Text = "";
                }
                else
                {
                    btnDelete.Enabled = txtReason.Enabled = false;
                    lblMessage.Text = "This order is not in PENDING status.";
                }

            }
            else
            {
                //not found or other problem
                lblSearchMsg.Text = "Cannot find order number " + _orderid.ToString();

                bOrderID.Text = "";
                bCreated.Text = "";
                bName.Text = "";
                bOrg.Text = "";
                bAddr1.Text = "";
                bAddr2.Text = "";
                bZip.Text = "";
                bCity.Text = "";
                bState.Text = "";
                bPhone.Text = "";
                bEmail.Text = "";
                bShipmethod.Text = "";
                bComment.Text = "";

                btnDelete.Enabled = false;
            }
        }


    }
}
