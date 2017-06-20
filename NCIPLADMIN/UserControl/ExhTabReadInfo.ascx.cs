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
using System.Drawing;

using PubEntAdmin.DAL;
using PubEntAdmin.BLL;

using GlobalUtils;

namespace PubEntAdmin.UserControl
{
    public partial class ExhTabReadInfo : System.Web.UI.UserControl
    {
        #region Fields
        private int intPubID;
        #endregion

        #region Events Handling
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.PubID >0)
                this.BindData();

            PubEntAdminManager.AssignMonitorTabChangeValuesOnPageLoadInReadMode(this.Page);
        }

        protected void dgInfoView_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (String.Compare(((DataRowView)e.Item.DataItem).Row[0].ToString(), "Display Status:", true) == 0)
                {
                    DataSet iSet = PE_DAL.GetExhDisplayStatusView(this.PubID);

                    if (iSet.Tables[0].Rows.Count > 0)
                    {
                        Label l_displayStatus = new Label();

                        foreach (DataRow s in iSet.Tables[0].Rows)
                        {
                            if (l_displayStatus.Text.Length > 0)
                                l_displayStatus.Text += ", ";
                            l_displayStatus.Text += s.ItemArray[1].ToString();
                        }
                        l_displayStatus.Text.Trim();
                        ((DataRowView)e.Item.DataItem).Row[0] = "";
                        e.Item.Cells[1].Controls.Clear();
                        e.Item.Cells[1].Controls.Add(l_displayStatus);
                    }
                    else
                    {
                        Label errLbl = new Label();
                        errLbl.Text = " - ";

                        e.Item.Cells[1].Controls.Clear();
                        e.Item.Cells[1].Controls.Add(errLbl);
                    }
                }
                else if (String.Compare(((DataRowView)e.Item.DataItem).Row[0].ToString(), "Conference:", true) == 0)
                {
                    DataGrid dgRegReimb2 = new DataGrid();
                    dgRegReimb2.ID = "innerDgRegReimb";

                    //Format the DataGrid to look cool.
                    dgRegReimb2.BorderWidth = (Unit)0;
                    dgRegReimb2.CellPadding = 4;
                    dgRegReimb2.CellSpacing = 0;
                    dgRegReimb2.GridLines = GridLines.None;
                    dgRegReimb2.BorderColor = Color.FromName("#E0E0E0");

                    dgRegReimb2.ItemStyle.BackColor = Color.White;
                    dgRegReimb2.AlternatingItemStyle.BackColor = Color.FromName("LightGray");

                    dgRegReimb2.ShowHeader = false;
                    dgRegReimb2.HeaderStyle.CssClass = "fieldLabel";
                    dgRegReimb2.HeaderStyle.BackColor = Color.FromName("#ffff00");
                    dgRegReimb2.AutoGenerateColumns = false;

                    //****Add a series of BoundColumns****//
                    //***Region Name***//
                    BoundColumn bc = new BoundColumn();
                    //Set the BoundColumn Values
                    bc.DataField = "CONFERENCENAME";
                    //bc.HeaderText = "Region(s)";
                    bc.ItemStyle.Wrap = false;
                    bc.ItemStyle.CssClass = "fieldLabel";
                    dgRegReimb2.Columns.Add(bc);

                    //****End BoundColumns****//
                    DataSet iSet = PE_DAL.GetExhConfView(this.PubID);

                    if (iSet != null)
                    {
                        dgRegReimb2.DataSource = iSet;
                        dgRegReimb2.DataBind();
                        ((DataRowView)e.Item.DataItem).Row[0] = "";
                        e.Item.Cells[1].Controls.Clear();
                        e.Item.Cells[1].Controls.Add(dgRegReimb2);
                    }
                    else
                    {
                        Label errLbl = new Label();
                        errLbl.Text = " - ";

                        e.Item.Cells[1].Controls.Clear();
                        e.Item.Cells[1].Controls.Add(errLbl);
                    }
                }

                else if (String.Compare(((DataRowView)e.Item.DataItem).Row[0].ToString(), "Rotate:", true) == 0)
                {
                    DataGrid dgRegReimb2 = new DataGrid();
                    dgRegReimb2.ID = "innerDgRegReimb";

                    //Format the DataGrid to look cool.
                    dgRegReimb2.BorderWidth = (Unit)0;
                    dgRegReimb2.CellPadding = 4;
                    dgRegReimb2.CellSpacing = 0;
                    dgRegReimb2.GridLines = GridLines.None;
                    dgRegReimb2.BorderColor = Color.FromName("#E0E0E0");

                    dgRegReimb2.ItemStyle.BackColor = Color.White;
                    dgRegReimb2.AlternatingItemStyle.BackColor = Color.FromName("LightGray");

                    dgRegReimb2.ShowHeader = false;
                    dgRegReimb2.HeaderStyle.CssClass = "fieldLabel";
                    dgRegReimb2.HeaderStyle.BackColor = Color.FromName("#ffff00");
                    dgRegReimb2.AutoGenerateColumns = false;

                    //****Add a series of BoundColumns****//
                    //***Region Name***//
                    BoundColumn bc = new BoundColumn();
                    //Set the BoundColumn Values
                    bc.DataField = "CONFERENCENAME";
                    //bc.HeaderText = "Region(s)";
                    bc.ItemStyle.Wrap = false;
                    bc.ItemStyle.CssClass = "fieldLabel";
                    dgRegReimb2.Columns.Add(bc);

                    //****End BoundColumns****//
                    DataSet iSet = PE_DAL.GetExhConfRotateView(this.PubID);

                    if (iSet != null)
                    {
                        dgRegReimb2.DataSource = iSet;
                        dgRegReimb2.DataBind();
                        ((DataRowView)e.Item.DataItem).Row[0] = "";
                        e.Item.Cells[1].Controls.Clear();
                        e.Item.Cells[1].Controls.Add(dgRegReimb2);
                    }
                    else
                    {
                        Label errLbl = new Label();
                        errLbl.Text = " - ";

                        e.Item.Cells[1].Controls.Clear();
                        e.Item.Cells[1].Controls.Add(errLbl);
                    }
                }
                
            }
        } 
        #endregion

        #region Methods
        protected void BindData()
        {
            DataSet ds = PE_DAL.GetExhInterfaceView(this.PubID);
            this.dgInfoView.DataSource = DataSetRoutines.FlipDataSet(ds);
            this.dgInfoView.DataBind();
        }

        protected bool Save()
        {
            return true;
        }
        #endregion

        #region Properties
        public int PubID
        {
            set
            {
                this.intPubID = value;
            }
            get
            {
                return this.intPubID;
            }
        }
        #endregion
    }
}