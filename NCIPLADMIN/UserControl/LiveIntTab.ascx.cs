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
using System.IO;

using AjaxControlToolkit;
using PubEntAdmin.BLL;
using PubEntAdmin.DAL;

namespace PubEntAdmin.UserControl
{
    public partial class LiveIntTab : System.Web.UI.UserControl
    {
        #region Fields
        private int intPubID;
        #endregion

        #region Event Handler
        protected void Page_Init(object sender, EventArgs e)
        {
            //((PubRecord)this.Page).PubRcrdPageScriptMger.Navigate += new EventHandler<HistoryEventArgs>(PubRcrdPageScriptMger_Navigate);

            if (!Page.IsPostBack)
            {
                //this.BindDefTabData();
                //if (Request.QueryString["mode"] == "add")
                //{
                //    Session["TabContentCtrlMode"] = "edit";
                //}
                //else
                //{
                //Session[PubEntAdminManager.strPubGlobalMode] = PubEntAdminManager.strPubGlobalVMode;
                //}
                if (Session[PubEntAdminManager.strTabContentCurrActTabIndex] == null)
                {
                    Session[PubEntAdminManager.strTabContentCurrActTabIndex] = 0;
                    this.tabContLiveInt.ActiveTabIndex = 0;
                }
            }
            //else
            //{
            //    if (Session[PubEntAdminManager.strTabContentPrevActTabIndex] == null)
            //    {
            //        this.udpUpdate();
            //    }
            //}

            #region not use
            //if (Session[PubEntAdminManager.strPubGlobalMode] != null)
            //{
            //    if ((string)Session[PubEntAdminManager.strPubGlobalMode] ==
            //        PubEntAdminManager.strPubGlobalAMode)
            //    {
            //        //AC -- 10/24/2008
            //        //disable all tabs, except Pub Hist
            //        //should be done each post back, but need to check how many different
            //        //interface are enabled/selected
            //        //NEED TO DO: ADD CODE TO RETRIEVE HOW MANY TABS SHOULD BE ENABLED
            //        //for (int i = 1; i < this.tabContLiveInt.Tabs.Count; i++)
            //        //{
            //        //    if (i != 5 && i != 6)
            //        //        this.tabContLiveInt.Tabs[i].Enabled = false;
            //        //}
            //    }
            //    else if ((string)Session[PubEntAdminManager.strPubGlobalMode] ==
            //        PubEntAdminManager.strPubGlobalEMode)
            //    {
            //        //retrieve data
            //        //fakeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee
            //        //for (int i = 1; i < this.tabContLiveInt.Tabs.Count; i++)
            //        //{
            //        //    if (i != 1 && i != 5 && i != 6)
            //        //        this.tabContLiveInt.Tabs[i].Enabled = false;
            //        //}
            //    }
            //    else if ((string)Session[PubEntAdminManager.strPubGlobalMode] ==
            //        PubEntAdminManager.strPubGlobalVMode)
            //    {
            //        //retrieve data
            //        //fakeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee
            //        //for (int i = 1; i < this.tabContLiveInt.Tabs.Count; i++)
            //        //{
            //        //    if (i != 1 && i != 5 && i != 6)
            //        //        this.tabContLiveInt.Tabs[i].Enabled = false;
            //        //}
            //    }
            //}

            //else//Just in case
            //{
            //    if (Request.QueryString[PubEntAdminManager.strPubGlobalMode] ==
            //        PubEntAdminManager.strPubGlobalAMode)
            //    {
            //        //AC -- 10/24/2008
            //        //disable all tabs, except Pub Hist
            //        //should be done each post back, but need to check how many different
            //        //interface are enabled/selected
            //        //NEED TO DO: ADD CODE TO RETRIEVE HOW MANY TABS SHOULD BE ENABLED
            //        for (int i = 1; i < this.tabContLiveInt.Tabs.Count; i++)
            //        {
            //            if (i != 5 && i != 6)
            //                this.tabContLiveInt.Tabs[i].Enabled = false;
            //        }
            //    }
            //    else if (Request.QueryString[PubEntAdminManager.strPubGlobalMode] ==
            //        PubEntAdminManager.strPubGlobalVMode)
            //    {
            //        //fakeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee
            //        for (int i = 1; i < this.tabContLiveInt.Tabs.Count; i++)
            //        {
            //            if (i != 1 && i != 5 && i != 6)
            //                this.tabContLiveInt.Tabs[i].Enabled = false;
            //        }
            //    }
            //}
            #endregion

            this.Recreation();
        }

