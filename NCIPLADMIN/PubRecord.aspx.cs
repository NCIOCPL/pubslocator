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
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using PubEntAdmin.UserControl;
using GlobalUtils;
using PubEntAdmin.BLL;


namespace PubEntAdmin
{
    public partial class PubRecord : System.Web.UI.Page
    {
        public event EventHandler ScriptManagerNavigate;
     
        protected void OnPubRecordScriptMgmtNavigate(HistoryEventArgs e)
        {
            if (ScriptManagerNavigate != null)
            {
                ScriptManagerNavigate(this, e);
            }
            
        }

        protected void PubRecordScriptMgmtNavigate(object sender, HistoryEventArgs e)
        {
            OnPubRecordScriptMgmtNavigate(e);
        }

        #region Variables
        protected int intPubId;
        protected string strMode;
        #endregion

        #region Controls
        protected UrlBuilder myUrlBuilder = new UrlBuilder(HttpContext.Current.Request.Url.AbsoluteUri, new Base64Encoder());
        protected LiveIntTab myLiveIntTab = new LiveIntTab();
        #endregion

        #region Events Handling
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            
            //try
            //{
            //    int a = 0;
            //    int b = 100 / a;
            //}
            //catch (Exception ex)
            //{
            //    //throw ex;
            //    LogEntry logEnt = new LogEntry();
            //    logEnt.Message = "\r\n" + "Test Writing Error." + "\r\n" + "Source: " + ex.Source + "\r\n" + "Description: " + ex.Message + "\r\n" + "Stack Trace: " + ex.StackTrace;
            //    Logger.Write(logEnt, "Logs");
            //}
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session.IsNewSession)//cross-site request forgery
            {
                Response.Redirect("Home.aspx");
            }

            this.AssignValues();
            this.ByPassRegisterMonitoredChanges();
            System.Web.UI.UserControl userControl;
            this.PageTitle = "Publication Record";

            if (Session[PubEntAdminManager.strGlobalMsg] != null)
            {
                this.lblMsg.Text = Session[PubEntAdminManager.strGlobalMsg].ToString();
            }

