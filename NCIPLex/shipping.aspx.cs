using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NCIPLex.DAL;
using NCIPLex.BLL;


using System.Data;
using System.Configuration;
using System.Collections;
//using System.Web;
using System.Web.Security;
//using System.Web.UI;
//using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
//using System.Collections.Generic;
using System.Data.Common;
using System.Xml;
using System.Net;
using System.Web.Services.Protocols;


using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Logging;
//using Microsoft.Practices.ObjectBuilder2;
using System.Text.RegularExpressions;
using NCIPLex.GlobalUtils;

//using PubEnt.AddressValidationServiceWebReference;


namespace NCIPLex
{
    public partial class shipping : System.Web.UI.Page
    {
        private bool IsOrderIntl = false;
        
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Utils.ValidateRedirect().Length > 0) //Important check
                Response.Redirect(Utils.ValidateRedirect(), true);
            
            if (this.shoppingcart == null || this.shoppingcart.Count < 1)
            {  //***EAC I need a shopping cart at this point
                ResetSessions();
                Response.Redirect("home.aspx?missingcart=true", true);
            }

            IsOrderIntl = Utils.IsOrderInternational(); //NCIPLex - International or domestic

            if (!IsPostBack)
            {   
                steps1.Activate("cell3");

                this.shoppingcartV2 = VirtualizedCart (this.shoppingcart);
                if (this.cc == null) this.cc = new CreditCard();
                    this.cc.Cost = this.shoppingcartV2.Cost;  //assume cart exists...if not error handler will take care of it

                for (int i = DateTime.Now.Year; i < DateTime.Now.Year+9; i++)
			    {                   
                    drpyr.Items.Add(new ListItem(i.ToString(),i.ToString().Substring(2)));
			    }

                ddlIntlCountry.DataSource = NCIPLex.DAL.SQLDataAccess.GetKVPairStr("SP_NCIPLex_GetCountry");
                ddlIntlCountry.DataTextField = "Val";
                //ddlIntlCountry.DataValueField = "Key";
                ddlIntlCountry.DataValueField = "Val";
                ddlIntlCountry.DataBind();
                ddlIntlCountry.Items.Insert(0, new ListItem("[Select from the list]", ""));
                
                if (this.shipto != null)
                {
                    #region Restore ship vals
                    try
                    {
                        txtzip5.Text = shipto.Zip5;
                        txtzip4.Text = shipto.Zip4;
                        txtcity.Text = shipto.City;
                        txtstate.Text = shipto.State;
                        //foreach (KVPair city in Zipcode.GetCities(Convert.ToInt32(shipto.Zip5)))
                        //    drpcity.Items.Add(new ListItem(city.Key, city.Key));
                        //drpcity.SelectedValue = shipto.City;
                        txtcity.Text = shipto.City;
                        txtcity.Visible = true;
                        drpcity.Visible = false;
                        txtname.Text = shipto.Fullname;
                        txtorg.Text = shipto.Organization;
                        txtaddr1.Text = shipto.Addr1;
                        txtaddr2.Text = shipto.Addr2;
                        txtphone.Text = shipto.Phone;
                        txtemail.Text = shipto.Email;

                        if (IsOrderIntl)
                        {
                            txtIntlZip.Text = shipto.Zip5;
                            txtIntlState.Text = shipto.State;
                            //txtIntlCountry.Text = shipto.Country;
                            ddlIntlCountry.SelectedValue = shipto.Country;
                        }

                    }
                    catch (Exception)
                    {                        
                    }

                    #endregion

                }

                if (this.billto != null)
                {
                    #region Restore bill vals
                    try
                    {

                    txt2zip5.Text = billto.Zip5;
                    txt2zip4.Text = billto.Zip4;
                    txt2city.Text = billto.City;
                    txt2state.Text = billto.State;
                    foreach (KVPair city in Zipcode.GetCities(Convert.ToInt32(billto.Zip5)))
                        drpcity2.Items.Add(new ListItem(city.Key, city.Key));
                    drpcity2.SelectedValue = billto.City;                       
                    txt2name.Text = billto.Fullname;
                    txt2org.Text = billto.Organization;
                    txt2addr1.Text = billto.Addr1;
                    txt2addr2.Text = billto.Addr2;
                    txt2phone.Text = billto.Phone;
                    txt2email.Text = billto.Email; 
                    }
                    catch (Exception)
                    {
                    }
                    #endregion
                }

                //JPJ 03-10-11 NCIPLex does not accept credit card orders
                //if (this.cc != null && this.cc.Cost > 0.0)
                //{
                //    Label1.Text = "Shipping & Payment";
                //    pnlPaymentInfo.Visible = true;
                //    #region Restor CC field values
                //    drpcard.SelectedValue = cc.Company;
                //    txtccnum.Text = cc.CCnum;
                //    txtcvv2.Text = cc.CVV2;
                //    drpmonth.SelectedValue = cc.ExpMon;
                //    drpyr.SelectedValue = cc.ExpYr;
                //    lblCost.Text = this.cc.Cost.ToString("c");
                //    #endregion
                //}
                //else
                //{
                    Label1.Text = "Shipping";
                    pnlPaymentInfo.Visible = false;
                    pnlBIllingInfo.Visible = false;
                    chkSameaddress.Visible = false;
                    lblBill.Visible = false;
                    lblShip.Visible = false;
                    rowIntlCountry.Visible = false;
                //} 

                    if (IsOrderIntl)
                    {
                        lbltxtzip5.Visible = false;
                        txtzip5.Visible = false;
                        lbltxtzip4.Visible = false;
                        txtzip4.Visible = false;
                        lbltxtstate.Visible = false;
                        txtstate.Visible = false;

                        txtphone.ToolTip = "";

                        RegularExpressionValidator3.Visible = false;
                        RegularExpressionValidator6.Visible = false;
                        RequiredFieldValidator13.Visible = false;
                        RegularExpressionValidator5.Visible = false;
                        RequiredFieldValidator6.Visible = false;


                        lblIntltxtzip.Visible = true;
                        txtIntlZip.Visible = true;
                        lbltxtIntlState.Visible = true;
                        txtIntlState.Visible = true;
                        rowIntlCountry.Visible = true;
                        divlblCountry.Visible = true;
                        lbltxtIntlCountry.Visible = true;
                        divtxtCountry.Visible = true;
                        //txtIntlCountry.Visible = true;
                        ddlIntlCountry.Visible = true;
                        RequiredFieldIntlZip.Visible = true;
                        RequiredFieldIntlCountry.Visible = true;
                    }
            
            }

