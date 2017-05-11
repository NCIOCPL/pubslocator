using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Collections;
using System.Web.Services.Protocols;

using PubEnt.DAL;
using PubEnt.BLL;
using PubEnt.FedExRateService;

namespace PubEnt
{
    public partial class cart : System.Web.UI.Page
    {

        #region Delegate code for usercontrol
        protected override void OnInit(EventArgs e)
        {
            searchbar.SearchButtonClick += new EventHandler(SearchBar_btnSearchClick);
            base.OnInit(e);
        }
        private void SearchBar_btnSearchClick(object sender, EventArgs e)
        {

            Session["PUBENT_SearchKeyword"] = searchbar.Terms;
            Session["PUBENT_TypeOfCancer"] = "";
            Session["PUBENT_Subject"] = "";
            Session["PUBENT_Audience"] = "";
            Session["PUBENT_Language"] = "";
            Session["PUBENT_ProductFormat"] = "";
            Session["PUBENT_StartsWith"] = "";
            Session["PUBENT_Series"] = "";
            Session["PUBENT_NewOrUpdated"] = "";
            Session["PUBENT_Race"] = "";

            Session["PUBENT_Criteria"] = Session["PUBENT_SearchKeyword"]; //For displaying criteria

            GlobalUtils.Utils objUtils = new GlobalUtils.Utils();
            string QueryParams = objUtils.GetQueryStringParams();
            objUtils = null;
            Response.Redirect("searchres.aspx" + "?sid=" + QueryParams);
        }
        #endregion
        //ArrayList arrlstNerdoPubIds;
        KVPairCollection kvpaircoll;

        Person shipto
        {
            get
            {
                if (Session["NCIPL_shipto"] == null)
                {
                    if (GlobalUtils.Utils.isLoggedin())
                        return shipto = DAL2.DAL.GetShippingInfo(GlobalUtils.Utils.LoggedinUser());
                    else
                        return null;
                }
                else
                    return (Person)Session["NCIPL_shipto"];
            }
            set { Session["NCIPL_shipto"] = value; }
        }
        ProductCollection shoppingcart
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


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                #region NERDO_Related
                //arrlstNerdoPubIds = DAL.DAL.GetNerdoPubIds();
                kvpaircoll = DAL.DAL.GetKVPair("sp_NCIPL_getNerdoPubIdsURLS");


                #endregion

                #region dummydata--remove before going live
                //Session["NCIPL_Pubs"] = "1043,117";
                //Session["NCIPL_Qtys"] = "20,20";
                //Session["NCIPL_User"] = "ncipl@test.com";
                //Session["NCIPL_Role"] = "NCIPL_PUBLIC";
                #endregion

                steps1.Activate("cell1");

                this.shoppingcart = null;     //destroy cart if it exists
                this.shoppingcart = new ProductCollection();

                //***EAC Give FEDEX,FEDEX_GROUND estimates by default
                //shoppingcart.ShipVendor = "FEDEX";
                shoppingcart.ShipMethod = "";

                //***EAC Parse the NCIPL_Pubs and NCIPL_qtys..assume they have same dimensions
                string[] pubs = Session["NCIPL_Pubs"].ToString().Split(new Char[] { ',' });
                string[] qtys = Session["NCIPL_Qtys"].ToString().Split(new Char[] { ',' });
                for (var i = 0; i < pubs.Length; i++)
                {
                    if (pubs[i].Trim() != "")
                    {
                        int pubid = Int32.Parse(pubs[i]);
                        Product p = Product.GetCartPubByPubID(pubid);
                        p.NumQtyOrdered = Int32.Parse(qtys[i]);
                        this.shoppingcart.Add(p);   //BLL will know what to do w/ dupes and 0 quantities
                    }
                }

                grdViewItems.DataSource = this.shoppingcart;
                grdViewItems.DataBind();

                BindTotals();
            }
            //Check for cart object here
            if (this.shoppingcart == null)
                Response.Redirect("default.aspx?redirect=cart1", true);
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {


            base.Render(writer);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (CheckForValidQuantity() < 0)
            {
                return;
            }

            //***EAC Bulk orders are forced to log in
            if (shoppingcart.isFree2Order(shipto != null ? shipto.State : "") || GlobalUtils.Utils.isLoggedin())
                Response.Redirect("~/shipping.aspx", true);
            else
                Response.Redirect("~/login.aspx?redir=shipping.aspx", true);
        }

