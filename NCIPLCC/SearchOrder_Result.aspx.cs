using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PubEnt.DAL;
using PubEnt.BLL;

using System.Text;
using System.Collections.Specialized;

namespace PubEnt
{
    public partial class SearchOrder_Result : System.Web.UI.Page
    {
        public int strCustomerType = 0;
        public string strKeyWord = "";

        public string strSDate = "";
        public string strEDate = "";

        public bool bTotalPage = false;
        public string strNav = "";
        public int iSearchOrderCount = 0;
        public int iRecPerPage = 10;
        public int iCurrPage = 1;
        public int iTotalPage;
        public int iFromRec;
        public int iToRec;
        public string strSearchSort = "CASE WHEN (SHIPTOEMAIL IS NULL OR SHIPTOEMAIL = '') THEN 1 ELSE 0 END, SHIPTOEMAIL, SHIPTONAME, CUSTID ASC"; //"SHIPTONAME, SHIPTOEMAIL, CUSTID ASC";
       
        protected void Page_Load(object sender, EventArgs e)
        {
            //Check GUAM UserId, Role for NCIPL_CC
            if (GlobalUtils.UserRoles.getLoggedInUserId().Length == 0 || GlobalUtils.UserRoles.getLoggedInUserRole() < 1)
            {
                string currASPXfilename = System.IO.Path.GetFileName(Request.Path).ToString();
                Session["NCIPL_REGISTERREFERRER"] = currASPXfilename;
                Response.Redirect("~/login.aspx?msg=invaliduser&redir=" + currASPXfilename);
            }

            if (!Page.IsPostBack)
            {
                //Display the master page tabs 
                GlobalUtils.Utils UtilMethod = new GlobalUtils.Utils();
                if (Session["NCIPL_Pubs"] != null)
                    Master.LiteralText = UtilMethod.GetTabHtmlMarkUp(Session["NCIPL_Qtys"].ToString(), "");
                else
                    Master.LiteralText = UtilMethod.GetTabHtmlMarkUp("", "");
                UtilMethod = null;

                bTotalPage = true;

                Session["VIEW_CUSTID"] = "";
                Session["VIEW_CUSTID"] = "";
                Session["VIEW_PREVPAGE"] = "";

                DoSearch();
            }
            else
            {
                if (hidCustID.Value != "" && hidOrderNum.Value != "")
                {
                    Session["VIEW_CUSTID"] = hidCustID.Value;
                    Session["VIEW_ORDERNUM"] = hidOrderNum.Value;
                    Session["VIEW_PREVPAGE"] = hidPrevPage.Value;
                    
                    Response.Redirect("OrderDetail.aspx");
                }
                else
                {
                    Session["VIEW_CUSTID"] = "";
                    Session["VIEW_CUSTID"] = "";
                    Session["VIEW_PREVPAGE"] = "";

                    GetPostBackValue();
                }
            }
            
        }

        protected void GetPostBackValue()
        {
            //--- Get current page
            if (drpQJumpsN.SelectedValue.Length > 0)
            {
                iCurrPage = Convert.ToInt32(drpQJumpsN.SelectedValue);
                Session["SEARCHORDER_CURRPAGE"] = iCurrPage.ToString();
            }
            
            DoSearch();
        }