            //Display the master page tabs 
            GlobalUtils.Utils UtilMethod = new GlobalUtils.Utils();
            if (Session["NCIPLEX_Pubs"] != null)
                Master.LiteralText = UtilMethod.GetTabHtmlMarkUp(Session["NCIPLEX_Qtys"].ToString(), "");
            else
                Master.LiteralText = UtilMethod.GetTabHtmlMarkUp("", "");
            UtilMethod = null;
        }

        protected void chkSameaddress_CheckedChanged(object sender, EventArgs e)
        {
            //if (chkSameaddress.Checked)
            //{
            //    txt2zip5.Text = txtzip5.Text;
            //    txt2zip4.Text = txtzip4.Text;
            //    txt2city.Text = txtcity.Text;
            //    txt2state.Text = txtstate.Text;
            //    txt2name.Text = txtname.Text;
            //    txt2org.Text = txtorg.Text;
            //    txt2addr1.Text = txtaddr1.Text;
            //    txt2addr2.Text = txtaddr2.Text;
            //    txt2phone.Text = txtphone.Text;
            //    txt2email.Text = txtemail.Text;

            //    txt2zip5.ReadOnly = true;
            //    txt2zip4.ReadOnly = true;
            //    txt2city.ReadOnly = true;
            //    txt2state.ReadOnly = true;
            //    txt2name.ReadOnly = true;
            //    txt2org.ReadOnly = true;
            //    txt2addr1.ReadOnly = true;
            //    txt2addr2.ReadOnly = true;
            //    txt2phone.ReadOnly = true;
            //    txt2email.ReadOnly = true;
            //}
            //else
            //{
            //    txt2zip5.ReadOnly = false;
            //    txt2zip4.ReadOnly = false;
            //    txt2city.ReadOnly = false;
            //    txt2state.ReadOnly = false;
            //    txt2name.ReadOnly = false;
            //    txt2org.ReadOnly = false;
            //    txt2addr1.ReadOnly = false;
            //    txt2addr2.ReadOnly = false;
            //    txt2phone.ReadOnly = false;
            //    txt2email.ReadOnly = false;
            //}
        }

        protected void TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        protected void TextBox4_TextChanged(object sender, EventArgs e)
        {

        }

        protected void TextBox7_TextChanged(object sender, EventArgs e)
        {

        }

        protected void TextBox8_TextChanged(object sender, EventArgs e)
        {

        }

        protected void TextBox21_TextChanged(object sender, EventArgs e)
        {

        }

        protected void LinkButton2_Click(object sender, EventArgs e)
        {


        }
        protected ProductCollection VirtualizedCart(ProductCollection cart)
        {
            ProductCollection temp = new ProductCollection();
            foreach (Product item in this.shoppingcart)
            {
                if (item.BookStatus == "V")
                {

                    foreach (Product kit in NCIPLex.DAL2.DAL.GetVirtualKits(item.PubId))
                    {

                        kit.NumQtyOrdered = kit.NumQtyOrdered * item.NumQtyOrdered;
                        temp.Add(kit);
                    }
                }
                else
                {
                    temp.Add(item);
                }
            }

            return (temp);
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            #region Toggle Billing Validators
            if (chkSameaddress.Checked)
            {
                RequiredFieldValidator7.Enabled = false;
                RequiredFieldValidator15.Enabled = false;
                RequiredFieldValidator9.Enabled = false;
                RequiredFieldValidator10.Enabled = false;
                RequiredFieldValidator11.Enabled = false;
                RequiredFieldValidator12.Enabled = false;
                RequiredFieldValidator14.Enabled = false;

                RegularExpressionValidator2.Enabled = false;
                RegularExpressionValidator4.Enabled = false;
            }
            else
            {
                RequiredFieldValidator7.Enabled = true;
                RequiredFieldValidator15.Enabled = true;
                RequiredFieldValidator9.Enabled = true;
                RequiredFieldValidator10.Enabled = true;
                RequiredFieldValidator11.Enabled = true;
                RequiredFieldValidator12.Enabled = true;
                RequiredFieldValidator14.Enabled = true;

                RegularExpressionValidator2.Enabled = true;
                RegularExpressionValidator4.Enabled = true;
            } 
            #endregion
            if (Page.IsValid)
            {
                //*** EAC We passed .Net validation so now just
                //*** validate the lengths so AppScan doesn't get angry
                if (txtzip5.Text.Length + txtzip4.Text.Length + txtcity.Text.Length + txtstate.Text.Length +
                    txtname.Text.Length + txtorg.Text.Length + txtaddr1.Text.Length + txtaddr2.Text.Length + txtphone.Text.Length + txtemail.Text.Length > 220)
                    throw (new ArgumentOutOfRangeException("Shipping object is too big"));

                if (txt2zip5.Text.Length + txt2zip4.Text.Length + txt2city.Text.Length + txt2state.Text.Length +
                    txt2name.Text.Length + txt2org.Text.Length + txt2addr1.Text.Length + txt2addr2.Text.Length + txt2phone.Text.Length + txt2email.Text.Length > 220)
                    throw (new ArgumentOutOfRangeException("Billing object is too big"));
                
                
                //JPJ - To Be Changed
                string strCounty = "";
                if (IsOrderIntl)
                    //strCounty = txtIntlCountry.Text;
                    strCounty = ddlIntlCountry.SelectedItem.Text;
                
                //***EAC At this point, data looks clean
                if (!IsOrderIntl)
                    this.shipto = new Person(1, txtname.Text, txtorg.Text, txtemail.Text, txtaddr1.Text, txtaddr2.Text, (txtcity.Visible == true ? txtcity.Text : drpcity.SelectedValue), txtstate.Text, txtzip5.Text, txtzip4.Text, txtphone.Text, strCounty);
                else
                    this.shipto = new Person(1, txtname.Text, txtorg.Text, txtemail.Text, txtaddr1.Text, txtaddr2.Text, txtcity.Text, txtIntlState.Text, txtIntlZip.Text, "", txtphone.Text, strCounty);
                this.cc = new CreditCard(drpcard.SelectedValue, txtccnum.Text, drpmonth.SelectedValue, drpyr.SelectedValue, txtcvv2.Text, this.cc.Cost, drpcard.SelectedValue );

                //03/31/11 JPJ -(Keep same as in NCIPL) this.billto = this.shipto; //NCIPLex - does not need a billto person, this is just to have some default value
                if (chkSameaddress.Checked) //NCIPLex
                    this.billto = this.shipto;
                else
                    this.billto = new Person(1, txt2name.Text, txt2org.Text, txt2email.Text, txt2addr1.Text, txt2addr2.Text, (txt2city.Visible == true ? txt2city.Text : drpcity2.SelectedValue), txt2state.Text, txt2zip5.Text, txt2zip4.Text, txt2phone.Text, strCounty);

                //Response.Redirect("verify.aspx", true);
                //Response.Redirect("test.aspx", true);
                Response.Redirect("verify_order.aspx", true);
            }
        }
        protected void Button2_Click(object sender, EventArgs e)
        {
            //JPJ 03-10-11 NCIPLex does not accept credit card orders
            
            //#region Toggle Billing Validators
            //if (chkSameaddress.Checked)
            //{
            //    RequiredFieldValidator7.Enabled = false;
            //    RequiredFieldValidator15.Enabled = false;
            //    RequiredFieldValidator9.Enabled = false;
            //    RequiredFieldValidator10.Enabled = false;
            //    RequiredFieldValidator11.Enabled = false;
            //    RequiredFieldValidator12.Enabled = false;

            //    RegularExpressionValidator2.Enabled = false;
            //    RegularExpressionValidator4.Enabled = false;
            //}
            //else
            //{
            //    RequiredFieldValidator7.Enabled = true;
            //    RequiredFieldValidator15.Enabled = true;
            //    RequiredFieldValidator9.Enabled = true;
            //    RequiredFieldValidator10.Enabled = true;
            //    RequiredFieldValidator11.Enabled = true;
            //    RequiredFieldValidator12.Enabled = true;

            //    RegularExpressionValidator2.Enabled = true;
            //    RegularExpressionValidator4.Enabled = true;
            //}
            //#endregion
            //if (Page.IsValid)
            //{
            //    //*** EAC We passed .Net validation so now just
            //    //*** validate the lengths so AppScan doesn't get angry
            //    if (txtzip5.Text.Length + txtzip4.Text.Length + txtcity.Text.Length + txtstate.Text.Length +
            //        txtname.Text.Length + txtorg.Text.Length + txtaddr1.Text.Length + txtaddr2.Text.Length + txtphone.Text.Length + txtemail.Text.Length > 220)
            //        throw (new ArgumentOutOfRangeException("Shipping object is too big"));

            //    if (txt2zip5.Text.Length + txt2zip4.Text.Length + txt2city.Text.Length + txt2state.Text.Length +
            //        txt2name.Text.Length + txt2org.Text.Length + txt2addr1.Text.Length + txt2addr2.Text.Length + txt2phone.Text.Length + txt2email.Text.Length > 220)
            //        throw (new ArgumentOutOfRangeException("Billing object is too big"));


            //    //***EAC At this point, data looks clean
            //    this.shipto = new Person(1, txtname.Text, txtorg.Text, txtemail.Text, txtaddr1.Text, txtaddr2.Text, txtcity.Text, txtstate.Text, txtzip5.Text, txtzip4.Text, txtphone.Text);
            //    this.cc = new CreditCard(drpcard.SelectedValue, txtccnum.Text, drpmonth.SelectedValue, drpyr.SelectedValue, txtcvv2.Text, this.cc.Cost, drpcard.SelectedValue );
            //    if (chkSameaddress.Checked)
            //        this.billto = this.shipto;
            //    else
            //        this.billto = new Person(1, txt2name.Text, txt2org.Text, txt2email.Text, txt2addr1.Text, txt2addr2.Text, txt2city.Text, txt2state.Text, txt2zip5.Text, txt2zip4.Text, txt2phone.Text);


            //    FEDEX(shipto);
            //    //Response.Redirect("verify.aspx", true);
            //}
        }

        //JPJ 03-10-11 NCIPLex does not accept CC orders, so FedEx shipping is not used.
        //protected void FEDEX(Person p)
        //{
        //    string straddress2 = txtaddr1.Text;
        //    string strzipcode = txtzip5.Text;
        //    string address = string.Empty;
        //    string zipfour = string.Empty;
        //    string ststate = string.Empty;
        //    string zip = string.Empty;
        //    string[] split = null;
        //    string delimStr = "-";
        //    char[] delimiter = delimStr.ToCharArray();

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

        //             foreach (AddressValidationResult result in reply.AddressResults)
        //            {
        //                foreach (ProposedAddressDetail detail in result.ProposedAddressDetails)
        //                {
        //                    address = detail.Address.StreetLines[0];
        //                    zip = detail.Address.PostalCode;
        //                    split = zip.Split(delimiter);
        //                    zip = split[0];
        //                    //zipfour = split[1];
        //                }
        //                if (score < 20) //(zipfour == null || zipfour.Length < 4)
        //                {
        //                    AVMessage.Text = "Address Not Found";//intentionally missing period  so I can tell if its usps or fedex
        //                }
        //                else
        //                {
        //                    AVMessage.Text = "SUCCESS";
        //                 }
        //            }
        //        }
        //    }
        //            catch (SoapException ex)
        //            {
        //            }
        //            catch (Exception ex)
        //            {
        //            }
        //}
   
        /*JPJ 03-10-11 NCIPlex does not accept orders with more than 20 items. FedEx Address check is not needed.*/
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
        //public  void USPS(Person p)
        //{
        //    string returnValue, XmlString;
        //    WebClient instance = new WebClient();
        //    XmlString = createXMl(p);
        //    Uri siteUri = new Uri("http://production.shippingapis.com/ShippingAPI.dll?API=Verify&XML=" + XmlString);
        //    WebRequest wr = WebRequest.Create(siteUri);
        //    returnValue = instance.DownloadString(siteUri);

        //    try
        //    {
        //        XmlDocument XmlDoc = new XmlDocument();
        //        XmlDoc.LoadXml(returnValue);
        //        AVMessage.Text = returnValue;
        //        XmlNode node;
        //        node = XmlDoc.SelectSingleNode("//AddressValidateResponse/Address/ReturnText");
        //        if (node != null)
        //        {
        //            AVMessage.Text = node.InnerText;
        //        }
        //        //if (returnValue.Contains("Error"))
        //        //{
        //        //    Strzipfour = "Invalid Address";
        //        //}
        //        //else
        //        //{
        //        //    Straddr2 = XmlDoc.FirstChild.NextSibling.LastChild.FirstChild.InnerText;
        //        //    Strcty = XmlDoc.FirstChild.NextSibling.LastChild.FirstChild.NextSibling.InnerText;
        //        //    Strst = XmlDoc.FirstChild.NextSibling.LastChild.FirstChild.NextSibling.NextSibling.InnerText;
        //        //    Strzip = XmlDoc.FirstChild.NextSibling.LastChild.FirstChild.NextSibling.NextSibling.NextSibling.InnerText;
        //        //    Strzipfour = XmlDoc.FirstChild.NextSibling.LastChild.LastChild.FirstChild.InnerText;

        //        //    address2.Text = Straddr2;
        //        //    int count = city.Items.Count;
        //        //    for (int iRetValue = count - 1; iRetValue >= 0; iRetValue--)
        //        //    {
        //        //        city.Items.RemoveAt(iRetValue);
        //        //    }
        //        //    city.Items.Add(Strcty);
        //        //    zipcode.Text = Strzip;
        //        //}
        //        //if (Strzipfour.Length > 4)
        //        //{
        //        //    myUrl = ("ErrorPage.aspx?Error=" + Strzipfour);
        //        //    Response.Redirect(myUrl);
        //        //}
        //        //else
        //        //{
        //        //    zipplusfour.Text = Strzipfour;
        //        //    AVMessage.Text = "The Address has been verified";
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Error message here.", ex);
        //    }
        //}
        #endregion
        protected void Zip_Changed(object sender, EventArgs e)
        {
            if (txtzip5.Text.Length == 5)
            {
                //***EAC HITT 7460 
                drpcity.Items.Clear();
                txtstate.Text = "";
                try
                {
                    foreach (KVPair city in Zipcode.GetCities(Convert.ToInt32(txtzip5.Text)))
                    {
                        drpcity.Items.Add(new ListItem(city.Key, city.Key));
                        txtstate.Text = city.Val; //doesn't hurt to do this n times
                    }
                    txtcity.Visible = true;     //**just in case the line below throws an error
                    drpcity.Visible = false;    //**just in case the line below throws an error
                    txtcity.Text = drpcity.Items[0].Text;
                    if (drpcity.Items.Count <= 1)
                    {                        
                        txtcity.Visible = true;
                        drpcity.Visible = false;
                        lbltxtcity.AssociatedControlID = "txtcity";
                    }
                    else
                    {
                        txtcity.Visible = false;
                        drpcity.Visible = true;
                        lbltxtcity.AssociatedControlID = "drpcity";
                    }
                }
                catch { }
            }
        }

        protected void BillZip_Changed(object sender, EventArgs e)
        {
            if (txt2zip5.Text.Length == 5)
            {
                //***EAC HITT 7460 
                drpcity2.Items.Clear();
                txt2state.Text = "";
                try
                {
                    foreach (KVPair city in Zipcode.GetCities(Convert.ToInt32(txt2zip5.Text)))
                    {
                        drpcity2.Items.Add(new ListItem(city.Key, city.Key));
                        txt2state.Text = city.Val; //doesn't hurt to do this n times
                    }
                    txt2city.Visible = true;     //**just in case the line below throws an error
                    drpcity2.Visible = false;    //**just in case the line below throws an error
                    txt2city.Text = drpcity2.Items[0].Text;
                    if (drpcity2.Items.Count <= 1)
                    {
                        txt2city.Visible = true;
                        drpcity2.Visible = false;
                        lbltxt2city.AssociatedControlID = "txt2city";
                    }
                    else
                    {
                        txt2city.Visible = false;
                        drpcity2.Visible = true;
                        lbltxt2city.AssociatedControlID = "drpcity2";
                    }
                }
                catch { }
            }

            //Zipcode z = Zipcode.GetCSZ(Int32.Parse(txt2zip5.Text));
            //txt2city.Text = z.City;
            //txt2state.Text = z.State;
        }


        private void ResetSessions()
        {

            Session["NCIPLEX_cart"] = null;       //destroy
            Session["NCIPLEX_shipto"] = null;     //destroy
            Session["NCIPLEX_billto"] = null;     //destroy
            Session["NCIPLEX_cc"] = null;         //destroy
            Session["NCIPLEX_DONE"] = "";         //neither done nor finished
        }

        protected void txtorg_TextChanged(object sender, EventArgs e)
        {

        }

        protected void chkSameaddress_CC(object sender, EventArgs e)
        {
            if (chkSameaddress.Checked)
            {
                pnlBIllingInfo.Enabled  = false;
            }
            else {
                pnlBIllingInfo.Enabled = true;
            }

        }

    }
}