            if (!Page.IsPostBack)
            {
                Session.Remove(PubEntAdminManager.strPubGlobalMode);
                //Session.Remove(PubEntAdminManager.strTabContentCurrActTabIndex);
                Session.Remove(PubEntAdminManager.strGlobalMsg);
                Session.Remove(PubEntAdminManager.strReloadPubHist);
                Session.Remove(PubEntAdminManager.strReloadRelatedPub);
                Session.Remove(PubEntAdminManager.strReloadRelatedTranslation);

                if (this.Mode == PubEntAdminManager.strPubGlobalAMode)//add
                {
                    if (((CustomPrincipal)Context.User).IsInRole(PubEntAdminManager.AdminRole))
                    {
                        Session[PubEntAdminManager.strPubGlobalMode] = PubEntAdminManager.strPubGlobalAMode;
                        this.hplnkBakSechRes.Visible = this.hplnkRefSech.Visible = false;
                        this.hplnkBakSechRes2.Visible = this.hplnkRefSech2.Visible = false;
                        this.btnEdit.Visible = this.btnEdit2.Visible = false;
                        this.btnSave.Visible = this.SpellCkr1.Visible = true;
                        this.btnSave2.Visible = this.SpellCkr2.Visible = true;
                        this.btnCancel.Visible = this.btnCancel2.Visible = false;
                        this.lblPageTitle.Text = "Add New Publication";

                        userControl =
                            (System.Web.UI.UserControl)this.LoadControl("UserControl/GenDataE.ascx");
                        ((GenDataE)userControl).Mode = this.Mode;
                        userControl.ID = "GenDataEInfo1";
                        this.plcHldGenData.Controls.Add(userControl);

                        userControl =
                            (System.Web.UI.UserControl)this.LoadControl("UserControl/GenProjSettingE.ascx");
                        userControl.ID = "GenProjSettingEInfo1";
                        ((GenProjSettingE)userControl).Mode = this.Mode;
                        this.plcHldGenProjSetting.Controls.Add(userControl);

                        this.trLiveInterfaces.Disabled = false;
                    }
                    else
                    {
                        if (PubEntAdminManager.TamperProof)
                        {
                            PubEntAdminManager.RedirectEncodedURLWithQS("UnauthorizedAccess.aspx",
                                PubEntAdminManager.strUnauthorizedDetail + "=Try to access pub record in Add mode");
                        }
                        else
                        {
                            Response.Redirect("UnauthorizedAccess.aspx?" +
                                PubEntAdminManager.strUnauthorizedDetail + "=Try to access pub record in Add mode", true);
                        }
                    }

                }
                else if (this.Mode == PubEntAdminManager.strPubGlobalEMode)//edit
                {
                    if (this.PubID > 0)
                    {
                        Session[PubEntAdminManager.strPubGlobalMode] = PubEntAdminManager.strPubGlobalEMode;

                        this.btnEdit.Visible = this.btnEdit2.Visible = false;

                        //if (((CustomPrincipal)Context.User).IsInRole(PubEntAdminManager.AdminRole))
                        //{
                            this.btnSave.Visible = this.SpellCkr1.Visible = true;
                            this.btnSave2.Visible = this.SpellCkr2.Visible = true;
                            this.btnCancel.Visible = this.btnCancel2.Visible = true;
                        //}
                        //else
                        //{
                        //    this.btnSave.Visible = this.SpellCkr1.Visible = false;
                        //    this.btnSave2.Visible = this.SpellCkr2.Visible = false;
                        //    this.btnCancel.Visible = this.btnCancel2.Visible = false;
                        //}
                        //authenticate user
                        this.lblPageTitle.Text = "Edit Publication";

                        if (PubEntAdminManager.TamperProof)
                        {
                            myUrlBuilder.PageName = "Home.aspx";
                            myUrlBuilder.Query = "action=refine";

                            this.hplnkRefSech.NavigateUrl = this.hplnkRefSech2.NavigateUrl = myUrlBuilder.ToString(true);
                            this.hplnkRefSech.Visible = this.hplnkRefSech2.Visible = true;

                            myUrlBuilder.PageName = "SearchResult.aspx";
                            myUrlBuilder.Query = "action=sechres";

                            this.hplnkBakSechRes.NavigateUrl = this.hplnkBakSechRes2.NavigateUrl = myUrlBuilder.ToString(true);
                            this.hplnkBakSechRes.Visible = this.hplnkBakSechRes2.Visible = true;
                        }
                        else
                        {
                            this.hplnkRefSech.NavigateUrl = this.hplnkRefSech2.NavigateUrl = "~/Home.aspx?action=refine";
                            this.hplnkRefSech.Visible = this.hplnkRefSech2.Visible = true;

                            this.hplnkBakSechRes.NavigateUrl = this.hplnkBakSechRes2.NavigateUrl = "~/SearchResult.aspx?action=sechres";
                            this.hplnkBakSechRes.Visible = this.hplnkBakSechRes2.Visible = true;
                        }

                        if (((CustomPrincipal)Context.User).IsInRole(PubEntAdminManager.AdminRole))
                        {
                            userControl =
                                (System.Web.UI.UserControl)this.LoadControl("UserControl/GenDataE.ascx");
                            ((GenDataE)userControl).ID = "GenDataEInfo1";
                            ((GenDataE)userControl).Mode = this.Mode;
                            ((GenDataE)userControl).PubID = this.PubID;
                        }
                        else
                        {
                            userControl =
                                (System.Web.UI.UserControl)this.LoadControl("UserControl/GenDataV.ascx");
                            ((GenDataV)userControl).ID = "GenDataVInfo1";
                            ((GenDataV)userControl).PubID = this.PubID;
                        }

                        this.plcHldGenData.Controls.Add(userControl);

                        if (((CustomPrincipal)Context.User).IsInRole(PubEntAdminManager.AdminRole))
                        {
                            userControl =
                                (System.Web.UI.UserControl)this.LoadControl("UserControl/GenProjSettingE.ascx");
                            ((GenProjSettingE)userControl).ID = "GenProjSettingEInfo1";
                            ((GenProjSettingE)userControl).Mode = this.Mode;
                            ((GenProjSettingE)userControl).PubID = this.PubID;
                        }
                        else
                        {
                            userControl =
                                (System.Web.UI.UserControl)this.LoadControl("UserControl/GenProjSettingV.ascx");
                            ((GenProjSettingV)userControl).ID = "GenProjSettingVInfo1";
                            ((GenProjSettingV)userControl).PubID = this.PubID;
                        }
                        
                        this.plcHldGenProjSetting.Controls.Add(userControl);

                        if (((CustomPrincipal)Context.User).IsInRole(PubEntAdminManager.AdminRole))
                            this.trLiveInterfaces.Disabled = false;
                        else
                            this.trLiveInterfaces.Disabled = true;
                    }
                }
                else if (this.Mode == PubEntAdminManager.strPubGlobalVMode)//view
                {
                    if (this.PubID > 0)
                    {
                        Session[PubEntAdminManager.strPubGlobalMode] = PubEntAdminManager.strPubGlobalVMode;

                        this.btnEdit.Visible = this.btnEdit2.Visible = true;

                        if (PubEntAdminManager.TamperProof)
                        {
                            myUrlBuilder.PageName = "PubRecord.aspx";
                            myUrlBuilder.Query = "mode=edit&pubid=" + this.PubID.ToString();

                            this.btnEdit.Attributes["onclick"] += "window.location = '" + myUrlBuilder.ToString(true) + "'";
                            this.btnEdit2.Attributes["onclick"] += "window.location = '" + myUrlBuilder.ToString(true) + "'";
                        }
                        else
                        {
                            this.btnEdit.Attributes["onclick"] += "window.location = 'PubRecord.aspx?mode=edit&pubid=" +
                                this.PubID + "'";
                            this.btnEdit2.Attributes["onclick"] += "window.location = 'PubRecord.aspx?mode=edit&pubid=" +
                                this.PubID + "'";
                        }

                        this.btnSave.Visible = this.SpellCkr1.Visible = false;
                        this.btnSave2.Visible = this.SpellCkr2.Visible = false;
                        this.btnCancel.Visible = this.btnCancel2.Visible = false;

                        this.lblPageTitle.Text = "View Publication";

                        if (PubEntAdminManager.TamperProof)
                        {
                            myUrlBuilder.PageName = "Home.aspx";
                            myUrlBuilder.Query = "action=refine";

                            this.hplnkRefSech.NavigateUrl = this.hplnkRefSech2.NavigateUrl = myUrlBuilder.ToString(true);
                            this.hplnkRefSech.Visible = this.hplnkRefSech2.Visible = true;

                            myUrlBuilder.PageName = "SearchResult.aspx";
                            myUrlBuilder.Query = "action=sechres";

                            this.hplnkBakSechRes.NavigateUrl = this.hplnkBakSechRes2.NavigateUrl = myUrlBuilder.ToString(true);
                            this.hplnkBakSechRes.Visible = this.hplnkBakSechRes2.Visible = true;
                        }
                        else
                        {
                            this.hplnkRefSech.NavigateUrl = this.hplnkRefSech2.NavigateUrl = "~/Home.aspx?action=refine";
                            this.hplnkRefSech.Visible = this.hplnkRefSech2.Visible = true;

                            this.hplnkBakSechRes.NavigateUrl = this.hplnkBakSechRes2.NavigateUrl = "~/SearchResult.aspx?action=sechres";
                            this.hplnkBakSechRes.Visible = this.hplnkBakSechRes2.Visible = true;
                        }

                        userControl =
                            (System.Web.UI.UserControl)this.LoadControl("UserControl/GenDataV.ascx");
                        ((GenDataV)userControl).ID = "GenDataVInfo1";
                        ((GenDataV)userControl).PubID = this.PubID;
                        this.plcHldGenData.Controls.Add(userControl);

                        userControl =
                            (System.Web.UI.UserControl)this.LoadControl("UserControl/GenProjSettingV.ascx");
                        ((GenProjSettingV)userControl).ID = "GenProjSettingVInfo1";
                        ((GenProjSettingV)userControl).PubID = this.PubID;
                        this.plcHldGenProjSetting.Controls.Add(userControl);

                        this.trLiveInterfaces.Disabled = true;
                    }
                }
            }
            else
            {
                if (Session[PubEntAdminManager.strPubGlobalMode] != null)
                {
                    if ((string)Session[PubEntAdminManager.strPubGlobalMode] ==
                        PubEntAdminManager.strPubGlobalAMode)
                    {
                        if (((CustomPrincipal)Context.User).IsInRole(PubEntAdminManager.AdminRole))
                        {
                            this.hplnkBakSechRes.Visible = this.hplnkRefSech.Visible = false;
                            this.hplnkBakSechRes2.Visible = this.hplnkRefSech2.Visible = false;
                            this.btnEdit.Visible = this.btnEdit2.Visible = false;
                            this.btnSave.Visible = this.SpellCkr1.Visible = true;
                            this.btnSave2.Visible = this.SpellCkr2.Visible = true;
                            this.btnCancel.Visible = this.btnCancel2.Visible = false;
                            this.lblPageTitle.Text = "Add New Record";

                            userControl =
                                (System.Web.UI.UserControl)this.LoadControl("UserControl/GenDataE.ascx");
                            userControl.ID = "GenDataEInfo1";
                            ((GenDataE)userControl).Mode = this.Mode;
                            this.plcHldGenData.Controls.Add(userControl);

                            userControl =
                                (System.Web.UI.UserControl)this.LoadControl("UserControl/GenProjSettingE.ascx");
                            userControl.ID = "GenProjSettingEInfo1";
                            ((GenProjSettingE)userControl).Mode = this.Mode;
                            this.plcHldGenProjSetting.Controls.Add(userControl);

                            this.trLiveInterfaces.Disabled = false;
                        }
                        else
                        {
                            if (PubEntAdminManager.TamperProof)
                            {
                                PubEntAdminManager.RedirectEncodedURLWithQS("UnauthorizedAccess.aspx",
                                    PubEntAdminManager.strUnauthorizedDetail + "=Try to access pub record in Add mode");
                            }
                            else
                            {
                                Response.Redirect("UnauthorizedAccess.aspx?" +
                                    PubEntAdminManager.strUnauthorizedDetail + "=Try to access pub record in Add mode", true);
                            }
                        }
                    }
                    else if ((string)Session[PubEntAdminManager.strPubGlobalMode] ==
                        PubEntAdminManager.strPubGlobalEMode)
                    {
                        this.btnEdit.Visible = this.btnEdit2.Visible = false;

                        //if (((CustomPrincipal)Context.User).IsInRole(PubEntAdminManager.AdminRole))
                        //{
                            this.btnSave.Visible = this.SpellCkr1.Visible = true;
                            this.btnSave2.Visible = this.SpellCkr2.Visible = true;
                            this.btnCancel.Visible = this.btnCancel2.Visible = true;
                        //}
                        //else
                        //{
                        //    this.btnSave.Visible = this.SpellCkr1.Visible = false;
                        //    this.btnSave2.Visible = this.SpellCkr2.Visible = false;
                        //    this.btnCancel.Visible = this.btnCancel2.Visible = false;
                        //}
                        //authenticate user
                        this.lblPageTitle.Text = "Edit Record";

                        if (PubEntAdminManager.TamperProof)
                        {
                            myUrlBuilder.PageName = "Home.aspx";
                            myUrlBuilder.Query = "action=refine";

                            this.hplnkRefSech.NavigateUrl = this.hplnkRefSech2.NavigateUrl = myUrlBuilder.ToString(true);
                            this.hplnkRefSech.Visible = this.hplnkRefSech2.Visible = true;

                            myUrlBuilder.PageName = "SearchResult.aspx";
                            myUrlBuilder.Query = "action=sechres";

                            this.hplnkBakSechRes.NavigateUrl = this.hplnkBakSechRes2.NavigateUrl = myUrlBuilder.ToString(true);
                            this.hplnkBakSechRes.Visible = this.hplnkBakSechRes2.Visible = true;
                        }
                        else
                        {
                            this.hplnkRefSech.NavigateUrl = this.hplnkRefSech2.NavigateUrl = "~/Home.aspx?action=refine";
                            this.hplnkRefSech.Visible = this.hplnkRefSech2.Visible = true;

                            this.hplnkBakSechRes.NavigateUrl = this.hplnkBakSechRes2.NavigateUrl = "~/SearchResult.aspx?action=sechres";
                            this.hplnkBakSechRes.Visible = this.hplnkBakSechRes2.Visible = true;
                        }

                        if (((CustomPrincipal)Context.User).IsInRole(PubEntAdminManager.AdminRole))
                        {
                            userControl =
                            (System.Web.UI.UserControl)this.LoadControl("UserControl/GenDataE.ascx");
                            ((GenDataE)userControl).ID = "GenDataEInfo1";
                            ((GenDataE)userControl).Mode = this.Mode;
                            ((GenDataE)userControl).PubID = this.PubID;
                        }
                        else
                        {
                            userControl =
                            (System.Web.UI.UserControl)this.LoadControl("UserControl/GenDataV.ascx");
                            ((GenDataV)userControl).ID = "GenDataVInfo1";
                            ((GenDataV)userControl).PubID = this.PubID;
                        }
                        
                        this.plcHldGenData.Controls.Add(userControl);

                        if (((CustomPrincipal)Context.User).IsInRole(PubEntAdminManager.AdminRole))
                        {
                            userControl =
                            (System.Web.UI.UserControl)this.LoadControl("UserControl/GenProjSettingE.ascx");
                            ((GenProjSettingE)userControl).ID = "GenProjSettingEInfo1";
                            ((GenProjSettingE)userControl).Mode = this.Mode;
                            ((GenProjSettingE)userControl).PubID = this.PubID;
                        }
                        else
                        {
                            userControl =
                            (System.Web.UI.UserControl)this.LoadControl("UserControl/GenProjSettingV.ascx");
                            ((GenProjSettingV)userControl).ID = "GenProjSettingVInfo1";
                            ((GenProjSettingV)userControl).PubID = this.PubID;
                        }
                        
                        this.plcHldGenProjSetting.Controls.Add(userControl);

                        if (((CustomPrincipal)Context.User).IsInRole(PubEntAdminManager.AdminRole))
                        {
                            this.trLiveInterfaces.Disabled = false;
                        }
                        else
                        {
                            this.trLiveInterfaces.Disabled = true;
                        }
                    }
                    else if ((string)Session[PubEntAdminManager.strPubGlobalMode] ==
                        PubEntAdminManager.strPubGlobalVMode)
                    {
                        //if (((CustomPrincipal)Context.User).IsInRole(PubEntAdminManager.AdminRole) ||
                        //    ((CustomPrincipal)Context.User).IsInRole(PubEntAdminManager.DWHStaffRole))
                        //{
                            this.btnEdit.Visible = this.btnEdit2.Visible = true;
                        //}
                        //else
                        //{
                        //    this.btnEdit.Visible = this.btnEdit2.Visible = false;
                        //}

                        if (PubEntAdminManager.TamperProof)
                        {
                            myUrlBuilder.PageName = "PubRecord.aspx";
                            myUrlBuilder.Query = "mode=edit&pubid=" + this.PubID.ToString();

                            this.btnEdit.Attributes["onclick"] += "window.location = '" + myUrlBuilder.ToString(true) + "'";
                            this.btnEdit2.Attributes["onclick"] += "window.location = '" + myUrlBuilder.ToString(true) + "'";
                        }
                        else
                        {
                            this.btnEdit.Attributes["onclick"] += "window.location = 'PubRecord.aspx?mode=edit&pubid=" +
                                this.PubID + "'";
                            this.btnEdit2.Attributes["onclick"] += "window.location = 'PubRecord.aspx?mode=edit&pubid=" +
                                this.PubID + "'";
                        }

                        this.btnSave.Visible = this.SpellCkr1.Visible = false;
                        this.btnSave2.Visible = this.SpellCkr2.Visible = false;
                        this.btnCancel.Visible = this.btnCancel2.Visible = false;

                        this.lblPageTitle.Text = "View Record";

                        if (PubEntAdminManager.TamperProof)
                        {
                            myUrlBuilder.PageName = "Home.aspx";
                            myUrlBuilder.Query = "action=refine";

                            this.hplnkRefSech.NavigateUrl = this.hplnkRefSech2.NavigateUrl = myUrlBuilder.ToString(true);
                            this.hplnkRefSech.Visible = this.hplnkRefSech2.Visible = true;

                            myUrlBuilder.PageName = "SearchResult.aspx";
                            myUrlBuilder.Query = "action=sechres";

                            this.hplnkBakSechRes.NavigateUrl = this.hplnkBakSechRes2.NavigateUrl = myUrlBuilder.ToString(true);
                            this.hplnkBakSechRes.Visible = this.hplnkBakSechRes2.Visible = true;
                        }
                        else
                        {
                            this.hplnkRefSech.NavigateUrl = this.hplnkRefSech2.NavigateUrl = "~/Home.aspx?action=refine";
                            this.hplnkRefSech.Visible = this.hplnkRefSech2.Visible = true;

                            this.hplnkBakSechRes.NavigateUrl = this.hplnkBakSechRes2.NavigateUrl = "~/SearchResult.aspx?action=sechres";
                            this.hplnkBakSechRes.Visible = this.hplnkBakSechRes2.Visible = true;
                        }

                        userControl =
                            (System.Web.UI.UserControl)this.LoadControl("UserControl/GenDataV.ascx");
                        ((GenDataV)userControl).ID = "GenDataVInfo1";
                        ((GenDataV)userControl).PubID = this.PubID;
                        this.plcHldGenData.Controls.Add(userControl);

                        userControl =
                        (System.Web.UI.UserControl)this.LoadControl("UserControl/GenProjSettingV.ascx");
                        ((GenProjSettingV)userControl).ID = "GenProjSettingVInfo1";
                        ((GenProjSettingV)userControl).PubID = this.PubID;
                        this.plcHldGenProjSetting.Controls.Add(userControl);

                        this.trLiveInterfaces.Disabled = true;
                    }
                }
            }

            
            userControl =
            (System.Web.UI.UserControl)this.LoadControl("UserControl/AdminMenu.ascx");
            this.plcHldMenu.Controls.Add(userControl);

