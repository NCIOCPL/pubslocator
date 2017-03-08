using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//added
using PubEntAdmin.BLL;
using PubEntAdmin.DAL;
using GlobalUtils;

namespace PubEntAdmin
{
    public partial class FeaturedPubsHistReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session.IsNewSession)
            {
                Response.Redirect("Home.aspx");
            }
            this.Title = "Featured Stacks History Report";
            this.PageTitle = "Featured Stacks History Report";
            //this.PageTitle = "Featured Publications Setup";
            //this.AddJS(Server.MapPath("JS/LUMgmt.js"));

            if (!((CustomPrincipal)Context.User).IsInRole(PubEntAdminManager.AdminRole))
            {
                PubEntAdminManager.UnathorizedAccess();
            }
           
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            string strsDate = this.txtStartDate.Text.Trim();
            string streDate = this.txtEndDate.Text.Trim();
            if (strsDate.Length == 0 || streDate.Length == 0)
            {
                lblMessage.Visible = true;
                lblMessage.Text = "Please enter required values.";
                this.SetVisiblity(false);
            }
            else
            {
                DateTime sDate = Convert.ToDateTime(strsDate);
                DateTime eDate = Convert.ToDateTime(streDate);
                if (sDate > eDate)
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Start Date cannot be greater than End Date";
                    this.SetVisiblity(false);
                }
                else
                {
                    StackSuperHistRecCollection SuperHistColl = PE_DAL.GetFeaturedStacksHistory(Convert.ToDateTime(strsDate.Trim()), Convert.ToDateTime(streDate.Trim()));
                    if (SuperHistColl.Count > 0)
                    {
                        //Related to HITT 11615 lblTextandDates.Text = "Active Featured Stacks between " + strsDate + " and " + streDate;
                        //lblTextandDates.Text = "Featured Stacks with Active Start Date between " + strsDate + " and " + streDate;
                        //lblTextandDates.Text = "Active Featured Stacks<br/>Date Range " + strsDate + " to " + streDate;
                        lblTextandDates.Text = "Featured Stacks: Active Start Date Range " + strsDate + " to " + streDate;
                        gvStackHistory.DataSource = SuperHistColl;
                        gvStackHistory.DataBind();
                        this.SetVisiblity(true);
                    }
                    else
                    {
                        lblTextandDates.Text = "No data available for the selected dates.";
                        this.SetVisiblity(false);
                    }
                }
            }

        }

        protected void gvStackHistory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                StackHistRecCollection StackHistColl = (StackHistRecCollection)e.Row.DataItem;
                
                Literal litPubTitles = (Literal)e.Row.FindControl("litPubTitles");
                Literal litActiveSDates = (Literal)e.Row.FindControl("litActiveSDates");
                Literal litActiveEDates = (Literal)e.Row.FindControl("litActiveEDates");

                string Titles = ""; string SDates = ""; string EDates = ""; string divHeight = "";
                string tempStartDt = ""; string tempEndDt = "";
                Titles = "<div style=\"padding-top:1px;padding-bottom:1px;height:10px;font-weight:bold\">" +
                            StackHistColl.StackTitle +
                        "</div>";

                tempStartDt = (StackHistColl.StackStartDate == DateTime.MinValue) ? "N/A" : StackHistColl.StackStartDate.ToShortDateString();
                SDates = "<div style=\"padding-top:1px;padding-bottom:1px;height:10px;\">" +
                            tempStartDt +
                        "</div>";
                tempEndDt = (StackHistColl.StackEndDate == DateTime.MinValue) ? "N/A" : StackHistColl.StackEndDate.ToShortDateString();
                EDates = "<div style=\"padding-top:1px;padding-bottom:1px;height:10px;\">" +
                            tempEndDt +
                        "</div>";
                        
                foreach (StackHistRec StackHistRec in StackHistColl)
                {
                    divHeight = "10px";
                    if (StackHistRec.LongTitle.Length >= 90)
                    {
                        divHeight = "25px";
                    }

                    //(dr["StackStartDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)dr["StackStartDate"],
                    tempStartDt = (StackHistRec.PubStartDate == DateTime.MinValue) ? "N/A" : StackHistRec.PubStartDate.ToShortDateString();
                    tempEndDt = (StackHistRec.PubEndDate == DateTime.MinValue) ? "N/A" : StackHistRec.PubEndDate.ToShortDateString();
                    
                    //Titles += "<div style=\"padding-left:5px;padding-top:1px;padding-bottom:1px;height:" + divHeight + ";\">" + StackHistRec.LongTitle + "</div>";
                    Titles += "<div style=\"padding-top:1px;padding-bottom:1px;height:" + divHeight + ";\">" + StackHistRec.ProductId + " - " + StackHistRec.LongTitle + "</div>";
                    SDates += "<div style=\"padding-top:1px;padding-bottom:1px;height:" + divHeight + ";\">" + tempStartDt + "</div>";
                    EDates += "<div style=\"padding-top:1px;padding-bottom:1px;height:" + divHeight + ";\">" + tempEndDt + "</div>";
                }

                litPubTitles.Text = Titles;
                litActiveSDates.Text = SDates;
                litActiveEDates.Text = EDates;

                StackHistColl = null;
            }
        }

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

        #region ExporttoExcel
        //*********************************************************************
        // Export to Excel
        //*********************************************************************
        public override void VerifyRenderingInServerForm(Control control)
        {
            return;
        }
        protected void ImgBtnExporttoExcel_Click(object sender, ImageClickEventArgs e)
        {
            string stDate = this.txtStartDate.Text;
            string endDate = this.txtEndDate.Text;
            string message = lblTextandDates.Text;
            ExportRoutines.ExportToExcel(this.Page, "ActiveStacksHistory", message, gvStackHistory);
        }
        #endregion
        private void SetVisiblity(bool Visible)
        {
            if (Visible)
            {
                lblExporttoExcel.Visible = true;
                ImgBtnExporttoExcel.Visible = true;
                //lblTextandDates.Visible = true;
                gvStackHistory.Visible = true;
            }
            else
            {
                lblExporttoExcel.Visible = false;
                ImgBtnExporttoExcel.Visible = false;
                lblTextandDates.Text = "";
                gvStackHistory.Visible = false;
            }
        }
    }
}
