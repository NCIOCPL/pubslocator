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
using System.Collections.Generic;
using PubEntAdmin.DAL;
using PubEntAdmin.BLL;
using AjaxControlToolkit;

namespace PubEntAdmin.UserControl
{
    public partial class SubCat : System.Web.UI.UserControl
    {
        #region Event Handling
        protected void Page_Load(object sender, EventArgs e)
        {
            this.hidSubCatIndex.Value = "";

            if (PubEntAdminManager.TamperProof)
            {
                if (PubEntAdminManager.ContainURLQS("subcatid"))
                    this.SubcatID = System.Convert.ToInt32(PubEntAdminManager.GetURLQS("subcatid"));

            }
            else
            {
                if (Request.QueryString["subcatid"] != null)
                {
                    this.SubcatID = System.Convert.ToInt32(Request.QueryString["subcatid"].ToString());
                }

            }

            if (PubEntAdminManager.TamperProof)
            {
                if (PubEntAdminManager.ContainURLQS("mode"))
                    this.SubcatMode = (PubEntAdminManager.GetURLQS("mode"));

            }
            else
            {
                if (Request.QueryString["mode"] != null)
                {
                    this.SubcatMode = (Request.QueryString["mode"].ToString());
                }
            }

            if (!Page.IsPostBack)
            {
                this.SubCatWizard.ActiveStepIndex = 0;
                this.CategoryBindOptions();

                if (this.SubcatID > 0)
                {
                    this.IncrementWizardPage();
                    
                    EditSubcat l_EditSubcat = ((EditSubcat)SubCategoryEditStep.ContentTemplateContainer.
                        FindControl("EditSubcat1"));
                    l_EditSubcat.BindOptions();
                    l_EditSubcat.SubCatID = this.SubcatID;
                    l_EditSubcat.BindValues();
                    this.IncrementWizardPage();

                    PubEntAdmin.BLL.SubCat l_SubCat = PE_DAL.GetSubCatBySubCatID2(this.SubcatID);
                    ((Label)SubCategoryAddEditStep.ContentTemplateContainer.FindControl("lblCategoryStep")).Text =
                    ((Label)SubCategoryEditStep.ContentTemplateContainer.FindControl("lblCategoryStep2")).Text = l_SubCat.AssignedCatName;
                    this.AddSubCatBindOptions(l_SubCat.AssignedCatID);
                    this.EditSubCatBindOptions(l_SubCat.AssignedCatID);
                    ((Button)SubCatWizard.FindControl("FinishNavigationTemplateContainerID").FindControl("FinishButton")).Visible = false;
                }
                else
                {

                }
            }
            List<TabItem> l = new List<TabItem>();
            TabItem t1 = new TabItem(0, "Add", "LookupMgmt.aspx");
            TabItem t2 = new TabItem(1, "Edit", "LookupMgmt.aspx");

            l.Add(t1);
            l.Add(t2);

            ((TabStrip)SubCategoryAddEditStep.ContentTemplateContainer.FindControl("TabStrip1")).TabSource = l;

            ((Button)SubCatWizard.FindControl("StepNavigationTemplateContainerID").FindControl("StepPreviousButton")).Visible = false;
            ((Button)SubCatWizard.FindControl("FinishNavigationTemplateContainerID").FindControl("FinishPreviousButton")).Visible = false;

            this.SecVal();

        }

        protected void OnNext(object sender, WizardNavigationEventArgs e)
        {
            if (this.SubCatWizard.WizardSteps[e.CurrentStepIndex].ID == "CategoryStep")
            {
                CategoryStepSelectedIndexChanged(((DropDownList)CategoryStep.ContentTemplateContainer.
                FindControl("ddlCategory")), new EventArgs());
            }
            else if (this.SubCatWizard.WizardSteps[e.CurrentStepIndex].ID == "SubCategoryAddEditStep")
            {
                SubCategoryAddEditStepSelectedIndexChanged(new object(), new EventArgs());
                ((Button)SubCatWizard.FindControl("FinishNavigationTemplateContainerID").FindControl("FinishButton")).Visible = false;
            }
            
        }

