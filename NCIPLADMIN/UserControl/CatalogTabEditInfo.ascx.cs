using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using PubEntAdmin.DAL;
using PubEntAdmin.BLL;

namespace PubEntAdmin.UserControl
{
    public partial class CatalogTabEditInfo : System.Web.UI.UserControl
    {
        #region Fields
        private int intPubID;
        #endregion

        #region Events Handling
        protected void Page_Load(object sender, EventArgs e)
        {
            #region ClientScript
            ScriptManager.RegisterStartupScript(this,
                typeof(CatalogTabEditInfo), "CatalogTabEditInfo_ClientScript",
            @"
            if (!document.getElementById('" + this.chkboxWYNTKSpanish.ClientID + @"').checked)
                document.getElementById('" + this.chkboxWYNTKSpanish.ClientID + @"').disabled = true;
            else
                document.getElementById('" + this.chkboxWYNTKSpanish.ClientID + @"').disabled = false;
        
            if (!document.getElementById('" + this.chkboxWYNTK.ClientID + @"').checked)
                document.getElementById('" + this.chkboxWYNTK.ClientID + @"').disabled = true;
            else
                document.getElementById('" + this.chkboxWYNTK.ClientID + @"').disabled = false;

            if (document.getElementById('" + this.ListSubject.ClientID + @"').selectedIndex <= 0)
                document.getElementById('" + this.ListSubject.ClientID + @"').disabled = true; 
            else
                document.getElementById('" + this.ListSubject.ClientID + @"').disabled = false;
 
            function listCategoryOnchange() {

                document.getElementById('" + this.chkboxWYNTK.ClientID + @"').checked = false;
                document.getElementById('" + this.chkboxWYNTKSpanish.ClientID + @"').checked = false;
                document.getElementById('" + this.ListSubject.ClientID + @"').selectedIndex = 0;

                document.getElementById('" + this.hidden_listCategory.ClientID + @"').value = 
                    document.getElementById('" + this.listCategory.ClientID + @"').
                    options[document.getElementById('" + this.listCategory.ClientID + @"').selectedIndex].value;

                document.getElementById('" + this.hidden_listSubCategory.ClientID + @"').value = '';
                document.getElementById('" + this.hidden_listSubSubCategory.ClientID + @"').value = '';
                document.getElementById('" + this.hidden_ListSubject.ClientID + @"').value = '';

                if (document.getElementById('" + this.listCategory.ClientID + @"').
		            options[document.getElementById('"+ this.listCategory.ClientID + @"').selectedIndex].text =='Specific Cancers') 
	            {
		            document.getElementById('" + this.chkboxWYNTK.ClientID + @"').disabled = false;
	            }
	            else {
		            document.getElementById('" + this.chkboxWYNTK.ClientID + @"').disabled = true;
                }
                document.getElementById('" + this.chkboxWYNTKSpanish.ClientID + @"').disabled = true;
                document.getElementById('" + this.ListSubject.ClientID + @"').disabled = true; 
            }

            function listSubCategoryOnchange()
            {
                document.getElementById('" + this.hidden_listSubCategory.ClientID + @"').value = 
                    document.getElementById('" + this.listSubCategory.ClientID + @"').
                    options[document.getElementById('" + this.listSubCategory.ClientID + @"').selectedIndex].value;

                document.getElementById('" + this.hidden_listSubSubCategory.ClientID + @"').value = '';
            }

            function listSubSubCategoryOnchange() {

                document.getElementById('" + this.hidden_listSubSubCategory.ClientID + @"').value = 
                    document.getElementById('" + this.listSubSubCategory.ClientID + @"').
                    options[document.getElementById('" + this.listSubSubCategory.ClientID + @"').selectedIndex].value;

                if ((document.getElementById('" + this.listSubSubCategory.ClientID + @"').
                    options[document.getElementById('" + this.listSubSubCategory.ClientID + @"').selectedIndex].text == 'Sitios de cáncer (Specific Cancers)')
                    && (document.getElementById('" + this.listSubCategory.ClientID + @"').
                    options[document.getElementById('" + this.listSubCategory.ClientID + @"').selectedIndex].text == 'Spanish') 
                    && (document.getElementById('" + this.listCategory.ClientID + @"').
                    options[document.getElementById('" + this.listCategory.ClientID + @"').selectedIndex].text == 'Materials in Other Languages'))
                {
                    document.getElementById('" + this.chkboxWYNTKSpanish.ClientID + @"').disabled = false;
                }
                else
                {
                    document.getElementById('" + this.chkboxWYNTKSpanish.ClientID + @"').disabled = true;
                }
                document.getElementById('" + this.chkboxWYNTK.ClientID + @"').disabled = true;
                document.getElementById('" + this.ListSubject.ClientID + @"').disabled = true; 
                document.getElementById('" + this.hidden_ListSubject.ClientID + @"').value = '';
            }

            function listSubjectOnchange()
            {
                
                document.getElementById('" + this.hidden_ListSubject.ClientID + @"').value = 
                    document.getElementById('" + this.ListSubject.ClientID + @"').
                    options[document.getElementById('" + this.ListSubject.ClientID + @"').selectedIndex].value;

                //alert(document.getElementById('" + this.hidden_ListSubject.ClientID + @"').value);
            }

            function chkboxWYNTKOnclick()
            {
                if (document.getElementById('" + this.chkboxWYNTK.ClientID + @"').checked)
                    document.getElementById('" + this.ListSubject.ClientID + @"').disabled = false; 
                else
                {
                    document.getElementById('" + this.ListSubject.ClientID + @"').disabled = true;
                    document.getElementById('" + this.ListSubject.ClientID + @"').selectedIndex = 0;
                    document.getElementById('" + this.hidden_ListSubject.ClientID + @"').value = '';
                } 
            }

            function chkboxWYNTKSpanishOnclick()
            {
                if (document.getElementById('" + this.chkboxWYNTKSpanish.ClientID + @"').checked)
                    document.getElementById('" + this.ListSubject.ClientID + @"').disabled = false; 
                else
                {
                    document.getElementById('" + this.ListSubject.ClientID + @"').disabled = true;
                    document.getElementById('" + this.ListSubject.ClientID + @"').selectedIndex = 0;
                    document.getElementById('" + this.hidden_ListSubject.ClientID + @"').value = '';
                } 
            }
            ", true);
            #endregion

            this.BindOption();
            if (this.PubID > 0)
                this.BindData();
            this.SecVal();

            if (Session[PubEntAdminManager.strTabContentPrevActTabIndex] == null)
            {
                this.RegisterMonitoredChanges();
                Session[PubEntAdminManager.strTabContentPrevActTabIndex] =
                    Session[PubEntAdminManager.strTabContentCurrActTabIndex];
            }
            else
            {
                if (Session[PubEntAdminManager.strTabContentPrevActTabIndex] !=
                    Session[PubEntAdminManager.strTabContentCurrActTabIndex])
                {
                    this.RegisterMonitoredChanges();
                    Session[PubEntAdminManager.strTabContentPrevActTabIndex] =
                    Session[PubEntAdminManager.strTabContentCurrActTabIndex];
                }
            }

            this.listCategory.Attributes.Add("onchange", "javascript:listCategoryOnchange()");
            this.listSubCategory.Attributes.Add("onchange", "javascript:listSubCategoryOnchange()");
            this.listSubSubCategory.Attributes.Add("onchange", "javascript:listSubSubCategoryOnchange()");
            this.ListSubject.Attributes.Add("onchange", "javascript:listSubjectOnchange()");
            this.chkboxWYNTK.Attributes.Add("onclick", "javascript:chkboxWYNTKOnclick()");
            this.chkboxWYNTKSpanish.Attributes.Add("onclick", "javascript:chkboxWYNTKSpanishOnclick()");
            
        }

        protected void listCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            //SubCatCollection a = PE_DAL.GetSubCatBySubID(System.Convert.ToInt32(this.listCategory.SelectedValue));

            //if (a.Count > 0)
            //{
            //    this.trCatalogSubcategory.Visible = true;
            //    this.trCatalogSubsubcategory.Visible = true;

            //    this.listSubCategory.DataSource = a;
            //    this.listSubCategory.DataTextField = "name";
            //    this.listSubCategory.DataValueField = "id";
            //    this.listSubCategory.DataBind();
            //    this.listSubCategory.Items.Insert(0, new ListItem("Select SubCategory", "0"));
                
            //}
            //else
            //{
            //    this.trCatalogSubcategory.Visible = false;
            //    this.trCatalogSubsubcategory.Visible = false;
            //}

        }