        protected void UpdateQty(object sender, EventArgs e)
        {
            if (CheckForValidQuantity() < 0)
            {
                return;
            }

            //yma add this checking
            int intTotalQty = 0;
            foreach (GridViewRow item in grdViewItems.Rows)
            {
                TextBox txtQty = (TextBox)item.FindControl("txtQty");
                intTotalQty += Int32.Parse(txtQty.Text);                
            }
            if (intTotalQty > 20)
            {
                Label MessageLabel = new Label();
                MessageLabel.ID = "msglabelid";
                MessageLabel.Text = "The total order quantity cannot exceed 20 items."; 
                MessageLabel.ForeColor = System.Drawing.Color.Red;
                Panel1.Controls.Add(MessageLabel);
                return;
            }

            foreach (GridViewRow item in grdViewItems.Rows)
            {
                TextBox txtQty = (TextBox)item.FindControl("txtQty");
                HiddenField hdnPubID = (HiddenField)item.FindControl("hdnPubID");
                this.shoppingcart.UpdateQty(Int32.Parse(hdnPubID.Value), Int32.Parse(txtQty.Text));
                Session["NCIPL_Pubs"] = this.shoppingcart.Pubs;
                Session["NCIPL_Qtys"] = this.shoppingcart.Qtys;
            }
            grdViewItems.DataSource = this.shoppingcart;
            grdViewItems.DataBind();
            BindTotals();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Response.Redirect("searchres.aspx");
        }

        //checks for a valid quantity
        private int CheckForValidQuantity()
        {
            int result = 1;
            //foreach (DataGridItem dItem in grdItems.Items)
            foreach (GridViewRow dItem in grdViewItems.Rows)
            {
                TextBox txtQty = (TextBox)dItem.FindControl("txtQty");
                Label lblLimit = (Label)dItem.FindControl("lblQty");
                PlaceHolder MessagePlaceHolder = (PlaceHolder)dItem.FindControl("MessagePlaceHolder");

                GlobalUtils.Utils UtilityMethods = new GlobalUtils.Utils();
                bool returnvalue = UtilityMethods.IsQtyValueValid(txtQty.Text, lblLimit.Text);
                if (returnvalue == false)
                {
                    GlobalUtils.Utils.DisplayMessage(ref MessagePlaceHolder, 2);
                    result = -1;
                    break;
                }
                else
                    GlobalUtils.Utils.DisplayMessage(ref MessagePlaceHolder, 99);
            }
            return result;
        }

        protected void grdViewItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Add a JavaScript confirmation message to Remove button
                Button Button2 = (Button)e.Row.FindControl("Button2");
                Button2.Attributes.Add("onclick", "return fnAskBeforeRemoval(" + "'" + Button2.ClientID + "')");

                Product p = (Product)e.Row.DataItem;
                HyperLink lnkItem = (HyperLink)e.Row.FindControl("lnkItem");
                Label lblItem = (Label)e.Row.FindControl("lblItem");
                Label lblQty = (Label)e.Row.FindControl("lblQty");
                Label lblQtyText = (Label)e.Row.FindControl("lblQtyText");
                TextBox txtQty = (TextBox)e.Row.FindControl("txtQty");
                HiddenField hdnPubID = (HiddenField)e.Row.FindControl("hdnPubID");

                Panel pnlNerdo = (Panel)e.Row.FindControl("pnlNerdo");
                HyperLink NerdoContentlink = (HyperLink)e.Row.FindControl("NerdoContentlink");
                Label lblCoverOnly = (Label)e.Row.FindControl("lblCoverOnly");

                lnkItem.Text = p.LongTitle;
                lnkItem.NavigateUrl = "detail.aspx?prodid=" + p.ProductId;

                string Explanation = ""; //Right now only for NERDO pubs

                if (kvpaircoll == null)
                    kvpaircoll = DAL.DAL.GetKVPair("sp_NCIPL_getNerdoPubIdsURLS");

