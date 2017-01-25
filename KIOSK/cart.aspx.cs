using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using PubEnt.DAL;
using PubEnt.BLL;
using PubEnt.GlobalUtils;
using System.Collections;
using System.Configuration;

namespace PubEnt
{
    public partial class cart : System.Web.UI.Page
    {
        string strConfId = "";
        ProductCollection shoppingcart
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

        protected void Page_Load(object sender, EventArgs e)
        {

            //Some checks - We need a good confid
            if (Request.QueryString["confid"] == null)
            {
                Utils.ResetSessions();
                Response.Redirect("default.aspx?redirect=cart", true);
            }
            else
                strConfId = Request.QueryString["confid"].ToString();

            if (strConfId.Length > 4 || strConfId == "") //potentially an intrusion
            {
                Utils.ResetSessions();
                Response.Redirect("default.aspx?redirect=cart", true);
            }
            //End of checks


            ImgBtnContinueSearch.Attributes.Add("onmousedown", "this.src='images/continuered_on.jpg'");
            ImgBtnContinueSearch.Attributes.Add("onmouseup", "this.src='images/continuered_off.jpg'");
            ImgBtnCancelOrder.Attributes.Add("onmousedown", "this.src='images/cancel_on.jpg'");
            ImgBtnCancelOrder.Attributes.Add("onmouseup", "this.src='images/cancel_off.jpg'");
            ImgBtnCheckOut.Attributes.Add("onmousedown", "this.src='images/checkout_on.jpg'");
            ImgBtnCheckOut.Attributes.Add("onmouseup", "this.src='images/checkout_off.jpg'");

            //***EAC Hide buttons or not...Better to do on PageLoad for each page than in Master.cs!
            Master.FindControl("btnViewCart").Visible = false;
            //Master.FindControl("btnSearchOther").Visible = false;
            Master.FindControl("btnFinish").Visible = false;
            Master.FindControl("lblFreePubsInfo").Visible = false;

            //Check whether the conference is domestic or international
            if (Session["KIOSK_ShipLocation"] != null)
            {
                if (Session["KIOSK_ShipLocation"].ToString().Length > 0)
                {
                    if (string.Compare(Session["KIOSK_ShipLocation"].ToString(), "Domestic", true) == 0)
                        hdnTotalLimit.Value = ConfigurationManager.AppSettings["DomesticOrderLimit"];
                    else if (string.Compare(Session["KIOSK_ShipLocation"].ToString(), "International", true) == 0)
                        //hdnTotalLimit.Value = ConfigurationManager.AppSettings["InternationalOrderLimit"]; 
                        hdnTotalLimit.Value = PubEnt.DAL.DAL.GetIntl_MaxOrder(int.Parse(strConfId)).ToString(); 
                }
            }

            if (!IsPostBack)
            {

                #region dummydata2--remove before going live
                if (Request.QueryString["debug"] == "yes")
                {
                    Session["KIOSK_Pubs"] = "4236,1095,1169";
                    Session["KIOSK_Qtys"] = "1,14,1";
                    Session["KIOSK_Urls"] = "34,47,110,116,117,118,119,120,121,122,123,124,127,128,129,130,131";
                    Session["KIOSK_ShipLocation"] = "Domestic";
                }
                #endregion

                
                this.shoppingcart = null;     //destroy cart if it exists
                this.shoppingcart = new ProductCollection();

                //***EAC Parse the KIOSK_Pubs and KIOSK_qtys..assume they have same dimensions
                string[] pubs = Session["KIOSK_Pubs"].ToString().Split(new Char[] { ',' });
                string[] qtys = Session["KIOSK_Qtys"].ToString().Split(new Char[] { ',' });
                for (var i=0; i<pubs.Length; i++)
                {
                    if (pubs[i].Trim() != "")
                    {
                        int pubid = Int32.Parse(pubs[i]);
                        Product p = Product.GetPubByPubID(pubid);
                        p.NumQtyOrdered = Int32.Parse(qtys[i]);
                        p.IsPhysicalItem = true;
                        shoppingcart.Add(p);   //TODO: BLL Add should differentiate physical from virtual items while deduping
                    }
                }


                #region EAC Add ONLINE-only items to the shopping cart (20130226)
                //***EAC We are assuming Session_pubs and Session_Urls are mutually exclusive
                string[] urls = Session["KIOSK_Urls"].ToString().Split(new Char[] { ',' });
                for (var i = 0; i < urls.Length; i++)
                {
                    if (urls[i].Trim() != "")
                    {
                        int pubid = Int32.Parse(urls[i]);
                        Product p = Product.GetPubByPubID(pubid);
                        p.NumQtyOrdered = 1;
                        p.IsPhysicalItem = false;
                        shoppingcart.Add(p);   //TODO: BLL Add should differentiate physical from virtual items while deduping
                    }
                }
                #endregion


                //--- Sorting the publication collection by long title
                this.shoppingcart.Sort(ProductCollection.ProductFields.LongTitle, true);
               
                grdViewItems.DataSource = this.shoppingcart;
                grdViewItems.DataBind();

                //Add client code
                //ImgBtnCancelOrder.Attributes.Add("onclick", "return fnAskBeforeCancelling(" + "'" + ImgBtnCancelOrder.ClientID + "')");
            }

            //Check for cart object here
            if (this.shoppingcart == null)
            {
                //Display empty shopping cart
                Panel1.Visible = false;
                Panel2.Visible = true;
                ImgBtnCheckOut.Visible = false;
            }
            else
            {
                if (this.shoppingcart.Count > 0 && hdnTotalLimit.Value.Length > 0)
                {
                    //***EAC at this point we have a usable cart
                    lblTot.Text = this.shoppingcart.TotalQtyForShipping.ToString();
                    lblFreeRemaining.Text = (Int32.Parse(hdnTotalLimit.Value) - this.shoppingcart.TotalQtyForShipping).ToString();

                    Panel1.Visible = true;
                    Panel2.Visible = false;

                    if (Session["KIOSK_ShipLocation"].ToString() != "Domestic")
                    {
                        h3free.Visible = false;
                        trFreeRemain.Visible = false;
                        spanQty.Visible = false;
                        trTotalItems.Visible = false;
                    } 
                }
                else//shopping cart is empty
                {
                    Panel1.Visible = false;
                    Panel2.Visible = true;
                    ImgBtnCheckOut.Visible = false;
                }
            }
        }

