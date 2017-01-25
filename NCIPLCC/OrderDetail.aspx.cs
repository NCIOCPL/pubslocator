using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PubEnt.DAL;
using PubEnt.BLL;

namespace PubEnt
{
    public partial class OrderDetail : System.Web.UI.Page
    {
        //ArrayList arrlstNerdoPubIds;
        KVPairCollection kvpaircoll;

        public string CustID = "";
        public string OrderNum = "";
        public string PrevPage = "";

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

        protected void Page_Load(object sender, EventArgs e)
        {
            //Check GUAM UserId, Role for NCIPL_CC
            if (GlobalUtils.UserRoles.getLoggedInUserId().Length == 0 || GlobalUtils.UserRoles.getLoggedInUserRole() < 1)
            {
                string currASPXfilename = System.IO.Path.GetFileName(Request.Path).ToString();
                Session["NCIPL_REGISTERREFERRER"] = currASPXfilename;
                Response.Redirect("~/login.aspx?msg=invaliduser&redir=" + currASPXfilename);
            }

            //--- Get data from query string 
            /*if (Request.QueryString["CustID"] != null)
            {
                CustID = Request.QueryString["CustID"].ToString();
            }
            if (Request.QueryString["OrderNum"] != null)
            {
                OrderNum = Request.QueryString["OrderNum"].ToString();
            }
            if (Request.QueryString["PrevPage"] != null)
            {
                PrevPage = Request.QueryString["PrevPage"].ToString();
            }*/

            if (Session["VIEW_CUSTID"] != null)
            {
                CustID = Session["VIEW_CUSTID"].ToString();
            }
            if (Session["VIEW_ORDERNUM"] != null)
            {
                OrderNum = Session["VIEW_ORDERNUM"].ToString();
            }
            if (Session["VIEW_PREVPAGE"] != null)
            {
                PrevPage = Session["VIEW_PREVPAGE"].ToString();
            }

            if (!IsPostBack)
            {
                //Display the master page tabs 
                GlobalUtils.Utils UtilMethod = new GlobalUtils.Utils();
                if (Session["NCIPL_Pubs"] != null)
                    Master.LiteralText = UtilMethod.GetTabHtmlMarkUp(Session["NCIPL_Qtys"].ToString(), "");
                else
                    Master.LiteralText = UtilMethod.GetTabHtmlMarkUp("", "");
                UtilMethod = null;

                GetOrderInfo();
            }
            else
            {

            }
        }

