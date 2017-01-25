using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NCIPLex.DAL;
using NCIPLex.BLL;

using System.Collections;
using NCIPLex.GlobalUtils;

namespace NCIPLex
{
    public partial class cart : System.Web.UI.Page
    {
        private string CostRecoveryInd = "";

        //ArrayList arrlstNerdoPubIds;
        KVPairCollection kvpaircoll;

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

            if (!IsPostBack)
            {

                #region NERDO_Related
                //arrlstNerdoPubIds = DAL.DAL.GetNerdoPubIds();
                kvpaircoll = SQLDataAccess.GetKVPair("sp_NCIPL_getNerdoPubIdsURLS");


                #endregion

                #region dummydata--remove before going live
                //Session["NCIPL_Pubs"] = "31,31";
                //Session["NCIPL_Qtys"] = "123,1";
                #endregion

                steps1.Activate("cell1");

                this.shoppingcart = null;     //destroy cart if it exists
                this.shoppingcart = new ProductCollection();
                              
                //***EAC Parse the NCIPL_Pubs and NCIPL_qtys..assume they have same dimensions
                string[] pubs = Session["NCIPLEX_Pubs"].ToString().Split(new Char[] { ',' });
                string[] qtys = Session["NCIPLEX_Qtys"].ToString().Split(new Char[] { ',' });
                for (var i=0; i<pubs.Length; i++)
                {
                    if (pubs[i].Trim() != "")
                    {
                        int pubid = Int32.Parse(pubs[i]);
                        Product p = Product.GetPubByPubID(pubid);
                        p.NumQtyOrdered = Int32.Parse(qtys[i]);
                        this.shoppingcart.Add(p);   //BLL will know what to do w/ dupes and 0 quantities
                    }
                }
                
                grdViewItems.DataSource = this.shoppingcart;
                grdViewItems.DataBind();
            }

            //Check for cart object here
            if (this.shoppingcart == null)
                Response.Redirect("default.aspx?redirect=cart1", true);
            
            if (this.shoppingcart.Count > 0)
            {
                //***EAC at this point we have a usable cart
                lblTot.Text = this.shoppingcart.TotalQty.ToString();
                //lblCost.Text = this.shoppingcart.Cost.ToString("c");

                /*Begin HITT 8716*/
                if (this.shoppingcart.Cost > 0.0)
                    CostRecoveryInd = "1";
                else
                    CostRecoveryInd = "";
                this.ToggleCostDivs(CostRecoveryInd);
                /*End HITT 8716*/

                Panel1.Visible = true;
                Panel2.Visible = false;
            }
            else//shopping cart is empty
            {
                Panel1.Visible = false;
                Panel2.Visible = true;
            }

            //Set the appropriate tab
            int intTotalQty = 0;
            if (Session["NCIPLEX_Qtys"] != null)
            {
                string[] qtys = Session["NCIPLEX_Qtys"].ToString().Split(new Char[] { ',' });
                for (int i = 0; i < qtys.Length; i++)
                {
                    if (qtys[i].Length > 0)
                        intTotalQty += Int32.Parse(qtys[i].ToString());
                }
            }

            //Display the master page tabs 
            GlobalUtils.Utils UtilMethod = new GlobalUtils.Utils();
            if (Session["NCIPLEX_Pubs"] != null)
                Master.LiteralText = UtilMethod.GetTabHtmlMarkUp(Session["NCIPLEX_Qtys"].ToString(), "cart");
            else
                Master.LiteralText = UtilMethod.GetTabHtmlMarkUp("", "cart");
            UtilMethod = null;

        }
        
        protected void Button1_Click1(object sender, EventArgs e)
        {
            ////this.cc.Cost = 0.0;
            //if (CheckForValidQuantity() < 0)
            //{
            //    return;
            //}
            //Response.Redirect("shipping.aspx", true);
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            if (CheckForValidQuantity() < 0)
            {
                return;
            }
            Response.Redirect("shipping.aspx", true);
        }