        protected void UpdateQty()
        {
            this.shoppingcart = null;     //destroy and re-create from GridView
            this.shoppingcart = new ProductCollection();

            foreach (GridViewRow item in grdViewItems.Rows)
            {
                HiddenField hdnQty = (HiddenField)item.FindControl("hdnQty");
                HiddenField hdnPubID = (HiddenField)item.FindControl("hdnPubID");
                Label lblEmailOnly = (Label)item.FindControl("lblEmailOnly");
                if (Int32.Parse(hdnQty.Value) > 0)
                {
                    Product p = Product.GetPubByPubID(Int32.Parse(hdnPubID.Value));
                    p.NumQtyOrdered = Int32.Parse(hdnQty.Value);
                    if (lblEmailOnly.Visible)  //***EAC dirty hack to tell if the line item is physical/virtual
                        p.IsPhysicalItem = false;
                    else
                        p.IsPhysicalItem = true;
                    this.shoppingcart.Add(p);

                }               
            }

            Session["KIOSK_Pubs"] = this.shoppingcart.PubsForShipping;
            Session["KIOSK_Qtys"] = this.shoppingcart.QtysForShipping;
            Session["KIOSK_Urls"] = this.shoppingcart.PubsForEmailing;

            grdViewItems.DataSource = this.shoppingcart;
            grdViewItems.DataBind();
        }


        //Call before Grid Remove Button UpdatedQty
        //This function will retain 0 quantity item
        protected void UpdateQtyInGrid()
        {   
            foreach (GridViewRow item in grdViewItems.Rows)
            {
                HiddenField hdnQty = (HiddenField)item.FindControl("hdnQty");
                HiddenField hdnPubID = (HiddenField)item.FindControl("hdnPubID");
                this.shoppingcart.UpdateQty(Int32.Parse(hdnPubID.Value), Int32.Parse(hdnQty.Value));
            }
            Session["KIOSK_Pubs"] = this.shoppingcart.PubsForShipping;
            Session["KIOSK_Qtys"] = this.shoppingcart.QtysForShipping;
            Session["KIOSK_Urls"] = this.shoppingcart.PubsForEmailing;

            grdViewItems.DataSource = this.shoppingcart;
            grdViewItems.DataBind();
        }



