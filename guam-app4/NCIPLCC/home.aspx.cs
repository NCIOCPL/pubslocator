using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using PubEnt.DAL;
using PubEnt.BLL;
using PubEnt.usercontrols;
using System.Configuration;

//added for CR-36
using System.Web.Services;
using System.Collections;
using System.Collections.Specialized;

namespace PubEnt
{
    public partial class home : System.Web.UI.Page
    {
        #region Variables
        private string slidedivid = "slides"; //CR-36
        private int slidedivcount = 0; //CR-36
        private ArrayList arrStackIds = new ArrayList(); //CR-36
        private ArrayList arrStackDivIds = new ArrayList(); //CR-36
        private string TurnOffFeaturedImages = "0"; //CR-36
        #endregion

        #region Delegate code for usercontrol
        protected override void OnInit(EventArgs e)
        {
            searchbar.SearchButtonClick += new EventHandler(SearchBar_btnSearchClick);
            base.OnInit(e);
        }
        private void SearchBar_btnSearchClick(object sender, EventArgs e)
        {   
            //***EAC Perform Search
            Session["PUBENT_SearchKeyword"] = this.searchbar.Terms;

            Session["PUBENT_TypeOfCancer"] = "";
            Session["PUBENT_Subject"] = "";
            Session["PUBENT_Audience"] = "";
            Session["PUBENT_Language"] = "";
            Session["PUBENT_ProductFormat"] = "";
            Session["PUBENT_StartsWith"] = "";
            Session["PUBENT_Series"] = "";
            Session["PUBENT_NewOrUpdated"] = "";
            Session["PUBENT_Race"] = "";

            Session["PUBENT_Criteria"] = Session["PUBENT_SearchKeyword"];

            /*Begin CR-31 - HITT 9815 */
            GlobalUtils.Utils objUtils = new GlobalUtils.Utils();
            string QueryParams = objUtils.GetQueryStringParams();
            objUtils = null;
            /*End CR-31 - HITT 9815 */

            //CR-31 HITT 9815 Response.Redirect("searchres.aspx");
            Response.Redirect("searchres.aspx" + "?sid=" + QueryParams);
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            //Check GUAM UserId, Role for NCIPL_CC
            if (GlobalUtils.UserRoles.getLoggedInUserId().Length == 0 || GlobalUtils.UserRoles.getLoggedInUserRole() < 1)
            {
                string currASPXfilename = System.IO.Path.GetFileName(Request.Path).ToString();
                Session["NCIPL_REGISTERREFERRER"] = currASPXfilename;
                Response.Redirect("~/login.aspx?msg=invaliduser&redir=" + currASPXfilename, true);
            }
            
            //Moving this to default page
            //if (Session["JSTurnedOn"] == null)
            //    Session["JSTurnedOn"] = "True"; //Assuming JavaScript is enabled, by default.

            ////Missing Session -- COMMENTED FOR NCIPLCC
            //if (Session["JSTurnedOn"] == null)
            //    Response.Redirect("default.aspx?missingsession=true", true);
            
            //For Hailstorm check length
            if (this.searchbar.Terms.Length > 100) //Using a hundred limit for search contains sp
                Response.Redirect("default.aspx", true);

            if (!Page.IsPostBack)
            {
                if (Request.QueryString["js"] != null) //Test for JavaScript
                    if (string.Compare(Request.QueryString["js"].ToString(), "2") == 0)
                        Session["JSTurnedOn"] = "False";

                //Begin CR-36 - A quick way to turn off Featured Images
                if (ConfigurationManager.AppSettings["TurnOffFeaturedImages"] != null)
                    TurnOffFeaturedImages = ConfigurationManager.AppSettings["TurnOffFeaturedImages"];
                //End CR-36

                ////***EAC Create the session variables asap
                Session["PUBENT_SearchKeyword"] = "";
                Session["PUBENT_TypeOfCancer"] = "";
                Session["PUBENT_Subject"] = "";
                Session["PUBENT_Audience"] = "";
                Session["PUBENT_ProductFormat"] = "";
                Session["PUBENT_Language"] = "";
                Session["PUBENT_StartsWith"] = "";
                Session["PUBENT_Series"] = ""; //Or collection
                Session["PUBENT_NewOrUpdated"] = "";
                Session["PUBENT_Race"] = "";
                Session["PUBENT_Criteria"] = "";


                ListCancerTypes.DataSource = KVPair.GetKVPair("sp_NCIPLCC_getCancerTypes");
                ListCancerTypes.DataBind();
                ListSubjs.DataSource = KVPair.GetKVPair("sp_NCIPLCC_getSubjects");
                ListSubjs.DataBind();
                ListProductFormat.DataSource = KVPair.GetKVPair("sp_NCIPLCC_getProductFormats");
                ListProductFormat.DataBind();
                ListCollections.DataSource = KVPair.GetKVPair("sp_NCIPLCC_getCollections");
                ListCollections.DataBind();
                //CR 11-001-36 ListProductUpdates.DataSource = KVPair.GetKVPair("sp_NCIPL_getProductUpdates");
                //CR 11-001-36 ListProductUpdates.DataBind();
                ListLanguages.DataSource = KVPair.GetKVPair("sp_NCIPLCC_getLanguages");
                ListLanguages.DataBind();
                ListAudience.DataSource = KVPair.GetKVPair("sp_NCIPLCC_getAudience");
                ListAudience.DataBind();
                ListRace.DataSource = KVPair.GetKVPair("sp_NCIPLCC_getRace");
                ListRace.DataBind();

                //ListAnnouncements.DataSource = KVPair.GetKVPair("sp_NCIPL_getAnnouncements");
                //ListAnnouncements.DataBind();
                ListAnnouncements.DataSource = Announcement.GetAnnouncements();
                ListAnnouncements.DataBind();
                if (ListAnnouncements.Items.Count == 0)
                    divAnnouncements.Visible = false;
                //ListFeatures.DataSource = KVPair.GetKVPair("sp_NCIPL_getFeatures");
                //ListFeatures.DataBind();
                //CR-36 ProductCollection p = DAL.DAL.GetProductFeatures();
                //CR-36 ListFeatures.DataSource = p;
                //CR-36 ListFeatures.DataBind();

                #region StackRelated
                //ListFeatures.DataSource = DAL.DAL.GetStacks(); //CR-36
                //ListFeatures.DataBind(); //CR-36
                #endregion

            }

            //Begin - Code for Appropriate Tabs
            GlobalUtils.Utils UtilMethod = new GlobalUtils.Utils();
            if (Session["NCIPL_Pubs"] != null)
                Master.LiteralText = UtilMethod.GetTabHtmlMarkUp(Session["NCIPL_Qtys"].ToString(), "home");
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
        
        protected void ListAnn_IDB(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Announcement annc = (Announcement)e.Item.DataItem;
                HyperLink linkAnn = (HyperLink)e.Item.FindControl("lnkAnn");
                //HyperLink linkAnnYear = (HyperLink)e.Item.FindControl("lnkAnnYear");
                Label lblAnnYear = (Label)e.Item.FindControl("lblAnnYear");

                linkAnn.Text = annc.AnnDesc;
                linkAnn.NavigateUrl = annc.AnnUrl;
                //linkAnnYear.Text = annc.AnnYear;
                lblAnnYear.Text = annc.AnnYear;
                //linkAnnYear.NavigateUrl = annc.AnnUrl;
            }
        }

