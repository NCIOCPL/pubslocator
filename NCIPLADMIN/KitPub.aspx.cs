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
using System.Text;
using PubEntAdmin.UserControl;
using PubEntAdmin.DAL;
using PubEntAdmin.BLL;
using GlobalUtils;
using AjaxControlToolkit;

namespace PubEntAdmin
{
    public partial class KitPub : System.Web.UI.Page
    {
        #region Constants
        private readonly string DisplayKitPubErrorMsg = "DisplayKitPubErrorMsg";
        private readonly string ParentPage = "DisplayKitPub.aspx";
        private readonly string PUBTYPE = "PUBTYPE";
        #endregion

        #region Controls
        protected UrlBuilder myUrlBuilder = new UrlBuilder(HttpContext.Current.Request.Url.AbsoluteUri, new Base64Encoder());
        protected string[] sarr;
        #endregion

        #region Fields
        protected int intKitID;
        protected bool blnNCIPL;
        protected bool blnROO;
        protected bool blnIsVK;
        #endregion

        #region Events Handling
        protected void Page_Load(object sender, EventArgs e)
        {
            if (1 == 2) // temp auth fix
            {
                PubEntAdminManager.UnathorizedAccess();
            }

            System.Web.UI.UserControl l_uc;

            l_uc = (System.Web.UI.UserControl)this.LoadControl("UserControl/AdminMenu.ascx");
            this.plcHldMenu.Controls.Clear();
            this.plcHldMenu.Controls.Add(l_uc);

            this.SecVal();

            if (PubEntAdminManager.TamperProof)
            {
                if (this.myUrlBuilder.QueryString.ContainsKey(PubEntAdminManager.strProdID))
                {
                    this.ProdID = (this.myUrlBuilder.QueryString[PubEntAdminManager.strProdID]);
                }

                if (this.myUrlBuilder.QueryString.ContainsKey(PubEntAdminManager.strPubID))
                {
                    this.KitID = System.Convert.ToInt32(this.myUrlBuilder.QueryString[PubEntAdminManager.strPubID]);
                }

                if (this.myUrlBuilder.QueryString.ContainsKey(PubEntAdminManager.strInterface))
                {
                    string s =this.myUrlBuilder.QueryString[PubEntAdminManager.strInterface];
                    if (s.IndexOf(',')<0)
                    {
                        this.IsNCIPL = (this.myUrlBuilder.QueryString[PubEntAdminManager.strInterface]) == PubEntAdminManager.NCIPL_INTERFACE ? true : false;
                        this.IsROO = (this.myUrlBuilder.QueryString[PubEntAdminManager.strInterface]) == PubEntAdminManager.ROO_INTERFACE ? true : false;
                    }
                    else{
                        sarr = s.Split(new char[]{','},StringSplitOptions.RemoveEmptyEntries);
                        foreach (string sstr in sarr)
                        {
                            if (!this.IsNCIPL)
                            {
                                this.IsNCIPL = ((sstr) == PubEntAdminManager.NCIPL_INTERFACE) ? true : false;
                            }

                            if (!this.IsROO)
                            {
                                this.IsROO = ((sstr) == PubEntAdminManager.ROO_INTERFACE) ? true : false;
                            }
                        }
                    }
                }

                if (this.myUrlBuilder.QueryString.ContainsKey(PubEntAdminManager.strVK_LPType))
                {
                    this.IsVK = (this.myUrlBuilder.QueryString[PubEntAdminManager.strVK_LPType]) == PubEntAdminManager.strVKType ? true : false;
                }
            }
            else
            {
                if (Request.QueryString[PubEntAdminManager.strProdID] != null)
                {
                    this.ProdID = (Request.QueryString[PubEntAdminManager.strProdID]);
                }

                if (Request.QueryString[PubEntAdminManager.strPubID] != null)
                {
                    this.KitID = System.Convert.ToInt32(Request.QueryString[PubEntAdminManager.strPubID]);
                }

                if (Request.QueryString[PubEntAdminManager.strInterface] != null)
                {
                    string s =Request.QueryString[PubEntAdminManager.strInterface];
                    if (s.IndexOf(',')<0)
                    {
                        this.IsNCIPL = (Request.QueryString[PubEntAdminManager.strInterface]) == PubEntAdminManager.NCIPL_INTERFACE ? true : false;
                        this.IsROO = (Request.QueryString[PubEntAdminManager.strInterface]) == PubEntAdminManager.ROO_INTERFACE ? true : false;

                        sarr = new string[1];

                        if (this.IsNCIPL)
                            sarr[0] = PubEntAdminManager.NCIPL_INTERFACE;
                        else if (this.IsROO)
                            sarr[0] = PubEntAdminManager.ROO_INTERFACE;
                    }
                    else{
                        sarr = s.Split(new char[]{','},StringSplitOptions.RemoveEmptyEntries);

                        foreach (string sstr in sarr)
                        {
                            if (!this.IsNCIPL)
                            {
                                this.IsNCIPL = ((sstr) == PubEntAdminManager.NCIPL_INTERFACE) ? true : false;
                            }

                            if (!this.IsROO)
                            {
                                this.IsROO = ((sstr) == PubEntAdminManager.ROO_INTERFACE) ? true : false;
                            }
                        }
                    }
                }

                if (Request.QueryString[PubEntAdminManager.strVK_LPType] != null)
                {
                    this.IsVK = (Request.QueryString[PubEntAdminManager.strVK_LPType]) == PubEntAdminManager.strVKType ? true : false;
                }
            }

            this.lblInstruction.Text = @"To add a publication to the " + (this.IsVK ? "Virtual Kit" : "Linked Publication") + @", enter the Publication ID then click Save.<br />To remove a publication from the " + (this.IsVK ? "Virtual Kit" : "Linked Publication") + @", select the check box next to the publication title.";

            if (!Page.IsPostBack)
            {
                if (Request.UrlReferrer == null || !Request.UrlReferrer.GetLeftPart(UriPartial.Path).
                    ToLower().Contains("displaykitpub.aspx"))
                {
                    throw new Exception(
                    "Must be invoked by DisplayKitPub.aspx");
                }
                else
                {
                    if (this.KitID == 0)
                    {
                        //validation for prodid, interfaces
                        if (this.NewVK_LPCreationVal())
                        {
                            int oPubID = PE_DAL.SetNewKITPUB(this.ProdID.Trim(), 0,
                                System.Convert.ToInt32(IsNCIPLSelected()),
                                System.Convert.ToInt32(IsROOSelected()), 
                                IsNCIPLSelected()? 1: -1,
                                IsROOSelected()? 1:-1,
                                System.Convert.ToInt32(this.IsVK));

                            if (oPubID > 0)
                                this.KitID = oPubID;

                            if (this.IsVK)
                            {
                                Pub l_pub = PE_DAL.GetPubInfoByProdID(this.ProdID.Trim());
                                this.lblTitle.Text = Server.HtmlEncode(l_pub.ShortTitle + "(" + l_pub.ProdID + ")");
                                
                                this.Title = this.lblPageTitle.Text = "Create Virtual Kit";
                                this.BindKitData();
                            }
                            else
                            {
                                this.Title = this.lblPageTitle.Text = "Create Linked Publication";
                                this.BindPubData();
                            }

                            this.lblRemv.Visible = false;
                        }
                        else
                            this.RejectCrossPagePostBack();
                    }
                    else
                    {
                        //get the vk/lp details
                        if (this.IsVK)
                        {
                            Pub l_pub = PE_DAL.GetPubInfoByPubID(this.KitID);
                            this.lblTitle.Text = Server.HtmlEncode(l_pub.ShortTitle + "(" + l_pub.ProdID + ")");

                            this.Title = this.lblPageTitle.Text = "Update Virtual Kit";
                            this.BindKitData();
                        }
                        else
                        {
                            this.Title = this.lblPageTitle.Text = "Update Linked Publication";
                            this.BindPubData();
                        }

                        //this.lblRemv.Visible = true;
                    }
                }
            }

            this.txtNewPub.Attributes.Add("onchange", "getPubTitle()");
            
        }