            userControl =
               (System.Web.UI.UserControl)this.LoadControl("UserControl/LiveIntSel.ascx");
            ((LiveIntSel)userControl).RepeatDirection = RepeatDirection.Vertical;
            ((LiveIntSel)userControl).PubID = this.PubID;
            this.plcHldLiveInt.Controls.Add(userControl);

            myLiveIntTab =
               (LiveIntTab)this.LoadControl("UserControl/LiveIntTab.ascx");
            myLiveIntTab.ID = "LiveIntTabInfo1";
            myLiveIntTab.PubID = this.PubID;
            this.plcHldLiveIntTab.Controls.Add(myLiveIntTab);

        }

        protected void Page_PreRender(object sender, System.EventArgs e)
        {
            this.InsertSplChkScript();

            this.SpellCkr1.RefImgSplChk().Attributes.Add("onclick", "PubRecord_SpellCheckClick(1)");
            this.SpellCkr2.RefImgSplChk().Attributes.Add("onclick", "PubRecord_SpellCheckClick(2)");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool blnAddProceed = true;

            //this.Page.Validate();
            if (Page.IsValid)
            {
                bool blnGenDataE = true;

                if (((CustomPrincipal)Context.User).IsInRole(PubEntAdminManager.AdminRole))
                {
                    blnGenDataE = ((GenDataE)this.plcHldGenData.Controls[0]).Save();
                }

                if (this.Mode == PubEntAdminManager.strPubGlobalAMode)
                {
                    if (blnGenDataE)
                    {
                        this.PubID = ((GenDataE)this.plcHldGenData.Controls[0]).PubID;
                        ((GenProjSettingE)this.plcHldGenProjSetting.Controls[0]).PubID = this.PubID;
                        ((LiveIntSel)this.plcHldLiveInt.Controls[0]).PubID = this.PubID;
                        ((LiveIntTab)this.plcHldLiveIntTab.Controls[0]).PubID = this.PubID;
                        //update pub global mode
                        //Session[PubEntAdminManager.strPubGlobalMode] =
                        //    PubEntAdminManager.strPubGlobalEMode;
                    }
                    else
                    {
                        blnAddProceed = false;
                    }
                }

                if (blnAddProceed)
                {
                    bool blnGenProjSettingE = true;
                    bool blnLiveIntSel = true;
                    

                    if (((CustomPrincipal)Context.User).IsInRole(PubEntAdminManager.AdminRole))
                    {
                        blnGenProjSettingE = ((GenProjSettingE)this.plcHldGenProjSetting.Controls[0]).Save();
                        blnLiveIntSel = ((LiveIntSel)this.plcHldLiveInt.Controls[0]).Save();
                    }

                    bool blnLiveIntTab = ((LiveIntTab)this.plcHldLiveIntTab.Controls[0]).Save();

                    if (blnGenDataE && blnGenProjSettingE && blnLiveIntSel && blnLiveIntTab)
                    {
                        Session[PubEntAdminManager.strGlobalMsg] = "Your publication record saved correctly.";

                        if (PubEntAdminManager.TamperProof)
                        {
                            this.myUrlBuilder.PageName = "PubRecord.aspx";
                            this.myUrlBuilder.Query = "mode=view&pubid=" + this.PubID;
                            this.myUrlBuilder.Navigate_useNewQuery(true);
                        }
                        else
                        {
                            Response.Redirect("PubRecord.aspx?mode=view&pubid=" + this.PubID, true);
                        }
                    }
                    else
                    {
                        this.lblMsg.Text = "Your publication record does not save correctly.";
                    }
                }
                else
                {
                    this.lblMsg.Text = Session[PubEntAdminManager.strGlobalMsg].ToString();
                    Session.Remove(PubEntAdminManager.strGlobalMsg);
                }
            }

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (PubEntAdminManager.TamperProof)
            {
                this.myUrlBuilder.PageName = "PubRecord.aspx";
                this.myUrlBuilder.Query = "mode=view&pubid=" + this.PubID;
                this.myUrlBuilder.Navigate_useNewQuery(true);
            }
            else
            {
                Response.Redirect("PubRecord.aspx?mode=view&pubid=" + this.PubID, true);
            }
        }

        #endregion

        #region Methods
        
        protected void AssignValues()
        {
            if (PubEntAdminManager.TamperProof)
            {
                this.PubID = this.myUrlBuilder.QueryString.ContainsKey(PubEntAdminManager.strPubID) ?
                    Convert.ToInt32(this.myUrlBuilder.QueryString[PubEntAdminManager.strPubID]) : -1;

                this.Mode = this.myUrlBuilder.QueryString.ContainsKey(PubEntAdminManager.strPubGlobalMode) ?
                    this.myUrlBuilder.QueryString[PubEntAdminManager.strPubGlobalMode] : PubEntAdminManager.strPubGlobalVMode;
            }
            else
            {
                this.PubID = Request.QueryString[PubEntAdminManager.strPubID] != null ?
                    Convert.ToInt32((string)Request.QueryString[PubEntAdminManager.strPubID]) : -1;

                this.Mode = Request.QueryString[PubEntAdminManager.strPubGlobalMode] != null ?
                   Request.QueryString[PubEntAdminManager.strPubGlobalMode] : PubEntAdminManager.strPubGlobalVMode;
            }

        }

        public string GetSpellCheckParticipants()
        {
            if (Session[PubEntAdminManager.strPubGlobalMode].ToString() !=
                PubEntAdminManager.strPubGlobalVMode)
            {
                if (((CustomPrincipal)Context.User).IsInRole(PubEntAdminManager.AdminRole))
                {
                    return ((GenDataE)this.plcHldGenData.FindControl("GenDataEInfo1")).GetSpellCheckParticipants() + "," +
                           ((GenProjSettingE)this.plcHldGenData.FindControl("GenProjSettingEInfo1")).GetSpellCheckParticipants();
                }
                else
                {
                    return "";
                }
            }
            else
                return "";
        }

        public void InsertSplChkScript()
        {
            if (!Page.ClientScript.IsClientScriptBlockRegistered("PubRecord_ClientScript"))
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "PubRecord_ClientScript",
                   @"
            
                window.onload = PubRecord_Onload
                function PubRecord_Onload()
                {
                    LiveIntTabOnload();

                    if (typeof monitorTabChangesIDs != 'undefined')
                    {
                        assignInitialValuesForMonitorTabChanges();
                    }
                }

                function PubRecord_SpellCheckClick(ctrlIndex)
                {
                    var langsel = document.getElementById('" + this.SpellCkr1.LangSelClientID + @"').
                        options[document.getElementById('" + this.SpellCkr1.LangSelClientID + @"').selectedIndex].value;

                    var langsel2 = document.getElementById('" + this.SpellCkr2.LangSelClientID + @"').
                        options[document.getElementById('" + this.SpellCkr2.LangSelClientID + @"').selectedIndex].value;

                    var finallangsel = '28001';//default to e

                    var t = '';
                    if(typeof TabParticipants == 'function') {
                        t = TabParticipants();
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
                        if (t.length>0)
                        {
                                window.open('" + ConfigurationSettings.AppSettings["SpellingCheckerLoc"] +
                                "?" + PubEntAdminManager.SPELLCHECK2CHECKFIELDSLIST + "=" + this.GetSpellCheckParticipants() +
                                @",'+t+'','winspell','toolbar=0,location=0,directories=0,status=0,scrollbars=0,resizable=0,width=500,height=220');
                        }
                        else
                        {
                                window.open('" + ConfigurationSettings.AppSettings["SpellingCheckerLoc"] +
                                "?" + PubEntAdminManager.SPELLCHECK2CHECKFIELDSLIST + "=" + this.GetSpellCheckParticipants() +
                                @"','winspell','toolbar=0,location=0,directories=0,status=0,scrollbars=0,resizable=0,width=500,height=220');
                        }
                    }
                    else if (finallangsel == '29552')//s
                    {
                        if (t.length>0)
                        {
                                window.open('" + ConfigurationSettings.AppSettings["SpellingCheckerLoc"] +
                                "?" + PubEntAdminManager.SPELLCHECK2CHECKFIELDSLIST + "=" + this.GetSpellCheckParticipants() +
                                @",'+t+'&dict=SPANISH','winspell','toolbar=0,location=0,directories=0,status=0,scrollbars=0,resizable=0,width=500,height=220');
                        }
                        else
                        {
                                window.open('" + ConfigurationSettings.AppSettings["SpellingCheckerLoc"] +
                                "?" + PubEntAdminManager.SPELLCHECK2CHECKFIELDSLIST + "=" + this.GetSpellCheckParticipants() +
                                @"&dict=SPANISH','winspell','toolbar=0,location=0,directories=0,status=0,scrollbars=0,resizable=0,width=500,height=220');
                        }
                    }
                    
                }
                ", true);
            }
        }

        protected void ByPassRegisterMonitoredChanges()
        {
            PubEntAdminManager.BypassModifiedMethod(this.btnSave, false);
            PubEntAdminManager.BypassModifiedMethod(this.btnSave2, false);
            PubEntAdminManager.BypassModifiedMethod(this.btnEdit, false);
            PubEntAdminManager.BypassModifiedMethod(this.btnEdit2, false);
            PubEntAdminManager.BypassModifiedMethod(this.SpellCkr1.LangSel(), false);
            PubEntAdminManager.BypassModifiedMethod(this.SpellCkr2.LangSel(), false);
        }

        #endregion

        #region Properties

        public int PubID
        {
            set {this.intPubId = value;}
            get {return this.intPubId;}
        }

        public string Mode
        {
            set { this.strMode = value; }
            get { return this.strMode; }
        }

        public ScriptManager PubRcrdPageScriptMger
        {
            get { return this.ScriptManager_PubRcod; }
        }

        public string GetPURL
        {
            get {
                //Control c = this.plcHldGenData.FindControl("GenDataEInfo1");
                //if (c != null)
                    return ((GenDataE)this.plcHldGenData.Controls[0]).URLClientID;
                //else
                //    return "";
                }
        }

        public string GetPPDFURL
        {
            get
            {
                return ((GenDataE)this.plcHldGenData.Controls[0]).PDFURLClientID;
               
            }
        }

        public string GetSaveBtn1
        {
            get {return this.btnSave.ClientID;}
        }

        public string GetSaveBtn2
        {
            get { return this.btnSave2.ClientID; }
        }

        public string SpellCkr1Lang
        {
            get { return this.SpellCkr1.LangSelClientID; }
        }

        public string SpellCkr2Lang
        {
            get { return this.SpellCkr2.LangSelClientID; }
        }
        public string PageTitle
        {
            set
            {
                ((Label)this.FindControl("lblPageTitle")).Text = value;
            }
            get
            {
                return ((Label)this.FindControl("lblPageTitle")).Text;
            }
        }
        #endregion

        

    }
}
