using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using PubEnt;
using PubEnt.DAL;
using PubEnt.BLL;
using PubEnt.GlobalUtils;
using System.Collections;
using System.Configuration;

using System.Net;
using System.Xml;
using Kiosk.AddressValidationServiceWebReference;
using Microsoft.Practices.EnterpriseLibrary.Logging;


namespace Kiosk
{
    public partial class verify_emailing : System.Web.UI.Page
    {
        public string overridejavascript = "";
        string ShipLocation = "";
        int ConfId = 0;
        
        //Shipping variables
        string kiosk_name = "";
        string kiosk_organization = "";
        string kiosk_address1 = "";
        string kiosk_address2 = "";
        string kiosk_zipcode = ""; //Used for postal code if international
        string kiosk_zipplus4 = "";
        string kiosk_city = "";
        string kiosk_state = ""; //Used for province if international
        string kiosk_country = ""; //Used for international order
        string kiosk_phone = "";
        string kiosk_email = "";


        //Billing variables - will be empty [used for billto person object]
        string kiosk_billtoname = "";
        string kiosk_billtoorganization = "";
        string kiosk_billtoaddress1 = "";
        string kiosk_billtoaddress2 = "";
        string kiosk_billtozipcode = ""; //Used for postal code if international
        string kiosk_billtozipplus4 = "";
        string kiosk_billtocity = "";
        string kiosk_billtostate = ""; //Used for province if international
        string kiosk_billtocountry = ""; //Used for international order
        string kiosk_billtophone = "";
        string kiosk_billtoemail = "";



        Person billto
        {
            get
            {
                if (Session["KIOSK_Name"] == null)
                    return null;
                else
                    return this.GetBillPerson();
            }
            //set { Session["NCIPL_billto"] = value; }
        }
        //Person shipto
        //{
        //    get
        //    {
        //        if (Session["NCIPL_shipto"] == null)
        //            return null;
        //        else
        //            return (Person)Session["NCIPL_shipto"];
        //    }
        //    set { Session["NCIPL_shipto"] = value; }
        //}
        Person shipto
        {
            get
            {
                if (HttpContext.Current.Session["KIOSK_Name"] == null)
                    return null;
                else
                {
                    return this.GetShipPerson();

                }
            }
            //set { Session["KIOSK_shipto"] = value; }
        }
        //CreditCard cc
        //{
        //    get
        //    {
        //        if (Session["KIOSK_cc"] == null)
        //            return null;
        //        else
        //            return (CreditCard)Session["KIOSK_cc"];
        //    }
        //    set { Session["KIOSK_cc"] = value; }
        //}
        ProductCollection shoppingcart
        {
            get
            {
                if (HttpContext.Current.Session["KIOSK_cart"] == null)
                    return null;
                else
                    return (ProductCollection)HttpContext.Current.Session["KIOSK_cart"];
            }
            set { HttpContext.Current.Session["KIOSK_cart"] = value; }
        }
        //ProductCollection shoppingcartV2   //***EAC 2nd version of shopping cart w/ virtual/linked pubs
        //{
        //    get
        //    {
        //        if (Session["NCIPL_cartV2"] == null)
        //            return null;
        //        else
        //            return (ProductCollection)Session["NCIPL_cartV2"];
        //    }
        //    set { Session["NCIPL_cartV2"] = value; }
        //}

        //JPJ FOR KIOSK keep shoppingcart and shoppingcartV2 the same since Exhibits do not have kits
        ProductCollection shoppingcartV2   //***EAC 2nd version of shopping cart w/ virtual/linked pubs
        {
            get
            {
                if (Session["KIOSK_cart"] == null)
                    return null;
                else
                    return (ProductCollection)Session["KIOSK_cart"];
            }
            set { Session["KIOSK_cart"] = value; }
        }

        KVPairCollection Nerdos = new KVPairCollection();

