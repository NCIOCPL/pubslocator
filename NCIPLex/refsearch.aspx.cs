using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//added
using NCIPLex.BLL;
using NCIPLex.GlobalUtils;

namespace NCIPLex
{
    public partial class refsearch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Utils.ValidateRedirect().Length > 0) //Important check
                Response.Redirect(Utils.ValidateRedirect(), true);
            
            //Missing Session
            if (Session["JSTurnedOn"] == null)
                Response.Redirect("default.aspx?missingsession=true", true);

            if (!IsPostBack)
            {
                this.LoadData();
            }

            //Begin - Code for Appropriate Tabs
            GlobalUtils.Utils UtilMethod = new GlobalUtils.Utils();
            if (Session["NCIPLEX_Pubs"] != null)
                Master.LiteralText = UtilMethod.GetTabHtmlMarkUp(Session["NCIPLEX_Qtys"].ToString(), "home");
            else
                Master.LiteralText = UtilMethod.GetTabHtmlMarkUp("", "home");
            UtilMethod = null;
            //End Code for Tab
        }

        private void LoadData()
        {
            txtSearch.Text = Session["NCIPLEX_SearchKeyword"].ToString();

            ddlTOC.DataSource = KVPair.GetKVPair("sp_NCIPLex_getCancerTypes");
            ddlTOC.DataTextField = "val";
            ddlTOC.DataValueField = "key";
            ddlTOC.DataBind();
            ddlTOC.Items.Insert(0, new ListItem("All Types", "-99"));
            if (Session["NCIPLEX_TypeOfCancer"].ToString().Length > 0)
            {
                int Cntr = 0;
                foreach (ListItem li in ddlTOC.Items)
                {
                    if (string.Compare(li.Value, Session["NCIPLEX_TypeOfCancer"].ToString()) == 0)
                    {
                        ddlTOC.SelectedIndex = Cntr;
                        break;
                    }
                    Cntr++;
                }
            }

            ddlCT.DataSource = KVPair.GetKVPair("sp_NCIPLex_getSubjects");
            ddlCT.DataTextField = "val";
            ddlCT.DataValueField = "key";
            ddlCT.DataBind();
            ddlCT.Items.Insert(0, new ListItem("All Cancer Topics", "-99"));
            if (Session["NCIPLEX_Subject"].ToString().Length > 0)
            {
                int Cntr = 0;
                foreach (ListItem li in ddlCT.Items)
                {
                    if (string.Compare(li.Value, Session["NCIPLEX_Subject"].ToString()) == 0)
                    {
                        ddlCT.SelectedIndex = Cntr;
                        break;
                    }
                    Cntr++;
                }
            }

            ddlAud.DataSource = KVPair.GetKVPair("sp_NCIPLex_getAudience");
            ddlAud.DataTextField = "val";
            ddlAud.DataValueField = "key";
            ddlAud.DataBind();
            ddlAud.Items.Insert(0, new ListItem("All Audiences", "-99"));
            if (Session["NCIPLEX_Audience"].ToString().Length > 0)
            {
                int Cntr = 0;
                foreach (ListItem li in ddlAud.Items)
                {
                    if (string.Compare(li.Value, Session["NCIPLEX_Audience"].ToString()) == 0)
                    {
                        ddlAud.SelectedIndex = Cntr;
                        break;
                    }
                    Cntr++;
                }
            }

            ddlLan.DataSource = KVPair.GetKVPair("sp_NCIPLex_getLanguages");
            ddlLan.DataTextField = "val";
            ddlLan.DataValueField = "key";
            ddlLan.DataBind();
            ddlLan.Items.Insert(0, new ListItem("All Languages", "-99"));
            if (Session["NCIPLEX_Language"].ToString().Length > 0)
            {
                int Cntr = 0;
                foreach (ListItem li in ddlLan.Items)
                {
                    if (string.Compare(li.Value, Session["NCIPLEX_Language"].ToString()) == 0)
                    {
                        ddlLan.SelectedIndex = Cntr;
                        break;
                    }
                    Cntr++;
                }
            }

            ddlForm.DataSource = KVPair.GetKVPair("sp_NCIPLex_getProductFormats");
            ddlForm.DataTextField = "val";
            ddlForm.DataValueField = "key";
            ddlForm.DataBind();
            ddlForm.Items.Insert(0, new ListItem("All Formats", "-99"));
            if (Session["NCIPLEX_ProductFormat"].ToString().Length > 0)
            {
                int Cntr = 0;
                foreach (ListItem li in ddlForm.Items)
                {
                    if (string.Compare(li.Value, Session["NCIPLEX_ProductFormat"].ToString()) == 0)
                    {
                        ddlForm.SelectedIndex = Cntr;
                        break;
                    }
                    Cntr++;
                }
            }

            ddlColl.DataSource = KVPair.GetKVPair("sp_NCIPLex_getCollections");
            ddlColl.DataTextField = "val";
            ddlColl.DataValueField = "key";
            ddlColl.DataBind();
            ddlColl.Items.Insert(0, new ListItem("All Collections", "-99"));
            if (Session["NCIPLEX_Series"].ToString().Length > 0)
            {
                int Cntr = 0;
                foreach (ListItem li in ddlColl.Items)
                {
                    if (string.Compare(li.Value, Session["NCIPLEX_Series"].ToString()) == 0)
                    {
                        ddlColl.SelectedIndex = Cntr;
                        break;
                    }
                    Cntr++;
                }
            }

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (this.txtSearch.Text.Trim().Length == 0
                && this.ddlTOC.SelectedIndex == 0
                && this.ddlCT.SelectedIndex == 0
                && this.ddlAud.SelectedIndex == 0
                && this.ddlLan.SelectedIndex == 0
                && this.ddlForm.SelectedIndex == 0
                && this.ddlColl.SelectedIndex == 0)
            {
                MsgPanel.Visible = true;
                lblMessage.Text = "Please enter a keyword or select at least one option.";
                return;
            }

            string SearchCriteria = "";
            Session["NCIPLEX_SearchKeyword"] = "";
            Session["NCIPLEX_TypeOfCancer"] = "";
            Session["NCIPLEX_Subject"] = "";
            Session["NCIPLEX_Audience"] = "";
            Session["NCIPLEX_Language"] = "";
            Session["NCIPLEX_ProductFormat"] = "";
            //Session["PUBENT_StartsWith"] = "";
            Session["NCIPLEX_Series"] = "";
            Session["NCIPLEX_NewOrUpdated"] = "";
            //Session["PUBENT_Race"] = "";

            Session["NCIPLEX_SearchKeyword"] = this.txtSearch.Text;
            if (ddlTOC.SelectedIndex != 0)
                Session["NCIPLEX_TypeOfCancer"] = this.ddlTOC.SelectedValue;
            if (ddlCT.SelectedIndex != 0)
                Session["NCIPLEX_Subject"] = this.ddlCT.SelectedValue;
            if (ddlAud.SelectedIndex != 0)
                Session["NCIPLEX_Audience"] = this.ddlAud.SelectedValue;
            if (ddlLan.SelectedIndex != 0)
                Session["NCIPLEX_Language"] = this.ddlLan.SelectedValue;
            if (ddlForm.SelectedIndex != 0)
                Session["NCIPLEX_ProductFormat"] = this.ddlForm.SelectedValue;
            if (ddlColl.SelectedIndex != 0)
                Session["NCIPLEX_Series"] = this.ddlColl.SelectedValue;
            
            ////Session["PUBENT_StartsWith"] = "";
            //Session["PUBENT_NewOrUpdated"] = "";
            ////Session["PUBENT_Race"] = "";

            #region SearchCriteriaText
            Session["NCIPLEX_Criteria"] = "";

            if (Session["NCIPLEX_SearchKeyword"].ToString().Length > 0)
                SearchCriteria = Session["NCIPLEX_SearchKeyword"].ToString();

            if (ddlTOC.SelectedIndex != 0)
            {
                if (SearchCriteria.Length == 0)
                    SearchCriteria = ddlTOC.SelectedItem.Text;
                else
                    SearchCriteria = SearchCriteria + ", " + ddlTOC.SelectedItem.Text;
            }

            if (ddlCT.SelectedIndex != 0)
            {
                if (SearchCriteria.Length == 0)
                    SearchCriteria = ddlCT.SelectedItem.Text;
                else
                    SearchCriteria = SearchCriteria + ", " + ddlCT.SelectedItem.Text;
            }


            if (ddlAud.SelectedIndex != 0)
            {
                if (SearchCriteria.Length == 0)
                    SearchCriteria = ddlAud.SelectedItem.Text;
                else
                    SearchCriteria = SearchCriteria + ", " + ddlAud.SelectedItem.Text;
            }

            if (ddlForm.SelectedIndex != 0)
            {
                if (SearchCriteria.Length == 0)
                    SearchCriteria = ddlForm.SelectedItem.Text;
                else
                    SearchCriteria = SearchCriteria + ", " + ddlForm.SelectedItem.Text;
            }

            if (ddlLan.SelectedIndex != 0)
            {
                if (SearchCriteria.Length == 0)
                    SearchCriteria = ddlLan.SelectedItem.Text;
                else
                    SearchCriteria = SearchCriteria + ", " + ddlLan.SelectedItem.Text;
            }

            if (ddlColl.SelectedIndex != 0)
            {
                if (SearchCriteria.Length == 0)
                    SearchCriteria = ddlColl.SelectedItem.Text;
                else
                    SearchCriteria = SearchCriteria + ", " + ddlColl.SelectedItem.Text;
            }

            if (Session["NCIPLEX_StartsWith"].ToString().Length > 0)
            {
                if (SearchCriteria.Length == 0)
                    SearchCriteria = Session["NCIPLEX_StartsWith"].ToString();
                else
                    SearchCriteria = SearchCriteria + ", " + Session["NCIPLEX_StartsWith"].ToString();
            }

            //New or Updated search Session Variable is set to empty string in refined search, so no need to consider that
            
            if (Session["NCIPLEX_Race"].ToString().Length > 0)
            {
                if (SearchCriteria.Length == 0)
                    SearchCriteria = Session["NCIPLEX_Race"].ToString();
                else
                    SearchCriteria = SearchCriteria + ", " + Session["NCIPLEX_Race"].ToString();
            }

            Session["NCIPLEX_Criteria"] = SearchCriteria;

            #endregion

            #region GenerateSearchResultsURL
            GlobalUtils.Utils objUtils = new GlobalUtils.Utils();
            string QueryParams = objUtils.GetQueryStringParams();
            objUtils = null;
            #endregion

            Response.Redirect("searchres.aspx" + "?sid=" + QueryParams);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {   
            Session["NCIPLEX_SearchKeyword"] = "";
            this.txtSearch.Text = "";
            Session["NCIPLEX_TypeOfCancer"] = "";
            this.ddlTOC.SelectedIndex = 0;
            Session["NCIPLEX_Subject"] = "";
            this.ddlCT.SelectedIndex = 0;
            Session["NCIPLEX_Audience"] = "";
            this.ddlAud.SelectedIndex = 0;
            Session["NCIPLEX_Language"] = "";
            this.ddlLan.SelectedIndex = 0;
            Session["NCIPLEX_ProductFormat"] = "";
            this.ddlForm.SelectedIndex = 0;
            Session["NCIPLEX_StartsWith"] = "";
            Session["NCIPLEX_Series"] = "";
            this.ddlColl.SelectedIndex = 0;
            Session["NCIPLEX_NewOrUpdated"] = "";
            Session["NCIPLEX_Race"] = "";

            MsgPanel.Visible = false;
        }
    }
}
