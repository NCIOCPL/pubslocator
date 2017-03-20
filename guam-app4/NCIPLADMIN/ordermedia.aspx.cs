using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//added
using PubEntAdmin.DAL;
using PubEntAdmin.BLL;
using System.Data.Common;
namespace PubEntAdmin
{
    public partial class ordermedia : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                this.BindGrid();

            this.Title = "Order Source";
            this.PageTitle = "Order Source";
        }
        private void BindGrid()
        {
            this.grdVwOrderMedia.DataSource = DAL.PE_DAL.GetAllOrderMedia(); //TO CHANGE
            this.grdVwOrderMedia.DataBind();
        }

        protected void grdVwOrderMedia_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grdVwOrderMedia.EditIndex = e.NewEditIndex;
            this.BindGrid();
        }

        protected void grdVwOrderMedia_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string OrderMediaId = ""; int Active = 0;
            if (e.CommandName == "MakeActive")
            {
                OrderMediaId = e.CommandArgument.ToString();
                Active = 1;
            }
            else if (e.CommandName == "MakeInactive")
            {
                OrderMediaId = e.CommandArgument.ToString();
                Active = 0;
            }
            //Carry out Active/Inactive db action here
            if (string.Compare(e.CommandName, "MakeActive", false) == 0 || string.Compare(e.CommandName, "MakeInactive", false) == 0)
            {
                int Id = Int32.Parse(OrderMediaId);
                PE_DAL.SetOrderMedia(Id, "ACTION", "", Active, -1, -1, -1); 
            }


            if (e.CommandName == "Update")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = grdVwOrderMedia.Rows[index];
                //    HiddenField hidCustId = (HiddenField)row.FindControl("hidCustId");

                //GridViewRow row = grdVwTypeofCustomer.Rows[e.RowIndex];
                HiddenField hidOrderMediaId = (HiddenField)row.FindControl("hidOrderMediaId");
                TextBox txtOrderMedia = (TextBox)row.FindControl("txtOrderMedia");
                int Id = Int32.Parse(hidOrderMediaId.Value);
                CheckBox chkCC = (CheckBox)row.FindControl("chkCC");
                CheckBox chkPOS = (CheckBox)row.FindControl("chkPOS");
                CheckBox chkLM = (CheckBox)row.FindControl("chkLM");
                PE_DAL.SetOrderMedia(Id, "U", txtOrderMedia.Text.Trim(), -1,(chkCC.Checked ? 1 : 0), (chkPOS.Checked ? 1 : 0), (chkLM.Checked ? 1 : 0));

            }

            //this.BindGrid();
            this.grdVwOrderMedia.EditIndex = -1;
            this.BindGrid();
        }

        protected void grdVwOrderMedia_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //DbDataRecord dbRecord = (DbDataRecord)e.Row.DataItem;
                MultiSelectListBoxItem lItem = (MultiSelectListBoxItem)e.Row.DataItem;

                //Status Label
                Label lblStatus = (Label)e.Row.FindControl("lblStatus");

                //Button column
                Control btnControl = e.Row.Cells[3].Controls[0];
                Button btn = btnControl as Button;
                CheckBox chkCC = (CheckBox)e.Row.FindControl("chkCC");
                CheckBox chkPOS = (CheckBox)e.Row.FindControl("chkPOS");
                CheckBox chkLM = (CheckBox)e.Row.FindControl("chkLM");
                chkCC.Checked = (PE_DAL.GetMediaStatusByRole(lItem.ID, "CC") > 0 ? true : false);
                chkPOS.Checked = (PE_DAL.GetMediaStatusByRole(lItem.ID, "POS") > 0 ? true : false);
                chkLM.Checked = (PE_DAL.GetMediaStatusByRole(lItem.ID, "LM") > 0 ? true : false);


                //Edit State
                if ((e.Row.RowState & DataControlRowState.Edit) > 0) //Edit
                {
                    HiddenField hidOrderMediaId = (HiddenField)e.Row.FindControl("hidOrderMediaId");
                    TextBox txtOrderMedia = (TextBox)e.Row.FindControl("txtOrderMedia");

                    hidOrderMediaId.Value = lItem.ID.ToString();
                    txtOrderMedia.Text = lItem.Name;
                    btn.CommandArgument = hidOrderMediaId.Value;
                    if (lItem.Enabled == true)
                    {
                        lblStatus.Text = "Active";
                        btn.Text = "Inactivate";
                        btn.CommandName = "MakeInactive";
                    }
                    else
                    {
                        lblStatus.Text = "Inactive";
                        btn.Text = "Activate";
                        btn.CommandName = "MakeActive";
                    }
                    #region 3 new checkboxes for the roles
                    chkCC.Enabled = true;
                    chkPOS.Enabled = true;
                    chkLM.Enabled = true;
                    #endregion
                }
                else //Normal or Alternate
                {
                    //Find the controls
                    HiddenField hidOrderMediaId = (HiddenField)e.Row.FindControl("hidOrderMediaId");
                    Label lblOrderMedia = (Label)e.Row.FindControl("lblOrderMedia");

                    //assing values to template controls
                    hidOrderMediaId.Value = lItem.ID.ToString();
                    lblOrderMedia.Text = lItem.Name;
                    btn.CommandArgument = hidOrderMediaId.Value;
                    if (lItem.Enabled == true)
                    {
                        lblStatus.Text = "Active";
                        btn.Text = "Inactivate";
                        btn.CommandName = "MakeInactive";
                    }
                    else
                    {
                        lblStatus.Text = "Inactive";
                        btn.Text = "Activate";
                        btn.CommandName = "MakeActive";
                    }
                    chkCC.Enabled = false;
                    chkPOS.Enabled = false;
                    chkLM.Enabled = false;
                }

            }
        }

        protected void grdVwOrderMedia_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            //GridViewRow row = grdVwTypeofCustomer.Rows[e.RowIndex];
            //HiddenField hidCustId = (HiddenField)row.FindControl("hidCustId");
            //TextBox txtCustType = (TextBox)row.FindControl("txtCustType");
            //int Id = Int32.Parse(hidCustId.Value);
            //PE_DAL.SetTypeofCustomer(Id, "U", txtCustType.Text.Trim(), -1);
        }

        protected void grdVwOrderMedia_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            this.grdVwOrderMedia.EditIndex = -1;
            this.BindGrid();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtOrderMedia.Text.Trim().Length > 0)
            {
                PE_DAL.SetOrderMedia(-1, "A", txtOrderMedia.Text.Trim(), 1,1,1,1);
                this.BindGrid();
                txtOrderMedia.Text = "";
            }
        }

        #region Properties
        public string PageTitle
        {
            set
            {
                ((Label)this.Master.FindControl("lblPageTitle")).Text = value;
            }
            get
            {
                return ((Label)this.Master.FindControl("lblPageTitle")).Text;
            }
        }
        #endregion
    }
}
