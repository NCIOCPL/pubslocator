using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//Manually added
using PubEnt.DAL;
using PubEnt.BLL;
using System.Data;
using PubEnt.GlobalUtils;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace PubEnt
{
    public partial class outofstock : System.Web.UI.Page
    {   
        #region Delegates and cart/cc objects

        protected override void OnInit(EventArgs e)
        {
            searchbar.SearchButtonClick += new EventHandler(SearchBar_btnSearchClick);
            base.OnInit(e);
        }
        private void SearchBar_btnSearchClick(object sender, EventArgs e)
        {
            //BindData(DropDownSortResultsTop.SelectedValue);
            //BindData();
            divResultsInfo.Visible = true;
            divNewUpdated.Visible = false;

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


            //CR-31 HITT 9815 BindDataForSorting(DropDownSortResultsTop.SelectedValue);
            /*Begin CR-31 - HITT 9815 */
            GlobalUtils.Utils objUtils = new GlobalUtils.Utils();
            string QueryParams = objUtils.GetQueryStringParams();
            objUtils = null;
            Response.Redirect("searchres.aspx" + "?sid=" + QueryParams, true);
            /*End CR-31 - HITT 9815 */
        }   
        #endregion   
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!GlobalUtils.Utils.isLoggedin())
            {  //***EAC I need a logged in user at this point
                GlobalUtils.Utils.ClearAllSessions();
                Response.Redirect("~/login.aspx", true);
            }

            string role = GlobalUtils.Utils.LoggedinRole();
            if (role != "NCIPL_LM" && role != "NCIPL_CC" && role != "NCIPL_POS")
            {
                throw (new ArgumentException("Your role is not allowed at this point."));
            }           
            //Display the master page tabs 
            GlobalUtils.Utils UtilMethod = new GlobalUtils.Utils();
            if (Session["NCIPL_Pubs"] != null)
                Master.LiteralText = UtilMethod.GetTabHtmlMarkUp(Session["NCIPL_Qtys"].ToString(), "");
            else
                Master.LiteralText = UtilMethod.GetTabHtmlMarkUp("", "");
            UtilMethod = null;


            if (!IsPostBack)
            {               
                PubOrderOK.OnClientClick = String.Format("fnAddItemToCart('{0}','{1}')", PubOrderOK.UniqueID, "");
                PubCoverOrderOK.OnClientClick = String.Format("fnAddItemToCart('{0}','{1}')", PubCoverOrderOK.UniqueID, "");

                //Newly added code to reset textbox when cancel button is clicked on popup
                PubOrderCancel.OnClientClick = String.Format("fnCancelClick('{0}')", QuantityOrdered.ClientID);
                PubCoverOrderCancel.OnClientClick = String.Format("fnCancelClick('{0}')", QuantityOrderedCover.ClientID);

                //CustomValidator1.ClientValidationFunction = String.Format("fnCheckValue('{0}','{1}','{2}')", QuantityOrdered.ClientID, PubQtyLimit.ClientID, labelErrMsgPubOrder.ClientID);
                PubOrderLink.Attributes.Add("href", "javascript:fnPostBack(" + "'" + PubOrderOK.UniqueID + "','" + "','" + QuantityOrdered.ClientID + "','" + PubQtyLimit.ClientID + "','" + labelErrMsgPubOrder.ClientID + "','" + PubOrderModalPopup.BehaviorID + "')");
                PubCoverOrderLink.Attributes.Add("href", "javascript:fnPostBack(" + "'" + PubCoverOrderOK.UniqueID + "','" + "','" + QuantityOrderedCover.ClientID + "','" + CoverQtyLimit.ClientID + "','" + labelErrMsgPubCover.ClientID + "','" + PubCoverOrderModalPopup.BehaviorID + "')");


                //Deal with changes on the search results for "Out-of-Stock Publications"
                literalDesc.Text = "Out of Stock Publications";
                divResultsInfo.Visible = false;
                divNewUpdated.Visible = true;
                //End of code for changes for "Out-of-Stock Publications"
                
                
                
                if (string.Compare(Session["JSTurnedOn"].ToString(), "False") == 0) //No JavaScript - display all data
                {
                    labelSortTop.Visible = false;
                    DropDownSortResultsTop.Visible = false;

                    //labelSortBottom.Visible = false;
                    //DropDownSortResultsBottom.Visible = false;

                    labelShowTop.Visible = false;
                    DropDownNumResultsTop.Visible = false;

                    labelShowBottom.Visible = false;
                    DropDownNumResultsBottom.Visible = false;

                    labelPageTop.Visible = false;
                    DataPagerTop.PageSize = 10000; //Set a very high value so all records are displayed
                    DataPagerTop.Visible = false;

                    labelPageBottom.Visible = false;
                    DataPagerBottom.PageSize = 10000; //Set a very high value so all records are displayed
                    DataPagerBottom.Visible = false;

                    //BindData("LONGTITLE"); //default
                    BindData();
                }
                else
                    BindData();

                if (Session["CurrentProdId"] == null) 
                    this.ClearScrollPosition(); //JavaScript function call - CR-31 HITT 9817
            }
            
            #region Rebuild Dynamic Controls
            if (IsPostBack)
            {
                //Begin - HITT 9846
                //The grid place holder controls needs to re-populated on each postback
                this.RebuildDynamicControls();
                //End - HITT 9846

                this.SetScrollPosition(); //JavaScript function call - CR-31 HITT 9817
            }
            #endregion

            if (!IsPostBack && Session["CurrentProdId"] != null && this.Page.Request.UrlReferrer.ToString().Contains("detail.aspx")) //an addition condition added for CR 28
            {
                Session["CurrentProdId"] = null;
                this.SetScrollPosition();
            }
            
        }

        //Populate the listview
        //private void BindData(string SortField)
        private void BindData()
        {   
            ProductCollection p = DAL.DAL.GetOutofStockPubs();


             //CR-28 User coming in from detail page
             #region BackToSearchResults
             if (p.Count > 0)
             {
                 if (Session["CurrentProdId"] != null && this.Page.Request.UrlReferrer.ToString().Contains("detail.aspx"))
                 {
                     DropDownNumResultsTop.SelectedValue = Session["PUBENT_PageSize"].ToString();
                     DropDownNumResultsBottom.SelectedValue = Session["PUBENT_PageSize"].ToString();

                     DropDownSortResultsTop.SelectedValue = Session["PUBENT_PageSortIndex"].ToString();
                     if (DropDownSortResultsTop.SelectedIndex == 1)
                         p.Sort(ProductCollection.ProductFields.LongTitle, true);
                     else if (DropDownSortResultsTop.SelectedIndex == 2)
                         p.Sort(ProductCollection.ProductFields.RevisedDate, false);

                     int recCounter = 0;
                     foreach (Product pr in p)
                     {   
                         if (string.Compare(pr.ProductId, Session["CurrentProdId"].ToString(), true) == 0)
                             break;
                         recCounter++;
                     }
                     int targetrow = recCounter;
                     int rowsperpage = Int32.Parse(Session["PUBENT_PageSize"].ToString());
                     int targetpage = 0;
                     int quotient = targetrow / rowsperpage;
                     int remainder = targetrow % rowsperpage;
                     //if (remainder > 0)
                     //    targetpage = quotient + 1;
                     //else
                         targetpage = quotient;

                     
                     //DataPagerTop.SetPageProperties(10, 10, false);
                     DataPagerTop.SetPageProperties(targetpage * rowsperpage, rowsperpage, false);
                     //Session["CurrentProdId"] = null;
                     //return;
                 }
             }
             #endregion
            
            ListViewSearchResults.DataSource = p;
            ListViewSearchResults.DataBind();
            if (p.Count == 0)
            {
                divPagerTop.Style.Add("display", "none");
                divPagerBottom.Style.Add("display", "none");
                divZeroResults.Visible = true;
            }
            else
                divZeroResults.Visible = false;
            if (p.Count == 1)
                labelDisplayText.Text = "Result for:";
            else
                labelDisplayText.Text = "Results for:";
            this.labelResultsCount.Text = p.Count.ToString();
            if (Session["PUBENT_Criteria"] != null)
                this.labelSearchCriteria.Text = Session["PUBENT_Criteria"].ToString();
        }


        private void BindDataForSorting(string SortField)
        {   
            //CR-36 ProductCollection p = DAL.DAL.GetNewUpdatedProducts();
            ProductCollection p = DAL.DAL.GetNewUpdatedProducts(1);
            
            switch (SortField)
            {
                case "LONGTITLE":
                    p.Sort(ProductCollection.ProductFields.LongTitle, true);
                    break;
                case "RECORDUPDATEDATE":
                    p.Sort(ProductCollection.ProductFields.RecordUpdateDate, true);
                    break;
                case "REVISEDDATE":
                    p.Sort(ProductCollection.ProductFields.RevisedDate, false);
                    break;
            }
            ListViewSearchResults.DataSource = p;
            ListViewSearchResults.DataBind();
            if (p.Count == 0)
            {
                divPagerTop.Style.Add("display", "none");
                divPagerBottom.Style.Add("display", "none");
                divZeroResults.Visible = true;
            }
            else
                divZeroResults.Visible = false;
            if (p.Count == 1)
                labelDisplayText.Text = "Search Result for:";
            else
                labelDisplayText.Text = "Search Results for:";
            this.labelResultsCount.Text = p.Count.ToString();
            if (Session["PUBENT_Criteria"] != null)
                this.labelSearchCriteria.Text = Session["PUBENT_Criteria"].ToString();
            //if (Session["PUBENT_SearchKeyword_OLD"] == null || Session["PUBENT_SearchKeyword"].ToString() != Session["PUBENT_SearchKeyword_OLD"].ToString())
            //{
            //    PubEnt.DAL2.DAL.SaveSearch(Session["PUBENT_SearchKeyword"].ToString(), p.Count);
            //    Session["PUBENT_SearchKeyword_OLD"] = Session["PUBENT_SearchKeyword"];
            //}
            GlobalUtils.Utils.SaveSearch(p.Count); //CR-31 HITT 7074
        }
        
        //Needed to get datapager work properly
        protected void ListViewSearchResults_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            int startrowindex = e.StartRowIndex;
            if (DataPagerTop.TotalRowCount < e.MaximumRows || DataPagerBottom.TotalRowCount < e.MaximumRows)
                startrowindex = 0;

            //DataPagerTop.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
            //DataPagerBottom.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
            DataPagerTop.SetPageProperties(startrowindex, e.MaximumRows, false);
            DataPagerBottom.SetPageProperties(startrowindex, e.MaximumRows, false);
            //if (DropDownNumResultsTop.SelectedIndex > 0)
            if (DropDownSortResultsTop.SelectedIndex > 0)
                BindDataForSorting(DropDownSortResultsTop.SelectedValue);
            else
                BindData();

            //COMMENTED CR-31 HITT 9817 this.ResetScrollPosition(); //HITT 8715
        }

        //User changed the number of items to be displayed dropdown
        protected void DropDownNumResultsTop_SelectedIndexChanged(object sender, EventArgs e)
        {

            int NumSelected, NumValue;
            NumSelected = Convert.ToInt32(this.DropDownNumResultsTop.SelectedValue);
            switch (NumSelected)
            {
                case 10: //TO DO: Change back to 10 later, after testing
                    NumValue = 10; //TO DO: Change back to 10 later, after testing
                    break;
                case 20:          //TO DO: Uncomment, after testing
                    NumValue = 20;
                    break;
                case 30:
                    NumValue = 30;
                    break;
                case 50:
                    NumValue = 50;
                    break;
                case 100:
                    NumValue = 100;
                    break;
                default:
                    NumValue = this.DataPagerTop.TotalRowCount;
                    break;
            }

            this.DropDownNumResultsBottom.Text = NumSelected.ToString();
            //this.DataPagerTop.PageSize = NumValue;
            //this.DataPagerBottom.PageSize = NumValue;

            //Reset data pager to first page
            DataPagerTop.SetPageProperties(0, NumValue, true);
            DataPagerBottom.SetPageProperties(0, NumValue, true);
            Session["PUBENT_PageSize"] = NumValue.ToString();
        }

        //User changed the number of items to be displayed dropdown
        protected void DropDownNumResultsBottom_SelectedIndexChanged(object sender, EventArgs e)
        {
            int NumSelected, NumValue;
            NumSelected = Convert.ToInt32(this.DropDownNumResultsBottom.SelectedValue);
            switch (NumSelected)
            {
                case 10: //TO DO: Change back to 10 later, after testing
                    NumValue = 10;  //TO DO: Change back to 10 later, after testing
                    break;
                case 20:          //TO DO: Uncomment, after testing
                    NumValue = 20;
                    break;
                case 30:
                    NumValue = 30;
                    break;
                case 50:
                    NumValue = 50;
                    break;
                case 100:
                    NumValue = 100;
                    break;
                default:
                    NumValue = this.DataPagerTop.TotalRowCount;
                    break;
            }

            this.DropDownNumResultsTop.Text = NumSelected.ToString();
            //this.DataPagerTop.PageSize = NumValue;
            //this.DataPagerBottom.PageSize = NumValue;

            //Reset data pager to first page
            DataPagerTop.SetPageProperties(0, NumValue, true);
            DataPagerBottom.SetPageProperties(0, NumValue, true);
            Session["PUBENT_PageSize"] = NumValue.ToString();
        }

        protected void DropDownSortResultsTop_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["PUBENT_PageSortIndex"] = DropDownSortResultsTop.SelectedValue; //CR 28
            //BindData(this.DropDownSortResultsTop.SelectedValue);
            BindDataForSorting(this.DropDownSortResultsTop.SelectedValue);
            //DropDownSortResultsBottom.Text = DropDownSortResultsTop.Text;

            //Reset data pager to first page
            int numrecords = 10;
            try { numrecords = Int32.Parse(this.DropDownNumResultsTop.SelectedValue); }
            catch { }
            DataPagerTop.SetPageProperties(0, numrecords, true);
            DataPagerBottom.SetPageProperties(0, numrecords, true);
            
        }

        //protected void DropDownSortResultsBottom_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    //BindData(this.DropDownSortResultsBottom.SelectedValue);
        //    BindDataForSorting(this.DropDownSortResultsBottom.SelectedValue);
        //    DropDownSortResultsTop.Text = DropDownSortResultsBottom.Text;

        //    //Reset data pager to first page
        //    int numrecords = 10;
        //    try { numrecords = Int32.Parse(this.DropDownNumResultsTop.SelectedValue); }
        //    catch { }
        //    DataPagerTop.SetPageProperties(0, numrecords, true);
        //    DataPagerBottom.SetPageProperties(0, numrecords, true);
        //}

        protected void ListViewSearchResults_ItemDataBound(object sender, ListViewItemEventArgs e)
        {  
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem item = (ListViewDataItem)e.Item;
                Product ProductItem = (Product)item.DataItem; //Get the Current Product Item in the collection

                //Get the Controls
                HiddenField PubId = (HiddenField)e.Item.FindControl("PubId");
                HiddenField ProdId = (HiddenField)e.Item.FindControl("ProdId");
                HiddenField PubIdCover = (HiddenField)e.Item.FindControl("PubIdCover");
                Image PubImage = (Image)e.Item.FindControl("PubImage");
                Image NewOrUpdatedImage = (Image)e.Item.FindControl("NewOrUpdatedImage");
                if (ProductItem.PubLargeImage.Length > 0)
                {
                    Panel panelLargeImg = (Panel)e.Item.FindControl("panelLargeImg");
                    panelLargeImg.Visible = true;
                    HyperLink MagnifierLink = (HyperLink)e.Item.FindControl("MagnifierLink");
                    MagnifierLink.Visible = true;
                    MagnifierLink.NavigateUrl = "dispimage.aspx?prodid=" + ProductItem.ProductId;
                }
                HyperLink DetailLink = (HyperLink)e.Item.FindControl("DetailLink");
                Label PublicationNumber = (Label)e.Item.FindControl("PublicationNumber"); //HITT 8515
                Label ProductFormat = (Label)e.Item.FindControl("ProductFormat");
                Label PubLastUpdateDate = (Label)e.Item.FindControl("PubLastUpdateDate");
                Label NumOfPubPages = (Label)e.Item.FindControl("NumOfPubPages");
                
                //ImageButton ReadOnline = (ImageButton)e.Item.FindControl("ReadOnline");
                ImageButton OrderPublication = (ImageButton)e.Item.FindControl("OrderPublication");
                ImageButton OrderCover = (ImageButton)e.Item.FindControl("OrderCover");

                //HyperLink ReadOnlineLink = (HyperLink)e.Item.FindControl("ReadOnlineLink");
                HyperLink HtmlLink = (HyperLink)e.Item.FindControl("HtmlLink");
                HyperLink PdfLink = (HyperLink)e.Item.FindControl("PdfLink");
                HyperLink KindleLink = (HyperLink)e.Item.FindControl("KindleLink");
                HyperLink EpubLink = (HyperLink)e.Item.FindControl("EpubLink");
                Label labelOrderMsg = (Label)e.Item.FindControl("labelOrderMsg");
                Label labelCoverMsg = (Label)e.Item.FindControl("labelCoverMsg");    

                //Assign Values
                PubId.Value = ProductItem.PubId.ToString();
                ProdId.Value = ProductItem.ProductId;
                PubIdCover.Value = ProductItem.PubIdCover.ToString();
                
                string imagepath = ConfigurationManager.AppSettings["PubImagesURL"];
                PubImage.ImageUrl = imagepath + "/" + ProductItem.PubImage;
                PubImage.AlternateText = ProductItem.ShortTitle + " (" + ProductItem.NumQtyAvailable.ToString() + ")";
                //PubImage.ImageUrl = "pubimages/" + ProductItem.PubImage;
                DetailLink.Text = ProductItem.LongTitle;
                DetailLink.NavigateUrl = "detail.aspx?prodid=" + ProductItem.ProductId;

                DetailLink.Attributes.Add("onclick", "fnSetScroll()"); //CR 28
                OrderPublication.Attributes.Add("onclick", "fnSetScroll()"); //CR-31 HITT 9817
                OrderCover.Attributes.Add("onclick", "fnSetScroll()"); //CR-31 HITT 9817
                

                PublicationNumber.Text = "#" + ProductItem.ProductId; //HITT 8515
                if (ProductItem.NumPages.Length > 0)
                {
                    if (string.Compare(ProductItem.NumPages, "1") == 0)
                        NumOfPubPages.Text = " - " + ProductItem.NumPages + " page";
                    else
                        NumOfPubPages.Text = " - " + ProductItem.NumPages + " pages";
                }

                //Display Last Updated Date
                if (ProductItem.RevisedMonth.Length > 0 && ProductItem.RevisedDay.Length > 0 && ProductItem.RevisedYear.Length > 0)
                    PubLastUpdateDate.Text = /*ProductItem.RevisedDateType +*/ ProductItem.RevisedMonth + " " + ProductItem.RevisedDay + ", " + ProductItem.RevisedYear;
                else if (ProductItem.RevisedMonth.Length > 0 && ProductItem.RevisedYear.Length > 0)
                    PubLastUpdateDate.Text = /*ProductItem.RevisedDateType +*/ ProductItem.RevisedMonth + " " + ProductItem.RevisedYear;
                else if (ProductItem.RevisedYear.Length > 0)
                    PubLastUpdateDate.Text = /*ProductItem.RevisedDateType +*/ ProductItem.RevisedYear;

                if (PubLastUpdateDate.Text.Length > 0)
                    PubLastUpdateDate.Text = " - " + PubLastUpdateDate.Text;


                if (ProductItem.Format.Length > 0)
                    ProductFormat.Text = "<br/>Format: " + ProductItem.Format;
                
                
                

                //Begin CR-30: HITT 8719 - Pub Translations 
                //[Note: plcTranslations population code is also present in RebuildDynamicControls()]
                PlaceHolder plcTranslations = (PlaceHolder)e.Item.FindControl("plcTranslations");
                int counter = 0;
                Panel linksPanel = new Panel();
                foreach (Product tranlatedproduct in DAL.DAL.GetProductTranslations(ProductItem.PubId))
                {
                    string MainProdId = ProductItem.ProductId; //HITT 11476
                    HyperLink TranslationsLink = new HyperLink();
                    TranslationsLink.CssClass = "linkProdTitle";
                    TranslationsLink.Text = tranlatedproduct.Language;
                    //HITT 11476 TranslationsLink.NavigateUrl = "detail.aspx?prodid=" + tranlatedproduct.ProductId;
                    TranslationsLink.NavigateUrl = "detail.aspx?prodid=" + tranlatedproduct.ProductId + "&mnprodid=" + MainProdId; //HITT 11476
                    TranslationsLink.Attributes.Add("onclick", "fnSetScroll()"); //HITT 11476
                    Label spaceLabel = new Label();
                    if (counter > 0 && counter % 3 == 0 )
                        spaceLabel.Text = "<br/>";
                    else
                        spaceLabel.Text = "&nbsp;";
                    linksPanel.Controls.Add(TranslationsLink);
                    linksPanel.Controls.Add(spaceLabel);
                    TranslationsLink = null;
                    spaceLabel = null;
                    counter++;
                }
                if (counter > 0)
                {
                    Label Translations = new Label();
                    Translations.CssClass = "textDefault";
                    //HITT 9914 Translations.Text = "Translations: ";
                    Translations.Text = "Also Available In: ";
                    linksPanel.Controls.AddAt(0, Translations);
                    Translations = null;
                    plcTranslations.Controls.Add(linksPanel);
                }
                linksPanel = null;
                //End CR-30

                //Handle - New or Updated image
                if (string.Compare(ProductItem.NewOrUpdated, "", true) == 0)
                {
                    //NewOrUpdatedImage.AlternateText = "";
                    //NewOrUpdatedImage.ImageUrl = "pubimages/" + "" //give later
                    //NewOrUpdatedImage.Style.Add("display", "none"); //Better to set visible to false due to Section 508 "Alt" requirements
                    NewOrUpdatedImage.Visible = false;
                }
                else if (string.Compare(ProductItem.NewOrUpdated, "UPDATED", true) == 0)
                {
                    NewOrUpdatedImage.AlternateText = "Updated";
                    NewOrUpdatedImage.ImageUrl = "images/" + "updated.gif"; //give later
                }
                else if (string.Compare(ProductItem.NewOrUpdated, "NEW", true) == 0)
                {
                    NewOrUpdatedImage.AlternateText = "New";
                    NewOrUpdatedImage.ImageUrl = "images/" + "new.gif"; //give later
                }
                   
                
                if (ProductItem.CanView == 1)
                {
                    //ReadOnlineLink.Visible = true;
                    //ReadOnlineLink.Target = "_blank";
                    //ReadOnlineLink.NavigateUrl = ProductItem.Url;
                    ((Label)e.Item.FindControl("textbreaker1")).Visible = true;
                    if (ProductItem.Url.Length > 0)
                    {   
                        HtmlLink.Visible = true;
                        HtmlLink.NavigateUrl = ProductItem.Url;
                        
                    }

                    if (ProductItem.PDFUrl.Length > 0)
                    {
                        //((Label)e.Item.FindControl("textbreaker2")).Visible = true;
                        PdfLink.Visible = true;
                        PdfLink.NavigateUrl = ProductItem.PDFUrl;
                    }
                    #region SHOW EBOOK URLS IF ANY
                    string kindle = DAL2.DAL.EbookUrl(ProductItem.PubId, "kindle");
                    if (kindle.Length > 0)
                    {
                        KindleLink.Visible = true;
                        KindleLink.NavigateUrl = kindle;
                    }

                    string epub = DAL2.DAL.EbookUrl(ProductItem.PubId, "epub");
                    if (epub.Length > 0)
                    {
                        EpubLink.Visible = true;
                        EpubLink.NavigateUrl = epub;
                    }
                    #endregion
                }

                if (ProductItem.OrderMsg.Length > 0)
                {
                    //HITT 7445 labelOrderMsg.Visible = true;
                    labelOrderMsg.Text = ProductItem.OrderMsg;
                }

                if (ProductItem.CanOrder == 1)
                {
                    OrderPublication.Visible = true;
                }

                if (ProductItem.CoverMsg.Length > 0)
                {
                    //HITT 7445 labelCoverMsg.Visible = true;
                    labelCoverMsg.Text = ProductItem.CoverMsg;
                }

                if (ProductItem.CanOrderCover == 1)
                {
                    OrderCover.Visible = true;
                }

                //Check whether this pub is already in cart
                if (!IsItemInCart(ProductItem.PubId.ToString()))
                {
                    OrderPublication.CommandArgument = ProductItem.PubId.ToString();
                    
                    OrderPublication.ImageUrl = "images/order_off.gif";
                    //OrderPublication.AlternateText = "Order Publication";
                    OrderPublication.AlternateText = "Order";

                    //NCIPL_CC
                    Utils objUtilMethod = new Utils();
                    string msg = "";
                    if (!objUtilMethod.IsTotalQtyValidForRole("", "", out msg))
                    {
                        OrderPublication.CommandArgument = "";
                        OrderPublication.ImageUrl = "images/LimitReached.gif";
                        OrderPublication.AlternateText = "Limit Reached";
                    }
                    objUtilMethod = null;

                }
                else
                {
                    OrderPublication.CommandArgument = "";
                    OrderPublication.ImageUrl = "images/PublicationInYourCart_off.gif";
                    OrderPublication.AlternateText = "Publication - In Your Cart";
                }

                //Check whether this pub is already in cart
                if (!IsItemInCart(ProductItem.PubIdCover.ToString()))
                {
                    OrderCover.CommandArgument = ProductItem.PubIdCover.ToString();
                    
                    OrderCover.ImageUrl = "images/ordercovers_off.gif";   //TODO: replace image
                    //CR 28 OrderCover.AlternateText = "Order Covers Only";
                    OrderCover.AlternateText = "Order Covers";
                }
                else
                {
                    OrderCover.CommandArgument = "";
                    OrderCover.ImageUrl = "images/CoverOnlyInYourCart_off.gif";
                    OrderCover.AlternateText = "Covers Only - In Your Cart";
                }
                
            }
        }


        //Server Side Event to call the Modal Popup Extender (Shopping Cart)
        //Or call method to add a single item to Shopping Cart if JavaScript is turned off
        protected void DisplayModalPopUp(object sender, CommandEventArgs e)
        {

            if (e.CommandArgument.ToString().Length == 0)
            {
                Response.Redirect("cart.aspx", true);
                return;
            }

            if (string.Compare(Session["JSTurnedOn"].ToString(), "False") == 0)
            {
                //Add a default quantity of one to the shopping cart if JavaScript is not enabled
                if (!IsItemInCart(e.CommandArgument.ToString())) //Check for browser re-load
                {
                    Session["NCIPL_Pubs"] += e.CommandArgument.ToString() + ",";
                    Session["NCIPL_Qtys"] += "1" + ",";
                }

                ImageButton OrderedPub = (ImageButton)sender;
                OrderedPub.CommandArgument = "";
                OrderedPub.ImageUrl = "images/PublicationInYourCart_off.gif";
                OrderedPub.AlternateText = "Publication - In Your Cart";

                Utils objUtilMethod = new Utils();
                string msg = "";
                //Change the searh results list view here to show "In Cart" items
                foreach (ListViewItem dItem in this.ListViewSearchResults.Items)
                {
                    if (dItem.ItemType == ListViewItemType.DataItem)
                    {
                        bool boolOtherPub = true; //NCIPL_CC
                        ImageButton OrderedPub2 = (ImageButton)dItem.FindControl("OrderPublication");
                        if (string.Compare(OrderedPub2.CommandArgument, PubOrderOK.CommandArgument, true) == 0)
                        {
                            OrderedPub2.CommandArgument = "";
                            OrderedPub2.ImageUrl = "images/PublicationInYourCart_off.gif";
                            OrderedPub2.AlternateText = "Publication - In Your Cart";
                            boolOtherPub = false; //NCIPL_CC
                        }

                        if (boolOtherPub) //NCIPL_CC
                        {
                            if (OrderedPub2.CommandArgument.Length > 0)
                            {
                                if (!objUtilMethod.IsTotalQtyValidForRole("", "", out msg))
                                {
                                    OrderedPub2.CommandArgument = "";
                                    OrderedPub2.ImageUrl = "images/LimitReached.gif";
                                    OrderedPub2.AlternateText = "Limit Reached";
                                }
                            }
                        }

                    }
                }
                objUtilMethod = null; //Declared for NCIPL_CC

                
                //Display the master page tabs 
                GlobalUtils.Utils UtilMethod = new GlobalUtils.Utils();
                if (Session["NCIPL_Pubs"] != null)
                    Master.LiteralText = UtilMethod.GetTabHtmlMarkUp(Session["NCIPL_Qtys"].ToString(), "");
                else
                    Master.LiteralText = UtilMethod.GetTabHtmlMarkUp("", "");
                UtilMethod = null;
            }
            else
            {
                this.PubOrderOK.CommandArgument = e.CommandArgument.ToString();

                Product p = DAL.DAL.GetProductbyPubID(Convert.ToInt32(e.CommandArgument));
                labelPubTitle.Text = p.LongTitle;
                PubQtyLimit.Text = p.NumQtyLimit.ToString();
                PubLimitLabel.Text = "Limit " + PubQtyLimit.Text;

                //Need to call update panel update to populate the values
                UpdatePanelOrderPub.UpdateMode = UpdatePanelUpdateMode.Conditional;
                UpdatePanelOrderPub.Update();
                
                //Show the Modal Popup
                this.PubOrderModalPopup.Show();
            }

        }

        #region NCIPL_CC_COMMENT_NERDO (If NERDO is Needed then make sure to add functionality for Role total limitation, as in Regular pubs (Refer DisplayModalPopUp() code)
        protected void DisplayModalPopUpCover(object sender, CommandEventArgs e)
        {
            //if (e.CommandArgument.ToString().Length == 0)
            //{
            //    Response.Redirect("cart.aspx", true);
            //    return;
            //}
          
            //if (string.Compare(Session["JSTurnedOn"].ToString(), "False") == 0)
            //{
            //    //Add a default quantity of one to the shopping cart if JavaScript is not enabled
            //    if (!IsItemInCart(e.CommandArgument.ToString())) //Check for browser re-load
            //    {
            //        Session["NCIPL_Pubs"] += e.CommandArgument.ToString() + ",";
            //        Session["NCIPL_Qtys"] += "1" + ",";
            //    }

            //    ImageButton OrderedCover = (ImageButton)sender;
            //    OrderedCover.CommandArgument = "";
            //    OrderedCover.ImageUrl = "images/CoverOnlyInYourCart_off.gif";
            //    OrderedCover.AlternateText = "Covers Only - In Your Cart";


            //    //Display the master page tabs 
            //    GlobalUtils.Utils UtilMethod = new GlobalUtils.Utils();
            //    if (Session["NCIPL_Pubs"] != null)
            //        Master.LiteralText = UtilMethod.GetTabHtmlMarkUp(Session["NCIPL_Qtys"].ToString(), "");
            //    else
            //        Master.LiteralText = UtilMethod.GetTabHtmlMarkUp("", "");
            //    UtilMethod = null;

            //}
            //else
            //{
            //    this.PubCoverOrderOK.CommandArgument = e.CommandArgument.ToString();
                
            //    Product p = DAL.DAL.GetProductbyPubID(Convert.ToInt32(e.CommandArgument));
            //    labelCoverPubTitle.Text = p.LongTitle;
            //    CoverQtyLimit.Text = p.NumQtyLimit.ToString(); //p.NumQtyAvailable.ToString();
            //    CoverLimitLabel.Text = "Pack of 25 covers" + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + "Limit " + CoverQtyLimit.Text;

            //    //Get the content URL - TO DO, can be optimized later to use a stored procedure 
            //    //that returns only one value.
            //    KVPairCollection kvpaircoll = DAL.DAL.GetKVPair("sp_NCIPL_getNerdoPubIdsURLS"); ;
            //    foreach (KVPair kvpair in kvpaircoll)
            //    {
            //        if (string.Compare(kvpair.Key, p.PubId.ToString()) == 0)
            //        {
            //            linkCoverPubUrl.NavigateUrl = kvpair.Val;
            //            break;
            //        }
            //    }

            //    //Need to call update panel update to populate the values
            //    UpdatePanelOrderCover.UpdateMode = UpdatePanelUpdateMode.Conditional;
            //    UpdatePanelOrderCover.Update();
                
            //    //Show the Modal Popup
            //    this.PubCoverOrderModalPopup.Show();
            //}
        }
        #endregion

        protected void PubOrderOK_Click(object sender, EventArgs e)
        {
            if (!IsQtyValueValid(QuantityOrdered.Text, PubQtyLimit.Text))
            {
                labelErrMsgPubOrder.Text = "Please enter a valid quantity.";
                UpdatePanelOrderPub.UpdateMode = UpdatePanelUpdateMode.Conditional;
                UpdatePanelOrderPub.Update();
                this.PubOrderModalPopup.Show();
                return;
            }
            
            //NCIPL_CC Specific Check
            Utils objUtilMethod = new Utils();
            string msg = "";
            if (!objUtilMethod.IsTotalQtyValidForRole(this.PubOrderOK.CommandArgument.ToString(), QuantityOrdered.Text.Trim(), out msg))
            {
                labelErrMsgPubOrder.Text = msg;
                UpdatePanelOrderPub.UpdateMode = UpdatePanelUpdateMode.Conditional;
                UpdatePanelOrderPub.Update();
                this.PubOrderModalPopup.Show();
                return;
            }
            //objUtilMethod = null; -- objUtilMethod to be used again below
            //End NCIPL_CC Specific Check

            int qty = Int32.Parse(QuantityOrdered.Text);
            if (qty > 0)
            {
                if (!IsItemInCart(this.PubOrderOK.CommandArgument.ToString())) //Check for browser re-load
                {
                    Session["NCIPL_Pubs"] += this.PubOrderOK.CommandArgument.ToString() + ",";
                    Session["NCIPL_Qtys"] += qty + ",";
                }
                
                //Change the searh results list view here to show "In Cart" items
                foreach (ListViewItem dItem in this.ListViewSearchResults.Items)
                {
                    if (dItem.ItemType == ListViewItemType.DataItem)
                    {
                        bool boolOtherPub = true; //NCIPL_CC
                        ImageButton OrderedPub = (ImageButton)dItem.FindControl("OrderPublication");
                        if (string.Compare(OrderedPub.CommandArgument, PubOrderOK.CommandArgument, true) == 0)
                        {
                            OrderedPub.CommandArgument = "";
                            OrderedPub.ImageUrl = "images/PublicationInYourCart_off.gif";
                            OrderedPub.AlternateText = "Publication - In Your Cart";
                            boolOtherPub = false; //NCIPL_CC
                        }

                        if (boolOtherPub) //NCIPL_CC
                        {
                            if (OrderedPub.CommandArgument.Length > 0)
                            {
                                if (!objUtilMethod.IsTotalQtyValidForRole("", "", out msg))
                                {
                                    OrderedPub.CommandArgument = "";
                                    OrderedPub.ImageUrl = "images/LimitReached.gif";
                                    OrderedPub.AlternateText = "Limit Reached";
                                }
                            }
                        }

                    }
                }
                objUtilMethod = null; //Declared for NCIPL_CC

                //Display the master page tabs 
                GlobalUtils.Utils UtilMethod = new GlobalUtils.Utils();
                if (Session["NCIPL_Pubs"] != null)
                    Master.LiteralText = UtilMethod.GetTabHtmlMarkUp(Session["NCIPL_Qtys"].ToString(), "");
                else
                    Master.LiteralText = UtilMethod.GetTabHtmlMarkUp("", "");
                UtilMethod = null;

                QuantityOrdered.Text = "1"; //Reset through code

            }
            else
            {
                //don't do anything for now
            }
        }

        #region NCIPL_CC_COMMENT_NERDO (If NERDO is Needed then make sure to add functionality for Role total limitation, as in Regular pubs (Refer PubOrderOK() code))
        protected void PubCoverOrderOK_Click(object sender, EventArgs e)
        {
            //if (!IsQtyValueValid(QuantityOrderedCover.Text, CoverQtyLimit.Text))
            //{
            //    labelErrMsgPubCover.Text = "Please enter a valid quantity.";
            //    UpdatePanelOrderCover.UpdateMode = UpdatePanelUpdateMode.Conditional;
            //    UpdatePanelOrderCover.Update();
            //    this.PubCoverOrderModalPopup.Show();
            //    return;
            //}
            
            //int qty = Int32.Parse(QuantityOrderedCover.Text);
            //if (qty > 0)
            //{
            //    if (!IsItemInCart(this.PubCoverOrderOK.CommandArgument.ToString())) //Check for browser re-load
            //    {
            //        Session["NCIPL_Pubs"] += this.PubCoverOrderOK.CommandArgument.ToString() + ",";
            //        Session["NCIPL_Qtys"] += qty + ",";
            //    }

            //    //Change the searh results listview here to show "In Cart" items
            //    foreach (ListViewItem dItem in this.ListViewSearchResults.Items)
            //    {
            //        if (dItem.ItemType == ListViewItemType.DataItem)
            //        {
            //            ImageButton OrderedCover = (ImageButton)dItem.FindControl("OrderCover");
            //            if (string.Compare(OrderedCover.CommandArgument, PubCoverOrderOK.CommandArgument, true) == 0)
            //            {
            //                OrderedCover.CommandArgument = "";
            //                OrderedCover.ImageUrl = "images/CoverOnlyInYourCart_off.gif";
            //                OrderedCover.AlternateText = "Covers Only - In Your Cart";
            //            }
            //        }
            //    }

            //    //Display the master page tabs 
            //    GlobalUtils.Utils UtilMethod = new GlobalUtils.Utils();
            //    if (Session["NCIPL_Pubs"] != null)
            //        Master.LiteralText = UtilMethod.GetTabHtmlMarkUp(Session["NCIPL_Qtys"].ToString(), "");
            //    else
            //        Master.LiteralText = UtilMethod.GetTabHtmlMarkUp("", "");
            //    UtilMethod = null;

            //    QuantityOrderedCover.Text = "1"; //Reset through code

            //}
            //else
            //{
            //    //don't do anything for now
            //}
        }
        #endregion

        private bool IsQtyValueValid(string val, string limit)
        {
            bool boolValidVal = false;
            if (!string.IsNullOrEmpty(val))
            {
                try
                {
                    //Int32.Parse(QuantityOrdered.Text);
                    if (Int32.Parse(val) <= Int32.Parse(limit) && Int32.Parse(val) > 0)
                        boolValidVal = true;
                    else
                        boolValidVal = false;
                }
                catch (FormatException)
                {
                    boolValidVal = false;
                }
            }
            else
                boolValidVal = false;

            return boolValidVal;
        }

        private bool IsItemInCart(string currentPub)
        {
            bool IsInCart = false;

            if (Session["NCIPL_Pubs"] == null)
                return IsInCart;

            string[] pubs = Session["NCIPL_Pubs"].ToString().Split(new Char[] { ',' });
            for (var i = 0; i < pubs.Length; i++)
            {
                if (pubs[i].Trim() != "")
                {
                    if (string.Compare(currentPub, pubs[i], true) == 0)
                    {
                        IsInCart = true;
                        break;
                    }
                }
            }
            
            return IsInCart;
        }

        ////Reset Scroll Position for pager, dropdown events
        //private void ResetScrollPosition() //NOT USED ANYMORE AFTER CR-31 HITT 9817
        //{
        //    if (!ClientScript.IsClientScriptBlockRegistered(GetType(), "CreateResetScrollPosition"))
        //    {
        //       //Create the ResetScrollPosition() function
        //       ClientScript.RegisterClientScriptBlock(GetType(), "CreateResetScrollPosition",
        //                        "function ResetScrollPosition() {\n" +
        //                        " var scrollX = document.getElementById('__SCROLLPOSITIONX');\n" +
        //                        " var scrollY = document.getElementById('__SCROLLPOSITIONY');\n" +
        //                        " if (scrollX && scrollY) {\n" +
        //                        "    scrollX.value = 0;\n" +
        //                        "    scrollY.value = 0;\n" +
        //                        " }\n" +
        //                        "}", true);

        //       //Add the call to the ResetScrollPosition() function
        //       ClientScript.RegisterStartupScript(GetType(), "CallResetScrollPosition", "ResetScrollPosition();", true);
        //    }
        //}

        //Clear scroll position if page is loading for the first time - CR-31 HITT 9817
        private void ClearScrollPosition()
        {
            if (!ClientScript.IsClientScriptBlockRegistered(GetType(), "CreateClearScrollPosition"))
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "CreateClearScrollPosition",
                    "function fnClearScroll() {\n" +
                    "   window.name = '';\n" +
                    "}", true);
                ClientScript.RegisterStartupScript(GetType(), "CallClearScrollPosition", "fnClearScroll();", true);
            }
        }

        //Set the scroll position after postback - CR-31 HITT 9817
        private void SetScrollPosition()
        {
            if (!ClientScript.IsClientScriptBlockRegistered(GetType(), "CreateSetScrollPosition"))
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "CreateSetScrollPosition",
                    "function fnGetScroll() {\n" +
                    "   var xPos = 0; var yPos = 0;\n" +
                    "   var objname = '';\n" +
                    "   objname = window.name;" +
                    "   if (objname.length > 0) {\n" +
                    "       var arrPos = objname.split('_');\n" +
                    "       xPos = arrPos[0];\n" +
                    "       yPos = arrPos[1];\n" +
                    "       setTimeout('window.scrollTo(0,' + yPos + ')', 100);\n" +
                    "   }\n" +
                    "   window.name = '';\n" +
                    "}", true);
                ClientScript.RegisterStartupScript(GetType(), "CallSetScrollPosition", "fnGetScroll();", true);
            }
        }

        //Rebuild dynamic controls
        private void RebuildDynamicControls()
        {
            //Rebuild the placeholder control contents
            //[Note: plcTranslations population code is also present in Listview Item databound event]
            foreach (ListViewItem dItem in this.ListViewSearchResults.Items)
            {
                if (dItem.ItemType == ListViewItemType.DataItem)
                {
                    //ListViewDataItem item = (ListViewDataItem)dItem;
                    //Product ProductItem = (Product)item.DataItem;
                    HiddenField PubId = (HiddenField)dItem.FindControl("PubId");
                    HiddenField ProdId = (HiddenField)dItem.FindControl("ProdId"); //HITT 11476

                    //Begin CR-30: HITT 8719 - Pub Translations
                    PlaceHolder plcTranslations = (PlaceHolder)dItem.FindControl("plcTranslations");
                    int counter = 0;
                    try
                    {
                        int _tempPubId = Int32.Parse(PubId.Value);
                        string _tempProdId = ProdId.Value; //HITT 11476
                        Panel linksPanel = new Panel();
                        foreach (Product tranlatedproduct in DAL.DAL.GetProductTranslations(_tempPubId))
                        {
                            HyperLink TranslationsLink = new HyperLink();
                            TranslationsLink.CssClass = "linkProdTitle";
                            TranslationsLink.Text = tranlatedproduct.Language;
                            //HITT 11476 TranslationsLink.NavigateUrl = "detail.aspx?prodid=" + tranlatedproduct.ProductId;
                            TranslationsLink.NavigateUrl = "detail.aspx?prodid=" + tranlatedproduct.ProductId + "&mnprodid=" + _tempProdId; //HITT 11476
                            TranslationsLink.Attributes.Add("onclick", "fnSetScroll()"); //HITT 11476
                            Label spaceLabel = new Label();
                            if (counter > 0 && counter % 3 == 0)
                                spaceLabel.Text = "<br/>";
                            else
                                spaceLabel.Text = "&nbsp;";
                            linksPanel.Controls.Add(TranslationsLink);
                            linksPanel.Controls.Add(spaceLabel);
                            TranslationsLink = null;
                            spaceLabel = null;
                            counter++;
                        }
                        if (counter > 0)
                        {
                            Label Translations = new Label();
                            Translations.CssClass = "textDefault";
                            //HITT 9914 Translations.Text = "Translations: ";
                            Translations.Text = "Also Available In: ";
                            linksPanel.Controls.AddAt(0, Translations);
                            Translations = null;
                            plcTranslations.Controls.Add(linksPanel);
                        }
                        linksPanel = null;
                    }
                    catch (Exception Ex)
                    {
                        LogEntry logEnt = new LogEntry();
                        string logmessage = "\r\n";
                        logmessage += "Translation Pubs Error on newupdated: " + "\r\n";
                        logmessage += Ex.Message + Ex.StackTrace;
                        logEnt.Message = logmessage;
                        Logger.Write(logEnt, "Logs");
                    }
                    //End CR-30
                }

            }
        }


        /// Pre-Render event handling added to implement HITT 10265 for search results
        protected void ListViewSearchResults_PreRender(object sender, EventArgs e)
        {
            bool boolHasOrderPub = false;
            bool boolHasOrderCoverPub = false;
            
            //Loop and find out whether there are visible Order or Cover Order buttons
            for (int i = 0; i < this.ListViewSearchResults.Controls[0].Controls.Count; i++ ) //Count: 3
            {
                string id = this.ListViewSearchResults.Controls[0].Controls[i].ID;
                if (id != null)
                {
                    ControlCollection cColl = this.ListViewSearchResults.Controls[0].Controls[i].Controls; //Count: 5
                    foreach (Control childControl in cColl)
                    {
                        string childid = childControl.ID;
                        if (childid != null)
                        {   
                            ControlCollection cChildColl = childControl.Controls;  //Count: 19
                            foreach (Control childchild in cChildColl)
                            {
                                string childchildid = childchild.ID;
                                if (childchildid != null)
                                {
                                    if (childchildid == "OrderPublication")
                                    {
                                        ImageButton imgbtn = (ImageButton)childchild;
                                        if (imgbtn.Visible == true && boolHasOrderPub == false) //need to enter only once
                                            boolHasOrderPub = true;
                                    }
                                    if (childchildid == "OrderCover")
                                    {
                                        ImageButton imgbtn = (ImageButton)childchild;
                                        if (imgbtn.Visible == true && boolHasOrderCoverPub == false) //need to enter only once
                                            boolHasOrderCoverPub = true;
                                    }
                                }
                            }
                        }
                    }
                }

            }

            //if no Order button is present do not render Pub Pop-up div
            if (boolHasOrderPub)
            {
                HiddenPopUpButton.Visible = true;
                PubOrderPanel.Visible = true;
            }
            else
            {
                HiddenPopUpButton.Visible = false;
                PubOrderPanel.Visible = false;
            }

            //if no Cover Order button is present do not render Pub Cover Pop-up div
            if (boolHasOrderCoverPub)
            {
                HiddenCoverPopUpButton.Visible = true;
                PubCoverOrderPanel.Visible = true;
            }
            else
            {
                HiddenCoverPopUpButton.Visible = false;
                PubCoverOrderPanel.Visible = false;
            }

        }

    }
}
