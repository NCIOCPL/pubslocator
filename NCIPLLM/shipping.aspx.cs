using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PubEnt.DAL;
using PubEnt.BLL;


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
using Microsoft.Practices.ObjectBuilder2;
using System.Text.RegularExpressions;

using PubEnt.AddressValidationServiceWebReference; 


namespace PubEnt
{
    public partial class shipping : System.Web.UI.Page
    {
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
                {
                    //NCPL_CC if (GlobalUtils.Utils.isLoggedin())
                    //NCPL_CC     return shipto = DAL2.DAL.GetShippingInfo(GlobalUtils.Utils.LoggedinUser());
                    //NCPL_CC else
                        return null;
                }
                else
                    return (Person)Session["NCIPL_shipto"];
            }
            set { Session["NCIPL_shipto"] = value; }
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
        ProductCollection shoppingcartV2   //***EAC We now have shoppingcart == shoppingcartV2 (20130420)
        {
            get
            {
                if (Session["NCIPL_cart"] == null)
                    return null;
                else
                    return (ProductCollection)Session["NCIPL_cart"];
            }
            set { Session["NCIPL_cart"] = value; }
        }
 
        protected void Page_Load(object sender, EventArgs e)
        {
            if (shoppingcartV2 == null || shoppingcartV2.Count < 1)
            {  //***EAC I need a shopping cart at this point
                GlobalUtils.Utils.ClearAllSessions();
                Response.Redirect("~/login.aspx", true);
            }
            if (!GlobalUtils.Utils.isLoggedin())
            {  //***EAC I need a logged in user at this point
                GlobalUtils.Utils.ClearAllSessions();
                Response.Redirect("~/login.aspx", true);
            }
            string role = GlobalUtils.Utils.LoggedinRole();
            if (role != "NCIPL_LM")
            {
                throw (new ArgumentException("Your role is not allowed at this point."));
            }
            if (!IsPostBack)
            {
                Session["NCIPLCC_PrinterSplitOrder"] = null; //NCIPL_CC
                steps1.Activate("cell3");

                //NCIPL_CC - Populate Customer Type Dropdown
                KVPairCollection kvpairColl = KVPair.GetKVPair("sp_NCIPLLM_getTypeOfCustomer");
                foreach (KVPair kvpair in kvpairColl)
                {
                    ListItem li = new ListItem();
                    li.Value = kvpair.Key;
                    li.Text = kvpair.Val;
                    drpCustomerType.Items.Add(li);
                }
                kvpairColl = null;
                
                //NCIPL_CC - Populate Order Media Dropdown
                KVPairCollection kvpairColl2 = KVPair.GetKVPair("sp_NCIPLLM_getOrderMedia");
                foreach (KVPair kvpair in kvpairColl2)
                {
                    ListItem li = new ListItem();
                    //li.Value = kvpair.Key;
                    li.Value = kvpair.Val; //Non-critical data. Just use description to make the data retrieval from tbl_orderheader easy.
                    li.Text = kvpair.Val;
                    drpOrderMedia.Items.Add(li);
                }
                kvpairColl2 = null;

                drpDelivery.DataSource = DAL2.DAL.GetAllShippingMethods();
                drpDelivery.DataTextField = "Val";
                drpDelivery.DataValueField = "Key";
                drpDelivery.DataBind();
                drpDelivery.Items.Insert(0, new ListItem("USPS", "S1"));
                drpDelivery.Items.Insert(0, new ListItem("Courier", "C1"));
                drpDelivery.Items.Insert(0, new ListItem("[Select a Shipping Method]", ""));

                drpCountry.DataSource       = drp2Country.DataSource = DAL2.DAL.GetCountries();
                drpCountry.DataTextField    = drp2Country.DataTextField = "Val";
                drpCountry.DataValueField   = drp2Country.DataValueField = "Key";
                drpCountry.DataBind();
                drp2Country.DataBind();
                drpCountry.Items.Insert(0, new ListItem("UNITED STATES", "US"));
                drp2Country.Items.Insert(0, new ListItem("UNITED STATES", "US"));

                BindShippingInfo();   //Restore ship vals
                BindBillingInfo();
                BindExtraInfo();

                BindTotals(shipto != null ? shipto.State : "");
            }


        }