        protected void gvResult_ItemCreated(object source, DataGridItemEventArgs e)
        {
            List<Pub> dt = ((List<Pub>)this.gvResult.DataSource);

            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                if (dt != null)
                {
                    Pub l_pub = dt[e.Item.ItemIndex];

                    if (l_pub.PubID == this.KitID)
                    {
                        CheckBox l_chkboxRemove = ((CheckBox)e.Item.FindControl("chkboxDel"));
                        if (!this.IsVK)
                            l_chkboxRemove.Attributes.Add("onclick", @"if((this.checked)&&(checkRemLKReminder==1))
                {checkRemLKReminder++;alert('The publication you selected to remove is the primary publication of this Linked Publication,\nNOTE: " +
                                    @"By seleting this to remove, all publications that associated to this Linked Publication will also be removed!')}");
                        
                    }
                }
            }
        }

        protected void gvResult_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            List<Pub> dt = ((List<Pub>)this.gvResult.DataSource);

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Pub l_pub = dt[e.Item.ItemIndex];

                if (this.IsLP)
                {
                    if (this.lblTitle.Text.Length == 0)
                    {
                        if ((l_pub.PubID == this.KitID) || (l_pub.ProdID == this.ProdID))
                        {
                            this.lblTitle.Text = Server.HtmlEncode(l_pub.ShortTitle + " (" + l_pub.ProdID + ")");
                            e.Item.Cells[1].Text = "[Main Publication ID] " + l_pub.ProdID;
                        }
                    }

                    e.Item.Cells[1].HorizontalAlign = HorizontalAlign.Right;
                }
                else
                {
                    e.Item.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                }

                e.Item.Cells[2].Text = Server.HtmlEncode(l_pub.ShortTitle);

                TextBox l_txtQty = e.Item.Cells[3].Controls[1] as TextBox;
                l_txtQty.Text = l_pub.Qty.ToString();

            }
        }

