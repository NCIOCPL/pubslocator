using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PubEnt.DAL;
using PubEnt.BLL;
using System.Net;
using System.Text;
using System.IO;
using System.Xml;
using System.Web.Mail;
using System.Configuration;
using PubEnt.AddressValidationServiceWebReference; 

namespace PubEnt
{
    public partial class verify : System.Web.UI.Page
    {
        private string CostRecoveryInd = ""; //HITT 8716

        Person billto
        {
            get
            {
                if (Session["NCIPL_billto"] == null)
                    return null;
                else
                    return (Person)Session["NCIPL_billto"];
            }
            set { Session["NCIPL_billto"] = value; }
        }
        Person shipto
        {
            get
            {
                if (Session["NCIPL_shipto"] == null)
                    return null;
                else
                    return (Person)Session["NCIPL_shipto"];
            }
            set { Session["NCIPL_shipto"] = value; }
        }
        CreditCard cc
        {
            get
            {
                if (Session["NCIPL_cc"] == null)
                    return null;
                else
                    return (CreditCard)Session["NCIPL_cc"];
            }
            set { Session["NCIPL_cc"] = value; }
        }
        //ProductCollection shoppingcart
        //{
        //    get
        //    {
        //        if (Session["NCIPL_cart"] == null)
        //            return null;
        //        else
        //            return (ProductCollection)Session["NCIPL_cart"];
        //    }
        //    set { Session["NCIPL_cart"] = value; }
        //}
        ProductCollection shoppingcartV2   //***EAC 2nd version of shopping cart w/ virtual/linked pubs
        {
            get
            {
                if (Session["NCIPL_cartV2"] == null)
                    return null;
                else
                    return (ProductCollection)Session["NCIPL_cartV2"];
            }
            set { Session["NCIPL_cartV2"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (shoppingcartV2 == null || shoppingcartV2.Count<1){  //***EAC I need a shopping cart at this point
                GlobalUtils.Utils.ClearAllSessions();
                Response.Redirect("~/login.aspx", true);              
            }
            if (!GlobalUtils.Utils.isLoggedin())
            {  //***EAC I need a logged in user at this point
                GlobalUtils.Utils.ClearAllSessions();
                Response.Redirect("~/login.aspx", true);
            }
            string role = GlobalUtils.Utils.LoggedinRole();
            if (role != "NCIPL_LM" && role != "NCIPL_CC" && role != "NCIPL_POS")    //***EAC In the future, we disallow NCIPL_LM
            {
                throw (new ArgumentException("Your role is not allowed at this point."));
            }            
            if (!IsPostBack)
            {
                //NCIPL_CC
                if (GlobalUtils.UserRoles.getLoggedInUserId().Length == 0 || GlobalUtils.UserRoles.getLoggedInUserRole() < 1)
                {
                    string currASPXfilename = System.IO.Path.GetFileName(Request.Path).ToString();
                    Session["NCIPL_REGISTERREFERRER"] = currASPXfilename;
                    Response.Redirect("~/login.aspx?msg=invaliduser&redir=" + currASPXfilename);
                }
                
                Session["NCIPL_DONE"] = "false";
                steps1.Activate("cell4");

                cc = new CreditCard();  //***EAC Create a dummy credit card object                

                lblname.Text = this.shipto.Fullname ;
                lblOrg.Text = this.shipto.Organization;
                lbladdr1.Text = this.shipto.Addr1;
                lbladdr2.Text = this.shipto.Addr2;
                lblcity.Text = this.shipto.City;
                lblst.Text = this.shipto.State;
                lblzip.Text = this.shipto.Zip5 + ((this.shipto.Zip4.Trim() == "") ? "" : "-" + this.shipto.Zip4);
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

                pnlSplitOrder.Visible = (shoppingcartV2.SplitOrder == "1") ? true : false;

                if (this.billto.Organization.Trim().Length < 1) lbl2org.Visible = false;
                if (this.billto.Addr2.Trim().Length < 1) lbl2addr2.Visible = false;
                if (this.billto.Email.Trim().Length < 1) lbl2email.Visible = false;
                if (this.billto.Phone.Trim().Length < 1) lbl2phone.Visible = false;

                if (!shoppingcartV2.isFree2Order(shipto != null ? shipto.State : ""))
                {
                    //NCIPL_CC pnlPaymentInfo.Visible = true;
                    pnlPaymentInfo.Visible = false; //NCIPL_CC
                    lblCost.Text = shoppingcartV2.ShipCost.ToString("c") ;
                    btn2Shipping.Text = "Change Addresses or Payment Information";
                    CostRecoveryInd = "1"; //HITT 8716
                    divCCPrompt.Visible = true;
                }
                else
                {
                    pnlPaymentInfo.Visible = false;
                    pnlBillingInfo.Visible = false;
                    btn2Shipping.Text = "Change Address";
                    divCCPrompt.Visible = false;
                }

                grdItems.DataSource = this.shoppingcartV2 ;
                grdItems.DataBind();
                
                lblTotalItems.Text = this.shoppingcartV2.TotalQty.ToString();
                lblCost.Text = this.shoppingcartV2.ShipCost.ToString("c");

                if (Nerdos.Count > 0)
                {
                    ListNerdos.DataSource = Nerdos;
                    ListNerdos.DataBind();
                    pnlContentDownload.Visible = true;
                }
                //***EAC test the address
                if (shoppingcartV2.ShipMethod == "")
                    USPS(this.shipto);
                else if (shoppingcartV2.ShipMethod[0] == 'F')
                     FEDEX(shipto);
                else if (shoppingcartV2.ShipMethod[0] == 'U')
                     UPS(shipto);


                //***EAC check if order within the last 30 days
                //CheckRecentOrders(this.shipto);   //***EAC TODO: Enable this line when we go live in SOLAS

                if (btn2Submit.Enabled == true && ConfigurationManager.AppSettings["AllowCheckRecentOrder"] == "1") //NCIPLCC - Check only if other validations are okay and if web config setting value is 1
                    CheckRecentOrders(this.shipto);

                if (btn2Submit.Enabled == true) //NCIPLCC - Check for No Ship flag, only if other validations are okay
                    this.CheckNoShipFlag(this.shipto);

                /*Begin HITT 8716*/
                if (string.Compare(CostRecoveryInd, "") == 0)
                {
                   // lblCCText.Visible = false;
                    divCost.Visible = false;
                    divCCExplanation.Visible = false;
                }
                /*End HITT 8716*/
            }

            //Display the master page tabs 
            GlobalUtils.Utils UtilMethod = new GlobalUtils.Utils();
            if (Session["NCIPL_Pubs"] != null)
                Master.LiteralText = UtilMethod.GetTabHtmlMarkUp(Session["NCIPL_Qtys"].ToString(), "");
            else
                Master.LiteralText = UtilMethod.GetTabHtmlMarkUp("", "");
            UtilMethod = null;

        }
        KVPairCollection Nerdos = new KVPairCollection();
        

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

                if (p.ProductId.ToLower().EndsWith("c"))
                {
                    KVPair Nerdo = new KVPair();
                    //***EAc  @%$#*@# UI is giving me problems...hardcoding class for now
                    lblTitle.Text = "<span class=textLoud>Cover Only: </span>" + p.LongTitle;
                    Nerdo.Key = p.LongTitle;
                    Nerdo.Val = PubEnt.DAL2.DAL.GetNerdoURLByChild(p.ProductId);
                    Nerdos.Add(Nerdo);
                }
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
            if (Session["NCIPL_DONE"].ToString() == "true")
            {
                Response.Redirect("default.aspx");
            }
            steps1.Activate("cell5");
            Transaction t = new Transaction(this.shipto, this.billto, this.cc, this.shoppingcartV2);

            //Begin HITT 8329 (CR 21) Save The Raw Order ###############################
            //Save the Raw Order Details before credit card and before order upload call
            try
            {
                string Fax = "";
                string Province = ""; //Only for International Exhibit Order
                string Country = ""; //Only for International Exhibit Order
                string interfacename = "ROO"; //"NCIPL"; //NCIPL, ROO or EXHIBIT
                string international = ""; //Pass "1" for International Order (Currently only for Exhibit)
                string Zip = t.ShipTo.Zip5; //Zip5 for NCIPL, ROO and Exhibit Domestic Orders. International Zip for Exhibit International Order.

                string rawpubids = "";
                string rawpubqtys = "";

                string[] rawpubs = Session["NCIPL_Pubs"].ToString().Split(new Char[] { ',' });
                for (var i = 0; i < rawpubs.Length; i++)
                {
                    //if (pubs[i].Trim() != "")
                    if (string.Compare(rawpubs[i].Trim(), "", true) != 0)
                        rawpubids += rawpubs[i].Trim() + ",";
                }

                string[] rawqtys = Session["NCIPL_Qtys"].ToString().Split(new Char[] { ',' });
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
                if (t.CC.Cost > 0)
                    CostRecoveryInd = "1";

                DAL.DAL.SaveRawOrder(t.ShipTo.Organization,
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
                                    PubEnt.GlobalUtils.Utils.UserIPAddrress(),
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
            if (true) //EAC This used to be Swipe(cc)
            {
                int returnvalue = 0;
                int returnordernum = 0;
                string pubids = "";
                string pubqtys = "";

                string[] pubs = Session["NCIPL_Pubs"].ToString().Split(new Char[] { ',' });
                for (var i = 0; i < pubs.Length; i++)
                {
                    //if (pubs[i].Trim() != "")
                    if (string.Compare(pubs[i].Trim(), "", true) != 0)
                            pubids += pubs[i].Trim() + ",";
                }

                string[] qtys = Session["NCIPL_Qtys"].ToString().Split(new Char[] { ',' });
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

                //bool bSaveOrderFlag = false;
                //if (Session["SEARCHORDER_CUSTID"] != null)
                //{
                    //if (Session["SEARCHORDER_CUSTID"].ToString() != "")
                    //{
                        //bSaveOrderFlag = t.Update(Request.UserHostAddress, Session["SEARCHORDER_CUSTID"].ToString(), pubids, pubqtys, out returnvalue, out returnordernum);
                    //}
                    //else
                    //{
                        //bSaveOrderFlag = t.Save(Request.UserHostAddress, pubids, pubqtys, out returnvalue, out returnordernum);
                    //}
                //}
                //else
                //{
                    //bSaveOrderFlag = t.Save(Request.UserHostAddress, pubids, pubqtys, out returnvalue, out returnordernum);
                //}

                if (t.Save(PubEnt.GlobalUtils.Utils.UserIPAddrress(), pubids, pubqtys, out returnvalue, out returnordernum))                
                {
                    //JPJ TO DO: Display returnordernum on Screen.
                    
                    //***EAC Everything looks good at this point
                    Session["NCIPL_DONE"] = "true";
                    lblAVS_Shipping.Visible = false;
                    //NCIPLCC Label1.Text = "Confirmation";
                    //Label1.Visible = false; //NCIPLCC
                    steps1.Visible = false; //HITT 8716
                    Label2.Text = "";
                    btn2Shipping.Visible = false;
                    btn2Cart.Visible = false;
                    btn2Cancel.Visible = false;
                    btn2Submit.Visible = false;
                    LinkButton1.Visible = false;
                    pnlConfirm1.Visible = true;
                    pnlConfirm2.Visible = true;
                    pnlConfirm3.Visible = true;
                    LblThank.Visible = true;
                    lblorderdt.Text = System.DateTime.Today.ToShortDateString();
                    lblOrderNum.Text = returnordernum.ToString();
                    //***EAC The if-then section below is now obsolete 20130109
                    if (false && shipto.Email.Length > 0 && HasCover(this.shoppingcartV2))//if we have a valid shipto.email address
                    {
                        StringWriter sw = new StringWriter();
                        HtmlTextWriter writer = new HtmlTextWriter(sw);
                        string s;
                        
                        //pnlConfirm1.RenderControl(writer);
                        Panel1.RenderControl(writer);
                        pnlBillingInfo.RenderControl(writer);
                        //pnlPaymentInfo.RenderControl(writer);
                        grdItems.RenderControl(writer);
                        ListNerdos.RenderControl(writer);
                        s = sw.ToString();

                        PubEnt.DAL2.DAL.SaveEmail(shipto.Email, s);
                        try
                        {   //SYSTEM.WEB.MAIL -- i know its becoming obsolete
                            MailMessage msg = new MailMessage();
                            msg.From = ConfigurationManager.AppSettings["PubEntEmailAddress"];
                            msg.To = shipto.Email;
                            msg.Subject = "Order Confirmation";
                            msg.Body = s;
                            msg.BodyFormat = MailFormat.Html;
                            SmtpMail.Send(msg);
                        }
                        catch (Exception) { }
                    }
                    //Code for HITT 8716
                    string printertext = ""; //Used like a flag, will not hold any value
                    string printerShipping = "", printerBilling = "", printerPayment = "";

                    //Begin - NCIPLCC
                    string printerSplitOrder = "";
                    if (shoppingcartV2.SplitOrder == "1")
                    {
                        StringWriter swSplitOrder = new StringWriter();
                        HtmlTextWriter writerSplitOrder = new HtmlTextWriter(swSplitOrder);
                        pnlSplitOrder.RenderControl(writerSplitOrder);
                        printerSplitOrder = swSplitOrder.ToString();
                        Session["NCIPLCC_PrinterSplitOrder"]
                            = printerSplitOrder;
                    }
                    string printerdtandordnum = "";
                    StringWriter swDtandOrdnum = new StringWriter();
                    HtmlTextWriter writerDtandOrdnum = new HtmlTextWriter(swDtandOrdnum);
                    pnlConfirm3.RenderControl(writerDtandOrdnum);
                    printerdtandordnum = swDtandOrdnum.ToString();
                    swDtandOrdnum.Dispose();
                    writerDtandOrdnum.Dispose();
                    Session["NCIPLCC_PrinterOrderDtandOrderNum"] = printerdtandordnum;
                    //End - NCIPLCC

                    StringWriter swShipping = new StringWriter();
                    HtmlTextWriter writerShipping = new HtmlTextWriter(swShipping);
                    Panel1.RenderControl(writerShipping);
                    printerShipping = swShipping.ToString();

                    StringWriter swBilling = new StringWriter();
                    HtmlTextWriter writerBilling = new HtmlTextWriter(swBilling);
                    pnlBillingInfo.RenderControl(writerBilling);
                    printerBilling = swBilling.ToString();

                    //StringWriter swPayment = new StringWriter();
                    //HtmlTextWriter writerPayment = new HtmlTextWriter(swPayment);
                    //pnlPaymentInfo.RenderControl(writerPayment);
                    //printerPayment = swPayment.ToString();
                   
                    Session["NCIPL_PrinterFriendly"] = printertext;
                    Session["NCIPL_PrinterGrid"] = this.shoppingcartV2;
                    Session["NCIPL_PrinterShipping"] = printerShipping;
                    Session["NCIPL_PrinterBilling"] = printerBilling;
                    Session["NCIPL_PrinterPayment"] = printerPayment;
                    if (!shoppingcartV2.isFree2Order(shipto != null ? shipto.State : ""))
                        Session["NCIPL_PrinterCostRecovery"] = "True";
                    else
                        Session["NCIPL_PrinterCostRecovery"] = "False";
                    //End code
                    
                    ResetSessions();
                }
                else{
                    throw (new ApplicationException("Cannot save order."));
                }
            }
        }



        protected void CancelOrder(object sender, EventArgs e)
        {
            //todo should confirm with fancy popup control
            //reset all sessions here then kick back to default.aspx
            ResetSessions();
            Response.Redirect("home.aspx", true);
        }
        private void ResetSessions()
        {
            Session["SEARCHORDER_CUSTID"] = null;       //destroy
            
            Session["NCIPL_cart"] = null;       //destroy
            Session["NCIPL_cartV2"] = null; //NCIPL_CC //destroy
            Session["NCIPL_shipto"] = null;     //destroy
            Session["NCIPL_billto"] = null;     //destroy
            Session["NCIPL_cc"] = null;         //destroy
            Session["NCIPL_DONE"] = "";         //neither done nor finished

            //***EAC Create the session variables asap
            Session["NCIPL_Pubs"] = "";
            Session["NCIPL_Qtys"] = "";
            Session["PUBENT_SearchKeyword"] = "";
            Session["PUBENT_TypeOfCancer"] = "";
            Session["PUBENT_Subject"] = "";
            Session["PUBENT_Audience"] = "";
            Session["PUBENT_ProductFormat"] = "";
            Session["PUBENT_Language"] = "";
            Session["PUBENT_StartsWith"] = "";
            Session["PUBENT_Series"] = ""; //Or collection
            Session["PUBENT_NewOrUpdated"] = "";
            Session["PUBENT_Race"] = "";

            //Display the master page tabs 
            GlobalUtils.Utils UtilMethod = new GlobalUtils.Utils();
            if (Session["NCIPL_Pubs"] != null)
                Master.LiteralText = UtilMethod.GetTabHtmlMarkUp(Session["NCIPL_Qtys"].ToString(), "");
            else
                Master.LiteralText = UtilMethod.GetTabHtmlMarkUp("", "");
            UtilMethod = null;
        }
        private bool SaveOrder(CreditCard cc, Person shipto, Person billto, ProductCollection cart)
        {
            try
            {
                return (true);
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        #region DEPRECATED Credit Card Swipe Routine
        //private bool Swipe(CreditCard cc)
        //{
        //    return (true); //***EAC This routine is no longer necessary

        //    if (this.cc.Cost > 0.0)
        //    {
        //        string APPROVED = "<ssl_result_message>APPROV";
        //        // string xmldata = "xmldata=<txn><ssl_merchant_ID>502928</ssl_merchant_ID><ssl_user_id>webform</ssl_user_id><ssl_pin>PK0GAL</ssl_pin><ssl_transaction_type>ccsale</ssl_transaction_type><ssl_card_number>5000300020003003</ssl_card_number><ssl_exp_date>1209</ssl_exp_date><ssl_amount>12.3</ssl_amount><ssl_salestax>0.00</ssl_salestax><ssl_cvv2cvc2_indicator>Present</ssl_cvv2cvc2_indicator><ssl_cvv2cvc2>123</ssl_cvv2cvc2><ssl_invoice_number></ssl_invoice_number><ssl_customer_code>0</ssl_customer_code><ssl_first_name></ssl_first_name><ssl_last_name></ssl_last_name><ssl_avs_address>123 mystreet</ssl_avs_address><ssl_address2></ssl_address2><ssl_city></ssl_city><ssl_state></ssl_state><ssl_avs_zip>90210</ssl_avs_zip><ssl_phone></ssl_phone><ssl_email></ssl_email></txn>";
        //        ASCIIEncoding encoding = new ASCIIEncoding();

        //        //***EAC had to do this next 2 lines because of a bug in billto.Addr1.Substring(0,20)
        //        string temp = billto.Addr1;
        //        if (temp.Length > 20) temp = temp.Substring(0, 20);

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
        //            this.cc.TransID = RESP.Substring(i + 12, j - i - 12);

        //            //***EAC get CC Transaction ID
        //            i = RESP.IndexOf("<ssl_approval_code>");
        //            j = RESP.IndexOf("</ssl_approval_code>");
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
        #endregion

        public void USPS(Person p)
        {
            string returnValue, XmlString;
            WebClient instance = new WebClient();
            XmlString = "<AddressValidateRequest USERID=\"USER\"><Address ID=\"0\"> <Address1>" +p.Addr2+"</Address1> <Address2>" + p.Addr1 +"</Address2> <City>"+p.City+"</City> <State>"+p.State+"</State> <Zip5></Zip5> <Zip4></Zip4></Address> </AddressValidateRequest>";
            Uri siteUri = new Uri("http://production.shippingapis.com/ShippingAPI.dll?API=Verify&XML=" + XmlString.Replace("&","%20"));
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

        protected void UPS(Person p)
        {
            btn2Submit.Enabled = true;
            lblAVS_Shipping.Visible = false;           
            try
            {
                UPSAddressValidation.XAVService xavSvc = new UPSAddressValidation.XAVService();
                UPSAddressValidation.XAVRequest xavRequest = new UPSAddressValidation.XAVRequest();
                UPSAddressValidation.UPSSecurity upss = new UPSAddressValidation.UPSSecurity();
                UPSAddressValidation.UPSSecurityServiceAccessToken upssSvcAccessToken = new UPSAddressValidation.UPSSecurityServiceAccessToken();
                upssSvcAccessToken.AccessLicenseNumber = ConfigurationManager.AppSettings["UPSAccessLicenseNumber"];
                upss.ServiceAccessToken = upssSvcAccessToken;
                UPSAddressValidation.UPSSecurityUsernameToken upssUsrNameToken = new UPSAddressValidation.UPSSecurityUsernameToken();
                upssUsrNameToken.Username = ConfigurationManager.AppSettings["UPSUserName"];
                upssUsrNameToken.Password = ConfigurationManager.AppSettings["UPSPassword"];
                upss.UsernameToken = upssUsrNameToken;
                xavSvc.UPSSecurityValue = upss;
                UPSAddressValidation.RequestType request = new UPSAddressValidation.RequestType();

                //Below code contains dummy data for reference. Please update as required.
                String[] requestOption = { "1" };//***EAC 1 - Address Validation 2 - Address Classification 3 - Address Validation and Address Classification.
                request.RequestOption = requestOption;
                xavRequest.Request = request;
                UPSAddressValidation.AddressKeyFormatType addressKeyFormat = new UPSAddressValidation.AddressKeyFormatType();
                String[] addressLine = { p.Addr1, p.Addr2, "" };
                addressKeyFormat.AddressLine = addressLine;
                addressKeyFormat.ConsigneeName = "dummy";                  //***EAC seems unused

                addressKeyFormat.PoliticalDivision2 = p.City ;
                addressKeyFormat.PoliticalDivision1 = p.State;
                addressKeyFormat.PostcodePrimaryLow = p.Zip5;
                addressKeyFormat.PostcodeExtendedLow = p.Zip4;
                addressKeyFormat.CountryCode = "US";
                xavRequest.AddressKeyFormat = addressKeyFormat;
                System.Net.ServicePointManager.CertificatePolicy = new PubEnt.UPS.TrustAllCertificatePolicy();
                UPSAddressValidation.XAVResponse xavResponse = xavSvc.ProcessXAV(xavRequest);

                if (xavResponse.ItemElementName != UPSAddressValidation.ItemChoiceType.ValidAddressIndicator)   //***EAC TODO: if (xavResponse.says_address_is bad  ValidAddressIndicator, AmbiguousAddressIndicator
                {
                    lblAVS_Shipping.Text = "Address Not Found (UPS)";
                    btn2Submit.Enabled = false;
                    lblAVS_Shipping.Visible = true;
                }
                else
                {
                    lblAVS_Shipping.Text = "SUCCESS";
                    btn2Submit.Enabled = true;
                    lblAVS_Shipping.Visible = false;
                }
            }

            catch (Exception ex)
            {
                //Write to log
                Microsoft.Practices.EnterpriseLibrary.Logging.LogEntry logEnt = new Microsoft.Practices.EnterpriseLibrary.Logging.LogEntry();
                logEnt.Message = "\r\n" + "UPS Address Verification error." + "\r\n" + "Source: " + ex.Source + "\r\n" + "Description: " + ex.Message + "\r\n" + "Stack Trace: " + ex.StackTrace;
                Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(logEnt, "Logs");
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
            btn2Submit.Enabled = true;
            lblAVS_Shipping.Visible = false;            

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
                            lblAVS_Shipping.Text = "Address Not Found (FedEx)";
                            btn2Submit.Enabled = false;
                            lblAVS_Shipping.Visible = true;
                        }
                        else
                        {
                            lblAVS_Shipping.Text = "SUCCESS";
                            btn2Submit.Enabled = true;
                            lblAVS_Shipping.Visible = false;

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
                //yma add this since fedex return exception when no valid address found
                lblAVS_Shipping.Text = "Address Not Found (FedEx)";
                btn2Submit.Enabled = false;
                lblAVS_Shipping.Visible = true;

                //Write to log
                Microsoft.Practices.EnterpriseLibrary.Logging.LogEntry logEnt = new Microsoft.Practices.EnterpriseLibrary.Logging.LogEntry();
                logEnt.Message = "\r\n" + "FedEx Address Verification error." + "\r\n" + "Source: " + ex.Source + "\r\n" + "Description: " + ex.Message + "\r\n" + "Stack Trace: " + ex.StackTrace;
                Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(logEnt, "Logs");
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

        protected void Nerdos_IDB(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                KVPair kv = (KVPair)e.Item.DataItem;
                Label lblNerdoTitle = (Label)e.Item.FindControl("lblNerdoTitle");
                lblNerdoTitle.Text = kv.Key;
                HyperLink lnkCanType = (HyperLink)e.Item.FindControl("lnkNerdo");
                
                lnkCanType.Text = kv.Val;
                lnkCanType.NavigateUrl =  kv.Val;
            }
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
        public void CheckRecentOrders(Person p)
        {            
            DateTime mostrecent = DAL2.DAL.MostRecentOrder(p.Addr1, p.Zip5);
            TimeSpan  diff = DateTime.Now.Subtract(mostrecent);
            //NCIPLCC if (diff.Days > 0) 
            //if (diff.Days >= 0 && diff.Days <= 30)
            if (diff.Days <= 30)
            {
                lblAVS_Shipping.Text = "Your last order was on " + mostrecent.ToShortDateString() + ". Please wait 30 days before ordering again.";
                btn2Submit.Enabled = false;
                lblAVS_Shipping.Visible = true;
            }
            else
            {
                lblAVS_Shipping.Text = "SUCCESS";
                btn2Submit.Enabled = true;
                lblAVS_Shipping.Visible = false;
            }
                
        }
        
        /*** Check whether the cusomer has a no-ship flag set in the database table ***/
        /*** during the last one month ***/
        public void CheckNoShipFlag(Person p) 
        {
            int NoShipFlag = DAL2.DAL.getNoShipFlag(p.Addr1, p.Zip5);
            if (NoShipFlag == 1)
            {
                //lblAVS_Shipping.Text = "Your last order was on " + mostrecent.ToShortDateString() + ". Please wait 30 days before ordering again.";
                 //lblAVS_Shipping.Visible = true;
                if (GlobalUtils.UserRoles.getLoggedInUserRole() == 3)
                {
                    btn2Submit.Enabled = false;
                    lblNoShipMessage.Visible = true;
                    lblNoShipMessage.Text = "<br/>There is a \"no ship\" message associated with the caller. Please refer caller to NCIDC staff.";
                    btn2Submit.Style.Add("cursor", "default");
                }
                else if (GlobalUtils.UserRoles.getLoggedInUserRole() == 2)
                {
                    btn2Submit.Enabled = false;
                    lblNoShipMessage.Visible = true;
                    lblNoShipMessage.Text = "<br/>There is a \"no ship\" message associated with the caller. Please verify that there are no outstanding rebills to clear before placing the order.";
                    //btn2Submit.Style.Add("cursor", "default");
                }

            }
            else
            {
                //lblAVS_Shipping.Text = "SUCCESS";
                btn2Submit.Enabled = true;
                //lblAVS_Shipping.Visible = false;
                lblNoShipMessage.Visible = false;
            }
        }

    }
}