        //checks for a valid quantity
        private int CheckForValidQuantity()
        {
            int result = 1;
            int Counter = 0;
            bool returnvalue = true;
            
            //Loop through the grid rows
            foreach (GridViewRow dItem in grdViewItems.Rows)
            {   
                Label lblQty = (Label)dItem.FindControl("lblQty");
                HiddenField hdnQty = (HiddenField)dItem.FindControl("hdnQty");
                LinkButton lnkBtnItem = (LinkButton)dItem.FindControl("lnkBtnItem");
                HiddenField hdnPubLimit = (HiddenField)dItem.FindControl("hdnPubLimit");
                Label lblEmailOnly = (Label)dItem.FindControl("lblEmailOnly");
                if (!lblEmailOnly.Visible)  //***EAC dirty hack to tell if the line item is physical/virtual
                {
                lblQty.Text = hdnQty.Value;
                int valQty = Int32.Parse(hdnQty.Value);
                if (valQty > 0 && returnvalue == true)
                {

                    Counter += Int32.Parse(hdnQty.Value);
                    GlobalUtils.Utils UtilityMethods = new GlobalUtils.Utils();

                    returnvalue = UtilityMethods.IsQtyValueValid(hdnQty.Value, hdnPubLimit.Value);
                    if (returnvalue == false)
                    {
                        result = -1;
                        if (string.Compare(hdnPubLimit.Value, "1") == 0)
                            lblMessage.Text = "Sorry, you cannot order more than " + hdnPubLimit.Value + " copy of " + lnkBtnItem.Text + ".";
                        else
                            lblMessage.Text = "Sorry, you cannot order more than " + hdnPubLimit.Value + " copies of " + lnkBtnItem.Text + ".";
                    }
                }
                }
            }

            //Check for max order limit
            if (Counter > Int32.Parse(hdnTotalLimit.Value) && result == 1)
            {
                result = -1;
                lblMessage.Text = "Sorry, you cannot order more than " + hdnTotalLimit.Value + " items.";
            }

            return result;
        }
        
        protected void grdViewItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Add a JavaScript confirmation message to Remove button
                //Button btnRemove = (Button)e.Row.FindControl("btnRemove");
                //btnRemove.Attributes.Add("onclick", "return fnAskBeforeRemoval(" + "'" + btnRemove.ClientID + "')");

                Product p = (Product)e.Row.DataItem;

                ImageButton ImgBtnRemove = (ImageButton)e.Row.FindControl("ImgBtnRemove");
                ImgBtnRemove.Attributes.Add("onclick", "return fnAskBeforeRemoval(" + "'" + ImgBtnRemove.ClientID + "')");
                ImgBtnRemove.Attributes.Add("onmousedown", "this.src='images/remove_on.jpg'");
                ImgBtnRemove.Attributes.Add("onmouseup", "this.src='images/remove_off.jpg'");
                
                LinkButton lnkBtnItem = (LinkButton)e.Row.FindControl("lnkBtnItem");
                Label lblQty = (Label)e.Row.FindControl("lblQty");
                HiddenField hdnQty = (HiddenField)e.Row.FindControl("hdnQty");
                HiddenField hdnPubID = (HiddenField)e.Row.FindControl("hdnPubID");
                HiddenField hdnPubLimit = (HiddenField)e.Row.FindControl("hdnPubLimit");

                Panel pnlNerdo = (Panel)e.Row.FindControl("pnlNerdo");
                HyperLink NerdoContentlink = (HyperLink)e.Row.FindControl("NerdoContentlink");
                Label lblCoverOnly = (Label)e.Row.FindControl("lblCoverOnly");

                //lnkItem.Text = p.LongTitle;
                //lnkItem.NavigateUrl = "detail.aspx?ConfID=" + strConfId + "&prodid=" + p.ProductId;

                lnkBtnItem.Text = p.LongTitle;
                string LinkButtonCommandArgument = "detail.aspx?ConfID=" + strConfId + "&prodid=" + p.ProductId;
                lnkBtnItem.CommandArgument = LinkButtonCommandArgument; // e.Row.RowIndex.ToString();
                
                ////string Explanation = ""; //Right now only for NERDO pubs
               
                ////if (kvpaircoll == null)
                ////    kvpaircoll = DAL.DAL.GetKVPair("sp_KIOSK_getNerdoPubIdsURLS");

                ////foreach (KVPair kvpair in kvpaircoll)
                ////{
                ////    if (string.Compare(kvpair.Key, p.PubId.ToString()) == 0)
                ////    {
                ////        Explanation = ": Pack of 25 covers";
                ////        pnlNerdo.Style["display"] = "";
                ////        NerdoContentlink.NavigateUrl = kvpair.Val;
                ////        NerdoContentlink.Text = "Print separate contents";
                ////        lblCoverOnly.Visible = true;
                ////        lnkItem.NavigateUrl = p.Url;
                ////        lnkItem.Target = "_blank";
                ////        break;
                ////    }
                ////}

