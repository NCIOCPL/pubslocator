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


namespace PubEntAdmin.UserControl
{
    public partial class EditSubcat : System.Web.UI.UserControl
    {
        #region EventHandler
        public event EventHandler BubbleSaveEditSubcatClick;
        public event EventHandler BubbleCancelEditSubcatClick;
        public event EventHandler BubbleDelEditSubcatClick;

        public event EventHandler BubbleAddSubsubcatClick;
        public event EventHandler BubbleEditSubsubcatClick;
        #endregion

        #region Events Handling
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (PubEntAdminManager.TamperProof)
                {
                    if (!PubEntAdminManager.ContainURLQS("mode"))
                    {
                        this.BindOptions();
                    }

                }
                else
                {
                    if (Request.QueryString["mode"] == null)
                    {
                        this.BindOptions();
                    }
                }
                
            }

            this.SecVal();
        }

        /**********************************************************************/
        protected void btnAdd_EditSubcat_Click(object sender, EventArgs e)
        {
            OnBubbleSaveEditSubcatClick(e);
        }

        protected void btnCancel_EditSubcat_Click(object sender, EventArgs e)
        {
            OnBubbleCancelEditSubcatClick(e);
        }

        protected void btnDel_EditSubcat_Click(object sender, EventArgs e)
        {
            OnBubbleDelEditSubcatClick(e);
        }

        protected void btnEditSubsubcat_Click(object sender, EventArgs e)
        {
            OnBubbleEditSubsubcatClick(e);
        }

        protected void btnAddSubsubcat_Click(object sender, EventArgs e)
        {
            OnBubbleAddSubsubcatClick(e);
        }

        #region Event Handler b
        /**********************************************************************/
        protected void OnBubbleSaveEditSubcatClick(EventArgs e)
        {
            if (BubbleSaveEditSubcatClick != null)
            {
                BubbleSaveEditSubcatClick(this, e);
            }
        }

        protected void OnBubbleCancelEditSubcatClick(EventArgs e)
        {
            if (BubbleCancelEditSubcatClick != null)
            {
                BubbleCancelEditSubcatClick(this, e);
            }
        }

        protected void OnBubbleDelEditSubcatClick(EventArgs e)
        {
            if (BubbleDelEditSubcatClick != null)
            {
                BubbleDelEditSubcatClick(this, e);
            }
        }

        protected void OnBubbleEditSubsubcatClick(EventArgs e)
        {
            if (BubbleEditSubsubcatClick != null)
            {
                BubbleEditSubsubcatClick(this, e);
            }
        }

        protected void OnBubbleAddSubsubcatClick(EventArgs e)
        {
            if (BubbleAddSubsubcatClick != null)
            {
                BubbleAddSubsubcatClick(this, e);
            }
        }
        #endregion

        #endregion

        #region Properties
        public int SubCatID
        {
            set { ViewState["intSubCatID"] = value; }
            get { return System.Convert.ToInt32(ViewState["intSubCatID"].ToString()); }
        }

        public string SubCatName
        {
            
            get { return this.txtSubcatDes.Text.Trim(); }
        }

        public int AssignToSubjID
        {
            get { return System.Convert.ToInt32(this.ddlCategory.SelectedValue); }
        }

        public bool AllowHvSubSubCat
        {
            get { return this.ckboxAllowSubsubcat.Checked; }
        }

        public string SubCatTextClientID
        {
            get
            {
                return this.txtSubcatDes.ClientID;
            }
        } 
        #endregion

        #region Methods
        public void BindOptions()
        {
            this.ddlCategory.DataSource = PE_DAL.GetAllSubjectHvSubcat(true);
            this.ddlCategory.DataTextField = "SubjName";
            this.ddlCategory.DataValueField = "SubjID";
            this.ddlCategory.DataBind();
            this.ddlCategory.Items.Insert(0, new ListItem("[Select Category]", "0"));
        }

        public void BindValues()
        {
            List<PubEntAdmin.BLL.SubCat> l_subcatcoll = PE_DAL.GetSubCatBySubCatID(this.SubCatID);

            if (l_subcatcoll.Count > 0)
            {
                PubEntAdmin.BLL.SubCat l_subcat = l_subcatcoll[0];

                this.txtSubcatDes.Text = l_subcat.SubCatName;
                this.ddlCategory.SelectedIndex = this.ddlCategory.Items.
                    IndexOf(this.ddlCategory.Items.FindByValue(l_subcat.AssignedCatID.ToString()));
                this.ckboxAllowSubsubcat.Checked = l_subcat.AllowHaveSubSubcat;
                if (this.ckboxAllowSubsubcat.Checked)
                {
                    this.AssignClientScript();
                }

                if (l_subcat.HvHowManySubSub > 0)
                {
                    this.btnAddSubsubcat.Visible = false;
                    this.lblAlert.Text = "";
                    this.btnEditSubsubcat.Visible = true;
                    
                }
                else
                {
                    this.btnAddSubsubcat.Visible = true;
                    this.lblAlert.Text = "No SubSubCategory has been created for this Subcategory. Click [Add SubSubCat] button to add a new one.";
                    this.btnEditSubsubcat.Visible = false;
                }
            }
        }

        public void AssignClientScript()
        {
            this.ckboxAllowSubsubcat.Attributes.Add("onclick", @"if((!this.checked)&&(uncheckAllowHsSubSubReminder==1))
                {uncheckAllowHsSubSubReminder++;alert('NOTE:\nALL the SubSubCategories assigned\nto this SubCategory will be deleted!')}");
        }

        #region Sec Val
        public void SecVal()
        {
            this.LenVal();
            this.TagVal();
            this.SpecialVal();
        }

        private void LenVal()
        {
            if ((!PubEntAdminManager.LenVal(this.txtSubcatDes.Text, 100)))
            {
                Response.Redirect("InvalidInput.aspx");
            }
        }

        private void TagVal()
        {
            if ((PubEntAdminManager.OtherVal(this.txtSubcatDes.Text)))
            {
                Response.Redirect("InvalidInput.aspx");
            }

            foreach (ListItem li in this.ddlCategory.Items)
            {
                if (PubEntAdminManager.OtherVal(li.Text) || PubEntAdminManager.OtherVal(li.Value))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }
        }

        private void SpecialVal()
        {
            if ((PubEntAdminManager.SpecialVal2(this.txtSubcatDes.Text.Replace(" ", ""))))
            {
                Response.Redirect("InvalidInput.aspx");
            }

            foreach (ListItem li in this.ddlCategory.Items)
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