        protected void Page_Load(object sender, EventArgs e)
        {

            //Some checks - We need a good confid
            string strConfId = "";
            if (Request.QueryString["confid"] == null)
            {  
                Utils.ResetSessions();
                Response.Redirect("default.aspx?redirect=verify", true);
            }
            else
                strConfId = Request.QueryString["confid"].ToString();

            if (strConfId.Length > 4 || strConfId == "") //potentially an intrusion
            {
                Utils.ResetSessions();
                Response.Redirect("default.aspx?redirect=verify", true);
            }
            else
            {
                ConfId = Int32.Parse(strConfId);
            }
            //End of initial checks

            ImgBtnChangeEmail.Attributes.Add("onmousedown", "this.src='images/changeemail_on.jpg'");
            ImgBtnChangeEmail.Attributes.Add("onmouseup", "this.src='images/changeemail_off.jpg'");
            ImgBtnBacktoCart.Attributes.Add("onmousedown", "this.src='images/backcart_on.jpg'");
            ImgBtnBacktoCart.Attributes.Add("onmouseup", "this.src='images/backcart_off.jpg'");
            ImgBtnCancelOrder.Attributes.Add("onmousedown", "this.src='images/cancel_on.jpg'");
            ImgBtnCancelOrder.Attributes.Add("onmouseup", "this.src='images/cancel_off.jpg'");
            ImgBtnPlaceOrder.Attributes.Add("onmousedown", "this.src='images/placeorder_on.jpg'");
            ImgBtnPlaceOrder.Attributes.Add("onmouseup", "this.src='images/placeorder_off.jpg'");

            //***EAC Hide buttons or not...Better to do on PageLoad for each page than in Master.cs!
            Master.FindControl("btnViewCart").Visible = false;
            Master.FindControl("btnSearchOther").Visible = false;
            Master.FindControl("btnFinish").Visible = false;
            Master.FindControl("lblFreePubsInfo").Visible = false;

            this.divVerifyErrDisplay.Visible = false;

            if (this.shoppingcart == null)
            {
                Utils.ResetSessions();
                Response.Redirect("attract.aspx?ConfID=" + strConfId, true);
            }
            
            if (this.shoppingcart.Count < 1)
            {  //***EAC I need a shopping cart at this point
                Utils.ResetSessions();
                Response.Redirect("attract.aspx?ConfID=" + strConfId, true);
            }

            //Check whether the conference is domestic or international
            
            if (Session["KIOSK_ShipLocation"] != null)
            {
                if (Session["KIOSK_ShipLocation"].ToString().Length > 0)
                {
                    if (string.Compare(Session["KIOSK_ShipLocation"].ToString(), "Domestic", true) == 0)
                        ShipLocation = "Domestic";
                    else if (string.Compare(Session["KIOSK_ShipLocation"].ToString(), "International", true) == 0)
                        ShipLocation = "International";
                }
            }

            if (ShipLocation.Length == 0)
            {
                Utils.ResetSessions();
                //Response.Redirect("default.aspx?missingsession=true", true);
                Response.Redirect("attract.aspx?ConfID=" + strConfId, true);
            }
            
            if (!IsPostBack)
            {
                HttpContext.Current.Session["KIOSK_DONE"] = "false";
                this.SetAddressValues();

                grdItems.DataSource = this.shoppingcartV2;
                grdItems.DataBind();


                lblTot.Text = "";   // this.shoppingcartV2.TotalQtyForShipping.ToString();
                ImgBtnCancelOrder.Attributes.Add("onclick", "return fnAskBeforeCancelling(" + "'" + ImgBtnCancelOrder.ClientID + "')");

            }

        }

        ///Creates the person object from KIOSK Shipping Session Variables
        private Person GetShipPerson()
        {

            if (HttpContext.Current.Session["KIOSK_Name"] != null)
                kiosk_name = HttpContext.Current.Session["KIOSK_Name"].ToString();
            if (HttpContext.Current.Session["KIOSK_Organization"] != null)
                kiosk_organization = HttpContext.Current.Session["KIOSK_Organization"].ToString();
            if (HttpContext.Current.Session["KIOSK_Address1"] != null)
                kiosk_address1 = HttpContext.Current.Session["KIOSK_Address1"].ToString();
            if (HttpContext.Current.Session["KIOSK_Address2"] != null)
                kiosk_address2 = HttpContext.Current.Session["KIOSK_Address2"].ToString();
            if (HttpContext.Current.Session["KIOSK_ZIPCode"] != null)
                kiosk_zipcode = HttpContext.Current.Session["KIOSK_ZIPCode"].ToString();
            if (HttpContext.Current.Session["KIOSK_ZIPPlus4"] != null)
                kiosk_zipplus4 = HttpContext.Current.Session["KIOSK_ZIPPlus4"].ToString();
            if (HttpContext.Current.Session["KIOSK_City"] != null)
                kiosk_city = HttpContext.Current.Session["KIOSK_City"].ToString();
            if (HttpContext.Current.Session["KIOSK_State"] != null)
                kiosk_state = HttpContext.Current.Session["KIOSK_State"].ToString();
            if (HttpContext.Current.Session["KIOSK_Country"] != null)
                kiosk_country = HttpContext.Current.Session["KIOSK_Country"].ToString();
            if (HttpContext.Current.Session["KIOSK_Email"] != null)
                kiosk_email = HttpContext.Current.Session["KIOSK_Email"].ToString();
            if (HttpContext.Current.Session["KIOSK_Phone"] != null)
                kiosk_phone = HttpContext.Current.Session["KIOSK_Phone"].ToString();

            int id = 1;

            return new Person(id,
                                kiosk_name,
                                kiosk_organization,
                                kiosk_email,
                                kiosk_address1,
                                kiosk_address2,
                                kiosk_city,
                                kiosk_state,
                                kiosk_zipcode,
                                kiosk_zipplus4,
                                kiosk_phone,
                                kiosk_country
                                    );

        }