        protected void GetOrderInfo()
        {

            string strOrderSource = "";
            string strOrderCreator = "";

            Customer Cust = DAL.DAL.GetOrderCustInfobyOrderNum(OrderNum);

            lblInvoice.Text = OrderNum;
            if (Cust != null)
            {
                lblShipToName.Text = Cust.ShipToName;
                lblShipToOrg.Text = Cust.ShipToOrg;
                lblShipToAddr1.Text = Cust.ShipToAddr1;
                lblShipToAddr2.Text = Cust.ShipToAddr2;
                lblShipToZip5.Text = Cust.ShipToZip5;
                lblShipToZip4.Text = Cust.ShipToZip4;
                lblShipToCity.Text = Cust.ShipToCity;
                lblShipToState.Text = Cust.ShipToState;
                lblShipToPhone.Text = Cust.ShipToPhone;
                lblShipToEmail.Text = Cust.ShipToEmail;

                if (Cust.BillToName != "")
                {
                    lblBillToName.Text = Cust.BillToName;
                }
                else
                {
                    lblBillToName.Text = "&nbsp;";
                }
                lblBillToOrg.Text = Cust.BillToOrg;
                lblBillToAddr1.Text = Cust.BillToAddr1;
                lblBillToAddr2.Text = Cust.BillToAddr2;
                lblBillToZip5.Text = Cust.BillToZip5;
                lblBillToZip4.Text = Cust.BillToZip4;
                lblBillToCity.Text = Cust.BillToCity;
                lblBillToState.Text = Cust.BillToState;
                lblBillToPhone.Text = Cust.BillToPhone;
                lblBillToEmail.Text = Cust.BillToEmail;
            }

            IDataReader dr = DAL.DAL.GetOrderStatus(OrderNum);
            try
            {
                using (dr)
                {
                    if (dr.Read())
                    {
                        if (dr["ORDERSOURCE"].ToString() == "IWEB")
                        {
                            strOrderSource = "NCIPL";
                            strOrderCreator = "NCIPL user";
                        }
                        else if (dr["ORDERSOURCE"].ToString() == "IVPR")
                        {
                            strOrderSource = "VPR";
                            strOrderCreator = "VPR user";
                        }
                        else if (dr["ORDERSOURCE"].ToString() == "IEOT")
                        {
                            strOrderSource = "NCIPLex";
                            strOrderCreator = "Exhibit Team";
                        }
                        else if (dr["ORDERSOURCE"].ToString() == "ICIS")
                        {
                            strOrderSource = dr["ORDERMEDIA"].ToString();
                            strOrderCreator = dr["ORDERCREATOR"].ToString();
                        }

                        lblOrderSource.Text = strOrderSource;
                        lblOrderCreator.Text = strOrderCreator;
                        lblTypeofCustomer.Text = dr["CUSTOMERTYPE"].ToString();
                        
                        lblCreated.Text = dr["CREATEDATE"].ToString();

                        if (dr["MethodDisplay"].ToString() == "")
                        {
                            lblShipMethod.Text = "USPS";
                        }
                        else
                        {
                            lblShipMethod.Text = dr["MethodDisplay"].ToString();
                        }
                        
                        //---- Current Logic
                        /*
                        if (dr["DOWNLOAD"].ToString() == "Y")
                        {
                            lblDownload.Text = "Yes";

                           if (dr["TERMCODE"].ToString() != "B")
                            {
                                if (dr["PACKSLIP"].ToString() != "")
                                {
                                    lblShipped.Text = "Shipped - " + dr["DATE_SHIP"].ToString();

                                    if (dr["TRACKTRACE"].ToString() != "")
                                    {
                                        lblTracking.Text = dr["TRACKTRACE"].ToString();
                                    }
                                    else
                                    {
                                        lblTracking.Text = "";
                                    }
                                }
                                else
                                {
                                    lblShipped.Text = "Pending";
                                }
                            }
                            else
                            {
                                lblShipped.Text = "Backorder";
                            }
                        }
                        else
                        {
                            lblDownload.Text = "No";
                        }*/

                        
                        //--- New Logic 11/05/2012
                        if (dr["TERMCODE"].ToString() == "B")
                        {
                            lblShipped.Text = "Not Processed";
                            lblDownload.Text = "No";
                        }
                        else if (dr["TERMCODE"].ToString() == "R" || dr["TERMCODE"].ToString() == "P")
                        {
                            lblShipped.Text = "Held – Under Review";
                            lblDownload.Text = "No";
                        }
                        else if (dr["TERMCODE"].ToString() == "G")
                        {
                            lblShipped.Text = "Pending";

                            if (dr["DOWNLOAD"].ToString() == "Y")
                            {
                                lblDownload.Text = "Yes";
                            }
                            else
                            {
                                lblDownload.Text = "No";
                            }
                        }
                        else 
                        {
                            if (dr["TERMCODE"].ToString() == "" && dr["PACKSLIP"].ToString() == "")
                            {
                                lblShipped.Text = "Pending";

                                if (dr["DOWNLOAD"].ToString() == "Y")
                                {
                                    lblDownload.Text = "Yes";
                                }
                                else
                                {
                                    lblDownload.Text = "No";
                                }
                            }
                            else
                            {
                                if (dr["DOWNLOAD"].ToString() == "Y")
                                {
                                    lblDownload.Text = "Yes";
                                }
                                else
                                {
                                    lblDownload.Text = "No";
                                }

                                if (dr["PACKSLIP"].ToString() != "")
                                {
                                    lblShipped.Text = "Shipped - " + dr["DATE_SHIP"].ToString();

                                    if (dr["TRACKTRACE"].ToString() != "")
                                    {
                                        lblTracking.Text = dr["TRACKTRACE"].ToString();
                                    }
                                    else
                                    {
                                        lblTracking.Text = "";
                                    }
                                }
                            }
                        }
                        //---------------------------------------------------------------------------------
                    }
                }
            }
            catch (Exception ECREAx)
            {
                //TO DO: log any error
                if (!dr.IsClosed)
                    dr.Close();
            }

            ProductCollection col = DAL.DAL.GetHistOrderDetail(OrderNum);

            grdViewItems.DataSource = col;
            grdViewItems.DataBind();
        }

        protected void grdViewItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Add a JavaScript confirmation message to Remove button
                //Button Button2 = (Button)e.Row.FindControl("Button2");
                //Button2.Attributes.Add("onclick", "return fnAskBeforeRemoval(" + "'" + Button2.ClientID + "')");

