using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

using Exhibit.BLL;
using Exhibit.GlobalUtils;
using PubEnt.DAL;
using PubEnt.GlobalUtils;

namespace PubEnt
{
    public partial class attract : System.Web.UI.Page
    {
        public string rackjavascript = "";
        public string rackImagejavascript = "";
        public string attracttimeout = "";

        public string confid = "1";//default
        
        protected void Page_Load(object sender, EventArgs e)
        {
            Utils.ResetSessions();

            //--- Set Session Conference for selected value from set up screen ---
            ScanInputData();
            if (Request.QueryString["ConfID"] != null)
            {
                confid = Request.QueryString["ConfID"];
            }
            attracttimeout = DAL2.DAL.GetTimeout(1, 1).ToString() + "000"; //***EAC DAL will guarantee that querystring will always contain an INT

            //***EAC Use this code if you have problems calling startloop()
            //System.Web.UI.HtmlControls.HtmlGenericControl ct = (System.Web.UI.HtmlControls.HtmlGenericControl)Page.Master.FindControl("MasterBody");
            //ct.Attributes.Add("onload", "startloop();");

            if (!Page.IsPostBack)
            {
                string temp;
                temp = DAL2.DAL.GetAttractPubs(int.Parse(confid)).ToString();
                //Session["KIOSK_AttractPubs"] = temp;  ***EAC SHOULD NOT BE USED!!!
                string[] attractpubs = temp.Split(',');

                string DirectoryPath = "pubimages/kioskimages/";
                string DirectoryName = Server.MapPath(DirectoryPath);
                rackjavascript = "";
                rackImagejavascript = "";

                int j = 0;
                for (int i = 0; i < attractpubs.Count(); i++)
                {
                    if (attractpubs[i].ToString() != "") 
                    {
                        //--- Validate large size publication image to see if it exists and not display if it is not.
                        rackjavascript += "rack[" + j.ToString() + "] = '" + attractpubs[i] + "';";
                        if (GlobalUtils.Utils.SearchDirectory(attractpubs[i].Trim() + ".jpg", DirectoryName) >= 1)
                        {
                            rackImagejavascript += "rackImage[" + j.ToString() + "] = '" + DirectoryPath + attractpubs[i].Trim() + ".jpg';";
                        }
                        else
                        {
                            rackImagejavascript += "rackImage[" + j.ToString() + "] = '" + DirectoryPath + "blank.jpg';";
                        }
                        j++;
                    }
                }

              
            }
        }

        protected void ScanInputData()
        {
            string strErrorPage = "default.aspx?redirect=location";

            if (Request.QueryString["ConfID"] != null)
            {
                if (!InputValidation.ContentNumVal(Request.QueryString["ConfID"].Trim()))
                {
                    Response.Redirect(strErrorPage, true);
                }
            }

        }
    }
}
