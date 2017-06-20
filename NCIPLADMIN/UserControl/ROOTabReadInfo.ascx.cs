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
    public partial class ROOTabReadInfo : System.Web.UI.UserControl
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
                    DataSet iSet = PE_DAL.GetROODisplayStatusView(this.PubID);

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
                else if (String.Compare(((DataRowView)e.Item.DataItem).Row[0].ToString(), "Subject:", true) == 0)
                {
                    DataGrid dgRegReimb = new DataGrid();
                    dgRegReimb.ID = "innerDgRegReimb";

                    //Format the DataGrid to look cool.
                    dgRegReimb.BorderWidth = (Unit)0;
                    dgRegReimb.CellPadding = 4;
                    dgRegReimb.CellSpacing = 0;
                    dgRegReimb.GridLines = GridLines.None;
                    dgRegReimb.BorderColor = Color.FromName("#E0E0E0");

                    dgRegReimb.ItemStyle.BackColor = Color.White;
                    dgRegReimb.AlternatingItemStyle.BackColor = Color.FromName("LightGray");

                    dgRegReimb.ShowHeader = false;
                    dgRegReimb.HeaderStyle.CssClass = "fieldLabel";
                    dgRegReimb.HeaderStyle.BackColor = Color.FromName("#ffff00");
                    dgRegReimb.AutoGenerateColumns = false;

                    //****Add a series of BoundColumns****//
                    //***Region Name***//
                    BoundColumn bc = new BoundColumn();
                    //Set the BoundColumn Values
                    bc.DataField = "Description";
                    //bc.HeaderText = "Region(s)";
                    bc.ItemStyle.Wrap = false;
                    bc.ItemStyle.CssClass = "fieldLabel";
                    dgRegReimb.Columns.Add(bc);

                    //****End BoundColumns****//
                    DataSet iSet = PE_DAL.GetROOSubjectView(this.PubID);

                    if (iSet != null)
                    {
                        dgRegReimb.DataSource = iSet;
                        dgRegReimb.DataBind();
                        ((DataRowView)e.Item.DataItem).Row[0] = "";
                        e.Item.Cells[1].Controls.Clear();
                        e.Item.Cells[1].Controls.Add(dgRegReimb);
                    }
                    else
                    {
                        Label errLbl = new Label();
                        errLbl.Text = " - ";

                        e.Item.Cells[1].Controls.Clear();
                        e.Item.Cells[1].Controls.Add(errLbl);
                    }
                }
                //NCIPL_CC - Part of changes to have collections on NCIPL tab and ROO tab
                else if (String.Compare(((DataRowView)e.Item.DataItem).Row[0].ToString(), "Collections:", true) == 0)
                {
                    //For Displaying Collections
                    DataGrid dgCollectionsView = new DataGrid();
                    dgCollectionsView.ID = "innerdgCollectionsView";

                    //Format the DataGrid to look cool.
                    dgCollectionsView.BorderWidth = (Unit)0;
                    dgCollectionsView.CellPadding = 4;
                    dgCollectionsView.CellSpacing = 0;
                    dgCollectionsView.GridLines = GridLines.None;
                    dgCollectionsView.BorderColor = Color.FromName("#E0E0E0");

                    dgCollectionsView.ItemStyle.BackColor = Color.White;
                    dgCollectionsView.AlternatingItemStyle.BackColor = Color.FromName("LightGray");

                    dgCollectionsView.ShowHeader = false;
                    dgCollectionsView.HeaderStyle.CssClass = "fieldLabel";
                    dgCollectionsView.HeaderStyle.BackColor = Color.FromName("#ffff00");
                    dgCollectionsView.AutoGenerateColumns = false;

                    //****Add a series of BoundColumns****//
                    //***Region Name***//
                    BoundColumn bc = new BoundColumn();
                    //Set the BoundColumn Values
                    //bc.DataField = "Description";
                    bc.DataField = "SeriesName";
                    //bc.HeaderText = "Region(s)";
                    bc.ItemStyle.Wrap = false;
                    bc.ItemStyle.CssClass = "fieldLabel";
                    dgCollectionsView.Columns.Add(bc);

                    //****End BoundColumns****//
                    //DataSet iSet = PE_DAL.GetNCIPLStacksView(this.PubID);
                    SeriesCollection iSet = PE_DAL.GetCollectionsByInterfaceByPubId("NCIPL_CC", this.PubID);

                    //if (iSet != null)
                    if (iSet.Count > 0)
                    {
                        dgCollectionsView.DataSource = iSet;
                        dgCollectionsView.DataBind();
                        ((DataRowView)e.Item.DataItem).Row[0] = "";
                        e.Item.Cells[1].Controls.Clear();
                        e.Item.Cells[1].Controls.Add(dgCollectionsView);
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
            DataSet ds = PE_DAL.GetROOInterfaceView(this.PubID);
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