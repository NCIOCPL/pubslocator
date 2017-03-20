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
using System.Collections.Generic;
using System.Xml.Linq;
using PubEntAdmin.DAL;

namespace PubEntAdmin.UserControl
{
    public partial class EditSubsubcat : System.Web.UI.UserControl
    {
        #region EventHandler
        public event EventHandler BubbleSaveEditSubsubcatClick;
        public event EventHandler BubbleCancelEditSubsubcatClick;
        public event EventHandler BubbleDelEditSubsubcatClick;
        #endregion

        #region Events Handling
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.BindOptions();
            }

            this.SecVal();
        }

        /**********************************************************************/
        protected void btnAdd_EditSubsubcat_Click(object sender, EventArgs e)
        {
            OnBubbleSaveEditSubsubcatClick(e);
        }

        protected void btnCancel_EditSubsubcat_Click(object sender, EventArgs e)
        {
            OnBubbleCancelEditSubsubcatClick(e);
        }

        protected void btnDel_EditSubsubcat_Click(object sender, EventArgs e)
        {
            OnBubbleDelEditSubsubcatClick(e);
        }

        #region Event Handler b
        /**********************************************************************/
        protected void OnBubbleSaveEditSubsubcatClick(EventArgs e)
        {
            if (BubbleSaveEditSubsubcatClick != null)
            {
                BubbleSaveEditSubsubcatClick(this, e);
            }
        }

        protected void OnBubbleCancelEditSubsubcatClick(EventArgs e)
        {
            if (BubbleCancelEditSubsubcatClick != null)
            {
                BubbleCancelEditSubsubcatClick(this, e);
            }
        }

        protected void OnBubbleDelEditSubsubcatClick(EventArgs e)
        {
            if (BubbleDelEditSubsubcatClick != null)
            {
                BubbleDelEditSubsubcatClick(this, e);
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

        public int SubSubCatID
        {
            set { ViewState["intSubSubCatID"] = value; }
            get { return System.Convert.ToInt32(ViewState["intSubSubCatID"].ToString()); }
        }

        public string SubSubCatName
        {

            get { return this.txtSubsubcatDes.Text.Trim(); }
        }

        public int AssignToSubCatID
        {
            get { return System.Convert.ToInt32(this.ddlSubCategory.SelectedValue); }
        }

        public string SubSubCatTextClientID
        {
            get
            {
                return this.txtSubsubcatDes.ClientID;
            }
        }
        #endregion

        #region Methods
        protected void BindOptions()
        {
            this.ddlSubCategory.DataSource = PE_DAL.GetAllCatalogSubcatHvSubSubcat(true);
            this.ddlSubCategory.DataTextField = "SubCatName";
            this.ddlSubCategory.DataValueField = "SubCatID";
            this.ddlSubCategory.DataBind();
            this.ddlSubCategory.Items.Insert(0, new ListItem("[Select Subcategory]", "0"));
        }

        public void BindValues(string SubCatID, string Subsubcatname)
        {
            this.ddlSubCategory.SelectedIndex = this.ddlSubCategory.Items.
                IndexOf(this.ddlSubCategory.Items.FindByValue(SubCatID));
            this.SubCatID = System.Convert.ToInt32(SubCatID);
            this.txtSubsubcatDes.Text = Subsubcatname;
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
            if ((!PubEntAdminManager.LenVal(this.txtSubsubcatDes.Text, 100)))
            {
                Response.Redirect("InvalidInput.aspx");
            }
        }

        private void TagVal()
        {
            if ((PubEntAdminManager.OtherVal(this.txtSubsubcatDes.Text)))
            {
                Response.Redirect("InvalidInput.aspx");
            }

            foreach (ListItem li in this.ddlSubCategory.Items)
            {
                if (PubEntAdminManager.OtherVal(li.Text) || PubEntAdminManager.OtherVal(li.Value))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }
        }

        private void SpecialVal()
        {
            if ((PubEntAdminManager.SpecialVal2(this.txtSubsubcatDes.Text.Replace(" ", ""))))
            {
                Response.Redirect("InvalidInput.aspx");
            }

            foreach (ListItem li in this.ddlSubCategory.Items)
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