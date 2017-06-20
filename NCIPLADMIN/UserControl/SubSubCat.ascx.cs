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
    public partial class SubSubCat : System.Web.UI.UserControl
    {
        //private int intSubsubcatID;
        //private string strSubsubcatMode;

        #region Events Handling
        protected void Page_Load(object sender, EventArgs e)
        {
            
            this.hidSubSubCatIndex.Value = "";

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

            if (PubEntAdminManager.TamperProof)
            {
                if (PubEntAdminManager.ContainURLQS("RetSub"))
                    this.RetSub = (PubEntAdminManager.GetURLQS("RetSub"));

            }
            else
            {
                if (Request.QueryString["RetSub"] != null)
                {
                    this.RetSub = (Request.QueryString["RetSub"].ToString());
                }
            }

            if (!Page.IsPostBack)
            {
                this.SubsubcatWizard.ActiveStepIndex = 0;
                

                if (this.SubcatID > 0)
                {
                    if (this.SubcatMode == "add")
                    {
                        this.EnableSubsubcategoryAddTab();
                        AddSubsubcat l_AddSubsubcat = ((AddSubsubcat)((MultiView)SubsubcategoryStep.
                ContentTemplateContainer.FindControl("MultiView1")).Views[0].FindControl("AddSubsubcat1"));
                        l_AddSubsubcat.SubSubCatMode = this.SubcatMode;
                    }
                    else if (this.SubcatMode == "edit")
                    {
                        this.EnableSubsubcategoryEditTab();
                    }
                    
                    this.BindOptions(this.SubcatID);
                    ((Label)this.SubsubcategoryStep.ContentTemplateContainer.
                    FindControl("lblCat_SubcatStep")).Text = Session[PubEntAdminManager.strCurrCatName].ToString() +
                    " > " + Session[PubEntAdminManager.strCurrSubcatName].ToString();
                }
                else
                {
                    this.EnableSubsubcategoryEditTab();
                    this.BindOptions();
                    ((Label)this.SubsubcategoryStep.ContentTemplateContainer.
                    FindControl("lblCat_SubcatStep")).Text = "Categories > Subcategories";
                }

            }

            List<TabItem> l = new List<TabItem>();
            TabItem t1 = new TabItem(0, "Add", "LookupMgmt.aspx");
            TabItem t2 = new TabItem(1, "Edit", "LookupMgmt.aspx");

            l.Add(t1);
            l.Add(t2);

            ((TabStrip)SubsubcategoryStep.ContentTemplateContainer.FindControl("TabStrip1")).TabSource = l;

            ((Button)SubsubcatWizard.FindControl("StepNavigationTemplateContainerID").FindControl("StepPreviousButton")).Visible = false;
            ((Button)SubsubcatWizard.FindControl("FinishNavigationTemplateContainerID").FindControl("FinishPreviousButton")).Visible = false;

            this.SecVal();
        }

        #region Tab Header
        protected void btnAddTab_Click(object sender, EventArgs args)
        {
            Session[PubEntAdminManager.strTabContentLUCurrActTabIndex] = 0;
        }

        protected void btnEditTab_Click(object sender, EventArgs args)
        {
            Session[PubEntAdminManager.strTabContentLUCurrActTabIndex] = 1;
        }
        #endregion

        protected void OnNext(object sender, WizardNavigationEventArgs e)
        {
            if (this.SubsubcatWizard.WizardSteps[e.CurrentStepIndex].ID == "SubsubcategoryStep")
            {
                SubsubcategoryStepSelectedIndexChanged(((DropDownList)SubsubcategoryStep.ContentTemplateContainer.
                FindControl("ddlSubSubCategory")), new EventArgs());
                ((Button)SubsubcatWizard.FindControl("FinishNavigationTemplateContainerID").FindControl("FinishButton")).Visible = false;
            }
        }

        protected void OnActiveStepChanged(object sender, EventArgs e)
        {
            if (this.SubsubcatWizard.ActiveStepIndex == 1)
            {
                if (((DropDownList)((MultiView)SubsubcategoryStep.
                ContentTemplateContainer.FindControl("MultiView1")).Views[1].
                FindControl("ddlSubSubCategory")).SelectedIndex == 0)
                {
                        ((Label)((MultiView)SubsubcategoryStep.
                    ContentTemplateContainer.FindControl("MultiView1")).Views[1].
                    FindControl("errMsg")).Text = 
                            "You must select one Subsubcategory to proceed.";
                    this.DecrementWizardPage();
                }
                else
                {
                        ((Label)((MultiView)SubsubcategoryStep.
                    ContentTemplateContainer.FindControl("MultiView1")).Views[1].
                    FindControl("errMsg")).Text = String.Empty;
                }
            }
        }

        protected void SubsubcategoryStepSelectedIndexChanged(object sender, EventArgs e)
        {
            List<PubEntAdmin.BLL.SubSubCat> l_SubSubCatColl = 
                PE_DAL.GetSubSubCatalogSubjectBySubSubCatalogID(
                System.Convert.ToInt32(((DropDownList)sender).SelectedValue));

            if (l_SubSubCatColl.Count > 0)
            {
                PubEntAdmin.BLL.SubSubCat l_SubSubCat = l_SubSubCatColl[0];

                ((Label)SubsubcategoryEditStep.ContentTemplateContainer.
                    FindControl("lblcategoryStep")).Text = l_SubSubCat.SubName;
                ((Label)SubsubcategoryEditStep.ContentTemplateContainer.
                    FindControl("lblsubcategoryStep")).Text = l_SubSubCat.SubCatName;

                AddSubsubcat l_AddSubsubcat = ((AddSubsubcat)((MultiView)SubsubcategoryStep.
                ContentTemplateContainer.FindControl("MultiView1")).Views[0].FindControl("AddSubsubcat1"));
                l_AddSubsubcat.ClearAll();

                EditSubsubcat l_EditSubsubcat = ((EditSubsubcat)SubsubcategoryEditStep.ContentTemplateContainer.
                    FindControl("EditSubsubcat1"));
                l_EditSubsubcat.SubSubCatID = this.SubSubCatID = l_SubSubCat.SubSubCatID;
                this.SubSubCatName = l_SubSubCat.SubSubCatName;
                l_EditSubsubcat.BindValues(l_SubSubCat.SubCatID.ToString(), l_SubSubCat.SubSubCatName);
            }
            //this.IncrementWizardPage();
            this.EnableSubsubcategoryEditTab();
        }

        #region AddSubsubcat
        protected void AddSubsubcat1Save_Click(object sender, EventArgs e)
        {
            AddSubsubcat l_AddSubsubcat = ((AddSubsubcat)((MultiView)SubsubcategoryStep.
                ContentTemplateContainer.FindControl("MultiView1")).Views[0].FindControl("AddSubsubcat1"));

            l_AddSubsubcat.SecVal();

            int l_subsubcatid = 0;
            bool blnSaveSubSubCat = false;
            
            if (this.SubcatID>0 && this.SubcatMode.Length >= 0)
                blnSaveSubSubCat = PE_DAL.SetSubSubCat(ref l_subsubcatid, l_AddSubsubcat.SubSubCatName,
                 this.SubcatID, 1);
            else
                blnSaveSubSubCat = PE_DAL.SetSubSubCat(ref l_subsubcatid, l_AddSubsubcat.SubSubCatName,
                 l_AddSubsubcat.AssignToSubCatID, 1);

            if (blnSaveSubSubCat)
            {
                if (PubEntAdminManager.TamperProof)
                {
                    PubEntAdminManager.RedirectEncodedURLWithQS("LookupMgmt.aspx", "sub=" + this.RetSub + "&mode=edit&subcatid=" + this.SubcatID);
                }
                else
                {
                    Response.Redirect("~/LookupMgmt.aspx?sub=" + this.RetSub + "&mode=edit&subcatid=" + this.SubcatID, true);
                }

                //l_AddSubsubcat.ClearAll();
                //this.EnableSubsubcategoryEditTab();
                //if (this.SubcatID > 0)
                //{
                //    this.BindOptions(this.SubcatID);
                //}
                //else
                //{
                //    this.BindOptions();
                //}
                //this.hidSubSubCatIndex.Value = "1";
            }
        }

        protected void AddNewSubsubcat1Cancel_Click(object sender, EventArgs e)
        {
            DropDownList l_SubSubCategory = ((DropDownList)((MultiView)SubsubcategoryStep.
                ContentTemplateContainer.FindControl("MultiView1")).Views[1].FindControl("ddlSubSubCategory"));
            l_SubSubCategory.SelectedIndex = 0;

            this.EnableSubsubcategoryEditTab();
            this.hidSubSubCatIndex.Value = "1";
        }
        #endregion

        #region EditSubsubcat
        protected void EditSubsubcat1Save_Click(object sender, EventArgs e)
        {
            EditSubsubcat l_EditSubsubcat = ((EditSubsubcat)SubsubcategoryEditStep.ContentTemplateContainer.
                FindControl("EditSubsubcat1"));

            l_EditSubsubcat.SecVal();

            int l_subsubcatid = l_EditSubsubcat.SubSubCatID;
            bool blnSaveSubSubCat = PE_DAL.SetSubSubCat(ref l_subsubcatid, l_EditSubsubcat.SubSubCatName,
                l_EditSubsubcat.AssignToSubCatID, 1);

            if (blnSaveSubSubCat)
            {
                this.DecrementWizardPage();
                this.EnableSubsubcategoryEditTab();
                this.hidSubSubCatIndex.Value = "1";
                if (this.SubcatID > 0)
                {
                    this.BindOptions(this.SubcatID);
                }
                else
                {
                    this.BindOptions();
                }
            }
        }

        protected void EditSubsubcat1Cancel_Click(object sender, EventArgs e)
        {
            EditSubsubcat l_EditSubsubcat = ((EditSubsubcat)SubsubcategoryEditStep.ContentTemplateContainer.
                FindControl("EditSubsubcat1"));
            l_EditSubsubcat.BindValues(this.SubSubCatID.ToString(), this.SubSubCatName);

            DropDownList l_SubSubCategory = ((DropDownList)((MultiView)SubsubcategoryStep.
                ContentTemplateContainer.FindControl("MultiView1")).Views[1].FindControl("ddlSubSubCategory"));
            l_SubSubCategory.SelectedIndex = 0;

            this.DecrementWizardPage();
            this.EnableSubsubcategoryEditTab();
            this.hidSubSubCatIndex.Value = "1";
        }

        protected void EditSubsubcat1Del_Click(object sender, EventArgs e)
        {
            EditSubsubcat l_EditSubsubcat = ((EditSubsubcat)SubsubcategoryEditStep.ContentTemplateContainer.
                FindControl("EditSubsubcat1"));

            int l_subsubcatid = l_EditSubsubcat.SubSubCatID;
            bool blnSaveSubSubCat = PE_DAL.SetSubSubCat(ref l_subsubcatid, l_EditSubsubcat.SubSubCatName,
                l_EditSubsubcat.AssignToSubCatID, 0);

            if (blnSaveSubSubCat)
            {
                this.DecrementWizardPage();
                this.EnableSubsubcategoryEditTab();
                this.hidSubSubCatIndex.Value = "1";
                if (this.SubcatID > 0)
                {
                    this.BindOptions(this.SubcatID);
                }
                else
                {
                    this.BindOptions();
                }
            }
        }
        #endregion

        protected void lnkSubsubategories_Click(object sender, EventArgs e)
        {
            EditSubsubcat l_EditSubsubcat = ((EditSubsubcat)SubsubcategoryEditStep.ContentTemplateContainer.
                FindControl("EditSubsubcat1"));
            l_EditSubsubcat.BindValues(this.SubSubCatID.ToString(), this.SubSubCatName);

            DropDownList l_SubCategory = ((DropDownList)((MultiView)SubsubcategoryStep.
                ContentTemplateContainer.FindControl("MultiView1")).Views[1].FindControl("ddlSubSubCategory"));
            l_SubCategory.SelectedIndex = 0;
            this.DecrementWizardPage();
            this.EnableSubsubcategoryEditTab();
            this.hidSubSubCatIndex.Value = "1";
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ((LookupMgmt)this.Page).ShowHideSpellChecker(false);

//            #region ClientScript
//            Page.ClientScript.RegisterClientScriptBlock(this.GetType(),
//            "Subsubcat_ClientScript",
//            @"
//                    function LkupParticipants()
//                    {
//                        return '" + this.GetSpellCheckParticipants() + @"';
//                    }
//                ", true);
//            #endregion
        }

        protected void TabStrip1_SelectionChanged(object sender, TabStrip.SelectionChangedEventArgs e)
        {
            if (e.TabNameSelected == "Add")
            {
                ((MultiView)SubsubcategoryStep.ContentTemplateContainer.FindControl("MultiView1")).ActiveViewIndex = 0;
            }
            else
            {
                ((MultiView)SubsubcategoryStep.ContentTemplateContainer.FindControl("MultiView1")).ActiveViewIndex = 1;
            }
        }

        #endregion

        #region Methods
        protected void BindOptions()
        {
            DropDownList l_ddlSubSubCategory = ((DropDownList)((MultiView)SubsubcategoryStep.
                ContentTemplateContainer.FindControl("MultiView1")).Views[1].FindControl("ddlSubSubCategory"));

            l_ddlSubSubCategory.DataSource = PE_DAL.GetAllSubSubCat(true);
            l_ddlSubSubCategory.DataTextField = "SubSubCatName";
            l_ddlSubSubCategory.DataValueField = "SubSubCatID";
            l_ddlSubSubCategory.DataBind();
            l_ddlSubSubCategory.Items.Insert(0, new ListItem("[Select Subsubcategory]", "0"));
        }

        protected void BindOptions(int subcatid)
        {
            DropDownList l_ddlSubSubCategory = ((DropDownList)((MultiView)SubsubcategoryStep.
                ContentTemplateContainer.FindControl("MultiView1")).Views[1].FindControl("ddlSubSubCategory"));

            List<PubEntAdmin.BLL.SubSubCat> l_SubSubCatcoll = PE_DAL.GetSubSubCatBySubID(subcatid);

            if (l_SubSubCatcoll.Count > 0)
            {
                l_ddlSubSubCategory.DataSource = l_SubSubCatcoll;
                l_ddlSubSubCategory.DataTextField = "SubSubCatName";
                l_ddlSubSubCategory.DataValueField = "SubSubCatID";
                l_ddlSubSubCategory.DataBind();
                l_ddlSubSubCategory.Items.Insert(0, new ListItem("[Select Subsubcategory]", "0"));
                this.EnableSubsubcategoryEditTab();
            }

        }

        protected void EnableSubsubcategoryEditTab()
        {
            ((MultiView)SubsubcategoryStep.ContentTemplateContainer.FindControl("MultiView1")).ActiveViewIndex = 1;
        }

        protected void EnableSubsubcategoryAddTab()
        {
            ((MultiView)SubsubcategoryStep.ContentTemplateContainer.FindControl("MultiView1")).ActiveViewIndex = 0;
        }

        protected void IncrementWizardPage()
        {
            this.SubsubcatWizard.ActiveStepIndex = this.SubsubcatWizard.ActiveStepIndex + 1;
        }

        protected void DecrementWizardPage()
        {
            this.SubsubcatWizard.ActiveStepIndex = this.SubsubcatWizard.ActiveStepIndex - 1;
        }

        public string GetSpellCheckParticipants()
        {
            string partipants = "";

            if (this.SubsubcatWizard.ActiveStepIndex == 0)
            {
                AddSubsubcat l_AddSubsubcat = ((AddSubsubcat)((MultiView)SubsubcategoryStep.
                ContentTemplateContainer.FindControl("MultiView1")).Views[0].FindControl("AddSubsubcat1"));

                partipants = l_AddSubsubcat.SubsubcatTextClientID;
            }
            else if (this.SubsubcatWizard.ActiveStepIndex == 1)
            {
                EditSubsubcat l_EditSubsubcat = ((EditSubsubcat)(SubsubcategoryEditStep.
                ContentTemplateContainer.FindControl("EditSubsubcat1")));

                partipants = l_EditSubsubcat.SubSubCatTextClientID;
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
            foreach (ListItem li in ((DropDownList)((MultiView)SubsubcategoryStep.
                ContentTemplateContainer.FindControl("MultiView1")).Views[0].FindControl("ddlSubSubCategory")).Items)
            {
                if (PubEntAdminManager.OtherVal(li.Text) || PubEntAdminManager.OtherVal(li.Value))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }
        }

        private void SpecialVal()
        {
            foreach (ListItem li in ((DropDownList)((MultiView)SubsubcategoryStep.
                ContentTemplateContainer.FindControl("MultiView1")).Views[0].FindControl("ddlSubSubCategory")).Items)
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

        public int SubSubCatID
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

        public string SubSubCatName
        {
            set
            {
                ViewState["strSubsubcatName"] = value;
            }
            get
            {
                if (ViewState["strSubsubcatName"] != null)
                    return (ViewState["strSubsubcatName"].ToString());
                else
                    return String.Empty;
            }
        }

        public int SubcatID
        {
            set
            {
                ViewState["intSubcatID"] = value;
            }
            get
            {
                if (ViewState["intSubcatID"] != null)
                    return System.Convert.ToInt32(ViewState["intSubcatID"].ToString());
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

        protected string RetSub
        {
            set
            {
                ViewState["RetSub"] = value;
            }
            get
            {
                if (ViewState["RetSub"] != null)
                    return ViewState["RetSub"].ToString();
                else
                    return String.Empty;
            }
        }

        #endregion

        

    }
}