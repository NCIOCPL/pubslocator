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
using GlobalUtils;
using System.Text;
using PubEntAdmin.BLL;

namespace PubEntAdmin
{
    public partial class LookupMgmt : System.Web.UI.Page
    {
        #region Fields
        private string qs_lkup = "";
        private StringBuilder SaveScrollLocation = new StringBuilder();
        private StringBuilder SetScrollLocation = new StringBuilder();
        #endregion

        #region Events Handling
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session.IsNewSession)
            {
                Response.Redirect("Home.aspx");
            }
            
            this.Title = "Look Up Table Management";
            this.PageTitle = "Look Up Management";
            this.AddJS("../JS/LUMgmt.js");

            ClientScript.RegisterHiddenField("__SCROLLLOC", "0");
            if (!ClientScript.IsClientScriptBlockRegistered("ClientSmartNavSaveScrollScript"))
            {
                SaveScrollLocation.Append("<script type='text/javascript' language='javascript'>");
                SaveScrollLocation.Append("function SaveScrollLocation () {");
                SaveScrollLocation.Append("     document.forms[0].__SCROLLLOC.value = document.documentElement.scrollTop;");
                SaveScrollLocation.Append("}");
                SaveScrollLocation.Append("document.body.onscroll=SaveScrollLocation;");
                SaveScrollLocation.Append("</script>");
                ClientScript.RegisterStartupScript(this.GetType(),"ClientSmartNavSaveScrollScript", SaveScrollLocation.ToString(),false);
            }

            if (!Page.IsPostBack)
            {
                Session.Remove(PubEntAdminManager.strTabContentLUCurrActTabIndex);
            }
            else
            {
                this.SecVal();

                if (!ClientScript.IsClientScriptBlockRegistered("ClientSmartNavSetScrollScript2"))
                {
                    SetScrollLocation.Append("<script type='text/javascript' language='javascript'>");
                    SetScrollLocation.Append("function SetScrollLocation () {");
                    SetScrollLocation.Append("     document.documentElement.scrollTop = " + Request["__SCROLLLOC"] + ";");
                    SetScrollLocation.Append("}");
                    SetScrollLocation.Append("document.body.onload=SetScrollLocation" + ";");
                    SetScrollLocation.Append("</script>");
                    ClientScript.RegisterStartupScript(this.GetType(), "ClientSmartNavSetScrollScript2", SetScrollLocation.ToString(),false);
                }
            }

            System.Web.UI.UserControl usrCtrl=null;

            if (PubEntAdminManager.TamperProof)
            {
                qs_lkup = PubEntAdminManager.GetURLQS(PubEntAdminManager.LOOKUP_SUB);
            }
            else
            {
                qs_lkup = Request.QueryString[PubEntAdminManager.LOOKUP_SUB].ToString();
            }

