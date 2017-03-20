using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//added
using PubEnt.GlobalUtils;

namespace PubEnt.usercontrols
{
    public partial class searchbar_search : System.Web.UI.UserControl
    {
        string strText = "";
        int intLetterCount = 0;
        string ParentPage = "";

        /// <summary>
        ///SearchButtonClick eventhandler exposes the search botton click event to the parent web page,  
        ///so that the parent web page can have code exectued when the usercontrol button is clicked.
        /// </summary>
        public EventHandler SearchButtonClick;

        protected void Page_Load(object sender, EventArgs e)
        {
            //For Hailstorm check length
            if (txtSearch.Text.Length > 100) //Using a hundred limit for search contains sp
                Response.Redirect("default.aspx?redirect=searchtext", true);


            #region ShowControls
            this.GetParentPageName();
            //ParentPage = "other"; //To Test
            if (string.Compare(ParentPage, "searchres") == 0)
            {
                btnSearch.Visible = true;
                btnSearch.Text = "New Search";
                btnSearch.CssClass = "btn newsearch";
                divBlank.Visible = false;
            }
            else if (string.Compare(ParentPage, "refsearch") == 0)
            {
                divSearchText.Visible = false;
                divAdvSpacer.Visible = true;
                divBlank.Visible = false;
            }
            else if (string.Compare(ParentPage, "home") == 0)
            {
                divBlank.Visible = false;
            }
            else if (string.Compare(ParentPage, "other") == 0)
            {
                divSearchText.Visible = false;
                divSearchTitle.Visible = false;
            }
            #endregion

            if (!IsPostBack)
            {
                if (Session["PUBENT_SearchKeyword"] != null)
                    txtSearch.Text = Session["PUBENT_SearchKeyword"].ToString();
                this.RepeaterLetterIndices.DataSource = DAL.DAL.GetIndexLetters();
                this.RepeaterLetterIndices.DataBind();
            }

        }

        protected void RepeaterLetterIndices_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                System.Data.IDataRecord IRec = (System.Data.IDataRecord)e.Item.DataItem;
                strText = IRec["IndexLetter"].ToString();
                intLetterCount = IRec.GetInt32(IRec.GetOrdinal("LetterCount"));
                IRec = null;

                HyperLink lnkIndexLetter = (HyperLink)e.Item.FindControl("lnkLetterIndex");
                lnkIndexLetter.Text = strText;
                if (intLetterCount > 0)
                    lnkIndexLetter.NavigateUrl = "../search.aspx?starts=" + strText;
                else
                    lnkIndexLetter.CssClass = "inactiveIndexLink";
                lnkIndexLetter = null;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            OnSearchButtonClick(e);
        }

        protected void OnSearchButtonClick(EventArgs e)
        {
            if (SearchButtonClick != null)
            {
                SearchButtonClick(this, e);
            }
        }

        //Get the parent page name
        private void GetParentPageName()
        {
            if (this.Page.Request.RawUrl.Contains("searchres.aspx")
                || this.Page.Request.RawUrl.Contains("newupdated.aspx")
                || this.Page.Request.RawUrl.Contains("outofstock.aspx")
                || this.Page.Request.RawUrl.Contains("cannedsearchres.aspx")//
                || this.Page.Request.RawUrl.Contains("detail.aspx")
                )
            {
                ParentPage = "searchres";

            }

            else if (this.Page.Request.RawUrl.Contains("refsearch.aspx"))
            {
                ParentPage = "refsearch";

            }
            else if (this.Page.Request.RawUrl.Contains("home.aspx"))
            {
                ParentPage = "home";
            }
            else
                ParentPage = "other";
        }

        public string Terms
        {
            //***EAC code to guarantee 1 space between words
            get
            {
                Utils GlobalUtils = new Utils();
                string words = GlobalUtils.Clean(txtSearch.Text);
                //TODO: remove everything in words that is not a space, char, number

                string[] split = words.Split(new Char[] { ' ' });
                string terms = "";
                string delim = "";
                foreach (string s in split)
                {
                    if (s.Trim() != "" && !GlobalUtils.NoiseWord(s.ToLower()))
                    {
                        terms += delim + s;
                        delim = " ";
                    }

                }
                GlobalUtils = null;
                return terms;
            }
        }
    }
}