        protected void chkSameaddress_CheckedChanged(object sender, EventArgs e)
        {
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

        //protected ProductCollection VirtualizedCart(ProductCollection cart)
        //{
        //    ProductCollection temp = new ProductCollection();
        //    foreach (Product item in cart)
        //    {
        //        if (item.BookStatus == "V")
        //        {

        //            foreach (Product kit in PubEnt.DAL2.DAL.GetVirtualKits(item.PubId))
        //            {

        //                kit.NumQtyOrdered = kit.NumQtyOrdered * item.NumQtyOrdered;
        //                temp.Add(kit);
        //            }
        //        }
        //        else
        //        {
        //            temp.Add(item);
        //        }
        //    }
        //    temp.ShipAcctNum = cart.ShipAcctNum;
        //    temp.ShipCost = cart.ShipCost;
        //    temp.ShipMethod = cart.ShipMethod;
        //    temp.ShipVendor = cart.ShipVendor;


        //    return (cart);
        //}
        protected void Button1_Click(object sender, EventArgs e) //Verify Order Button Click
        {
            //submitit
            #region Toggle Billing Validators (if billing is not visible, this part is irrelevant)
            if (chkSameaddress.Checked || drpDelivery.SelectedIndex < 3)    //***EAC any shipmethod other than FEDEX/UPS (dirty but it works 20130426)
            {
                RequiredFieldValidator9.Enabled = false;    //city
                RequiredFieldValidator10.Enabled = false;   //state
                RequiredFieldValidator11.Enabled = false;   //name
                RequiredFieldValidator12.Enabled = false;   //addr1
                RequiredFieldValidator14.Enabled = false;   //zip

                RegularExpressionValidator2.Enabled = false;    //email
                RegularExpressionValidator4.Enabled = false;    //phone

                custValidatorPhoneEmail.Enabled = false;       //"please collect billing phone or email"
                RegularExpressionValidator7.Enabled = false;    //check_billing_zipcode
                RequiredFieldValidator10.Enabled = false;        //re bill state
            }
            else
            {
                RequiredFieldValidator9.Enabled = true;
                RequiredFieldValidator10.Enabled = true;
                RequiredFieldValidator11.Enabled = true;
                RequiredFieldValidator12.Enabled = true;
                RequiredFieldValidator14.Enabled = true;

                RegularExpressionValidator2.Enabled = true;
                RegularExpressionValidator4.Enabled = true;

                custValidatorPhoneEmail.Enabled = true;
                RegularExpressionValidator7.Enabled = true;
                RequiredFieldValidator10.Enabled = true;
            }
            #endregion

            if (drpDelivery.SelectedIndex > 2)
            {
                RequiredFieldValidator18.Enabled = true;    //fedex
                RequiredFieldValidator20.Enabled = true;    //ups
                if (drpDelivery.SelectedValue[0] == 'F')
                    FedExExpressionValidator10.Enabled = true;
                else
                    FedExExpressionValidator10.Enabled = false;
                if (drpDelivery.SelectedValue[0] == 'U')
                    UPSExpressionValidator10.Enabled = true;
                else
                    UPSExpressionValidator10.Enabled = false;
            }
            else
            {
                RequiredFieldValidator18.Enabled = false;
                RequiredFieldValidator20.Enabled = false;
            }
            
            if (drpCountry.SelectedValue != "US")
            {
                RegularExpressionValidator6.Enabled = false;    //check_zip_code
                RequiredFieldValidator6.Enabled = false;        //state
            }
            else
            {
                RegularExpressionValidator6.Enabled = true;
                RequiredFieldValidator6.Enabled = true;
            }
                           
            Page.Validate();
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

                //***EAC At this point, data looks clean
                this.shipto = new Person(1, txtname.Text, txtorg.Text, txtemail.Text, txtaddr1.Text, txtaddr2.Text, (txtcity.Visible == true ? txtcity.Text : drpcity.SelectedValue), txtstate.Text, txtzip5.Text, txtzip4.Text, txtphone.Text, drpCountry.SelectedValue );
                shoppingcartV2.CustomerTypeId = Int32.Parse(this.drpCustomerType.SelectedValue);

                if (txtOrderComment.Text.Trim().Length > 0)
                    shoppingcartV2.OrderComment = txtOrderComment.Text.Trim();

                if (chkSameaddress.Checked)
                    this.billto = this.shipto;
                else
                    this.billto = new Person(1, txt2name.Text, txt2org.Text, txt2email.Text, txt2addr1.Text, txt2addr2.Text, (txt2city.Visible == true ? txt2city.Text : drpcity2.SelectedValue), txt2state.Text, txt2zip5.Text, txt2zip4.Text, txt2phone.Text, drp2Country.SelectedValue );

                if (false && shoppingcartV2.isFree2Order(shipto != null ? shipto.State : ""))//In NCIPLLM, we always have shipmenthod even for free items
                {
                    shoppingcartV2.ShipAcctNum = "";
                    shoppingcartV2.ShipMethod = "";
                    shoppingcartV2.ShipCost = 0.0;
                }
                else
                {
                    shoppingcartV2.ShipAcctNum = txtAccountNumber.Text;
                    shoppingcartV2.ShipMethod = drpDelivery.SelectedValue;
                    shoppingcartV2.ShipCost = EstimatedRate(shipto, shoppingcartV2);
                }

                shoppingcartV2.OrderMedia = drpOrderMedia.SelectedValue;

                string role = GlobalUtils.Utils.LoggedinRole();
                if (role != "NCIPL_CC" && role != "NCIPL_POS" && role != "NCIPL_LM")
                    shoppingcartV2.OrderCreator = "PUBLIC";
                else
                    shoppingcartV2.OrderCreator = GlobalUtils.Utils.LoggedinUser();
                Response.Redirect("verify.aspx", true);
            }
        }

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

