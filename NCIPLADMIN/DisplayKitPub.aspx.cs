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
using GlobalUtils;
using PubEntAdmin.BLL;
using PubEntAdmin.DAL;
using PubEntAdmin.UserControl;

namespace PubEntAdmin
{
    public partial class DisplayKitPub : System.Web.UI.Page
    {
        #region Constants
        private readonly string PUBTYPE = "PUBTYPE";
        private readonly string ChildPage = "KitPub.aspx";
        private readonly string DisplayKitPubErrorMsg = "DisplayKitPubErrorMsg";
        #endregion

        #region Controls
        protected UrlBuilder myUrlBuilder = new UrlBuilder(HttpContext.Current.Request.Url.AbsoluteUri, new Base64Encoder());
        protected bool blnIsVK;
        #endregion

        #region Events Handling
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session.IsNewSession)//cross-site request forgery
            {
                Response.Redirect("Home.aspx");
            }


            if (!((CustomPrincipal)Context.User).IsInRole(PubEntAdminManager.AdminRole))
            {
                PubEntAdminManager.UnathorizedAccess();
            }

            if (Session[DisplayKitPubErrorMsg] != null)
            {
                this.lblErrmsg.Text = Session[DisplayKitPubErrorMsg].ToString();
                Session.Remove(DisplayKitPubErrorMsg);
            }

            string type = "";
            if (PubEntAdminManager.TamperProof)
            {
                if (myUrlBuilder.QueryString.ContainsKey(PubEntAdminManager.strVK_LPType))
                {
                    type = this.myUrlBuilder.QueryString[PubEntAdminManager.strVK_LPType];
                }
                //else
                //{
                //    PubEntAdminManager.UnathorizedAccess();
                //}
            }
            else
            {
                if (Request.QueryString[PubEntAdminManager.strVK_LPType] != null)
                {
                    type = Request.QueryString[PubEntAdminManager.strVK_LPType].ToString();
                }
                //else
                //{
                //    PubEntAdminManager.UnathorizedAccess();
                //}
            }

            if (type == PubEntAdminManager.strVKType)
            {
                this.HiddenIsVK.Value = "1";
                this.lblInstruction.Text = @"To create a new Virtual Kit, type a Kit ID in the textbox and click Create New.<br />
                        To edit/delete an existing Virtual Kit, click the corresponding Edit/Delete link next to the Virtual Kit title";
                this.Title = "Virtual Kits Display";
                this.lblPageTitle.Text = "Virtual Kits";

                this.blnIsVK = true;
            }
            else if (type == PubEntAdminManager.strLPType)
            {
                this.HiddenIsVK.Value = "0";
                this.lblInstruction.Text = @"To create a new Linked Publication, type a Publication ID in the textbox and click Create New.<br />
                        To edit/delete an existing Linked Publication, click the corresponding Edit/Delete link next to the Linked Publication title";
                this.Title = "Linked Publication Display";
                this.lblPageTitle.Text = "Linked Publication";

                this.blnIsVK = false;
            }

            this.SecVal();

            System.Web.UI.UserControl l_uc;

            l_uc = (System.Web.UI.UserControl)this.LoadControl("UserControl/AdminMenu.ascx");
            this.plcHldMenu.Controls.Clear();
            this.plcHldMenu.Controls.Add(l_uc);

            l_uc = (System.Web.UI.UserControl)this.LoadControl("UserControl/KitPubList.ascx");
            l_uc.ID = "KitPubList";
            this.plcHldKits.Controls.Clear();
            this.plcHldKits.Controls.Add(l_uc);