        protected void listSubCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            //SubSubCatCollection a = PE_DAL.GetSubSubCatBySubID(System.Convert.ToInt32(this.listSubCategory.SelectedValue));

            //if (a.Count > 0)
            //{
            //    this.trCatalogSubsubcategory.Visible = true;
            //    this.listSubSubCategory.DataSource = a;
            //    this.listSubSubCategory.DataTextField = "name";
            //    this.listSubSubCategory.DataValueField = "id";
            //    this.listSubSubCategory.DataBind();
            //    this.listSubSubCategory.Items.Insert(0, new ListItem("Select SubSubCategory", "0"));
            //}
            //else
            //{
            //    this.trCatalogSubsubcategory.Visible = false;
            //}

        }

        #endregion

        #region Methods

        protected void BindOption()
        {
            if (Session[PubEntAdminManager.strPubGlobalMode].ToString() ==
                PubEntAdminManager.strPubGlobalAMode)
            {
                this.ListSubject.DataSource = PE_DAL.GetAllCatalogWYNTKSubjectForNew(true);
                this.ListSubject.DataTextField = "name";
                this.ListSubject.DataValueField = "id";
                this.ListSubject.DataBind();
                this.ListSubject.Items.Insert(0, new ListItem("[Select Subject]", "0"));
            }
            else
            {
                this.ListSubject.DataSource = PE_DAL.GetAllCatalogWYNTKSubjectForExist(this.PubID, true);
                this.ListSubject.DataTextField = "name";
                this.ListSubject.DataValueField = "id";
                this.ListSubject.DataBind();
                this.ListSubject.Items.Insert(0,new ListItem("[Select Subject]", "0"));
            }
        }

