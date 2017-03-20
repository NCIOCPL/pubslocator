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
using NCIPLex.BLL;
using NCIPLex.DAL;
using NCIPLex.GlobalUtils;


namespace NCIPLex
{
	/// <summary>
	/// Summary description for Tester.
	/// </summary>
	public class Search : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.Label Label4;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
            if (Utils.ValidateRedirect().Length > 0) //Important check
                Response.Redirect(Utils.ValidateRedirect(), true);

            if (!Page.IsPostBack)
			{
                Session["NCIPLEX_SearchKeyword"] = "";
                Session["NCIPLEX_TypeOfCancer"] = "";
                Session["NCIPLEX_Subject"] = "";
                Session["NCIPLEX_Audience"] = "";
                Session["NCIPLEX_Language"] = "";
                Session["NCIPLEX_ProductFormat"] = "";
                Session["NCIPLEX_StartsWith"] = "";
                Session["NCIPLEX_Series"] = "";
                Session["NCIPLEX_NewOrUpdated"] = "";
                Session["NCIPLEX_Race"] = "";


				if (Request.QueryString["cantype"] != null)
					Session["NCIPLEX_TypeOfCancer"]	= Request.QueryString["cantype"];
				else if (Request.QueryString["subj"] != null)
					Session["NCIPLEX_Subject"]	= Request.QueryString["subj"];
				else if (Request.QueryString["aud"] != null)
					Session["NCIPLEX_Audience"]	= Request.QueryString["aud"];
				else if (Request.QueryString["lang"] != null)
					Session["NCIPLEX_Language"]	= Request.QueryString["lang"];
				else if (Request.QueryString["form"] != null)
					Session["NCIPLEX_ProductFormat"]	= Request.QueryString["form"];
                else if (Request.QueryString["starts"] != null)
                    Session["NCIPLEX_StartsWith"] = Request.QueryString["starts"];
                else if (Request.QueryString["coll"] != null)
                    Session["NCIPLEX_Series"] = Request.QueryString["coll"];
                else if (Request.QueryString["newupt"] != null)
                    Session["NCIPLEX_NewOrUpdated"] = Request.QueryString["newupt"];
                else if (Request.QueryString["race"] != null)
                    Session["NCIPLEX_Race"] = Request.QueryString["race"];

				
                //Begin - Code to show search criteria on search results
                Session["NCIPLEX_Criteria"] = "";
                string SearchCriteria = "";
                GlobalUtils.Utils.InitializeCriteriaTextSessionVariables(); //CR-31 HITT 7074
                if (Session["NCIPLEX_TypeOfCancer"].ToString().Length > 0)
                {
                    KVPairCollection collCancerTypes = KVPair.GetKVPair("sp_NCIPLex_getCancerTypes");
                    foreach (KVPair kvItem in collCancerTypes)
                    {
                        if (string.Compare(kvItem.Key, Session["NCIPLEX_TypeOfCancer"].ToString(), true) == 0)
                        {
                            if (SearchCriteria.Length == 0)
                                SearchCriteria = kvItem.Val;
                            else
                                SearchCriteria = SearchCriteria + ", " + kvItem.Val;
                            GlobalUtils.Utils.SetCriteriaText("TypeOfCancer", kvItem.Val); //HITT 7074 CR-31
                        }
                    }
                }
                
                if (Session["NCIPLEX_Subject"].ToString().Length > 0)
                {
                    KVPairCollection collSubjects = KVPair.GetKVPair("sp_NCIPLex_getSubjects");
                    foreach (KVPair kvItem in collSubjects)
                    {
                        if (string.Compare(kvItem.Key, Session["NCIPLEX_Subject"].ToString(), true) == 0)
                        {
                            if (SearchCriteria.Length == 0)
                                SearchCriteria = kvItem.Val;
                            else
                                SearchCriteria = SearchCriteria + ", " + kvItem.Val;
                            GlobalUtils.Utils.SetCriteriaText("Subject", kvItem.Val); //HITT 7074 CR-31
                        }
                    }
                }
                if (Session["NCIPLEX_ProductFormat"].ToString().Length > 0)
                {
                    KVPairCollection collProdFormats = KVPair.GetKVPair("sp_NCIPLex_getProductFormats");
                    foreach (KVPair kvItem in collProdFormats)
                    {
                        if (string.Compare(kvItem.Key, Session["NCIPLEX_ProductFormat"].ToString(), true) == 0)
                        {
                            if (SearchCriteria.Length == 0)
                                SearchCriteria = kvItem.Val;
                            else
                                SearchCriteria = SearchCriteria + ", " + kvItem.Val;
                            GlobalUtils.Utils.SetCriteriaText("ProductFormat", kvItem.Val); //HITT 7074 CR-31
                        }
                    }
                }
                if (Session["NCIPLEX_Series"].ToString().Length > 0)
                {
                    KVPairCollection collSeries = KVPair.GetKVPair("sp_NCIPLex_getCollections");
                    foreach (KVPair kvItem in collSeries)
                    {
                        if (string.Compare(kvItem.Key, Session["NCIPLEX_Series"].ToString(), true) == 0)
                        {
                            if (SearchCriteria.Length == 0)
                                SearchCriteria = kvItem.Val;
                            else
                                SearchCriteria = SearchCriteria + ", " + kvItem.Val;
                            GlobalUtils.Utils.SetCriteriaText("Series", kvItem.Val); //HITT 7074 CR-31
                        }
                    }
                }
                if (Session["NCIPLEX_NewOrUpdated"].ToString().Length > 0)
                {
                    KVPairCollection collProdUpdates = KVPair.GetKVPair("sp_NCIPLex_getProductUpdates");
                    foreach (KVPair kvItem in collProdUpdates)
                    {
                        if (string.Compare(kvItem.Key, Session["NCIPLEX_NewOrUpdated"].ToString(), true) == 0)
                        {
                            if (SearchCriteria.Length == 0)
                                SearchCriteria = kvItem.Val;
                            else
                                SearchCriteria = SearchCriteria + ", " + kvItem.Val;
                            GlobalUtils.Utils.SetCriteriaText("NewOrUpdated", kvItem.Val); //HITT 7074 CR-31
                        }
                    }
                }
                if (Session["NCIPLEX_Language"].ToString().Length > 0)
                {
                    KVPairCollection collLanguages = KVPair.GetKVPair("sp_NCIPLex_getLanguages");
                    foreach (KVPair kvItem in collLanguages)
                    {
                        if (string.Compare(kvItem.Key, Session["NCIPLEX_Language"].ToString(), true) == 0)
                        {
                            if (SearchCriteria.Length == 0)
                                SearchCriteria = kvItem.Val;
                            else
                                SearchCriteria = SearchCriteria + ", " + kvItem.Val;
                            GlobalUtils.Utils.SetCriteriaText("Language", kvItem.Val); //HITT 7074 CR-31
                        }
                    }
                }
                if (Session["NCIPLEX_Audience"].ToString().Length > 0)
                {
                    KVPairCollection collAudience = KVPair.GetKVPair("sp_NCIPLex_getAudience");
                    foreach (KVPair kvItem in collAudience)
                    {
                        if (string.Compare(kvItem.Key, Session["NCIPLEX_Audience"].ToString(), true) == 0)
                        {
                            if (SearchCriteria.Length == 0)
                                SearchCriteria = kvItem.Val;
                            else
                                SearchCriteria = SearchCriteria + ", " + kvItem.Val;
                            GlobalUtils.Utils.SetCriteriaText("Audience", kvItem.Val); //HITT 7074 CR-31
                        }
                    }
                }

                if (Session["NCIPLEX_StartsWith"].ToString().Length > 0)
                {
                    if (SearchCriteria.Length == 0)
                        SearchCriteria = Session["NCIPLEX_StartsWith"].ToString();
                    else
                        SearchCriteria = SearchCriteria + ", " + Session["NCIPLEX_StartsWith"].ToString();
                    GlobalUtils.Utils.SetCriteriaText("StartsWith", Session["NCIPLEX_StartsWith"].ToString()); //HITT 7074 CR-31
                }

                if (Session["NCIPLEX_Race"].ToString().Length > 0)
                {
                    KVPairCollection collRace = KVPair.GetKVPair("sp_NCIPLex_getRace");
                    foreach (KVPair kvItem in collRace)
                    {
                        if (string.Compare(kvItem.Key, Session["NCIPLEX_Race"].ToString(), true) == 0)
                        {
                            if (SearchCriteria.Length == 0)
                                SearchCriteria = kvItem.Val;
                            else
                                SearchCriteria = SearchCriteria + ", " + kvItem.Val;
                            GlobalUtils.Utils.SetCriteriaText("Race", kvItem.Val); //HITT 7074 CR-31
                        }
                    }
                }
                
                Session["NCIPLEX_Criteria"] = SearchCriteria;
                
                //End Code

                /*Begin CR-31 - HITT 9815 */
                GlobalUtils.Utils objUtils = new GlobalUtils.Utils();
                string QueryParams = objUtils.GetQueryStringParams();
                objUtils = null;
                /*End CR-31 - HITT 9815 */

                if (Session["NCIPLEX_NewOrUpdated"].ToString().Length > 0) //HITT 8300 - New & Updated now has its own page
                    Response.Redirect("newupdated.aspx");
                else
                {
                    //CR-31 HITT 9815 Response.Redirect("searchres.aspx");
                    Response.Redirect("searchres.aspx" + "?sid=" + QueryParams);
                }
			}
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