        #region StackRelated
        
        //protected void ListFeatures_PreRender(object sender, EventArgs e)
        //{   
        //    #region No_JS_Or_FeaturedImagesOff
        //    if (string.Compare(Session["JSTurnedOn"].ToString(), "False", true) == 0
        //        || string.Compare(TurnOffFeaturedImages, "1", true) == 0)
        //    {
        //        litStackStyle.Text = "";
        //        litStackScript.Text = "";
        //        return;
        //    }
        //    #endregion

        //    #region Dynamic_Style
        //    System.Text.StringBuilder litStyle = new System.Text.StringBuilder();
        //    string ImageWidth = ConfigurationManager.AppSettings["FeaturedImageWidth"];
        //    string ImageHeight = ConfigurationManager.AppSettings["FeaturedImageHeight"];
            
        //    //string litStyle;
        //    litStyle.AppendLine("<style type=\"text/css\" media=\"screen\">");
        //    for (int i = 0; i < arrStackDivIds.Count; i++)
        //    {
        //        //litStyle.AppendLine("#" + arrStackDivIds[i].ToString() + " .slides_container { width:" + ImageWidth + "px; display:none; }"); ///Original
        //        litStyle.AppendLine("#" + arrStackDivIds[i].ToString() + " .slides_container { width:" + ImageWidth + "px; height:" + ImageHeight + "px; display:block; }"); ///Modified - Don't know why, but if display:none is used and if there is only one image in the div, it does not show up. So changed to display:block.
        //        litStyle.AppendLine("#" + arrStackDivIds[i].ToString() + " .slides_container div { width:" + ImageWidth + "px; height:" + ImageHeight + "px; display:block; }");
        //    }
        //    litStyle.AppendLine(".next { outline: none; }");
        //    litStyle.AppendLine(".prev { outline: none; }");
        //    litStyle.AppendLine(".btn_sliderprev { width:71px; height:15px; }");
        //    litStyle.AppendLine(".btn_slidernext { width:65px; height:15px; }");

