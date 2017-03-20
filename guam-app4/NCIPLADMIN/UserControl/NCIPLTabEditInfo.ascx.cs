using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.IO;
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
    public partial class NCIPLTabEditInfo : System.Web.UI.UserControl
    {
        #region Fields
        private int intPubID;
        #endregion

        #region Events Handling
        protected void Page_Load(object sender, EventArgs e)
        {

            #region ClientScript
            ScriptManager.RegisterStartupScript(this,
                typeof(NCIPLTabEditInfo), "NCIPLTabEditInfo_ClientScript",
            @"
                function NCIPLDisplayStatusVal(oSrc, args)
                {
                    var e = window.event;
					var targ
					if (e.target) targ = e.target
					else if (e.srcElement) targ = e.srcElement
					if ((targ.id == '" + this.GetSaveBtn + @"') || 
                        (targ.id == '" + this.GetSaveBtn2 + @"'))
					{
                        var elementRef = document.getElementById('" + this.ckboxListDisplayStatusNCIPL.ClientID + @"');
                        var PURL = document.getElementById('" + this.GetPURL + @"');
                        var PPDFURL = document.getElementById('" + this.GetPDFURL + @"');
                        var checkBoxArray = elementRef.getElementsByTagName('input');
                        if ((checkBoxArray[0].checked && PURL.value.length == 0) && (checkBoxArray[0].checked && PPDFURL.value.length== 0))
                        {
                            args.IsValid = false;
                            alert('NCIPL ONLINE Display Status is selected. Please provide Publication URL.');
                            needToConfirm = true;
                        }
                        else
                        {
                            args.IsValid = true;
                        }
                    }
                    else
                    {
                        args.IsValid = true;
                    }
                }

                function NCIPLDisplayStatusVal2(elementId)
                {
                    
                    var elementRef = elementId;
                    var PURL = document.getElementById('" + this.GetPURL + @"');
                    //alert(PURL.value);
                    var PPDFURL = document.getElementById('" + this.GetPDFURL + @"');
                    var checkBoxArray = elementRef.getElementsByTagName('input');
                    //alert(checkBoxArray.length );
                    if ((checkBoxArray[0].checked && PURL.value.length == 0) && (checkBoxArray[0].checked && PPDFURL.value.length== 0))
                    {
                        alert('NCIPL ONLINE Display Status is selected. Please provide Publication URL.');
                        checkBoxArray[0].checked = false;
                    }
                }  

                    function copyfeaturedimage() {
                        var fakeinput = document.getElementById('" + this.fakeFeaturedinput.ClientID + @"');
                        var realinput = document.getElementById('" + this.realFeaturedinput.ClientID + @"');                      
                       
                    fakeinput.value=realinput.value; 
  
                    }          
                
            ", true);
            #endregion

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
            this.rdbtnListYesNoShowInSearchRes.DataBind();
            this.rdbtnListYesNoFeatured.DataBind();
            this.ckboxListDisplayStatusNCIPL.DataBind();
            

            //this.CkboxListNewUpdated.DataBinder(this.PubID);

            this.listSubject.DataSource = PE_DAL.GetAllNCIPLSubject(true);
            this.listSubject.DataTextField = "name";
            this.listSubject.DataValueField = "id";
            this.listSubject.DataBind();

            //Begin CR-11-001-36 Featured Stacks
            this.listStacks.DataSource = PE_DAL.GetAllNCIPLStacks();
            this.listStacks.DataTextField = "name";
            this.listStacks.DataValueField = "id";
            this.listStacks.DataBind();
            //End Cr-36

            //NCIPL_CC - Part of changes to have collections on NCIPL tab and ROO tab
            this.listCollections.DataSource = PE_DAL.GetAllCollectionsByInterface("NCIPL");
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
                }
                else
                {

                    MultiSelectListBoxItemCollection rcoll = PE_DAL.GetNCIPLDisplayStatusByPubID(this.PubID);
                    foreach (DisplayStatus p in rcoll)
                    {
                        ListItem matchItem = this.ckboxListDisplayStatusNCIPL.Items.FindByValue(p.DisplayStatusID.ToString());
                        if (matchItem != null)
                        {
                            matchItem.Selected = true;
                        }
                    }

                    NCIPLCollection l = PE_DAL.GetNCIPLInterface(this.PubID);
                    if (l.Count > 0)
                    {
                        NCIPL l_NCIPL = l[0];
                        //this.rdbtnListYesNoEveryOrder.Yes = l_NCIPL.EVERYORDER_NCIPL ? true : false;
                        //this.rdbtnListYesNoShowInSearchRes.Yes = l_NCIPL.ISSEARCHABLE_NCIPL ? true : false;
                        //this.rdbtnListYesNoFeatured.Yes = l_NCIPL.NCIPLFeatured ? true : false;

                        //this.txtMaxQtyNCIPL.Text = l_NCIPL.MAXQTY_NCIPL.ToString();
                        //this.txtMaxQtyIntl.Text = l_NCIPL.MAXINTL_NCIPL.ToString();

                        if (l_NCIPL.EVERYORDER_NCIPL > 0)
                            this.rdbtnListYesNoEveryOrder.Yes = true;
                        else if (l_NCIPL.EVERYORDER_NCIPL == 0)
                            this.rdbtnListYesNoEveryOrder.No = true;

                        if (l_NCIPL.ISSEARCHABLE_NCIPL > 0)
                            this.rdbtnListYesNoShowInSearchRes.Yes = true;
                        else if (l_NCIPL.ISSEARCHABLE_NCIPL == 0)
                            this.rdbtnListYesNoShowInSearchRes.No = true;

                        //if (l_NCIPL.NEW > 0)
                        //    this.CkboxListNewUpdated.New = true;
                        //else if (l_NCIPL.NEW == 0)
                        //    this.CkboxListNewUpdated.New = false;

                        //if (l_NCIPL.UPDATED > 0)
                        //    this.CkboxListNewUpdated.Updated = true;
                        //else if (l_NCIPL.NEW == 0)
                        //    this.CkboxListNewUpdated.Updated = false;

                        //if (l_NCIPL.EXPDATE.CompareTo(DateTime.MinValue) > 0)
                        //{
                        //    this.txtExpDate.Text = l_NCIPL.EXPDATE.ToShortDateString();
                        //}

                        if (l_NCIPL.NCIPLFeatured > 0)
                            this.rdbtnListYesNoFeatured.Yes = true;
                        else if (l_NCIPL.NCIPLFeatured == 0)
                            this.rdbtnListYesNoFeatured.No = true;

                        if (l_NCIPL.MAXQTY_NCIPL != -1)
                            this.txtMaxQtyNCIPL.Text = l_NCIPL.MAXQTY_NCIPL.ToString();

                        if (l_NCIPL.MAXINTL_NCIPL != -1)
                            this.txtMaxQtyIntl.Text = l_NCIPL.MAXINTL_NCIPL.ToString();

                        if (l_NCIPL.Rank_NCIPL != -1)
                            this.txtRank.Text = l_NCIPL.Rank_NCIPL.ToString();

                        this.fakeFeaturedinput.Value = l_NCIPL.NCIPLFeaturedImage.ToString();

                    }

                    rcoll = PE_DAL.GetNCIPLSubjectByPubID(this.PubID);
                    foreach (PubEntAdmin.BLL.Subject p in rcoll)
                    {
                        ListItem matchItem = this.listSubject.Items.FindByValue(p.SubjID.ToString());
                        if (matchItem != null)
                        {
                            matchItem.Selected = true;
                        }
                    }

                    //Begin CR-36
                    rcoll = PE_DAL.GetNCIPLStacksByPubId(this.PubID);
                    foreach (MultiSelectListBoxItem Item in rcoll)
                    {
                        ListItem matchItem = this.listStacks.Items.FindByValue(Item.ID.ToString());
                        if (matchItem != null)
                        {
                            matchItem.Selected = true;
                        }
                    }
                    //End CR-36

                    //NCIPL_CC - Part of changes to have collections on NCIPL tab and ROO tab.
                    rcoll = PE_DAL.GetCollectionsByInterfaceByPubId("NCIPL", this.PubID);
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
            string strDisplayStatusSelection = "";
            for (int i = 0; i < this.ckboxListDisplayStatusNCIPL.Items.Count; i++)
            {
                if (this.ckboxListDisplayStatusNCIPL.Items[i].Selected)
                {
                    if (strDisplayStatusSelection.Length > 0)
                        strDisplayStatusSelection += "," + this.ckboxListDisplayStatusNCIPL.Items[i].Value;
                    else
                        strDisplayStatusSelection += this.ckboxListDisplayStatusNCIPL.Items[i].Value;
                }
            }

            if (strDisplayStatusSelection.Contains('1') && this.txtRank.Text.Trim() == "" || strDisplayStatusSelection.Contains('2') && this.txtRank.Text.Trim() == "")
            {
                this.lblErRank.Text = "This is a required field.";
                return false;
            }

            else
            {
                this.SecVal();

                //Begin CR-36 - Validations
                if (rdbtnListYesNoFeatured.Yes && System.IO.Path.GetFileName(this.fakeFeaturedinput.Value.Trim()).Length == 0)
                {
                    lblErrStack.Text = "An image must be uploaded to the \"NCIPL Featured Image\" field in order to save this record.";
                    return false;
                }

                if (rdbtnListYesNoFeatured.Yes
                    && System.IO.Path.GetFileName(this.fakeFeaturedinput.Value.Trim()).Length > 0
                    && this.listStacks.SelectedItems.Count == 0)
                {   
                    lblErrStack.Text = "Image Stack field must have a title selected in order to save this record.";
                    return false;
                }
                //End CR-36

                bool blnDisplayStatusSave = PE_DAL.SetNCIPLDisplayStatusByPubID(this.PubID, this.ckboxListDisplayStatusNCIPL.SelectedValueToString(), ',');

                bool blnNCIPLInterfaceSave = PE_DAL.SetNCIPLInterface(this.PubID,
                    this.txtMaxQtyNCIPL.Text.Trim().Length != 0 ? System.Convert.ToInt32(this.txtMaxQtyNCIPL.Text.Trim()) : -1,
                    this.txtMaxQtyIntl.Text.Trim().Length != 0 ? System.Convert.ToInt32(this.txtMaxQtyIntl.Text.Trim()) : -1,
                    System.Convert.ToInt32(this.rdbtnListYesNoEveryOrder.Yes),
                    System.Convert.ToInt32(this.rdbtnListYesNoShowInSearchRes.Yes),
                    //System.Convert.ToInt32(this.CkboxListNewUpdated.New),
                    //System.Convert.ToInt32(this.CkboxListNewUpdated.Updated),
                    //this.txtExpDate.Text.Trim().Length != 0 ? System.Convert.ToDateTime(this.txtExpDate.Text.Trim()) : DateTime.MinValue,
                    System.Convert.ToInt32(this.rdbtnListYesNoFeatured.Yes),
                    this.fakeFeaturedinput.Value.Trim().Length > 0 ? System.IO.Path.GetFileName(this.fakeFeaturedinput.Value.Trim()) : null
                    );

                bool blnUploadFeauturedIamge = this.UploadFeaturedIamge();

                //Begin Insert for CR-36
                #region Featured_Stacks
                string strSelectedValues = this.listStacks.SelectedValueToString();
                if (strSelectedValues == "0") //ALL - needs special treatment
                {
                    strSelectedValues = "";
                    foreach (ListItem li in this.listStacks.Items)
                    {

                        if (strSelectedValues.Length > 0 && li.Value != "0")
                            strSelectedValues += "," + li.Value;
                        else if (li.Value != "0")
                            strSelectedValues = li.Value;
                    }
                }

                string errorMsg = "";
                bool blnNCIPLStacksSave = PE_DAL.SetNCIPLStacksByPubId(this.PubID, strSelectedValues, ",", out errorMsg);
                if (blnNCIPLStacksSave == false)
                {
                    lblErrStack.Text = errorMsg;
                    return false;
                }
                //End Insert
                //Save change to StackPub History table
                if (rdbtnListYesNoFeatured.Yes && System.IO.Path.GetFileName(this.fakeFeaturedinput.Value.Trim()).Length > 0)
                    PE_DAL.SetNCIPLStackPubHistoryByPubId(this.PubID, strSelectedValues, 1);
                else
                    PE_DAL.SetNCIPLStackPubHistoryByPubId(this.PubID, strSelectedValues, 0);
                #endregion
                //End CR-36

                bool blnNCIPLSujSave = PE_DAL.SetNCIPLSubjectByPubID(this.PubID, this.listSubject.SelectedValueToString(), ',');
                //Begin NCIPL_CC - Part of changes to have collections on NCIPL tab and ROO tab
                strSelectedValues = this.listCollections.SelectedValueToString();
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
                bool blnNCIPLSeriesSave = PE_DAL.SetSeries_ByInterfaceByPubId(this.PubID, strSelectedValues, ",", "NCIPL");
                //End NCIPL_CC
                
                if (this.txtRank.Text.Trim() != "")
                {
                    bool blnNCIPLRankSave = PE_DAL.SetNCIPLRankByPubID(this.PubID, this.txtRank.Text, 1);

                    //NCIPL_CC if (blnDisplayStatusSave && blnNCIPLInterfaceSave && blnNCIPLSujSave && blnNCIPLRankSave && blnUploadFeauturedIamge)
                    if (blnDisplayStatusSave && blnNCIPLInterfaceSave && blnNCIPLSujSave && blnNCIPLRankSave && blnUploadFeauturedIamge && blnNCIPLSeriesSave) //NCIPL_CC
                        return true;
                    else
                        return false;

                }
                else
                {

                    //NCIPL_CC if (blnDisplayStatusSave && blnNCIPLInterfaceSave && blnNCIPLSujSave && blnUploadFeauturedIamge)
                    if (blnDisplayStatusSave && blnNCIPLInterfaceSave && blnNCIPLSujSave && blnUploadFeauturedIamge && blnNCIPLSeriesSave) //NCIPL_CC
                        return true;
                    else
                        return false;
                }
            }
        }

        protected bool UploadFeaturedIamge()
        {
            if (this.realFeaturedinput.Value.Length == 0)
                return true;

            if (!Directory.Exists(PubEntAdminManager.UploadFileFeaturedIamgePath))
            {
                Directory.CreateDirectory(PubEntAdminManager.UploadFileFeaturedIamgePath);
            }

            string strFileName;

            strFileName = this.realFeaturedinput.Value;
            string c = System.IO.Path.GetFileName(strFileName);

            try
            {
                if (realFeaturedinput.PostedFile != null)
                {
                    realFeaturedinput.PostedFile.SaveAs(PubEntAdminManager.UploadFileFeaturedIamgePath + "\\" + c);
                    this.fakeFeaturedinput.Value = c;
                    return true;
                }
                else
                    return false;
            }
            catch (Exception Exp)
            {
                return false;
                // Handle Error 
            }

        }

//        protected void Page_PreRender(object sender, System.EventArgs e)
//        {
//            #region ClientScript
//             ScriptManager.RegisterStartupScript(this,
//                typeof(NCIPLTabEditInfo), "NCIPLTabEditInfo_ClientScript",
//            @"
//
//                    function copyfeaturedimage() {
//                        document.getElementById('<%=this.fakeFeaturedinput.ClientID %>').value =
//                         document.getElementById('<%=this.realFeaturedinput.ClientID %>').value;
//                  }
//                
//            ", true);
//            #endregion

//        }

        #region MonitorChanges
        protected void RegisterMonitoredChanges()
        {
            PubEntAdminManager.MonitorChanges2(this.Page, this, this.ckboxListDisplayStatusNCIPL);
            PubEntAdminManager.MonitorChanges2(this.Page, this, this.txtMaxQtyNCIPL);
            PubEntAdminManager.MonitorChanges2(this.Page, this, this.txtMaxQtyIntl);
            PubEntAdminManager.MonitorChanges2(this.Page, this, this.rdbtnListYesNoEveryOrder);
            PubEntAdminManager.MonitorChanges2(this.Page, this, this.rdbtnListYesNoShowInSearchRes);
            PubEntAdminManager.MonitorChanges2(this.Page, this, this.rdbtnListYesNoFeatured);
            PubEntAdminManager.MonitorChanges2(this.Page, this, this.listSubject);
            PubEntAdminManager.MonitorChanges2(this.Page, this, this.listStacks); //CR-36
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
            if ((!PubEntAdminManager.LenVal(this.txtMaxQtyNCIPL.Text, 8)) ||
                (!PubEntAdminManager.LenVal(this.txtMaxQtyIntl.Text, 8)))
            {
                Response.Redirect("InvalidInput.aspx");
            }
        }

        private void TypeVal()
        {
            if (this.txtMaxQtyNCIPL.Text.Trim().Length > 0)
            {
                if (!PubEntAdminManager.ContentNumVal(this.txtMaxQtyNCIPL.Text.Trim()))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            if (this.txtMaxQtyIntl.Text.Trim().Length > 0)
            {
                if (!PubEntAdminManager.ContentNumVal(this.txtMaxQtyIntl.Text.Trim()))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

        }

        private void TagVal()
        {
            if ((PubEntAdminManager.OtherVal(this.txtMaxQtyNCIPL.Text)) ||
                (PubEntAdminManager.OtherVal(this.txtMaxQtyIntl.Text)) || 
                (PubEntAdminManager.OtherVal(this.txtRank.Text)))
            {
                Response.Redirect("InvalidInput.aspx");
            }          

            foreach (ListItem li in this.ckboxListDisplayStatusNCIPL.Items)
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

            foreach (ListItem li in rdbtnListYesNoFeatured.Items)
            {
                if (PubEntAdminManager.OtherVal(li.Text) || PubEntAdminManager.OtherVal(li.Value))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in listSubject.Items)
            {
                if (PubEntAdminManager.OtherVal(li.Text) || PubEntAdminManager.OtherVal(li.Value))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            //Begin CR-36
            foreach (ListItem li in listStacks.Items)
            {
                if (PubEntAdminManager.OtherVal(li.Text) || PubEntAdminManager.OtherVal(li.Value))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }
            //End CR-36

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

            if ((PubEntAdminManager.SpecialVal2(this.txtMaxQtyNCIPL.Text.Replace(" ", ""))) ||
                (PubEntAdminManager.SpecialVal2(this.txtMaxQtyIntl.Text.Replace(" ", ""))) )
            {
                Response.Redirect("InvalidInput.aspx");
            }

            foreach (ListItem li in ckboxListDisplayStatusNCIPL.Items)
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

            foreach (ListItem li in rdbtnListYesNoFeatured.Items)
            {
                if (PubEntAdminManager.SpecialVal2(li.Text.Replace(" ", "")) ||
                    PubEntAdminManager.SpecialVal2(li.Value.Replace(" ", "")))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in rdbtnListYesNoFeatured.Items)
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

        protected string GetPURL
        {
            get { return ((PubRecord)this.Page).GetPURL; }
        }

        protected string GetPDFURL
        {
            get { return ((PubRecord)this.Page).GetPPDFURL; }
        }

        protected string GetSaveBtn
        {
            get { return ((PubRecord)this.Page).GetSaveBtn1; }
        }

        protected string GetSaveBtn2
        {
            get { return ((PubRecord)this.Page).GetSaveBtn2; }
        }

        
        #endregion
    }
}