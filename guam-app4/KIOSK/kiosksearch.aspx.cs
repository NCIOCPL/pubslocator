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

namespace Kiosk
{
    public partial class kiosksearch : System.Web.UI.Page
    {
        public string rackjavascript = "";
        public string rackImagejavascript = "";
        public string confid = "1";//default
        public string extratext = "";


        protected void Page_Load(object sender, EventArgs e)
        {
            ScanInputData();

            confid = Request.QueryString["ConfID"];

            //***EAC Hide ViewCart button or not...Better to do on PageLoad for each page than in Master.cs!
            if (Session["KIOSK_Qtys"].ToString() != "")
                Master.FindControl("btnViewCart").Visible = true;
            else
                Master.FindControl("btnViewCart").Visible = false;
            Master.FindControl("btnFinish").Visible = false;

            //Session["KIOSK_TypeOfCancer"] = "";
            //Session["KIOSK_Subject"] = "";
            //Session["KIOSK_Audience"] = "";
            //Session["KIOSK_ProductFormat"] = "";
            //Session["KIOSK_Series"] = ""; //Or collection
            //Session["KIOSK_Language"] = "";

            if (Request.QueryString["CancerType"] != null)
                Session["KIOSK_TypeOfCancer"] = Request.QueryString["CancerType"];
            if (Request.QueryString["Subject"] != null)
                Session["KIOSK_Subject"] = Request.QueryString["Subject"];
            if (Request.QueryString["Audience"] != null)
                Session["KIOSK_Audience"] = Request.QueryString["Audience"];
            if (Request.QueryString["ProductFormat"] != null)
                Session["KIOSK_ProductFormat"] = Request.QueryString["ProductFormat"];
            if (Request.QueryString["Series"] != null)
                Session["KIOSK_Series"] = Request.QueryString["Series"];
            if (Request.QueryString["Languages"] != null)
                Session["KIOSK_Language"] = Request.QueryString["Languages"];
            if (Request.QueryString["st"] != null)
            {
                PubEnt.GlobalUtils.Utils UtilMethodClean = new PubEnt.GlobalUtils.Utils();
                Session["KIOSK_Extratext"] = " for " + UtilMethodClean.Clean(Request.QueryString["st"]);
            }
            extratext = Session["KIOSK_Extratext"].ToString();

            if (!Page.IsPostBack)
            {

                Confs PubsList = new Confs();
                PubsList = PubEnt.DAL.DAL.GetSearchPubs(int.Parse(confid), Session["KIOSK_TypeOfCancer"].ToString(),
                                                            Session["KIOSK_Subject"].ToString(), Session["KIOSK_Audience"].ToString(),
                                                            Session["KIOSK_ProductFormat"].ToString(), Session["KIOSK_Series"].ToString(),
                                                            Session["KIOSK_Language"].ToString());

                string DirectoryPath = "pubimages/kioskimages/";
                string DirectoryName = Server.MapPath(DirectoryPath);
                rackjavascript = "";
                rackImagejavascript = "";

                int i = 0;
                if (PubsList.Count > 0)
                {
                    foreach (Conf PubsListItem in PubsList)
                    {
                        //--- Validate large size publication image to see if it exists and not display if it is not.
                        rackjavascript += "rack[" + i.ToString() + "] = '" + PubsListItem.ConfName + "';";
                        if (PubEnt.GlobalUtils.Utils.SearchDirectory(PubsListItem.ConfName.Trim() + ".jpg", DirectoryName) >= 1)
                        {
                            rackImagejavascript += "rackImage[" + i.ToString() + "] = '" + DirectoryPath + PubsListItem.ConfName.Trim() + ".jpg';";
                        }
                        else
                        {
                            rackImagejavascript += "rackImage[" + i.ToString() + "] = '" + DirectoryPath + "blank.jpg';";
                        }
                        i++;
                    }
                    
                }

            }
        }

        protected void ScanInputData()
        {
            string strErrorPage = "default.aspx?redirect=kiosksearch";

            if (Request.QueryString["ConfID"] != null)
            {
                if (!InputValidation.ContentNumVal(Request.QueryString["ConfID"].Trim()))
                {
                    Response.Redirect(strErrorPage, true);
                }
            }
            
            if (Request.QueryString["CancerType"] != null)
            {
                if (!InputValidation.ContentNumVal(Request.QueryString["CancerType"].Trim()))
                {
                    Response.Redirect(strErrorPage, true);
                }
            }

            if (Request.QueryString["Subject"] != null)
            {
                if (!InputValidation.ContentNumVal(Request.QueryString["Subject"].Trim()))
                {
                    Response.Redirect(strErrorPage, true);
                }
            }

            if (Request.QueryString["Audience"] != null)
            {
                if (!InputValidation.ContentNumVal(Request.QueryString["Audience"].Trim()))
                {
                    Response.Redirect(strErrorPage, true);
                }
            }

            if (Request.QueryString["ProductFormat"] != null)
            {
                if (!InputValidation.ContentNumVal(Request.QueryString["ProductFormat"].Trim()))
                {
                    Response.Redirect(strErrorPage, true);
                }
            }

            if (Request.QueryString["Series"] != null)
            {
                if (!InputValidation.ContentNumVal(Request.QueryString["Series"].Trim()))
                {
                    Response.Redirect(strErrorPage, true);
                }
            }

            if (Request.QueryString["Languages"] != null)
            {
                if (!InputValidation.ContentNumVal(Request.QueryString["Languages"].Trim()))
                {
                    Response.Redirect(strErrorPage, true);
                }
            }

        }
    }
}
