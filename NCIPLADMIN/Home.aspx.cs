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

using PubEntAdmin.UserControl;
using PubEntAdmin.BLL;
using GlobalUtils;

namespace PubEntAdmin
{
    public partial class Home : System.Web.UI.Page
    {
        #region Variables
        protected string strAction;
        #endregion

        #region Controls
        private PubEntAdmin.BLL.Search mySearch;
        protected UrlBuilder myUrlBuilder = new UrlBuilder(HttpContext.Current.Request.Url.AbsoluteUri, new Base64Encoder());
        #endregion

        #region Events Handling
        protected void Page_Load(object sender, EventArgs e)
        {

            this.PageTitle = "Search";
            if (!Session.IsNewSession)
            {
                this.Title = "Admin Tool Home";
                this.PageTitle = "Search";

                if (!Page.IsPostBack)
                {
                    Session.Remove(PubEntAdminManager.strTabContentCurrActTabIndex);
                    this.AssignValues();
                }
            }
            else
                Session.Clear();
            
           
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (this.Action == PubEntAdminManager.strSearchRefine)
            {
                this.mySearch = ((PubEntAdmin.BLL.Search)Session[PubEntAdminManager.strSearchCriteria]);
                this.BindValues();
            }
        }
        #endregion

        #region Properties

        public string Action
        {
            get { return this.strAction; }
        }

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

        public bool NCIPL
        {
            get
            {
                return ((LiveIntSel)this.Master.FindControl("middleContent").FindControl("Search1").
                    FindControl("LiveIntSelSearch")).InNCIPL;
            }
        }

        public bool ROO
        {
            get
            {
                return ((LiveIntSel)this.Master.FindControl("middleContent").FindControl("Search1").
                    FindControl("LiveIntSelSearch")).InROO;
            }
        }

        public bool Exh
        {
            get
            {
                return ((LiveIntSel)this.Master.FindControl("middleContent").FindControl("Search1").
                    FindControl("LiveIntSelSearch")).InExh;
            }
        }

        public bool Catalog
        {
            get
            {
                return ((LiveIntSel)this.Master.FindControl("middleContent").FindControl("Search1").
                    FindControl("LiveIntSelSearch")).InCatalog;
            }
        }

        public string Keyword
        {
            get
            {
                return ((TextBox)this.Master.FindControl("middleContent").FindControl("Search1").
                    FindControl("txtKeyword")).Text;
            }
        }

        public string NIHNum1
        {
            get
            {
                return ((TextBox)this.Master.FindControl("middleContent").FindControl("Search1").
                    FindControl("txtNIHNum1")).Text;
            }
        }

        public string NIHNum2
        {
            get
            {
                return ((TextBox)this.Master.FindControl("middleContent").FindControl("Search1").
                    FindControl("txtNIHNum2")).Text;
            }
        }

        public string CreatedFrom
        {
            get
            {
                return ((TextBox)this.Master.FindControl("middleContent").FindControl("Search1").
                    FindControl("txtSrRecordStartDate")).Text;
            }
        }


        public string CreatedTo
        {
            get
            {
                return ((TextBox)this.Master.FindControl("middleContent").FindControl("Search1").
                    FindControl("txtEnRecordEndDate")).Text;
            }
        }

        public bool Newpub
        {
            get
            {
                return ((LiveNewUpStatus)this.Master.FindControl("middleContent").FindControl("Search1").
                 FindControl("NewUpStatus")).InNew;
            }
        }

        public bool Updatedpub
        {
            get
            {
                return ((LiveNewUpStatus)this.Master.FindControl("middleContent").FindControl("Search1").
                 FindControl("NewUpStatus")).InUpdated;
            }
        }

        public string SelectedCancerType
        {
            get
            {
                return ((ListMultiSelect)this.Master.FindControl("middleContent").
                    FindControl("Search1").FindControl("listCancerType")).SelectedValueToString();
            }
        }

        public string SelectedCancerTypeItem
        {
            get
            {
                return ((ListMultiSelect)this.Master.FindControl("middleContent").
                    FindControl("Search1").FindControl("listCancerType")).SelectedTextToString();
            }
        }

        public string SelectedSubject
        {
            get
            {
                return ((ListMultiSelect)this.Master.FindControl("middleContent").
                    FindControl("Search1").FindControl("listSubject")).SelectedValueToString();
            }
        }

        public string SelectedSubjectItem
        {
            get
            {
                return ((ListMultiSelect)this.Master.FindControl("middleContent").
                    FindControl("Search1").FindControl("listSubject")).SelectedTextToString();
            }
        }

        public string SelectedAudience
        {
            get
            {
                return ((ListMultiSelect)this.Master.FindControl("middleContent").
                    FindControl("Search1").FindControl("listAudience")).SelectedValueToString();
            }
        }

        public string SelectedAudienceItem
        {
            get
            {
                return ((ListMultiSelect)this.Master.FindControl("middleContent").
                    FindControl("Search1").FindControl("listAudience")).SelectedTextToString();
            }
        }