        //    litStyle.AppendLine("</style>");

        //    litStackStyle.Text = litStyle.ToString();
        //    #endregion
        //    #region Dynamic_Script
        //    System.Text.StringBuilder litScript = new System.Text.StringBuilder();
            
        //    litScript.Append("<script src=\"js/jquery-1.6.2.min.js\"></script>");
        //    litScript.AppendLine("<script src=\"js/slides.min.jquery.js\"></script>");
            
        //    int stackid = -1; //declare and initialize
        //    litScript.AppendLine("<script type=\"text/javascript\" language=\"javascript\">");
        //    litScript.AppendLine("var startpos=1"); //Global JS variable
        //    litScript.AppendLine("var currstackid=-99"); //Global JS variable
            
        //    litScript.AppendLine("$(function(){");
        //    for (int i = 0; i < arrStackDivIds.Count; i++)
        //    {
        //        //adding try catch for safety - 09/08/2011
        //        try
        //        {
        //            stackid = Convert.ToInt32(arrStackIds[i].ToString());
        //        }
        //        catch (Exception Ex)
        //        {
        //            Microsoft.Practices.EnterpriseLibrary.Logging.LogEntry logEnt = new Microsoft.Practices.EnterpriseLibrary.Logging.LogEntry();
        //            logEnt.Message = "\r\n" + "ListFeatures_PreRender Error: Convert failed while obtaining stackid from Stack Id Array. " + "\r\n" + "Source: " + Ex.Source + "\r\n" + "Description: " + Ex.Message + "\r\n" + "Stack Trace: " + Ex.StackTrace;
        //            Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(logEnt, "Logs");
        //            stackid = -1; //set it to default value
        //        }
                
        //        ///Begin - Use synchronous Ajax call to get the last stack image for the slide, 
        //        ///from the session dictionary collection, and assign it to startpos variable
        //        litScript.AppendLine(" currstackid=" + stackid + ";");
        //        litScript.AppendLine(" $.ajax({ ");
        //        litScript.AppendLine(" type: \"POST\", ");
        //        litScript.AppendLine(" url:  \"home.aspx/GetPos\", ");
        //        litScript.AppendLine(" async:  false, ");
        //        litScript.AppendLine(" data: \"{'searchstackid':'\" + currstackid + \"' }\", ");
        //        litScript.AppendLine(" contentType: \"application/json; charset=utf-8\", ");
        //        litScript.AppendLine(" dataType: \"json\", ");
        //        litScript.AppendLine(" success: function(msg) { ");
        //        litScript.AppendLine(" startpos=msg.d;  }, ");
        //        litScript.AppendLine(" error: function OnAjaxCallError(request, status, error) { ");
        //        litScript.AppendLine(" } ");
        //        litScript.AppendLine(" });");
        //        ///End - Use Ajax call to get the last stack image for slide

