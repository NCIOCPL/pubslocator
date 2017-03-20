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
    public partial class kioskshipping : System.Web.UI.Page
    {
        public string shipLocation = "";
        public string strConfId = "";
        public string boolEmail = "false";
        
        protected void Page_Load(object sender, EventArgs e)
        {
            //***EAC the session variable is not used anymore except this page.  I added next line.
            //Session["KIOSK_Conference"] = Request.QueryString["ConfID"];

            //Some checks - We need a good confid
            if (Request.QueryString["ConfID"] == null)
            {
                Utils.ResetSessions();
                Response.Redirect("default.aspx?redirect=kioskshipping", true);
            }
            else
                strConfId = Request.QueryString["ConfID"].ToString();

            if (strConfId.Length > 4 || strConfId == "") //potentially an intrusion
            {
                Utils.ResetSessions();
                Response.Redirect("default.aspx?redirect=kioskshipping", true);
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
            if (Session["KIOSK_Urls"].ToString().Length > 0)
            {
                labelEmail.Text  = "E-mail*";
                boolEmail = "true";
            }
            else
            {
                labelEmail.Text = "E-mail";
                boolEmail = "false";
            }

            if (!Page.IsPostBack)
            {
                setShippingAddress();

                BindContactInfo();
            }
            else
            {
                ScanInputData();
            }
        }

        protected void ScanInputData()
        {
            string strErrorPage = "default.aspx?redirect=kioskshipping";

            if (!InputValidation.LenVal(this.txtContRef.Text, 30))
            {
                Response.Redirect(strErrorPage, true);
            }

            if (!InputValidation.LenVal(this.txtOrg.Text, 30))
            {
                Response.Redirect(strErrorPage, true);
            }

            if (InputValidation.SpecialVal(this.txtOrg.Text))
            {
                Response.Redirect(strErrorPage, true);
            }

            if (!InputValidation.LenVal(this.txtAddr1.Text, 30))
            {
                Response.Redirect(strErrorPage, true);
            }

            if (!InputValidation.LenVal(this.txtAddr2.Text, 30))
            {
                Response.Redirect(strErrorPage, true);
            }

            if (!InputValidation.LenVal(this.txtZip.Text, 5))
            {
                Response.Redirect(strErrorPage, true);
            }

            if (!InputValidation.LenVal(this.txtZipPlus4.Value, 4))
            {
                Response.Redirect(strErrorPage, true);
            }

            if (!InputValidation.LenVal(this.txtPostal.Text, 10))
            {
                Response.Redirect(strErrorPage, true);
            }

            if (this.selCity.SelectedValue != "")
            {
                if (!InputValidation.ContentVal(this.selCity.SelectedValue, @"^[0-9a-zA-Z-@.'_#,()/]"))
                {
                    Response.Redirect(strErrorPage, true);
                }
            }

            if (!InputValidation.LenVal(this.txtCity.Value, 20))
            {
                Response.Redirect(strErrorPage, true);
            }

            if (Session["KIOSK_ShipLocation"] != null)
            {
                if (Session["KIOSK_ShipLocation"].ToString() == "Domestic")
                {
                    if (!InputValidation.LenVal(this.txtState.Text, 2))
                    {
                        Response.Redirect(strErrorPage, true);

                    }
                }
                else
                {
                    if (!InputValidation.LenVal(this.txtState.Text, 30))
                    {
                        Response.Redirect(strErrorPage, true);
                    }
                }
            }


            if (!InputValidation.LenVal(this.txtEMail.Value, 40))
            {
                Response.Redirect(strErrorPage, true);
            }

            if (!InputValidation.LenVal(this.txtPhone.Value, 30))
            {
                Response.Redirect(strErrorPage, true);
            }
            if (!InputValidation.LenVal(this.txtPhone2.Value, 30))
            {
                Response.Redirect(strErrorPage, true);
            }
        }
            
        protected void setShippingAddress()
        {
            //this.hidRem.Value = "";

            //this.trAddressError.Visible = false;
            //this.lblAddressError.InnerHtml = "";

            if (Session["KIOSK_ShipLocation"].ToString() == "Domestic")
            {
                //this.btnOrder.OnClientClick = String.Format("return VerifyDomesticOrder('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')", this.txtContRef.ClientID, this.txtAddr1.ClientID, this.txtZip.ClientID, this.txtState.ClientID, this.hidRem.ClientID, this.hidMaxLimit.ClientID);
                //this.btnOrder2.OnClientClick = String.Format("return VerifyDomesticOrder('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')", this.txtContRef.ClientID, this.txtAddr1.ClientID, this.txtZip.ClientID, this.txtState.ClientID, this.hidRem.ClientID, this.hidMaxLimit.ClientID);

                //this.hidMaxLimit.Value = NCIPLConfManager.DomesticMaxLimit.ToString();
                
                this.trZIP.Visible = true;
                //this.trDomesticZipNote.Visible = true;
                this.trPostal.Visible = false;
                this.txtZip.Text = "";

                this.lblState.Text = "State*";
                this.txtState.Enabled = false;
                this.txtState.Text = "";
                this.txtState.MaxLength = 2;
                this.txtState.CssClass = "inputtextstate";
                this.countryRow.Visible = false;

                this.txtPhone.Visible = true;
                this.txtPhone2.Visible = false;

                this.selCity.Items.Clear();
                if (this.selCity.Items.Count > 0)
                {
                    this.selCity.Enabled = true;
                }
                else
                {
                    this.selCity.Enabled = false;
                }
                this.trCityDomestic.Visible = true;
                this.trCityIntl.Visible = false;
            }
            else
            {
                //this.btnOrder.OnClientClick = String.Format("return VerifyIntlOrder('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}')", this.txtContRef.ClientID, this.txtAddr1.ClientID, this.txtPostal.ClientID, this.txtCity.ClientID, this.txtCountry.ClientID, this.hidRem.ClientID, this.hidMaxLimit.ClientID);
                //this.btnOrder2.OnClientClick = String.Format("return VerifyIntlOrder('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}')", this.txtContRef.ClientID, this.txtAddr1.ClientID, this.txtPostal.ClientID, this.txtCity.ClientID, this.txtCountry.ClientID, this.hidRem.ClientID, this.hidMaxLimit.ClientID);
                
                //this.hidMaxLimit.Value = NCIPLConfManager.ForeignMaxLimit.ToString();

                this.trZIP.Visible = false;
                //this.trDomesticZipNote.Visible = false;
                this.trPostal.Visible = true;
                this.txtPostal.Text = "";

                this.lblState.Text = "Province";
                this.txtState.Visible = true;
                this.txtState.Text = "";
                this.txtState.MaxLength = 30;
                this.txtState.CssClass = "inputtextlong";
                this.txtState.Enabled = true;

                this.trCityDomestic.Visible = false;
                this.trCityIntl.Visible = true;
                this.txtCity.Value = "";

                this.countryRow.Visible = true;
                this.txtCountry.Text = "";

                this.txtPhone.Visible = false;
                this.txtPhone2.Visible = true;
            }
           
        }

        protected void BindContactInfo()
        {
            if (Session["KIOSK_Name"] != null)
            {
                if (Session["KIOSK_Name"].ToString().Length != 0)
                {
                    this.txtContRef.Text = Server.HtmlDecode(Session["KIOSK_Name"].ToString());
                }
            }

            if (Session["KIOSK_Organization"] != null)
            {
                if (Session["KIOSK_Organization"].ToString().Length != 0)
                {
                    this.txtOrg.Text = Server.HtmlDecode(Session["KIOSK_Organization"].ToString());
                }
            }

            if (Session["KIOSK_Address1"] != null)
            {
                if (Session["KIOSK_Address1"].ToString().Length != 0)
                {
                    this.txtAddr1.Text = Server.HtmlDecode(Session["KIOSK_Address1"].ToString());
                }
            }

            if (Session["KIOSK_Address2"] != null)
            {
                if (Session["KIOSK_Address2"].ToString().Length != 0)
                {
                    this.txtAddr2.Text = Server.HtmlDecode(Session["KIOSK_Address2"].ToString());
                }
            }

            if (Session["KIOSK_ZIPCode"] != null)
            {
                if (Session["KIOSK_ZIPCode"].ToString().Length != 0)
                {
                    //handle city and state control as well
                    if (Session["KIOSK_ShipLocation"] != null)
                    {
                        switch (Session["KIOSK_ShipLocation"].ToString())
                        {
                            case "Domestic":
                                this.txtZip.Text = Server.HtmlDecode(Session["KIOSK_ZIPCode"].ToString());
                                if (Session["KIOSK_ZIPPlus4"] != null)
                                {
                                    if (Session["KIOSK_ZIPPlus4"].ToString().Length != 0) {
                                        this.txtZipPlus4.Value = Server.HtmlDecode(Session["KIOSK_ZIPPlus4"].ToString());
                                    }
                                }
                                GetCity(Session["KIOSK_ZIPCode"].ToString().Trim());
                                if (Session["KIOSK_City"] != null)
                                {
                                    if (Session["KIOSK_City"].ToString().Length != 0)
                                    {
                                        this.selCity.SelectedIndex = this.selCity.Items.IndexOf(this.selCity.Items.FindByText(Session["KIOSK_City"].ToString()));
                                    }
                                }
                                break;

                            case "International":
                                this.txtPostal.Text = Server.HtmlDecode(Session["KIOSK_ZIPCode"].ToString());
                                if (Session["KIOSK_City"] != null)
                                {
                                    if (Session["KIOSK_City"].ToString().Length != 0)
                                    {
                                        this.txtCity.Value = Server.HtmlDecode(Session["KIOSK_City"].ToString());
                                    }
                                }

                                if (Session["KIOSK_State"] != null)
                                {
                                    if (Session["KIOSK_State"].ToString().Length != 0)
                                    {
                                        this.txtState.Text = Server.HtmlDecode(Session["KIOSK_State"].ToString());
                                    }
                                }

                                if (Session["KIOSK_Country"] != null)
                                {
                                    if (Session["KIOSK_Country"].ToString().Length != 0)
                                    {
                                        this.txtCountry.Text = Server.HtmlDecode(Session["KIOSK_Country"].ToString());
                                    }
                                }
                                break;
                        }
                    }
                }
            }

            if (Session["KIOSK_Phone"] != null)
            {
                if (Session["KIOSK_Phone"].ToString().Length != 0)
                {
                    if (Session["KIOSK_ShipLocation"] != null)
                    {
                        switch (Session["KIOSK_ShipLocation"].ToString())
                        {
                            case "Domestic":
                                this.txtPhone.Value = Server.HtmlDecode(Session["KIOSK_Phone"].ToString());
                                break;
                            case "International":
                                this.txtPhone2.Value = Server.HtmlDecode(Session["KIOSK_Phone"].ToString());
                                break;
                        }
                    }
                }
            }

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
            if (this.txtZip.Text.Length == 5)
            {
                if (!GetCity(this.txtZip.Text))
                {
                    //if (this.lblPubError.Visible)
                    //{
                        //this.lblPubError.Visible = false;
                    //}
                };
            }
            else
            {
                this.selCity.Items.Clear();
                this.selCity.Enabled = false;
                this.txtState.Text = "";
            }
        }

        protected Boolean GetCity(string strZIP)
        {
            string tmpCity = "";
            if (Session["KIOSK_ShipLocation"].ToString() == "Domestic")
            {
                if (this.selCity.Items.Count != 0)
                {
                    tmpCity = this.selCity.SelectedValue;
                }
                this.selCity.Items.Clear();
                this.selCity.Enabled = false;
                this.txtState.Text = "";
                try
                {
                    foreach (PubEnt.BLL.KVPair city in PubEnt.BLL.Zipcode.GetCities(Convert.ToInt32(strZIP)))
                    {
                        this.selCity.Items.Add(new ListItem(city.Key, city.Key));
                        this.selCity.Enabled = true;
                        this.txtState.Text = city.Val;
                    }

                    if (this.selCity.Items.Count == 0)
                    {
                        cusvaltxtZIP.IsValid = false;
                        this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "onload", "window.onload = function() { document.getElementById('divErrMsg').style.display = '';}", true);
                        return false;
                    }
                    else
                    {
                        this.selCity.SelectedIndex = this.selCity.Items.IndexOf(this.selCity.Items.FindByText(tmpCity));
                        cusvaltxtZIP.IsValid = true;
                        return true;
                    }
                }
                catch
                {
                    cusvaltxtZIP.IsValid = false;
                    this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "onload", "window.onload = function() { document.getElementById('divErrMsg').style.display = '';}", true);
                    return false;
                }
            }
            else
            {
                return true;
            }
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
            string strPageName = "verify_shipping.aspx";
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
            if (Session["KIOSK_Name"] != null)
            {
                Session["KIOSK_Name"] = Server.HtmlEncode(this.txtContRef.Text.Trim());
            }

            if (Session["KIOSK_Organization"] != null)
            {
                Session["KIOSK_Organization"] = Server.HtmlEncode(this.txtOrg.Text.Trim());
            }

            if (Session["KIOSK_Address1"] != null)
            {
                Session["KIOSK_Address1"] = Server.HtmlEncode(this.txtAddr1.Text.Trim());
            }

            if (Session["KIOSK_Address2"] != null)
            {
                Session["KIOSK_Address2"] = Server.HtmlEncode(this.txtAddr2.Text.Trim());
            }

            if (Session["KIOSK_ZIPCode"] != null)
            {
                if (Session["KIOSK_ShipLocation"] != null)
                {
                    if (Session["KIOSK_ShipLocation"].ToString() == "Domestic")
                    {
                        Session["KIOSK_ZIPCode"] = Server.HtmlEncode(this.txtZip.Text.Trim());

                        if (Session["KIOSK_ZIPPlus4"] != null)
                        {
                            Session["KIOSK_ZIPPlus4"] = Server.HtmlEncode(this.txtZipPlus4.Value.Trim());
                        }
                    }
                    else
                    {
                        Session["KIOSK_ZIPCode"] = Server.HtmlEncode(this.txtPostal.Text.Trim());
                    }
                }
            }

            if (Session["KIOSK_City"] != null)
            {
                if (Session["KIOSK_ShipLocation"] != null)
                {
                    if (Session["KIOSK_ShipLocation"].ToString() == "Domestic")
                    {
                        Session["KIOSK_City"] = Server.HtmlEncode(this.selCity.SelectedValue.Trim());
                    }
                    else
                    {
                        Session["KIOSK_City"] = Server.HtmlEncode(this.txtCity.Value.Trim());
                    }
                }
            }

            if (Session["KIOSK_State"] != null)
            {
                Session["KIOSK_State"] = Server.HtmlEncode(this.txtState.Text.Trim());
            }

            if (Session["KIOSK_Country"] != null)
            {
                Session["KIOSK_Country"] = Server.HtmlEncode(this.txtCountry.Text.Trim());
            }

            if (Session["KIOSK_Email"] != null)
            {
                Session["KIOSK_Email"] = Server.HtmlEncode(this.txtEMail.Value.Trim());
            }

            if (Session["KIOSK_Phone"] != null)
            {
                if (Session["KIOSK_ShipLocation"] != null)
                {
                    if (Session["KIOSK_ShipLocation"].ToString() == "Domestic")
                    {
                        Session["KIOSK_Phone"] = Server.HtmlEncode(this.txtPhone.Value.Trim());
                    }
                    else
                    {
                        Session["KIOSK_Phone"] = Server.HtmlEncode(this.txtPhone2.Value.Trim());
                    }
                }
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