        protected void OnActiveStepChanged(object sender, EventArgs e)
        {
            if (this.SubcatMode == String.Empty)
            {
                if (this.SubCatWizard.ActiveStepIndex == 1)
                {
                    if (((DropDownList)CategoryStep.ContentTemplateContainer.
                        FindControl("ddlCategory")).SelectedIndex == 0)
                    {
                        ((Label)this.CategoryStep.ContentTemplateContainer.FindControl("errMsg")).Text =
                            "You must select one Category to proceed.";
                        this.DecrementWizardPage();
                    }
                    else
                    {
                        ((Label)this.CategoryStep.ContentTemplateContainer.FindControl("errMsg")).Text = String.Empty;
                    }
                }
                else if (this.SubCatWizard.ActiveStepIndex == 2)
                {
                    if (((DropDownList)((MultiView)SubCategoryAddEditStep.
                    ContentTemplateContainer.FindControl("MultiView1")).Views[1].
                    FindControl("ddlSubCategory")).SelectedIndex == 0)
                    {
                        ((Label)((MultiView)SubCategoryAddEditStep.
                    ContentTemplateContainer.FindControl("MultiView1")).Views[1].
                    FindControl("errMsg")).Text =
                        "You must select one Subcategory to proceed.";
                        this.DecrementWizardPage();
                    }
                    else
                    {
                        ((Label)((MultiView)SubCategoryAddEditStep.
                    ContentTemplateContainer.FindControl("MultiView1")).Views[1].
                    FindControl("errMsg")).Text = String.Empty;
                    }
                }
            }
        }

        protected void TabStrip1_SelectionChanged(object sender, TabStrip.SelectionChangedEventArgs e)
        {
            if (e.TabNameSelected == "Add")
            {
                ((MultiView)SubCategoryAddEditStep.ContentTemplateContainer.FindControl("MultiView1")).ActiveViewIndex = 0;
            }
            else
            {
                ((MultiView)SubCategoryAddEditStep.ContentTemplateContainer.FindControl("MultiView1")).ActiveViewIndex = 1;
            }
        }

        #region Tab Header
        //protected void btnAddTab_Click(object sender, EventArgs args)
        //{
        //    ((Label)SubCategoryAddEditStep.ContentTemplateContainer.FindControl("lblCategoryStep")).Text = "Add New Category";

        //    Session[PubEntAdminManager.strTabContentLUCurrActTabIndex] = 0;
        //    ((TabContainer)SubCategoryAddEditStep.ContentTemplateContainer.FindControl("tabCont")).ActiveTabIndex = 0;
        //}

        //protected void btnEditTab_Click(object sender, EventArgs args)
        //{
        //    Session[PubEntAdminManager.strTabContentLUCurrActTabIndex] = 1;
        //    ((TabContainer)SubCategoryAddEditStep.ContentTemplateContainer.FindControl("tabCont")).ActiveTabIndex = 1;
        //}
        #endregion

        protected void CategoryStepSelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedCategory = System.Convert.ToInt32(((DropDownList)CategoryStep.ContentTemplateContainer.
                FindControl("ddlCategory")).SelectedValue);

            ((Label)SubCategoryAddEditStep.ContentTemplateContainer.FindControl("lblCategoryStep")).Text =
                ((DropDownList)CategoryStep.ContentTemplateContainer.FindControl("ddlCategory")).SelectedItem.Text;
            
            //this.IncrementWizardPage();
            this.EnableSubcategoryEditTab();
            this.EditSubCatBindOptions(selectedCategory);
            this.AddSubCatBindOptions(selectedCategory);
            this.hidSubCatIndex.Value = "1";
        }

        protected void SubCategoryAddEditStepSelectedIndexChanged(object sender, EventArgs e)
        {
            ((Label)SubCategoryEditStep.ContentTemplateContainer.FindControl("lblCategoryStep2")).Text =
            ((Label)SubCategoryAddEditStep.ContentTemplateContainer.FindControl("lblCategoryStep")).Text;

            this.SubCatBindValues();
            //this.IncrementWizardPage();
        }

        protected void lnkSubCategories_Click(object sender, EventArgs e)
        {
            DropDownList l_SubCategory = ((DropDownList)((MultiView)SubCategoryAddEditStep.
                ContentTemplateContainer.FindControl("MultiView1")).Views[1].FindControl("ddlSubCategory"));
            l_SubCategory.SelectedIndex = 0;

            EditSubcat l_EditSubcat = ((EditSubcat)SubCategoryEditStep.ContentTemplateContainer.
                FindControl("EditSubcat1"));
            l_EditSubcat.SubCatID = this.SubcatID;
            l_EditSubcat.BindValues();

            this.DecrementWizardPage();
            this.EnableSubcategoryEditTab();
            this.hidSubCatIndex.Value = "1";
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ((LookupMgmt)this.Page).ShowHideSpellChecker(false);

            #region ClientScript
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(),
            "Subcat_ClientScript",
            @"
                    function LkupParticipants()
                    {
                        return '" + this.GetSpellCheckParticipants() + @"';
                    }
                ", true);
            #endregion
        }