        private void BindOption_Category()
        {
            this.listCategory.DataSource = PE_DAL.GetAllCatalogSubject(true);
            this.listCategory.DataTextField = "name";
            this.listCategory.DataValueField = "id";
            this.listCategory.DataBind();
            this.listCategory.Items.Insert(0, new ListItem("[Select Category]", "0"));
        }

        private void BindOption_SubCategory(int categoryid)
        {
            this.listSubCategory.DataSource = PE_DAL.GetSubCatBySubID(categoryid);
            this.listSubCategory.DataTextField = "name";
            this.listSubCategory.DataValueField = "id";
            this.listSubCategory.DataBind();
            this.listSubCategory.Items.Insert(0, new ListItem("[Select SubCategory]", "0"));
        }

        private void BindOption_SubSubCategory(int subcategoryid)
        {
            this.listSubSubCategory.DataSource = PE_DAL.GetSubSubCatBySubID(subcategoryid);
            this.listSubSubCategory.DataTextField = "name";
            this.listSubSubCategory.DataValueField = "id";
            this.listSubSubCategory.DataBind();
            this.listSubSubCategory.Items.Insert(0, new ListItem("Select SubSubCategory", "0"));
        }

        protected void BindData()
        {
            CatalogCollection ccoll = PE_DAL.GetCatalogInterface(this.PubID);
            
            if (ccoll.Count > 0)
            {
                Catalog c = ccoll[0];

                this.CascadingDropDown1.SelectedValue = this.hidden_listCategory.Value = c.CATEGORY.ToString();
                this.CascadingDropDown2.ContextKey = this.hidden_listSubCategory.Value = c.SUBCATEGORY.ToString();
                this.CascadingDropDown3.ContextKey = this.hidden_listSubSubCategory.Value = c.SUBSUBCATEGORY.ToString();

                if (c.WYNTK > 0)
                {
                    this.chkboxWYNTK.Checked = System.Convert.ToBoolean(c.WYNTK);
                    if (c.CATALOG_SUBJECT > 0)
                    {
                        this.ListSubject.SelectedIndex =
                                this.ListSubject.Items.IndexOf(
                                this.ListSubject.Items.FindByValue(c.CATALOG_SUBJECT.ToString()));
                    }
                }

                if (c.SPANISH_WYNTK > 0)
                {
                    this.chkboxWYNTKSpanish.Checked = System.Convert.ToBoolean(c.SPANISH_WYNTK);
                    if (c.CATALOG_SUBJECT > 0)
                    {
                        this.ListSubject.SelectedIndex =
                                this.ListSubject.Items.IndexOf(
                                this.ListSubject.Items.FindByValue(c.CATALOG_SUBJECT.ToString()));
                    }
                }
            }
        }

        public bool Save()
        {
            int intCategory = -1;
            int intSubCategory = -1;
            int intSubSubCategory = -1;
            int intCatalogSub = -1;

            this.SecVal();

            if (this.hidden_listCategory.Value.Trim().Length>0)
                intCategory = System.Convert.ToInt32(this.hidden_listCategory.Value.Trim());
            if (this.hidden_listSubCategory.Value.Trim().Length > 0)
                intSubCategory = System.Convert.ToInt32(this.hidden_listSubCategory.Value.Trim());
            if (this.hidden_listSubSubCategory.Value.Trim().Length > 0)
                intSubSubCategory = System.Convert.ToInt32(this.hidden_listSubSubCategory.Value.Trim());
            if (this.hidden_ListSubject.Value.Trim().Length > 0)
                intCatalogSub = System.Convert.ToInt32(this.hidden_ListSubject.Value.Trim());
            return PE_DAL.SetCatalogInterface(this.PubID,
                intCategory > 0 ? intCategory : -1,
                intSubCategory > 0 ? intSubCategory : -1,
                intSubSubCategory > 0 ? intSubSubCategory : -1,
                this.chkboxWYNTK.Checked?1:0,this.chkboxWYNTKSpanish.Checked?1:0,
                intCatalogSub > 0 ? intCatalogSub : -1);
        }

