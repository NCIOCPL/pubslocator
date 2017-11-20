using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using NCIPLex.BLL;
using NCIPLex.DAL;

namespace NCIPLex
{
    public partial class confirm_order : System.Web.UI.Page
    {
        public string idletimeout = "";
        public string idle2timeout = "";
        
        //NCIPLex KVPairCollection Nerdos = new KVPairCollection();
        string CostRecoverInd = "";

        ProductCollection shoppingcartV2   //***EAC 2nd version of shopping cart w/ virtual/linked pubs
        {
            get
            {
                if ((ProductCollection)Session["NCIPLEX_PrinterGrid"] == null)
                    return null;
                else
                    return (ProductCollection)Session["NCIPLEX_PrinterGrid"];
            }
            //set { Session["NCIPL_cartV2"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            idletimeout = SQLDataAccess.GetTimeout(1, 2).ToString() + "000";
            idle2timeout = SQLDataAccess.GetTimeout(1, 3).ToString();
            
            if (Session["NCIPLEX_DONE"] == null)
            {   
                Response.End();
                return;
            }

            if (Session["NCIPLEX_PrinterFriendly"] != null)
            {
                //Label1.Text = Session["NCIPL_PrinterFriendly"].ToString();
                //DataSet ds = (DataSet)Session["NCIPL_PrinterGrid"];

                string shippingtext = "", billingtext = "", paymenttext = "";

                lblorderdt.Text = System.DateTime.Today.ToShortDateString();
                if (Session["NCIPLEX_PrinterShipping"] != null)
                {
                    shippingtext = Session["NCIPLEX_PrinterShipping"].ToString();
                    divShipping.InnerHtml = shippingtext;
                }
                if (Session["NCIPLEX_PrinterBilling"] != null)
                {
                    billingtext = Session["NCIPLEX_PrinterBilling"].ToString();
                    //divBilling.InnerHtml = billingtext;
                }
                if (Session["NCIPLEX_PrinterPayment"] != null)
                {
                    paymenttext = Session["NCIPLEX_PrinterPayment"].ToString();
                    //divPayment.InnerHtml = paymenttext;
                }

                if (Session["NCIPLEX_PrinterCostRecovery"] != null)
                {
                    if (string.Compare(Session["NCIPLEX_PrinterCostRecovery"].ToString(), "True") == 0)
                        CostRecoverInd = "1";
                    else
                        CostRecoverInd = "0";
                }
                else
                    CostRecoverInd = "0";

                //if (string.Compare(CostRecoverInd, "1") == 0)
                //{
                    //divBilling.Visible = true;
                    //divPayment.Visible = true;
                    //divCCPrompt.Visible = true;
                //}
                //else
                //{
                    //divBilling.Visible = false;
                    //divPayment.Visible = false;
                    //divCCPrompt.Visible = false;
                //}


                grdItems.DataSource = this.shoppingcartV2;//(ProductCollection)Session["NCIPL_PrinterGrid"];
                grdItems.DataBind();

                //Nerdos = (KVPairCollection)Session["NCIPL_NerdoDataList"];
                //if (Nerdos.Count > 0)
                //{
                //    ListNerdos.DataSource = Nerdos;
                //    ListNerdos.DataBind();
                //}
                //else
                //    divNerdo.Visible = false;
                //divNerdo.Visible = false; //NCIPLex

                lblTotalItems.Text = this.shoppingcartV2.TotalQty.ToString();
                //if (string.Compare(CostRecoverInd, "1") == 0)
                    //lblCost.Text = this.shoppingcartV2.Cost.ToString("c");
                //else
                    //divCost.Visible = false;
            }
            else
                Response.Redirect("location.aspx?redirect=confirmation", true); //Not valid
        }

        //DataGrid Bind
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

                //if (p.ProductId.ToLower().EndsWith("c"))
                //{
                //    KVPair Nerdo = new KVPair();
                //    //***EAc  @%$#*@# UI is giving me problems...hardcoding class for now
                //    lblTitle.Text = "<span class=textLoud>Cover Only: </span>" + p.LongTitle;
                //    Nerdo.Key = p.LongTitle;
                //    Nerdo.Val = NCIPLex.DAL2.DAL.GetNerdoURLByChild(p.ProductId);
                //    Nerdos.Add(Nerdo);
                //}
            }
        }

        //Datalist for Nerdo Contents
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
    }
}
