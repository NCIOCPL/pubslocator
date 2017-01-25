using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PubEnt.BLL;
using PubEnt.GlobalUtils;
using System.Configuration;
using PubEnt.DAL;

namespace PubEnt
{
    public partial class pubmasterShipping : System.Web.UI.MasterPage
    {
        public string idletimeout = "";
        public string idle2timeout = "";
        public string cancerjavascript = "";
        public string subjectjavascript = "";
        public string audiencejavascript = "";
        public string publicationformatjavascript = "";
        public string seriesjavascript = "";
        public string languagesjavascript = "";
        public string conferenceid = "";
        public string viewcartjavascript = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            Label1.Text = Session["KIOSK_Qtys"].ToString();
            //***EAC EACH CALL NEEDS CONFID PARAMETER
            if (Request.QueryString["ConfID"] == null)
                throw new ArgumentException("Required parameter missing: ", "Conference ID");


            conferenceid = Request.QueryString["ConfID"].ToString();

            idletimeout = DAL2.DAL.GetTimeout(1, 2).ToString() + "000";
            idle2timeout = DAL2.DAL.GetTimeout(1, 3).ToString();

            if (Session["KIOSK_JSCancer"] == null || Session["KIOSK_JSCancer"].ToString() == "")
            {
                cancerjavascript = Utils.GetSearchCategory(int.Parse(conferenceid), "CANCERTYPE");
                Session["KIOSK_JSCancer"] = cancerjavascript;
            }
            else
                cancerjavascript = Session["KIOSK_JSCancer"].ToString();

            if (Session["KIOSK_JSSubject"] == null || Session["KIOSK_JSSubject"].ToString() == "")
            {
                subjectjavascript = Utils.GetSearchCategory(int.Parse(conferenceid), "SUBJECT");
                Session["KIOSK_JSSubject"] = subjectjavascript;
            }
            else
                subjectjavascript = Session["KIOSK_JSSubject"].ToString();

            if (Session["KIOSK_JSAudience"] == null || Session["KIOSK_JSAudience"].ToString() == "")
            {
                audiencejavascript = Utils.GetSearchCategory(int.Parse(conferenceid), "AUDIENCE");
                Session["KIOSK_JSAudience"] = audiencejavascript;
            }
            else
                audiencejavascript = Session["KIOSK_JSAudience"].ToString();

            if (Session["KIOSK_JSFormat"] == null || Session["KIOSK_JSFormat"].ToString() == "")
            {
                publicationformatjavascript = Utils.GetSearchCategory(int.Parse(conferenceid), "PUBLICATIONFORMAT");
                Session["KIOSK_JSFormat"] = publicationformatjavascript;
            }
            else
                publicationformatjavascript = Session["KIOSK_JSFormat"].ToString();

            if (Session["KIOSK_JSSeries"] == null || Session["KIOSK_JSSeries"].ToString() == "")
            {
                seriesjavascript = Utils.GetSearchCategory(int.Parse(conferenceid), "SERIES");
                Session["KIOSK_JSSeries"] = seriesjavascript;
            }
            else
                seriesjavascript = Session["KIOSK_JSSeries"].ToString();

            if (Session["KIOSK_JSlanguage"] == null || Session["KIOSK_JSlanguage"].ToString() == "")
            {
                languagesjavascript = Utils.GetSearchCategory(int.Parse(conferenceid), "LANGUAGES");
                Session["KIOSK_JSlanguage"] = languagesjavascript;
            }
            else
                languagesjavascript = Session["KIOSK_JSlanguage"].ToString();
            #region Update View Cart Button Text -- dirty but it works
            string temp = "";
            try
            {

                if (Session["KIOSK_Qtys"].ToString().Trim() != "")
                {
                    int tot = 0;
                    string[] qtys = Session["KIOSK_Qtys"].ToString().Split(new Char[] { ',' });
                    for (int i = 0; i < qtys.Length - 1; i++)
                    {
                        tot += int.Parse(qtys[i]);
                    }
                    temp = " (" + tot.ToString();
                    if (Session["KIOSK_ShipLocation"].ToString() == "Domestic")
                        temp += " of " + ConfigurationManager.AppSettings["DomesticOrderLimit"];
                    if (Session["KIOSK_ShipLocation"].ToString() == "International")
                        //temp += " of " + ConfigurationManager.AppSettings["InternationalOrderLimit"];
                        temp += " of " + PubEnt.DAL.DAL.GetIntl_MaxOrder(int.Parse(conferenceid));
                    temp += ")";
                }

            }
            catch (Exception)
            {
            }
            //btnViewCart.Value = "View Cart" + temp;

            #endregion

        }
    }
}
