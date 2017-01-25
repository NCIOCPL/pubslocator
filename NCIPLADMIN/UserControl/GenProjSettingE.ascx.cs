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
    public partial class GenProjSettingE : System.Web.UI.UserControl
    {
        #region Fields
        private int intPubID;
        private string strMode;
        #endregion

        #region Events Handling
        protected void Page_Load(object sender, EventArgs e)
        {

            #region ClientScript
            ScriptManager.RegisterStartupScript(this,
                typeof(GenProjSettingE), "GenProjSettingE_ClientScript",
            @"
                function GenProjSettingOnload()
                {
                    var expdtTxt = document.getElementById('" + this.txtExpDate.ClientID + @"');
                    if (expdtTxt.value.length==0)
                        expdtTxt.disabled=true;
                }

                function GenProjSettingENewUpdatedSetting()
                {
                    var e = window.event;
					var targ
					if (e.target) targ = e.target
					else if (e.srcElement) targ = e.srcElement

                    var elementRef = document.getElementById('" + this.CkboxListNewUpdated.ClientID + @"');
                    var checkBoxArray = elementRef.getElementsByTagName('input');
                    var expdtTxt = document.getElementById('" + this.txtExpDate.ClientID + @"');

                    if (checkBoxArray[0].checked && checkBoxArray[0] == targ)
                    {
                        checkBoxArray[1].checked = false;
                        expdtTxt.disabled=false;
                    }
                    else if (checkBoxArray[1].checked && checkBoxArray[1] == targ)
                    {
                        checkBoxArray[0].checked = false;
                        expdtTxt.disabled=false;
                    }
                    else if (!checkBoxArray[0].checked && !checkBoxArray[1].checked)
                    {
                        expdtTxt.value = '';
                    }
                }
                
                function MyOwnExpDateVal(source, clientside_arguments){        
                  
                    var expdtTxt = document.getElementById('" + this.txtExpDate.ClientID + @"');
                    var elementRef = document.getElementById('" + this.CkboxListNewUpdated.ClientID + @"');
                    var checkBoxArray = elementRef.getElementsByTagName('input'); 
                    var checkDate = new Date(expdtTxt.value);
                    var dPart = expdtTxt.value.split('/');
                    
                    //alert(dPart[0]+'--'+dPart[1]+'--'+dPart[2]);
                    if (dPart[0].length == 0 && dPart[1].length == 0 && dPart[2].length == 0)
                       clientside_arguments.IsValid=true; 
                    else if(dPart[0]!=checkDate.getMonth()+1 ||
                            dPart[1]!=checkDate.getDate() ||
                            dPart[2]!=checkDate.getFullYear()){
                           
                            alert('This date is invalid.');
                            expdtTxt.value ='';
                            expdtTxt.focus();
                            clientside_arguments.IsValid=false;
                            
                    }
                    else if ((dPart[0]==checkDate.getMonth()+1 &&
                             dPart[1]==checkDate.getDate() &&
                             dPart[2]==checkDate.getFullYear())){

                        
                        if (!checkBoxArray[0].checked && !checkBoxArray[1].checked) 
                        {     
                            alert('New or Updated List value is required.');
                            clientside_arguments.IsValid=false;
                        }
                    }
                    else
                        clientside_arguments.IsValid=true;
                    
                }

                function MyOwnNewUpdatedListVal(source, clientside_arguments){        
                  
                    var expdtTxt = document.getElementById('" + this.txtExpDate.ClientID + @"');
                    var elementRef = document.getElementById('" + this.CkboxListNewUpdated.ClientID + @"');
                    var checkBoxArray = elementRef.getElementsByTagName('input'); 

                    var e = window.event;
					var targ
					if (e.target) targ = e.target
					else if (e.srcElement) targ = e.srcElement
					if ((targ.id == '" + this.GetSaveBtn + @"') || 
                        (targ.id == '" + this.GetSaveBtn2 + @"'))
					{
                        if ((checkBoxArray[0].checked || checkBoxArray[1].checked) && 
                            (expdtTxt.value.length==0)) 
                        {
                            alert('The Expiration Date is required.');
                            clientside_arguments.IsValid=false;
                            needToConfirm = true;
                        }
                    }
                }
            ", true);
            #endregion

            this.BindOptions();
            this.BindValues();
            //this.imgUpload.Src = Server.MapPath("Image/Browse.gif");
            this.RegisterMonitoredChanges();
            this.TextCtrl_SpellCkDescription.Attributes["onkeypress"] += "return handleEnter(this, event);";
            this.TextCtrl_SpellCk_Summary.Attributes["onkeypress"] += "return handleEnter(this, event);";
            this.TextCtrl_SpellCk_POSInst.Attributes["onkeypress"] += "return handleEnter(this, event);";
            this.TextCtrl_SpellCk_Other.Attributes["onkeypress"] += "return handleEnter(this, event);";
            this.TextCtrl_SpellCk_Keyword.Attributes["onkeypress"] += "return handleEnter(this, event);";
            this.TextCtrl_SpellCk_Dimension.Attributes["onkeypress"] += "return handleEnter(this, event);";
            this.TextCtrl_SpellCk_Color.Attributes["onkeypress"] += "return handleEnter(this, event);";

            this.SecVal();
        }

        protected void btnUploaded_Click(object sender, System.EventArgs e)
        {

            //if (Session["UploadedDocumentsList"] == null)
            //    return;
            // display list of files uploaded in the listbox 
            //ArrayList filesArray = (ArrayList)Session["UploadedDocumentsList"];
            //lstFilesList.DataSource = filesArray;
            //lstFilesList.DataBind();
        }
        
        #endregion

        #region Methods

        public string GetSpellCheckParticipants()
        {
            return this.TextCtrl_SpellCkDescription.TextCtrl_SpellCheckClentID + "," +
                this.TextCtrl_SpellCk_Keyword.TextCtrl_SpellCheckClentID + "," +
                this.TextCtrl_SpellCk_Summary.TextCtrl_SpellCheckClentID + "," +
                //this.TextCtrl_SpellCk_Dimension.TextCtrl_SpellCheckClentID + "," +
                this.TextCtrl_SpellCk_Other.TextCtrl_SpellCheckClentID + "," +
                this.TextCtrl_SpellCk_Color.TextCtrl_SpellCheckClentID + "," +
                this.TextCtrl_SpellCk_POSInst.TextCtrl_SpellCheckClentID;
        }

        protected void BindOptions()
        {
            //if (this.Mode == PubEntAdminManager.strPubGlobalAMode)
            //{
                this.listLang.DataSource = PE_DAL.GetAllLang(true);
                this.listAudience.DataSource = PE_DAL.GetAllAudience(true);
                this.listCancerType.DataSource = PE_DAL.GetAllCancerType(true);
                this.listProdFormat.DataSource = PE_DAL.GetAllProdFormat(true);
                this.listReadingLevel.DataSource = PE_DAL.GetAllReadinglevel(true);
                //NCIPL_CC this.listSeries.DataSource = PE_DAL.GetAllSeries(true);
                this.listAward.DataSource = PE_DAL.GetAllAward(true);
                this.listRace.DataSource = PE_DAL.GetAllRace(true);
            //}
            //else
            //{
            //    this.listLang.DataSource = PE_DAL.GetAllLang(false);
            //    this.listAudience.DataSource = PE_DAL.GetAllAudience(false);
            //    this.listCancerType.DataSource = PE_DAL.GetAllCancerType(false);
            //    this.listProdFormat.DataSource = PE_DAL.GetAllProdFormat(false);
            //    this.listReadingLevel.DataSource = PE_DAL.GetAllReadinglevel(false);
            //    this.listSeries.DataSource = PE_DAL.GetAllSeries(false);
            //    this.listAward.DataSource = PE_DAL.GetAllAward(false);
            //    this.listRace.DataSource = PE_DAL.GetAllRace(false);
            //}

            this.listLang.DataTextField = "name";
            this.listLang.DataValueField = "id";
            this.listLang.DataBind();

            this.listAudience.DataTextField = "name";
            this.listAudience.DataValueField = "id";
            this.listAudience.DataBind();

            this.listCancerType.DataTextField = "name";
            this.listCancerType.DataValueField = "id";
            this.listCancerType.DataBind();

            this.listProdFormat.DataTextField = "name";
            this.listProdFormat.DataValueField = "id";
            this.listProdFormat.DataBind();

            this.listReadingLevel.DataTextField = "name";
            this.listReadingLevel.DataValueField = "id";
            this.listReadingLevel.DataBind();

            //NCIPL_CC this.listSeries.DataTextField = "name";
            //NCIPL_CC this.listSeries.DataValueField = "id";
            //NCIPL_CC this.listSeries.DataBind();

            this.listAward.DataTextField = "name";
            this.listAward.DataValueField = "id";
            this.listAward.DataBind();

            this.listRace.DataTextField = "name";
            this.listRace.DataValueField = "id";
            this.listRace.DataBind();
            
            this.rdbtnListCopyRightMaterial.DataBind();

            this.CkboxListNewUpdated.DataBind();

            this.CkboxListNewUpdated.Attributes.Add("onclick", "javascript:GenProjSettingENewUpdatedSetting();");
        }

        protected void BindValues()
        {
            Pub l_pub = PE_DAL.GetProdComData(this.PubID);
            this.TextCtrl_SpellCkDescription.Text = l_pub.Description;
            this.TextCtrl_SpellCk_Keyword.Text = l_pub.Keywords;
            this.TextCtrl_SpellCk_Summary.Text = l_pub.Summary;

            if (l_pub.IsCopyRight > 0)
                this.rdbtnListCopyRightMaterial.Yes = true;
            else if (l_pub.IsCopyRight == 0)
                this.rdbtnListCopyRightMaterial.No = true;

            //this.fileTestUpload..Text = l_pub.Thumbnail;
            if (l_pub.TotalNumPage != 0)
            {
                this.txtTotalPage.Text = l_pub.TotalNumPage.ToString();
            }
            //this.txtTotalPage.Text = l_pub.TotalNumPage.ToString();
            this.fakeinput.Value = l_pub.Thumbnail;
            this.fakeLginput.Value = l_pub.LargeImage;

            this.TextCtrl_SpellCk_Dimension.Text = l_pub.Dimension;
            this.TextCtrl_SpellCk_Color.Text = l_pub.Color;
            this.TextCtrl_SpellCk_Other.Text = l_pub.Other;

            this.TextCtrl_SpellCk_POSInst.Text = l_pub.POSInst;

            if (l_pub.NEW > 0)
                this.CkboxListNewUpdated.New = true;
            else if (l_pub.NEW == 0)
                this.CkboxListNewUpdated.New = false;

            if (l_pub.UPDATED > 0)
                this.CkboxListNewUpdated.Updated = true;
            else if (l_pub.UPDATED == 0)
                this.CkboxListNewUpdated.Updated = false;

            if (l_pub.EXPDATE.CompareTo(DateTime.MinValue) > 0)
            {
                this.txtExpDate.Text = l_pub.EXPDATE.ToShortDateString();
            }

            MultiSelectListBoxItemCollection rcoll = PE_DAL.GetLangByPubID(this.PubID);
            foreach (Lang p in rcoll)
            {
                ListItem matchItem = this.listLang.Items.FindByValue(p.LangID.ToString());
                if (matchItem != null)
                {
                    matchItem.Selected = true;
                }
            }
            

            rcoll = PE_DAL.GetCancerTypeByPubID(this.PubID);
            foreach (PubEntAdmin.BLL.CancerType p in rcoll)
            {
                ListItem matchItem = this.listCancerType.Items.FindByValue(p.CancerTypeID.ToString());
                if (matchItem != null)
                {
                    matchItem.Selected = true;
                }
            }

            rcoll = PE_DAL.GetAudienceByPubID(this.PubID);
            foreach (PubEntAdmin.BLL.Audience p in rcoll)
            {
                ListItem matchItem = this.listAudience.Items.FindByValue(p.AudID.ToString());
                if (matchItem != null)
                {
                    matchItem.Selected = true;
                }
            }

            rcoll = PE_DAL.GetProdFormatByPubID(this.PubID);
            foreach (ProdFormat p in rcoll)
            {
                ListItem matchItem = this.listProdFormat.Items.FindByValue(p.ProdFormatID.ToString());
                if (matchItem != null)
                {
                    matchItem.Selected = true;
                }
            }

            //Commented - NCIPL_CC (Moved Series to NCIPL and ROO tabs)
            //rcoll = PE_DAL.GetSeriesByPubID(this.PubID);
            //foreach (PubEntAdmin.BLL.Series p in rcoll)
            //{
            //    ListItem matchItem = this.listSeries.Items.FindByValue(p.SeriesID.ToString());
            //    if (matchItem != null)
            //    {
            //        matchItem.Selected = true;
            //    }
            //}

            rcoll = PE_DAL.GetRaceByPubID(this.PubID);
            foreach (PubEntAdmin.BLL.Race p in rcoll)
            {
                ListItem matchItem = this.listRace.Items.FindByValue(p.RaceID.ToString());
                if (matchItem != null)
                {
                    matchItem.Selected = true;
                }
            }

            rcoll = PE_DAL.GetReadlevelByPubID(this.PubID);
            foreach (Readlevel p in rcoll)
            {
                ListItem matchItem = this.listReadingLevel.Items.FindByValue(p.ReadlevelID.ToString());
                if (matchItem != null)
                {
                    matchItem.Selected = true;
                }
            }

            rcoll = PE_DAL.GetAwardByPubID(this.PubID);
            foreach (PubEntAdmin.BLL.Award p in rcoll)
            {
                ListItem matchItem = this.listAward.Items.FindByValue(p.AwardID.ToString());
                if (matchItem != null)
                {
                    matchItem.Selected = true;
                }
            }
        }

        protected void GenerateUploadForm()
        {
            // if querystring parameter "MODE" is set to "IFRAME" 
            //then just display file upload panel (pnlFileUpload) and hide everything else 
            //string mode = Request.QueryString["mode"];
            ////if (!(mode == "add"))
            //if ((mode != "add"))
            //    return;
            //pnlAjax.Visible = false;
            //pnlIFrame.Visible = false;
            //lblPageLoadTime.Visible = false;
            //pnlFileUpload.Visible = true;
            if (!IsPostBack)
                Session["UploadedDocumentsList"] = null;
        }

        public bool Save()
        {
            this.SecVal();
            bool blnGenProjSave = PE_DAL.SetProdComData(this.PubID,
                this.TextCtrl_SpellCkDescription.Text.Trim().Length > 0 ? this.TextCtrl_SpellCkDescription.Text.Trim() : null,
                this.TextCtrl_SpellCk_Keyword.Text.Trim().Length > 0 ? this.TextCtrl_SpellCk_Keyword.Text.Trim() : null,
                this.TextCtrl_SpellCk_Summary.Text.Trim().Length > 0 ? this.TextCtrl_SpellCk_Summary.Text.Trim() : null,
                System.Convert.ToInt32(this.rdbtnListCopyRightMaterial.Yes),
                this.fakeinput.Value.Trim().Length > 0 ? System.IO.Path.GetFileName(this.fakeinput.Value.Trim()) : null,
                this.fakeLginput.Value.Trim().Length > 0 ? System.IO.Path.GetFileName(this.fakeLginput.Value.Trim()) : null,
                this.txtTotalPage.Text.Trim().Length > 0 ? System.Convert.ToInt32(this.txtTotalPage.Text.Trim()) : -1,
                this.TextCtrl_SpellCk_Dimension.Text.Trim().Length > 0 ? this.TextCtrl_SpellCk_Dimension.Text.Trim() : null,
                this.TextCtrl_SpellCk_Color.Text.Trim().Length > 0 ? this.TextCtrl_SpellCk_Color.Text.Trim() : null,
                this.TextCtrl_SpellCk_Other.Text.Trim().Length > 0 ? this.TextCtrl_SpellCk_Other.Text.Trim() : null,
                this.TextCtrl_SpellCk_POSInst.Text.Trim().Length > 0 ? this.TextCtrl_SpellCk_POSInst.Text.Trim() : null,
                
                System.Convert.ToInt32(this.CkboxListNewUpdated.New),
                System.Convert.ToInt32(this.CkboxListNewUpdated.Updated),
                this.txtExpDate.Text.Trim().Length != 0 ? System.Convert.ToDateTime(this.txtExpDate.Text.Trim()) : DateTime.MinValue
                );

            bool blnUploadThumdnail = this.UploadThumdnail();
            bool blnUploadLargeImage = this.UploadLargeIamge();

            bool blnSetCancer = PubEntAdmin.BLL.CancerType.SetCancer(this.PubID, this.listCancerType.SelectedValueToString(), PubEntAdminManager.charDelim);
            bool blnSetAudience = PubEntAdmin.BLL.Audience.SetAudience(this.PubID, this.listAudience.SelectedValueToString(), PubEntAdminManager.charDelim);
            bool blnSetLang = Lang.SetLang(this.PubID, this.listLang.SelectedValueToString(), PubEntAdminManager.charDelim);
            bool blnSetProdFormat = ProdFormat.SetProdFormat(this.PubID, this.listProdFormat.SelectedValueToString(), PubEntAdminManager.charDelim);
            //NCIPL_CC bool blnSetSeries = PubEntAdmin.BLL.Series.SetSeries(this.PubID, this.listSeries.SelectedValueToString(), PubEntAdminManager.charDelim);
            bool blnSetRace = PubEntAdmin.BLL.Race.SetRace(this.PubID, this.listRace.SelectedValueToString(), PubEntAdminManager.charDelim);
            bool blnSetReadlevel = Readlevel.SetReadlevel(this.PubID, System.Convert.ToInt32(this.listReadingLevel.SelectedValueToString().Length>0?this.listReadingLevel.SelectedValue[0]:"0"));
            bool blnSetAward = PubEntAdmin.BLL.Award.SetAward(this.PubID, this.listAward.SelectedValueToString(), PubEntAdminManager.charDelim);

            if (blnGenProjSave && blnUploadThumdnail && blnUploadLargeImage &&
                blnSetCancer && blnSetAudience && blnSetLang &&
                //NCIPL_CC blnSetProdFormat && blnSetSeries && blnSetRace &&
                blnSetProdFormat && blnSetRace &&
                blnSetReadlevel && blnSetAward)
                return true;
            else 
                return false;
        }

        //protected bool UploadThumdnail()
        //{
        //    if (this.realinput.Value.Length == 0 && this.realLginput.Value.Length==0)
        //        return true;

        //    if (!Directory.Exists(PubEntAdminManager.UploadFileThumbnailPath))
        //    {
        //        Directory.CreateDirectory(PubEntAdminManager.UploadFileThumbnailPath);
        //    }


        //    if (!Directory.Exists(PubEntAdminManager.UploadFileLargeImagePath))
        //    {
        //        Directory.CreateDirectory(PubEntAdminManager.UploadFileLargeImagePath);
        //    }            


        //    string strFileName;
        //    string strLgFileName;

        //    strFileName = this.realinput.Value;
        //    strLgFileName = this.realLginput.Value;

        //    string c = System.IO.Path.GetFileName(strFileName);
        //    string lgc = System.IO.Path.GetFileName(strLgFileName);

        //    try
        //    {
        //        if (realinput.PostedFile != null)
        //        {
        //            realinput.PostedFile.SaveAs(PubEntAdminManager.UploadFileThumbnailPath + "\\" + c);
        //            this.fakeinput.Value = c;
        //            return true;
        //        }
        //        else if (realLginput.PostedFile != null)
        //        {
        //            realLginput.PostedFile.SaveAs(PubEntAdminManager.UploadFileLargeImagePath + "\\" + c);
        //            this.fakeLginput.Value = lgc;
        //            return true;
        //        }
        //        else
        //            return false;
        //    }
        //    catch (Exception Exp)
        //    {
        //        return false;
        //        // Handle Error 
        //    }
            
        //}


        protected bool UploadThumdnail()
        {
            if (this.realinput.Value.Length == 0)
                return true;

            if (!Directory.Exists(PubEntAdminManager.UploadFileThumbnailPath))
            {
                Directory.CreateDirectory(PubEntAdminManager.UploadFileThumbnailPath);
            }

            string strFileName;

            strFileName = this.realinput.Value;
            string c = System.IO.Path.GetFileName(strFileName);

            try
            {
                if (realinput.PostedFile != null)
                {
                    realinput.PostedFile.SaveAs(PubEntAdminManager.UploadFileThumbnailPath + "\\" + c);
                    this.fakeinput.Value = c;
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

        protected bool UploadLargeIamge()
        {
            if (this.realLginput.Value.Length == 0)
                return true;

            if (!Directory.Exists(PubEntAdminManager.UploadFileLargeImagePath))
            {
                Directory.CreateDirectory(PubEntAdminManager.UploadFileLargeImagePath);
            }

            string strFileName;

            strFileName = this.realLginput.Value;
            string c = System.IO.Path.GetFileName(strFileName);

            try
            {
                if (realLginput.PostedFile != null)
                {
                    realLginput.PostedFile.SaveAs(PubEntAdminManager.UploadFileLargeImagePath + "\\" + c);
                    this.fakeLginput.Value = c;
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

        #region MonitorChanges
        protected void RegisterMonitoredChanges()
        {
            PubEntAdminManager.MonitorChanges(this.Page, this.rdbtnListCopyRightMaterial);
            PubEntAdminManager.MonitorChanges(this.Page, this.CkboxListNewUpdated);
            PubEntAdminManager.MonitorChanges(this.Page, this.txtExpDate);
            PubEntAdminManager.MonitorChanges(this.Page, this.txtTotalPage);
            PubEntAdminManager.MonitorChanges(this.Page, this.listCancerType);
            PubEntAdminManager.MonitorChanges(this.Page, this.listAudience);
            PubEntAdminManager.MonitorChanges(this.Page, this.listLang);
            PubEntAdminManager.MonitorChanges(this.Page, this.listProdFormat);
            //NCIPL_CC PubEntAdminManager.MonitorChanges(this.Page, this.listSeries);
            PubEntAdminManager.MonitorChanges(this.Page, this.listRace);
            PubEntAdminManager.MonitorChanges(this.Page, this.listReadingLevel);
            PubEntAdminManager.MonitorChanges(this.Page, this.listAward);
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
            if ((!PubEntAdminManager.LenVal(this.TextCtrl_SpellCkDescription.Text, 2000)) ||
                (!PubEntAdminManager.LenVal(this.TextCtrl_SpellCk_Keyword.Text, 1000)) ||
                (!PubEntAdminManager.LenVal(this.TextCtrl_SpellCk_Summary.Text, 300)) ||
                (!PubEntAdminManager.LenVal(this.txtTotalPage.Text, 8)) ||
                (!PubEntAdminManager.LenVal(this.TextCtrl_SpellCk_Dimension.Text, 25)) ||
                (!PubEntAdminManager.LenVal(this.TextCtrl_SpellCk_Color.Text, 20)) ||
                (!PubEntAdminManager.LenVal(this.TextCtrl_SpellCk_Other.Text, 50)) ||
                (!PubEntAdminManager.LenVal(this.TextCtrl_SpellCk_POSInst.Text, 300)) ||
                (!PubEntAdminManager.LenVal(this.txtExpDate.Text, 10)) 
                )
            {
                Response.Redirect("InvalidInput.aspx");
            }

        }

        private void TypeVal()
        {
            if (this.txtTotalPage.Text.Trim().Length > 0 )
            {
                if (!PubEntAdminManager.ContentNumVal(this.txtTotalPage.Text.Trim()))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            
        }

        private void TagVal()
        {
            if ((PubEntAdminManager.OtherVal(this.TextCtrl_SpellCkDescription.Text)) ||
                (PubEntAdminManager.OtherVal(this.TextCtrl_SpellCk_Keyword.Text)) ||
                (PubEntAdminManager.OtherVal(this.TextCtrl_SpellCk_Summary.Text)) ||
                (PubEntAdminManager.OtherVal(this.txtTotalPage.Text)) ||
                (PubEntAdminManager.OtherVal(this.TextCtrl_SpellCk_Dimension.Text)) ||
                (PubEntAdminManager.OtherVal(this.TextCtrl_SpellCk_Color.Text)) ||
                (PubEntAdminManager.OtherVal(this.TextCtrl_SpellCk_Other.Text)) ||
                (PubEntAdminManager.OtherVal(this.TextCtrl_SpellCk_POSInst.Text)) ||
                (PubEntAdminManager.OtherVal(this.txtExpDate.Text))
                )
            {
                Response.Redirect("InvalidInput.aspx");
            }

            foreach (ListItem li in listCancerType.Items)
            {
                if (PubEntAdminManager.OtherVal(li.Text)||PubEntAdminManager.OtherVal(li.Value))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in listAudience.Items)
            {
                if (PubEntAdminManager.OtherVal(li.Text) || PubEntAdminManager.OtherVal(li.Value))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in listLang.Items)
            {
                if (PubEntAdminManager.OtherVal(li.Text) || PubEntAdminManager.OtherVal(li.Value))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in listProdFormat.Items)
            {
                if (PubEntAdminManager.OtherVal(li.Text) || PubEntAdminManager.OtherVal(li.Value))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            //Commented - NCIPL_CC
            //foreach (ListItem li in listSeries.Items)
            //{
            //    if (PubEntAdminManager.OtherVal(li.Text) || PubEntAdminManager.OtherVal(li.Value))
            //    {
            //        Response.Redirect("InvalidInput.aspx");
            //    }
            //}

            foreach (ListItem li in listRace.Items)
            {
                if (PubEntAdminManager.OtherVal(li.Text) || PubEntAdminManager.OtherVal(li.Value))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in listReadingLevel.Items)
            {
                if (PubEntAdminManager.OtherVal(li.Text) || PubEntAdminManager.OtherVal(li.Value))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in listAward.Items)
            {
                if (PubEntAdminManager.OtherVal(li.Text) || PubEntAdminManager.OtherVal(li.Value))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }
        }

        private void SpecialVal()
        {
            if ((PubEntAdminManager.SpecialVal2(this.TextCtrl_SpellCkDescription.Text.Replace(" ", ""))) ||
                (PubEntAdminManager.SpecialVal2(this.TextCtrl_SpellCk_Keyword.Text.Replace(" ", ""))) ||
                (PubEntAdminManager.SpecialVal2(this.TextCtrl_SpellCk_Summary.Text.Replace(" ", ""))) ||
                (PubEntAdminManager.SpecialVal2(this.txtTotalPage.Text.Replace(" ", ""))) ||
                (PubEntAdminManager.SpecialVal2(this.TextCtrl_SpellCk_Dimension.Text.Replace(" ", ""))) ||
                (PubEntAdminManager.SpecialVal2(this.TextCtrl_SpellCk_Color.Text.Replace(" ", ""))) ||
                (PubEntAdminManager.SpecialVal2(this.TextCtrl_SpellCk_Other.Text.Replace(" ", ""))) ||
                (PubEntAdminManager.SpecialVal2(this.TextCtrl_SpellCk_POSInst.Text.Replace(" ", "")))||
                (PubEntAdminManager.SpecialVal2(this.txtExpDate.Text.Replace(" ", ""))))
            {
                Response.Redirect("InvalidInput.aspx");
            }

            foreach (ListItem li in listCancerType.Items)
            {
                if (PubEntAdminManager.SpecialVal2(li.Text.Replace(" ", "")) ||
                    PubEntAdminManager.SpecialVal2(li.Value.Replace(" ", "")))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in listAudience.Items)
            {
                if (PubEntAdminManager.SpecialVal2(li.Text.Replace(" ", "")) ||
                    PubEntAdminManager.SpecialVal2(li.Value.Replace(" ", "")))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in listLang.Items)
            {
                if (PubEntAdminManager.SpecialVal2(li.Text.Replace(" ", "")) ||
                    PubEntAdminManager.SpecialVal2(li.Value.Replace(" ", "")))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in listProdFormat.Items)
            {
                if (PubEntAdminManager.SpecialVal2(li.Text.Replace(" ", "")) ||
                    PubEntAdminManager.SpecialVal2(li.Value.Replace(" ", "")))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            //Commented - NCIPL_CC
            //foreach (ListItem li in listSeries.Items)
            //{
            //    if (PubEntAdminManager.SpecialVal2(li.Text.Replace(" ", "")) ||
            //        PubEntAdminManager.SpecialVal2(li.Value.Replace(" ", "")))
            //    {
            //        Response.Redirect("InvalidInput.aspx");
            //    }
            //}

            foreach (ListItem li in listRace.Items)
            {
                if (PubEntAdminManager.SpecialVal2(li.Text.Replace(" ", "")) ||
                    PubEntAdminManager.SpecialVal2(li.Value.Replace(" ", "")))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in listReadingLevel.Items)
            {
                if (PubEntAdminManager.SpecialVal2(li.Text.Replace(" ", "")) ||
                    PubEntAdminManager.SpecialVal2(li.Value.Replace(" ", "")))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }

            foreach (ListItem li in listAward.Items)
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

        public string Mode
        {
            set { this.strMode = value; }
            get { return this.strMode; }
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