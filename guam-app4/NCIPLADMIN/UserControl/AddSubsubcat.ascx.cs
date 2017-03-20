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

namespace PubEntAdmin.UserControl
{
    public partial class AddSubsubcat : System.Web.UI.UserControl
    {
        #region Event Handler
        public event EventHandler BubbleSaveSubsubcatClick;
        public event EventHandler BubbleCancelSubsubcatClick;
        #endregion

        #region Events Handling
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.SubSubCatMode.Length > 0)
            {
                this.ddlSubCategory.Visible = false;
                this.lblAssign_to_Subcategory.Visible = false;
            }

            if (!Page.IsPostBack)
            {
                
                this.BindOptions();
            }

            this.SecVal();
        }

        protected void btnAddSubsubcat_Click(object sender, EventArgs e)
        {
            OnBubbleSaveSubsubcatClick(e);
        }

        protected void btnCancelSubsubcat_Click(object sender, EventArgs e)
        {
            OnBubbleCancelSubsubcatClick(e);
        }

        #region Events Handling b
        protected void OnBubbleSaveSubsubcatClick(EventArgs e)
        {
            if (BubbleSaveSubsubcatClick != null)
            {
                BubbleSaveSubsubcatClick(this, e);
            }
        }

        protected void OnBubbleCancelSubsubcatClick(EventArgs e)
        {
            if (BubbleCancelSubsubcatClick != null)
            {
                BubbleCancelSubsubcatClick(this, e);
            }
        }
        #endregion

        #endregion

        #region Methods
        public void BindOptions()
        {
            this.ddlSubCategory.DataSource = PE_DAL.GetAllCatalogSubcatHvSubSubcat(true);
            this.ddlSubCategory.DataTextField = "SubCatName";
            this.ddlSubCategory.DataValueField = "SubCatID";
            this.ddlSubCategory.DataBind();
            this.ddlSubCategory.Items.Insert(0, new ListItem("[Select Subcategory]", "0"));
        }

        public void ClearAll()
        {
            this.txtSubSubcatDes.Text = String.Empty;
            this.ddlSubCategory.SelectedIndex = 0;
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
            if ((!PubEntAdminManager.LenVal(this.txtSubSubcatDes.Text, 100)))
            {
                Response.Redirect("InvalidInput.aspx");
            }
        }

        private void TagVal()
        {
            if ((PubEntAdminManager.OtherVal(this.txtSubSubcatDes.Text)))
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
            if ((PubEntAdminManager.SpecialVal2(this.txtSubSubcatDes.Text.Replace(" ", ""))))
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

        #region Properties
        public int SubSubCatID
        {
            set { ViewState["intSubSubCatID"] = value; }
            get
            {
                if (ViewState["intSubSubCatID"] != null)
                    return System.Convert.ToInt32(ViewState["intSubSubCatID"].ToString());
                else
                    return 0;
            }
        }

        public string SubSubCatName
        {
            get { return this.txtSubSubcatDes.Text.Trim(); }
        }

        public int AssignToSubCatID
        {
            get { return System.Convert.ToInt32(this.ddlSubCategory.SelectedValue.ToString()); }
        }

        public string SubsubcatTextClientID
        {
            get
            {
                return this.txtSubSubcatDes.ClientID;
            }
        }

        public string SubSubCatMode
        {
            set { ViewState["strSubSubCatMode"] = value; }
            get
            {
                if (ViewState["strSubSubCatMode"] != null)
                    return (ViewState["strSubSubCatMode"].ToString());
                else
                    return String.Empty;
            }
        }
        #endregion

    }
}