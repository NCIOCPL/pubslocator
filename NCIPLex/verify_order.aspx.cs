using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using NCIPLex.BLL;
using NCIPLex.DAL;
using System.IO;
using System.Net;
using System.Xml;
//using System.Web.Mail;
using System.Configuration;
using NCIPLex.GlobalUtils;

namespace NCIPLex
{
    public partial class verify_order : System.Web.UI.Page
    {
        private string CostRecoveryInd = ""; //HITT 8716
        string ShipLocation = "";
        string strConfId = "";
        int ConfId = 0;

        Person billto
        {
            get
            {
                if (Session["NCIPLEX_billto"] == null)
                    return null;
                else
                    return (Person)Session["NCIPLEX_billto"];
            }
            set { Session["NCIPLEX_billto"] = value; }
        }
        
        Person shipto
        {
            get
            {
                if (Session["NCIPLEX_shipto"] == null)
                    return null;
                else
                    return (Person)Session["NCIPLEX_shipto"];
            }
            set { Session["NCIPLEX_shipto"] = value; }
        }

        CreditCard cc
        {
            get
            {
                if (Session["NCIPLEX_cc"] == null)
                    return null;
                else
                    return (CreditCard)Session["NCIPLEX_cc"];
            }
            set { Session["NCIPLEX_cc"] = value; }
        }

        ProductCollection shoppingcart
        {
            get
            {
                if (Session["NCIPLEX_cart"] == null)
                    return null;
                else
                    return (ProductCollection)Session["NCIPLEX_cart"];
            }
            set { Session["NCIPLEX_cart"] = value; }
        }
        
        ProductCollection shoppingcartV2   //***EAC 2nd version of shopping cart w/ virtual/linked pubs
        {
            get
            {
                if (Session["NCIPLEX_cartV2"] == null)
                    return null;
                else
                    return (ProductCollection)Session["NCIPLEX_cartV2"];
            }
            set { Session["NCIPLEX_cartV2"] = value; }
        }

        //NCIPLex KVPairCollection Nerdos = new KVPairCollection();

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Request.QueryString["confid"] == null)
            //{
            //    Utils.ResetSessions();
            //    Response.Redirect("default.aspx?redirect=verify", true);
            //}
            //else
            //    strConfId = Request.QueryString["confid"].ToString();

            if (Utils.ValidateRedirect().Length > 0) //Important check
                Response.Redirect(Utils.ValidateRedirect(), true);

            strConfId = Session["NCIPLEX_ConfId"].ToString();

            if (strConfId.Length > 4 || strConfId == "") //potentially an intrusion
            {
                ResetSessions();
                Response.Redirect("location.aspx?redirect=verify", true);
            }
            else
                ConfId = Int32.Parse(strConfId);

            if (this.shoppingcart == null || this.shoppingcart.Count < 1)
            {  //***EAC I need a shopping cart at this point
                ResetSessions();
                Response.Redirect("location.aspx?missingsession=true", true);
            }

            if (Session["NCIPLEX_ShipLocation"] != null)
            {
                if (Session["NCIPLEX_ShipLocation"].ToString().Length > 0)
                {
                    if (string.Compare(Session["NCIPLEX_ShipLocation"].ToString(), "Domestic", true) == 0)
                        ShipLocation = "Domestic";
                    else if (string.Compare(Session["NCIPLEX_ShipLocation"].ToString(), "International", true) == 0)
                        ShipLocation = "International";
                }
            }

            if (ShipLocation.Length == 0)
            {
                ResetSessions();
                Response.Redirect("location.aspx", true);
            }