        #region AddSubcat
        protected void AddSubcat1Save_Click(object sender, EventArgs e)
        {
            ((Label)SubCategoryAddEditStep.ContentTemplateContainer.FindControl("lblCategoryStep")).Text =
                ((DropDownList)CategoryStep.ContentTemplateContainer.FindControl("ddlCategory")).SelectedItem.Text;

            AddSubcat l_AddSubcat = ((AddSubcat)((MultiView)SubCategoryAddEditStep.
                ContentTemplateContainer.FindControl("MultiView1")).Views[0].FindControl("AddSubcat1"));

            l_AddSubcat.SecVal();

            int l_subcatid = 0;
            bool blnSaveSubCat = PE_DAL.SetSubCat(ref l_subcatid, l_AddSubcat.SubCatName,
                l_AddSubcat.AssignToSubjID, System.Convert.ToInt32(l_AddSubcat.AllowHvSubSubCat), 1);

            if (blnSaveSubCat)
            {
                this.EnableSubcategoryEditTab();
                this.EditSubCatBindOptions(l_AddSubcat.AssignToSubjID);
                //this.hidSubCatIndex.Value = "1";
            }
        }

        protected void AddSubcat1Cancel_Click(object sender, EventArgs e)
        {
            ((Label)SubCategoryAddEditStep.ContentTemplateContainer.FindControl("lblCategoryStep")).Text =
                ((DropDownList)CategoryStep.ContentTemplateContainer.FindControl("ddlCategory")).SelectedItem.Text;

            DropDownList l_SubCategory = ((DropDownList)((MultiView)SubCategoryAddEditStep.ContentTemplateContainer.FindControl("MultiView1")).Views[1].FindControl("ddlSubCategory"));
            l_SubCategory.SelectedIndex = 0;

            this.EnableSubcategoryEditTab();
            this.hidSubCatIndex.Value = "1";
        }
        #endregion

        #region EditSubcat
        protected void EditSubcat1Save_Click(object sender, EventArgs e)
        {
            ((Label)SubCategoryAddEditStep.ContentTemplateContainer.FindControl("lblCategoryStep")).Text =
                ((DropDownList)CategoryStep.ContentTemplateContainer.FindControl("ddlCategory")).SelectedItem.Text;

            EditSubcat l_EditSubcat = ((EditSubcat)SubCategoryEditStep.ContentTemplateContainer.
                FindControl("EditSubcat1"));

            l_EditSubcat.SecVal();

            int l_subcatid = l_EditSubcat.SubCatID;
            bool blnSaveSubCat = PE_DAL.SetSubCat(ref l_subcatid, l_EditSubcat.SubCatName, 
                l_EditSubcat.AssignToSubjID, System.Convert.ToInt32(l_EditSubcat.AllowHvSubSubCat), 1);

            if (blnSaveSubCat)
            {
                //DropDownList l_SubCategory = ((DropDownList)((TabContainer)SubCategoryAddEditStep.ContentTemplateContainer.FindControl("tabCont")).Tabs[1].FindControl("ddlSubCategory"));
                //l_SubCategory.SelectedIndex = 0;
                this.DecrementWizardPage();
                this.EnableSubcategoryEditTab();
                this.EditSubCatBindOptions(l_EditSubcat.AssignToSubjID);
                this.hidSubCatIndex.Value = "1";
            }
            
        }

        protected void EditSubcat1Cancel_Click(object sender, EventArgs e)
        {
            ((Label)SubCategoryAddEditStep.ContentTemplateContainer.FindControl("lblCategoryStep")).Text =
                ((DropDownList)CategoryStep.ContentTemplateContainer.FindControl("ddlCategory")).SelectedItem.Text;

            DropDownList l_SubCategory = ((DropDownList)((MultiView)SubCategoryAddEditStep.
                ContentTemplateContainer.FindControl("MultiView1")).Views[1].FindControl("ddlSubCategory"));
            l_SubCategory.SelectedIndex = 0;

            EditSubcat l_EditSubcat = ((EditSubcat)SubCategoryEditStep.ContentTemplateContainer.
                FindControl("EditSubcat1"));
            l_EditSubcat.SubCatID = this.SubcatID;
            l_EditSubcat.BindValues();

            this.DecrementWizardPage();
            this.EnableSubcategoryEditTab();
            this.hidSubCatIndex.Value = "1";
        }

