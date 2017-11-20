using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

//References added
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging;

namespace PubEnt
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {
		Session["NCIPL_DEMOGRAPHICSDONE"] = "false";

            //***EAC Create the session variables asap
            Session["NCIPL_User"] = "";
            Session["NCIPL_Role"] = "";
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
            
            /*Begin CR-28 (for detail page Back to Search Results link)*/
            Session["PUBENT_PageSize"] = "10";
            Session["PUBENT_PageSortIndex"] = "";
            /*End CR-28*/

            Session["NCIPL_REGISTERREFERRER"] = "~/home.aspx"; //Feb 15, 2012 - NCIPL_CC default valueSE

            //Search Order
            Session["SEARCHORDER_CUSTOMERTYPE"] = "";
            Session["SEARCHORDER_CUSTOMERTYPEDESC"] = "";
            Session["SEARCHORDER_KEYWORD"] = "";
            Session["SEARCHORDER_CURRPAGE"] = "";
            Session["SEARCHORDER_CUSTID"] = "";

            Session["SEARCHORDER_SDATE"] = "";
            Session["SEARCHORDER_EDATE"] = "";

            Session["VIEW_CUSTID"] = "";
            Session["VIEW_ORDERNUM"] = "";
            Session["VIEW_PREVPAGE"] = ""; 

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
#if DEBUG
            return;
#endif
            //Code here for logging the unhandled exceptions
            Exception Ex = Server.GetLastError().GetBaseException();
            ExceptionPolicy.HandleException(Ex, "Exception Policy");
            //Server.ClearError();
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}