            if (!IsPostBack)
            {
                Session["NCIPLEX_DONE"] = "false";
                steps1.Activate("cell4");

                //PubEnt.shipping.US

                lblname.Text = this.shipto.Fullname;
                lblOrg.Text = this.shipto.Organization;
                lbladdr1.Text = this.shipto.Addr1;
                lbladdr2.Text = this.shipto.Addr2;
                lblcity.Text = this.shipto.City;
                lblst.Text = this.shipto.State;
                lblzip.Text = this.shipto.Zip5 + ((this.shipto.Zip4.Trim() == "") ? "" : "-" + this.shipto.Zip4);
                
                lblCountry.Text = this.shipto.Country;
                if (string.Compare(ShipLocation, "International", true) == 0)
                    divCountry.Visible = true;
                
                lblemail.Text = "E-mail: " + this.shipto.Email;
                lblphone.Text = "Phone: " + this.shipto.Phone;


                lbl2name.Text = this.billto.Fullname;
                lbl2addr1.Text = this.billto.Addr1;
                lbl2addr2.Text = this.billto.Addr2;
                lbl2city.Text = this.billto.City;
                lbl2st.Text = this.billto.State;
                lbl2zip.Text = this.billto.Zip5 + ((this.billto.Zip4.Trim() == "") ? "" : "-" + this.billto.Zip4);
                lbl2email.Text = "E-mail: " + this.billto.Email;
                lbl2phone.Text = "Phone: " + this.billto.Phone;
                lbl2org.Text = this.billto.Organization;

                if (this.shipto.Organization.Trim().Length < 1) lblOrg.Visible = false;
                if (this.shipto.Addr2.Trim().Length < 1) lbladdr2.Visible = false;
                if (this.shipto.Email.Trim().Length < 1) lblemail.Visible = false;
                if (this.shipto.Phone.Trim().Length < 1) lblphone.Visible = false;

                if (this.billto.Organization.Trim().Length < 1) lbl2org.Visible = false;
                if (this.billto.Addr2.Trim().Length < 1) lbl2addr2.Visible = false;
                if (this.billto.Email.Trim().Length < 1) lbl2email.Visible = false;
                if (this.billto.Phone.Trim().Length < 1) lbl2phone.Visible = false;

                //JPJ 03-10-11 NCIPLex does not accept credit card orders
                //if (this.cc != null && this.cc.Cost > 0.0)
                //{
                //    pnlPaymentInfo.Visible = true;
                //    lblcc.Text = "Credit Card: " + this.cc.CompanyText ;
                //    lblccnum.Text = "Credit Card Number: ************" + this.cc.CCnum.Substring(12);
                //    lblexpires.Text = "Expiration Date: " + this.cc.ExpMon + "/" + this.cc.ExpYr;
                //    lblCost.Text = this.cc.Cost.ToString("c");
                //    btn2Shipping.Text = "Change Addresses or Payment Information";
                //    CostRecoveryInd = "1"; //HITT 8716
                //}
                //else
                //{
                pnlPaymentInfo.Visible = false;
                pnlBillingInfo.Visible = false;
                btn2Shipping.Text = "Change Address";
                //}

                grdItems.DataSource = this.shoppingcartV2;
                grdItems.DataBind();


                //***EAC housekeeping
                lblTotalItems.Text = this.shoppingcartV2.TotalQty.ToString();
                lblCost.Text = this.shoppingcartV2.Cost.ToString("c");

                //NCIPLex does not allow Nerdos
                //if (Nerdos.Count > 0)
                //{
                //    ListNerdos.DataSource = Nerdos;
                //    ListNerdos.DataBind();
                //    pnlContentDownload.Visible = true;
                //}

                ////***EAC test the address
                //if (shoppingcartV2.TotalQty <= 20)
                ////if (true)
                //    USPS(this.shipto);
                //else
                //    FEDEX(this.shipto);

                //JPJ 03-10-11 NCIPLex does not use FedEx because irders more than 20 are not allowed

                //Address Verification only for domestic orders
                if (string.Compare(ShipLocation, "Domestic", true) == 0)
                {
                    ////***EAC test the address
                    //if (shoppingcartV2.TotalQty <= 20)
                    //    //if (true)
                        USPS(this.shipto);
                    //else
                    //    FEDEX(this.shipto);
                }


                /*Begin HITT 8716*/
                if (string.Compare(CostRecoveryInd, "") == 0)
                {
                    lblCCText.Visible = false;
                    divCost.Visible = false;
                    divCCExplanation.Visible = false;
                }
                /*End HITT 8716*/
            }

            //Display the master page tabs 
            GlobalUtils.Utils UtilMethod = new GlobalUtils.Utils();
            if (Session["NCIPLEX_Pubs"] != null)
                Master.LiteralText = UtilMethod.GetTabHtmlMarkUp(Session["NCIPLEX_Qtys"].ToString(), "");
            else
                Master.LiteralText = UtilMethod.GetTabHtmlMarkUp("", "");
            UtilMethod = null;
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
                lblQty.Text = p.NumQtyOrdered.ToString();
                lblDetails.Text = "TODO:";