        protected void btnGetKitPubTitle_Click(object sender, EventArgs args)
        {
            Pub l_pub = PE_DAL.GetPubInfoByProdID(this.txtNewPub.Text.Trim());

            if (l_pub != null)
            {
                this.lblMsg.Text = String.Empty;
                this.lblNewTitle.Text = Server.HtmlEncode(l_pub.ShortTitle);
                this.txtNewQty.Text = "1";
            }
            else
            {
                this.lblMsg.Text = "Invalid Publication ID.";
            }
            this.udpnlNewPub.Update();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool blnTryAddInvalidNewPub = false;

            string l_GetUpdatedInfo = this.GetUpdatedInfo();

            if (this.txtNewPub.Text.Trim().Length > 0 && this.txtNewQty.Text.Trim().Length > 0)
            {
                Pub l_pub = PE_DAL.GetPubInfoByProdID(this.txtNewPub.Text.Trim());

                if (l_pub != null)
                {
                    l_GetUpdatedInfo = l_pub.PubID.ToString() + PubEntAdminManager.indDelim +
                        this.txtNewQty.Text.Trim() + PubEntAdminManager.indDelim + "0" +
                        PubEntAdminManager.pairDelim + l_GetUpdatedInfo;
                }
                else
                {
                    blnTryAddInvalidNewPub = true;
                }
            }

            bool blnSave = PE_DAL.SetKITPUBs(this.KitID, l_GetUpdatedInfo, this.IsNCIPL ? 1 : 0, this.IsROO ? 1 : 0,
                this.IsVK ? 1 : 0, PubEntAdminManager.pairDelim, PubEntAdminManager.indDelim);

            if (blnSave)
            {
                if (this.IsVK)
                    this.BindKitData();
                else
                    this.BindPubData();

                if (!blnTryAddInvalidNewPub)
                {
                    this.lblNewTitle.Text = this.txtNewPub.Text = this.txtNewQty.Text = "";
                    this.lblRemv.Visible = true;
                    this.lblMsg.Text = "Your changes have been saved";
                }
                else
                {
                    this.lblMsg.Text = "Can not add the invalid publication to the collection";
                }
            }
            else
            {
                this.lblMsg.Text = "Your changes have not been saved";
            }
            
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            if (PubEntAdminManager.TamperProof)
            {
                PubEntAdminManager.RedirectEncodedURLWithQS(ParentPage, PubEntAdminManager.strVK_LPType +
                    "=" + (this.IsVK ? PubEntAdminManager.strVKType : PubEntAdminManager.strLPType));
            }
            else
            {
                Response.Redirect(ParentPage + "?" + PubEntAdminManager.strVK_LPType +
                    "=" + (this.IsVK ? PubEntAdminManager.strVKType : PubEntAdminManager.strLPType));
            }
        }


        #endregion

        #region Methods
        protected bool IsNCIPLSelected()
        {
            for (int i = 0; i < sarr.Length; i++)
            {
                if (String.Compare(sarr[i], PubEntAdminManager.NCIPL_INTERFACE, true) == 0)
                {
                    this.IsNCIPL = true;
                    return true;
                }
            }
            this.IsNCIPL = false;
            return false;
        }

        protected bool IsROOSelected()
        {
            for (int i = 0; i < sarr.Length; i++)
            {
                if (String.Compare(sarr[i], PubEntAdminManager.ROO_INTERFACE, true) == 0)
                {
                    this.IsROO = true;
                    return true;
                }
            }
            this.IsROO = false;
            return false;
        }