        private Person GetBillPerson()
        {
            int id = 2;
            return new Person (id,
                                kiosk_billtoname,
                                kiosk_billtoorganization,
                                kiosk_billtoemail,
                                kiosk_billtoaddress1,
                                kiosk_billtoaddress2,
                                kiosk_billtocity,
                                kiosk_billtostate,
                                kiosk_billtozipcode,
                                kiosk_billtozipplus4,
                                kiosk_billtophone,
                                kiosk_billtocountry
                                    );  
        }

        private void SetAddressValues()
        {
            EmailLabel.Text = "E-mail";
            EmailText.Text = shipto.Email;
            if (EmailText.Text == "")
                divEmail.Visible = false;
        }

        protected void grdItems_IDB(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                Product p = (Product)e.Item.DataItem;

                Label lblTitle = (Label)e.Item.FindControl("lblTitle");
                Label lblQty = (Label)e.Item.FindControl("lblQty");
                Label lblDetails = (Label)e.Item.FindControl("lblDetails");

                lblTitle.Text = p.LongTitle;
                lblQty.Text = "&nbsp";  //***we already know all cart items are ONLINE-ONLY 
                //lblDetails.Text = "(E-mail)";

            }
        }

        protected void Nerdos_IDB(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                KVPair kv = (KVPair)e.Item.DataItem;
                Label lblNerdoTitle = (Label)e.Item.FindControl("lblNerdoTitle");
                lblNerdoTitle.Text = kv.Key;
                HyperLink lnkCanType = (HyperLink)e.Item.FindControl("lnkNerdo");

                lnkCanType.Text = kv.Val;
                lnkCanType.NavigateUrl = kv.Val;
            }
        }

        protected void btnCancelOrder_Click(object sender, EventArgs e)
        {
            Utils.ResetSessions();
            Response.Redirect("attract.aspx?ConfID=" + Request.QueryString["ConfID"].ToString(), true);
        }

        protected void btnPlaceOrder_Click(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["KIOSK_DONE"].ToString() == "true")
            {
                Utils.ResetSessions();
                //Response.Redirect("default.aspx");
                Response.Redirect("attract.aspx?ConfID=" + Request.QueryString["ConfID"].ToString(), true);
            }

            Transaction t = new Transaction(this.shipto, this.billto, null, this.shoppingcart);

            // ALL SET -- SAVE THE ORDER
            int outputvalue = 0;
            int returnordernum = 0;
            t.Save(Request.UserHostAddress, ConfId, out outputvalue, out returnordernum);

            //Change to Confirmation
            HttpContext.Current.Session["KIOSK_DONE"] = "true";
            this.shoppingcartV2 = null;
            this.shoppingcart = null;
            pnlVerifyHeader.Visible = false;
            pnlConfirmHeader.Visible = true;
            ImgBtnChangeEmail.Visible = false;
            pnlVerifyButtons.Visible = false;

            //Reset Session Variables
            Utils.ResetSessions();

            //--- Modified by Vu Tran 01/31/2010
            Master.FindControl("btnFinish").Visible = true;
            overridejavascript = "function expiredprompt(){gobackAttractScreen(); }";
        }
       
        protected void btnChangeShippingInfo_Click(object sender, EventArgs e)
        {
            Response.Redirect("kioskemailing.aspx?ConfID=" + ConfId.ToString().ToString(), true);
        }

        protected void ImgBtnBacktoCart_Click(object sender, ImageClickEventArgs e)
        {
            //TODO: Modify if shipping expects any other parameter
            Response.Redirect("cart.aspx?ConfID=" + ConfId.ToString(), true); //EAC assume ConfID is supplied to all pages
        }

        protected void btnChangeE_mail(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("kioskemailing.aspx?ConfID=" + ConfId.ToString().ToString(), true);
        }


    }
}
