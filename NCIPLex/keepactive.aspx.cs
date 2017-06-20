using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//added
using System.Configuration;

namespace NCIPLex
{
    public partial class keepactive : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string ManualRefreshOn = ConfigurationManager.AppSettings["ManualRefreshOn"];
            int TimeOutTime = Int32.Parse(ConfigurationManager.AppSettings["AutoRefresh"]);
            
            if (string.Compare(ManualRefreshOn, "1", true) == 0)
                Response.AddHeader("Refresh", Convert.ToString(TimeOutTime));
            else
                Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) - TimeOutTime));
            
        }
    }
}