                lblQty.Text = p.NumQtyOrdered.ToString();
                hdnQty.Value = p.NumQtyOrdered.ToString();
                hdnPubID.Value = p.PubId.ToString();
                hdnPubLimit.Value = p.NumQtyLimit.ToString();
                ImgBtnRemove.CommandArgument = e.Row.RowIndex.ToString();
                

                //Up, Down image button JavaScript events
                ImageButton ImgBtnUp = (ImageButton)e.Row.FindControl("ImgBtnUp");
                
                //Handle single quote in the long title
                //string jsMessage = p.LongTitle.Replace("'", "\\'");
                string jsMessage = p.ProductId.Replace("'", "\\'"); //Pass ProductId since this value is not displayed anymore

                ImgBtnUp.Attributes.Add("onclick", "return fnUpdateQty(" + "'" + jsMessage + "','" + lblQty.ClientID + "','" + hdnQty.ClientID + "','" + hdnPubLimit.Value + "'," + "'" + hdnTotalLimit.Value + "'," + "'plus','" + lblTot.ClientID + "','" + lblFreeRemaining.ClientID + "','" + lblMessage.ClientID + "')");
                ImgBtnUp.Attributes.Add("onmousedown", "this.src='images/arrowup_on.jpg'");
                ImgBtnUp.Attributes.Add("onmouseup", "this.src='images/arrowup_off.jpg'");

                ImageButton ImgBtnDown = (ImageButton)e.Row.FindControl("ImgBtnDown");
                ImgBtnDown.Attributes.Add("onclick", "return fnUpdateQty(" + "'" + jsMessage + "','" + lblQty.ClientID + "','" + hdnQty.ClientID + "','" + hdnPubLimit.Value + "'," + "'" + hdnTotalLimit.Value + "'," + "'minus','" + lblTot.ClientID + "','" + lblFreeRemaining.ClientID + "','" + lblMessage.ClientID + "')");
                ImgBtnDown.Attributes.Add("onmousedown", "this.src='images/arrowdown_on.jpg'");
                ImgBtnDown.Attributes.Add("onmouseup", "this.src='images/arrowdown_off.jpg'");

