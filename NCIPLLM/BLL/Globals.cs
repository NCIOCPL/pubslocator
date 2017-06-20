using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace PubEnt.BLL
{
    //*********************************************************************
    //
    // Global Class
    //
    // Contains static properties which are used globally throughout
    // the application.
    //
    //*********************************************************************
    public class Globals
    {
        public static string DataAccessType
        {
            get
            {
                string str = ConfigurationSettings.AppSettings["DataAccessType"];
                if (str == null || str == String.Empty)
                    throw (new ApplicationException("DataAccessType configuration is missing from the web.config. It should contain  <appSettings><add key=\"DataAccessType\" value=\"data access type\" /></appSettings> "));
                else
                    return (str);
            }
        }
    }
}