        protected void UpdateQty(object sender, EventArgs e)
        {
            if (CheckForValidQuantity() < 0)
            {   
                return;
            }
            
            //try
            //{
                //foreach (DataGridItem item in grdItems.Items)
                //{
                //    TextBox txtQty = (TextBox)item.FindControl("txtQty");
                //    HiddenField hdnPubID = (HiddenField)item.FindControl("hdnPubID");
                //    this.shoppingcart.UpdateQty(Int32.Parse(hdnPubID.Value), Int32.Parse(txtQty.Text));
                //    Session["NCIPL_Pubs"] = this.shoppingcart.Pubs;
                //    Session["NCIPL_Qtys"] = this.shoppingcart.Qtys;
                //}
                //grdItems.DataSource = this.shoppingcart;
                //grdItems.DataBind();

                //NCIPLex - new code
                int itemsCount = 0;
                foreach (GridViewRow item in grdViewItems.Rows)
                {
                    TextBox txtQty = (TextBox)item.FindControl("txtQty");
                    itemsCount += Int32.Parse(txtQty.Text);
                }
                if (itemsCount > Utils.GetOrderLimit())
                {
                    //Please enter a valid quantity.The limit is X total items per order.
                    Utils.DisplayMessage(ref MessagePlaceHolder2, 3);
                    return;
                    
                }
                //End new code

            
                foreach (GridViewRow item in grdViewItems.Rows)
                {
                    TextBox txtQty = (TextBox)item.FindControl("txtQty");
                    HiddenField hdnPubID = (HiddenField)item.FindControl("hdnPubID");
                    this.shoppingcart.UpdateQty(Int32.Parse(hdnPubID.Value), Int32.Parse(txtQty.Text));
                    Session["NCIPLEX_Pubs"] = this.shoppingcart.Pubs;
                    Session["NCIPLEX_Qtys"] = this.shoppingcart.Qtys;
                }
                grdViewItems.DataSource = this.shoppingcart;
                grdViewItems.DataBind();

            //}
            //catch (Exception ex)
            //{
            //    throw new ArgumentException(ex.Message);
            //}

                /*Begin HITT 8716*/
                if (this.shoppingcart.Cost > 0.0)
                    CostRecoveryInd = "1";
                else
                    CostRecoveryInd = "";
                this.ToggleCostDivs(CostRecoveryInd);
                /*End HITT 8716*/

                //Display the master page tabs 
                GlobalUtils.Utils UtilMethod = new GlobalUtils.Utils();
                if (Session["NCIPLEX_Pubs"] != null)
                    Master.LiteralText = UtilMethod.GetTabHtmlMarkUp(Session["NCIPLEX_Qtys"].ToString(), "cart");
                else
                    Master.LiteralText = UtilMethod.GetTabHtmlMarkUp("", "cart");
                UtilMethod = null;
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            //Response.Redirect("searchres.aspx");
            Response.Redirect("home.aspx");
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
                    kvpaircoll = SQLDataAccess.GetKVPair("sp_NCIPL_getNerdoPubIdsURLS");

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
            lblTot.Text = this.shoppingcart.TotalQty.ToString();
            //lblCost.Text = this.shoppingcart.Cost.ToString("c");
        }

        protected void grdViewItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Remove")
            {
                this.shoppingcart.Remove(this.shoppingcart[Int32.Parse (e.CommandArgument.ToString())]);
                Session["NCIPLEX_Pubs"] = this.shoppingcart.Pubs;
                Session["NCIPLEX_Qtys"] = this.shoppingcart.Qtys;
                
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

                /*Begin HITT 8716*/
                if (this.shoppingcart.Cost > 0.0)
                    CostRecoveryInd = "1";
                else
                    CostRecoveryInd = "";
                this.ToggleCostDivs(CostRecoveryInd);
                /*End HITT 8716*/

                //Display the master page tabs 
                GlobalUtils.Utils UtilMethod = new GlobalUtils.Utils();
                if (Session["NCIPLEX_Pubs"] != null)
                    Master.LiteralText = UtilMethod.GetTabHtmlMarkUp(Session["NCIPLEX_Qtys"].ToString(), "cart");
                else
                    Master.LiteralText = UtilMethod.GetTabHtmlMarkUp("", "cart");
                UtilMethod = null;
            }
        }

        protected void grdViewItems_PreRender(object sender, EventArgs e)
        {
            grdViewItems.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        //HITT 8716 - Show cost related text only for cost recovery orders
        private void ToggleCostDivs(string costindicator)
        {
            string CostRecovery = "";
            if (costindicator.Length > 0)
                CostRecovery = costindicator;
            if (string.Compare(CostRecovery, "") == 0)
            {
                //lblCCText.Visible = false;
                //divCost.Visible = false;
                divCCExplanation.Visible = false;
                divOrderingHelp.Visible = false;
            }
            else
            {
                //divCost.Visible = true;
                divCCExplanation.Visible = true;
                divOrderingHelp.Visible = true;
            }
        }
    }
}
