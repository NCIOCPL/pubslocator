using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using PubEnt.BLL;
using PubEnt.DAL;

namespace PubEnt
{
	/// <summary>
	/// Summary description for Tester.
	/// </summary>
	public class cannedsearch : System.Web.UI.Page
	{
        //protected System.Web.UI.WebControls.Label Label1;
        //protected System.Web.UI.WebControls.Label Label2;
        //protected System.Web.UI.WebControls.Label Label3;
        //protected System.Web.UI.WebControls.Label Label4;

        string rid = "";
        string SearchCriteria = "";
        string CannId = "";
	
		private void Page_Load(object sender, System.EventArgs e)
		{
           
            ////For the canned search page -- allow users to come in directly
            //if (Session["JSTurnedOn"] == null)
            //{
            //    Session["JSTurnedOn"] = "True"; //Assuming JavaScript is enabled, by default.
            //    if (Session["NCIPL_Pubs"] == null)
            //        Session["NCIPL_Pubs"] = "";
            //    if (Session["NCIPL_Qtys"] == null)
            //        Session["NCIPL_Qtys"] = "";
            //    if (Session["PUBENT_SearchKeyword"] == null)
            //        Session["PUBENT_SearchKeyword"] = "";
            //    if (Session["PUBENT_TypeOfCancer"] == null)
            //        Session["PUBENT_TypeOfCancer"] = "";
            //    if (Session["PUBENT_Subject"] == null)
            //        Session["PUBENT_Subject"] = "";
            //    if (Session["PUBENT_Audience"] == null)
            //        Session["PUBENT_Audience"] = "";
            //    if (Session["PUBENT_ProductFormat"] == null)
            //        Session["PUBENT_ProductFormat"] = "";
            //    if (Session["PUBENT_Language"] == null)
            //        Session["PUBENT_Language"] = "";
            //    if (Session["PUBENT_StartsWith"] == null)
            //        Session["PUBENT_StartsWith"] = "";
            //    if (Session["PUBENT_Series"] == null)
            //        Session["PUBENT_Series"] = ""; //Or collection
            //    if (Session["PUBENT_NewOrUpdated"] == null)
            //        Session["PUBENT_NewOrUpdated"] = "";
            //    if (Session["PUBENT_Race"] == null)
            //        Session["PUBENT_Race"] = "";
            //}
            ////End of code
            
            //if (!Page.IsPostBack)
            //{
            //    Session["PUBENT_SearchKeyword"] = "";
            //    Session["PUBENT_TypeOfCancer"]	= "";
            //    Session["PUBENT_Subject"]	= "";
            //    Session["PUBENT_Audience"] = "";
            //    Session["PUBENT_Language"] = "";
            //    Session["PUBENT_ProductFormat"] = "";
            //    Session["PUBENT_StartsWith"] = "";
            //    Session["PUBENT_Series"] = "";
            //    Session["PUBENT_NewOrUpdated"] = "";
            //    Session["PUBENT_Race"] = "";

            //    if (Request.UrlReferrer != null) //Added if condition to avoid resetting session variables if coming from detail page
            //    {
            //        if (Session["PUBENT_CannedSearch"] != null && Request.UrlReferrer.ToString().Contains("detail.aspx"))
            //            Response.Redirect("cannedsearchres.aspx", true);
            //    }
                
            //    Session["PUBENT_CannedSearch"] = "";
            //    Session["PUBENT_Criteria"] = "";
                
            //}

            if (Request.QueryString["rid"] != null)
            {
                rid = Request.QueryString["rid"].Trim();

                //Some checks
                if (string.Compare(rid, "") == 0)
                    Response.Redirect("default.aspx?redirect=cannedsearch1", true);
                if (rid.Length > 6)
                    Response.Redirect("default.aspx?redirect=cannedsearch2", true);

                ///Expecting only one entry in the KVPair Collection, but the collecion 
                ///is used in order to make use of the KVPair class. 
                ///If multiple cannid values are present (unlikely) then the last cannid will be used.
                KVPairCollection collCann = DAL.DAL.GetCannedSearchIdText("sp_NCIPL_CannedSearchIdText", rid);
                foreach (KVPair kvItem in collCann)
                {
                    CannId = kvItem.Key;
                    SearchCriteria = kvItem.Val;
                }
                if (CannId.Length == 0)
                    Response.Redirect("default.aspx?redirect=cannedsearch3", true);
            }
            else
                Response.Redirect("default.aspx?redirect=cannedsearch4", true);

            //At this point everything is good
            Session["PUBENT_CannedSearch"] = CannId;
            Session["PUBENT_Criteria"] = SearchCriteria;
            
            //Response.Redirect("cannedsearchres.aspx", true);
            //yma change it to directly goto searchres.aspx
            Response.Redirect("searchres.aspx?canned=1", true);

		}

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }
		
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {    
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion

	}
}
