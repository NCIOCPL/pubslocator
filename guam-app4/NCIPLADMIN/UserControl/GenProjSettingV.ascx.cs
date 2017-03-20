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
    public partial class GenProjSettingV : System.Web.UI.UserControl
    {
        #region Fields
        private int intPubID;
        #endregion

        #region Events Handling
        protected void Page_Load(object sender, EventArgs e)
        {
            this.BindOptions();
            this.BindValues();
            
            this.listLang.InnerList.Attributes.Add("onclick", "disableListItems('"+this.listLang.InnerList.ClientID+"');");
            this.listCancerType.InnerList.Attributes.Add("onclick", "disableListItems('" + this.listCancerType.InnerList.ClientID + "');");
            this.listAudience.InnerList.Attributes.Add("onclick", "disableListItems('" + this.listAudience.InnerList.ClientID + "');");
            this.listProdFormat.InnerList.Attributes.Add("onclick", "disableListItems('" + this.listProdFormat.InnerList.ClientID + "');");
            //NCIPL_CC this.listSeries.InnerList.Attributes.Add("onclick", "disableListItems('" + this.listSeries.InnerList.ClientID + "');");
            this.listRace.InnerList.Attributes.Add("onclick", "disableListItems('" + this.listRace.InnerList.ClientID + "');");
            this.listReadingLevel.InnerList.Attributes.Add("onclick", "disableListItems('" + this.listReadingLevel.InnerList.ClientID + "');");
            this.listAward.InnerList.Attributes.Add("onclick", "disableListItems('" + this.listAward.InnerList.ClientID + "');");

            if (!Page.ClientScript.IsClientScriptBlockRegistered("GenProjSettingVClientScript"))
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "GenProjSettingVClientScript",
                    @"
                    function disableListItems(ListId)
                    {
                        if (ListId.indexOf('listLang',0)>=0)
                        {
                            arrSel = document.getElementById('"+this.hiddenLangSelected.ClientID+ @"').value.split(',');
                        }
                        else if (ListId.indexOf('listCancerType',0)>=0)
                        {
                            arrSel = document.getElementById('" + this.hiddenCancerSelected.ClientID + @"').value.split(',');
                        }
                        else if (ListId.indexOf('listAudience',0)>=0)
                        {
                            arrSel = document.getElementById('" + this.hiddenAudienceSelected.ClientID + @"').value.split(',');
                        }
                        else if (ListId.indexOf('listProdFormat',0)>=0)
                        {
                            arrSel = document.getElementById('" + this.hiddenProdFormatSelected.ClientID + @"').value.split(',');
                        }
                        /*NCIPL_CC
                        else if (ListId.indexOf('listSeries',0)>=0)
                        {
                            arrSel = document.getElementById('" + this.hiddenSeriesSelected.ClientID + @"').value.split(',');
                        }
                        */
                        else if (ListId.indexOf('listRace',0)>=0)
                        {
                            arrSel = document.getElementById('" + this.hiddenRaceSelected.ClientID + @"').value.split(',');
                        }
                        else if (ListId.indexOf('listReadingLevel',0)>=0)
                        {
                            arrSel = document.getElementById('" + this.hiddenReadLevelSelected.ClientID + @"').value.split(',');
                        }
                        else if (ListId.indexOf('listAward',0)>=0)
                        {
                            arrSel = document.getElementById('" + this.hiddenAwardSelected.ClientID + @"').value.split(',');
                        }

                        objCtrl = document.getElementById(ListId);

                        objCtrl.selectedIndex = -1;

                        for (i=0;i<objCtrl.length;i++)
                        {
                            for (j=0;j<arrSel.length;j++)
                            {
                                if (objCtrl.options[i].value == arrSel[j])
                                   objCtrl.options[i].selected = true; 
                            }
                        }
                        
                    }", true);
            }
        }
        #endregion

        #region Methods

        protected void BindOptions()
        {
            this.listLang.DataSource = PE_DAL.GetAllLang(true);
            this.listLang.DataTextField = "name";
            this.listLang.DataValueField = "id";
            this.listLang.DataBind();
            this.listLang.ReadOnly = true;

            this.listAudience.DataSource = PE_DAL.GetAllAudience(true);
            this.listAudience.DataTextField = "name";
            this.listAudience.DataValueField = "id";
            this.listAudience.DataBind();
            this.listAudience.ReadOnly = true;

            this.listCancerType.DataSource = PE_DAL.GetAllCancerType(true);
            this.listCancerType.DataTextField = "name";
            this.listCancerType.DataValueField = "id";
            this.listCancerType.DataBind();
            this.listCancerType.ReadOnly = true;

            this.listProdFormat.DataSource = PE_DAL.GetAllProdFormat(true);
            this.listProdFormat.DataTextField = "name";
            this.listProdFormat.DataValueField = "id";
            this.listProdFormat.DataBind();
            this.listProdFormat.ReadOnly = true;

            this.listReadingLevel.DataSource = PE_DAL.GetAllReadinglevel(true);
            this.listReadingLevel.DataTextField = "name";
            this.listReadingLevel.DataValueField = "id";
            this.listReadingLevel.DataBind();
            this.listReadingLevel.ReadOnly = true;

            //Commented - NCIPL_CC Moved Series to NCIPL and ROO tabs
            //this.listSeries.DataSource = PE_DAL.GetAllSeries(true);
            //this.listSeries.DataTextField = "name";
            //this.listSeries.DataValueField = "id";
            //this.listSeries.DataBind();
            //this.listSeries.ReadOnly = true;

            this.listAward.DataSource = PE_DAL.GetAllAward(true);
            this.listAward.DataTextField = "name";
            this.listAward.DataValueField = "id";
            this.listAward.DataBind();
            this.listAward.ReadOnly = true;

            this.listRace.DataSource = PE_DAL.GetAllRace(true);
            this.listRace.DataTextField = "name";
            this.listRace.DataValueField = "id";
            this.listRace.DataBind();
            this.listRace.ReadOnly = true;

            this.CkboxListNewUpdated.DataBind();

        }
       
        protected void BindValues()
        {
            Pub l_pub = PE_DAL.GetProdComData(this.PubID);
            this.TextCtrl_SpellCkDescription.Text = l_pub.Description;
            this.TextCtrl_SpellCk_Keyword.Text = l_pub.Keywords;
            this.TextCtrl_SpellCk_Summary.Text = l_pub.Summary;

            if (l_pub.IsCopyRight > 0)
                this.lblCopyRight.Text = "Yes";
            else if (l_pub.IsCopyRight == 0)
                this.lblCopyRight.Text = "No";
            //this.lblCopyRight.Text = l_pub.IsCopyRight ? "Yes" : "No";
            this.txtThumbnail.Text = l_pub.Thumbnail;
            this.txtLgImage.Text = l_pub.LargeImage;
            this.txtTotalPage.Text = l_pub.TotalNumPage.ToString();

            this.txtThumbnail.Attributes.CssStyle.Add("color", "#808080");
            this.txtLgImage.Attributes.CssStyle.Add("color", "#808080");

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
                this.txtExpDate.Attributes.CssStyle.Add("color", "#808080");
            }

            this.ClearHidSelection();

            MultiSelectListBoxItemCollection rcoll = PE_DAL.GetLangByPubID(this.PubID);
            foreach (Lang p in rcoll)
            {
                ListItem matchItem = this.listLang.Items.FindByValue(p.LangID.ToString());
                if (matchItem != null)
                {
                    matchItem.Selected = true;
                    if (this.hiddenLangSelected.Value.Length != 0)
                        this.hiddenLangSelected.Value += ",";
                    this.hiddenLangSelected.Value += matchItem.Value;
                }
            }

            rcoll = PE_DAL.GetCancerTypeByPubID(this.PubID);
            foreach (PubEntAdmin.BLL.CancerType p in rcoll)
            {
                ListItem matchItem = this.listCancerType.Items.FindByValue(p.CancerTypeID.ToString());
                if (matchItem != null)
                {
                    matchItem.Selected = true;
                    if (this.hiddenCancerSelected.Value.Length != 0)
                        this.hiddenCancerSelected.Value += ",";
                    this.hiddenCancerSelected.Value += matchItem.Value;
                }
            }

            rcoll = PE_DAL.GetAudienceByPubID(this.PubID);
            foreach (PubEntAdmin.BLL.Audience p in rcoll)
            {
                ListItem matchItem = this.listAudience.Items.FindByValue(p.AudID.ToString());
                if (matchItem != null)
                {
                    matchItem.Selected = true;
                    if (this.hiddenAudienceSelected.Value.Length != 0)
                        this.hiddenAudienceSelected.Value += ",";
                    this.hiddenAudienceSelected.Value += matchItem.Value;
                }
            }

            rcoll = PE_DAL.GetProdFormatByPubID(this.PubID);
            foreach (ProdFormat p in rcoll)
            {
                ListItem matchItem = this.listProdFormat.Items.FindByValue(p.ProdFormatID.ToString());
                if (matchItem != null)
                {
                    matchItem.Selected = true;
                    if (this.hiddenProdFormatSelected.Value.Length != 0)
                        this.hiddenProdFormatSelected.Value += ",";
                    this.hiddenProdFormatSelected.Value += matchItem.Value;
                }
            }

            //NCIPL_CC rcoll = PE_DAL.GetSeriesByPubID(this.PubID);
            //foreach (PubEntAdmin.BLL.Series p in rcoll)
            //{
            //    ListItem matchItem = this.listSeries.Items.FindByValue(p.SeriesID.ToString());
            //    if (matchItem != null)
            //    {
            //        matchItem.Selected = true;
            //        if (this.hiddenSeriesSelected.Value.Length != 0)
            //            this.hiddenSeriesSelected.Value += ",";
            //        this.hiddenSeriesSelected.Value += matchItem.Value;
            //    }
            //}

            rcoll = PE_DAL.GetRaceByPubID(this.PubID);
            foreach (PubEntAdmin.BLL.Race p in rcoll)
            {
                ListItem matchItem = this.listRace.Items.FindByValue(p.RaceID.ToString());
                if (matchItem != null)
                {
                    matchItem.Selected = true;
                    if (this.hiddenRaceSelected.Value.Length != 0)
                        this.hiddenRaceSelected.Value += ",";
                    this.hiddenRaceSelected.Value += matchItem.Value;
                }
            }
            

            rcoll = PE_DAL.GetReadlevelByPubID(this.PubID);
            foreach (Readlevel p in rcoll)
            {
                ListItem matchItem = this.listReadingLevel.Items.FindByValue(p.ReadlevelID.ToString());
                if (matchItem != null)
                {
                    matchItem.Selected = true;
                    if (this.hiddenReadLevelSelected.Value.Length != 0)
                        this.hiddenReadLevelSelected.Value += ",";
                    this.hiddenReadLevelSelected.Value += matchItem.Value;
                }
            }

            rcoll = PE_DAL.GetAwardByPubID(this.PubID);
            foreach (PubEntAdmin.BLL.Award p in rcoll)
            {
                ListItem matchItem = this.listAward.Items.FindByValue(p.AwardID.ToString());
                if (matchItem != null)
                {
                    matchItem.Selected = true;
                    if (this.hiddenAwardSelected.Value.Length != 0)
                        this.hiddenAwardSelected.Value += ",";
                    this.hiddenAwardSelected.Value += matchItem.Value;
                }
            }

        }

        private void ClearHidSelection()
        {
            this.hiddenLangSelected.Value = String.Empty;
            this.hiddenCancerSelected.Value = String.Empty;
            this.hiddenAudienceSelected.Value = String.Empty;
            this.hiddenProdFormatSelected.Value = String.Empty;
            this.hiddenSeriesSelected.Value = String.Empty;
            this.hiddenRaceSelected.Value = String.Empty;
            this.hiddenReadLevelSelected.Value = String.Empty;
            this.hiddenAwardSelected.Value = String.Empty;
        }
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