                Product p = (Product)e.Row.DataItem;
                HyperLink lnkItem = (HyperLink)e.Row.FindControl("lnkItem");
                Label lblItem = (Label)e.Row.FindControl("lblItem");
                //Label lblQty = (Label)e.Row.FindControl("lblQty");
                //Label lblQtyText = (Label)e.Row.FindControl("lblQtyText");
                Label lblQuantity = (Label)e.Row.FindControl("lblQuantity");
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
                //lblQtyText.Text = "Limit ";
                //lblQty.Text = p.NumQtyLimit.ToString();
                txtQty.Text = p.NumQtyOrdered.ToString();
                lblQuantity.Text = p.NumQtyOrdered.ToString();
                hdnPubID.Value = p.PubId.ToString();
                //Button2.CommandArgument = e.Row.RowIndex.ToString();
            }
        }

        protected void grdViewItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            
        }

        protected void grdViewItems_PreRender(object sender, EventArgs e)
        {
            if (grdViewItems.Rows.Count > 0)
                grdViewItems.HeaderRow.TableSection = TableRowSection.TableHeader;
        }


        protected void btnReturn_Click(object sender, EventArgs e)
        {
            string temp = "";

            if (PrevPage == "CustDetail")
            {
                temp = "CustomerDetail.aspx?CustID=" + CustID;
            }
            else
            {
                temp = "SearchOrder_Result.aspx";
            }
            Response.Redirect(temp);
        }

        protected void chkReOrder_CheckedChanged(object sender, EventArgs e)
        {
            if (chkReOrder.Checked)
            {
                this.shoppingcart = null;     //destroy cart if it exists
                this.shoppingcart = new ProductCollection();

                ProductCollection colOrigOrder = DAL.DAL.GetOrigOrderDetail(OrderNum);

                /*foreach (GridViewRow item in grdViewItems.Rows)
                {
                    TextBox txtQty = (TextBox)item.FindControl("txtQty");
                    HiddenField hdnPubID = (HiddenField)item.FindControl("hdnPubID");

                    int pubid = Int32.Parse(hdnPubID.Value);
                    Product p = Product.GetCartPubByPubID(pubid);
                    p.NumQtyOrdered = Int32.Parse(txtQty.Text);
                    this.shoppingcart.Add(p);
                    
                    Session["NCIPL_Pubs"] = this.shoppingcart.Pubs;
                    Session["NCIPL_Qtys"] = this.shoppingcart.Qtys;
                }*/


                foreach (Product item in colOrigOrder)
                {
                    //TextBox txtQty = (TextBox)item.FindControl("txtQty");
                    //HiddenField hdnPubID = (HiddenField)item.FindControl("hdnPubID");

                    int pubid = item.PubId;
                    //Do not add to the cart if Pub is a Promotion Pub
                    if (DAL.DAL.IsPromotionPub(pubid) == 0)
                    {
                        Product p = Product.GetCartPubByPubID(pubid);
                        p.NumQtyOrdered = item.NumQtyOrdered;
                        this.shoppingcart.Add(p);

                        Session["NCIPL_Pubs"] = this.shoppingcart.Pubs;
                        Session["NCIPL_Qtys"] = this.shoppingcart.Qtys;
                    }
                }

                if (chkUseAddress4NewOrder.Checked)
                {
                }
                else
                {
                    Session["SEARCHORDER_CUSTID"] = CustID;

                    Customer Cust = DAL.DAL.GetOrderCustInfobyCustID(CustID);
                    this.shipto = new Person(1, Cust.ShipToName, Cust.ShipToOrg, Cust.ShipToEmail, Cust.ShipToAddr1, Cust.ShipToAddr2, Cust.ShipToCity, Cust.ShipToState, Cust.ShipToZip5, Cust.ShipToZip4, Cust.ShipToPhone);
                    this.billto = new Person(1, Cust.BillToName, Cust.BillToOrg, Cust.BillToEmail, Cust.BillToAddr1, Cust.BillToAddr2, Cust.BillToCity, Cust.BillToState, Cust.BillToZip5, Cust.BillToZip4, Cust.BillToPhone);
                }
            }
            else
            {
                Session["NCIPL_cart"] = null;           //destroy
                Session["NCIPL_Pubs"] = null;           //destroy
                Session["NCIPL_Qtys"] = null;           //destroy

                if (chkUseAddress4NewOrder.Checked)
                {
                }
                else
                {
                    Session["SEARCHORDER_CUSTID"] = null;   //destroy

                    Session["NCIPL_shipto"] = null;         //destroy
                    Session["NCIPL_billto"] = null;         //destroy
                }
            }

            //Display the master page tabs 
            GlobalUtils.Utils UtilMethod = new GlobalUtils.Utils();
            if (Session["NCIPL_Pubs"] != null)
                Master.LiteralText = UtilMethod.GetTabHtmlMarkUp(Session["NCIPL_Qtys"].ToString(), "cart");
            else
                Master.LiteralText = UtilMethod.GetTabHtmlMarkUp("", "cart");
            UtilMethod = null;
        }

        protected void chkUseAddress4NewOrder_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUseAddress4NewOrder.Checked)
            {
                if (chkReOrder.Checked)
                {
                }
                else
                {
                    Session["SEARCHORDER_CUSTID"] = CustID;

                    Customer Cust = DAL.DAL.GetOrderCustInfobyCustID(CustID);
                    this.shipto = new Person(1, Cust.ShipToName, Cust.ShipToOrg, Cust.ShipToEmail, Cust.ShipToAddr1, Cust.ShipToAddr2, Cust.ShipToCity, Cust.ShipToState, Cust.ShipToZip5, Cust.ShipToZip4, Cust.ShipToPhone);
                    this.billto = new Person(1, Cust.BillToName, Cust.BillToOrg, Cust.BillToEmail, Cust.BillToAddr1, Cust.BillToAddr2, Cust.BillToCity, Cust.BillToState, Cust.BillToZip5, Cust.BillToZip4, Cust.BillToPhone);
                }
            }
            else
            {
                if (chkReOrder.Checked)
                {
                }
                else
                {
                    Session["SEARCHORDER_CUSTID"] = null;   //destroy

                    Session["NCIPL_shipto"] = null;         //destroy
                    Session["NCIPL_billto"] = null;         //destroy
                }
            }
        }
    }
}
