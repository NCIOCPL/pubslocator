using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NCIPLex
{
    public partial class location : System.Web.UI.Page
    {
        public string strPageName = "home.aspx";

        protected void Page_Load(object sender, EventArgs e)
        {   
            /*Begin check - A special case - hoping to catch the app pool recycling scenario*/
            if (Session["NCIPLEX_ConfId"] == null)
                Response.Redirect("conf.aspx" + "?missingconf=true", true);
            else if (string.Compare(Session["NCIPLEX_ConfId"].ToString(), "", true) == 0)
                Response.Redirect("conf.aspx" + "?missingconfid=true", true);
            //if (!Response.IsClientConnected)
            //    Response.Redirect("Default.aspx?missingvstate=true", true);
            /*End check*/

            Session["NCIPLEX_Pubs"] = "";
            Session["NCIPLEX_Qtys"] = "";
            Session["NCIPLEX_SearchKeyword"] = "";
            Session["NCIPLEX_TypeOfCancer"] = "";
            Session["NCIPLEX_Subject"] = "";
            Session["NCIPLEX_Audience"] = "";
            Session["NCIPLEX_ProductFormat"] = "";
            Session["NCIPLEX_Language"] = "";
            Session["NCIPLEX_StartsWith"] = "";
            Session["NCIPLEX_Series"] = ""; //Or collection
            Session["NCIPLEX_NewOrUpdated"] = "";
            Session["NCIPLEX_Race"] = "";

            Session["NCIPLEX_PageSize"] = "10";
            Session["NCIPLEX_PageSortIndex"] = "";

            Session["NCIPLEX_shipto"] = null;
            Session["NCIPLEX_billto"] = null;
            Session["NCIPLEX_cc"] = null;
            Session["NCIPLEX_PrinterFriendly"] = null;
            
        }

        protected void btnUS_Click(object sender, ImageClickEventArgs e)
        {
            Session["NCIPLEX_ShipLocation"] = "Domestic";
            Response.Redirect(strPageName, true);
        }

        protected void btnInternational_Click(object sender, ImageClickEventArgs e)
        {
            Session["NCIPLEX_ShipLocation"] = "International";
            Response.Redirect(strPageName, true);
        }
    }
}