        //        //litScript.AppendLine("alert(startpos);");

        //        ///Begin - Use slides js library functions to setup the slides and assign startpos as the start slide
        //        litScript.AppendLine("$('#" + arrStackDivIds[i].ToString() + "').slides({");
        //        //litScript.AppendLine("$('#slides').slides({");
        //        litScript.AppendLine("generatePagination: false,");
        //        litScript.AppendLine("generateNextPrev: false,");
        //        litScript.AppendLine("play: 0,");
        //        //litScript.AppendLine("preload: true,");
        //        //litScript.AppendLine("effect: 'fade',");
        //        //litScript.AppendLine("crossfade: true,");
        //        litScript.AppendLine("start: startpos,");
        //        litScript.AppendLine("animationComplete: function(current) {");
        //        litScript.AppendLine(" var StackId=" + stackid + ";");
        //        litScript.AppendLine(" $.ajax({ ");
        //        litScript.AppendLine(" type: \"POST\", ");
        //        litScript.AppendLine(" url:  \"home.aspx/SavePos\", ");
        //        litScript.AppendLine(" data: \"{'pos':'\" + StackId + \"', 'num':'\" + current + \"' }\", ");
        //        litScript.AppendLine(" contentType: \"application/json; charset=utf-8\", ");
        //        litScript.AppendLine(" dataType: \"json\", ");
        //        litScript.AppendLine(" success: function(msg) { ");
        //        litScript.AppendLine(" /*alert(msg.d)*/ }, ");
        //        litScript.AppendLine(" error: function OnAjaxCallError(request, status, error) { ");
        //        litScript.AppendLine(" } ");
        //        litScript.AppendLine(" });");
        //        litScript.AppendLine(" } ");
        //        litScript.AppendLine("});");
        //        ///End - Use slides js functions to setup the slides
                
        //    };
        //    litScript.AppendLine( "});");
            
        //    ///Begin - Use jQuery to assign the slider next/previous image source attributes
        //    litScript.AppendLine("$(\".btn_sliderprev\").attr(\"src\", \"js/slider_previous.gif\")");
        //    litScript.AppendLine("$(\".btn_slidernext\").attr(\"src\", \"js/slider_next.gif\")");
        //    ///End - Use jQuery to assign the slider next/previous image source attributes
            
        //    litScript.AppendLine("</script>");
            
        //    litStackScript.Text = litScript.ToString();
        //    #endregion
        //}
        //protected void ListFeatures_IDB(object sender, DataListItemEventArgs e)
        //{   
        //    ///Begin - JavaScript is turned off
        //    #region No_JS_Or_FeaturedImagesOff
        //    if (string.Compare(Session["JSTurnedOn"].ToString(), "False", true) == 0
        //        || string.Compare(TurnOffFeaturedImages,"1",true) == 0)
        //    {
        //        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        //        {
        //            string litHTML = "";
        //            Label lblStackTitle = (Label)e.Item.FindControl("lblStackTitle");
        //            lblStackTitle.Text = ""; //initialize
        //            Literal litStackScript = (Literal)e.Item.FindControl("litStackScript"); //For JavaScript
        //            Literal litStackMarkup = (Literal)e.Item.FindControl("litStackMarkup"); //For HTML

        //            StackCollection StackColl = (StackCollection)e.Item.DataItem;
        //            lblStackTitle.Text = StackColl.StackCollTitle + " (" + StackColl.Count + ")";

        //            litHTML =   "<div class=" + "\"" + "slides_nojs" + "\"" + ">";

        //            string newtempStr = "";
        //            foreach (PubEnt.BLL.Stack StackItem in StackColl)
        //            {
        //                newtempStr += "<div style=" + "\"padding-top:2px;padding-bottom:2px;\"" + ">" +
        //                               "<a href=" + "\"" + "detail.aspx?prodid=" + StackItem.StackPubProdId + "\"" + ">" +
        //                                    StackItem.StackPubShortTitle +
        //                                "</a>" +
        //                             "</div>";
        //            }

        //            litHTML = litHTML + newtempStr +

        //                            "</div>";
                        

