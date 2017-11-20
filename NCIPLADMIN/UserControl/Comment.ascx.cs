using System;
using System.IO;
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

using PubEntAdmin.BLL;
using PubEntAdmin.DAL;

namespace PubEntAdmin.UserControl
{
    public partial class Comment : System.Web.UI.UserControl
    {
        #region Fields
        private int intPubID;
        private string strMode;
        #endregion

        #region Events Handling
        protected void Page_Load(object sender, EventArgs e)
        {
            

            if (this.PubID>0)
                this.BindComments();
        }

        
        protected void Page_PreRender(object sender, System.EventArgs e)
        {
            #region ClientScript
            ScriptManager.RegisterStartupScript(this,
                typeof(CatalogTabEditInfo), "CatalogTabEditInfo_ClientScript",
            @"
                function TabParticipants()
                {
                    return '"+this.GetSpellCheckParticipants()+@"';
                }
            ", true);
            #endregion

            if (this.Mode != PubEntAdminManager.strPubGlobalVMode)
            {
                this.pnlTop.Visible = true;

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
                //this.RegisterMonitoredChanges();
                //this.ByPassRegisterMonitoredChanges();
            }
            else
            {
                this.pnlTop.Visible = false;
            }
        }

        protected void grdComments_ItemDataBound(Object s, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                PubEntAdmin.BLL.Comment currentComment = (PubEntAdmin.BLL.Comment)e.Item.DataItem;

                Label lblCreatorDisplayName = (Label)e.Item.FindControl("lblCreatorDisplayName");
                lblCreatorDisplayName.Text = currentComment.CreatorUsername;

                Label lblDateCreated = (Label)e.Item.FindControl("lblDateCreated");
                lblDateCreated.Text = currentComment.DateCreated.ToString("f");

                Literal ltlComment = (Literal)e.Item.FindControl("ltlComment");
                ltlComment.Text = currentComment.CommentContent;

                ltlComment.Text = Page.Server.HtmlEncode(ltlComment.Text.Trim().Replace("__CRLF__", "<br>"));
                //ltlComment.Text = Page.Server.HtmlEncode(ltlComment.Text.Trim());
                //ltlComment.Text = ltlComment.Text.Replace("__CRLF__", "<br>");

            }
        }

        public void Comment_Click(Object s, EventArgs e)
        {
            this.SaveThis();
        }
        #endregion

        #region Methods

        public bool Save()
        {
            this.SecVal();
            return this.SaveThis();
        }

        protected bool SaveThis()
        {
            bool ret = false;
            if (this.PubID > 0)
            {
                if (Page.IsValid && this.txtComment.Text.Trim().Length > 0)
                {
                    PubEntAdmin.BLL.Comment l_comment = new PubEntAdmin.BLL.Comment(
                        this.PubID, this.txtComment.Text.Trim().Replace(System.Environment.NewLine, "__CRLF__"),
                        ((CustomPrincipal)HttpContext.Current.User).UserID,
                        ((CustomPrincipal)HttpContext.Current.User).FullName);

                    ret = l_comment.Save();
                    txtComment.Text = String.Empty;
                }
                this.BindComments();
                this.lblErrmsg.Text = String.Empty;
            }
            else
            {
                this.txtComment.Text = String.Empty;
                this.lblErrmsg.Text = "This publication has not been created.  Please add any comment after creating this publication.";
            }
            return ret;
        }

        protected void BindComments()
        {
            if (this.PubID > 0)
            {
                this.grdComments.DataSource = PE_DAL.GetCommentsByPubID(this.PubID);
                this.grdComments.DataBind();
            }
        }

        public string GetSpellCheckParticipants()
        {
            return this.txtComment.TextCtrl_SpellCheckClentID;
        }

        #region MonitorChanges
        protected void RegisterMonitoredChanges()
        {
            PubEntAdminManager.MonitorChanges2(this.Page, this, this.txtComment.TextCtrl_SpellCheckInnerTxtbox);

        }

        protected void ByPassRegisterMonitoredChanges()
        {
            PubEntAdminManager.BypassModifiedMethod(this.btnAdd, false);
        }
        #endregion

        #region Sec Val
        private void SecVal()
        {
            this.LenVal();
            this.TagVal();
            this.SpecialVal();
        }

        private void LenVal()
        {
            if ((!PubEntAdminManager.LenVal(this.txtComment.Text, 300)))
            {
                Response.Redirect("InvalidInput.aspx");
            }
        }

        private void TagVal()
        {
            if ((PubEntAdminManager.OtherVal(this.txtComment.Text)))
            {
                Response.Redirect("InvalidInput.aspx");
            }
        }

        private void SpecialVal()
        {
            if ((PubEntAdminManager.SpecialVal2(this.txtComment.Text.Replace(" ", ""))))
            {
                Response.Redirect("InvalidInput.aspx");
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

        public string Mode
        {
            set { this.strMode = value; }
            get { return this.strMode; }
        }
        #endregion
    }
}