        void PubRcrdPageScriptMger_Navigate(object sender, HistoryEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.State[PubEntAdminManager.strPubGlobalMode]) &&
                !string.IsNullOrEmpty(e.State[PubEntAdminManager.strTabContentCurrActTabIndex]))
            {
                string PubGlobalMode = e.State[PubEntAdminManager.strPubGlobalMode];
                string CurrTabIndex = e.State[PubEntAdminManager.strTabContentCurrActTabIndex];

                Session[PubEntAdminManager.strPubGlobalMode] = PubGlobalMode;

                System.Web.UI.UserControl userControl;

                if (PubGlobalMode == PubEntAdminManager.strPubGlobalAMode)
                {

                }
                else if (PubGlobalMode == PubEntAdminManager.strPubGlobalEMode)
                {
                    if (1 == 1) // temp auth fix
                    {
                        switch (CurrTabIndex)
                        {
                            case "0":
                                break;
                            case "1":
                                userControl = (System.Web.UI.UserControl)this.LoadControl("NCIPLTabEditInfo.ascx");
                                userControl.ID = "NCIPLTabEditInfo1";
                                ((NCIPLTabEditInfo)userControl).PubID = this.PubID;
                                this.plHldNCIPLTabInfo.Controls.Clear();
                                this.plHldNCIPLTabInfo.Controls.Add(userControl);
                                Session[PubEntAdminManager.strTabContentCurrActTabIndex] = 1;
                                this.tabContLiveInt.ActiveTab = this.tabpnlNCIPL;
                                this.tabContLiveInt.ActiveTabIndex = 1;
                                this.udpUpdate();
                                break;
                            case "2":
                                userControl = (System.Web.UI.UserControl)this.LoadControl("ROOTabEditInfo.ascx");
                                userControl.ID = "ROOTabEditInfo1";
                                ((ROOTabEditInfo)userControl).PubID = this.PubID;
                                this.plHldROOTabInfo.Controls.Clear();
                                this.plHldROOTabInfo.Controls.Add(userControl);
                                Session[PubEntAdminManager.strTabContentCurrActTabIndex] = 2;
                                this.tabContLiveInt.ActiveTab = this.tabpnlROO;
                                this.tabContLiveInt.ActiveTabIndex = 2;
                                this.udpUpdate();
                                break;
                            case "3":
                                break;
                        }
                    }
                    else if (2 == 2) // temp auth fix
                    {

                    }
                    else if (3 == 3) // temp auth fix
                    {

                    }
                }
                else if (PubGlobalMode == PubEntAdminManager.strPubGlobalVMode)
                {

                }
            }

        }

        protected void OnActiveTabChanged(object sender, EventArgs e)
        {

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //string script = @"WebForm_InitCallback=function() {};";
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "removeWebForm_InitCallback", script, true);

        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (Session[PubEntAdminManager.strReloadPubHist] != null)
            {
                if (System.Convert.ToBoolean(Session[PubEntAdminManager.strReloadPubHist].ToString()))
                {
                    //System.Web.UI.UserControl userControl = (System.Web.UI.UserControl)this.LoadControl("PubHistTabReadInfo.ascx");
                    //userControl.ID = "PubHistTabReadInfo";
                    //this.plHldPubHistTabViewInfo.Controls.Clear();
                    //this.plHldPubHistTabViewInfo.Controls.Add(userControl);

                    System.Web.UI.UserControl userControl = (System.Web.UI.UserControl)this.plHldPubHistTabViewInfo.FindControl("PubHistTabReadInfo1");
                    ((PubHistTabReadInfo)userControl).ReloadData();
                    Session.Remove(PubEntAdminManager.strReloadPubHist);
                }
            }

            if (Session[PubEntAdminManager.strReloadRelatedPub] != null)
            {
                if (System.Convert.ToBoolean(Session[PubEntAdminManager.strReloadRelatedPub].ToString()))
                {
                    System.Web.UI.UserControl userControl =
                        (System.Web.UI.UserControl)this.plHldRelatedTabInfo.FindControl("RelatedTabEditInfo1");
                    ((RelatedTabEditInfo)userControl).ReBindData();
                    Session.Remove(PubEntAdminManager.strReloadRelatedPub);
                }
            }

            if (Session[PubEntAdminManager.strReloadRelatedTranslation] != null)
            {
                if (System.Convert.ToBoolean(Session[PubEntAdminManager.strReloadRelatedTranslation].ToString()))
                {
                    System.Web.UI.UserControl userControl =
                        (System.Web.UI.UserControl)this.plHldTranslationTabInfo.FindControl("TranslationTabEditInfo1");
                    ((TranslationTabEditInfo)userControl).ReBindData();
                    Session.Remove(PubEntAdminManager.strReloadRelatedTranslation);
                }
            }

            //if (!Page.IsPostBack)
            //{
            //    this.tabContLiveInt.ActiveTabIndex = 0;
            //}
            //else
            //{
            //    this.tabContLiveInt.ActiveTabIndex = (int)Session[PubEntAdminManager.strTabContentCurrActTabIndex];
            //}

            if (Session[PubEntAdminManager.strTabContentCurrActTabIndex] != null)
            {
                this.hidCurrTabIndex.Value = Session[PubEntAdminManager.strTabContentCurrActTabIndex].ToString();
                this.tabContLiveInt.ActiveTabIndex = (int)Session[PubEntAdminManager.strTabContentCurrActTabIndex];
            }
            else
            {
                this.hidCurrTabIndex.Value = "0";
                this.tabContLiveInt.ActiveTabIndex = 0;
            }
        }

        #region Tabs Header
        protected void btnPubHist_Click(object sender, EventArgs args)
        {
            System.Web.UI.UserControl userControl;

            if (Session[PubEntAdminManager.strPubGlobalMode] != null)
            {
                Session[PubEntAdminManager.strTabContentPrevActTabIndex] =
                    Session[PubEntAdminManager.strTabContentCurrActTabIndex];
                Session[PubEntAdminManager.strTabContentCurrActTabIndex] = 0;

                if ((string)Session[PubEntAdminManager.strPubGlobalMode] ==
                    PubEntAdminManager.strPubGlobalAMode)
                {
                    //userControl = (System.Web.UI.UserControl)this.LoadControl("PubHistTabEditInfo.ascx");
                    //userControl.ID = "PubHistTabEditInfo1";
                    //((PubHistTabEditInfo)userControl).PubID = this.PubID;
                    Label l = new Label();
                    l.Text = "Add Publication History information after creating the publication.";
                    this.plHldPubHistTabAddInfo.Controls.Clear();
                    this.plHldPubHistTabAddInfo.Controls.Add(l);
                }
                else if ((string)Session[PubEntAdminManager.strPubGlobalMode] ==
                    PubEntAdminManager.strPubGlobalEMode)
                {
                    if (1 == 1) // temp auth fix
                    // if (((CustomPrincipal)Context.User).IsInRole(PubEntAdminManager.AdminRole) ||
                    //        ((CustomPrincipal)Context.User).IsInRole(PubEntAdminManager.DWHStaffRole))
                    {
                        userControl = (System.Web.UI.UserControl)this.LoadControl("PubHistTabEditInfo.ascx");
                        userControl.ID = "PubHistTabEditInfo1";
                        ((PubHistTabEditInfo)userControl).PubID = this.PubID;
                        this.plHldPubHistTabAddInfo.Controls.Clear();
                        this.plHldPubHistTabAddInfo.Controls.Add(userControl);                      
                    }
                    else
                    {
                        userControl = (System.Web.UI.UserControl)this.LoadControl("PubHistTabReadInfo.ascx");
                        userControl.ID = "PubHistTabReadInfo1";
                        ((PubHistTabReadInfo)userControl).PubID = this.PubID;
                        this.plHldPubHistTabViewInfo.Controls.Clear();
                        this.plHldPubHistTabViewInfo.Controls.Add(userControl);
                    }
                }
                else
                {
                    userControl = (System.Web.UI.UserControl)this.LoadControl("PubHistTabReadInfo.ascx");
                    userControl.ID = "PubHistTabReadInfo1";
                    ((PubHistTabReadInfo)userControl).PubID = this.PubID;
                    this.plHldPubHistTabViewInfo.Controls.Clear();
                    this.plHldPubHistTabViewInfo.Controls.Add(userControl);
                }
                
            }

            
            this.udpUpdate();
        }
        protected void btnNCIPL_Click(object sender, EventArgs args)
        {
            System.Web.UI.UserControl userControl;
            if (Session[PubEntAdminManager.strPubGlobalMode] != null)
            {
                Session[PubEntAdminManager.strTabContentPrevActTabIndex] =
                    Session[PubEntAdminManager.strTabContentCurrActTabIndex];
                Session[PubEntAdminManager.strTabContentCurrActTabIndex] = 1;

                if ((string)Session[PubEntAdminManager.strPubGlobalMode] ==
                    PubEntAdminManager.strPubGlobalAMode)
                {
                    if (1 == 1) // temp auth fix
                    {
                        userControl =
                   (System.Web.UI.UserControl)this.LoadControl("NCIPLTabEditInfo.ascx");
                        userControl.ID = "NCIPLTabEditInfo1";
                        this.plHldNCIPLTabInfo.Controls.Clear();
                        this.plHldNCIPLTabInfo.Controls.Add(userControl);

                       // Response.Redirect("PubRecord.aspx?mode=edit&pubid=" + this.PubID);
                    }
                    else
                    {
                        if (PubEntAdminManager.TamperProof)
                        {
                            PubEntAdminManager.RedirectEncodedURLWithQS("UnauthorizedAccess.aspx",
                                PubEntAdminManager.strUnauthorizedDetail + "=Try to access pub record (NCIPL) in Add mode");
                        }
                        else
                        {
                            Response.Redirect("UnauthorizedAccess.aspx?" +
                                PubEntAdminManager.strUnauthorizedDetail + "=Try to access pub record (NCIPL) in Add mode", true);
                        }
                    }
                }
                else if ((string)Session[PubEntAdminManager.strPubGlobalMode] ==
                    PubEntAdminManager.strPubGlobalEMode)
                {
                    if (1 == 1) // temp auth fix
                    {
                        userControl =
                       (System.Web.UI.UserControl)this.LoadControl("NCIPLTabEditInfo.ascx");
                        userControl.ID = "NCIPLTabEditInfo1";
                        ((NCIPLTabEditInfo)userControl).PubID = this.PubID;
                        this.plHldNCIPLTabInfo.Controls.Clear();
                        this.plHldNCIPLTabInfo.Controls.Add(userControl);

                       // Response.Redirect("PubRecord.aspx?mode=edit&pubid=" + this.PubID);
                    }
                    else
                    {
                        userControl =
                   (System.Web.UI.UserControl)this.LoadControl("NCIPLTabReadInfo.ascx");
                        userControl.ID = "NCIPLTabReadInfo1";
                        ((NCIPLTabReadInfo)userControl).PubID = this.PubID;
                        this.plHldNCIPLTabInfo.Controls.Clear();
                        this.plHldNCIPLTabInfo.Controls.Add(userControl);
                    }

                    //((PubRecord)this.Page).PubRcrdPageScriptMger.AddHistoryPoint(PubEntAdminManager.strPubGlobalMode,
                    //    PubEntAdminManager.strPubGlobalEMode);

                }
                else if ((string)Session[PubEntAdminManager.strPubGlobalMode] ==
                    PubEntAdminManager.strPubGlobalVMode)
                {
                    userControl =
                   (System.Web.UI.UserControl)this.LoadControl("NCIPLTabReadInfo.ascx");
                    userControl.ID = "NCIPLTabReadInfo1";
                    ((NCIPLTabReadInfo)userControl).PubID = this.PubID;
                    this.plHldNCIPLTabInfo.Controls.Clear();
                    this.plHldNCIPLTabInfo.Controls.Add(userControl);
                }
            }
            //((PubRecord)this.Page).PubRcrdPageScriptMger.AddHistoryPoint(PubEntAdminManager.strTabContentCurrActTabIndex,
            //                    Session[PubEntAdminManager.strTabContentCurrActTabIndex].ToString());
            this.udpUpdate();

        }
        protected void btnROO_Click(object sender, EventArgs args)
        {
            System.Web.UI.UserControl userControl;

            if (Session[PubEntAdminManager.strPubGlobalMode] != null)
            {
                Session[PubEntAdminManager.strTabContentPrevActTabIndex] =
                    Session[PubEntAdminManager.strTabContentCurrActTabIndex];
                Session[PubEntAdminManager.strTabContentCurrActTabIndex] = 2;

                if ((string)Session[PubEntAdminManager.strPubGlobalMode] ==
                    PubEntAdminManager.strPubGlobalAMode)
                {
                    if (1 == 1) // temp auth fix
                    {
                        userControl =
                       (System.Web.UI.UserControl)this.LoadControl("ROOTabEditInfo.ascx");
                        userControl.ID = "ROOTabEditInfo1";
                        //((ROOTabEditInfo)userControl).PubID = this.PubID;
                        this.plHldROOTabInfo.Controls.Clear();
                        this.plHldROOTabInfo.Controls.Add(userControl);
                    }
                    else
                    {
                        if (PubEntAdminManager.TamperProof)
                        {
                            PubEntAdminManager.RedirectEncodedURLWithQS("UnauthorizedAccess.aspx",
                                PubEntAdminManager.strUnauthorizedDetail + "=Try to access pub record (ROO) in Add mode");
                        }
                        else
                        {
                            Response.Redirect("UnauthorizedAccess.aspx?" +
                                PubEntAdminManager.strUnauthorizedDetail + "=Try to access pub record (ROO) in Add mode", true);
                        }
                    }
                }
                else if ((string)Session[PubEntAdminManager.strPubGlobalMode] ==
                    PubEntAdminManager.strPubGlobalEMode)
                {
                    if (1 == 1) // temp auth fix
                    {
                        userControl =
                       (System.Web.UI.UserControl)this.LoadControl("ROOTabEditInfo.ascx");
                        userControl.ID = "ROOTabEditInfo1";
                        ((ROOTabEditInfo)userControl).PubID = this.PubID;
                        this.plHldROOTabInfo.Controls.Clear();
                        this.plHldROOTabInfo.Controls.Add(userControl);
                    }
                    else
                    {
                        userControl =
                   (System.Web.UI.UserControl)this.LoadControl("ROOTabReadInfo.ascx");
                        userControl.ID = "ROOTabReadInfo1";
                        ((ROOTabReadInfo)userControl).PubID = this.PubID;
                        this.plHldROOTabInfo.Controls.Clear();
                        this.plHldROOTabInfo.Controls.Add(userControl);
                    }

                    //((PubRecord)this.Page).PubRcrdPageScriptMger.AddHistoryPoint(PubEntAdminManager.strPubGlobalMode,
                    //    PubEntAdminManager.strPubGlobalEMode);
                }
                else if ((string)Session[PubEntAdminManager.strPubGlobalMode] ==
                    PubEntAdminManager.strPubGlobalVMode)
                {
                    userControl =
                   (System.Web.UI.UserControl)this.LoadControl("ROOTabReadInfo.ascx");
                    userControl.ID = "ROOTabReadInfo1";
                    ((ROOTabReadInfo)userControl).PubID = this.PubID;
                    this.plHldROOTabInfo.Controls.Clear();
                    this.plHldROOTabInfo.Controls.Add(userControl);
                }
            }

            
            //((PubRecord)this.Page).PubRcrdPageScriptMger.AddHistoryPoint(PubEntAdminManager.strTabContentCurrActTabIndex,
            //                    Session[PubEntAdminManager.strTabContentCurrActTabIndex].ToString());
            this.udpUpdate();
        }
        protected void btnExh_Click(object sender, EventArgs args)
        {
            System.Web.UI.UserControl userControl;

            if (Session[PubEntAdminManager.strPubGlobalMode] != null)
            {
                Session[PubEntAdminManager.strTabContentPrevActTabIndex] =
                    Session[PubEntAdminManager.strTabContentCurrActTabIndex];

                Session[PubEntAdminManager.strTabContentCurrActTabIndex] = 3;

                if ((string)Session[PubEntAdminManager.strPubGlobalMode] ==
                    PubEntAdminManager.strPubGlobalAMode)
                {
                    if (1 == 1) // temp auth fix
                    {
                        userControl =
                       (System.Web.UI.UserControl)this.LoadControl("ExhTabEditInfo.ascx");
                        userControl.ID = "ExhTabEditInfo1";
                        this.plHldExhTabInfo.Controls.Clear();
                        this.plHldExhTabInfo.Controls.Add(userControl);

                        //yma quoted it out since it cause js alert leaving the page. 5/13/2014
                        //Response.Redirect("PubRecord.aspx?mode=edit&pubid=" + this.PubID);
                    }
                    else
                    {
                        if (PubEntAdminManager.TamperProof)
                        {
                            PubEntAdminManager.RedirectEncodedURLWithQS("UnauthorizedAccess.aspx",
                                PubEntAdminManager.strUnauthorizedDetail + "=Try to access pub record (EXH) in Add mode");
                        }
                        else
                        {
                            Response.Redirect("UnauthorizedAccess.aspx?" +
                                PubEntAdminManager.strUnauthorizedDetail + "=Try to access pub record (EXH) in Add mode", true);
                        }
                    }
                }
                else if ((string)Session[PubEntAdminManager.strPubGlobalMode] ==
                    PubEntAdminManager.strPubGlobalEMode)
                {
                    if (1 == 1) // temp auth fix
                    {
                        userControl =
                       (System.Web.UI.UserControl)this.LoadControl("ExhTabEditInfo.ascx");
                        userControl.ID = "ExhTabEditInfo1";
                        ((ExhTabEditInfo)userControl).PubID = this.PubID;
                        this.plHldExhTabInfo.Controls.Clear();
                        this.plHldExhTabInfo.Controls.Add(userControl);
                        
                        //yma quoted it out since it cause js alert leaving the page. 5/13/2014
                        //Response.Redirect("PubRecord.aspx?mode=edit&pubid=" + this.PubID);
                    }
                    else
                    {
                        userControl =
                   (System.Web.UI.UserControl)this.LoadControl("ExhTabReadInfo.ascx");
                        userControl.ID = "ExhTabReadInfo1";
                        ((ExhTabReadInfo)userControl).PubID = this.PubID;
                        this.plHldExhTabInfo.Controls.Clear();
                        this.plHldExhTabInfo.Controls.Add(userControl);
                    }
                }
                else if ((string)Session[PubEntAdminManager.strPubGlobalMode] ==
                    PubEntAdminManager.strPubGlobalVMode)
                {
                    userControl =
                   (System.Web.UI.UserControl)this.LoadControl("ExhTabReadInfo.ascx");
                    userControl.ID = "ExhTabReadInfo1";
                    ((ExhTabReadInfo)userControl).PubID = this.PubID;
                    this.plHldExhTabInfo.Controls.Clear();
                    this.plHldExhTabInfo.Controls.Add(userControl);
                }
            }

            
            this.udpUpdate();
        }
        protected void btnCatalog_Click(object sender, EventArgs args)
        {
            
            System.Web.UI.UserControl userControl;
            if (Session[PubEntAdminManager.strPubGlobalMode] != null)
            {
                Session[PubEntAdminManager.strTabContentPrevActTabIndex] =
                    Session[PubEntAdminManager.strTabContentCurrActTabIndex];
                Session[PubEntAdminManager.strTabContentCurrActTabIndex] = 4;

                if ((string)Session[PubEntAdminManager.strPubGlobalMode] ==
                    PubEntAdminManager.strPubGlobalAMode)
                {
                    if (1 == 1) // temp auth fix
                    {
                        userControl =
                       (System.Web.UI.UserControl)this.LoadControl("CatalogTabEditInfo.ascx");
                        userControl.ID = "CatalogTabEditInfo1";
                        this.plHldCatalogTabInfo.Controls.Clear();
                        this.plHldCatalogTabInfo.Controls.Add(userControl);
                    }
                    else
                    {
                        if (PubEntAdminManager.TamperProof)
                        {
                            PubEntAdminManager.RedirectEncodedURLWithQS("UnauthorizedAccess.aspx",
                                PubEntAdminManager.strUnauthorizedDetail + "=Try to access pub record (Catalog) in Add mode");
                        }
                        else
                        {
                            Response.Redirect("UnauthorizedAccess.aspx?" +
                                PubEntAdminManager.strUnauthorizedDetail + "=Try to access pub record (Catalog) in Add mode", true);
                        }
                    }
                }
                else if ((string)Session[PubEntAdminManager.strPubGlobalMode] ==
                    PubEntAdminManager.strPubGlobalEMode)
                {
                    if (1 == 1) // temp auth fix
                    {
                        userControl =
                       (System.Web.UI.UserControl)this.LoadControl("CatalogTabEditInfo.ascx");
                        userControl.ID = "CatalogTabEditInfo1";
                        ((CatalogTabEditInfo)userControl).PubID = this.PubID;
                        this.plHldCatalogTabInfo.Controls.Clear();
                        this.plHldCatalogTabInfo.Controls.Add(userControl);
                    }
                    else
                    {
                        userControl =
                   (System.Web.UI.UserControl)this.LoadControl("CatalogTabReadInfo.ascx");
                        userControl.ID = "CatalogTabReadInfo1";
                        ((CatalogTabReadInfo)userControl).PubID = this.PubID;
                        this.plHldCatalogTabInfo.Controls.Clear();
                        this.plHldCatalogTabInfo.Controls.Add(userControl);
                    }
                }
                else if ((string)Session[PubEntAdminManager.strPubGlobalMode] ==
                    PubEntAdminManager.strPubGlobalVMode)
                {
                    userControl =
                   (System.Web.UI.UserControl)this.LoadControl("CatalogTabReadInfo.ascx");
                    userControl.ID = "CatalogTabReadInfo1";
                    ((CatalogTabReadInfo)userControl).PubID = this.PubID;
                    this.plHldCatalogTabInfo.Controls.Clear();
                    this.plHldCatalogTabInfo.Controls.Add(userControl);
                }
            }
            
            this.udpUpdate();
        }
        protected void btnCmt_Click(object sender, EventArgs args)
        {
            Session[PubEntAdminManager.strTabContentPrevActTabIndex] =
                    Session[PubEntAdminManager.strTabContentCurrActTabIndex];
            Session[PubEntAdminManager.strTabContentCurrActTabIndex] = 5;

            System.Web.UI.UserControl userControl = 
                (System.Web.UI.UserControl)this.LoadControl("Comment.ascx");
            userControl.ID = "Comment1";
            ((Comment)userControl).PubID = this.PubID;
            if ((string)Session[PubEntAdminManager.strPubGlobalMode] ==
                            PubEntAdminManager.strPubGlobalAMode)
            {
                ((Comment)userControl).Mode = PubEntAdminManager.strPubGlobalAMode;
            }
            else if ((string)Session[PubEntAdminManager.strPubGlobalMode] ==
                            PubEntAdminManager.strPubGlobalEMode)
            {
                ((Comment)userControl).Mode = PubEntAdminManager.strPubGlobalEMode;
            }
            else if ((string)Session[PubEntAdminManager.strPubGlobalMode] ==
                    PubEntAdminManager.strPubGlobalVMode)
            {
                ((Comment)userControl).Mode = PubEntAdminManager.strPubGlobalVMode;
            }
            this.plHldCmtTabInfo.Controls.Clear();
            this.plHldCmtTabInfo.Controls.Add(userControl);

            this.udpUpdate();
        }
        protected void btnAttach_Click(object sender, EventArgs args)
        {
            Session[PubEntAdminManager.strTabContentPrevActTabIndex] =
                    Session[PubEntAdminManager.strTabContentCurrActTabIndex];
            Session[PubEntAdminManager.strTabContentCurrActTabIndex] = 6;

            System.Web.UI.UserControl userControl =
                (System.Web.UI.UserControl)this.LoadControl("Attachments.ascx");
            userControl.ID = "Attachments1";
            ((Attachments)userControl).PubID = this.PubID;
            this.plHldAttachTabInfo.Controls.Clear();
            this.plHldAttachTabInfo.Controls.Add(userControl);

            this.udpUpdate();
        }
        protected void btnRelated_Click(object sender, EventArgs args)
        {
            System.Web.UI.UserControl userControl;

            if (Session[PubEntAdminManager.strPubGlobalMode] != null)
            {
                Session[PubEntAdminManager.strTabContentPrevActTabIndex] =
                    Session[PubEntAdminManager.strTabContentCurrActTabIndex];
                Session[PubEntAdminManager.strTabContentCurrActTabIndex] = 7;

                if ((string)Session[PubEntAdminManager.strPubGlobalMode] ==
                    PubEntAdminManager.strPubGlobalAMode)
                {
                    if (1 == 1) // temp auth fix
                    {
                        userControl =
                            (System.Web.UI.UserControl)this.LoadControl("RelatedTabEditInfo.ascx");
                        userControl.ID = "RelatedTabEditInfo1";

                        //get rid of this in actual coding
                        //((RelatedTabEditInfo)userControl).InitialAddLoad = true;

                        this.plHldRelatedTabInfo.Controls.Clear();
                        this.plHldRelatedTabInfo.Controls.Add(userControl);

                        //this.trRelated.Visible = true;
                    }
                    else
                    {
                        if (PubEntAdminManager.TamperProof)
                        {
                            PubEntAdminManager.RedirectEncodedURLWithQS("UnauthorizedAccess.aspx",
                                PubEntAdminManager.strUnauthorizedDetail + "=Try to access pub record (Related) in Add mode");
                        }
                        else
                        {
                            Response.Redirect("UnauthorizedAccess.aspx?" +
                                PubEntAdminManager.strUnauthorizedDetail + "=Try to access pub record (Related) in Add mode", true);
                        }
                    }
                }
                else if ((string)Session[PubEntAdminManager.strPubGlobalMode] ==
                    PubEntAdminManager.strPubGlobalEMode)
                {
                    if (1 == 1) // temp auth fix
                    {
                        userControl =
                            (System.Web.UI.UserControl)this.LoadControl("RelatedTabEditInfo.ascx");
                        userControl.ID = "RelatedTabEditInfo1";
                        ((RelatedTabEditInfo)userControl).PubID = this.PubID;
                        this.plHldRelatedTabInfo.Controls.Clear();
                        this.plHldRelatedTabInfo.Controls.Add(userControl);

                        //this.trRelated.Visible = true;
                    }
                    else
                    {
                        userControl =
                        (System.Web.UI.UserControl)this.LoadControl("RelatedTabReadInfo.ascx");
                        userControl.ID = "RelatedTabReadInfo1";
                        ((RelatedTabReadInfo)userControl).PubID = this.PubID;
                        this.plHldRelatedTabInfo.Controls.Clear();
                        this.plHldRelatedTabInfo.Controls.Add(userControl);

                        //this.trRelated.Visible = false;
                    }
                }
                else if ((string)Session[PubEntAdminManager.strPubGlobalMode] ==
                    PubEntAdminManager.strPubGlobalVMode)
                {
                    userControl =
                        (System.Web.UI.UserControl)this.LoadControl("RelatedTabReadInfo.ascx");
                    userControl.ID = "RelatedTabReadInfo1";
                    ((RelatedTabReadInfo)userControl).PubID = this.PubID;
                    this.plHldRelatedTabInfo.Controls.Clear();
                    this.plHldRelatedTabInfo.Controls.Add(userControl);

                    //this.trRelated.Visible = false;
                }
            }
            
            this.udpUpdate();
        }
        protected void btnTranslation_Click(object sender, EventArgs args)
        {
            System.Web.UI.UserControl userControl;

            if (Session[PubEntAdminManager.strPubGlobalMode] != null)
            {
                Session[PubEntAdminManager.strTabContentPrevActTabIndex] =
                    Session[PubEntAdminManager.strTabContentCurrActTabIndex];
                Session[PubEntAdminManager.strTabContentCurrActTabIndex] = 8;

                if ((string)Session[PubEntAdminManager.strPubGlobalMode] ==
                    PubEntAdminManager.strPubGlobalAMode)
                {
                    if (1 == 1) // temp auth fix
                    {
                        userControl =
                            (System.Web.UI.UserControl)this.LoadControl("TranslationTabEditInfo.ascx");
                        userControl.ID = "TranslationTabEditInfo1";

                        //get rid of this in actual coding
                        //((TranslationTabEditInfo)userControl).InitialAddLoad = true;

                        this.plHldTranslationTabInfo.Controls.Clear();
                        this.plHldTranslationTabInfo.Controls.Add(userControl);

                        //this.RegisterMonitoredChanges(8);
                        //this.ByPassRegisterMonitoredChanges(8);

                        //this.trTranslation.Visible = true;
                    }
                    else
                    {
                        if (PubEntAdminManager.TamperProof)
                        {
                            PubEntAdminManager.RedirectEncodedURLWithQS("UnauthorizedAccess.aspx",
                                PubEntAdminManager.strUnauthorizedDetail + "=Try to access pub record (Translation) in Add mode");
                        }
                        else
                        {
                            Response.Redirect("UnauthorizedAccess.aspx?" +
                                PubEntAdminManager.strUnauthorizedDetail + "=Try to access pub record (Translation) in Add mode", true);
                        }
                    }
                }
                else if ((string)Session[PubEntAdminManager.strPubGlobalMode] ==
                    PubEntAdminManager.strPubGlobalEMode)
                {
                    if (1 == 1) // temp auth fix
                    {
                        userControl =
                            (System.Web.UI.UserControl)this.LoadControl("TranslationTabEditInfo.ascx");
                        userControl.ID = "TranslationTabEditInfo1";
                        ((TranslationTabEditInfo)userControl).PubID = this.PubID;
                        this.plHldTranslationTabInfo.Controls.Clear();
                        this.plHldTranslationTabInfo.Controls.Add(userControl);

                        //this.RegisterMonitoredChanges(8);
                        //this.ByPassRegisterMonitoredChanges(8);

                        //this.trTranslation.Visible = true;
                    }
                    else
                    {
                        userControl =
                        (System.Web.UI.UserControl)this.LoadControl("TranslationTabReadInfo.ascx");
                        userControl.ID = "TranslationTabReadInfo";
                        ((TranslationTabReadInfo)userControl).PubID = this.PubID;
                        this.plHldTranslationTabInfo.Controls.Clear();
                        this.plHldTranslationTabInfo.Controls.Add(userControl);

                        //this.trTranslation.Visible = false;
                    }
                }
                else if ((string)Session[PubEntAdminManager.strPubGlobalMode] ==
                    PubEntAdminManager.strPubGlobalVMode)
                {
                    userControl =
                        (System.Web.UI.UserControl)this.LoadControl("TranslationTabReadInfo.ascx");
                    userControl.ID = "TranslationTabReadInfo";
                    ((TranslationTabReadInfo)userControl).PubID = this.PubID;
                    this.plHldTranslationTabInfo.Controls.Clear();
                    this.plHldTranslationTabInfo.Controls.Add(userControl);

                    //this.trTranslation.Visible = false;
                }
            }
            
            this.udpUpdate();
        }

        #endregion

        #region Tabs Content
        //protected void btnPubHistTabInfo_Click(object sender, EventArgs e)
        //{
        //    if (((Button)sender).Text.ToLower() == "edit")
        //    {
        //        this.lblPubHistTabLoadStatus.Text = "";
        //        //Load controls for editing

        //        //this.btnPubHistTabInfo.Visible = false;
        //        this.btnPubHistTabInfo.Text = "Cancel";
        //        Session["TabContentCtrlMode"] = "edit";
        //        this.udpUpdate();
        //    }
        //    else
        //    {
        //        System.Web.UI.UserControl userControl =
        //           (System.Web.UI.UserControl)this.LoadControl("PubHistTabReadInfo.ascx");
        //        userControl.ID = "PubHistTabReadInfo1";
        //        this.plHldPubHistTabInfo.Controls.Clear();
        //        this.plHldPubHistTabInfo.Controls.Add(userControl);

        //        this.btnPubHistTabInfo.Text = "Edit";
        //        Session["TabContentCtrlMode"] = "read";
        //        this.udpUpdate();
        //    }
        //}
        protected void btnNCIPLTabInfo_Click(object sender, EventArgs e)
        {
            if (((Button)sender).Text.ToLower() == "edit")
            {
                //this.lblNCIPLTabLoadStatus.Text = "";
                //Load controls for editing
                System.Web.UI.UserControl userControl =
                   (System.Web.UI.UserControl)this.LoadControl("NCIPLTabEditInfo.ascx");
                userControl.ID = "NCIPLTabEditInfo1";
                this.plHldNCIPLTabInfo.Controls.Clear();
                this.plHldNCIPLTabInfo.Controls.Add(userControl);

                //this.btnNCIPLTabInfo.Visible = false;
                this.btnNCIPLTabInfo.Text = "Cancel";
                Session["TabContentCtrlMode"] = "edit";
                this.udpUpdate();
            }
            else
            {
                System.Web.UI.UserControl userControl =
                   (System.Web.UI.UserControl)this.LoadControl("NCIPLTabReadInfo.ascx");
                userControl.ID = "NCIPLTabReadInfo1";
                this.plHldNCIPLTabInfo.Controls.Clear();
                this.plHldNCIPLTabInfo.Controls.Add(userControl);

                this.btnNCIPLTabInfo.Text = "Edit";
                Session["TabContentCtrlMode"] = "read";
                this.udpUpdate();
            }
        }
        protected void btnCatalogTabInfo_Click(object sender, EventArgs e)
        {
            if (((Button)sender).Text.ToLower() == "edit")
            {
                //this.lblCatalogTabLoadStatus.Text = "";
                //Load controls for editing

                //this.btnCatalogTabInfo.Visible = false;
                this.btnCatalogTabInfo.Text = "Cancel";
                Session["TabContentCtrlMode"] = "edit";
                this.udpUpdate();
            }
            else
            {
                this.btnCatalogTabInfo.Text = "Edit";
                Session["TabContentCtrlMode"] = "read";
                this.udpUpdate();
            }
        }
        protected void btnROOTabInfo_Click(object sender, EventArgs e)
        {
            if (((Button)sender).Text.ToLower() == "edit")
            {
                //this.lblROOTabLoadStatus.Text = "";
                //Load controls for editing
                System.Web.UI.UserControl userControl =
                   (System.Web.UI.UserControl)this.LoadControl("ROOTabEditInfo.ascx");

                userControl.ID = "ROOTabEditInfo1";

                this.plHldROOTabInfo.Controls.Clear();
                this.plHldROOTabInfo.Controls.Add(userControl);

                //this.btnNCIPLTabInfo.Visible = false;
                this.btnROOTabInfo.Text = "Cancel";
                Session["TabContentCtrlMode"] = "edit";

                this.udpUpdate();
            }
            else
            {
                //TabContainer container = (TabContainer)this.FindControl("tabContLiveInt");
                //TabPanel myActiveTabPanel = container.ActiveTab;
                //UpdatePanel a1 = (UpdatePanel)myActiveTabPanel.FindControl("updpnlROO");
                //PlaceHolder a2 = ((PlaceHolder)a1.ContentTemplateContainer.FindControl("plHldROOTabInfo"));
                //ROOTabEditInfo r = (ROOTabEditInfo)a2.FindControl("ROOTabEditInfo1");
                 
                //ROOTabEditInfo a =((ROOTabEditInfo)this.plHldROOTabInfo.FindControl("ROOTabEditInfo1"));
                //string fck = a.testtxtbox;

                System.Web.UI.UserControl userControl =
                   (System.Web.UI.UserControl)this.LoadControl("ROOTabReadInfo.ascx");
                userControl.ID = "ROOTabReadInfo1";
                this.plHldROOTabInfo.Controls.Clear();
                this.plHldROOTabInfo.Controls.Add(userControl);

                this.btnROOTabInfo.Text = "Edit";
                Session["TabContentCtrlMode"] = "read";
                this.udpUpdate();
            }
        }
        protected void btnExhTabInfo_Click(object sender, EventArgs e)
        {
            if (((Button)sender).Text.ToLower() == "edit")
            {
                //this.lblExhTabLoadStatus.Text = "";
                //Load controls for editing

                //this.btnExhTabInfo.Visible = false;
                this.btnExhTabInfo.Text = "Cancel";
                Session["TabContentCtrlMode"] = "edit";
                this.udpUpdate();
            }
            else
            {
                this.btnExhTabInfo.Text = "Edit";
                Session["TabContentCtrlMode"] = "read";
                this.udpUpdate();
            }
        }
        protected void btnCmtTabInfo_Click(object sender, EventArgs e)
        {
            //this.lblCmtTabLoadStatus.Text = "";
            //Save comment first

            //Always load the grid to show the comments
            //System.Web.UI.UserControl userControl =
            //   (System.Web.UI.UserControl)this.LoadControl("CmtTabInfo.ascx");
            //userControl.ID = "CmtTabInfo1";
            //this.plHldCmtTabInfo.Controls.Clear();
            //this.plHldCmtTabInfo.Controls.Add(userControl);

            this.udpUpdate();
        }
        protected void btnAttachTabInfo_Click(object sender, EventArgs e)
        {
            
        }
        protected void btnRelatedTabInfo_Click(object sender, EventArgs e)
        {
            
        }
        protected void btnTranslationTabInfo_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #endregion

        #region Methods

        protected void Recreation()
        {
            

            System.Web.UI.UserControl userControl = null;
            switch ((int)Session[PubEntAdminManager.strTabContentCurrActTabIndex])
            {
                case 0:
                    if (Session[PubEntAdminManager.strPubGlobalMode] != null)
                    {
                        if ((string)Session[PubEntAdminManager.strPubGlobalMode] ==
                            PubEntAdminManager.strPubGlobalAMode)
                        {
                            //userControl = (System.Web.UI.UserControl)this.LoadControl("PubHistTabEditInfo.ascx");
                            //userControl.ID = "PubHistTabEditInfo1";
                            Label l = new Label();
                            l.Text = "Add Publication History information after creating the publication.";
                            this.plHldPubHistTabAddInfo.Controls.Clear();
                            this.plHldPubHistTabAddInfo.Controls.Add(l);
                        }
                        else if ((string)Session[PubEntAdminManager.strPubGlobalMode] ==
                            PubEntAdminManager.strPubGlobalEMode)
                        {
                            if (1 == 1) // temp auth fix
                            // if (((CustomPrincipal)Context.User).IsInRole(PubEntAdminManager.AdminRole) ||
                            // ((CustomPrincipal)Context.User).IsInRole(PubEntAdminManager.DWHStaffRole))
                            {
                                userControl = (System.Web.UI.UserControl)this.LoadControl("PubHistTabEditInfo.ascx");
                                userControl.ID = "PubHistTabEditInfo1";
                                ((PubHistTabEditInfo)userControl).PubID = this.PubID;
                                this.plHldPubHistTabAddInfo.Controls.Clear();
                                this.plHldPubHistTabAddInfo.Controls.Add(userControl);
                            }
                            else
                            {
                                userControl = (System.Web.UI.UserControl)this.LoadControl("PubHistTabReadInfo.ascx");
                                userControl.ID = "PubHistTabReadInfo1";
                                ((PubHistTabReadInfo)userControl).PubID = this.PubID;
                                this.plHldPubHistTabViewInfo.Controls.Clear();
                                this.plHldPubHistTabViewInfo.Controls.Add(userControl);
                            }
                        }
                        else
                        {
                            userControl = (System.Web.UI.UserControl)this.LoadControl("PubHistTabReadInfo.ascx");
                            userControl.ID = "PubHistTabReadInfo1";
                            ((PubHistTabReadInfo)userControl).PubID = this.PubID;
                            this.plHldPubHistTabViewInfo.Controls.Clear();
                            this.plHldPubHistTabViewInfo.Controls.Add(userControl);
                        }
                    }
                   
                    break;
                case 1:
                    
                    if (((string)Session[PubEntAdminManager.strPubGlobalMode]).ToLower() ==
                        PubEntAdminManager.strPubGlobalAMode)
                    {
                        if (1 == 1) // temp auth fix
                        {
                            userControl = (System.Web.UI.UserControl)this.LoadControl("NCIPLTabEditInfo.ascx");
                            userControl.ID = "NCIPLTabEditInfo1";
                            this.plHldNCIPLTabInfo.Controls.Clear();
                            this.plHldNCIPLTabInfo.Controls.Add(userControl);
                            
                        }
                        else
                        {
                            if (PubEntAdminManager.TamperProof)
                            {
                                PubEntAdminManager.RedirectEncodedURLWithQS("UnauthorizedAccess.aspx",
                                    PubEntAdminManager.strUnauthorizedDetail + "=Try to access pub record (NCIPL) in Add mode");
                            }
                            else
                            {
                                Response.Redirect("UnauthorizedAccess.aspx?" +
                                    PubEntAdminManager.strUnauthorizedDetail + "=Try to access pub record (NCIPL) in Add mode", true);
                            }
                        }
                    }
                    else if (((string)Session[PubEntAdminManager.strPubGlobalMode]).ToLower() ==
                        PubEntAdminManager.strPubGlobalEMode)
                    {
                        if (1 == 1) // temp auth fix
                        {
                            userControl = (System.Web.UI.UserControl)this.LoadControl("NCIPLTabEditInfo.ascx");
                            userControl.ID = "NCIPLTabEditInfo1";
                            ((NCIPLTabEditInfo)userControl).PubID = this.PubID;
                            this.plHldNCIPLTabInfo.Controls.Clear();
                            this.plHldNCIPLTabInfo.Controls.Add(userControl);
                            
                        }
                        else
                        {
                            userControl = (System.Web.UI.UserControl)this.LoadControl("NCIPLTabReadInfo.ascx");
                            userControl.ID = "NCIPLTabReadInfo1";
                            ((NCIPLTabReadInfo)userControl).PubID = this.PubID;
                            this.plHldNCIPLTabInfo.Controls.Clear();
                            this.plHldNCIPLTabInfo.Controls.Add(userControl);
                        }
                    }
                    else
                    {
                        userControl = (System.Web.UI.UserControl)this.LoadControl("NCIPLTabReadInfo.ascx");
                        userControl.ID = "NCIPLTabReadInfo1";
                        ((NCIPLTabReadInfo)userControl).PubID = this.PubID;
                        this.plHldNCIPLTabInfo.Controls.Clear();
                        this.plHldNCIPLTabInfo.Controls.Add(userControl);
                    }
                    break;
                case 2:
                    if (((string)Session[PubEntAdminManager.strPubGlobalMode]).ToLower() ==
                        PubEntAdminManager.strPubGlobalAMode)
                    {
                        if (1 == 1) // temp auth fix
                        {
                            userControl = (System.Web.UI.UserControl)this.LoadControl("ROOTabEditInfo.ascx");
                            userControl.ID = "ROOTabEditInfo1";
                            this.plHldROOTabInfo.Controls.Clear();
                            this.plHldROOTabInfo.Controls.Add(userControl);
                        }
                        else
                        {
                            if (PubEntAdminManager.TamperProof)
                            {
                                PubEntAdminManager.RedirectEncodedURLWithQS("UnauthorizedAccess.aspx",
                                    PubEntAdminManager.strUnauthorizedDetail + "=Try to access pub record (ROO) in Add mode");
                            }
                            else
                            {
                                Response.Redirect("UnauthorizedAccess.aspx?" +
                                    PubEntAdminManager.strUnauthorizedDetail + "=Try to access pub record (ROO) in Add mode", true);
                            }
                        }
                    }
                    else if (((string)Session[PubEntAdminManager.strPubGlobalMode]).ToLower() ==
                        PubEntAdminManager.strPubGlobalEMode)
                    {
                        if (1 == 1) // temp auth fix
                        {
                            userControl = (System.Web.UI.UserControl)this.LoadControl("ROOTabEditInfo.ascx");
                            userControl.ID = "ROOTabEditInfo1";
                            ((ROOTabEditInfo)userControl).PubID = this.PubID;
                            this.plHldROOTabInfo.Controls.Clear();
                            this.plHldROOTabInfo.Controls.Add(userControl);

                        }
                        else
                        {
                            userControl = (System.Web.UI.UserControl)this.LoadControl("ROOTabReadInfo.ascx");
                            userControl.ID = "ROOTabReadInfo1";
                            ((ROOTabReadInfo)userControl).PubID = this.PubID;
                            this.plHldROOTabInfo.Controls.Clear();
                            this.plHldROOTabInfo.Controls.Add(userControl);
                        }
                    }
                    else
                    {
                        userControl = (System.Web.UI.UserControl)this.LoadControl("ROOTabReadInfo.ascx");
                        userControl.ID = "ROOTabReadInfo1";
                        ((ROOTabReadInfo)userControl).PubID = this.PubID;
                        this.plHldROOTabInfo.Controls.Clear();
                        this.plHldROOTabInfo.Controls.Add(userControl);
                    }
                    break;
                case 3:
                    if (((string)Session[PubEntAdminManager.strPubGlobalMode]).ToLower() ==
                        PubEntAdminManager.strPubGlobalAMode)
                    {
                        if (1 == 1) // temp auth fix
                        {
                            userControl = (System.Web.UI.UserControl)this.LoadControl("ExhTabEditInfo.ascx");
                            userControl.ID = "ExhTabEditInfo1";
                            this.plHldExhTabInfo.Controls.Clear();
                            this.plHldExhTabInfo.Controls.Add(userControl);
                        }
                        else
                        {
                            if (PubEntAdminManager.TamperProof)
                            {
                                PubEntAdminManager.RedirectEncodedURLWithQS("UnauthorizedAccess.aspx",
                                    PubEntAdminManager.strUnauthorizedDetail + "=Try to access pub record (EXH) in Add mode");
                            }
                            else
                            {
                                Response.Redirect("UnauthorizedAccess.aspx?" +
                                    PubEntAdminManager.strUnauthorizedDetail + "=Try to access pub record (EXH) in Add mode", true);
                            }
                        }
                    }
                    else if (((string)Session[PubEntAdminManager.strPubGlobalMode]).ToLower() ==
                        PubEntAdminManager.strPubGlobalEMode)
                    {
                        if (1 == 1) // temp auth fix
                        {
                            userControl = (System.Web.UI.UserControl)this.LoadControl("ExhTabEditInfo.ascx");
                            userControl.ID = "ExhTabEditInfo1";
                            ((ExhTabEditInfo)userControl).PubID = this.PubID;
                            this.plHldExhTabInfo.Controls.Clear();
                            this.plHldExhTabInfo.Controls.Add(userControl);
                        }
                        else
                        {
                            userControl = (System.Web.UI.UserControl)this.LoadControl("ExhTabReadInfo.ascx");
                            userControl.ID = "ExhTabReadInfo1";
                            ((ExhTabReadInfo)userControl).PubID = this.PubID;
                            this.plHldExhTabInfo.Controls.Clear();
                            this.plHldExhTabInfo.Controls.Add(userControl);
                        }
                    }
                    else
                    {
                        userControl = (System.Web.UI.UserControl)this.LoadControl("ExhTabReadInfo.ascx");
                        userControl.ID = "ExhTabReadInfo1";
                        ((ExhTabReadInfo)userControl).PubID = this.PubID;
                        this.plHldExhTabInfo.Controls.Clear();
                        this.plHldExhTabInfo.Controls.Add(userControl);
                    }
                    break;
                case 4:
                    if (((string)Session[PubEntAdminManager.strPubGlobalMode]).ToLower() ==
                        PubEntAdminManager.strPubGlobalAMode)
                    {
                        if (1 == 1) // temp auth fix
                        {
                            userControl = (System.Web.UI.UserControl)this.LoadControl("CatalogTabEditInfo.ascx");
                            userControl.ID = "CatalogTabEditInfo1";
                            this.plHldCatalogTabInfo.Controls.Clear();
                            this.plHldCatalogTabInfo.Controls.Add(userControl);
                        }
                        else
                        {
                            if (PubEntAdminManager.TamperProof)
                            {
                                PubEntAdminManager.RedirectEncodedURLWithQS("UnauthorizedAccess.aspx",
                                    PubEntAdminManager.strUnauthorizedDetail + "=Try to access pub record (Catalog) in Add mode");
                            }
                            else
                            {
                                Response.Redirect("UnauthorizedAccess.aspx?" +
                                    PubEntAdminManager.strUnauthorizedDetail + "=Try to access pub record (Catalog) in Add mode", true);
                            }
                        }
                    }
                    else if (((string)Session[PubEntAdminManager.strPubGlobalMode]).ToLower() ==
                        PubEntAdminManager.strPubGlobalEMode)
                    {
                        if (1 == 1) // temp auth fix
                        {
                            userControl = (System.Web.UI.UserControl)this.LoadControl("CatalogTabEditInfo.ascx");
                            userControl.ID = "CatalogTabEditInfo1";
                            ((CatalogTabEditInfo)userControl).PubID = this.PubID;
                            this.plHldCatalogTabInfo.Controls.Clear();
                            this.plHldCatalogTabInfo.Controls.Add(userControl);
                        }
                        else
                        {
                            userControl = (System.Web.UI.UserControl)this.LoadControl("CatalogTabReadInfo.ascx");
                            userControl.ID = "CatalogTabReadInfo1";
                            ((CatalogTabReadInfo)userControl).PubID = this.PubID;
                            this.plHldCatalogTabInfo.Controls.Clear();
                            this.plHldCatalogTabInfo.Controls.Add(userControl);
                        }
                    }
                    else
                    {
                        userControl = (System.Web.UI.UserControl)this.LoadControl("CatalogTabReadInfo.ascx");
                        userControl.ID = "CatalogTabReadInfo1";
                        ((CatalogTabReadInfo)userControl).PubID = this.PubID;
                        this.plHldCatalogTabInfo.Controls.Clear();
                        this.plHldCatalogTabInfo.Controls.Add(userControl);
                    }
                    break;
                
                case 5:
                    userControl = (System.Web.UI.UserControl)this.LoadControl("Comment.ascx");
                    userControl.ID = "Comment1";
                    ((Comment)userControl).PubID = this.PubID;

                    if ((string)Session[PubEntAdminManager.strPubGlobalMode] ==
                            PubEntAdminManager.strPubGlobalAMode)
                    {
                        ((Comment)userControl).Mode = PubEntAdminManager.strPubGlobalAMode;
                    }
                    else if ((string)Session[PubEntAdminManager.strPubGlobalMode] ==
                            PubEntAdminManager.strPubGlobalEMode)
                    {
                        ((Comment)userControl).Mode = PubEntAdminManager.strPubGlobalEMode;
                    }
                    else if ((string)Session[PubEntAdminManager.strPubGlobalMode] ==
                            PubEntAdminManager.strPubGlobalVMode)
                    {
                        ((Comment)userControl).Mode = PubEntAdminManager.strPubGlobalVMode;
                    }
                    this.plHldCmtTabInfo.Controls.Clear();
                    this.plHldCmtTabInfo.Controls.Add(userControl);
                    break;
                case 6:
                    userControl = (System.Web.UI.UserControl)this.LoadControl("Attachments.ascx");
                    userControl.ID = "Attachments1";
                    ((Attachments)userControl).PubID = this.PubID;
                    this.plHldAttachTabInfo.Controls.Clear();
                    this.plHldAttachTabInfo.Controls.Add(userControl);

                    //this.RegisterMonitoredChanges(6);
                    break;
                case 7:
                    if (Session[PubEntAdminManager.strPubGlobalMode] != null)
                    {
                        if ((string)Session[PubEntAdminManager.strPubGlobalMode] ==
                            PubEntAdminManager.strPubGlobalAMode)
                        {
                            if (1 == 1) // temp auth fix
                            {
                                userControl =
                               (System.Web.UI.UserControl)this.LoadControl("RelatedTabEditInfo.ascx");
                                userControl.ID = "RelatedTabEditInfo1";
                                this.plHldRelatedTabInfo.Controls.Clear();
                                this.plHldRelatedTabInfo.Controls.Add(userControl);

                            }
                            else
                            {
                                if (PubEntAdminManager.TamperProof)
                                {
                                    PubEntAdminManager.RedirectEncodedURLWithQS("UnauthorizedAccess.aspx",
                                        PubEntAdminManager.strUnauthorizedDetail + "=Try to access pub record (Related) in Add mode");
                                }
                                else
                                {
                                    Response.Redirect("UnauthorizedAccess.aspx?" +
                                        PubEntAdminManager.strUnauthorizedDetail + "=Try to access pub record (Related) in Add mode", true);
                                }
                            }
                        }
                        else if ((string)Session[PubEntAdminManager.strPubGlobalMode] ==
                            PubEntAdminManager.strPubGlobalEMode)
                        {
                            if (1 == 1) // temp auth fix
                            {
                                userControl =
                               (System.Web.UI.UserControl)this.LoadControl("RelatedTabEditInfo.ascx");
                                userControl.ID = "RelatedTabEditInfo1";
                                ((RelatedTabEditInfo)userControl).PubID = this.PubID;
                                this.plHldRelatedTabInfo.Controls.Clear();
                                this.plHldRelatedTabInfo.Controls.Add(userControl);

                            }
                            else
                            {
                                userControl =
                           (System.Web.UI.UserControl)this.LoadControl("RelatedTabReadInfo.ascx");
                                userControl.ID = "RelatedTabReadInfo1";
                                ((RelatedTabReadInfo)userControl).PubID = this.PubID;
                                this.plHldRelatedTabInfo.Controls.Clear();
                                this.plHldRelatedTabInfo.Controls.Add(userControl);
                            }
                        }
                        else if ((string)Session[PubEntAdminManager.strPubGlobalMode] ==
                            PubEntAdminManager.strPubGlobalVMode)
                        {
                            userControl =
                           (System.Web.UI.UserControl)this.LoadControl("RelatedTabReadInfo.ascx");
                            userControl.ID = "RelatedTabReadInfo1";
                            ((RelatedTabReadInfo)userControl).PubID = this.PubID;
                            this.plHldRelatedTabInfo.Controls.Clear();
                            this.plHldRelatedTabInfo.Controls.Add(userControl);
                        }
                    }
                    break;
                case 8:
                    if (Session[PubEntAdminManager.strPubGlobalMode] != null)
                    {
                        if ((string)Session[PubEntAdminManager.strPubGlobalMode] ==
                            PubEntAdminManager.strPubGlobalAMode)
                        {
                            if (1 == 1) // temp auth fix
                            {
                                userControl =
                               (System.Web.UI.UserControl)this.LoadControl("TranslationTabEditInfo.ascx");
                                userControl.ID = "TranslationTabEditInfo1";
                                this.plHldTranslationTabInfo.Controls.Clear();
                                this.plHldTranslationTabInfo.Controls.Add(userControl);

                            }
                            else
                            {
                                if (PubEntAdminManager.TamperProof)
                                {
                                    PubEntAdminManager.RedirectEncodedURLWithQS("UnauthorizedAccess.aspx",
                                        PubEntAdminManager.strUnauthorizedDetail + "=Try to access pub record (Translation) in Add mode");
                                }
                                else
                                {
                                    Response.Redirect("UnauthorizedAccess.aspx?" +
                                        PubEntAdminManager.strUnauthorizedDetail + "=Try to access pub record (Translation) in Add mode", true);
                                }
                            }
                        }
                        else if ((string)Session[PubEntAdminManager.strPubGlobalMode] ==
                            PubEntAdminManager.strPubGlobalEMode)
                        {
                            if (1 == 1) // temp auth fix
                            {
                                userControl =
                               (System.Web.UI.UserControl)this.LoadControl("TranslationTabEditInfo.ascx");
                                userControl.ID = "TranslationTabEditInfo1";
                                ((TranslationTabEditInfo)userControl).PubID = this.PubID;
                                this.plHldTranslationTabInfo.Controls.Clear();
                                this.plHldTranslationTabInfo.Controls.Add(userControl);

                            }
                            else
                            {
                                userControl =
                           (System.Web.UI.UserControl)this.LoadControl("TranslationTabReadInfo.ascx");
                                userControl.ID = "TranslationTabReadInfo1";
                                ((TranslationTabReadInfo)userControl).PubID = this.PubID;
                                this.plHldTranslationTabInfo.Controls.Clear();
                                this.plHldTranslationTabInfo.Controls.Add(userControl);
                            }
                        }
                        else if ((string)Session[PubEntAdminManager.strPubGlobalMode] ==
                            PubEntAdminManager.strPubGlobalVMode)
                        {
                            userControl =
                           (System.Web.UI.UserControl)this.LoadControl("TranslationTabReadInfo.ascx");
                            userControl.ID = "TranslationTabReadInfo1";
                            ((TranslationTabReadInfo)userControl).PubID = this.PubID;
                            this.plHldTranslationTabInfo.Controls.Clear();
                            this.plHldTranslationTabInfo.Controls.Add(userControl);
                        }
                    }
                    break;
            }

            //this.udpUpdate();
        }

        //protected void BindDefTabData()
        //{
        //    //this.lblNCIPLTabLoadStatus.Text = "";
        //    System.Web.UI.UserControl userControl =
        //           (System.Web.UI.UserControl)this.LoadControl("NCIPLTabReadInfo.ascx");
        //    this.plHldNCIPLTabInfo.Controls.Add(userControl);
        //}

        protected void udpUpdate()
        {
            this.updpnlPubHist.Update();
            this.updpnlNCIPL.Update();
            this.updpnlROO.Update();
            this.updpnlExh.Update();
            this.updpnlCatelog.Update();
            this.updpnlCmt.Update();
            this.updpnlAttach.Update();
            this.updpnlRelated.Update();
            this.updpnlTranslation.Update();
        }

        public bool Save()
        {
            bool ret = false;
            TabContainer container = (TabContainer)this.FindControl("tabContLiveInt");
            TabPanel myActiveTabPanel = container.ActiveTab;
            switch (myActiveTabPanel.ID)
            {
                case "tabpnlPubHist":
                    ret = this.PubHistSave();
                    this.hidCurrTabIndex.Value = "0";
                    break;
                case "tabpnlNCIPL":
                    ret = this.NCIPLSave();
                    this.hidCurrTabIndex.Value = "1";
                    break;
                case "tabpnlROO":
                    ret = this.ROOSave();
                    this.hidCurrTabIndex.Value = "2";
                    break;
                case "tabpnlExh":
                    ret = this.ExhSave();
                    this.hidCurrTabIndex.Value = "3";
                    break;
                case "tabpnlCatalog":
                    ret = this.CatalogSave();
                    this.hidCurrTabIndex.Value = "4";
                    break;
                case "tabpnlCmt":
                    ret = this.CommentSave();
                    this.hidCurrTabIndex.Value = "5";
                    break;
                case "tabpnlAttach":
                    ret = this.AttachSave();
                    this.hidCurrTabIndex.Value = "6";
                    break;
                case "tabpnlRelated":
                    ret = this.RelatedSave();
                    this.hidCurrTabIndex.Value = "7";
                    break;
                case "tabpnlTranslation":
                    ret = this.TranslateSave();
                    this.hidCurrTabIndex.Value = "8";
                    break;
            }

            return ret;
        }

        #region Actual Saving Methods
        protected bool PubHistSave()
        {
            if (1 == 1) // temp auth fix
            // if (((CustomPrincipal)Context.User).IsInRole(PubEntAdminManager.AdminRole) ||
            // ((CustomPrincipal)Context.User).IsInRole(PubEntAdminManager.DWHStaffRole))
            {
                if ((string)Session[PubEntAdminManager.strPubGlobalMode] !=
                            PubEntAdminManager.strPubGlobalAMode)
                {
                    PubHistTabEditInfo a = ((PubHistTabEditInfo)this.plHldPubHistTabAddInfo.FindControl("PubHistTabEditInfo1"));
                    return a.Save();
                }
                else
                    return true;
            }
            else
            {
                return true;
            }
        }
        protected bool NCIPLSave()
        {
            if (1 == 1) // temp auth fix
            {
                if ((string)Session[PubEntAdminManager.strPubGlobalMode] !=
                            PubEntAdminManager.strPubGlobalAMode)
                {
                    NCIPLTabEditInfo a = ((NCIPLTabEditInfo)this.plHldNCIPLTabInfo.FindControl("NCIPLTabEditInfo1"));
                    return a.Save();
                }
                else
                    return true;
                
            }
            else
            {
                return true;
            }
        }
        protected bool ROOSave()
        {
            //TabContainer container = (TabContainer)this.FindControl("tabContLiveInt");
            //TabPanel myActiveTabPanel = container.ActiveTab;
            //PlaceHolder a = ((PlaceHolder)myActiveTabPanel.FindControl("plHldROOTabInfo"));
            //ROOTabEditInfo r = (ROOTabEditInfo)a.FindControl("ROOTabEditInfo");
            if (1 == 1) // temp auth fix
            {
                if ((string)Session[PubEntAdminManager.strPubGlobalMode] !=
                            PubEntAdminManager.strPubGlobalAMode)
                {
                    ROOTabEditInfo a = ((ROOTabEditInfo)this.plHldROOTabInfo.FindControl("ROOTabEditInfo1"));
                    return a.Save();
                }
                else
                    return true;
                
            }
            else
            {
                return true;
            }
        }
        protected bool ExhSave()
        {
            if (1 == 1) // temp auth fix
            {
                if ((string)Session[PubEntAdminManager.strPubGlobalMode] !=
                            PubEntAdminManager.strPubGlobalAMode)
                {
                    ExhTabEditInfo a = ((ExhTabEditInfo)this.plHldExhTabInfo.FindControl("ExhTabEditInfo1"));
                    return a.Save();
                }
                else
                    return true;
                
            }
            else
            {
                return true;
            }
        }
        protected bool CatalogSave()
        {
            if (1 == 1) // temp auth fix
            {
                if ((string)Session[PubEntAdminManager.strPubGlobalMode] !=
                            PubEntAdminManager.strPubGlobalAMode)
                {
                    CatalogTabEditInfo a = ((CatalogTabEditInfo)this.plHldCatalogTabInfo.FindControl("CatalogTabEditInfo1"));
                    return a.Save();
                }
                else
                    return true;
                
            }
            else
            {
                return true;
            }
        }
        protected bool CommentSave()
        {
            if ((string)Session[PubEntAdminManager.strPubGlobalMode] !=
                            PubEntAdminManager.strPubGlobalAMode)
            {
                Comment a = ((Comment)this.plHldCmtTabInfo.FindControl("Comment1"));
                return a.Save();
            }
            else
                return true;
            
        }
        protected bool AttachSave()
        {
            if ((string)Session[PubEntAdminManager.strPubGlobalMode] !=
                            PubEntAdminManager.strPubGlobalAMode)
            {
                Attachments a = ((Attachments)this.plHldAttachTabInfo.FindControl("Attachments1"));
                return a.Save();
            }
            else
                return true;
            
        }
        protected bool RelatedSave()
        {
            if (1 == 1) // temp auth fix
            {
                if ((string)Session[PubEntAdminManager.strPubGlobalMode] !=
                            PubEntAdminManager.strPubGlobalAMode)
                {
                    RelatedTabEditInfo a = ((RelatedTabEditInfo)this.plHldRelatedTabInfo.FindControl("RelatedTabEditInfo1"));
                    return a.Save();
                }
                else
                    return true;
                
            }
            else
            {
                return true;
            }
        }
        protected bool TranslateSave()
        {
            if (1 == 1) // temp auth fix
            {
                if ((string)Session[PubEntAdminManager.strPubGlobalMode] !=
                            PubEntAdminManager.strPubGlobalAMode)
                {
                    TranslationTabEditInfo a = ((TranslationTabEditInfo)this.plHldTranslationTabInfo.FindControl("TranslationTabEditInfo1"));
                    return a.Save();
                }
                else
                    return true;
                
            }
            else
            {
                return true;
            }
        }
        #endregion

        protected void RecursiveAssignPubID()
        {
            System.Web.UI.UserControl userControl;
            switch ((int)Session[PubEntAdminManager.strTabContentCurrActTabIndex])
            {
                case 0:

                    //if ((string)Session[PubEntAdminManager.strPubGlobalMode] ==
                    //        PubEntAdminManager.strPubGlobalEMode)
                    //{
                    //    userControl = (System.Web.UI.UserControl)this.plHldPubHistTabAddInfo.
                    //    FindControl("PubHistTabEditInfo1");
                    //    if (userControl == null)
                    //    {
                    //        userControl = (System.Web.UI.UserControl)this.LoadControl("PubHistTabEditInfo.ascx");
                    //        userControl.ID = "PubHistTabEditInfo1";
                    //        ((PubHistTabEditInfo)userControl).PubID = this.PubID;
                    //        this.plHldPubHistTabAddInfo.Controls.Clear();
                    //        this.plHldPubHistTabAddInfo.Controls.Add(userControl);
                    //    }
                    //}

                    //userControl = (System.Web.UI.UserControl)this.plHldPubHistTabAddInfo.
                    //    FindControl("PubHistTabEditInfo1");
                    //((PubHistTabEditInfo)userControl).PubID = this.PubID;
                    //userControl = (System.Web.UI.UserControl)this.plHldPubHistTabViewInfo.
                    //    FindControl("PubHistTabReadInfo1");
                    //((PubHistTabReadInfo)userControl).PubID = this.PubID;
                            
                    break;
                case 1:

                    userControl = (System.Web.UI.UserControl)this.plHldNCIPLTabInfo.
                        FindControl("NCIPLTabEditInfo1");
                    ((NCIPLTabEditInfo)userControl).PubID = this.PubID;

                    break;
                case 2:

                    userControl = (System.Web.UI.UserControl)this.plHldROOTabInfo.
                        FindControl("ROOTabEditInfo1");
                    ((ROOTabEditInfo)userControl).PubID = this.PubID;

                    break;
                case 3:

                    userControl = (System.Web.UI.UserControl)this.plHldExhTabInfo.
                        FindControl("ExhTabEditInfo1");
                    ((ExhTabEditInfo)userControl).PubID = this.PubID;

                    break;
                case 4:

                    userControl = (System.Web.UI.UserControl)this.plHldCatalogTabInfo.
                        FindControl("CatalogTabEditInfo1");
                    ((CatalogTabEditInfo)userControl).PubID = this.PubID;

                    break;

                case 5:

                    userControl = (System.Web.UI.UserControl)this.plHldCmtTabInfo.
                        FindControl("Comment1");
                    ((Comment)userControl).PubID = this.PubID;

                    break;
                case 6:

                    userControl = (System.Web.UI.UserControl)this.plHldAttachTabInfo.
                        FindControl("Attachments1");
                    ((Attachments)userControl).PubID = this.PubID;

                    break;
                case 7:

                    userControl = (System.Web.UI.UserControl)this.plHldRelatedTabInfo.
                        FindControl("RelatedTabEditInfo1");
                    ((RelatedTabEditInfo)userControl).PubID = this.PubID;

                    break;
                case 8:

                    userControl = (System.Web.UI.UserControl)this.plHldTranslationTabInfo.
                        FindControl("TranslationTabEditInfo1");
                    ((TranslationTabEditInfo)userControl).PubID = this.PubID;

                    break;
            } 
        }

        #endregion

        #region Properties
        public int PubID
        {
            set
            {
                this.intPubID = value;
                if (Session[PubEntAdminManager.strPubGlobalMode].ToString() ==
                    PubEntAdminManager.strPubGlobalAMode && this.PubID > 0)
                {
                    this.RecursiveAssignPubID();
                }
            }
            get
            {
                return this.intPubID;
            }
        }
        #endregion

        
    }
}