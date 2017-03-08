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
    public partial class donotmail : System.Web.UI.Page
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
            grdUsers.DataSource = PubEntAdmin.BLL.Person.GetDNMList("");
            grdUsers.DataBind();
        }



        protected void PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdUsers.PageIndex = e.NewPageIndex;
            Bindgrid(_txtfind);
        }

        protected void RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //GridViewRow row = e.Row;
            //if (row.DataItem == null)       //**EAC make sure its a datarow
            //    return;
            
            //Person o = (Person)row.DataItem;

            //if ((e.Row.RowState & DataControlRowState.Edit) > 0) //edit
            //{
            //    TextBox lblAddress = (TextBox)row.FindControl("txtAddress");
            //    TextBox lblZip = (TextBox)row.FindControl("txtZip");
            //    TextBox lblIPAddress = (TextBox)row.FindControl("txtIPAddress");
            //    lblAddress.Text = o.Addr1;
            //    lblZip.Text = o.Zip5;
            //    lblIPAddress.Text = o.Addr2;
            //}
            //else //normal
            //{
            //    Label lblAddress = (Label)row.FindControl("lblAddress");
            //    Label lblZip = (Label)row.FindControl("lblZip");
            //    Label lblIPAddress = (Label)row.FindControl("lblIPAddress");
            //    lblAddress.Text = o.Addr1;
            //    lblZip.Text = o.Zip5;
            //    lblIPAddress.Text = o.Addr2;
            //}




            //Button btnEdit = (Button)row.FindControl("btnEdit");
            //Button btnDelete = (Button)row.FindControl("btnDelete");
            //btnEdit.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this order?');");
            //btnDelete.Attributes.Add("onclick", "return confirm('Release this order?');");

            //lnkEdit.PostBackUrl = "~/guam/edituser.aspx?userid=" + o.ID;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            lblMessage2.Text = "";

            txtAddress.Text = PubEntAdminManager.Clean(txtAddress.Text);
            txtZip.Text = PubEntAdminManager.Clean(txtZip.Text);
            txtZip.Text = txtZip.Text.ToString().PadLeft(5, '0');
            txtIPAddress.Text = PubEntAdminManager.Clean(txtIPAddress.Text);

            if (txtIPAddress.Text.Trim().Length > 0)
            {
                if (DAL.PE_DAL.AddDNMRecord("", "", txtIPAddress.Text) == false)
                {
                    lblMessage.Text = "Unable to save IP Address";
                }
                else
                {
                    lblMessage.Text = "IP Address added successfully";
                    txtIPAddress.Text = txtAddress.Text = txtZip.Text = "";
                }
            }
            else if (txtAddress.Text.Trim().Length > 0 && txtZip.Text.Trim().Length > 0)
            {
                if (DAL.PE_DAL.AddDNMRecord(txtAddress.Text, txtZip.Text, "") == false)
                {
                    lblMessage.Text = "Unable to save IP Address";

                }
                else
                {
                    lblMessage.Text = "Mailing Address added successfully";
                    txtIPAddress.Text = txtAddress.Text = txtZip.Text = "";
                }
            }
            else
            {
                lblMessage.Text = "Mailing -or- IP Address required";
            }
            Bindgrid("");
        }


        protected void grdUsers_RowEditing(object sender, GridViewEditEventArgs e)
        {
            lblMessage.Text = "";
            lblMessage2.Text = "";
            grdUsers.EditIndex = e.NewEditIndex;
            Bindgrid("");
        }

        protected void grdUsers_Deleting(object sender, GridViewDeleteEventArgs e)
        {
            lblMessage.Text = "";
            lblMessage2.Text = "";
            GridViewRow row = grdUsers.Rows[e.RowIndex];
            Label lblID = (Label)row.FindControl("lblID");
            if (DAL.PE_DAL.DeleteDNMRecord(Int32.Parse(lblID.Text)) == false)
            {
                lblMessage2.Text = "Unable to delete record";
            }
            else
            {
                lblMessage2.Text = "";
            }
            Bindgrid("");
        }

        protected void grdUsers_Cancel(object sender, GridViewCancelEditEventArgs e)
        {
            grdUsers.EditIndex = -1;
            Bindgrid("");
        }

        protected void grdUsers_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {            
            GridViewRow row = grdUsers.Rows[e.RowIndex];
            TextBox txtAddress = (TextBox)row.FindControl("txtAddress");
            TextBox txtZip = (TextBox)row.FindControl("txtZip");
            txtZip.Text = txtZip.Text.ToString().PadLeft(5, '0');
            TextBox txtIPAddress = (TextBox)row.FindControl("txtIPAddress");
            Label lblID = (Label)row.FindControl("lblID");

            if (DAL.PE_DAL.UpdateDNMRecord(Int32.Parse(lblID.Text), txtAddress.Text , txtZip.Text , txtIPAddress.Text) == false)
            {
                //lblMessage.Text = "Unable to save IP Address";
            }
            else
            {
                //lblMessage.Text = "IP Address added successfully (if you entered a mailing address, it was ignored)";

            }
            grdUsers.EditIndex = -1;
            Bindgrid("");
        }

    }
}