                    BindTotals(txtstate.Text);
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
        }


        private void ResetSessions()
        {

            Session["NCIPL_cart"] = null;       //destroy
            Session["NCIPL_shipto"] = null;     //destroy
            Session["NCIPL_billto"] = null;     //destroy
            Session["NCIPL_cc"] = null;         //destroy
            Session["NCIPL_DONE"] = "";         //neither done nor finished
        }

        protected void txtorg_TextChanged(object sender, EventArgs e)
        {

        }

        protected void chkSameaddress_CC(object sender, EventArgs e)
        {
            if (chkSameaddress.Checked)
            {
                pnlBIllingInfo.Enabled = false;
                shoppingcartV2.SameShipBill = true;
            }
            else
            {
                pnlBIllingInfo.Enabled = true;
                shoppingcartV2.SameShipBill = false;
            }
        }

        protected void Click_BacktoCart(object sender, EventArgs e)
        {
            this.shipto = new Person(1, txtname.Text, txtorg.Text, txtemail.Text, txtaddr1.Text, txtaddr2.Text, (txtcity.Visible == true ? txtcity.Text : drpcity.SelectedValue), txtstate.Text, txtzip5.Text, txtzip4.Text, txtphone.Text, drpCountry.SelectedValue);
            Response.Redirect("cart.aspx", true);
        }
        
        private void BindShippingInfo()
        {
            if (shipto != null)
            {
                txtname.Text = shipto.Fullname;
                txtorg.Text = shipto.Organization;
                txtaddr1.Text = shipto.Addr1;
                txtaddr2.Text = shipto.Addr2;
                txtphone.Text = shipto.Phone;
                txtemail.Text = shipto.Email;

                txtzip5.Text = shipto.Zip5;
                txtzip4.Text = shipto.Zip4;
                txtcity.Text = shipto.City;
                txtstate.Text = shipto.State;
                txtcity.Visible = true;
                drpcity.Visible = false;
                drpCountry.SelectedValue = shipto.Country;
            }
        }
        private void BindBillingInfo()
        {
            if (billto != null)
            {
                //EAC use a try-catch because the billto object may be non-existent at this point
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
                    drp2Country.SelectedValue = billto.Country;
                }
                catch (Exception)
                {
                }
            }            
        }
        private void BindExtraInfo()
        {
            if (shoppingcartV2 != null)
            {
                if (shoppingcartV2.CustomerTypeId != -1)
                    drpCustomerType.SelectedValue = shoppingcartV2.CustomerTypeId.ToString();

                if (shoppingcartV2.OrderMedia != "")
                    drpOrderMedia.SelectedValue = shoppingcartV2.OrderMedia;

                if (shoppingcartV2.OrderComment.Length > 0)
                    txtOrderComment.Text = shoppingcartV2.OrderComment;

                if (shoppingcartV2.SameShipBill)
                    chkSameaddress.Checked = true;
                else
                    chkSameaddress.Checked = false;

                if (shoppingcartV2.ShipMethod.Length > 0)
                    drpDelivery.SelectedValue = shoppingcartV2.ShipMethod;
                if (shoppingcartV2.ShipAcctNum != null)
                    txtAccountNumber.Text = txtAccountNumber2.Text = shoppingcartV2.ShipAcctNum;
                if (shoppingcartV2.ShipCost > 0)//TODO: this should be isfree2order (20130429)
                {
                    lblShipping.Text = "Estimated shipping charge (" + shoppingcartV2.TotalWeight.ToString(".#") + " lbs)";
                    lblCost.Text = shoppingcartV2.ShipCost.ToString("c") + " USD";
                }

            }
        }
 
        protected void DeliveryChanged(object sender, EventArgs e)
        {
            shoppingcartV2.ShipMethod = drpDelivery.SelectedValue;
            BindTotals(txtstate.Text);
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            //recalculate
            //if (drpVendor.SelectedIndex > 0 && drpDelivery.SelectedIndex >0)
            if (drpDelivery.SelectedIndex > 0)
            {
                shipto = new Person(1, txtname.Text, txtorg.Text, txtemail.Text, txtaddr1.Text, txtaddr2.Text, (txtcity.Visible == true ? txtcity.Text : drpcity.SelectedValue), txtstate.Text, txtzip5.Text, txtzip4.Text, txtphone.Text, drpCountry.SelectedValue );
                //if (chkBxSplitOrder.Checked) //NCIPL_CC - DONTKNOW why JPJ has this (20120508)
                //    shipto.SplitOrder = "1";
                shoppingcartV2.ShipMethod = drpDelivery.SelectedValue;
                //this.shoppingcartV2.ShipVendor = drpVendor.SelectedValue;
                shoppingcartV2.ShipAcctNum = txtAccountNumber.Text;
                shoppingcartV2.ShipCost = EstimatedRate(shipto, shoppingcartV2);

                if (shoppingcartV2.ShipCost > 0)
                {
                    lblShipping.Text = "Estimated shipping charge (" + shoppingcartV2.TotalWeight.ToString(".#") + " lbs)";
                    lblCost.Text = shoppingcartV2.ShipCost.ToString("c") + " USD";
                }
                else
                {
                    if (shoppingcartV2.ShipMethod[0] == 'F')
                        lblShipping.Text = "<font color=red>The FedEx estimation service is unavailable at this time.</font>";
                    else if (shoppingcartV2.ShipMethod[0] == 'U')
                        lblShipping.Text = "<font color=red>The UPS estimation service is unavailable at this time.</font>";
                    else
                        lblShipping.Text = "<font color=red>Unable to estimate rates at this time.</font>";
                    lblCost.Text = "<font color=red>N/A</font>";
                }
            }
            else
            {
                lblCost.Text = "<font color=red>N/A</font>";
            }
        }

        string ShipMethod(string vendor, string method)//**EAC unused for now
        {
            if (vendor == "FEDEX")
                return (FedEx.ShipMethod(method));
            else
                return ("TODO:ups.shipmethod");
        }
        double EstimatedRate(Person shipto, ProductCollection cart)
        {
            try
            {
                if (cart.ShipMethod[0] == 'F')
                    return FedEx.FedExEstimatedRate(shipto, cart);

                else if (cart.ShipMethod[0] == 'U')
                    return UPS.UPSEstimatedRate(shipto, cart);
            }
            catch(Exception ex)
            {
                return 0.0;
            }
            return 0.0;
        }
        protected void ShowBillingItems()
        {
            Label1.Text = "Shipping & Payment";
            pnlPaymentInfo.Visible = true;
            RequiredFieldValidator19.Enabled = true;
            pnlBIllingInfo.Visible = true;
            chkSameaddress.Visible = true;
            lblBill.Visible = true;
            lblShip.Visible = true;
            lblCost.Text = shoppingcartV2.ShipCost.ToString("c");
        }
        protected void HideBillingItems()
        {
            Label1.Text = "Shipping";
            pnlPaymentInfo.Visible = true;  //NCIPLLM users want to see shipmethod all the time (20130426)
            RequiredFieldValidator19.Enabled = false;
            pnlBIllingInfo.Visible = false;
            chkSameaddress.Visible = false;
            lblBill.Visible = false;
            lblShip.Visible = false;
            lblCost.Text = "0.00";
        }
        //***EAC update count, shipcosts etc
        protected void BindTotals(string state)
        {
            if (shoppingcartV2.isFree2Order(state))
                HideBillingItems();
            else
                ShowBillingItems();

            // Enable-disable the submit button (XPO requirement)
            if (shoppingcartV2.isOrderAllowed(state))
            {
                Button1.Enabled = true;
                lblXPO.Text = "";
            }
            else
            {
                Button1.Enabled = false;
                lblXPO.Text = "Please reduce total items to " + PubEnt.GlobalUtils.Const.XPOMaxQuantity.ToString() + " or less";
            }

            //Display the master page tabs 
            GlobalUtils.Utils UtilMethod = new GlobalUtils.Utils();
            if (Session["NCIPL_Pubs"] != null)
                Master.LiteralText = UtilMethod.GetTabHtmlMarkUp(Session["NCIPL_Qtys"].ToString(), "");
            else
                Master.LiteralText = UtilMethod.GetTabHtmlMarkUp("", "");
            UtilMethod = null;
        }

        protected void ValBillPhoneorEmail(object source, ServerValidateEventArgs args)
        {
            bool boolStopSubmit = false;
            if (!shoppingcartV2.isFree2Order(shipto != null ? shipto.State : ""))
            {
                if (chkSameaddress.Checked == true)
                {
                    if (txtphone.Text.Trim().Length == 0 && txtemail.Text.Trim().Length == 0)
                        boolStopSubmit = true;
                }
                else
                {
                    if (txt2phone.Text.Trim().Length == 0 && txt2email.Text.Trim().Length == 0)
                        boolStopSubmit = true;
                }
            }
            if (boolStopSubmit)
                args.IsValid = false;
        }

    }
}

