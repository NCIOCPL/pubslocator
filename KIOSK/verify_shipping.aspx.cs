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
    public partial class verify_shipping : System.Web.UI.Page
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


            ImgBtnChangeShipInfo.Attributes.Add("onmousedown", "this.src='images/changeship_on.jpg'");
            ImgBtnChangeShipInfo.Attributes.Add("onmouseup", "this.src='images/changeship_off.jpg'");
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

            #region dummyvalues -- Please remove at integration -- it should come from shipping

            //Shipping Information


            //HttpContext.Current.Session["KIOSK_Name"] = "Jikku P Jacob";
            //HttpContext.Current.Session["KIOSK_Organization"] = "LM IS&GS";
            //HttpContext.Current.Session["KIOSK_Address1"] = "3377 Research Blvd";
            //HttpContext.Current.Session["KIOSK_Address2"] = "";

            //HttpContext.Current.Session["KIOSK_ZIPCode"] = "20850";  //Also use for Postal if International Order
            //HttpContext.Current.Session["KIOSK_ZIPPlus4"] = "";
            //HttpContext.Current.Session["KIOSK_City"] = "Rockville";
            //HttpContext.Current.Session["KIOSK_State"] = "MD";    //Also use of Province if International Order
            //HttpContext.Current.Session["KIOSK_Country"] = "USA";  //Use only if International Order
            //HttpContext.Current.Session["KIOSK_Email"] = "jikku.p.jacob@lmco.com";
            //HttpContext.Current.Session["KIOSK_Phone"] = "(301) 519-5751";


            #endregion

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
                
                //if (Nerdos.Count > 0)
                //{
                //    ListNerdos.DataSource = Nerdos;
                //    ListNerdos.DataBind();
                //    pnlContentDownload.Visible = true;
                //}

                lblTot.Text = this.shoppingcartV2.TotalQtyForShipping.ToString();

                //Address Verification only for domestic orders
                if (string.Compare(ShipLocation, "Domestic", true) == 0)
                {
                    if (shoppingcartV2.TotalQtyForShipping > 0)
                    //***EAC test the address
                    {
                        if (shoppingcartV2.TotalQtyForShipping <= 20)
                            //if (true)
                            USPS(this.shipto);
                        else
                            FEDEX(this.shipto);
                    }
                    else
                    {
                        //do nothing, we just have orders for online(email) items
                    }
                }

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
            NameLabel.Text = "Name";
            NameText.Text = shipto.Fullname;
            OrgLabel.Text = "Organization";
            OrgText.Text = shipto.Organization;
            if (OrgText.Text == "")
                divOrg.Visible = false;
            Addr1Label.Text = "Address";
            Addr1Text.Text = shipto.Addr1;
            Addr2Label.Text = "&nbsp;";
            Addr2Text.Text = shipto.Addr2;
            if (Addr2Text.Text == "")
                divAddress2.Visible = false;
            CityText.Text = shipto.City;
            StateLabel.Text = "&nbsp;";
            StateText.Text = shipto.State;
            ZipText.Text = shipto.Zip5;
            Zip4Text.Text = shipto.Zip4;
            if (Zip4Text.Text != "")
                Zip4Text.Text = "-&nbsp;" + Zip4Text.Text;
            PhoneLabel.Text = "Phone";
            PhoneText.Text = shipto.Phone;
            if (PhoneText.Text == "")
                divPhone.Visible = false;
            EmailLabel.Text = "E-mail";
            EmailText.Text = shipto.Email;
            if (EmailText.Text == "")
                divEmail.Visible = false;
            CountyLabel.Text = "&nbsp;";
            CountryText.Text = shipto.Country;
            if (CountryText.Text == "")
                divCountry.Visible = false;
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
                
                lblDetails.Text = "TODO:";

                //if (p.ProductId.ToLower().EndsWith("c"))
                //{
                //    KVPair Nerdo = new KVPair();
                //    //***EAc  @%$#*@# UI is giving me problems...hardcoding class for now
                //    lblTitle.Text = "<span class=textLoud>Cover Only: </span>" + p.LongTitle;
                //    Nerdo.Key = p.LongTitle;
                //    Nerdo.Val = PubEnt.DAL2.DAL.GetNerdoURLByChild(p.ProductId);
                //    Nerdos.Add(Nerdo);
                //}
                #region Handle ONLINE-only items (20130319)
                //if (p.OrderDisplayStatus != "ORDER")
                if (!p.IsPhysicalItem)  //EAC patch (20130411)
                {
                    //The item must be ONLINE-only, for emailing purposes
                    lblQty.Text = "-"; 
                }
                else
                {
                    lblQty.Text = p.NumQtyOrdered.ToString();
                }
                #endregion
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

        #region AddressValidationMethods

        public void USPS(Person p)
        {
            string returnValue, XmlString;
            WebClient instance = new WebClient();
            XmlString = "<AddressValidateRequest USERID=\"USER\"><Address ID=\"0\"> <Address1>" + p.Addr2 + "</Address1> <Address2>" + p.Addr1 + "</Address2> <City>" + p.City + "</City> <State>" + p.State + "</State> <Zip5></Zip5> <Zip4></Zip4></Address> </AddressValidateRequest>";
            Uri siteUri = new Uri("http://production.shippingapis.com/ShippingAPI.dll?API=Verify&XML=" + XmlString.Replace("&", "%20"));
            WebRequest wr = WebRequest.Create(siteUri);
            returnValue = instance.DownloadString(siteUri);
            //btn2Submit.Enabled = true;
            lblAVS_Shipping.Visible = false;
            try
            {
                XmlDocument XmlDoc = new XmlDocument();
                XmlDoc.LoadXml(returnValue);

                if (returnValue.Contains("Error"))//***EAC HITT 8227 overrode all previous tickets.
                {
                    //lblAVS_Shipping.Text = "We did not find this shipping address in the U.S. Postal Service’s database. To ensure proper delivery, please review the address carefully before submitting your order."; // node.InnerText;
                    lblAVS_Shipping.Text = "Please review: We did not find this address in the U.S. Postal Service database.";
                    lblAVS_Shipping.Visible = true;
                    this.divVerifyErrDisplay.Visible = true;
                }

            }
            catch (Exception ex)
            {
                //***EAC ignore problems w/ AVS
            }
        }

        protected void FEDEX(Person p)
        {
            string straddress2 = p.Addr1;
            string strzipcode = p.Zip5;
            string address = string.Empty;
            string zipfour = string.Empty;
            string ststate = string.Empty;
            string zip = string.Empty;
            string[] split = null;
            string delimStr = "-";
            char[] delimiter = delimStr.ToCharArray();
            //btn2Submit.Enabled = true;
            lblAVS_Shipping.Visible = false;
            this.divVerifyErrDisplay.Visible = false;

            AddressValidationRequest request = CreateAddressValidationRequest(straddress2, strzipcode);
            //
            AddressValidationService addressValidationService = new AddressValidationService();
            //
            try
            {
                // This is the call to the web service passing in an AddressValidationRequest and returning an AddressValidationReply
                AddressValidationReply reply = addressValidationService.addressValidation(request);
                //
                int score = Convert.ToInt32(reply.AddressResults[0].ProposedAddressDetails[0].Score);
                if (reply.HighestSeverity == NotificationSeverityType.SUCCESS || reply.HighestSeverity == NotificationSeverityType.NOTE || reply.HighestSeverity == NotificationSeverityType.WARNING)
                {

                    foreach (AddressValidationResult result in reply.AddressResults)
                    {
                        foreach (ProposedAddressDetail detail in result.ProposedAddressDetails)
                        {
                            address = detail.Address.StreetLines[0];
                            zip = detail.Address.PostalCode;
                            split = zip.Split(delimiter);
                            zip = split[0];
                            //zipfour = split[1];
                        }
                        if (score < 20)//(split.Length<2) (zipfour == null || zipfour.Length < 4)
                        {
                            lblAVS_Shipping.Text = "Address Not Found";//intentionally missing period  so I can tell if its usps or fedex
                            //btn2Submit.Enabled = false;
                            lblAVS_Shipping.Visible = true;
                            this.divVerifyErrDisplay.Visible = true;
                        }
                        else
                        {
                            lblAVS_Shipping.Text = "SUCCESS";
                            //btn2Submit.Enabled = true;
                            lblAVS_Shipping.Visible = false;
                            this.divVerifyErrDisplay.Visible = false;

                        }
                    }
                }
                else
                {
                    foreach (Notification notification in reply.Notifications)
                        lblAVS_Shipping.Text = notification.Message;
                }
            }
            catch (Exception ex)
            {

            }


        }
        private static string ShowAddressValidationReply(AddressValidationReply reply)
        {
            string message = null;
            foreach (AddressValidationResult result in reply.AddressResults)
            {
                foreach (ProposedAddressDetail detail in result.ProposedAddressDetails)
                {
                    message = Convert.ToString(reply.HighestSeverity);

                }

            }
            return message;
        }

        private static AddressValidationRequest CreateAddressValidationRequest(string straddress2, string strzipcode)
        {
            // Build the AddressValidationRequest
            AddressValidationRequest request = new AddressValidationRequest();
            //
            request.WebAuthenticationDetail = new WebAuthenticationDetail();
            request.WebAuthenticationDetail.UserCredential = new WebAuthenticationCredential();
            request.WebAuthenticationDetail.UserCredential.Key = "";
            request.WebAuthenticationDetail.UserCredential.Password = "";
            //
            request.ClientDetail = new ClientDetail();
            request.ClientDetail.AccountNumber = "";
            request.ClientDetail.MeterNumber = "";
            //
            request.TransactionDetail = new TransactionDetail();
            request.TransactionDetail.CustomerTransactionId = "***Address Validation v2 Request using VC#***"; // This is just an echo back 
            //
            request.Version = new VersionId(); // Creates the Version element with all child elements populated
            //
            request.RequestTimestamp = DateTime.Now;
            //
            request.Options = new AddressValidationOptions();
            request.Options.CheckResidentialStatus = true;
            request.Options.MaximumNumberOfMatches = "5";
            request.Options.StreetAccuracy = AddressValidationAccuracyType.LOOSE;
            request.Options.DirectionalAccuracy = AddressValidationAccuracyType.LOOSE;
            request.Options.CompanyNameAccuracy = AddressValidationAccuracyType.LOOSE;
            request.Options.ConvertToUpperCase = true;
            request.Options.RecognizeAlternateCityNames = true;
            request.Options.ReturnParsedElements = true;
            //
            request.AddressesToValidate = new AddressToValidate[1];
            request.AddressesToValidate[0] = new AddressToValidate();
            request.AddressesToValidate[0].AddressId = "LM";
            request.AddressesToValidate[0].Address = new Address();
            request.AddressesToValidate[0].Address.StreetLines = new String[1] { straddress2 };
            request.AddressesToValidate[0].Address.PostalCode = strzipcode;
            request.AddressesToValidate[0].CompanyName = "LM";

            return request;
        }


        public String createXMl(Person p)
        {
            string straddress1 = p.Addr2;
            string straddress2 = p.Addr1;
            string strcity = p.City;
            string strzipcode = p.Zip5;
            string strzipplusfour = p.Zip4;
            string strstate = p.State;
            string user = '"' + "USER" + '"';
            string Id = '"' + "0" + '"';

            string XmlString = "<AddressValidateRequest USERID=@user@><Address ID=@Id@> <Address1>@address1@</Address1> <Address2>@address2@</Address2> <City>@city@</City> <State>@state@</State> <Zip5></Zip5> <Zip4>@zipplusfour@</Zip4></Address> </AddressValidateRequest>";

            XmlString = XmlString.Replace("@address1@", straddress1);
            XmlString = XmlString.Replace("@address2@", straddress2);
            XmlString = XmlString.Replace("@city@", strcity);
            XmlString = XmlString.Replace("@state@", strstate);
            XmlString = XmlString.Replace("@zipplusfour@", strzipplusfour);
            XmlString = XmlString.Replace("@user@", user);
            XmlString = XmlString.Replace("@Id@", Id);
            return XmlString;
        }

        #endregion

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
            //btnChangeShippingInfo.Visible = false;
            ImgBtnChangeShipInfo.Visible = false;
            pnlVerifyButtons.Visible = false;
            //Reset Session Variables
            Utils.ResetSessions();

            //--- Modified by Vu Tran 01/31/2010
            Master.FindControl("btnFinish").Visible = true;
            overridejavascript = "function expiredprompt(){gobackAttractScreen(); }";
        }

        protected void btnChangeShippingInfo_Click(object sender, EventArgs e)
        {   
            Response.Redirect("kioskshipping.aspx?ConfID=" + ConfId.ToString().ToString(), true);
        }

        protected void ImgBtnBacktoCart_Click(object sender, ImageClickEventArgs e)
        {
            //TODO: Modify if shipping expects any other parameter
            Response.Redirect("cart.aspx?ConfID=" + ConfId.ToString(), true); //EAC assume ConfID is supplied to all pages
        }

    }
}
