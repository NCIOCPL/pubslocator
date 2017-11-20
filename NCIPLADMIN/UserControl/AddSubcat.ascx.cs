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

namespace PubEntAdmin.UserControl
{
    public partial class AddSubcat : System.Web.UI.UserControl
    {
        #region EventHandler
        public event EventHandler BubbleSaveSubcatClick;
        public event EventHandler BubbleCancelSubcatClick;
        #endregion

        #region Events Handling

        protected void Page_Load(object sender, EventArgs e)
        {
            this.SecVal();
        }

        protected void btnAddSubcat_Click(object sender, EventArgs e)
        {
            OnBubbleSaveSubcatClick(e);
        }

        protected void btnCancelSubcat_Click(object sender, EventArgs e)
        {
            OnBubbleCancelSubcatClick(e);
        }

        #region Event Handler b
        /**********************************************************************/
        protected void OnBubbleSaveSubcatClick(EventArgs e)
        {
            if (BubbleSaveSubcatClick != null)
            {
                BubbleSaveSubcatClick(this, e);
            }
        }

        protected void OnBubbleCancelSubcatClick(EventArgs e)
        {
            if (BubbleCancelSubcatClick != null)
            {
                BubbleCancelSubcatClick(this, e);
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
            set { ViewState["intAssignToSubjID"] = value; }
            get { return System.Convert.ToInt32(ViewState["intAssignToSubjID"].ToString()); }
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

        public void ClearAll()
        {
            this.txtSubcatDes.Text = String.Empty;
            this.ckboxAllowSubsubcat.Checked = false;
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
        }

        private void SpecialVal()
        {
            if ((PubEntAdminManager.SpecialVal2(this.txtSubcatDes.Text.Replace(" ", ""))))
            {
                Response.Redirect("InvalidInput.aspx");
            }
        }
        #endregion

        #endregion
    }
}