        protected void DoSearch()
        {
            string strResult = "";
            string strOrder = "";
            string strOrderDate = "";
            string currShipToEmail = "";

            if (Session["SEARCHORDER_CUSTOMERTYPEDESC"] != null)
            {
                if (Session["SEARCHORDER_CUSTOMERTYPEDESC"].ToString() != "")
                {
                    lblTypeofCustomer.Text = Session["SEARCHORDER_CUSTOMERTYPEDESC"].ToString();
                    divTypeofCustomer.Visible = true;
                }
                else
                {
                    lblTypeofCustomer.Text = "";
                    divTypeofCustomer.Visible = false;
                }
            }
            
            if (Session["SEARCHORDER_CUSTOMERTYPE"] != null)
            {
                if (Session["SEARCHORDER_CUSTOMERTYPE"].ToString() != "" && Session["SEARCHORDER_CUSTOMERTYPE"].ToString() != "All")
                {
                    strCustomerType = Convert.ToInt32(Session["SEARCHORDER_CUSTOMERTYPE"].ToString());
                }
                else
                {
                    strCustomerType = 0;
                }
            }
            else
            {
                strCustomerType = 0;
            }

            if (Session["SEARCHORDER_KEYWORD"] != null)
            {
                strKeyWord = Session["SEARCHORDER_KEYWORD"].ToString().Replace("'", "''");

                if (Session["SEARCHORDER_KEYWORD"].ToString() != "")
                {
                    lblSearchPhase.Text = Session["SEARCHORDER_KEYWORD"].ToString();
                    divSearchPhase.Visible = true;
                }
                else
                {
                    lblSearchPhase.Text = "";
                    divSearchPhase.Visible = false;
                }
            }
            else
            {
                lblSearchPhase.Text = "";
                divSearchPhase.Visible = false;
            }

            if (Session["SEARCHORDER_SDATE"] != null)
            {
                strSDate = Session["SEARCHORDER_SDATE"].ToString();

                if (Session["SEARCHORDER_SDATE"].ToString() != "")
                {
                    lblStartDate.Text = Session["SEARCHORDER_SDATE"].ToString();
                    divStartDate.Visible = true;
                }
                else
                {
                    lblStartDate.Text = "";
                    divStartDate.Visible = false;
                }
            }
            else
            {
                lblStartDate.Text = "";
                divStartDate.Visible = false;
            }

            if (Session["SEARCHORDER_EDATE"] != null)
            {
                strEDate = Session["SEARCHORDER_EDATE"].ToString();

                if (Session["SEARCHORDER_EDATE"].ToString() != "")
                {
                    lblEndDate.Text = Session["SEARCHORDER_EDATE"].ToString();
                    divEndDate.Visible = true;
                }
                else
                {
                    lblEndDate.Text = "";
                    divEndDate.Visible = false;
                }
            }
            else
            {
                lblEndDate.Text = "";
                divEndDate.Visible = false;
            }

            if (Session["SEARCHORDER_CURRPAGE"] != null)
            {
                if (Session["SEARCHORDER_CURRPAGE"].ToString() != "")
                {
                    iCurrPage = Convert.ToInt32(Session["SEARCHORDER_CURRPAGE"].ToString());
                }
            }

            iToRec = iCurrPage * iRecPerPage;
            iFromRec = iToRec - iRecPerPage + 1;
            
            iSearchOrderCount = DAL.DAL.GetSearchOrderCount(strCustomerType, strKeyWord, strSDate, strEDate);
            iTotalPage = 0;
            if (iSearchOrderCount > 0)
            {
                this.divSearchOrderResult.Style["display"] = "";
                this.divNoRecord.Style["display"] = "none";
                
                //--- Calculate Total number of pages for search result
                if ((iSearchOrderCount % iRecPerPage) == 0)
                {
                    iTotalPage = iSearchOrderCount / iRecPerPage;
                }
                else
                {
                    iTotalPage = (iSearchOrderCount / iRecPerPage) + 1;
                }

                //--- Search Result count
                this.lblRecordCount.Text = iSearchOrderCount.ToString();
                this.divRecNavigation.Visible = true;
                this.divRecNavigationBtm.Visible = true;

                //--- Get navigation on right
                Display_RecNavigation();

                //--- Get navigation dropdown
                if (bTotalPage)
                {
                    Navigation_AssignPages(iSearchOrderCount, iRecPerPage, iTotalPage);
                }

                
                CustomerCollection CustColl = DAL.DAL.GetSearchOrderResult(strCustomerType, strKeyWord, strSearchSort, strSDate, strEDate, iFromRec, iToRec);

                foreach (Customer o in CustColl)
                {
                    if (currShipToEmail == o.ShipToEmail)
                    {
                        //strResult = strResult + "<tr>" +
                        //            "<td colspan=\"5\">" +
                        //            "&nbsp;" +
                        //            "</td>" +
                        //            "</tr>";
                    }
                    else
                    {
                        //strResult = strResult + "<tr>" +
                        //            "<td colspan=\"5\">" +
                        //            "<div style=\"height:1px; border-top:solid 1px #bbb\"></div>" +
                        //            "</td>" +
                        //            "</tr>";
                    }

                    currShipToEmail = o.ShipToEmail;
                    
                    strOrder = "";
                    strOrderDate = "";

                    //StringCollection OrderColl = DAL.DAL.GetOrderByCustomer(o.CustID);
                    //foreach (string orderinvoice in OrderColl)

                    KVPairCollection kvpairColl = KVPair.GetKVPair_Order_Date(o.CustID);
                    foreach (KVPair kvpair in kvpairColl)
                    {
                        //strOrder = strOrder + "<a href=\"OrderDetail.aspx?CustID=" + o.CustID + "&OrderNum=" + orderinvoice + "&PrevPage=SearchOrder\">" + orderinvoice + "</a>";
                        strOrder = strOrder + "<a href=\"javascript:ViewOrderDetail('" + drpQJumpsN.ClientID + "'";
                        strOrder = strOrder + ", '" + hidCustID.ClientID + "'";
                        strOrder = strOrder + ", '" + hidOrderNum.ClientID + "'";
                        strOrder = strOrder + ", '" + hidPrevPage.ClientID + "'";
                        //strOrder = strOrder + ", '" + o.CustID + "', '" + orderinvoice + "', 'SearchOrder');\">" + orderinvoice + "</a>";
                        strOrder = strOrder + ", '" + o.CustID + "', '" + kvpair.Key + "', 'SearchOrder');\">" + kvpair.Key + "</a>";
                        strOrder = strOrder + "<br />";

                        strOrderDate = strOrderDate + kvpair.Val;
                        strOrderDate = strOrderDate + "<br />";
                    }

                    strResult = strResult + "<tr>" +
                                "<td>" + o.ShipToName + "<br />";
                                //"<a href=\"CustomerDetail.aspx?CustID=" + o.CustID + "\">" + o.ShipToName + "</a>" + "<br />";

                    if (o.ShipToAddr1 != "") 
                    {
                        strResult = strResult + o.ShipToAddr1 + "<br />";
                    }
                    if (o.ShipToAddr2 != "")
                    {
                        strResult = strResult + o.ShipToAddr2 + "<br />";
                    }
                    if (o.ShipToCity != "" || o.ShipToState != "" || o.ShipToZip5 != "")
                    {
                        strResult = strResult + o.ShipToCity + ", " + o.ShipToState + " " + o.ShipToZip5;
                    }
                    
                    strResult = strResult + "</td>" +
                                "<td>" + o.ShipToOrg + "</td>" +
                                "<td>" + o.ShipToEmail + "</td>" +
                                "<td>" + strOrder + "</td>" +
                                "<td>" + strOrderDate + "</td>" +
                                "</tr>";

                }

                ltrlSearchOrderResult.Text = strResult;
            }
            else
            {
                this.lblRecordCount.Text = "";
                this.divRecNavigation.Visible = false;
                this.divRecNavigationBtm.Visible = false;

                this.divSearchOrderResult.Style["display"] = "none";
                this.divNoRecord.Style["display"] = "";
            }
        }

