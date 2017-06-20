using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PubEnt
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //JPJ Moved Initialization code to global asax file, to fix the session resetting problem experienced with IE on the server.
            ////***EAC Create the session variables asap
            //Session["NCIPL_Pubs"] = "";
            //Session["NCIPL_Qtys"] = "";
            //Session["PUBENT_SearchKeyword"] = "";
            //Session["PUBENT_TypeOfCancer"] = "";
            //Session["PUBENT_Subject"] = "";
            //Session["PUBENT_Audience"] = "";
            //Session["PUBENT_ProductFormat"] = "";
            //Session["PUBENT_Language"] = "";
            //Session["PUBENT_StartsWith"] = "";
            //Session["PUBENT_Series"] = ""; //Or collection
            //Session["PUBENT_NewOrUpdated"] = "";
            //Session["PUBENT_Race"] = "";

            //Newly added - test with caution
            if (Session["JSTurnedOn"] == null)
            {
                Session["JSTurnedOn"] = "True"; //Assuming JavaScript is enabled, by default.
                Session["NCIPL_Pubs"] = "";
                Session["NCIPL_Qtys"] = "";
                Session["PUBENT_SearchKeyword"] = "";
                Session["PUBENT_TypeOfCancer"] = "";
                Session["PUBENT_Subject"] = "";
                Session["PUBENT_Audience"] = "";
                Session["PUBENT_ProductFormat"] = "";
                Session["PUBENT_Language"] = "";
                Session["PUBENT_StartsWith"] = "";
                Session["PUBENT_Series"] = ""; //Or collection
                Session["PUBENT_NewOrUpdated"] = "";
                Session["PUBENT_Race"] = "";
                Session["PUBENT_CannedSearch"] = "";
            }

        }
    }
}