        #region MonitorChanges
        protected void RegisterMonitoredChanges()
        {
            //PubEntAdminManager.MonitorChanges2(this.Page, this, this.listCategory);
            //PubEntAdminManager.MonitorChanges2(this.Page, this, this.listSubCategory);
            //PubEntAdminManager.MonitorChanges2(this.Page, this, this.listSubSubCategory);

            PubEntAdminManager.MonitorChanges2(this.Page, this, this.hidden_listCategory);
            PubEntAdminManager.MonitorChanges2(this.Page, this, this.hidden_listSubCategory);
            PubEntAdminManager.MonitorChanges2(this.Page, this, this.hidden_listSubSubCategory);
            PubEntAdminManager.MonitorChanges2(this.Page, this, this.chkboxWYNTK);
            PubEntAdminManager.MonitorChanges2(this.Page, this, this.chkboxWYNTKSpanish);
            PubEntAdminManager.MonitorChanges2(this.Page, this, this.ListSubject);
        }

        protected void RegisterMonitoredScript()
        {
            
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "CatalogTabMonitorClientScript",
                    @"
                    function CascadingDropDown1onPopulated() {
                        var cdd2 = $find('" + this.CascadingDropDown2.ClientID + @"');
                        
                        if (cdd2) {
                            //alert('cdd2: '+cdd2.get_element().options.length);
                            if (cdd2.get_element().options.length>1)
                            {
                                cdd2.add_populated(CascadingDropDown2onPopulated);
                            }
                            else
                                assignInitialValuesForMonitorTabChanges();
                        }
                        else
                            assignInitialValuesForMonitorTabChanges();
                    }

                    function CascadingDropDown2onPopulated() {
                        var cdd3 = $find('" + this.CascadingDropDown3.ClientID + @"');
                        
                        if (cdd3) {
                            //alert('cdd3: '+cdd3.get_element().options.length);
                            if (cdd3.get_element().options.length>1)
                            {
                                cdd3.add_populated(CascadingDropDown3onPopulated);
                            }
                            else
                                assignInitialValuesForMonitorTabChanges();
                        }
                        else
                            assignInitialValuesForMonitorTabChanges();
                    }

                    function CascadingDropDown3onPopulated() {
                        assignInitialValuesForMonitorTabChanges();
                    }
                    ", true);
        }
        #endregion

        #region Sec Val
        private void SecVal()
        {
            this.TagVal();
            this.SpecialVal();
        }

        private void TagVal()
        {
            foreach (ListItem li in this.listCategory.Items)
            {
                if (PubEntAdminManager.OtherVal(li.Text) || PubEntAdminManager.OtherVal(li.Value))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in listSubCategory.Items)
            {
                if (PubEntAdminManager.OtherVal(li.Text) || PubEntAdminManager.OtherVal(li.Value))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in listSubSubCategory.Items)
            {
                if (PubEntAdminManager.OtherVal(li.Text) || PubEntAdminManager.OtherVal(li.Value))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in ListSubject.Items)
            {
                if (PubEntAdminManager.OtherVal(li.Text) || PubEntAdminManager.OtherVal(li.Value))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }
            
        }

        private void SpecialVal()
        {
            foreach (ListItem li in listCategory.Items)
            {
                if (PubEntAdminManager.SpecialVal2(li.Text.Replace(" ", "")) ||
                    PubEntAdminManager.SpecialVal2(li.Value.Replace(" ", "")))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in listSubCategory.Items)
            {
                if (PubEntAdminManager.SpecialVal2(li.Text.Replace(" ", "")) ||
                    PubEntAdminManager.SpecialVal2(li.Value.Replace(" ", "")))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in listSubSubCategory.Items)
            {
                if (PubEntAdminManager.SpecialVal2(li.Text.Replace(" ", "")) ||
                    PubEntAdminManager.SpecialVal2(li.Value.Replace(" ", "")))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in ListSubject.Items)
            {
                if (PubEntAdminManager.SpecialVal2(li.Text.Replace(" ", "")) ||
                    PubEntAdminManager.SpecialVal2(li.Value.Replace(" ", "")))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }
        }
        #endregion

        #endregion

        #region Properties
        public int PubID
        {
            set
            {
                this.intPubID = value;
            }
            get
            {
                return this.intPubID;
            }
        }
        #endregion

        
    }
}