        public string SelectedLang
        {
            get
            {
                return ((ListMultiSelect)this.Master.FindControl("middleContent").
                    FindControl("Search1").FindControl("listLang")).SelectedValueToString();
            }
        }

        public string SelectedLangItem
        {
            get
            {
                return ((ListMultiSelect)this.Master.FindControl("middleContent").
                    FindControl("Search1").FindControl("listLang")).SelectedTextToString();
            }
        }

        public string SelectedProdFormat
        {
            get
            {
                return ((ListMultiSelect)this.Master.FindControl("middleContent").
                    FindControl("Search1").FindControl("listProdFormat")).SelectedValueToString();
            }
        }

        public string SelectedProdFormatItem
        {
            get
            {
                return ((ListMultiSelect)this.Master.FindControl("middleContent").
                    FindControl("Search1").FindControl("listProdFormat")).SelectedTextToString();
            }
        }

        public string SelectedSeries
        {
            get
            {
                return ((ListMultiSelect)this.Master.FindControl("middleContent").
                    FindControl("Search1").FindControl("listSeries")).SelectedValueToString();
            }
        }

        public string SelectedSeriesItem
        {
            get
            {
                return ((ListMultiSelect)this.Master.FindControl("middleContent").
                    FindControl("Search1").FindControl("listSeries")).SelectedTextToString();
            }
        }

        public string SelectedRace
        {
            get
            {
                return ((ListMultiSelect)this.Master.FindControl("middleContent").
                    FindControl("Search1").FindControl("listRace")).SelectedValueToString();
            }
        }

        public string SelectedRaceItem
        {
            get
            {
                return ((ListMultiSelect)this.Master.FindControl("middleContent").
                    FindControl("Search1").FindControl("listRace")).SelectedTextToString();
            }
        }

        public string SelectedBookStatus
        {
            get
            {
                return ((ListMultiSelect)this.Master.FindControl("middleContent").
                    FindControl("Search1").FindControl("listBookStatus")).SelectedValueToString();
            }
        }

        public string SelectedBookStatusItem
        {
            get
            {
                return ((ListMultiSelect)this.Master.FindControl("middleContent").
                    FindControl("Search1").FindControl("listBookStatus")).SelectedTextToString();
            }
        }

        public string SelectedReadingLevel
        {
            get
            {
                return ((ListMultiSelect)this.Master.FindControl("middleContent").
                    FindControl("Search1").FindControl("listReadingLevel")).SelectedValueToString();
            }
        }

        public string SelectedReadingLevelItem
        {
            get
            {
                return ((ListMultiSelect)this.Master.FindControl("middleContent").
                    FindControl("Search1").FindControl("listReadingLevel")).SelectedTextToString();
            }
        }

        public bool ROOMostCom
        {
            get
            {
                return ((CheckBox)this.Master.FindControl("middleContent").
                    FindControl("Search1").FindControl("chboxMostCommonList")).Checked;
            }
        }

        public string SelectedROOSubject
        {
            get
            {
                return ((ListMultiSelect)this.Master.FindControl("middleContent").
                    FindControl("Search1").FindControl("listROOSubject")).SelectedValueToString();
            }
        }

        public string SelectedROOSubjectItem
        {
            get
            {
                return ((ListMultiSelect)this.Master.FindControl("middleContent").
                    FindControl("Search1").FindControl("listROOSubject")).SelectedTextToString();
            }
        }

        public string SelectedAward
        {
            get
            {
                return ((ListMultiSelect)this.Master.FindControl("middleContent").
                    FindControl("Search1").FindControl("listAward")).SelectedValueToString();
            }
        }

        public string SelectedAwardItem
        {
            get
            {
                return ((ListMultiSelect)this.Master.FindControl("middleContent").
                    FindControl("Search1").FindControl("listAward")).SelectedTextToString();
            }
        }

        public string SelectedOwner
        {
            get
            {
                return ((ListMultiSelect)this.Master.FindControl("middleContent").
                    FindControl("Search1").FindControl("listOwner")).SelectedValueToString();
            }
        }

        public string SelectedOwnerItem
        {
            get
            {
                return ((ListMultiSelect)this.Master.FindControl("middleContent").
                    FindControl("Search1").FindControl("listOwner")).SelectedTextToString();
            }
        }

        public string SelectedSponsor
        {
            get
            {
                return ((ListMultiSelect)this.Master.FindControl("middleContent").
                    FindControl("Search1").FindControl("listSponsor")).SelectedValueToString();
            }
        }

        public string SelectedSponsorItem
        {
            get
            {
                return ((ListMultiSelect)this.Master.FindControl("middleContent").
                    FindControl("Search1").FindControl("listSponsor")).SelectedTextToString();
            }
        }

        #endregion