        //            litStackMarkup.Text = litHTML; //+"<div style=background-color:yellow>some data</div>";

        //            ///Release objects
        //            StackColl = null;
        //            lblStackTitle = null;
        //            litStackScript = null;
        //            litStackMarkup = null;

        //        }
        //        return; //Exit method
        //    }
        //    #endregion
        //    ///End - JavaScript is truned off

        //    int pubsincurrstack = 0;
        //    slidedivcount++;
        //    string ImgWidth = ""; int initWidth = 0; int divWidth = 0;  string ImgHeight = ""; int initHeight = 0;
        //    string PubFeaturedImagesURL = "";
        //    #region GetConfigValues
        //    ImgWidth = ConfigurationManager.AppSettings["FeaturedImageWidth"];
        //    if (ImgWidth.Length > 0)
        //        initWidth = Int32.Parse(ImgWidth);
        //    ImgHeight = ConfigurationManager.AppSettings["FeaturedImageHeight"];
        //    if (ImgHeight.Length > 0)
        //        initHeight = Int32.Parse(ImgHeight);
        //    PubFeaturedImagesURL = ConfigurationManager.AppSettings["PubFeaturedImagesURL"];
        //    #endregion

        //    if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        //    {
        //        string litHTML = ""; /*string litScript = "";*/ string tempStr = "";
        //        Label lblStackTitle = (Label)e.Item.FindControl("lblStackTitle");
        //        lblStackTitle.Text = ""; //initialize
        //        Literal litStackScript = (Literal)e.Item.FindControl("litStackScript"); //For JavaScript
        //        Literal litStackMarkup = (Literal)e.Item.FindControl("litStackMarkup"); //For HTML

        //        StackCollection StackColl = (StackCollection)e.Item.DataItem;
        //        //Moved to DAL StackColl.Sort(StackCollection.StackFields.LongTitle, true); //Each collection needs to be sorted by the pubs long title
        //        lblStackTitle.Text = StackColl.StackCollTitle + " (" + StackColl.Count + ")";

        //        divWidth = initWidth * StackColl.Count; ///Count times one image width

        //        tempStr = "slides_" + slidedivcount.ToString();
        //        litHTML = "<div id=" + "\"" + tempStr + "\"" + ">" +
        //                        "<div class=" + "\"" + "slides_container" + "\"" + ">";
        //        arrStackDivIds.Add(tempStr);

        //        string newtempStr = ""; tempStr = ""; //clear for re-use
        //        foreach (PubEnt.BLL.Stack StackItem in StackColl)
        //        {   
        //            newtempStr += "<div" + ">" + 
        //                           "<a href=" + "\"" + "detail.aspx?prodid=" + StackItem.StackPubProdId + "\"" + ">" +
        //                                "<img src=" + "\"" + PubFeaturedImagesURL + "/" + StackItem.StackPubFeaturedImage + "\"" + " alt=\"" + StackItem.StackPubShortTitle + "\"" + "/>" +
        //                            "</a>" +
        //                         "</div>";

        //            if (!arrStackIds.Contains(StackItem.StackId)) //maintain the list of unique stack ids
        //                arrStackIds.Add(StackItem.StackId);

        //            pubsincurrstack++;
        //        }
                
        //        tempStr = "<a href=\"#\" class=\"prev\"><img class=\"btn_sliderprev\" alt=\"Previous\"></a>" +
        //                  "<a href=\"#\" class=\"next\"><img class=\"btn_slidernext\" alt=\"Next\"></a>";

        //        if (pubsincurrstack <= 1) //add the slider buttons html only if there is more than one pub in the slider
        //            tempStr = "";
                
        //        litHTML = litHTML + newtempStr + 
                                    
        //                        "</div>" +
        //                        //"<a href=\"#\" class=\"prev\"><img class=\"btn_sliderprev\" alt=\"Previous\"></a>" +
        //                        //"<a href=\"#\" class=\"next\"><img class=\"btn_slidernext\" alt=\"Next\"></a>" + 
        //                        tempStr + 
        //                    "</div>";

        //        litStackMarkup.Text = litHTML; //+"<div style=background-color:yellow>some data</div>";
                