                //JPJ 03-10-11 NCIPLex does not allow ordering of Nerdo cover publications
                //if (p.ProductId.ToLower().EndsWith("c"))
                //{
                //    KVPair Nerdo = new KVPair();
                //    //***EAc  @%$#*@# UI is giving me problems...hardcoding class for now
                //    lblTitle.Text = "<span class=textLoud>Cover Only: </span>" + p.LongTitle;
                //    Nerdo.Key = p.LongTitle;
                //    Nerdo.Val = PubEnt.DAL2.DAL.GetNerdoURLByChild(p.ProductId);
                //    Nerdos.Add(Nerdo);
                //}
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("shipping.aspx", true);
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Response.Redirect("cart.aspx", true);
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            if (Session["NCIPLEX_DONE"].ToString() == "true")
            {
                Response.Redirect("location.aspx");
            }
            steps1.Activate("cell5");
            //NCIPLex Transaction t = new Transaction(this.shipto, this.billto, this.cc, this.shoppingcart );
            Transaction t = new Transaction(this.shipto, this.billto, this.cc, this.shoppingcart);
            //Begin HITT 8329 (CR 21) Save The Raw Order ###############################
            //Save the Raw Order Details before credit card and before order upload call
            try
            {
                string Fax = "";
                string Province = ""; //Only for International Exhibit Order
                string Country = ""; //Only for International Exhibit Order
                //string interfacename = "NCIPLex"; //NCIPL, ROO or EXHIBIT
                string interfacename = ConfigurationManager.AppSettings["InterfaceName"];
                string international = ""; //Pass "1" for International Order (Currently only for Exhibit)
                string Zip = t.ShipTo.Zip5; //Zip5 for NCIPL, ROO and Exhibit Domestic Orders. International Zip for Exhibit International Order.

                if (string.Compare(ShipLocation, "International", true) == 0)
                {
                    international = "1";
                    Province = t.ShipTo.State;
                    Country = t.ShipTo.Country;
                }

                string rawpubids = "";
                string rawpubqtys = "";

                string[] rawpubs = Session["NCIPLEX_Pubs"].ToString().Split(new Char[] { ',' });
                for (var i = 0; i < rawpubs.Length; i++)
                {
                    //if (pubs[i].Trim() != "")
                    if (string.Compare(rawpubs[i].Trim(), "", true) != 0)
                        rawpubids += rawpubs[i].Trim() + ",";
                }

                string[] rawqtys = Session["NCIPLEX_Qtys"].ToString().Split(new Char[] { ',' });
                for (var i = 0; i < rawqtys.Length; i++)
                {
                    //if (qtys[i].Trim() != "")
                    if (string.Compare(rawqtys[i].Trim(), "", true) != 0)
                        rawpubqtys += rawqtys[i].Trim() + ",";
                }

                if (rawpubids.Length > 1)
                    rawpubids = rawpubids.TrimEnd(',');

                if (rawpubqtys.Length > 1)
                    rawpubqtys = rawpubqtys.TrimEnd(',');

                string CostRecoveryInd = "";

                //JPJ 03-10-11 NCIPLex does not accept credit card orders
                //if (t.CC.Cost > 0)
                //    CostRecoveryInd = "1";

                SQLDataAccess.SaveRawOrder(t.ShipTo.Organization,
                                    t.ShipTo.Fullname,
                                    t.ShipTo.Addr1,
                                    t.ShipTo.Addr2,
                                    t.ShipTo.City,
                                    t.ShipTo.State,
                                    Zip,
                                    t.ShipTo.Zip4,
                                    t.ShipTo.Phone,
                                    Fax,
                                    t.ShipTo.Email,
                                    Province,
                                    Country,
                                    CostRecoveryInd,
                                    rawpubids,
                                    rawpubqtys,
                                    Request.UserHostAddress,
                                    interfacename,
                                    international);
            }
            catch (Exception Ex)
            {
                //Write to log
                Microsoft.Practices.EnterpriseLibrary.Logging.LogEntry logEnt = new Microsoft.Practices.EnterpriseLibrary.Logging.LogEntry();
                logEnt.Message = "\r\n" + "Unable to invoke SaveRawOrder()." + "\r\n" + "Source: " + Ex.Source + "\r\n" + "Description: " + Ex.Message + "\r\n" + "Stack Trace: " + Ex.StackTrace;
                Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(logEnt, "Logs");

            }
            //End HITT 8329(CR 21) #####################################################

