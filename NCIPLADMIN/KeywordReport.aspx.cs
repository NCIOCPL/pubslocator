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
    public partial class KeywordReport : System.Web.UI.Page
    {
        private static readonly string SP_ADMIN_GetAllReportKeywords = "SP_ADMIN_GetAllReportKeywords";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session.IsNewSession)
            {
                Response.Redirect("Home.aspx");
            }
            if (!Page.IsPostBack)
            {
                this.Title = "Keywords Report";
                this.PageTitle = "Keywords Report";
                this.SecVal();
            }

        }

        #region Sec Val
        private void SecVal()
        {
            this.LenVal();
            this.TagVal();
            this.SpecialVal();
        }

        private void LenVal()
        {
            if ((!PubEntAdminManager.LenVal(this.TxtPubid.Text, 50)))
            {
                Response.Redirect("InvalidInput.aspx");
            }
        }

        private void TagVal()
        {
            if ((PubEntAdminManager.OtherVal(this.TxtPubid.Text)))
            {
                Response.Redirect("InvalidInput.aspx");
            }

        }

        private void SpecialVal()
        {

            if ((PubEntAdminManager.SpecialVal2(this.TxtPubid.Text.Replace(" ", ""))))
            {
                Response.Redirect("InvalidInput.aspx");
            }

        }
        #endregion

        //*********************************************************************
        // GetArray List
        // Get data from the Database
        //*********************************************************************
        public ArrayList strQuery(string[] iPRODUCTID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            string strSql = "SELECT PRODUCTID, KEYWORDS FROM TBL_PRODUCTS where PRODUCTID in (@PRODUCTID)";
            
            string productid = "'" + iPRODUCTID[0].Replace("'","") + "'";
            int len = iPRODUCTID.Length;
            if (len > 0 )
            {
                for (int i = 1; i < len; i++)
                {
                    string strproducdid = iPRODUCTID[i].Trim().Replace("'", "");
                    productid = productid + "," + "'" + strproducdid + "'";
                }
            }
            //System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(SP_ADMIN_GetKeywordsbypubid);
            //db.AddInParameter(dbCommand, "@PRODUCTID", DbType.String, productid);
            strSql = System.Text.RegularExpressions.Regex.Replace(strSql, "@PRODUCTID", productid);
            System.Data.Common.DbCommand dbCommand = db.GetSqlStringCommand(strSql);
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

        public ArrayList strAllQuery()
        {
            Database db = DatabaseFactory.CreateDatabase();
            System.Data.Common.DbCommand dbCommand = db.GetStoredProcCommand(SP_ADMIN_GetAllReportKeywords);
           
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
            KwGridView.Visible = false;
            ButtonExcel_Click.Visible = false;
            Label8.Visible = false;
            /****EAC BAD CODE int Singlequote=0;*/

            Message.Visible = false;
            string[] split = null;
            string iProdId = null;
            string delimStr = ",";
            char[] delimiter = delimStr.ToCharArray();
            Boolean valid = false;
            Boolean validnum = false;
            Boolean validlen = false;
            iProdId = TxtPubid.Text;

            if (iProdId.Length != 0)
            {
                valid = PubEntAdminManager.OtherVal(iProdId);
                validnum = PubEntAdminManager.SpecialVal2(iProdId);
                validlen = PubEntAdminManager.LenVal(iProdId, 50);
                /****EAC BAD CODE Singlequote = iProdId.IndexOf(",");*/
            }

            if ((valid == false) && (validnum == false) && (validlen == true) /****EAC BAD CODE & Singlequote==0*/)
            {
                split = iProdId.Split(delimiter);
                ArrayList list = strQuery(split);
                if (iProdId == "")
                {
                    Message.Visible = true;
                }
                else if (list.Count == 0)
                {
                    ButtonExcel_Click.Visible = false;
                    Label8.Visible = false;
                    Message.Text = "Invalid Publication ID(s)";
                    Message.Visible = true;
                }
                else
                {
                    KwGridView.DataSource = list;
                    KwGridView.DataBind();
                    KwGridView.Visible = true;
                    Label8.Visible = true;
                    ButtonExcel_Click.Visible = true;
                }
            }
            else
            {

                Response.Redirect("InvalidInput.aspx");
            }
        }

        public class Publications
        {
            private string spubid;
            private string skeyword;

            public Publications(object spubid, object skeyword)
                {
                    PublicationID = Convert.ToString(spubid);
                    Keywords = Convert.ToString(skeyword);
                }

            public string PublicationID
                {
                    get
                    {
                        return spubid;
                    }
                    set
                    {
                        spubid = value;
                    }
                }

            public string Keywords
                {
                    get
                    {
                        return skeyword;
                    }
                    set
                    {
                        skeyword = value;
                    }
                }
        }

               
        public override void VerifyRenderingInServerForm(Control control)
        {
            return;
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

        protected void ButtonExcel_Click_Click(object sender, ImageClickEventArgs e)
        {
            //ExportToExcel(Server.MapPath("KeyWordReport.xls"), KwGridView);
            ExportRoutines.ExportToExcel(this.Page, "KeyWordReport", "Keywords Report", KwGridView);
        }

        protected void btAll_Click(object sender, EventArgs e)
        {
            Message.Visible = false;
            TxtPubid.Text = "";
            ArrayList list = strAllQuery();
            KwGridView.DataSource = list;
            KwGridView.DataBind();
            KwGridView.Visible = true;
            Label8.Visible = true;
            ButtonExcel_Click.Visible = true;
        }
    }
    
}
