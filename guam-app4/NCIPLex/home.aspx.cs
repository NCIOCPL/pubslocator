using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using NCIPLex.DAL;
using NCIPLex.BLL;
using NCIPLex.usercontrols;
using System.Configuration;
using NCIPLex.GlobalUtils;

namespace NCIPLex
{
    public partial class home : System.Web.UI.Page
    {

        #region Delegate code for usercontrol
        protected override void OnInit(EventArgs e)
        {
            searchbar.SearchButtonClick += new EventHandler(SearchBar_btnSearchClick);
            base.OnInit(e);
        }
        private void SearchBar_btnSearchClick(object sender, EventArgs e)
        {   
            //This event does not get executed
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Utils.ValidateRedirect().Length > 0) //Important check
                Response.Redirect(Utils.ValidateRedirect(), true);
            
            //Moving this to default page
            //if (Session["JSTurnedOn"] == null)
            //    Session["JSTurnedOn"] = "True"; //Assuming JavaScript is enabled, by default.

            ////Missing Session
            //if (Session["JSTurnedOn"] == null)
            //    //Response.Redirect("default.aspx?missingsession=true", true);
            //    Response.Redirect("conf.aspx?missingjs=true", true);
            
            //For Hailstorm check length
            if (this.searchbar.Terms.Length > 100) //Using a hundred limit for search contains sp
                Response.Redirect("default.aspx", true);

            if (!Page.IsPostBack)
            {
                if (Request.QueryString["js"] != null) //Test for JavaScript
                    if (string.Compare(Request.QueryString["js"].ToString(), "2") == 0)
                        Session["JSTurnedOn"] = "False";

                ////***EAC Create the session variables asap
                //Session["NCIPL_Pubs"] = "";
                //Session["NCIPL_Qtys"] = "";
                //Session["PUBENT_SearchKeyword"] = "";
                //Session["PUBENT_TypeOfCancer"] = "";
                //Session["PUBENT_Subject"] = "";
                //Session["PUBENT_Audience"] = "";
                //Session["PUBENT_ProductFormat"] = "";
                //Session["PUBENT_Language"] = "";
                //Session["PUBENT_StartsWith"] = "";
                //Session["PUBENT_Series"] = ""; //Or collection
                //Session["PUBENT_NewOrUpdated"] = "";
                //Session["PUBENT_Race"] = "";


                //Added the session values clearing (other than pubids qtys) back for HITT 7432
                Session["NCIPLEX_SearchKeyword"] = "";
                Session["NCIPLEX_TypeOfCancer"] = "";
                Session["NCIPLEX_Subject"] = "";
                Session["NCIPLEX_Audience"] = "";
                Session["NCIPLEX_ProductFormat"] = "";
                Session["NCIPLEX_Language"] = "";
                Session["NCIPLEX_StartsWith"] = "";
                Session["NCIPLEX_Series"] = ""; //Or collection
                Session["NCIPLEX_NewOrUpdated"] = "";
                Session["NCIPLEX_Race"] = "";
                Session["NCIPLEX_Criteria"] = "";


                ListCancerTypes.DataSource = KVPair.GetKVPair("sp_NCIPLex_getCancerTypes");
                ListCancerTypes.DataBind();
                ListSubjs.DataSource = KVPair.GetKVPair("sp_NCIPLex_getSubjects");
                ListSubjs.DataBind();
                ListProductFormat.DataSource = KVPair.GetKVPair("sp_NCIPLex_getProductFormats");
                ListProductFormat.DataBind();
                ListCollections.DataSource = KVPair.GetKVPair("sp_NCIPLex_getCollections");
                ListCollections.DataBind();
                ListProductUpdates.DataSource = KVPair.GetKVPair("sp_NCIPLex_getProductUpdates");
                ListProductUpdates.DataBind();
                ListLanguages.DataSource = KVPair.GetKVPair("sp_NCIPLex_getLanguages");
                ListLanguages.DataBind();
                ListAudience.DataSource = KVPair.GetKVPair("sp_NCIPLex_getAudience");
                ListAudience.DataBind();
                ListRace.DataSource = KVPair.GetKVPair("sp_NCIPLex_getRace");
                ListRace.DataBind();

                ////ListAnnouncements.DataSource = KVPair.GetKVPair("sp_NCIPL_getAnnouncements");
                ////ListAnnouncements.DataBind();
                //ListAnnouncements.DataSource = Announcement.GetAnnouncements();
                //ListAnnouncements.DataBind();
                //if (ListAnnouncements.Items.Count == 0)
                //    divAnnouncements.Visible = false;
                ////ListFeatures.DataSource = KVPair.GetKVPair("sp_NCIPL_getFeatures");
                ////ListFeatures.DataBind();
                //ProductCollection p = DAL.DAL.GetProductFeatures();
                //ListFeatures.DataSource = p;
                //ListFeatures.DataBind();
            }
            else //HITT 7426 - The else code is added to handle enter key pressed in the text box
            {
                
                //txtSearch.Text = Session["PUBENT_SearchKeyword"].ToString();
                Session["NCIPLEX_SearchKeyword"] = this.searchbar.Terms;

                Session["NCIPLEX_TypeOfCancer"] = "";
                Session["NCIPLEX_Subject"] = "";
                Session["NCIPLEX_Audience"] = "";
                Session["NCIPLEX_Language"] = "";
                Session["NCIPLEX_ProductFormat"] = "";
                Session["NCIPLEX_StartsWith"] = "";
                Session["NCIPLEX_Series"] = "";
                Session["NCIPLEX_NewOrUpdated"] = "";
                Session["NCIPLEX_Race"] = "";

                Session["NCIPLEX_Criteria"] = Session["NCIPLEX_SearchKeyword"];

                /*Begin CR-31 - HITT 9815 */
                GlobalUtils.Utils objUtils = new GlobalUtils.Utils();
                string QueryParams = objUtils.GetQueryStringParams();
                objUtils = null;
                /*End CR-31 - HITT 9815 */

                //CR-31 HITT 9815 Response.Redirect("searchres.aspx");
                Response.Redirect("searchres.aspx" + "?sid=" + QueryParams);
            }


