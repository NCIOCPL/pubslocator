///The only purpose of the form is to call the report generator dll
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

///Add reference to PubEntReports.dll
using PubEntReports.DAL;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace PubEntAdmin
{
    public partial class reportform : System.Web.UI.Page
    {
        private string strpubid = "";
        private int pubid = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["pubid"] != null)
            {
                strpubid = Request.QueryString["pubid"].ToString();

                //Some Hail Storm checks
                if (strpubid.Length == 0 || strpubid.Length > 5)
                {
                    //Write to log
                    LogEntry logEnt = new LogEntry();
                    string logmessage = "\r\n";
                    logmessage += "PubEntReports: " + "Invalid Publication Id." + "\r\n";
                    logEnt.Message = logmessage;
                    Logger.Write(logEnt, "Logs");

                    Response.Redirect("PEAUHErrorScreen.htm");
                    Response.End();
                }

                try
                {
                    pubid = Int32.Parse(strpubid);
                }
                catch (Exception Ex)
                {
                    //Write to log
                    LogEntry logEnt = new LogEntry();
                    string logmessage = "\r\n";
                    logmessage += "PubEntReports: " + "Parsing failed." + "\r\n";
                    logmessage += "PubEntReports: " + Ex.Message + "\n\n" + Ex.StackTrace + "\r\n";
                    logEnt.Message = logmessage;
                    Logger.Write(logEnt, "Logs");

                    Response.Redirect("PEAUHErrorScreen.htm");
                    Response.End();
                }

                if (pubid == 0)
                {
                    //Write to log
                    LogEntry logEnt = new LogEntry();
                    string logmessage = "\r\n";
                    logmessage += "PubEntReports: " + "Publication Id is not valid." + "\r\n";
                    logEnt.Message = logmessage;
                    Logger.Write(logEnt, "Logs");

                    Response.Redirect("PEAUHErrorScreen.htm");
                    Response.End();
                }

                //Everything looks good at this point, proceed with the report
                SQLDataAcccess DBAccess = new SQLDataAcccess(pubid);

            }
            else
            {
                Response.Write("Publication Id not provided.");
            }
        }
    }
}
