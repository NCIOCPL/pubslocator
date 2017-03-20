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
    public partial class TranslationTabEditInfo : System.Web.UI.UserControl
    {
        #region Fields
        private int intPubID;
        //private bool blnInitialAddLoad;
        #endregion

        #region Events Handling
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.PubID > 0)
            {
                this.BindData();
            }
            else
            {
                this.lblErrRelTranslation.Text = "This publication has not been created.  Please add any publication translation info after creating this publication.";
            }
            this.RegisterMonitoredChanges();

            this.SecVal();
            if (((string)Session[PubEntAdminManager.strPubGlobalMode] ==
                   PubEntAdminManager.strPubGlobalAMode) ||
                ((string)Session[PubEntAdminManager.strPubGlobalMode] ==
                   PubEntAdminManager.strPubGlobalVMode))
                this.trTranslation.Visible = false;
            else
                this.trTranslation.Visible = true;
        }

        protected void gvResult_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandSource is LinkButton)
            {
                LinkButton l_lnkbtn_del = e.CommandSource as LinkButton;
                if (l_lnkbtn_del.Text.ToLower() == "delete")
                {
                    //pub_id
                    int S_PubID = System.Convert.ToInt32(((System.Web.UI.WebControls.TableRow)(e.Item)).Cells[0].Text);
                    int ret = PE_DAL.DeleteRelatedTranslation(this.PubID, S_PubID);

                    if (ret > 0)
                    {
                        this.ReBindData();
                    }
                }
            }
        }

        protected void gvResult_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            PubCollection dt = ((PubCollection)this.gvResult.DataSource);

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Pub l_pub = dt[e.Item.ItemIndex];
                ////delete btn col
                Panel l_pnl = e.Item.Cells[4].Controls[1] as Panel;
                ((Label)l_pnl.Controls[1]).Text = "Are you sure you want to delete this translation<br />[" + l_pub.ProdID + @" - " + l_pub.Name + "]?";
            }
        }

        protected void btnTranslationTabInfo_Click(object sender, EventArgs e)
        {
            if (this.PubID > 0)
            {
                if (this.txtTranslation.Text.Trim().Length != 0)
                {
                    int ret = PE_DAL.SetRelatedTranslation(this.PubID, this.txtTranslation.Text.Trim());
                    if (ret == 0)
                    {
                        this.lblErrRelTranslation.Text = "The Publication ID you provide is invalid. Please make sure this Publication exists.";
                    }
                    else
                    {
                        this.txtTranslation.Text = String.Empty;
                        this.lblErrRelTranslation.Text = "";
                        Session[PubEntAdminManager.strReloadRelatedTranslation] = "true";
                    }
                }
            }
           
        }
        #endregion

        #region Methods

        public void ReBindData()
        {
            this.BindData();
        }

        protected void BindData()
        {
            if (this.PubID > 0)
            {
                PubCollection p = PE_DAL.GetTranslationDisplay(this.PubID);

                if (p.Count > 0)
                {
                    this.gvResult.DataSource = p;
                    this.gvResult.DataBind();
                }
                else
                {
                    this.gvResult.Visible = false;
                    this.lblErrRelTranslation.Text = "This publication does not have any other translation recorded.";
                }
            }

        }

        protected bool TranslateSave()
        {
            bool oret = false;
            if (this.PubID > 0)
            {
                if (this.txtTranslation.Text.Trim().Length != 0)
                {
                    int ret = PE_DAL.SetRelatedTranslation(this.PubID, this.txtTranslation.Text.Trim());
                    if (ret == 0)
                    {
                        this.lblErrRelTranslation.Text = "The Publication ID you provide is invalid. Please make sure this Publication exists.";
                    }
                    else
                    {
                        this.txtTranslation.Text = String.Empty;
                        this.lblErrRelTranslation.Text = "";
                        Session[PubEntAdminManager.strReloadRelatedTranslation] = "true";
                        oret = true;
                    }
                }
                else
                    oret = true;
            }

            return oret;

        }

        public bool Save()
        {
            this.SecVal();
            return this.TranslateSave();
        }

        #region MonitorChanges
        protected void RegisterMonitoredChanges()
        {

            PubEntAdminManager.MonitorChanges2(this.Page, this, this.txtTranslation);

        }
        #endregion

        #region Sec Val
        private void SecVal()
        {
            this.LenVal();
            this.TypeVal();
            this.TagVal();
            this.SpecialVal();
        }

        private void LenVal()
        {
            if ((!PubEntAdminManager.LenVal(this.txtTranslation.Text, 10)))
            {
                Response.Redirect("InvalidInput.aspx");
            }
        }

        private void TypeVal()
        {
            if (this.txtTranslation.Text.Trim().Length > 0)
            {
                if (!PubEntAdminManager.ContentVal(this.txtTranslation.Text.Trim(), @"^[a-zA-Z0-9]{1,10}$"))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }
        }

        private void TagVal()
        {
            if ((PubEntAdminManager.OtherVal(this.txtTranslation.Text)))
            {
                Response.Redirect("InvalidInput.aspx");
            }
        }

        private void SpecialVal()
        {

            if ((PubEntAdminManager.SpecialVal2(this.txtTranslation.Text.Replace(" ", ""))))
            {
                Response.Redirect("InvalidInput.aspx");
            }
        }
        #endregion

        #endregion

        #region Properties
        //no need this in actual coding
        //public bool InitialAddLoad
        //{
        //    set
        //    {
        //        this.blnInitialAddLoad = value;
        //    }
        //    get
        //    {
        //        return this.blnInitialAddLoad;
        //    }
        //}

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