        //        ///Release objects
        //        StackColl = null;
        //        lblStackTitle = null;
        //        litStackScript = null;
        //        litStackMarkup = null;
        //    }
        //}

        ///// <summary>
        ///// Method accepts the stackid and the current slider position for the stack.
        ///// The details are saved to the dictionary object in the session.
        ///// The method is invoked using Ajax and jQuery/JSON
        ///// </summary>
        ///// <param name="pos"></param>
        ///// <param name="num"></param>
        ///// <returns></returns>
        //[WebMethod]
        //public static string SavePos(string pos, string num)
        ////public static string GetDate(string pos)
        //{
        //    if (pos == "-1") //09/08/2011 - For safety, some error may have occurred
        //        return "";

        //    string stacknum = pos;
        //    string currentnum = num;
        //    //HttpContext.Current.Session["StackNUM"] = stacknum;
        //    //NameValueCollection nmColl = new NameValueCollection();
        //    Dictionary<string,string> dictColl = new Dictionary<string,string>();
        //    bool boolFound = false;

        //    if (HttpContext.Current.Session["StackInfo"] == null)
        //    {
        //        //nmColl.Add(stacknum, currentnum);
        //        dictColl.Add(stacknum, currentnum);
        //        //HttpContext.Current.Session["StackInfo"] = nmColl;
        //        HttpContext.Current.Session["StackInfo"] = dictColl;

        //        ///Record this new access to the database
        //        DAL.DAL.SaveStackAccess(Int32.Parse(stacknum));
        //    }
        //    else
        //    {
        //        //nmColl = (NameValueCollection)HttpContext.Current.Session["StackInfo"];
        //        dictColl = (Dictionary<string,string>)HttpContext.Current.Session["StackInfo"];
        //        //foreach (string currKey in nmColl.AllKeys)
        //        string currKey = ""; string currValue = "";
        //        foreach (KeyValuePair<string,string> kvp in dictColl)
        //        {
        //            currKey = kvp.Key; currValue = kvp.Value;
        //            if (currKey == stacknum) //If already present in the collection update it
        //            {
        //                //nmColl.Remove(currKey); //remove the key value pair
        //                //nmColl.Add(currKey, currentnum); //add it back
        //                dictColl[currKey] = currentnum;
        //                boolFound = true;
        //            }
        //        }
        //        if (!boolFound) //If not already present in the collection add it instead of updating it
        //        {
        //            //nmColl.Add(stacknum, currentnum);
        //            dictColl.Add(stacknum, currentnum);

        //            ///Record this new access to the database
        //            DAL.DAL.SaveStackAccess(Int32.Parse(stacknum));
        //        }
        //        //HttpContext.Current.Session["StackInfo"] = nmColl;
        //        HttpContext.Current.Session["StackInfo"] = dictColl;
        //    }
        //    dictColl = null; //release

        //    //return DateTime.Now.ToString();
        //    return "";
        //}

        ///// <summary>
        ///// Method accepts a stackid and returns the last 
        ///// saved slider position for the stack.
        ///// The method is invoked using Ajax and jQuery/JSON
        ///// </summary>
        ///// <param name="searchstackid"></param>
        ///// <returns></returns>
        //[WebMethod]
        //public static string GetPos(string searchstackid)
        //{
        //    string pos = "1"; //default position
        //    if (HttpContext.Current.Session["StackInfo"] != null)
        //    {
        //        Dictionary<string, string> dictColl = (Dictionary<string, string>)HttpContext.Current.Session["StackInfo"];
        //        string currKey = ""; string currValue = "";
        //        foreach (KeyValuePair<string, string> kvp in dictColl)
        //        {
        //            currKey = kvp.Key; currValue = kvp.Value;
        //            if (currKey == searchstackid) //If already present in the collection update it
        //            {
        //                //nmColl.Remove(currKey); //remove the key value pair
        //                //nmColl.Add(currKey, currentnum); //add it back
        //                pos = dictColl[currKey];
        //                break;
        //            }
        //        }
        //    }
        //    return pos;
        //}

        ////End CR-36

        #endregion
    }
}
