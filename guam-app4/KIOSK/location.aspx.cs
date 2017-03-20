using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Exhibit.GlobalUtils;

namespace Kiosk
{
    public partial class location : System.Web.UI.Page
    {
        public string strPageName = "detail.aspx";
        
        protected void Page_Load(object sender, EventArgs e)
        {
            //***EAC Make sure we have CONFID and PROID, otherwise kick to default page
            ScanInputData();
            strPageName = strPageName + "?ConfID=" + Request.QueryString["ConfID"].ToString() + "&prodid=" + Request.QueryString["prodid"];

            //***EAC Hide buttons or not...Better to do on PageLoad for each page than in Master.cs!
            Master.FindControl("btnViewCart").Visible = false;
            Master.FindControl("btnSearchOther").Visible = false;
            Master.FindControl("btnFinish").Visible = false;
            Master.FindControl("lblFreePubsInfo").Visible = false;

            btnUS.Attributes.Add("onmousedown", "this.src='images/inus_on.jpg'");
            btnUS.Attributes.Add("onmouseup", "this.src='images/inus_off.jpg'");
            btnInternational.Attributes.Add("onmousedown", "this.src='images/outsideus_on.jpg'");
            btnInternational.Attributes.Add("onmouseup", "this.src='images/outsideus_off.jpg'");


            if (!Page.IsPostBack)
            {
                if (Session["KIOSK_ShipLocation"] != null)
                {
                    if (Session["KIOSK_ShipLocation"].ToString() != "")
                    {
                        Response.Redirect(strPageName, true);
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
            if (Request.QueryString["prodid"] == null)  //***EAC probably should validate for a valid prodid here (20130308)
            {
                Response.Redirect(strErrorPage, true);
            }
        }

        protected void btnUS_Click(object sender, EventArgs e)
        {
            //if (Session["KIOSK_ShipLocation"] != null)
            //{
                Session["KIOSK_ShipLocation"] = "Domestic";
                Response.Redirect(strPageName, true);
            //}
        }

        protected void btnInternational_Click(object sender, EventArgs e)
        {
            //if (Session["KIOSK_ShipLocation"] != null)
            //{
                Session["KIOSK_ShipLocation"] = "International";
                Response.Redirect(strPageName, true);
            //}
        }
    }
}
