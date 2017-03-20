using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Data.Common;
using System.Xml;
using System.Net;
using System.Web.Services.Protocols;
//EntLib References
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Logging;
//using Microsoft.Practices.ObjectBuilder2;
using System.Text.RegularExpressions;
using GlobalUtils;

namespace PubEntAdmin
{
    public partial class SearchKeywords : System.Web.UI.Page
    {
        private static readonly string SP_ADMIN_GetAllReportSearchKeywords = "SP_ADMIN_GetAllReportSearchKeywords";
       // protected System.Web.UI.WebControls.TextBox TxtPubid;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session.IsNewSession)//cross-site request forgery
            {
                Response.Redirect("Home.aspx");
            }
            
            this.Title = "Search Keywords Report";
            this.PageTitle = "Search Keywords Report";

        }
        //*********************************************************************
        // GetArray List
        // Get data from the Database
        //*********************************************************************
        public ArrayList strQuery(DateTime sDate, DateTime eDate)
        {
            Database db = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(SP_ADMIN_GetAllReportSearchKeywords);

            db.AddInParameter(dbCommand, "@sDate", DbType.String, sDate);
            db.AddInParameter(dbCommand, "@eDate", DbType.String, eDate);
            
            ArrayList arPubid = new ArrayList();
            ArrayList list = new ArrayList();
           
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    object[] values = new object[dataReader.FieldCount];
                    dataReader.GetValues(values);
                    arPubid.Add(values);
                    list.Add(new Publications(values[0], values[1]));
                }
            }
            return list;
        }

        protected void btQuery_Click(object sender, EventArgs e)
        {
            string strsDate = this.txtStartDate.Text;
            string streDate = this.txtEndDate.Text;
            ArrayList list = null;
            Label9.ToolTip = "";
            Label8.Visible = false;
            ButtonExcel_Click.Visible = false;
            KwGridView.DataSource = list;
            KwGridView.AllowSorting = false;
            KwGridView.DataBind();
      
            Message.Text = "";
            if (strsDate.Length == 0 || streDate.Length == 0)
            {
                Message.Visible = true;
                Message.Text = "Please Enter required values";
            }
            else if (strsDate.Length != 0 && streDate.Length != 0)
            {
                DateTime sDate = Convert.ToDateTime(strsDate);
                DateTime eDate = Convert.ToDateTime(streDate);
                if (sDate > eDate)
                {
                    Message.Visible = true;
                    Message.Text = "Start Date cannot be greater than End Date";
                }
            }
            if (strsDate.Length != 0 && streDate.Length != 0 && (Convert.ToDateTime(strsDate) <= Convert.ToDateTime(streDate)))
            {
                Label9.Visible = true;
                lbtextDates.Text = " dates between " + strsDate + " and " + streDate;
                 list = strQuery(Convert.ToDateTime(strsDate), Convert.ToDateTime(streDate));
                if (list.Count != 0)
                {
                    KwGridView.DataSource = list;
                    KwGridView.AllowSorting = false;
                    KwGridView.DataBind();
                    Label8.Visible = true;
                    ButtonExcel_Click.Visible = true;
                }
                else if (list.Count == 0)
                {
                    Message.Text = "No Data available for selected Dates";
                }
            }

        }

        public class Publications
        {
            private string sSearchkeyword;
            private string sTotal;

            public Publications(object sSearchkeyword, object sTotal)
            {
                SearchKeyword = Convert.ToString(sSearchkeyword);
                Total = Convert.ToString(sTotal);
            }

            public string SearchKeyword
            {
                get
                {
                    return sSearchkeyword;
                }
                set
                {
                    sSearchkeyword = value;
                }
            }
            
            public string Total
            {
                get
                {
                    return sTotal;
                }
                set
                {
                    sTotal = value;
                }
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
        //*********************************************************************
        // Export to Excel
        //*********************************************************************
        public override void VerifyRenderingInServerForm(Control control)
        {
            return;
        }

        protected void ButtonExcel_Click_Click(object sender, ImageClickEventArgs e)
        {
            string stDate = this.txtStartDate.Text;
            string endDate = this.txtEndDate.Text;
            string message = "Search Keywords Report" + " -  Start Date: " + stDate + "  and " + "End Date: " + endDate;
            ExportRoutines.ExportToExcel(this.Page, "SearchKeyWordReport", message, KwGridView);
        }

    }

}
