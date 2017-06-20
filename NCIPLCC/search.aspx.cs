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
	public class Search : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.Label Label4;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
   
            if (!Page.IsPostBack)
			{
				Session["PUBENT_SearchKeyword"] = "";
				Session["PUBENT_TypeOfCancer"]	= "";
				Session["PUBENT_Subject"]	= "";
				Session["PUBENT_Audience"] = "";
				Session["PUBENT_Language"] = "";
				Session["PUBENT_ProductFormat"] = "";
                Session["PUBENT_StartsWith"] = "";
                Session["PUBENT_Series"] = "";
                Session["PUBENT_NewOrUpdated"] = "";
                Session["PUBENT_Race"] = "";


				if (Request.QueryString["cantype"] != null)
					Session["PUBENT_TypeOfCancer"]	= Request.QueryString["cantype"];
				else if (Request.QueryString["subj"] != null)
					Session["PUBENT_Subject"]	= Request.QueryString["subj"];
				else if (Request.QueryString["aud"] != null)
					Session["PUBENT_Audience"]	= Request.QueryString["aud"];
				else if (Request.QueryString["lang"] != null)
					Session["PUBENT_Language"]	= Request.QueryString["lang"];
				else if (Request.QueryString["form"] != null)
					Session["PUBENT_ProductFormat"]	= Request.QueryString["form"];
                else if (Request.QueryString["starts"] != null)
                    Session["PUBENT_StartsWith"] = Request.QueryString["starts"];
                else if (Request.QueryString["coll"] != null)
                    Session["PUBENT_Series"] = Request.QueryString["coll"];
                else if (Request.QueryString["newupt"] != null)
                    Session["PUBENT_NewOrUpdated"] = Request.QueryString["newupt"];
                else if (Request.QueryString["race"] != null)
                    Session["PUBENT_Race"] = Request.QueryString["race"];

				
                //Begin - Code to show search criteria on search results
                Session["PUBENT_Criteria"] = "";
                string SearchCriteria = "";
                GlobalUtils.Utils.InitializeCriteriaTextSessionVariables(); //CR-31 HITT 7074
                if (Session["PUBENT_TypeOfCancer"].ToString().Length > 0)
                {
                    //NCIPLCC KVPairCollection collCancerTypes = KVPair.GetKVPair("sp_NCIPL_getCancerTypes");
                    KVPairCollection collCancerTypes = KVPair.GetKVPair("sp_NCIPLCC_getCancerTypes");
                    foreach (KVPair kvItem in collCancerTypes)
                    {
                        if (string.Compare(kvItem.Key, Session["PUBENT_TypeOfCancer"].ToString(), true) == 0)
                        {
                            if (SearchCriteria.Length == 0)
                                SearchCriteria = kvItem.Val;
                            else
                                SearchCriteria = SearchCriteria + ", " + kvItem.Val;
                            GlobalUtils.Utils.SetCriteriaText("TypeOfCancer", kvItem.Val); //HITT 7074 CR-31
                        }
                    }
                }
                
                if (Session["PUBENT_Subject"].ToString().Length > 0)
                {
                    //NCIPLCC KVPairCollection collSubjects = KVPair.GetKVPair("sp_NCIPL_getSubjects");
                    KVPairCollection collSubjects = KVPair.GetKVPair("sp_NCIPLCC_getSubjects");
                    foreach (KVPair kvItem in collSubjects)
                    {
                        if (string.Compare(kvItem.Key, Session["PUBENT_Subject"].ToString(), true) == 0)
                        {
                            if (SearchCriteria.Length == 0)
                                SearchCriteria = kvItem.Val;
                            else
                                SearchCriteria = SearchCriteria + ", " + kvItem.Val;
                            GlobalUtils.Utils.SetCriteriaText("Subject", kvItem.Val); //HITT 7074 CR-31
                        }
                    }
                }
                if (Session["PUBENT_ProductFormat"].ToString().Length > 0)
                {
                    //NCIPLCC KVPairCollection collProdFormats = KVPair.GetKVPair("sp_NCIPL_getProductFormats");
                    KVPairCollection collProdFormats = KVPair.GetKVPair("sp_NCIPLCC_getProductFormats");
                    foreach (KVPair kvItem in collProdFormats)
                    {
                        if (string.Compare(kvItem.Key, Session["PUBENT_ProductFormat"].ToString(), true) == 0)
                        {
                            if (SearchCriteria.Length == 0)
                                SearchCriteria = kvItem.Val;
                            else
                                SearchCriteria = SearchCriteria + ", " + kvItem.Val;
                            GlobalUtils.Utils.SetCriteriaText("ProductFormat", kvItem.Val); //HITT 7074 CR-31
                        }
                    }
                }
                if (Session["PUBENT_Series"].ToString().Length > 0)
                {
                    //NCIPLCC KVPairCollection collSeries = KVPair.GetKVPair("sp_NCIPL_getCollections");
                    KVPairCollection collSeries = KVPair.GetKVPair("sp_NCIPLCC_getCollections");
                    foreach (KVPair kvItem in collSeries)
                    {
                        if (string.Compare(kvItem.Key, Session["PUBENT_Series"].ToString(), true) == 0)
                        {
                            if (SearchCriteria.Length == 0)
                                SearchCriteria = kvItem.Val;
                            else
                                SearchCriteria = SearchCriteria + ", " + kvItem.Val;
                            GlobalUtils.Utils.SetCriteriaText("Series", kvItem.Val); //HITT 7074 CR-31
                        }
                    }
                }
                if (Session["PUBENT_NewOrUpdated"].ToString().Length > 0)
                {
                    //NCIPLCC KVPairCollection collProdUpdates = KVPair.GetKVPair("sp_NCIPL_getProductUpdates");
                    KVPairCollection collProdUpdates = KVPair.GetKVPair("sp_NCIPLCC_getProductUpdates");
                    foreach (KVPair kvItem in collProdUpdates)
                    {
                        if (string.Compare(kvItem.Key, Session["PUBENT_NewOrUpdated"].ToString(), true) == 0)
                        {
                            if (SearchCriteria.Length == 0)
                                SearchCriteria = kvItem.Val;
                            else
                                SearchCriteria = SearchCriteria + ", " + kvItem.Val;
                            GlobalUtils.Utils.SetCriteriaText("NewOrUpdated", kvItem.Val); //HITT 7074 CR-31
                        }
                    }
                }
                if (Session["PUBENT_Language"].ToString().Length > 0)
                {
                    //NCIPLCC KVPairCollection collLanguages = KVPair.GetKVPair("sp_NCIPL_getLanguages");
                    KVPairCollection collLanguages = KVPair.GetKVPair("sp_NCIPLCC_getLanguages");
                    foreach (KVPair kvItem in collLanguages)
                    {
                        if (string.Compare(kvItem.Key, Session["PUBENT_Language"].ToString(), true) == 0)
                        {
                            if (SearchCriteria.Length == 0)
                                SearchCriteria = kvItem.Val;
                            else
                                SearchCriteria = SearchCriteria + ", " + kvItem.Val;
                            GlobalUtils.Utils.SetCriteriaText("Language", kvItem.Val); //HITT 7074 CR-31
                        }
                    }
                }
                if (Session["PUBENT_Audience"].ToString().Length > 0)
                {
                    //NCIPLCC KVPairCollection collAudience = KVPair.GetKVPair("sp_NCIPL_getAudience");
                    KVPairCollection collAudience = KVPair.GetKVPair("sp_NCIPLCC_getAudience");
                    foreach (KVPair kvItem in collAudience)
                    {
                        if (string.Compare(kvItem.Key, Session["PUBENT_Audience"].ToString(), true) == 0)
                        {
                            if (SearchCriteria.Length == 0)
                                SearchCriteria = kvItem.Val;
                            else
                                SearchCriteria = SearchCriteria + ", " + kvItem.Val;
                            GlobalUtils.Utils.SetCriteriaText("Audience", kvItem.Val); //HITT 7074 CR-31
                        }
                    }
                }

                if (Session["PUBENT_StartsWith"].ToString().Length > 0)
                {
                    if (SearchCriteria.Length == 0)
                        SearchCriteria = Session["PUBENT_StartsWith"].ToString();
                    else
                        SearchCriteria = SearchCriteria + ", " + Session["PUBENT_StartsWith"].ToString();
                    GlobalUtils.Utils.SetCriteriaText("StartsWith", Session["PUBENT_StartsWith"].ToString()); //HITT 7074 CR-31
                }

                if (Session["PUBENT_Race"].ToString().Length > 0)
                {
                    //NCIPLCC KVPairCollection collRace = KVPair.GetKVPair("sp_NCIPL_getRace");
                    KVPairCollection collRace = KVPair.GetKVPair("sp_NCIPLCC_getRace");
                    foreach (KVPair kvItem in collRace)
                    {
                        if (string.Compare(kvItem.Key, Session["PUBENT_Race"].ToString(), true) == 0)
                        {
                            if (SearchCriteria.Length == 0)
                                SearchCriteria = kvItem.Val;
                            else
                                SearchCriteria = SearchCriteria + ", " + kvItem.Val;
                            GlobalUtils.Utils.SetCriteriaText("Race", kvItem.Val); //HITT 7074 CR-31
                        }
                    }
                }
                
                Session["PUBENT_Criteria"] = SearchCriteria;
                
                //End Code

                /*Begin CR-31 - HITT 9815 */
                GlobalUtils.Utils objUtils = new GlobalUtils.Utils();
                string QueryParams = objUtils.GetQueryStringParams();
                objUtils = null;
                /*End CR-31 - HITT 9815 */

                if (Session["PUBENT_NewOrUpdated"].ToString().Length > 0) //HITT 8300 - New & Updated now has its own page
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