            //***EAC SOP---swipe first before creating order 
            //if (Swipe(this.cc)) //Should return true for a free transaction -COMMENTED FOR NCIPLex
            //{
            int returnvalue = 0;
            int returnordernum = 0;
            string pubids = "";
            string pubqtys = "";

            string[] pubs = Session["NCIPLEX_Pubs"].ToString().Split(new Char[] { ',' });
            for (var i = 0; i < pubs.Length; i++)
            {
                //if (pubs[i].Trim() != "")
                if (string.Compare(pubs[i].Trim(), "", true) != 0)
                    pubids += pubs[i].Trim() + ",";
            }

            string[] qtys = Session["NCIPLEX_Qtys"].ToString().Split(new Char[] { ',' });
            for (var i = 0; i < qtys.Length; i++)
            {
                //if (qtys[i].Trim() != "")
                if (string.Compare(qtys[i].Trim(), "", true) != 0)
                    pubqtys += qtys[i].Trim() + ",";
            }

            if (pubids.Length > 1)
                pubids = pubids.TrimEnd(',');

            if (pubqtys.Length > 1)
                pubqtys = pubqtys.TrimEnd(',');


            if (t.Save(Request.UserHostAddress, pubids, pubqtys, ConfId, ShipLocation, out returnvalue, out returnordernum))
            {
                //***EAC Everything looks good at this point
                Session["NCIPLEX_DONE"] = "true";
                lblAVS_Shipping.Visible = false;
                Label1.Text = "Confirmation";
                steps1.Visible = false; //HITT 8716
                Label2.Text = "";
                btn2Shipping.Visible = false;
                btn2Cart.Visible = false;
                btn2Cancel.Visible = false;
                btn2Submit.Visible = false;
                LinkButton1.Visible = false;
                pnlConfirm1.Visible = true;
                pnlConfirm2.Visible = true;
                LblThank.Visible = true;
                lblorderdt.Text = System.DateTime.Today.ToShortDateString();

                //NCIPLex does not allow Nerdo publications to be ordered - COMMENT below eamil section
                //if (shipto.Email.Length > 0 && HasCover(this.shoppingcart))//if we have a valid shipto.email address
                //{
                //    StringWriter sw = new StringWriter();
                //    HtmlTextWriter writer = new HtmlTextWriter(sw);
                //    string s;
                //    //pnlConfirm1.RenderControl(writer);
                //    Panel1.RenderControl(writer);
                //    pnlBillingInfo.RenderControl(writer);
                //    pnlPaymentInfo.RenderControl(writer);
                //    grdItems.RenderControl(writer);
                //    ListNerdos.RenderControl(writer);
                //    s = sw.ToString();

                //    NCIPLex.DAL2.DAL.SaveEmail(shipto.Email, s);
                //    try
                //    {   //SYSTEM.WEB.MAIL -- i know its becoming obsolete
                //        MailMessage msg = new MailMessage();
                //        msg.From = ConfigurationManager.AppSettings["PubEntEmailAddress"];
                //        msg.To = shipto.Email;
                //        msg.Subject = "Order Confirmation";
                //        msg.Body = s;
                //        msg.BodyFormat = MailFormat.Html;
                //        SmtpMail.Send(msg);
                //    }
                //    catch (Exception) { }
                //}
                //Code for HITT 8716
                string printertext = ""; //Used like a flag, will not hold any value
                string printerShipping = "", printerBilling = "", printerPayment = "";

                StringWriter swShipping = new StringWriter();
                HtmlTextWriter writerShipping = new HtmlTextWriter(swShipping);
                Panel1.RenderControl(writerShipping);
                printerShipping = swShipping.ToString();

                StringWriter swBilling = new StringWriter();
                HtmlTextWriter writerBilling = new HtmlTextWriter(swBilling);
                pnlBillingInfo.RenderControl(writerBilling);
                printerBilling = swBilling.ToString();

                StringWriter swPayment = new StringWriter();
                HtmlTextWriter writerPayment = new HtmlTextWriter(swPayment);
                pnlPaymentInfo.RenderControl(writerPayment);
                printerPayment = swPayment.ToString();

                Session["NCIPLEX_PrinterFriendly"] = printertext;
                Session["NCIPLEX_PrinterGrid"] = this.shoppingcartV2;
                Session["NCIPLEX_PrinterShipping"] = printerShipping;
                Session["NCIPLEX_PrinterBilling"] = printerBilling;
                Session["NCIPLEX_PrinterPayment"] = printerPayment;

               // if (this.cc.Cost > 0)
               //     Session["NCIPLEX_PrinterCostRecovery"] = "True";
               // else
                    Session["NCIPLEX_PrinterCostRecovery"] = "False";
                //End code

                ResetSessions();
                Response.Redirect("confirm_order.aspx", true); //JPJ 032211 - Newly added confirmation page
            }
            else
            {
                throw (new ApplicationException("Cannot save order."));
            }
            //}
            //else{
            //    //throw (new ApplicationException("Problem with credit card transaction"));
            //    Response.Redirect("ccproblem.aspx", true);
            //}
        }

        protected void CancelOrder(object sender, EventArgs e)
        {
            //todo should confirm with fancy popup control
            //reset all sessions here then kick back to default.aspx
            ResetSessions();
            Response.Redirect("location.aspx", true);
        }

        private void ResetSessions()
        {

            Session["NCIPLEX_cart"] = null;       //destroy
            Session["NCIPLEX_shipto"] = null;     //destroy
            Session["NCIPLEX_billto"] = null;     //destroy
            Session["NCIPLEX_cc"] = null;         //destroy
            Session["NCIPLEX_DONE"] = "";         //neither done nor finished

            //***EAC Create the session variables asap
            Session["NCIPLEX_Pubs"] = "";
            Session["NCIPLEX_Qtys"] = "";
            Session["NCIPLEX_SearchKeyword"] = "";
            Session["NCIPLEX_TypeOfCancer"] = "";
            Session["NCIPLEX_Subject"] = "";
            Session["NCIPLEX_Audience"] = "";
            Session["NCIPLEX_ProductFormat"] = "";
            Session["NCIPLEX_Language"] = "";
            Session["NCIPLEX_StartsWith"] = "";
            Session["NCIPLEX_Series"] = ""; //Or collection
            Session["NCIPLEX_NewOrUpdated"] = "";
            Session["NCIPLEX_Race"] = "";

            Session["NCIPLEX_ShipLocation"] = "";   //JPJ - Comment this, to retain ship location after order submit

            //Display the master page tabs 
            GlobalUtils.Utils UtilMethod = new GlobalUtils.Utils();
            if (Session["NCIPLEX_Pubs"] != null)
                Master.LiteralText = UtilMethod.GetTabHtmlMarkUp(Session["NCIPLEX_Qtys"].ToString(), "");
            else
                Master.LiteralText = UtilMethod.GetTabHtmlMarkUp("", "");
            UtilMethod = null;
        }

        //JPJ Not used
        //private bool SaveOrder(CreditCard cc, Person shipto, Person billto, ProductCollection cart)
        //{
        //    try
        //    {
        //        return (true);
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        //JPJ 03-10-11 NCIPLex does not accept credit card orders (commented below method)

        //private bool Swipe(CreditCard cc)
        //{
        //    if (this.cc.Cost > 0.0)
        //    {
        //        string APPROVED = "<ssl_result_message>APPROV";
        //        // string xmldata = "xmldata=<txn><ssl_merchant_ID>502928</ssl_merchant_ID><ssl_user_id>webform</ssl_user_id><ssl_pin>PK0GAL</ssl_pin><ssl_transaction_type>ccsale</ssl_transaction_type><ssl_card_number>5000300020003003</ssl_card_number><ssl_exp_date>1209</ssl_exp_date><ssl_amount>12.3</ssl_amount><ssl_salestax>0.00</ssl_salestax><ssl_cvv2cvc2_indicator>Present</ssl_cvv2cvc2_indicator><ssl_cvv2cvc2>123</ssl_cvv2cvc2><ssl_invoice_number></ssl_invoice_number><ssl_customer_code>0</ssl_customer_code><ssl_first_name></ssl_first_name><ssl_last_name></ssl_last_name><ssl_avs_address>123 mystreet</ssl_avs_address><ssl_address2></ssl_address2><ssl_city></ssl_city><ssl_state></ssl_state><ssl_avs_zip>90210</ssl_avs_zip><ssl_phone></ssl_phone><ssl_email></ssl_email></txn>";
        //        ASCIIEncoding encoding = new ASCIIEncoding();

        //        //***EAC had to do this next 2 lines because of a bug in billto.Addr1.Substring(0,20)
        //        string temp = billto.Addr1;
        //        if (temp.Length>20) temp = temp.Substring(0, 20);

        //        string postData = "xmldata=<txn><ssl_merchant_ID>502928</ssl_merchant_ID><ssl_user_id>webform</ssl_user_id><ssl_pin>PK0GAL</ssl_pin><ssl_transaction_type>ccsale</ssl_transaction_type><ssl_card_number>" + this.cc.CCnum + "</ssl_card_number><ssl_exp_date>" + this.cc.ExpMon + this.cc.ExpYr + "</ssl_exp_date><ssl_amount>" + this.cc.Cost + "</ssl_amount><ssl_salestax>0.00</ssl_salestax><ssl_cvv2cvc2_indicator>Present</ssl_cvv2cvc2_indicator><ssl_cvv2cvc2>" + this.cc.CVV2 + "</ssl_cvv2cvc2><ssl_invoice_number></ssl_invoice_number><ssl_customer_code>0</ssl_customer_code><ssl_first_name></ssl_first_name><ssl_last_name></ssl_last_name><ssl_avs_address>" + temp + "</ssl_avs_address><ssl_address2></ssl_address2><ssl_city></ssl_city><ssl_state></ssl_state><ssl_avs_zip>" + billto.Zip5 + "</ssl_avs_zip><ssl_phone></ssl_phone><ssl_email></ssl_email></txn>";

        //        byte[] data = encoding.GetBytes(postData);

        //        // Prepare web request...
        //        HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create("https://www.myvirtualmerchant.com/VirtualMerchant/processxml.do");
        //        myRequest.Method = "POST";
        //        myRequest.ContentType = "application/x-www-form-urlencoded";
        //        myRequest.ContentLength = data.Length;
        //        Stream newStream = myRequest.GetRequestStream();
        //        // Send the data.
        //        newStream.Write(data, 0, data.Length);
        //        newStream.Close();

        //        HttpWebResponse resp = (HttpWebResponse)myRequest.GetResponse();
        //        Encoding enc = System.Text.Encoding.GetEncoding(1252);
        //        StreamReader respstr = new StreamReader(resp.GetResponseStream(), enc);
        //        string RESP = respstr.ReadToEnd();
        //        resp.Close();
        //        respstr.Close();

        //        PubEnt.DAL.DAL.SaveCCResponse(RESP);
        //        if (RESP.Contains(APPROVED))
        //        {
        //            //***EAC get CC Transaction ID
        //            int i = RESP.IndexOf("<ssl_txn_id>");
        //            int j = RESP.IndexOf("</ssl_txn_id>");
        //            this.cc.TransID = RESP.Substring(i+12, j - i - 12);

        //            //***EAC get CC Transaction ID
        //             i = RESP.IndexOf("<ssl_approval_code>");
        //             j = RESP.IndexOf("</ssl_approval_code>");
        //            this.cc.ApprovalCode = RESP.Substring(i + 19, j - i - 19);

        //            return (true);
        //        }
        //        else
        //        {
        //            return (false);
        //        }
        //    }
        //    else return (true); //free transaction
        //}

        public void USPS(Person p)
        {
            string returnValue, XmlString;
            WebClient instance = new WebClient();
            XmlString = "<AddressValidateRequest USERID=\"USER\"><Address ID=\"0\"> <Address1>" + p.Addr2 + "</Address1> <Address2>" + p.Addr1 + "</Address2> <City>" + p.City + "</City> <State>" + p.State + "</State> <Zip5></Zip5> <Zip4></Zip4></Address> </AddressValidateRequest>";
            Uri siteUri = new Uri("http://production.shippingapis.com/ShippingAPI.dll?API=Verify&XML=" + XmlString.Replace("&", "%20"));
            WebRequest wr = WebRequest.Create(siteUri);
            //returnValue = instance.DownloadString(siteUri);
            btn2Submit.Enabled = true;
            lblAVS_Shipping.Visible = false;
            try
            {
                returnValue = instance.DownloadString(siteUri);
                XmlDocument XmlDoc = new XmlDocument();
                XmlDoc.LoadXml(returnValue);

                if (returnValue.Contains("Error"))//***EAC HITT 8227 overrode all previous tickets.
                {
                    lblAVS_Shipping.Text = "We did not find this shipping address in the U.S. Postal Service’s database. To ensure proper delivery, please review the address carefully before submitting your order."; // node.InnerText;
                    lblAVS_Shipping.Visible = true;
                }

            }
            catch (Exception ex)
            {
                //***EAC ignore problems w/ AVS

                //Write to log
                Microsoft.Practices.EnterpriseLibrary.Logging.LogEntry logEnt = new Microsoft.Practices.EnterpriseLibrary.Logging.LogEntry();
                logEnt.Message = "\r\n" + "USPS Address Verification error." + "\r\n" + "Source: " + ex.Source + "\r\n" + "Description: " + ex.Message + "\r\n" + "Stack Trace: " + ex.StackTrace;
                Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(logEnt, "Logs");
            }
        }

        //JPJ 03-10-11 NCIPLex does not accept Cost Recovery order (>20). So no FedEx orders.
        //protected void FEDEX(Person p)
        //{
        //    string straddress2 = p.Addr1;
        //    string strzipcode = p.Zip5;
        //    string address = string.Empty;
        //    string zipfour = string.Empty;
        //    string ststate = string.Empty;
        //    string zip = string.Empty;
        //    string[] split = null;
        //    string delimStr = "-";
        //    char[] delimiter = delimStr.ToCharArray();
        //    btn2Submit.Enabled = true;
        //    lblAVS_Shipping.Visible = false;            

        //    AddressValidationRequest request = CreateAddressValidationRequest(straddress2, strzipcode);
        //    //
        //    AddressValidationService addressValidationService = new AddressValidationService();
        //    //
        //    try
        //    {
        //        // This is the call to the web service passing in an AddressValidationRequest and returning an AddressValidationReply
        //        AddressValidationReply reply = addressValidationService.addressValidation(request);
        //        //
        //        int score = Convert.ToInt32(reply.AddressResults[0].ProposedAddressDetails[0].Score);
        //        if (reply.HighestSeverity == NotificationSeverityType.SUCCESS || reply.HighestSeverity == NotificationSeverityType.NOTE || reply.HighestSeverity == NotificationSeverityType.WARNING)
        //        {

        //            foreach (AddressValidationResult result in reply.AddressResults)
        //            {
        //                foreach (ProposedAddressDetail detail in result.ProposedAddressDetails)
        //                {
        //                    address = detail.Address.StreetLines[0];
        //                    zip = detail.Address.PostalCode;
        //                    split = zip.Split(delimiter);
        //                    zip = split[0];
        //                    //zipfour = split[1];
        //                }
        //                if (score < 20)//(split.Length<2) (zipfour == null || zipfour.Length < 4)
        //                {
        //                    lblAVS_Shipping.Text = "Address Not Found";//intentionally missing period  so I can tell if its usps or fedex
        //                    btn2Submit.Enabled = false;
        //                    lblAVS_Shipping.Visible = true;
        //                }
        //                else
        //                {
        //                    lblAVS_Shipping.Text = "SUCCESS";
        //                    btn2Submit.Enabled = true;
        //                    lblAVS_Shipping.Visible = false;

        //                }
        //            }
        //        }
        //        else
        //        {
        //            foreach (Notification notification in reply.Notifications)
        //                lblAVS_Shipping.Text = notification.Message;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //Write to log
        //        Microsoft.Practices.EnterpriseLibrary.Logging.LogEntry logEnt = new Microsoft.Practices.EnterpriseLibrary.Logging.LogEntry();
        //        logEnt.Message = "\r\n" + "FedEx Address Verification error." + "\r\n" + "Source: " + ex.Source + "\r\n" + "Description: " + ex.Message + "\r\n" + "Stack Trace: " + ex.StackTrace;
        //        Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(logEnt, "Logs");
        //    }


        //}

        //JPJ 03-10-11 NCIPLex does not accept credit card orders, so no FedEx shipping (commented ShowAddressValidationReply() and CreateAddressValidationRequest())

        //private static string ShowAddressValidationReply(AddressValidationReply reply)
        //{
        //    string message = null;
        //    foreach (AddressValidationResult result in reply.AddressResults)
        //    {
        //        foreach (ProposedAddressDetail detail in result.ProposedAddressDetails)
        //        {
        //            message = Convert.ToString(reply.HighestSeverity);

        //        }

        //    }
        //    return message;
        //}

        //private static AddressValidationRequest CreateAddressValidationRequest(string straddress2, string strzipcode)
        //{
        //    // Build the AddressValidationRequest
        //    AddressValidationRequest request = new AddressValidationRequest();
        //    //
        //    request.WebAuthenticationDetail = new WebAuthenticationDetail();
        //    request.WebAuthenticationDetail.UserCredential = new WebAuthenticationCredential();
        //    request.WebAuthenticationDetail.UserCredential.Key = "";
        //    request.WebAuthenticationDetail.UserCredential.Password = "";
        //    //
        //    request.ClientDetail = new ClientDetail();
        //    request.ClientDetail.AccountNumber = "";
        //    request.ClientDetail.MeterNumber = "";
        //    //
        //    request.TransactionDetail = new TransactionDetail();
        //    request.TransactionDetail.CustomerTransactionId = "***Address Validation v2 Request using VC#***"; // This is just an echo back 
        //    //
        //    request.Version = new VersionId(); // Creates the Version element with all child elements populated
        //    //
        //    request.RequestTimestamp = DateTime.Now;
        //    //
        //    request.Options = new AddressValidationOptions();
        //    request.Options.CheckResidentialStatus = true;
        //    request.Options.MaximumNumberOfMatches = "5";
        //    request.Options.StreetAccuracy = AddressValidationAccuracyType.LOOSE;
        //    request.Options.DirectionalAccuracy = AddressValidationAccuracyType.LOOSE;
        //    request.Options.CompanyNameAccuracy = AddressValidationAccuracyType.LOOSE;
        //    request.Options.ConvertToUpperCase = true;
        //    request.Options.RecognizeAlternateCityNames = true;
        //    request.Options.ReturnParsedElements = true;
        //    //
        //    request.AddressesToValidate = new AddressToValidate[1];
        //    request.AddressesToValidate[0] = new AddressToValidate();
        //    request.AddressesToValidate[0].AddressId = "LM";
        //    request.AddressesToValidate[0].Address = new Address();
        //    request.AddressesToValidate[0].Address.StreetLines = new String[1] { straddress2 };
        //    request.AddressesToValidate[0].Address.PostalCode = strzipcode;
        //    request.AddressesToValidate[0].CompanyName = "LM";

        //    return request;
        //}

        #region NotUsed
        /// <summary>
        /// JPJ     03-11-11    Commented since the methods in this section are not used, 
        ///                     they are probably remnants from development code
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        //public String createXMl(Person p)
        //{
        //    string straddress1 = p.Addr2;
        //    string straddress2 = p.Addr1;
        //    string strcity = p.City;
        //    string strzipcode = p.Zip5;
        //    string strzipplusfour = p.Zip4;
        //    string strstate = p.State;
        //    string user = '"' + "USER" + '"';
        //    string Id = '"' + "0" + '"';

        //    string XmlString = "<AddressValidateRequest USERID=@user@><Address ID=@Id@> <Address1>@address1@</Address1> <Address2>@address2@</Address2> <City>@city@</City> <State>@state@</State> <Zip5></Zip5> <Zip4>@zipplusfour@</Zip4></Address> </AddressValidateRequest>";

        //    XmlString = XmlString.Replace("@address1@", straddress1);
        //    XmlString = XmlString.Replace("@address2@", straddress2);
        //    XmlString = XmlString.Replace("@city@", strcity);
        //    XmlString = XmlString.Replace("@state@", strstate);
        //    XmlString = XmlString.Replace("@zipplusfour@", strzipplusfour);
        //    XmlString = XmlString.Replace("@user@", user);
        //    XmlString = XmlString.Replace("@Id@", Id);
        //    return XmlString;
        //}
        #endregion

        protected void Nerdos_IDB(object sender, DataListItemEventArgs e)
        {
            //NCIPLex does not allow Nerdo publications to be ordered
            //if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            //{
            //    KVPair kv = (KVPair)e.Item.DataItem;
            //    Label lblNerdoTitle = (Label)e.Item.FindControl("lblNerdoTitle");
            //    lblNerdoTitle.Text = kv.Key;
            //    HyperLink lnkCanType = (HyperLink)e.Item.FindControl("lnkNerdo");

            //    lnkCanType.Text = kv.Val;
            //    lnkCanType.NavigateUrl =  kv.Val;
            //}
        }

        private bool HasCover(ProductCollection cart)
        {
            foreach (Product p in cart)
            {
                if (p.ProductId.ToLower().EndsWith("c"))
                {
                    return (true);
                }
            }
            return (false);
        }

    }
}