        protected void EditSubcat1Del_Click(object sender, EventArgs e)
        {
            ((Label)SubCategoryAddEditStep.ContentTemplateContainer.FindControl("lblCategoryStep")).Text =
                ((DropDownList)CategoryStep.ContentTemplateContainer.FindControl("ddlCategory")).SelectedItem.Text;

            EditSubcat l_EditSubcat = ((EditSubcat)SubCategoryEditStep.ContentTemplateContainer.
                FindControl("EditSubcat1"));

            int l_subcatid = l_EditSubcat.SubCatID;
            bool blnSaveSubCat = PE_DAL.SetSubCat(ref l_subcatid, l_EditSubcat.SubCatName, 
                l_EditSubcat.AssignToSubjID, System.Convert.ToInt32(l_EditSubcat.AllowHvSubSubCat), 0);

            if (blnSaveSubCat)
            {
                //DropDownList l_SubCategory = ((DropDownList)((TabContainer)SubCategoryAddEditStep.ContentTemplateContainer.FindControl("tabCont")).Tabs[1].FindControl("ddlSubCategory"));
                
                this.DecrementWizardPage();
                this.EnableSubcategoryEditTab();
                this.EditSubCatBindOptions(l_EditSubcat.AssignToSubjID);
                this.hidSubCatIndex.Value = "1";
            }
            
        }

        protected void EditSubcat1AddSubsubcat_Click(object sender, EventArgs e)
        {
            Session[PubEntAdminManager.strCurrCatName] = 
                ((Label)SubCategoryAddEditStep.ContentTemplateContainer.FindControl("lblCategoryStep")).Text;

            EditSubcat l_EditSubcat = ((EditSubcat)SubCategoryEditStep.ContentTemplateContainer.
                FindControl("EditSubcat1"));

            Session[PubEntAdminManager.strCurrSubcatName] = l_EditSubcat.SubCatName;

            if (PubEntAdminManager.TamperProof)
            {
                PubEntAdminManager.RedirectEncodedURLWithQS("LookupMgmt.aspx", "sub=subsubcat&mode=add&subcatid=" + l_EditSubcat.SubCatID +
                    "&retSub=subcat");
            }
            else
            {
                Response.Redirect("~/LookupMgmt.aspx?sub=subsubcat&mode=add&subcatid=" + l_EditSubcat.SubCatID +
                    "&retSub=subcat", true);
            }
        }

        protected void EditSubcat1EditSubsubcat_Click(object sender, EventArgs e)
        {
            Session[PubEntAdminManager.strCurrCatName] = 
                ((Label)SubCategoryAddEditStep.ContentTemplateContainer.FindControl("lblCategoryStep")).Text;

            EditSubcat l_EditSubcat = ((EditSubcat)SubCategoryEditStep.ContentTemplateContainer.
                FindControl("EditSubcat1"));

            Session[PubEntAdminManager.strCurrSubcatName] = l_EditSubcat.SubCatName;

            if (PubEntAdminManager.TamperProof)
            {
                PubEntAdminManager.RedirectEncodedURLWithQS("LookupMgmt.aspx", "sub=subsubcat&mode=edit&subcatid=" + l_EditSubcat.SubCatID);
            }
            else
            {
                Response.Redirect("~/LookupMgmt.aspx?sub=subsubcat&mode=edit&subcatid=" + l_EditSubcat.SubCatID);
            }
        }
        #endregion

        #endregion

        #region Properties

        protected string ParentSpellCkrID1
        {
            get
            {
                return ((LookupMgmt)this.Page).ParentSpellCkr1;
            }
        }

        protected string ParentSpellCkrID2
        {
            get
            {
                return ((LookupMgmt)this.Page).ParentSpellCkr2;
            }
        }

        public int SubcatID
        {
            set
            {
                ViewState["intSubsubcatID"] = value;
            }
            get
            {
                if (ViewState["intSubsubcatID"] != null)
                    return System.Convert.ToInt32(ViewState["intSubsubcatID"].ToString());
                else
                    return 0;
            }
        }

        public string SubcatMode
        {
            set
            {
                ViewState["strSubcatMode"] = value;
            }
            get
            {
                if (ViewState["strSubcatMode"] != null)
                    return (ViewState["strSubcatMode"].ToString());
                else
                    return string.Empty;
            }
        }
        //public string TabContClientID
        //{
        //    get
        //    {
        //        return ((TabContainer)SubCategoryAddEditStep.ContentTemplateContainer.FindControl("tabCont")).ClientID;
        //    }
        //}

