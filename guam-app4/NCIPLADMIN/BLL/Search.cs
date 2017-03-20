using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace PubEntAdmin.BLL
{
    [Serializable]
    public class Search
    {
        #region Fields
        private string strPubIDs = "";

        private bool blnNCIPL = false;
        private bool blnROO = false;
        private bool blnExh = false;
        private bool blnCatalog = false;

        private string strKeyword = "";
        private string strNIH1 = "";
        private string strNIH2 = "";
        private string strCreateFrom = "";
        private string strCreateTo = "";
        private bool blnNew = false;
        private bool blnUpdated = false;
        private string strCancer = "";
        private string strSubj = "";
        private string strAud = "";
        private string strLang = "";
        private string strFormat = "";
        private string strSerie = "";
        private string strRace = "";
        private string strStatus = "";
        private string strLevel = "";
        private bool blnROOCOM = false;
        private string strROOCOMSubj = "";
        private string strAward= "";
        private string strOwner = "";
        private string strSponsor = "";

        private string strSearchCriteriaDisplay = "";
        #endregion

        #region Properties
        public string PUBIDs
        {
            set { this.strPubIDs = value; }
            get { return this.strPubIDs; }
        }

        public bool NCIPL
        {
            set { this.blnNCIPL = value; }
            get { return this.blnNCIPL; }
        }

        public bool ROO
        {
            set { this.blnROO = value; }
            get { return this.blnROO; }
        }

        public bool EXH
        {
            set { this.blnExh = value; }
            get { return this.blnExh; }
        }

        public bool CATALOG
        {
            set { this.blnCatalog = value; }
            get { return this.blnCatalog; }
        }

        public bool ISNEW
        {
            set { this.blnNew = value; }
            get { return this.blnNew; }
        }

        public bool ISUPDATED
        {
            set { this.blnUpdated = value; }
            get { return this.blnUpdated; }
        }

        public int Count
        {
            get
            {
                return this.PUBIDs.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Length;
            }
        }

        public string Key { set { this.strKeyword = value; } get { return this.strKeyword; } }
        public string NIH1 { set { this.strNIH1 = value; } get { return this.strNIH1; } }
        public string NIH2 { set { this.strNIH2 = value; } get { return this.strNIH2; } }

        public string CreateFrom { set { this.strCreateFrom = value; } get { return this.strCreateFrom; } }
        public string CreateTo { set { this.strCreateTo = value; } get { return this.strCreateTo; } }

        public string Cancer { set { this.strCancer = value; } get { return this.strCancer; } }
        public string Subj { set { this.strSubj = value; } get { return this.strSubj; } }
        public string Aud { set { this.strAud = value; } get { return this.strAud; } }
        public string Lang { set { this.strLang = value; } get { return this.strLang; } }
        public string Format { set { this.strFormat = value; } get { return this.strFormat; } }
        public string Serie { set { this.strSerie = value; } get { return this.strSerie; } }
        public string Race { set { this.strRace = value; } get { return this.strRace; } }
        public string Status { set { this.strStatus = value; } get { return this.strStatus; } }
        public string Level { set { this.strLevel = value; } get { return this.strLevel; } }
        public bool ROOCOM { set { this.blnROOCOM = value; } get { return this.blnROOCOM; } }
        public string ROOCOMSubj { set { this.strROOCOMSubj = value; } get { return this.strROOCOMSubj; } }
        public string Award { set { this.strAward = value; } get { return this.strAward; } }
        public string SearchCriteriaDisplay { set { this.strSearchCriteriaDisplay = value; } get { return this.strSearchCriteriaDisplay; } }
        public string Owner { set { this.strOwner = value; } get { return this.strOwner; } }
        public string Sponsor { set { this.strSponsor = value; } get { return this.strSponsor; } }
        #endregion

        #region Methods
        public void AddPubId(int pubid)
        {
            StringBuilder sb = new StringBuilder(this.PUBIDs);
            if (sb.Length > 0)
                sb.Append(",");
            sb.Append(pubid.ToString());
            this.PUBIDs = sb.ToString();
            //if (this.PUBIDs.Length > 0)
            //    this.PUBIDs += ",";
            //this.PUBIDs += pubid.ToString();
        }

        public void ClearPubId()
        {
            this.PUBIDs = String.Empty;
        }
        #endregion

        public Search()
        {

        }
    }


}
