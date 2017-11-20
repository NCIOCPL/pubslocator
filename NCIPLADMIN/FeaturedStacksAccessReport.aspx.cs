using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//added
using PubEntAdmin.DAL;
using PubEntAdmin.BLL;
using GlobalUtils;

namespace PubEntAdmin
{
    public partial class FeaturedStacksAccessReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session.IsNewSession)
            {
                Response.Redirect("Home.aspx");
            }
            this.Title = "Featured Stacks Access Report";
            this.PageTitle = "Featured Stacks Access Report";

            if (!((CustomPrincipal)Context.User).IsInRole(PubEntAdminManager.AdminRole))
            {
                PubEntAdminManager.UnathorizedAccess();
            }

            //this.BindGrid();
        }

        private void BindGrid(DateTime sDate, DateTime eDate)
        {
            gvStackAccess.DataSource = PE_DAL.GetFeaturedStacksAccess(sDate, eDate);
            gvStackAccess.DataBind();
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
                    //StackSuperHistRecCollection SuperHistColl = PE_DAL.GetFeaturedStacksHistory(Convert.ToDateTime(strsDate.Trim()), Convert.ToDateTime(streDate.Trim()));
                    this.BindGrid(sDate, eDate);
                    if (gvStackAccess.Rows.Count > 0)
                    {
                        //HITT 11616 lblTextandDates.Text = "Featured Stacks accessed between " + strsDate + " and " + streDate;
                        //lblTextandDates.Text = "Featured Stacks Access<br/>Date Range " + strsDate + " to " + streDate;
                        lblTextandDates.Text = "Featured Stacks Access: Date Range " + strsDate + " to " + streDate;
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

        private void SetVisiblity(bool Visible)
        {
            if (Visible)
            {
                lblExporttoExcel.Visible = true;
                ImgBtnExporttoExcel.Visible = true;
                //lblTextandDates.Visible = true;
                gvStackAccess.Visible = true;
            }
            else
            {
                lblExporttoExcel.Visible = false;
                ImgBtnExporttoExcel.Visible = false;
                lblTextandDates.Text = "";
                gvStackAccess.Visible = false;
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
            ExportRoutines.ExportToExcel(this.Page, "StacksAccessed", message, gvStackAccess);
        }
        #endregion
    }
}