        #region Methods
        protected void AssignValues()
        {
            if (PubEntAdminManager.TamperProof)
            {
                this.strAction = myUrlBuilder.QueryString.ContainsKey(PubEntAdminManager.strSearchAction) ?
                    this.myUrlBuilder.QueryString[PubEntAdminManager.strSearchAction] : PubEntAdminManager.strSearchNew;
            }
            else
            {
                this.strAction = Request.QueryString[PubEntAdminManager.strSearchAction] != null ?
                    Request.QueryString[PubEntAdminManager.strSearchAction].ToString() : PubEntAdminManager.strSearchNew;
            }
        }

        protected void BindValues()
        {
            ((LiveIntSel)this.Master.FindControl("middleContent").FindControl("Search1").
                FindControl("LiveIntSelSearch")).InNCIPL = this.mySearch.NCIPL;
            ((LiveIntSel)this.Master.FindControl("middleContent").FindControl("Search1").
                FindControl("LiveIntSelSearch")).InROO = this.mySearch.ROO;
            ((LiveIntSel)this.Master.FindControl("middleContent").FindControl("Search1").
                FindControl("LiveIntSelSearch")).InExh = this.mySearch.EXH;
            ((LiveIntSel)this.Master.FindControl("middleContent").FindControl("Search1").
                FindControl("LiveIntSelSearch")).InCatalog = this.mySearch.CATALOG;

            ((TextBox)this.Master.FindControl("middleContent").FindControl("Search1").
                    FindControl("txtKeyword")).Text = this.mySearch.Key;

            ((LiveNewUpStatus)this.Master.FindControl("middleContent").FindControl("Search1").
                    FindControl("NewUpStatus")).InNew = this.mySearch.ISNEW;

            ((LiveNewUpStatus)this.Master.FindControl("middleContent").FindControl("Search1").
                    FindControl("NewUpStatus")).InUpdated = this.mySearch.ISUPDATED;

            ((TextBox)this.Master.FindControl("middleContent").FindControl("Search1").
                    FindControl("txtNIHNum1")).Text = this.mySearch.NIH1;
            ((TextBox)this.Master.FindControl("middleContent").FindControl("Search1").
                    FindControl("txtNIHNum2")).Text = this.mySearch.NIH2;

            ((TextBox)this.Master.FindControl("middleContent").FindControl("Search1").
                    FindControl("txtSrRecordStartDate")).Text = this.mySearch.CreateFrom;

            ((TextBox)this.Master.FindControl("middleContent").FindControl("Search1").
                    FindControl("txtEnRecordEndDate")).Text  = this.mySearch.CreateTo;

            ((ListMultiSelect)this.Master.FindControl("middleContent").
                    FindControl("Search1").FindControl("listCancerType")).SelectedValue =
                    this.mySearch.Cancer.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            ((ListMultiSelect)this.Master.FindControl("middleContent").
                    FindControl("Search1").FindControl("listSubject")).SelectedValue =
                    this.mySearch.Subj.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            ((ListMultiSelect)this.Master.FindControl("middleContent").
                    FindControl("Search1").FindControl("listAudience")).SelectedValue =
                    this.mySearch.Aud.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            ((ListMultiSelect)this.Master.FindControl("middleContent").
                    FindControl("Search1").FindControl("listLang")).SelectedValue =
                    this.mySearch.Lang.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            ((ListMultiSelect)this.Master.FindControl("middleContent").
                    FindControl("Search1").FindControl("listProdFormat")).SelectedValue =
                    this.mySearch.Format.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            
            ((ListMultiSelect)this.Master.FindControl("middleContent").
                    FindControl("Search1").FindControl("listSeries")).SelectedValue =
                    this.mySearch.Serie.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            
            ((ListMultiSelect)this.Master.FindControl("middleContent").
                    FindControl("Search1").FindControl("listRace")).SelectedValue =
                    this.mySearch.Race.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            
            ((ListMultiSelect)this.Master.FindControl("middleContent").
                    FindControl("Search1").FindControl("listBookStatus")).SelectedValue =
                    this.mySearch.Status.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            
            ((ListMultiSelect)this.Master.FindControl("middleContent").
                    FindControl("Search1").FindControl("listReadingLevel")).SelectedValue =
                    this.mySearch.Level.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            ((CheckBox)this.Master.FindControl("middleContent").
                    FindControl("Search1").FindControl("chboxMostCommonList")).Checked =
                    this.mySearch.ROOCOM;

            ((ListMultiSelect)this.Master.FindControl("middleContent").
                    FindControl("Search1").FindControl("listROOSubject")).SelectedValue =
                    this.mySearch.ROOCOMSubj.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            
            ((ListMultiSelect)this.Master.FindControl("middleContent").
                    FindControl("Search1").FindControl("listAward")).SelectedValue =
                    this.mySearch.Award.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            ((ListMultiSelect)this.Master.FindControl("middleContent").
                    FindControl("Search1").FindControl("listOwner")).SelectedValue =
                    this.mySearch.Owner.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            ((ListMultiSelect)this.Master.FindControl("middleContent").
                   FindControl("Search1").FindControl("listSponsor")).SelectedValue =
                   this.mySearch.Sponsor.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
        }

        #endregion
    }
}
