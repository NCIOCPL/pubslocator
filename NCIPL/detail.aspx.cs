using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PubEnt.BLL;
using PubEnt.DAL;
using System.Configuration;

//using System.Data.SqlClient;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace PubEnt
{
    public partial class detail : System.Web.UI.Page
    {
        private int _pubid;
        private string _prodid;

        /// <summary>
        /// HtmlLink
        /// </summary>
        private string _varHtmlLink;
        public string varHtmlLink
        {
            get { return _varHtmlLink; }
            set { _varHtmlLink = value; }
        }

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
            
            /*Begin CR-31 - HITT 9815 */
            GlobalUtils.Utils objUtils = new GlobalUtils.Utils();
            string QueryParams = objUtils.GetQueryStringParams();
            objUtils = null;
            /*End CR-31 - HITT 9815 */

            //CR-31 HITT 9815 Response.Redirect("searchres.aspx");
            Response.Redirect("searchres.aspx" + "?sid=" + QueryParams);
        }
        #endregion
        private void Page_Load(object sender, System.EventArgs e)
		{
            //Missing Session
            //if (Session["JSTurnedOn"] == null)
            //    Response.Redirect("default.aspx?missingsession=true", true);

            //Just for the details page--allow users to come in directly
            if (Session["JSTurnedOn"] == null)
            {
                Session["JSTurnedOn"] = "True"; //Assuming JavaScript is enabled, by default.
                if (Session["NCIPL_Pubs"] == null)
                    Session["NCIPL_Pubs"] = "";
                if (Session["NCIPL_Qtys"] == null)
                    Session["NCIPL_Qtys"] = "";
                if (Session["PUBENT_SearchKeyword"] == null)
                    Session["PUBENT_SearchKeyword"] = "";
                if (Session["PUBENT_TypeOfCancer"] == null)
                    Session["PUBENT_TypeOfCancer"] = "";
                if (Session["PUBENT_Subject"] == null)
                    Session["PUBENT_Subject"] = "";
                if (Session["PUBENT_Audience"] == null)
                    Session["PUBENT_Audience"] = "";
                if (Session["PUBENT_ProductFormat"] == null)
                    Session["PUBENT_ProductFormat"] = "";
                if (Session["PUBENT_Language"] == null)
                    Session["PUBENT_Language"] = "";
                if (Session["PUBENT_StartsWith"] == null)
                    Session["PUBENT_StartsWith"] = "";
                if (Session["PUBENT_Series"] == null)
                    Session["PUBENT_Series"] = ""; //Or collection
                if (Session["PUBENT_NewOrUpdated"] == null)
                    Session["PUBENT_NewOrUpdated"] = "";
                if (Session["PUBENT_Race"] == null)
                    Session["PUBENT_Race"] = "";
                if (Session["PUBENT_CannedSearch"] == null)
                    Session["PUBENT_CannedSearch"] = "";
            }
            //End of code

            //Some more checks -- for HailStorm
            if (QuantityOrdered.Text.Length > 4)
                Response.Redirect("default.aspx?redirect=detail1", true);
            else if (PubQtyLimit.Text.Length > 5)
                Response.Redirect("default.aspx?redirect=detail2", true);
            else if (QuantityOrderedCover.Text.Length > 4)
                Response.Redirect("default.aspx?redirect=detail3", true);
            else if (CoverQtyLimit.Text.Length > 5)
                Response.Redirect("default.aspx?redirect=detail4", true);
            //End of HailStorm checks
            
            //Other important checks - redirect to default page
            if (Request.QueryString["prodid"] == null)
                Response.Redirect("default.aspx?redirect=detail", true);
            else if (Request.QueryString["prodid"].Length > 10)
                Response.Redirect("default.aspx?redirect=detail", true);

            if (!Page.IsPostBack)
            {
                //***EAC decided to use product ID instead on PUBID to dicourage users from guessing this param
                if (Request.QueryString["prodid"] != null)
                {
                    GlobalUtils.Utils UtilMethodClean = new GlobalUtils.Utils();
                    _prodid = UtilMethodClean.Clean(Request.QueryString["prodid"].ToString());
                    UtilMethodClean = null;

                    ProductCollection pColl = DAL.DAL.GetProductbyProductIDWithRules(_prodid);
                    //Product p = Product.GetPubByProductID(Request.QueryString["prodid"].ToString());
                    
                    //Checking for a valid Product Id
                    if (pColl == null)
                        Response.Redirect("default.aspx?redirect=detail", true);
                    else if (pColl.Pubs.Length == 0)
                        Response.Redirect("default.aspx?redirect=detail", true);

                    Product p = pColl[0];
                                
                    lblTitle.Text = p.LongTitle;
                    lblTitlesm.Text = p.LongTitle;
                    lblFormat.Text = p.Format;
                    lblFormatsm.Text = p.Format;
                    if (lblFormat.Text.Length <= 0)
                    {
                        lblFormatText.Visible = false; lblFormatTextsm.Visible = false;
                    }
                    lblNumPages.Text = p.NumPages;
                    if (lblNumPages.Text.Length <= 0)
                        lblNumPagesText.Visible = false;
                    //Image1.ImageUrl = "pubimages/" + p.PubImage;
                    lblAud.Text = p.Audience;
                    lblAudsm.Text = p.Audience;
                    if (lblAud.Text.Length <= 0)
                    { lblAudText.Visible = false; lblAudTextsm.Visible = false; }
                    lblLang.Text = p.Language;
                    if (lblLang.Text.Length <= 0)
                        lblLangText.Visible = false;
                    //lblSumm.Text = p.Summary;
                    lblDesc.Text = p.Abstract;
                    lblDescsm.Text = p.Abstract;
                    //if (lblDesc.Text.Length <= 0)
                        //lblDescText.Visible = false; //LSK no label for description


                    string LastUpdatedMon = "";
                    string LastUpdatedDay = "";
                    string LastUpdatedYear = "";
                    string LastUpdatedText = "";

                    LastUpdatedMon = p.RevisedMonth;
                    LastUpdatedDay = p.RevisedDay;
                    LastUpdatedYear = p.RevisedYear;

                    if (LastUpdatedMon.Length > 0 && LastUpdatedDay.Length > 0 && LastUpdatedYear.Length > 0)
                        LastUpdatedText = LastUpdatedMon + " " + LastUpdatedDay + ", " + LastUpdatedYear;
                    else if (LastUpdatedMon.Length > 0 && LastUpdatedYear.Length > 0)
                        LastUpdatedText = LastUpdatedMon + " " + LastUpdatedYear;
                    else if (LastUpdatedYear.Length > 0)
                        LastUpdatedText = LastUpdatedYear;
                    else
                        LastUpdatedText = "";

                    lblLastupd.Text = LastUpdatedText;
                    //lblLastupdText.Text = p.RevisedDateType;
                    lblLastupdText.Text = p.RevisedDateType.ToLower();
                    
                    if (lblLastupd.Text.Length <= 0)
                        lblLastupdText.Visible = false;
                    lblProductID.Text = p.ProductId;
                    lblProductIDsm.Text = p.ProductId;
                    if (lblProductID.Text.Length <= 0)
                    {
                        lblProductIDText.Visible = false; lblProductIDTextsm.Visible = false;
                    }
                    lblOrderLimit.Text = p.NumQtyLimit.ToString(); //HITT 8288
                    if (p.CanOrder == 0) //HITT 8288
                    {
                        lblOrderLimit.Visible = false;
                        lblOrderLimitText.Visible = false;
                    }
                    lblNIH.Text = p.NIHNum;
                    lblNIHsm.Text = p.NIHNum;
                    if (lblNIH.Text.Length <= 0)
                    {
                        lblNIHText.Visible = false; lblNIHTextsm.Visible = false;
                    }
                    //if (lblNIHText.Visible == true) //CR-10 HITT 9456
                    //{
                    //    if (p.NIHNum.Length > 7)
                    //        lblNIHText.Text = "NIH Number(s):&nbsp;";
                    //}  
                    lblAwards.Text = p.Awards;
                    lblAwardssm.Text = p.Awards;
                    //if (lblAwards.Text.Length > 0)
                    //    lblAwards.Text = p.Awards;
                        //lblAwardsText.Visible = false; //LSK no longer show awards label
                    if (p.PubImage.Length > 0)
                    {
                        panelThumbImg.Visible = true;
                        string imagepath = ConfigurationManager.AppSettings["PubImagesURL"];
                        //Image1.ImageUrl = "pubimages/" + p.PubImage;
                        Image1.ImageUrl = imagepath + "/" + p.PubImage;
                        Image1.AlternateText = p.ShortTitle;
                        //Image3.AlternateText = p.ShortTitle;
                        if (p.PubLargeImage.Length > 0)
                        {
                            panelThumbImg.Visible = false; 
                            panelLargeImg.Visible = true;
                            string lgimagepath = ConfigurationManager.AppSettings["PubLargeImagesURL"];
                            Image2.AlternateText = p.ShortTitle;
                            Image2.ImageUrl = lgimagepath + "/" + p.PubLargeImage;
                            //Image3.AlternateText = p.ShortTitle;
                            //Image3.ImageUrl = lgimagepath + "/" + p.PubLargeImage;
                            //MagnifierLink.Visible = true;
                            //MagnifierLink.NavigateUrl = "dispimage.aspx?prodid=" + p.ProductId;
                        }
                    }
                    
                   #region PATCH for Omniture order buttons (20130221)
                   if (Session["NCIPL_Pubs"] == null || Session["NCIPL_Pubs"].ToString() == "")
                   {
                        OrderPublication.Attributes.Add("prodid", p.ProductId);
                        OrderPublication.Attributes.Add("onclick", "NCIAnalytics.CartStarted(this.getAttribute('prodid'));");
                        OrderCover.Attributes.Add("prodid", p.ProductId + "C");
                        OrderCover.Attributes.Add("onclick", "NCIAnalytics.CartStarted(this.getAttribute('prodid'));");
                   }
                   #endregion

                    OrderPublication.CommandArgument = p.PubId.ToString();
                    OrderCover.CommandArgument = p.PubIdCover;

                    if (p.CanView == 1)
                    {
                        //ReadOnlineLink.Visible = true;
                        //ReadOnlineLink.Target = "_blank";
                        //ReadOnlineLink.NavigateUrl = p.Url;

                        if (p.Url.Length > 0)
                        {
                            //textbreaker1.Visible = true;
                            HtmlLink.Visible = true;
                            HtmlLink.NavigateUrl = p.Url;
                            varHtmlLink = p.Url;
                            HtmlLinksm.Visible = true;
                            HtmlLinksm.NavigateUrl = p.Url;
                        }

                        if (p.PDFUrl.Length > 0)
                        {
                            //textbreaker2.Visible = true;
                            PdfLink.Visible = true;
                            PdfLink.NavigateUrl = p.PDFUrl;
                            PdfLinksm.Visible = true;
                            PdfLinksm.NavigateUrl = p.PDFUrl;
                        }
                    }
                    #region SHOW EBOOK URLS IF ANY
                    string kindle = DAL2.DAL.EbookUrl(p.PubId, "kindle");
                    if (kindle.Length > 0)
                    {
                        KindleLink.Visible = true;
                        KindleLink.NavigateUrl = kindle;
                        KindleLinksm.Visible = true;
                        KindleLinksm.NavigateUrl = kindle;
                    }

                    string epub = DAL2.DAL.EbookUrl(p.PubId, "epub");
                    if (epub.Length > 0)
                    {
                        EpubLink.Visible = true;
                        EpubLink.NavigateUrl = epub;
                        EpubLinksm.Visible = true;
                        EpubLinksm.NavigateUrl = epub;
                    }
                    #endregion 

                    if (p.OrderMsg.Length > 0)
                    {
                        //HITT 7445 labelOrderMsg.Visible = true;
                        labelOrderMsg.Text = p.OrderMsg;
                    }

                    if (p.CanOrder == 1)
                    {
                        OrderPublication.Visible = true;
                    }
                    else //HITT 10265
                    {
                        HiddenPopUpButton.Visible = false;
                        PubOrderPanel.Visible = false;
                    }

                    if (p.CoverMsg.Length > 0)
                    {
                        //HITT 7445 labelCoverMsg.Visible = true;
                        labelCoverMsg.Text = p.CoverMsg;
                    }

                    if (p.CanOrderCover == 1)
                    {
                        OrderCover.Visible = true;
                    }
                    else //HITT 10265
                    {
                        HiddenCoverPopUpButton.Visible = false;
                        PubCoverOrderPanel.Visible = false;
                    }

                    //Check whether this pub is already in cart
                    if (!IsItemInCart(p.PubId.ToString()))
                    {
                        OrderPublication.CommandArgument = p.PubId.ToString();
                        
                        //OrderPublication.ImageUrl = "images/order_off.gif";
                        //OrderPublication.AlternateText = "Order Publication";
                        //OrderPublication.AlternateText = "Order";
                        OrderPublication.Text = "Order Publication";
                    }
                    else
                    {
                        OrderPublication.CommandArgument = "";
                        //OrderPublication.ImageUrl = "images/PublicationInYourCart_off.gif";
                        //OrderPublication.AlternateText = "Publication - In Your Cart";
                        OrderPublication.Text = "Publication - In Your Cart";
                        OrderPublication.CssClass = "btn";
                    }

                    //Check whether this pub is already in cart
                    if (!IsItemInCart(p.PubIdCover.ToString()))
                    {
                        OrderCover.CommandArgument = p.PubIdCover.ToString();
                       
                        //OrderCover.ImageUrl = "images/ordercovers_off.gif";
                        //CR 28 OrderCover.AlternateText = "Order Covers Only";
                        //OrderCover.AlternateText = "Order Covers";
                        OrderCover.Text = "Order Covers";
                    }
                    else
                    {
                        OrderCover.CommandArgument = "";
                        //OrderCover.ImageUrl = "images/CoverOnlyInYourCart_off.gif";
                        //OrderCover.AlternateText = "Covers Only - In Your Cart";
                        OrderCover.Text = "Covers Only - In Your Cart";
                        OrderCover.CssClass = "btn";
                    }

                    //_pubid = DAL.DAL.GetPubIdFromProductId(Request.QueryString["prodid"]);
                    //_pubid = DAL.DAL.GetPubIdFromProductId(_prodid);
                    _pubid = p.PubId;


                    //Code for physical description
                    string Dimension = "";
                    string Color = "";
                    string Other = "";
                    //Get the physical description of the product from the database
                    IDataReader dr = DAL.DAL.GetPubPhysicalDesc(_pubid);
                    try
                    {
                        using (dr)
                        {
                            while (dr.Read())
                            {
                                Dimension = dr["DIMENSION"].ToString();
                                Color = dr["COLOR"].ToString();
                                Other = dr["OTHER"].ToString();
                            }
                        }
                    }
                    catch (Exception Ex)
                    {
                        //TO DO: log any error
                        if (!dr.IsClosed)
                            dr.Close();
                    }
                   
                    //Assigning to label
                    string phydesc = "";
                    if (Dimension.Length > 0)
                        phydesc += Dimension + "; ";
                    if (Color.Length > 0)
                        phydesc += Color + "; ";
                    if (Other.Length > 0)
                        phydesc += Other + "; ";

                    char[] temparr = { ';', ' ' };

                    if (phydesc.Length > 2)
                        phydesc = phydesc.TrimEnd(temparr);

                    lblPhysicalDesc.Text = phydesc;

                    //if (lblPhysicalDesc.Text.Length <= 0)
                        //lblPhysicalDescText.Visible = false; //LSK label for physdesc no longer shown
                    //End code for physical description

                    //Get Collections that this pub belongs to
                    try
                    {
                        grdViewPubCollections.DataSource = DAL.DAL.GetPubCollections(_pubid);
                        grdViewPubCollections.DataBind();
                    }
                    catch (Exception Ex)
                    {
                        //Do nothing
                    }
                    if (grdViewPubCollections.Rows.Count > 0)
                    {
                        //Display singular label
                        lblPubCollections.Text = "part of the collection:&nbsp;";
                        if (grdViewPubCollections.Rows.Count > 1)
                        {
                            //Display plural label
                            lblPubCollections.Text = "part of the collections:&nbsp;";
                        }
                    }
                    else
                        lblPubCollections.Visible = false;
                    //End code for displaying the publication collection

                    //Begin code for Contents & Cover URL
                    if (p.UrlNerdo.Length > 0 && p.UrlCover.Length > 0)
                    {
                        NerdoContentsLink.Text = "Print Contents";
                        NerdoContentsLink.NavigateUrl = p.UrlNerdo;
                        NerdoCoverLink.Text = "Print Cover";
                        NerdoCoverLink.NavigateUrl = p.UrlCover;
                    }
                    else
                    {
                       /* lblNerdo.Visible = false;*/
                        NerdoContentsLink.Visible = false;
                       /* lblNerdoSpace.Visible = false;*/
                        NerdoCoverLink.Visible = false;
                    }

                    ////Begin CR-30: HITT 8719 - Pub Translations
                    ////PlaceHolder plcTranslations = (PlaceHolder)e.Item.FindControl("plcTranslations");
                    //int counter = 0;
                    //Panel linksPanel = new Panel();
                    //foreach (Product tranlatedproduct in DAL.DAL.GetProductTranslations(p.PubId))
                    //{
                    //    HyperLink TranslationsLink = new HyperLink();
                    //    TranslationsLink.CssClass = "linkDefault";
                    //    TranslationsLink.Text = tranlatedproduct.Language;
                    //    TranslationsLink.NavigateUrl = "detail.aspx?prodid=" + tranlatedproduct.ProductId;
                    //    Label spaceLabel = new Label();
                    //    //if (counter > 0 && counter % 3 == 0)
                    //    //    spaceLabel.Text = "<br/>";
                    //    //else
                    //    spaceLabel.Text = "&nbsp;&nbsp;";
                    //    linksPanel.Controls.Add(TranslationsLink);
                    //    linksPanel.Controls.Add(spaceLabel);
                    //    TranslationsLink = null;
                    //    spaceLabel = null;
                    //    counter++;
                    //}
                    //if (counter > 0)
                    //{
                    //    //Label Translations = new Label();
                    //    //Translations.CssClass = "textDefault";
                    //    //Translations.Text = "Translations: ";
                    //    //linksPanel.Controls.AddAt(0, Translations);
                    //    //Translations = null;
                    //    plcTranslations.Controls.Add(linksPanel);
                    //}
                    //else
                    //    lblTranslations.Visible = false;
                    //linksPanel = null;
                    ////End CR-30

                    //Manage div heights here
                    //pnlTitleLabel.Height = pnlTitleText.Height;
                    //lblDescText.Height = lblDesc.Height;

                    //End of Code

                    //Add controls to the dynamic table here
                    
                    //End code to add controls to dynamic table

                    if (Request.UrlReferrer != null)
                        hdnReferrer.Value = Request.UrlReferrer.ToString();
                    if (Session["JSTurnedOn"] != null && Request.UrlReferrer != null)
                    {   
                        if (string.Compare(Session["JSTurnedOn"].ToString(), "True", true) == 0
                                && (this.Page.Request.UrlReferrer.ToString().Contains("searchres.aspx") || this.Page.Request.UrlReferrer.ToString().Contains("newupdated.aspx")))
                        {   
                                //BackToSearchResultsLink.Visible = true;
                                lnkBtnSearchRes.Visible = true;
                        }
                    }
                }
                else throw new ArgumentException("Missing parameter in detail.aspx", "value");


                PubOrderOK.OnClientClick = String.Format("fnAddItemToCart('{0}','{1}')", PubOrderOK.UniqueID, "");
                PubCoverOrderOK.OnClientClick = String.Format("fnAddItemToCart('{0}','{1}')", PubCoverOrderOK.UniqueID, "");

                //Newly added code to reset textbox when cancel button is clicked on popup
                PubOrderCancel.OnClientClick = String.Format("fnCancelClick('{0}')", QuantityOrdered.ClientID);
                PubCoverOrderCancel.OnClientClick = String.Format("fnCancelClick('{0}')", QuantityOrderedCover.ClientID);
                
                PubOrderLink.Attributes.Add("href", "javascript:fnPostBack(" + "'" + PubOrderOK.UniqueID + "','" + "','" + QuantityOrdered.ClientID + "','" + PubQtyLimit.ClientID + "','" + labelErrMsgPubOrder.ClientID + "','" + PubOrderModalPopup.BehaviorID + "')");
                PubCoverOrderLink.Attributes.Add("href", "javascript:fnPostBack(" + "'" + PubCoverOrderOK.UniqueID + "','" + "','" + QuantityOrderedCover.ClientID + "','" + CoverQtyLimit.ClientID + "','" + labelErrMsgPubCover.ClientID + "','" + PubCoverOrderModalPopup.BehaviorID + "')");
                
                
                dlistRelatedProducs.DataSource = (ProductCollection)DAL.DAL.GetRelatedProducts(_pubid);
                dlistRelatedProducs.DataBind();

                if (dlistRelatedProducs.Items.Count > 0)
                {
                    //Do not do anything
                }
                else
                    divRelatedProducts.Visible = false;
                
                //Display the master page tabs 
                GlobalUtils.Utils UtilMethod = new GlobalUtils.Utils();
                if (Session["NCIPL_Pubs"] != null)
                    Master.LiteralText = UtilMethod.GetTabHtmlMarkUp(Session["NCIPL_Qtys"].ToString(), "");
                else
                    Master.LiteralText = UtilMethod.GetTabHtmlMarkUp("", "");
                UtilMethod = null;
            }

            #region Rebuild Dynamic Controls
            //Begin HITT - 9846 - Dynamic Controls need to re-initalized each time when the page posts back
            try 
            {
                //Begin CR-30: HITT 8719 - Pub Translations
                _pubid = DAL.DAL.GetPubIdFromProductId(Request.QueryString["prodid"]);
                int counter = 0;
                Panel linksPanel = new Panel();
                foreach (Product tranlatedproduct in DAL.DAL.GetProductTranslations(_pubid))
                {
                    HyperLink TranslationsLink = new HyperLink();
                    TranslationsLink.CssClass = "linkDefault";
                    TranslationsLink.Text = tranlatedproduct.Language;
                    TranslationsLink.NavigateUrl = "detail.aspx?prodid=" + tranlatedproduct.ProductId;
                    Label spaceLabel = new Label();
                
                    spaceLabel.Text = "&nbsp;&nbsp;";
                    linksPanel.Controls.Add(TranslationsLink);
                    linksPanel.Controls.Add(spaceLabel);
                    TranslationsLink = null;
                    spaceLabel = null;
                    counter++;
                }
                if (counter > 0)
                {
                    plcTranslations.Controls.Add(linksPanel);
                }
                else
                    lblTranslations.Visible = false;
                linksPanel = null;
                //End CR-30
            }
            catch (Exception Ex) 
            {
                LogEntry logEnt = new LogEntry();
                string logmessage = "\r\n";
                logmessage += "Translation Pubs Error on detail: " + "\r\n"; 
                logmessage += Ex.Message + Ex.StackTrace;
                logEnt.Message = logmessage;
                Logger.Write(logEnt, "Logs");
            }
            //End HITT - 9846
            #endregion
        }

        protected void dlistRelatedProducs_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Product ProductItem = (Product)e.Item.DataItem;
                Image PubImage = (Image)e.Item.FindControl("PubImage");
                HyperLink DetailLink = (HyperLink)e.Item.FindControl("DetailLink");
                PubImage.ImageUrl = "pubimages/" + ProductItem.PubImage.ToString();
                PubImage.AlternateText = ProductItem.ShortTitle;
                DetailLink.Text = ProductItem.LongTitle;
                DetailLink.NavigateUrl = "detail.aspx?prodid=" + ProductItem.ProductId;
            }
        }
        //BEGIN COPIED CODE#########################################
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
                //BackToSearchResultsLink.NavigateUrl = "javascript:history.go(-2);"; //Do not show once modal pop-up is displayed
            }
            
        }

        protected void DisplayModalPopUpCover(object sender, CommandEventArgs e)
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

                ImageButton OrderedCover = (ImageButton)sender;
                OrderedCover.CommandArgument = "";
                OrderedCover.ImageUrl = "images/CoverOnlyInYourCart_off.gif";
                OrderedCover.AlternateText = "Covers Only - In Your Cart";

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
                this.PubCoverOrderOK.CommandArgument = e.CommandArgument.ToString();

                Product p = DAL.DAL.GetProductbyPubID(Convert.ToInt32(e.CommandArgument));
                labelCoverPubTitle.Text = p.LongTitle;
                CoverQtyLimit.Text = p.NumQtyLimit.ToString(); //p.NumQtyAvailable.ToString();
                CoverLimitLabel.Text = "Pack of 25 covers" + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + "Limit " + CoverQtyLimit.Text;

                //Get the content URL - TO DO, can be optimized later to use a stored procedure 
                //that returns only one value.
                KVPairCollection kvpaircoll = DAL.DAL.GetKVPair("sp_NCIPL_getNerdoPubIdsURLS"); ;
                foreach (KVPair kvpair in kvpaircoll)
                {
                    if (string.Compare(kvpair.Key, p.PubId.ToString()) == 0)
                    {
                        linkCoverPubUrl.NavigateUrl = kvpair.Val;
                        break;
                    }
                }
                
                //Need to call update panel update to populate the values
                UpdatePanelOrderCover.UpdateMode = UpdatePanelUpdateMode.Conditional;
                UpdatePanelOrderCover.Update();

                //Show the Modal Popup
                this.PubCoverOrderModalPopup.Show();
                //BackToSearchResultsLink.NavigateUrl = "javascript:history.go(-2);"; //Do not show once modal pop-up is displayed
            }

        }

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

            int qty = Int32.Parse(QuantityOrdered.Text);
            if (qty > 0)
            {
                if (!IsItemInCart(this.PubOrderOK.CommandArgument.ToString())) //Check for browser re-load
                {
                    Session["NCIPL_Pubs"] += this.PubOrderOK.CommandArgument.ToString() + ",";
                    Session["NCIPL_Qtys"] += qty + ",";
                }

                //Change the searh results list view here to show "In Cart" items
                //foreach (ListViewItem dItem in this.ListViewSearchResults.Items)
                //{
                //    if (dItem.ItemType == ListViewItemType.DataItem)
                //    {
                //        ImageButton OrderedPub = (ImageButton)dItem.FindControl("OrderPublication");
                        if (string.Compare(OrderPublication.CommandArgument, PubOrderOK.CommandArgument, true) == 0)
                        {
                            OrderPublication.CommandArgument = "";
                            //OrderPublication.ImageUrl = "images/PublicationInYourCart_off.gif";
                            //OrderPublication.AlternateText = "Publication - In Your Cart";
                            OrderPublication.Text = "Publication - In Your Cart";
                            OrderPublication.CssClass = "btn";
                        }
                        #region EAC remove omniture code from the 2 buttons(20130221)
                        OrderPublication.Attributes.Remove("onclick"); 
                        OrderCover.Attributes.Remove("onclick");
                        #endregion
                //    }
                //}

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
                //dont do anything for now
            }
            //BackToSearchResultsLink.NavigateUrl = "javascript:history.go(-3);"; //Do not show once modal pop-up is displayed
        }

        protected void PubCoverOrderOK_Click(object sender, EventArgs e)
        {
            if (!IsQtyValueValid(QuantityOrderedCover.Text, CoverQtyLimit.Text))
            {
                labelErrMsgPubCover.Text = "Please enter a valid quantity.";
                UpdatePanelOrderCover.UpdateMode = UpdatePanelUpdateMode.Conditional;
                UpdatePanelOrderCover.Update();
                this.PubCoverOrderModalPopup.Show();
                return;
            }

            int qty = Int32.Parse(QuantityOrderedCover.Text);
            if (qty > 0)
            {
                if (!IsItemInCart(this.PubCoverOrderOK.CommandArgument.ToString())) //Check for browser re-load
                {
                    Session["NCIPL_Pubs"] += this.PubCoverOrderOK.CommandArgument.ToString() + ",";
                    Session["NCIPL_Qtys"] += qty + ",";
                }

                //Change the searh results listview here to show "In Cart" items
                //foreach (ListViewItem dItem in this.ListViewSearchResults.Items)
                //{
                //    if (dItem.ItemType == ListViewItemType.DataItem)
                //    {
                //        ImageButton OrderedCover = (ImageButton)dItem.FindControl("OrderCover");
                        if (string.Compare(OrderCover.CommandArgument, PubCoverOrderOK.CommandArgument, true) == 0)
                        {
                            OrderCover.CommandArgument = "";
                            //OrderCover.ImageUrl = "images/CoverOnlyInYourCart_off.gif";
                            //OrderCover.AlternateText = "Covers Only - In Your Cart";
                            OrderCover.Text = "Covers Only - In Your Cart";
                            OrderCover.CssClass = "btn";
                        }
                        #region EAC remove omniture code from the 2 buttons(20130221)
                        OrderPublication.Attributes.Remove("onclick");
                        OrderCover.Attributes.Remove("onclick");
                        #endregion
                //    }
                //}

                        //Display the master page tabs 
                        GlobalUtils.Utils UtilMethod = new GlobalUtils.Utils();
                        if (Session["NCIPL_Pubs"] != null)
                            Master.LiteralText = UtilMethod.GetTabHtmlMarkUp(Session["NCIPL_Qtys"].ToString(), "");
                        else
                            Master.LiteralText = UtilMethod.GetTabHtmlMarkUp("", "");
                        UtilMethod = null;

                        QuantityOrderedCover.Text = "1"; //Reset through code
                        //BackToSearchResultsLink.Visible = false; //Do not show if pub was orded from details page
            }
            else
            {
                //dont do anything for now
            }
            //BackToSearchResultsLink.NavigateUrl = "javascript:history.go(-3);"; //Do not show once modal pop-up is displayed
        }

        /// <summary>
        /// Check if entered quantity is valid. If the quantity is greater than the limit, less than one, or non-numerical, set to "false".
        /// </summary>
        /// <param name="val"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
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

        //END COPIED CODE###########################################

        protected void grdViewPubCollections_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {  
                System.Data.Common.DbDataRecord rd = (System.Data.Common.DbDataRecord)e.Row.DataItem;
                HyperLink PubCollectionLink = (HyperLink)e.Row.FindControl("PubCollectionLink");
                PubCollectionLink.Text = rd["description"].ToString();
                PubCollectionLink.NavigateUrl = "search.aspx?coll=" + rd["seriesid"].ToString();
                rd = null;
            }
        }

        protected void lnkBtnSearchRes_Click(object sender, EventArgs e)
        {
            Session["CurrentProdId"] = Request.QueryString["prodid"].ToString();
            //Begin HITT 11476
            if (Request.QueryString["mnprodid"] != null) //Coming from searchres.aspx, cannedsearchres.aspx or newupdated.aspx - Translation Link click
            {
                if (Request.QueryString["mnprodid"].ToString().Length > 0 && Request.QueryString["mnprodid"].ToString().Length < 11) //Some checks for security
                    Session["CurrentProdId"] = Request.QueryString["mnprodid"].ToString(); //The current product id should be the main pub id for the back to search results functionality to find the correct record in search results, if coming from a translation link click
            }
            //End HITT 11476
            //if (Session["PUBENT_CannedSearch"] != null && hdnReferrer.Value.Contains("cannedsearchres.aspx"))
            //    Response.Redirect("cannedsearch.aspx",true);

            //if (hdnReferrer.Value.Contains("newupdated.aspx"))
            //    Response.Redirect("search.aspx?newupt=1", true);
            if (hdnReferrer.Value.Contains("canned=1"))
                Response.Redirect("searchres.aspx?canned=1&back2result=1"); //yma add this adding back2result=1 to avoid other link on detail page be affected
            if (hdnReferrer.Value.Contains("newupt=1"))
                Response.Redirect("searchres.aspx?newupt=1&back2result=1"); //yma add this adding back2result=1 to avoid other link on detail page be affected
            #region GenerateSearchResultsURL
            GlobalUtils.Utils objUtils = new GlobalUtils.Utils();
            string QueryParams = objUtils.GetQueryStringParams();
            objUtils = null;
            #endregion
            //Response.Redirect("searchres.aspx" + "?sid=" + QueryParams);
            Response.Redirect("searchres.aspx" + "?sid=" + QueryParams + "&back2result=1"); //yma add this 
        }
    }

}
