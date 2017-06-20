using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections;

namespace Exhibit.GlobalUtils
{
    public class InputValidation
    {
        public static ArrayList arrlst;

        public static bool LenVal(string s, int len)
        {
            return (s.Length <= len) ? true : false;
        }

        public static bool ContentVal(string s, string regExp)
        {
            return System.Text.RegularExpressions.Regex.
                IsMatch(s, regExp);
        }

        public static bool ContentNumVal(string s)
        {
            if (s.Length > 0)
                return ContentVal(s, @"^\d*[0-9](\.\d*[0-9])?$");
            else
                return true;
        }

        public static bool ContentDateVal(string s)
        {
            try
            {
                DateTime t_dt = DateTime.Parse(s);
                return true;
            }
            catch (FormatException fex)
            {
                return false;
            }
        }

        public static bool OtherVal(string s)
        {
            arrlst = new ArrayList();
            arrlst.Add("<applet");
            arrlst.Add("<body");
            arrlst.Add("<embed");
            arrlst.Add("<frame");
            arrlst.Add("<script");
            arrlst.Add("<frameset");
            arrlst.Add("<html");
            arrlst.Add("<iframe");
            arrlst.Add("<img");
            arrlst.Add("<style");
            arrlst.Add("<layer");
            arrlst.Add("<link");
            arrlst.Add("<ilayer");
            arrlst.Add("<meta");
            arrlst.Add("<object");

            foreach (object o in arrlst)
            {
                int pos = s.IndexOf(o.ToString());
                if (pos >= 0)
                {
                    if (s.IndexOf("\"", 0, pos) < 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool SpecialVal(string val)
        {
            if (System.Text.RegularExpressions.Regex.
                IsMatch(val, "('(or|and).+?'='.+?)|('(or|and)[0-9]+=[0-9]+)"))
            {
                return true;
            }

            return false;

        }
    }
}