            if (qs_lkup.Length > 0)
            {
                this.plcHldContent.Controls.Clear();
                switch (qs_lkup)
                {
                    case "audience":
                        usrCtrl = (System.Web.UI.UserControl)this.LoadControl("UserControl/Audience.ascx");
                        this.plcHldContent.Controls.Add(usrCtrl);
                        break;
                    case "award":
                        usrCtrl = (System.Web.UI.UserControl)this.LoadControl("UserControl/Award.ascx");
                        this.plcHldContent.Controls.Add(usrCtrl);
                        break;
                    case "cancertype":
                        usrCtrl = (System.Web.UI.UserControl)this.LoadControl("UserControl/CancerType.ascx");
                        this.plcHldContent.Controls.Add(usrCtrl);
                        break;
                    case "language":
                        usrCtrl = (System.Web.UI.UserControl)this.LoadControl("UserControl/Language.ascx");
                        this.plcHldContent.Controls.Add(usrCtrl);
                        break;
                    case "prodformat":
                        usrCtrl = (System.Web.UI.UserControl)this.LoadControl("UserControl/ProductFormat.ascx");
                        this.plcHldContent.Controls.Add(usrCtrl);
                        break;
                    case "readinglevel":
                        usrCtrl = (System.Web.UI.UserControl)this.LoadControl("UserControl/ReadingLevel.ascx");
                        this.plcHldContent.Controls.Add(usrCtrl);
                        break;                    
                    case "race":
                        usrCtrl = (System.Web.UI.UserControl)this.LoadControl("UserControl/Race.ascx");
                        this.plcHldContent.Controls.Add(usrCtrl);
                        break;
                    case "serie":
                        usrCtrl = (System.Web.UI.UserControl)this.LoadControl("UserControl/Series.ascx");
                        this.plcHldContent.Controls.Add(usrCtrl);
                        break;
                    case "subj":
                        usrCtrl = (System.Web.UI.UserControl)this.LoadControl("UserControl/Subject.ascx");
                        usrCtrl.ID = "Suject1";
                        this.plcHldContent.Controls.Add(usrCtrl);
                        break;
                    case "subcat":
                        if (Session[PubEntAdminManager.JS] != null)
                        {

                            if (System.Convert.ToBoolean(Session[PubEntAdminManager.JS].ToString()))
                            {
                                usrCtrl = (System.Web.UI.UserControl)this.LoadControl("UserControl/SubCatx.ascx");
                                usrCtrl.ID = "SubCatx1";
                            }
                            else
                            {
                                usrCtrl = (System.Web.UI.UserControl)this.LoadControl("UserControl/SubCat.ascx");
                                usrCtrl.ID = "SubCat1";
                            }
                        }
                        else
                        {
                            usrCtrl = (System.Web.UI.UserControl)this.LoadControl("UserControl/SubCat.ascx");
                            usrCtrl.ID = "SubCat1";
                        }

                        this.plcHldContent.Controls.Add(usrCtrl);
                        break;
                    case "subsubcat":
                        if (Session[PubEntAdminManager.JS] != null)
                        {

                            if (System.Convert.ToBoolean(Session[PubEntAdminManager.JS].ToString()))
                            {
                                usrCtrl = (System.Web.UI.UserControl)this.LoadControl("UserControl/SubSubCatx.ascx");
                                usrCtrl.ID = "SubSubCatx1";
                            }
                            else
                            {
                                usrCtrl = (System.Web.UI.UserControl)this.LoadControl("UserControl/SubSubCat.ascx");
                                usrCtrl.ID = "SubSubCat1";
                            }
                        }
                        else
                        {
                            usrCtrl = (System.Web.UI.UserControl)this.LoadControl("UserControl/SubSubCat.ascx");
                            usrCtrl.ID = "SubSubCat1";
                        }
                        this.plcHldContent.Controls.Add(usrCtrl);
                        break;
                    case "sponsor":
                        usrCtrl = (System.Web.UI.UserControl)this.LoadControl("UserControl/Sponsor.ascx");
                        usrCtrl.ID = "Sponsor1";
                        this.plcHldContent.Controls.Add(usrCtrl);
                        break;
                    case "owner":
                        usrCtrl = (System.Web.UI.UserControl)this.LoadControl("UserControl/Owner.ascx");
                        usrCtrl.ID = "Owner1";
                        this.plcHldContent.Controls.Add(usrCtrl);
                        break;

                }
                
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (PubEntAdminManager.TamperProof)
            {
                if (PubEntAdminManager.ContainURLQS("subsubcatid"))
                    PubEntAdminManager.RedirectEncodedURLWithQS("LookupMgmt.aspx", "sub=subsubcat");
            }
            else
            {
                if (Request.QueryString["subsubcatid"] != null)
                {
                    Response.Redirect("~/LookupMgmt.aspx?sub=subsubcat");
                }
            }

            if (Session[PubEntAdminManager.JS] != null)
            {
                if (System.Convert.ToBoolean(Session[PubEntAdminManager.JS].ToString()))
                {
                    this.InsertSplChkScript();

                    this.SpellCkr1.RefImgSplChk().Attributes.Add("onclick", "LookupMgmt_SpellCheckClick(1)");
                    this.SpellCkr2.RefImgSplChk().Attributes.Add("onclick", "LookupMgmt_SpellCheckClick(2)");
                }
                else
                {
                    ShowHideSpellChecker(false);
                }
            }
            else
            {
                ShowHideSpellChecker(false);
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            return;
        }

        #endregion

        #region Properties
        public string PageTitle
        {
            set
            {
                ((Label)this.Master.FindControl("lblPageTitle")).Text = value;
            }
            get
            {
                return ((Label)this.Master.FindControl("lblPageTitle")).Text;
            }
        }

        public string ParentSpellCkr1
        {
            get {
                return this.trSpellCkr1.ClientID;
            }
        }

        public string ParentSpellCkr2
        {
            get
            {
                return this.trSpellCkr2.ClientID;
            }
        }

        public string QS_LKUP
        {
            get
            {
                return this.qs_lkup;
            }
        }
        #endregion

        #region Methods
        public void AddJS(string strJSPath)
        {
            ((System.Web.UI.ScriptManager)this.Master.FindControl("ScriptManager_Default2Master")).
                Scripts.Add(new ScriptReference(strJSPath));
        }

        public void ShowHideSpellChecker(bool show)
        {
            this.trSpellCkr1.Visible = this.trSpellCkr2.Visible = show;
        }

        protected void InsertSplChkScript()
        {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "LookupMgmt_ClientScript",
               @"
                function LookupMgmt_SpellCheckClick(ctrlIndex)
                {
                    var langsel = document.getElementById('" + this.SpellCkr1.LangSelClientID + @"').
                        options[document.getElementById('" + this.SpellCkr1.LangSelClientID + @"').selectedIndex].value;

                    var langsel2 = document.getElementById('" + this.SpellCkr2.LangSelClientID + @"').
                        options[document.getElementById('" + this.SpellCkr2.LangSelClientID + @"').selectedIndex].value;
    
                    var finallangsel = '28001';//default to e

                    var t = '';
                    if(typeof LkupParticipants == 'function') {
                        t = LkupParticipants();
                    } 

                    if (ctrlIndex == 1)
                    {
                        finallangsel = langsel;
                    }
                    else if (ctrlIndex == 2)
                    {
                        finallangsel = langsel2;
                    }

                    if (finallangsel == '28001')//e
                    {
                            window.open('" + ConfigurationSettings.AppSettings["SpellingCheckerLoc"] +
                        "?" + PubEntAdminManager.SPELLCHECK2CHECKFIELDSLIST + "=" +
                        @"'+t+'','winspell','toolbar=0,location=0,directories=0,status=0,scrollbars=0,resizable=0,width=500,height=220');
                    }
                    else if (finallangsel == '29552')//s
                    {
                        window.open('" + ConfigurationSettings.AppSettings["SpellingCheckerLoc"] +
                        "?" + PubEntAdminManager.SPELLCHECK2CHECKFIELDSLIST + "=" +
                        @"'+t+'&dict=SPANISH','winspell','toolbar=0,location=0,directories=0,status=0,scrollbars=0,resizable=0,width=500,height=220');
                    }
                }
                ", true);
        }

        #region Sec Val
        private void SecVal()
        {
            this.TagVal();
            this.SpecialVal();
        }

        private void TagVal()
        {
            if ((PubEntAdminManager.OtherVal2(Request["__SCROLLLOC"].ToString().Trim())))
            {
                Response.Redirect("InvalidInput.aspx");
            }
        }

        private void SpecialVal()
        {
            if ((PubEntAdminManager.SpecialVal2(Request["__SCROLLLOC"].ToString().Trim().Replace(" ", ""))))
            {
                Response.Redirect("InvalidInput.aspx");
            }
        }
        #endregion

        #endregion
    }
}
