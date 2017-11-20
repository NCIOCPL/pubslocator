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
    public partial class ROOTabEditInfo : System.Web.UI.UserControl
    {
        #region Fields
        private int intPubID;
        #endregion

        #region Events Handling

        protected void Page_Load(object sender, EventArgs e)
        {
            this.BindOption();
            this.BindValues();
            this.SecVal();
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
            
        }
        #endregion

        #region Methods
        protected void BindOption()
        {
            this.rdbtnListYesNoEveryOrder.DataBind();
            //NCIPLCC this.rdbtnListYesNoMostCommonList.DataBind();
            this.rdbtnListYesNoShowInSearchRes.DataBind();
            this.rdbtnListYesNoROOKit.DataBind(); 
            this.ckboxListDisplayStatusROO.DataBind();

            //NCIPLCC this.listMCLSubject.DataSource = PE_DAL.GetAllROOSubject(true);
            //NCIPLCC this.listMCLSubject.DataTextField = "name";
            //NCIPLCC this.listMCLSubject.DataValueField = "id";
            //NCIPLCC this.listMCLSubject.DataBind();

            this.listSubject.DataSource = PE_DAL.GetAllROOSubject(true);
            this.listSubject.DataTextField = "name";
            this.listSubject.DataValueField = "id";
            this.listSubject.DataBind();

            //NCIPL_CC - Part of changes to have collections on NCIPL tab and ROO tab
            this.listCollections.DataSource = PE_DAL.GetAllCollectionsByInterface("NCIPL_CC");
            this.listCollections.DataTextField = "name";
            this.listCollections.DataValueField = "id";
            this.listCollections.DataBind();
        }

        protected void BindValues()
        {
            if (Session[PubEntAdminManager.strPubGlobalMode] != null)
            {
                if (Session[PubEntAdminManager.strPubGlobalMode].ToString() == PubEntAdminManager.strPubGlobalAMode)//add
                {
                    this.rdbtnListYesNoEveryOrder.No = true;
                    this.rdbtnListYesNoShowInSearchRes.Yes = true;
                    this.rdbtnListYesNoROOKit.No = true;
                 }
                else
                {
                    MultiSelectListBoxItemCollection rcoll = PE_DAL.GetROODisplayStatusByPubID(this.PubID);
                    foreach (DisplayStatus p in rcoll)
                    {
                        ListItem matchItem = this.ckboxListDisplayStatusROO.Items.FindByValue(p.DisplayStatusID.ToString());
                        if (matchItem != null)
                        {
                            matchItem.Selected = true;
                        }
                    }

                    ROOCollection l = PE_DAL.GetROOInterface(this.PubID);
                    if (l.Count > 0)
                    {
                        ROO l_ROO = l[0];
                        //this.rdbtnListYesNoEveryOrder.Yes = l_ROO.EVERYORDER_ROO ? true : false;
                        //this.rdbtnListYesNoShowInSearchRes.Yes = l_ROO.ISSEARCHABLE_ROO ? true : false;
                        //this.rdbtnListYesNoMostCommonList.Yes = l_ROO.ROOCOMMONLIST ? true : false;

                        //this.txtMaxQtyROO.Text = l_ROO.MAXQTY_ROO.ToString();

                        if (l_ROO.EVERYORDER_ROO > 0)
                            this.rdbtnListYesNoEveryOrder.Yes = true;
                        else if (l_ROO.EVERYORDER_ROO == 0)
                            this.rdbtnListYesNoEveryOrder.No = true;

                        if (l_ROO.ISSEARCHABLE_ROO > 0)
                            this.rdbtnListYesNoShowInSearchRes.Yes = true;
                        else if (l_ROO.ISSEARCHABLE_ROO == 0)
                            this.rdbtnListYesNoShowInSearchRes.No = true;

                        if (l_ROO.ISROO_KIT > 0)
                            this.rdbtnListYesNoROOKit.Yes = true;
                        else if (l_ROO.ISROO_KIT == 0)
                            this.rdbtnListYesNoROOKit.No = true;
                        else
                            this.rdbtnListYesNoROOKit.No = true;

                        //NCIPLCC if (l_ROO.ROOCOMMONLIST > 0)
                        //NCIPLCC     this.rdbtnListYesNoMostCommonList.Yes = true;
                        //NCIPLCC else if (l_ROO.ROOCOMMONLIST == 0)
                        //NCIPLCC     this.rdbtnListYesNoMostCommonList.No = true;

                        if (l_ROO.MAXQTY_ROO != -1)
                            this.txtMaxQtyROO.Text = l_ROO.MAXQTY_ROO.ToString();

                        //NCIPLCC this.listMCLSubject.SelectedIndex =
                        //NCIPLCC     this.listMCLSubject.Items.IndexOf(this.listMCLSubject.Items.FindByValue(l_ROO.FK_COMMONLISTSUBJ.ToString()));
                    }

                    rcoll = PE_DAL.GetROOSubjectByPubID(this.PubID);
                    foreach (PubEntAdmin.BLL.Subject p in rcoll)
                    {
                        ListItem matchItem = this.listSubject.Items.FindByValue(p.SubjID.ToString());
                        if (matchItem != null)
                        {
                            matchItem.Selected = true;
                        }
                    }

                    //NCIPL_CC - Part of changes to have collections on NCIPL tab and ROO tab.
                    rcoll = PE_DAL.GetCollectionsByInterfaceByPubId("NCIPL_CC", this.PubID);
                    foreach (PubEntAdmin.BLL.Series p in rcoll)
                    {
                        ListItem matchItem = this.listCollections.Items.FindByValue(p.ID.ToString());
                        if (matchItem != null)
                        {
                            matchItem.Selected = true;
                        }
                    }

                }
            }
        }

        public bool Save()
        {
            //string strDisplayStatusSelection = "";
            //for (int i = 0; i < this.ckboxListDisplayStatusROO.Items.Count; i++)
            //{
            //    if (this.ckboxListDisplayStatusROO.Items[i].Selected)
            //    {
            //        if (strDisplayStatusSelection.Length > 0)
            //            strDisplayStatusSelection += "," + this.ckboxListDisplayStatusROO.Items[i].Value;
            //        else
            //            strDisplayStatusSelection += this.ckboxListDisplayStatusROO.Items[i].Value;
            //    }
            //}
            this.SecVal();
            bool blnDisplayStatusSave = PE_DAL.SetROODisplayStatusByPubID(this.PubID, this.ckboxListDisplayStatusROO.SelectedValueToString(), ',');

            bool blnROOInterfaceSave = PE_DAL.SetROOInterface(this.PubID,
                this.txtMaxQtyROO.Text.Trim().Length != 0 ? System.Convert.ToInt32(this.txtMaxQtyROO.Text.Trim()) : -1,
                System.Convert.ToInt32(this.rdbtnListYesNoEveryOrder.Yes),
                System.Convert.ToInt32(this.rdbtnListYesNoShowInSearchRes.Yes),
                System.Convert.ToInt32(this.rdbtnListYesNoROOKit.Yes),
                -1, //NCIPLCC System.Convert.ToInt32(this.rdbtnListYesNoMostCommonList.Yes),
                -1); //NCIPLCC this.listMCLSubject.SelectedValueToString().Length>0?System.Convert.ToInt32(this.listMCLSubject.SelectedValueToString()):-1);

            bool blnROOSujSave = PE_DAL.SetROOSubjectByPubID(this.PubID, this.listSubject.SelectedValueToString(), ',');

            //Begin NCIPL_CC - Part of changes to have collections on NCIPL tab and ROO tab
            string strSelectedValues = this.listCollections.SelectedValueToString();
            if (strSelectedValues == "0") //ALL - needs special treatment
            {
                strSelectedValues = "";
                foreach (ListItem li in this.listCollections.Items)
                {
                    if (strSelectedValues.Length > 0 && li.Value != "0")
                        strSelectedValues += "," + li.Value;
                    else if (li.Value != "0")
                        strSelectedValues = li.Value;
                }
            }
            bool blnROOSeriesSave = PE_DAL.SetSeries_ByInterfaceByPubId(this.PubID, strSelectedValues, ",", "NCIPL_CC");
            //End NCIPL_CC

            //NCIPL_CC if (blnDisplayStatusSave && blnROOInterfaceSave && blnROOSujSave)
            if (blnDisplayStatusSave && blnROOInterfaceSave && blnROOSujSave && blnROOSeriesSave) //NCIPL_CC
                return true;
            else
                return false;

            
            //return true;
        }

        #region MonitorChanges
        protected void RegisterMonitoredChanges()
        {
            PubEntAdminManager.MonitorChanges2(this.Page, this, this.ckboxListDisplayStatusROO);
            PubEntAdminManager.MonitorChanges2(this.Page, this, this.txtMaxQtyROO);
            PubEntAdminManager.MonitorChanges2(this.Page, this, this.rdbtnListYesNoEveryOrder);
            PubEntAdminManager.MonitorChanges2(this.Page, this, this.rdbtnListYesNoShowInSearchRes);
            PubEntAdminManager.MonitorChanges2(this.Page, this, this.rdbtnListYesNoROOKit);
            //NCIPLCC PubEntAdminManager.MonitorChanges2(this.Page, this, this.rdbtnListYesNoMostCommonList);
            //NCIPLCC PubEntAdminManager.MonitorChanges2(this.Page, this, this.listMCLSubject);
            PubEntAdminManager.MonitorChanges2(this.Page, this, this.listSubject);
            PubEntAdminManager.MonitorChanges2(this.Page, this, this.listCollections); //NCIPL_CC
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
            if ((!PubEntAdminManager.LenVal(this.txtMaxQtyROO.Text, 8)))
            {
                Response.Redirect("InvalidInput.aspx");
            }
        }

        private void TypeVal()
        {
            if (this.txtMaxQtyROO.Text.Trim().Length > 0)
            {
                if (!PubEntAdminManager.ContentNumVal(this.txtMaxQtyROO.Text.Trim()))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }
        }

        private void TagVal()
        {
            if ((PubEntAdminManager.OtherVal(this.txtMaxQtyROO.Text)))
            {
                Response.Redirect("InvalidInput.aspx");
            }

            foreach (ListItem li in this.ckboxListDisplayStatusROO.Items)
            {
                if (PubEntAdminManager.OtherVal(li.Text) || PubEntAdminManager.OtherVal(li.Value))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in rdbtnListYesNoEveryOrder.Items)
            {
                if (PubEntAdminManager.OtherVal(li.Text) || PubEntAdminManager.OtherVal(li.Value))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in rdbtnListYesNoShowInSearchRes.Items)
            {
                if (PubEntAdminManager.OtherVal(li.Text) || PubEntAdminManager.OtherVal(li.Value))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            //NCIPLCC foreach (ListItem li in rdbtnListYesNoMostCommonList.Items)
            //NCIPLCC {
            //NCIPLCC     if (PubEntAdminManager.OtherVal(li.Text) || PubEntAdminManager.OtherVal(li.Value))
            //NCIPLCC     {
            //NCIPLCC         Response.Redirect("InvalidInput.aspx");
            //NCIPLCC     }
            //NCIPLCC }

            //NCIPLCC foreach (ListItem li in listMCLSubject.Items)
            //NCIPLCC {
            //NCIPLCC     if (PubEntAdminManager.OtherVal(li.Text) || PubEntAdminManager.OtherVal(li.Value))
            //NCIPLCC     {
            //NCIPLCC         Response.Redirect("InvalidInput.aspx");
            //NCIPLCC     }
            //NCIPLCC }

            foreach (ListItem li in listSubject.Items)
            {
                if (PubEntAdminManager.OtherVal(li.Text) || PubEntAdminManager.OtherVal(li.Value))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            //NCIPL_CC
            foreach (ListItem li in listCollections.Items)
            {
                if (PubEntAdminManager.OtherVal(li.Text) || PubEntAdminManager.OtherVal(li.Value))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }
        }

        private void SpecialVal()
        {

            if ((PubEntAdminManager.SpecialVal2(this.txtMaxQtyROO.Text.Replace(" ", ""))))
            {
                Response.Redirect("InvalidInput.aspx");
            }

            foreach (ListItem li in ckboxListDisplayStatusROO.Items)
            {
                if (PubEntAdminManager.SpecialVal2(li.Text.Replace(" ", "")) ||
                    PubEntAdminManager.SpecialVal2(li.Value.Replace(" ", "")))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in rdbtnListYesNoEveryOrder.Items)
            {
                if (PubEntAdminManager.SpecialVal2(li.Text.Replace(" ", "")) ||
                    PubEntAdminManager.SpecialVal2(li.Value.Replace(" ", "")))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in rdbtnListYesNoShowInSearchRes.Items)
            {
                if (PubEntAdminManager.SpecialVal2(li.Text.Replace(" ", "")) ||
                    PubEntAdminManager.SpecialVal2(li.Value.Replace(" ", "")))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            //NCIPLCC foreach (ListItem li in rdbtnListYesNoMostCommonList.Items)
            //NCIPLCC {
            //NCIPLCC     if (PubEntAdminManager.SpecialVal2(li.Text.Replace(" ", "")) ||
            //NCIPLCC         PubEntAdminManager.SpecialVal2(li.Value.Replace(" ", "")))
            //NCIPLCC     {
            //NCIPLCC         Response.Redirect("InvalidInput.aspx");
            //NCIPLCC     }
            //NCIPLCC }

            //NCIPLCC foreach (ListItem li in listMCLSubject.Items)
            //NCIPLCC {
            //NCIPLCC     if (PubEntAdminManager.SpecialVal2(li.Text.Replace(" ", "")) ||
            //NCIPLCC         PubEntAdminManager.SpecialVal2(li.Value.Replace(" ", "")))
            //NCIPLCC     {
            //NCIPLCC         Response.Redirect("InvalidInput.aspx");
            //NCIPLCC     }
            //NCIPLCC }

            foreach (ListItem li in listSubject.Items)
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