                foreach (KVPair kvpair in kvpaircoll)
                {
                    if (string.Compare(kvpair.Key, p.PubId.ToString()) == 0)
                    {
                        Explanation = ": Pack of 25 covers";
                        pnlNerdo.Style["display"] = "";
                        NerdoContentlink.NavigateUrl = kvpair.Val;
                        NerdoContentlink.Text = "Print separate contents";
                        lblCoverOnly.Visible = true;
                        lnkItem.NavigateUrl = p.Url;
                        lnkItem.Target = "_blank";
                        break;
                    }
                }
                if (lblCoverOnly.Visible == false)
                    pnlNerdo.Visible = false;

                lblItem.Text = p.ProductId + Explanation;
                lblQtyText.Text = "Limit ";
                lblQty.Text = p.NumQtyLimit.ToString();
                txtQty.Text = p.NumQtyOrdered.ToString();
                hdnPubID.Value = p.PubId.ToString();
                Button2.CommandArgument = e.Row.RowIndex.ToString();
            }
        }

        protected void grdViewItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Remove")
            {
                this.shoppingcart.Remove(this.shoppingcart[Int32.Parse(e.CommandArgument.ToString())]);
                Session["NCIPL_Pubs"] = this.shoppingcart.Pubs;
                Session["NCIPL_Qtys"] = this.shoppingcart.Qtys;

                grdViewItems.DataSource = this.shoppingcart;
                grdViewItems.DataBind();

                if (this.shoppingcart.Count > 0)
                {
                    //Do not do anything
                }
                else
                {
                    Panel1.Visible = false;
                    Panel2.Visible = true;
                }
            }
            BindTotals();
        }

        protected void grdViewItems_PreRender(object sender, EventArgs e)
        {
            //***EAC I dont know what this is for 2/14/2012
            //grdViewItems.HeaderRow.TableSection = TableRowSection.TableHeader;
        }


        //***EAC update count, shipcosts etc
        protected void BindTotals()
        {
            if (Zipcode.isXPO(shipto != null ? shipto.State : ""))
            {
                divCCExplanation.Visible = false;
                lblTot.Text = this.shoppingcart.TotalQty.ToString();
                divOrderingHelp.InnerText = "Please note: We will provide free shipping via U.S. Postal Service for orders up to " + PubEnt.GlobalUtils.Const.XPOMaxQuantity.ToString() + " items to your location. We are sorry we cannot send orders of more than " + PubEnt.GlobalUtils.Const.XPOMaxQuantity.ToString() + " items or send items via FedEx or UPS to your shipping address.";
            }
            else
            {
                if (shoppingcart.isFree2Order(shipto != null ? shipto.State : ""))
                {
                    divCCExplanation.Visible = false;
                    lblTot.Text = this.shoppingcart.TotalQty.ToString();
                    divOrderingHelp.InnerText = "";
                }
                else
                {
                    divCCExplanation.Visible = true;
                    lblTot.Text = this.shoppingcart.TotalQty.ToString() + " (" + this.shoppingcart.TotalWeight.ToString(".#") + " lbs)<sup>*</sup>";
                    divOrderingHelp.InnerHtml = "Ordering more than 20 items? You will be asked to log in or register, and provide a FedEx or UPS shipping number to pay actual shipping costs. <a href='nciplhelp.aspx#register'>Learn more about registering for an account</a>";
                }
            }

            //Hide or unhide the cart
            if (this.shoppingcart.Count > 0)
            {
                //***EAC at this point we have a usable cart
                Panel1.Visible = true;
                Panel2.Visible = false;
            }
            else//shopping cart is empty
            {
                Panel1.Visible = false;
                Panel2.Visible = true;
            }

            // Enable-disable the submit button (XPO requirement)
            if (shoppingcart.isOrderAllowed(shipto != null ? shipto.State : ""))
            {
                btn2shipping.Enabled = true;
                divOrderingHelp.Attributes["style"] = "text-align: left; padding-bottom: 10px;";
            }
            else
            {
                btn2shipping.Enabled = false;
                divOrderingHelp.Attributes["style"] = "text-align: left; padding-bottom: 10px; color: #FF0000";
            }
            //Display the master page tabs 
            GlobalUtils.Utils UtilMethod = new GlobalUtils.Utils();
            if (Session["NCIPL_Pubs"] != null)
                Master.LiteralText = UtilMethod.GetTabHtmlMarkUp(Session["NCIPL_Qtys"].ToString(), "cart");
            else
                Master.LiteralText = UtilMethod.GetTabHtmlMarkUp("", "cart");
            UtilMethod = null;
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            //test fedex rates
        }
    }
}