        //public string SubCatWizardClientID
        //{
        //    get
        //    {
        //        return ((TabContainer)SubCategoryAddEditStep.ContentTemplateContainer.FindControl("tabCont")).ClientID;
        //    }
        //}
        #endregion

        #region Methods
        protected void EnableSubcategoryEditTab()
        {
            ((MultiView)SubCategoryAddEditStep.ContentTemplateContainer.FindControl("MultiView1")).ActiveViewIndex = 1;
        }

        protected void IncrementWizardPage()
        {
            this.SubCatWizard.ActiveStepIndex = this.SubCatWizard.ActiveStepIndex + 1;
        }

        protected void DecrementWizardPage()
        {
            this.SubCatWizard.ActiveStepIndex = this.SubCatWizard.ActiveStepIndex - 1;
            
        }

        protected void EditSubCatBindOptions(int selectedCategory)
        {
            DropDownList l_SubCategory = ((DropDownList)((MultiView)SubCategoryAddEditStep.ContentTemplateContainer.FindControl("MultiView1")).Views[1].FindControl("ddlSubCategory"));
            l_SubCategory.DataSource = PE_DAL.GetSubCatBySubID(selectedCategory);
            l_SubCategory.DataTextField = "SubCatName";
            l_SubCategory.DataValueField = "SubCatID";
            l_SubCategory.DataBind();
            l_SubCategory.Items.Insert(0, new ListItem("[Select SubCategory]", "0"));

        }

        protected void AddSubCatBindOptions(int selectedCategory)
        {
            AddSubcat l_AddSubcat = ((AddSubcat)((MultiView)SubCategoryAddEditStep.
                ContentTemplateContainer.FindControl("MultiView1")).Views[0].FindControl("AddSubcat1"));
            l_AddSubcat.AssignToSubjID = selectedCategory;

        }

        protected void SubCatBindValues()
        {
            DropDownList l_SubCategory = ((DropDownList)((MultiView)SubCategoryAddEditStep.ContentTemplateContainer.
                FindControl("MultiView1")).Views[1].FindControl("ddlSubCategory"));

            AddSubcat l_AddSubcat = ((AddSubcat)((MultiView)SubCategoryAddEditStep.
                ContentTemplateContainer.FindControl("MultiView1")).Views[0].FindControl("AddSubcat1"));
            l_AddSubcat.ClearAll();

            EditSubcat l_EditSubcat = ((EditSubcat)SubCategoryEditStep.ContentTemplateContainer.
                FindControl("EditSubcat1"));
            l_EditSubcat.SubCatID = this.SubcatID = System.Convert.ToInt32(l_SubCategory.SelectedItem.Value);
            l_EditSubcat.BindValues();
        }

        protected void CategoryBindOptions()
        {
            DropDownList l_category = ((DropDownList)this.CategoryStep.ContentTemplateContainer.
                FindControl("ddlCategory"));

            l_category.DataSource = PE_DAL.GetAllSubjectHvSubcat(true);
            l_category.DataTextField = "SubjName";
            l_category.DataValueField = "SubjID";
            l_category.DataBind();
            l_category.Items.Insert(0, new ListItem("[Select Category]", "0"));
        }

        public string GetSpellCheckParticipants()
        {
            string partipants = "";

            if (this.SubCatWizard.ActiveStepIndex == 1)
            {
                AddSubcat l_AddSubcat = ((AddSubcat)((MultiView)SubCategoryAddEditStep.
                ContentTemplateContainer.FindControl("MultiView1")).Views[0].FindControl("AddSubcat1"));

                partipants = l_AddSubcat.SubCatTextClientID;
            }
            else if (this.SubCatWizard.ActiveStepIndex == 2)
            {
                EditSubcat l_EditSubcat = ((EditSubcat)(SubCategoryEditStep.
                ContentTemplateContainer.FindControl("EditSubcat1")));

                partipants = l_EditSubcat.SubCatTextClientID;
            }

            return partipants;
        }

        #region Sec Val
        private void SecVal()
        {
            this.TagVal();
            this.SpecialVal();
        }

        private void TagVal()
        {
            foreach (ListItem li in ((DropDownList)CategoryStep.ContentTemplateContainer.
                FindControl("ddlCategory")).Items)
            {
                if (PubEntAdminManager.OtherVal(li.Text) || PubEntAdminManager.OtherVal(li.Value))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }
        }

        private void SpecialVal()
        {
            foreach (ListItem li in ((DropDownList)CategoryStep.ContentTemplateContainer.
                FindControl("ddlCategory")).Items)
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

    }
}