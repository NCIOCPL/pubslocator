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

using PubEntAdmin.DAL;
using PubEntAdmin.BLL;

namespace PubEntAdmin.UserControl
{
    public partial class PubHistTabReadInfo : System.Web.UI.UserControl
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

        protected void gvResult_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PubHistCollection dt = ((PubHistCollection)this.gvResult.DataSource);


            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                PubHist l_pub = dt[e.Row.DataItemIndex];

                /*if (e.Row.RowIndex % 2 == 0)
                {
                    e.Row.Style["color"] = "#0000ff";
                }
                else
                {
                    e.Row.Style["color"] = "#00bb00";
                }*/

                Label lbldt = new Label();
                if (l_pub.ReceivedDate.CompareTo(PubEntAdminManager.SQLDefaultDt) == 0)
                {
                    lbldt.Text = "-";
                    e.Row.Cells[1].Controls.Add(lbldt);
                }
                else
                {
                    lbldt.Text = l_pub.ReceivedDate.ToShortDateString();
                    e.Row.Cells[1].Controls.Add(lbldt);
                }

                Label lblNIHNum = new Label();

                lblNIHNum.Text = l_pub.NIHNum1 + "-" + l_pub.NIHNum2;
                e.Row.Cells[3].Controls.Add(lblNIHNum);

                lbldt = new Label();
                if (l_pub.LatestPrintDate_M == 0)
                {
                    if (l_pub.LatestPrintDate_Y == 0)
                        lbldt.Text = "-";
                    else
                        lbldt.Text = l_pub.LatestPrintDate_Y.ToString();

                    e.Row.Cells[4].Controls.Add(lbldt);
                }
                else
                {

                    lbldt.Text = l_pub.LatestPrintDate_M.ToString();

                    if (l_pub.LatestPrintDate_D == 0)
                    {
                        if (l_pub.LatestPrintDate_Y == 0)
                        {
                            lbldt.Text = "-";
                        }
                        else
                        {
                            lbldt.Text += "/" + l_pub.LatestPrintDate_Y.ToString();
                        }
                    }
                    else
                    {
                        lbldt.Text += "/" + l_pub.LatestPrintDate_D.ToString();
                        if (l_pub.LatestPrintDate_Y == 0)
                        {
                            lbldt.Text = "-";
                        }
                        else
                        {
                            lbldt.Text += "/" + l_pub.LatestPrintDate_Y.ToString();
                        }
                    }
                    e.Row.Cells[4].Controls.Add(lbldt);
                }

                lbldt = new Label();
                if (l_pub.RevisedDate_M == 0)
                {
                    if (l_pub.RevisedDate_Y == 0)
                        lbldt.Text = "-";
                    else
                        lbldt.Text = l_pub.RevisedDate_Y.ToString();

                    e.Row.Cells[5].Controls.Add(lbldt);
                }
                else
                {

                    lbldt.Text = l_pub.RevisedDate_M.ToString();

                    if (l_pub.RevisedDate_D == 0)
                    {
                        if (l_pub.RevisedDate_Y == 0)
                        {
                            lbldt.Text = "-";
                        }
                        else
                        {
                            lbldt.Text += "/" + l_pub.RevisedDate_Y.ToString();
                        }
                    }
                    else
                    {
                        lbldt.Text += "/" + l_pub.RevisedDate_D.ToString();
                        if (l_pub.RevisedDate_Y == 0)
                        {
                            lbldt.Text = "-";
                        }
                        else
                        {
                            lbldt.Text += "/" + l_pub.RevisedDate_Y.ToString();
                        }
                    }
                    e.Row.Cells[5].Controls.Add(lbldt);
                }

            }
        }

        protected void gvResult_PreRender(object sender, EventArgs e)
        {
            this.gvResult.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
        #endregion

        #region Methods
        protected void BindData()
        {
            this.gvResult.DataSource = PE_DAL.GetProdHist(this.PubID);
            this.gvResult.DataBind();
        }

        public void ReloadData()
        {
            this.BindData();
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