            //Set the appropriate tab
            //int intTotalQty = 0;
            //if (Session["NCIPL_Qtys"] != null)
            //{
            //    string[] qtys = Session["NCIPL_Qtys"].ToString().Split(new Char[] { ',' });
            //    for (int i = 0; i < qtys.Length; i++)
            //    {
            //        if (qtys[i].Length > 0)
            //            intTotalQty += Int32.Parse(qtys[i].ToString());
            //    }
            //}
            
            //string litText1 = @"<li id=""selected""><a href=""home.aspx"">Home</a></li>";
            //string litText2 = @"<li><a href=""self.aspx"">Self-Printing Options</a></li>";
            //string litText3 = @"<li><a href=""cart.aspx"">" + "Shopping Cart (" + intTotalQty.ToString() + ") </a></li>";
            //Master.LiteralText = "<ul>" + litText1 + litText2 + litText3 + "</ul>";

            //Begin - Code for Appropriate Tabs
            GlobalUtils.Utils UtilMethod = new GlobalUtils.Utils();
            if (Session["NCIPLEX_Pubs"] != null)
                Master.LiteralText = UtilMethod.GetTabHtmlMarkUp(Session["NCIPLEX_Qtys"].ToString(), "home");
            else
                Master.LiteralText = UtilMethod.GetTabHtmlMarkUp("", "home");
            UtilMethod = null;
            //End Code for Tab

        }
        protected void ListCancerTypes_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                KVPair kv = (KVPair)e.Item.DataItem;
                HyperLink lnkCanType = (HyperLink)e.Item.FindControl("lnkCanType");
                lnkCanType.Text = kv.Val;
                lnkCanType.NavigateUrl = "search.aspx?cantype=" + kv.Key;
            }
        }
        protected void ListSubjs_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                KVPair kv = (KVPair)e.Item.DataItem;
                Label lblSubj = (Label)e.Item.FindControl("lblSubj");
                lblSubj.Text = kv.Val;

                HyperLink lnkSubj = (HyperLink)e.Item.FindControl("lnkSubj");
                lnkSubj.Text = kv.Val;
                lnkSubj.NavigateUrl = "search.aspx?subj=" + kv.Key;
            }
        }
        protected void ListLanguages_IDB(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                KVPair kv = (KVPair)e.Item.DataItem;

                HyperLink lnkLanguage = (HyperLink)e.Item.FindControl("lnkLanguage");
                lnkLanguage.Text = kv.Val;
                lnkLanguage.NavigateUrl = "search.aspx?lang=" + kv.Key;
            }
        }
        protected void ListProductFormat_IDB(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                KVPair kv = (KVPair)e.Item.DataItem;
                Label lblProductFormat = (Label)e.Item.FindControl("lblProductFormat");
                lblProductFormat.Text = kv.Val;

                HyperLink lnkProductFormat = (HyperLink)e.Item.FindControl("lnkProductFormat");
                lnkProductFormat.Text = kv.Val;
                lnkProductFormat.NavigateUrl = "search.aspx?form=" + kv.Key;
            }

        }
        protected void ListProductUpdates_IDB(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                KVPair kv = (KVPair)e.Item.DataItem;

                HyperLink lnkPU = (HyperLink)e.Item.FindControl("lnkPU");
                lnkPU.Text = kv.Val;
                lnkPU.NavigateUrl = "search.aspx?newupt=" + kv.Key;

                Image imgnewupdated = (Image)e.Item.FindControl("imgnewupdated");
                //imgnewupdated.AlternateText = lnkPU.Text;
                imgnewupdated.AlternateText = "New & Updated";
            }

        }
        protected void ListCollections_IDB(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                KVPair kv = (KVPair)e.Item.DataItem;

                HyperLink lnkCollection = (HyperLink)e.Item.FindControl("lnkCollection");
                lnkCollection.Text = kv.Val;
                lnkCollection.NavigateUrl = "search.aspx?coll=" + kv.Key;
            }
        }
        protected void ListAudience_IDB(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                KVPair kv = (KVPair)e.Item.DataItem;

                HyperLink lnkAudience = (HyperLink)e.Item.FindControl("lnkAudience");
                lnkAudience.Text = kv.Val;
                lnkAudience.NavigateUrl = "search.aspx?aud=" + kv.Key;
            }
        }
        
        protected void ListRace_IDB(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                KVPair kv = (KVPair)e.Item.DataItem;

                HyperLink lnkRace = (HyperLink)e.Item.FindControl("lnkRace");
                lnkRace.Text = kv.Val;
                lnkRace.NavigateUrl = "search.aspx?race=" + kv.Key;
            }
        }
        
        //protected void ListAnn_IDB(object sender, DataListItemEventArgs e)
        //{
        //    if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        //    {
        //        //KVPair kv = (KVPair)e.Item.DataItem;
        //        //Label lblAnn = (Label)e.Item.FindControl("lblAnn");
        //        //HyperLink lnkAnn = (HyperLink)e.Item.FindControl("lnkAnn");

        //        //lnkAnn.Text = kv.Val;
        //        //lnkAnn.NavigateUrl = kv.Val;
        //        //lblAnn.Text = "hardcodedtext";


        //        Announcement annc = (Announcement)e.Item.DataItem;
        //        HyperLink linkAnn = (HyperLink)e.Item.FindControl("lnkAnn");
        //        //HyperLink linkAnnYear = (HyperLink)e.Item.FindControl("lnkAnnYear");
        //        Label lblAnnYear = (Label)e.Item.FindControl("lblAnnYear");

        //        linkAnn.Text = annc.AnnDesc;
        //        linkAnn.NavigateUrl = annc.AnnUrl;
        //        //linkAnnYear.Text = annc.AnnYear;
        //        lblAnnYear.Text = annc.AnnYear;
        //        //linkAnnYear.NavigateUrl = annc.AnnUrl;
        //    }
        //}
        
        //protected void ListFeatures_IDB(object sender, DataListItemEventArgs e)
        //{
        //    if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        //    {
        //        //KVPair kv = (KVPair)e.Item.DataItem;
        //        //Label lblFeat = (Label)e.Item.FindControl("lblFeat");
        //        //HyperLink lnkFeat = (HyperLink)e.Item.FindControl("lnkFeat");
        //        //System.Web.UI.WebControls.Image imgFeat = (System.Web.UI.WebControls.Image)e.Item.FindControl("imgFeat");


        //        //lblFeat.Visible = false;

        //        //imgFeat.ImageUrl = "pubimages/" + kv.Val;
        //        //lnkFeat.NavigateUrl = kv.Val + kv.Val + kv.Val;

        //        Product ProductItem = (Product)e.Item.DataItem;
        //        //CR 28 Image imgFeat = (Image)e.Item.FindControl("imgFeat");
        //        HyperLink lnkFeat = (HyperLink)e.Item.FindControl("lnkFeat");
        //        //HyperLink lnkFeatYear = (HyperLink)e.Item.FindControl("lnkFeatYear");
        //        //CR 28 Label lblFeatYear = (Label)e.Item.FindControl("lblFeatYear");

        //        //CR 28 string imagepath = ConfigurationManager.AppSettings["PubImagesURL"];
        //        string imagepath = ConfigurationManager.AppSettings["PubFeaturedImagesURL"];
        //        //CR 28 imgFeat.ImageUrl = imagepath + "/" + ProductItem.PubFeaturedImage;
        //        //CR 28 imgFeat.AlternateText = ProductItem.ShortTitle;

        //        //CR 28 lnkFeat.Text = ProductItem.LongTitle;
        //        lnkFeat.Text = ProductItem.ShortTitle;
        //        lnkFeat.ImageUrl = imagepath + "/" + ProductItem.PubFeaturedImage; ;
                
        //        lnkFeat.NavigateUrl = "detail.aspx?prodid=" + ProductItem.ProductId;
        //        //lnkFeatYear.Text = ProductItem.RevisedYear;
        //        //lnkFeatYear.NavigateUrl = "detail.aspx?prodid=" + ProductItem.ProductId;
        //        //CR 28 lblFeatYear.Text = ProductItem.RevisedYear;

                    
        //    }
        //}
    }
}