                #region Handle ONLINE-only items (20130227)
                Label lblEmailOnly = (Label)e.Row.FindControl("lblEmailOnly");
                if (p.IsPhysicalItem) 
                {
                    ImgBtnUp.Visible = ImgBtnDown.Visible = true;
                    lblEmailOnly.Visible = false;
                    lblQty.Visible = true;
                }
                else
                {
                    //The item must be ONLINE-only, for emailing purposes
                    ImgBtnUp.Visible = ImgBtnDown.Visible = false;
                    lblEmailOnly.Visible = true;
                    lblQty.Visible = true;
                    lblQty.Text = "-";
                }
                #endregion
                //End events
            }
            lblTot.Text = this.shoppingcart.TotalQtyForShipping.ToString();
            //lblCost.Text = this.shoppingcart.Cost.ToString("c");
        }

        protected void grdViewItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "lnkBtn")
            {
                if (CheckForValidQuantity() < 0)
                    return;
                this.UpdateQty(); //Update the Shopping Cart Session Variables
                
                Response.Redirect(e.CommandArgument.ToString(), true);
            }

            if (e.CommandName == "Remove" && this.shoppingcart.Count > 0)
            {
                this.UpdateQtyInGrid();
                
                this.shoppingcart.Remove(this.shoppingcart[Int32.Parse (e.CommandArgument.ToString())]);
                lblFreeRemaining.Text = (Int32.Parse(hdnTotalLimit.Value) - this.shoppingcart.TotalQtyForShipping).ToString();

                grdViewItems.DataSource = this.shoppingcart;
                grdViewItems.DataBind();
                
                this.UpdateQty();

                if (this.shoppingcart.Count > 0)
                {
                    //Do not do anything
                }
                else
                {
                    Panel1.Visible = false;
                    Panel2.Visible = true;
                    ImgBtnCheckOut.Visible = false;
                }
            }
        }

        protected void btnCheckOut_Click(object sender, EventArgs e)
        {
            if (CheckForValidQuantity() < 0)
                return;
            
            this.UpdateQty(); //Update the Shopping Cart Session Variables

            if (shoppingcart.NumItemsForShipping + shoppingcart.NumItemsForEmailing > 0)
            {
                if (shoppingcart.NumItemsForShipping < 1)
                    Response.Redirect("kioskemailing.aspx?ConfID=" + strConfId, true);
                else
                    Response.Redirect("kioskshipping.aspx?ConfID=" + strConfId, true);
            }
            else
            {
                //shopping cart is empty....reset just to be sure
                Session["KIOSK_cart"] = null;
                Session["KIOSK_Pubs"] = "";
                Session["KIOSK_Qtys"] = "";
                Session["KIOSK_Urls"] = "";
                //display empty-cart message
                Panel1.Visible = false;
                Panel2.Visible = true;
                ImgBtnCheckOut.Visible = false;
            }
        }

        protected void btnContinueSearch_Click(object sender, EventArgs e)
        {
            if (CheckForValidQuantity() < 0)
                return;

            this.UpdateQty(); //Update the Shopping Cart Session Variables

            //if (this.shoppingcart.TotalQty > 0)
            //{
                
            //}
            //else//shopping cart is empty
            //{
            //    Session["KIOSK_cart"] = null;
            //    Session["KIOSK_Pubs"] = "";
            //    Session["KIOSK_Qtys"] = "";
            //    Panel1.Visible = false;
            //    Panel2.Visible = true;
            //    ImgBtnCheckOut.Visible = false;
            //}

            Response.Redirect("kiosksearch.aspx?ConfID=" + strConfId, true); //EAC assume ConfID is supplied to all pages
        }

        protected void btnCancelOrder_Click(object sender, EventArgs e)
        {
            Session["KIOSK_cart"] = null;
            Utils.ResetSessions();
            
            Response.Redirect("attract.aspx?ConfID=" + strConfId, true); //EAC assume ConfID is supplied to all pages
        }

        protected void lnkBtnClick(object sender, EventArgs e)
        {
            //LinkButton lnkBtn = (LinkButton)sender;
            //GridViewRow row = (GridViewRow)lnkBtn.Parent.Parent;
        }

        protected void lnkBtnItem_Command(object sender, CommandEventArgs e)
        {
            
            //LinkButton lnkBtn = (LinkButton)sender;
            //lnkBtn.PostBackUrl = e.CommandArgument.ToString() ;
            //lnkBtn.Attributes["href"] = e.CommandArgument.ToString();
        }

        private void GoToKioskSearch()
        {
            Session["KIOSK_TypeOfCancer"] = "";
            Session["KIOSK_Subject"] = "";
            Session["KIOSK_Audience"] = "";
            Session["KIOSK_ProductFormat"] = "";
            Session["KIOSK_Series"] = "";
            Session["KIOSK_Language"] = "";

            if (this.hidCancerType.Value.ToString() != "")
                Session["KIOSK_TypeOfCancer"] = this.hidCancerType.Value.ToString();
            if (this.hidSubject.Value.ToString() != "")
                Session["KIOSK_Subject"] = this.hidSubject.Value.ToString();
            if (this.hidAudience.Value.ToString() != "")
                Session["KIOSK_Audience"] = this.hidAudience.Value.ToString();
            if (this.hidProductFormat.Value.ToString() != "")
                Session["KIOSK_ProductFormat"] = this.hidProductFormat.Value.ToString();
            if (this.hidSeries.Value.ToString() != "")
                Session["KIOSK_Series"] = this.hidSeries.Value.ToString();
            if (this.hidLanguages.Value.ToString() != "")
                Session["KIOSK_Language"] = this.hidLanguages.Value.ToString();
            if (this.hidst.Value.ToString() != "")
            {
                //Session["KIOSK_Extratext"] = " for " + Server.HtmlEncode(this.hidst.Value.ToString());
                Session["KIOSK_Extratext"] = " for " + this.hidst.Value.ToString();
            }
            if (CheckForValidQuantity() < 0)
                return;

            this.UpdateQty(); //Update the Shopping Cart Session Variables
            
            Response.Redirect("kiosksearch.aspx?ConfID=" + strConfId, true); //EAC assume ConfID is supplied to all pages

        }

        //Event raised from the search pop up.
        protected void btnKioskSearch_Click(object sender, EventArgs e)
        {
            this.GoToKioskSearch();
        }

        protected void grdViewItems_RowCreated(object sender, GridViewRowEventArgs e)
        {
         }

    }
}
