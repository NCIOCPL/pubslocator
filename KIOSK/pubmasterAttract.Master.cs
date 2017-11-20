using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using PubEnt.GlobalUtils;

namespace PubEnt
{
    public partial class pubmasterAttract : System.Web.UI.MasterPage
    {

        public string cancerjavascript = "";
        public string subjectjavascript = "";
        public string audiencejavascript = "";
        public string publicationformatjavascript = "";
        public string seriesjavascript = "";
        public string languagesjavascript = "";
        public string conferenceid = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            //***EAC EACH CALL NEEDS CONFID PARAMETER
            if (Request.QueryString["ConfID"] == null)
                throw new ArgumentException("Required parameter missing: ", "Conference ID");

            btnSearch.Attributes.Add("onmousedown", "this.src='images/search_big_on.jpg'");
            btnSearch.Attributes.Add("onmouseup", "this.src='images/search_big.jpg'");

            Session["KIOSK_TypeOfCancer"] = "";
            Session["KIOSK_Subject"] = "";
            Session["KIOSK_Audience"] = "";
            Session["KIOSK_ProductFormat"] = "";
            Session["KIOSK_Series"] = ""; //Or collection
            Session["KIOSK_Language"] = "";

            Session["KIOSK_JSCancer"] = "";
            Session["KIOSK_JSSubject"] = "";
            Session["KIOSK_JSAudience"] = "";
            Session["KIOSK_JSFormat"] = "";
            Session["KIOSK_JSSeries"] = "";
            Session["KIOSK_JSlanguage"] = "";

            if (Request.QueryString["CancerType"] != null)
                Session["KIOSK_TypeOfCancer"] = Request.QueryString["CancerType"];
            if (Request.QueryString["Subject"] != null)
                Session["KIOSK_Subject"] = Request.QueryString["Subject"];
            if (Request.QueryString["Audience"] != null)
                Session["KIOSK_Audience"] = Request.QueryString["Audience"];
            if (Request.QueryString["ProductFormat"] != null)
                Session["KIOSK_ProductFormat"] = Request.QueryString["ProductFormat"];
            if (Request.QueryString["Series"] != null)
                Session["KIOSK_Series"] = Request.QueryString["Series"];
            if (Request.QueryString["Languages"] != null)
                Session["KIOSK_Language"] = Request.QueryString["Languages"];

            conferenceid = Request.QueryString["ConfID"].ToString();
            
//            idletimeout = DAL2.DAL.GetTimeout(1, 2).ToString() + "000";

            //--- Get Cancer Type search values ---
            cancerjavascript = Utils.GetSearchCategory(int.Parse(conferenceid), "CANCERTYPE");

            //--- Get Subject search values ---
            subjectjavascript = Utils.GetSearchCategory(int.Parse(conferenceid), "SUBJECT");

            //--- Get Audience search values ---
            audiencejavascript = Utils.GetSearchCategory(int.Parse(conferenceid), "AUDIENCE");

            //--- Get Publication Format search values ---
            publicationformatjavascript = Utils.GetSearchCategory(int.Parse(conferenceid), "PUBLICATIONFORMAT");

            //--- Get Series search values ---
            seriesjavascript = Utils.GetSearchCategory(int.Parse(conferenceid), "SERIES");

            //--- Get Languages search values ---
            languagesjavascript = Utils.GetSearchCategory(int.Parse(conferenceid), "LANGUAGES");
            
        }
        
        
    }
}