        protected void Display_RecNavigation()
        {
            strNav = "";

            if (iTotalPage > 1)
            {
                if (iCurrPage > 1)
                {
                    // build - move first:
                    //strNav = strNav + " <a href=\"javascript:Navigate('N', 1, '" + strPageName + "');\" class=\"link_navigation\"";
                    strNav = strNav + " <a href=\"javascript:Navigate('" + drpQJumpsN.ClientID + "', 1);\" class=\"link_navigation\"";
                    strNav = strNav + " onMouseOver=\"self.status='First Page'; return true;\" onMouseOut=\"self.status='';\"";
                    strNav = strNav + " onFocus=\"self.status='First Page'; return true;\" onBlur=\"self.status=''; return true;\"";
                    strNav = strNav + " alt=\"First Page\" title=\"Go to First Page\"> << </a>";

                    // build - move previous:
                    //strNav = strNav + " <a href=\"javascript:Navigate('N'," + (iCurrPage - 1) + ", '" + strPageName + "');\" class=\"link_navigation\"";
                    strNav = strNav + " <a href=\"javascript:Navigate('" + drpQJumpsN.ClientID + "', " + (iCurrPage - 1) + ");\" class=\"link_navigation\"";
                    strNav = strNav + " onMouseOver=\"self.status='Previous Page'; return true;\" onMouseOut=\"self.status='';\"";
                    strNav = strNav + " onFocus=\"self.status='Previous Page'; return true;\" onBlur=\"self.status=''; return true;\"";
                    strNav = strNav + " alt=\"Previous Page\" title=\"Go to Previous Page\"> < </a>";
                }
                strNav = strNav + " Page " + iCurrPage + " of " + iTotalPage;
                if (iCurrPage < iTotalPage)
                {
                    // build - move next:
                    //strNav = strNav + " <a href=\"javascript:Navigate('N', " + (iCurrPage + 1) + ", '" + strPageName + "');\" class=\"link_navigation\"";
                    strNav = strNav + " <a href=\"javascript:Navigate('" + drpQJumpsN.ClientID + "', " + (iCurrPage + 1) + ");\" class=\"link_navigation\"";
                    strNav = strNav + " onMouseOver=\"self.status='Next Page'; return true;\" onMouseOut=\"self.status='';\"";
                    strNav = strNav + " onFocus=\"self.status='Next Page'; return true;\" onBlur=\"self.status=''; return true;\"";
                    strNav = strNav + " alt=\"First Page\" title=\"Go to Next Page\"> > </a>";
                    // build - move last:
                    //strNav = strNav + " <a href=\"javascript:Navigate('N', " + iTotalPage + ", '" + strPageName + "');\" class=\"link_navigation\"";
                    strNav = strNav + " <a href=\"javascript:Navigate('" + drpQJumpsN.ClientID + "', " + iTotalPage + ");\" class=\"link_navigation\"";
                    strNav = strNav + " onMouseOver=\"self.status='Last Page'; return true;\" onMouseOut=\"self.status='';\"";
                    strNav = strNav + " onFocus=\"self.status='Last Page'; return true;\" onBlur=\"self.status=''; return true;\"";
                    strNav = strNav + " alt=\"Last Page\" title=\"Go to Last Page\"> >> </a>";
                }
            }

        }

        protected void Navigation_AssignPages(int iRecsTotal, int iRecsPage, int iPages)
        {
            int iCounter;
            string sName, sVal;

            iCounter = 1;
            for (int intI = 0; intI < iPages; intI++)
            {
                if (intI == iPages - 1)
                {
                    sName = iCounter.ToString() + " - " + iRecsTotal.ToString();
                }
                else
                {
                    sName = iCounter.ToString() + " - " + (iCounter + iRecsPage - 1).ToString();
                }
                sVal = (intI + 1).ToString();
                ListItem item = new ListItem(sName, sVal);
                drpQJumpsN.Items.Add(item);
                iCounter = iCounter + iRecsPage;
            }

            if (iCurrPage > 1)
            {
                drpQJumpsN.SelectedValue = iCurrPage.ToString();
            }
        }

        protected void drpQJumpsN_Changed(object sender, EventArgs e)
        {
            //GetPostBackValue();
        }
    }
}