            this.txtKitID.Attributes.Add("onchange", "ProIDInterfaceVal()");
            this.lstboxKitPubInt.Attributes.Add("onclick", "start(this)");
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            if (PubEntAdminManager.TamperProof)
            {
                if (this.IsVK)
                {
                    PubEntAdminManager.RedirectEncodedURLWithQS(ChildPage,
                        PubEntAdminManager.strProdID + "=" + this.newKitPubID + "&"+
                        PubEntAdminManager.strInterface + "=" + this.newInterfaceToString + "&"+
                        PubEntAdminManager.strVK_LPType + "=" + PubEntAdminManager.strVKType);
                }
                else{
                    PubEntAdminManager.RedirectEncodedURLWithQS(ChildPage,
                        PubEntAdminManager.strProdID + "=" + this.newKitPubID + "&" +
                        PubEntAdminManager.strInterface + "=" + this.newInterfaceToString + "&" +
                        PubEntAdminManager.strVK_LPType + "=" + PubEntAdminManager.strLPType);
                }
                
            }
            else
            {
                if (this.IsVK)
                {
                    Response.Redirect(ChildPage + "?" +
                        PubEntAdminManager.strProdID + "=" + this.newKitPubID + "&" +
                        PubEntAdminManager.strInterface + "=" + this.newInterfaceToString + "&" + 
                        PubEntAdminManager.strVK_LPType + "=" + PubEntAdminManager.strVKType, true);
                }
                else
                {
                    Response.Redirect(ChildPage + "?" +
                        PubEntAdminManager.strProdID + "=" + this.newKitPubID + "&" +
                        PubEntAdminManager.strInterface + "=" + this.newInterfaceToString + "&" + 
                        PubEntAdminManager.strVK_LPType + "=" + PubEntAdminManager.strLPType, true);
                }
               
            }
        }
         
        #endregion

        #region Properties
        public string newKitPubID
        {
            get { return this.txtKitID.Text; }
        }

        public string newTitle
        {
            get { return this.lblPageTitle.Text; }
        }

        protected string newInterfaceToString
        {
            get
            {
                string ret = String.Empty;
                ListItemCollection l = this.newInterface;
                if (l.Count > 0)
                {
                    foreach (ListItem li in l)
                    {
                        if (ret.Length > 0)
                            ret += ",";
                        ret += li.Value;
                    }
                    return ret;
                }
                else
                {
                    return String.Empty;
                }

            }
        }

        public ListItemCollection newInterface
        {
            get
            {
                ListItemCollection lcoll = new ListItemCollection();
                foreach (ListItem l in this.lstboxKitPubInt.Items)
                {
                    if (l.Selected)
                        lcoll.Add(l);
                }
                return lcoll;
            }
        }

        public string BtnCreateID
        {
            get { return this.btnCreate.ID; }
        }
        
        public bool IsVK
        {
            set { blnIsVK = value; }
            get { return blnIsVK; }
        }

        public bool IsLP
        {
            get { return !(IsVK); }
        }
        #endregion

        #region Methods

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
            if ((!PubEntAdminManager.LenVal(this.txtKitID.Text, 10)) ||
                (!PubEntAdminManager.LenVal(this.HiddenIsVK.Value, 1)) ||
                (!PubEntAdminManager.LenVal(this.HiddenVal.Value, 1)))
            {
                Response.Redirect("InvalidInput.aspx");
            }
        }

        private void TypeVal()
        {
            if (this.HiddenVal.Value.Trim().Length > 0)
            {
                if (!PubEntAdminManager.ContentVal(this.HiddenVal.Value.Trim(),@"^\d{1}$"))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            if (this.HiddenIsVK.Value.Trim().Length > 0)
            {
                if (!PubEntAdminManager.ContentVal(this.HiddenIsVK.Value.Trim(),@"^\d{1}$"))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }
        }

        private void TagVal()
        {
            if ((PubEntAdminManager.OtherVal(this.txtKitID.Text)) ||
                (PubEntAdminManager.OtherVal(this.HiddenIsVK.Value)) ||
                (PubEntAdminManager.OtherVal(this.HiddenVal.Value)))
            {
                Response.Redirect("InvalidInput.aspx");
            }

            foreach (ListItem li in this.lstboxKitPubInt.Items)
            {
                if (PubEntAdminManager.OtherVal(li.Text) || PubEntAdminManager.OtherVal(li.Value))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

        }

        private void SpecialVal()
        {

            if ((PubEntAdminManager.SpecialVal2(this.txtKitID.Text.Replace(" ", ""))) ||
                (PubEntAdminManager.SpecialVal2(this.HiddenIsVK.Value.Replace(" ", ""))) ||
                (PubEntAdminManager.SpecialVal2(this.HiddenVal.Value.Replace(" ", ""))))
            {
                Response.Redirect("InvalidInput.aspx");
            }

            foreach (ListItem li in lstboxKitPubInt.Items)
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