        protected void NewPubCreateVal()
        {
            if ((!PubEntAdminManager.LenVal(this.ProdID, 10)))
            {
                Response.Redirect("InvalidInput.aspx");
            }
            //------------------------
            if ((PubEntAdminManager.OtherVal(this.ProdID)))
            {
                Response.Redirect("InvalidInput.aspx");
            }

            foreach (string s in this.sarr)
            {
                if ((PubEntAdminManager.OtherVal(s)))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }
            //------------------------
            if ((PubEntAdminManager.SpecialVal2(this.ProdID.Replace(" ", ""))))
            {
                Response.Redirect("InvalidInput.aspx");
            }

            foreach (string s in this.sarr)
            {
                if ((PubEntAdminManager.SpecialVal2(s)))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }
        }

        protected bool NewVK_LPCreationVal()
        {
            if (this.ProdID.Length > 0 && this.sarr.Length > 0)
            {
                this.NewPubCreateVal();

                string pubid = this.ProdID.Trim();

                List<string> val = PE_DAL.GetProdInt(pubid, System.Convert.ToInt32(this.IsVK));

                string[] selectedInt = new string[this.sarr.Length];
                string[] t_val;

                for (int k = 0; k < sarr.Length; k++)
                {
                    selectedInt[k] = (sarr[k]);
                }

                if (val.Count == 0)
                {
                    Session[DisplayKitPubErrorMsg] =
                                    "The Publication you provide does not exist.";
                    return false;
                }
                else
                {
                    if (System.Convert.ToInt32(val[0]) > 0)
                    {
                        t_val = new string[2];
                        for (int i = 3; i < val.Count; i++)
                        {
                            t_val[i - 3] = val[i];
                        }

                        if (this.containsAny(t_val, selectedInt))
                        {
                            if (this.IsVK)
                            {
                                Session[DisplayKitPubErrorMsg] =
                                    "This Virtual Publication ID already exists for the interface(s) selected.";
                            }
                            else
                            {
                                Session[DisplayKitPubErrorMsg] =
                                    "This Linked Publication ID already exists for the interface(s) selected.";
                            }

                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        t_val = new string[4];
                        for (int i = 1; i < val.Count; i++)
                        {
                            t_val[(i - 1)] = val[i];
                        }

                        if (!this.containsAll(t_val, selectedInt))
                        {
                            Session[DisplayKitPubErrorMsg] =
                                "The publication you provide has not been assigned to the interface(s) you selected.";
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }
            else
            {
                return false;
            }
        }

        private bool contains (string[] one , string another)
        {
            for (int i = 0; i < one.Length; i++)
            {
                if (one[i] == another)
                    return true;
            }
            return false;
        }

        private bool containsAny(string[] one, string [] another) {
            for (var i = 0; i < another.Length; i++)
            {
                if (contains(one, another[i]))
                    return true;
            }
            return false;
        }

        private bool containsAll(string[] one, string[] another)
        {
            for (var i = 0; i < another.Length; i++)
            {
                if (!contains(one, another[i]))
                    return false;

            }
            return true;
        }

        protected void RejectCrossPagePostBack()
        {
            if (PubEntAdminManager.TamperProof)
            {
                PubEntAdminManager.RedirectEncodedURLWithQS(ParentPage, PubEntAdminManager.strVK_LPType + 
                    "=" + (this.IsVK ? PubEntAdminManager.strVKType : PubEntAdminManager.strLPType));
            }
            else
            {
                Response.Redirect(ParentPage + "?" + PubEntAdminManager.strVK_LPType + 
                    "=" + (this.IsVK ? PubEntAdminManager.strVKType : PubEntAdminManager.strLPType));
            }
        }

        protected string GetParticipatedInterface()
        {
            string s = "";

            if (this.IsNCIPL)
                s = PubEntAdminManager.NCIPL_INTERFACE;

            if (this.IsROO)
            {
                if (s.Length > 0)
                    s += PubEntAdminManager.indDelim;
                s += PubEntAdminManager.ROO_INTERFACE;
            }

            return s;
        }

        protected void BindKitData()
        {
            List<Pub> l_publist = PE_DAL.GetKitDisplay(this.KitID, GetParticipatedInterface());

            this.gvResult.DataSource = l_publist;
            this.gvResult.DataBind();

            if (l_publist.Count > 0)
            {
                this.lblRemv.Visible = true;
            }
            else
                this.lblRemv.Visible = false;
        }

        protected void BindPubData()
        {
            List<Pub> l_publist = PE_DAL.GetPubDisplay(this.KitID, GetParticipatedInterface());

            //if (l_publist.Count > 0)
            //{
                this.gvResult.DataSource = l_publist;
                this.gvResult.DataBind();
            //}
            //else
            //    this.RejectCrossPagePostBack();
        }

        protected string GetUpdatedInfo()
        {
            string id_qtys_rem = "";

            foreach (DataGridItem c in this.gvResult.Items)
            {
                if (c.ItemType == ListItemType.Item || c.ItemType == ListItemType.AlternatingItem)
                {
                    if (id_qtys_rem.Length > 0)
                        id_qtys_rem += PubEntAdminManager.pairDelim;

                    string qty = ((TextBox)c.Cells[3].FindControl("txtQty")).Text.Trim();

                    try
                    {
                        if (qty.Length > 0 && System.Convert.ToInt32(qty) > 0)
                        {
                            id_qtys_rem += c.Cells[0].Text + PubEntAdminManager.indDelim + qty;
                        }
                    }
                    catch (FormatException fex)
                    {
                        Response.Redirect("InvalidInput.aspx");
                    }

                    if (((CheckBox)c.Cells[4].FindControl("chkboxDel")).Checked)
                    {
                        id_qtys_rem += PubEntAdminManager.indDelim + "1";
                    }
                    else
                    {
                        id_qtys_rem += PubEntAdminManager.indDelim + "0";
                    }
                }
            }

            return id_qtys_rem;
        }

        #region Sec Val
        private void SecVal()
        {
            this.LenVal();
            this.TypeVal();
            this.TagVal();
            this.SpecialVal();
            this.QueryStringVal();
        }

        private void LenVal()
        {
            if ((!PubEntAdminManager.LenVal(this.txtNewPub.Text, 10)) ||
                (!PubEntAdminManager.LenVal(this.txtNewQty.Text, 8)))
            {
                Response.Redirect("InvalidInput.aspx");
            }
        }

        private void TypeVal()
        {
            if (this.txtNewQty.Text.Trim().Length > 0)
            {
                if (!PubEntAdminManager.ContentNumVal(this.txtNewQty.Text.Trim()))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }
        }

        private void TagVal()
        {
            if ((PubEntAdminManager.OtherVal(this.txtNewPub.Text)) ||
                (PubEntAdminManager.OtherVal(this.txtNewQty.Text)))
            {
                Response.Redirect("InvalidInput.aspx");
            }
        }

        private void SpecialVal()
        {
            if ((PubEntAdminManager.SpecialVal2(this.txtNewPub.Text.Replace(" ", ""))) ||
                (PubEntAdminManager.SpecialVal2(this.txtNewQty.Text.Replace(" ", ""))))
            {
                Response.Redirect("InvalidInput.aspx");
            }
        }

        private void QueryStringVal()
        {
            foreach (object item in Request.QueryString)
            {
                string strToBeTest = Request.QueryString[item.ToString()].Replace(" ", "");
                if (strToBeTest.Length > 0)
                {
                    if (!PubEntAdminManager.ContentVal(strToBeTest,
                        "^[0-9a-zA-Z=,]+$"))
                    {
                        Response.Redirect("InvalidInput.aspx");
                    }
                }
            }
        }
        #endregion

        #endregion

        #region Properties
        protected string ProdID
        {
            set { ViewState["strProdID"] = value; }
            get { 
                if (ViewState["strProdID"] != null)
                    return (ViewState["strProdID"].ToString()); 
                else 
                    return String.Empty;
            }
        }

        protected int KitID
        {
            set { ViewState["intKitID"] = value; }
            get { 
                if (ViewState["intKitID"]!=null)
                    return System.Convert.ToInt32(ViewState["intKitID"]);
                else
                    return 0;
            }
        }

        protected bool IsVK
        {
            set { ViewState["blnIsVK"] = value; }
            get { return System.Convert.ToBoolean(ViewState["blnIsVK"]); }
        }

        protected bool IsLP
        {
            get { return !(IsVK); }
        }

        protected bool IsNCIPL
        {
            set { ViewState["blnNCIPL"] = value; }
            get { return System.Convert.ToBoolean(ViewState["blnNCIPL"]); }
        }

        protected bool IsROO
        {
            set { ViewState["blnROO"] = value; }
            get { return System.Convert.ToBoolean(ViewState["blnROO"]); }
        }
        #endregion

        
    }
}
