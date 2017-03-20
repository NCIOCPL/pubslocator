using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Exhibit.BLL;
using PubEnt.DAL;
using PubEnt.GlobalUtils;
using Exhibit.GlobalUtils;

namespace Kiosk
{
    public partial class kioskemailing : System.Web.UI.Page
    {
        public string shipLocation = "";
        public string strConfId = "";
        
        protected void Page_Load(object sender, EventArgs e)
        {
            //***EAC the session variable is not used anymore except this page.  I added next line.
            //Session["KIOSK_Conference"] = Request.QueryString["ConfID"];

            //Some checks - We need a good confid
            if (Request.QueryString["ConfID"] == null)
            {
                Utils.ResetSessions();
                Response.Redirect("default.aspx?redirect=kioskemailing", true);
            }
            else
                strConfId = Request.QueryString["ConfID"].ToString();

            if (strConfId.Length > 4 || strConfId == "") //potentially an intrusion
            {
                Utils.ResetSessions();
                Response.Redirect("default.aspx?redirect=kioskemailing", true);
            }
            //End of checks

            btnBacktoCart.Attributes.Add("onmousedown", "this.src='images/backcart_on.jpg'");
            btnBacktoCart.Attributes.Add("onmouseup", "this.src='images/backcart_off.jpg'");
            btnCancelOrder.Attributes.Add("onmousedown", "this.src='images/cancel_on.jpg'");
            btnCancelOrder.Attributes.Add("onmouseup", "this.src='images/cancel_off.jpg'");
            btnVerifyOrder.Attributes.Add("onmousedown", "this.src='images/verify_on.jpg'");
            btnVerifyOrder.Attributes.Add("onmouseup", "this.src='images/verify_off.jpg'");

            //***EAC Hide buttons or not...Better to do on PageLoad for each page than in Master.cs!
            //Master.FindControl("btnViewCart").Visible = false;
            //Master.FindControl("btnSearchOther").Visible = false;
            //Master.FindControl("btnFinish").Visible = false;
            Master.FindControl("lblFreePubsInfo").Visible = false;

            shipLocation = Session["KIOSK_ShipLocation"].ToString();
                      
            if (!Page.IsPostBack)
            {
                setShippingAddress();//no idea what this is here
                BindContactInfo();
            }
            else
            {
                ScanInputData();
            }
        }

        protected void ScanInputData()
        {
            string strErrorPage = "default.aspx?redirect=kioskemailing";

            if (!InputValidation.LenVal(this.txtEMail.Value, 40))
            {
                Response.Redirect(strErrorPage, true);
            }

        }
            
        protected void setShippingAddress()
        {
            //TODO: restore email address here maybe?
           
        }

        protected void BindContactInfo()
        {
            if (Session["KIOSK_Email"] != null)
            {
                if (Session["KIOSK_Email"].ToString().Length != 0)
                {
                    this.txtEMail.Value = Server.HtmlDecode(Session["KIOSK_Email"].ToString());
                }
            }
        }

        protected void txtZip_TextChanged(object sender, EventArgs e)
        {
        }

       

        protected void cusvaltxtZIP_ServerValidate(object source, ServerValidateEventArgs args)
        {
            /*if (this.rdbtnDomestic.Checked)
            {
                if (GetCity(this.txtZip.Text))
                {
                    args.IsValid = true;
                }
                else
                {
                    args.IsValid = false;
                }
            }*/
        }

        protected void btnVerifyOrder_Click(object sender, ImageClickEventArgs e)
        {
            string strPageName = "verify_emailing.aspx";
            //if (Session["KIOSK_Conference"] != null)
            //{
            strPageName = strPageName + "?ConfID=" + Server.HtmlDecode(strConfId);
            //}
            SetContactInfo();
            Response.Redirect(strPageName, true);

        }

        protected void ClearContactInfo()
        {
            if (Session["KIOSK_Name"] != null)
            {
                Session["KIOSK_Name"] = "";
            }

            if (Session["KIOSK_Organization"] != null)
            {
                Session["KIOSK_Organization"] = "";
            }

            if (Session["KIOSK_Address1"] != null)
            {
                Session["KIOSK_Address1"] = "";
            }

            if (Session["KIOSK_Address2"] != null)
            {
                Session["KIOSK_Address2"] = "";
            }

            if (Session["KIOSK_ZIPCode"] != null)
            {
                Session["KIOSK_ZIPCode"] = "";
            }

            if (Session["KIOSK_ZIPPlus4"] != null)
            {
                Session["KIOSK_ZIPPlus4"] = "";
            }

            if (Session["KIOSK_City"] != null)
            {
                Session["KIOSK_City"] = "";
            }

            if (Session["KIOSK_State"] != null)
            {
                Session["KIOSK_State"] = "";
            }

            if (Session["KIOSK_Country"] != null)
            {
                Session["KIOSK_Country"] = "";
            }

            if (Session["KIOSK_Email"] != null)
            {
                Session["KIOSK_Email"] = "";
            }

            if (Session["KIOSK_Phone"] != null)
            {
                Session["KIOSK_Phone"] = "";
            }
        }

        protected void SetContactInfo()
        {            
            if (Session["KIOSK_Email"] != null)
            {
                Session["KIOSK_Email"] = Server.HtmlEncode(this.txtEMail.Value.Trim());
            }
        }

        protected void btnCancelOrder_Click(object sender, ImageClickEventArgs e)
        {
            /*string strPageName = "kiosksearch.aspx";
            if (Session["KIOSK_Conference"] != null)
            {
                strPageName = strPageName + "?ConfID=" + Server.HtmlDecode(Session["KIOSK_Conference"].ToString());
            }

            if (Session["KIOSK_TypeOfCancer"] != null)
            {
                strPageName = strPageName + "&CancerType=" + Server.HtmlDecode(Session["KIOSK_TypeOfCancer"].ToString());
            }

            if (Session["KIOSK_Subject"] != null)
            {
                strPageName = strPageName + "&Subject=" + Server.HtmlDecode(Session["KIOSK_Subject"].ToString());
            }

            if (Session["KIOSK_Audience"] != null)
            {
                strPageName = strPageName + "&Audience=" + Server.HtmlDecode(Session["KIOSK_Audience"].ToString());
            }

            if (Session["KIOSK_ProductFormat"] != null)
            {
                strPageName = strPageName + "&ProductFormat=" + Server.HtmlDecode(Session["KIOSK_ProductFormat"].ToString());
            }

            if (Session["KIOSK_Series"] != null)
            {
                strPageName = strPageName + "&Series=" + Server.HtmlDecode(Session["KIOSK_Series"].ToString());
            }

            if (Session["KIOSK_Language"] != null)
            {
                strPageName = strPageName + "&Languages=" + Server.HtmlDecode(Session["KIOSK_Language"].ToString());
            }*/

            string strPageName = "attract.aspx";
            //if (Session["KIOSK_Conference"] != null)
            //{
            strPageName = strPageName + "?ConfID=" + Server.HtmlDecode(strConfId);
            //}
            
            ClearContactInfo();
            Utils.ResetSessions();
            Response.Redirect(strPageName, true);
        }

        protected void btnBacktoCart_Click(object sender, ImageClickEventArgs e)
        {
            string strPageName = "cart.aspx";
            //if (Session["KIOSK_Conference"] != null)
            //{
                strPageName = strPageName + "?ConfID=" + Server.HtmlDecode(strConfId);
            //}

            SetContactInfo();
            Response.Redirect(strPageName, true);
        }


    }
}
