using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

//References added
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging;

namespace NCIPLex
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {
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

            ///Possible values are "Domestic" or "International". 
            ///Shopping cart will check this session variable to 
            ///set maximum quantity limit for the order.
            Session["NCIPLEX_ShipLocation"] = "";

            /*Begin CR-28 (for detail page Back to Search Results link)*/
            Session["NCIPLEX_PageSize"] = "10";
            Session["NCIPLEX_PageSortIndex"] = "";
            /*End CR-28*/
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
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