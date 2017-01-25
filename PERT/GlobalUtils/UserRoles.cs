using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PubEnt.GlobalUtils
{
    public class UserRoles
    {

        public static int getLoggedInUserRole()
        {
            int iRole = -1; //default
            if (HttpContext.Current.Session["NCIPL_Role"] != null)
            {
                if (string.Compare(HttpContext.Current.Session["NCIPL_Role"].ToString(), "NCIPL_PUBLIC", true) == 0)
                    iRole = 0; //Public Users - Not valid for NCIPL_CC
                else if (string.Compare(HttpContext.Current.Session["NCIPL_Role"].ToString(), "NCIPL_CC", true) == 0)
                    iRole = 1; //Regular Contact Center User
                else if (string.Compare(HttpContext.Current.Session["NCIPL_Role"].ToString(), "NCIPL_LM", true) == 0)
                    iRole = 2; //Lockheed Martin Employee
                else if (string.Compare(HttpContext.Current.Session["NCIPL_Role"].ToString(), "NCIPL_POS", true) == 0)
                    iRole = 3; //POS - Not sure whether this role will be present
                else if (string.Compare(HttpContext.Current.Session["NCIPL_Role"].ToString(), "NCIPL_PO", true) == 0)
                    iRole = 4; //Supposedly Project Office
            }
            return iRole; 
        }

        public static string getLoggedInUserId()
        {
            string uid = "";
            if (HttpContext.Current.Session["NCIPL_User"] != null)
            {
                uid = HttpContext.Current.Session["NCIPL_User"].ToString();
            }
            return uid;
        }
    }
}
