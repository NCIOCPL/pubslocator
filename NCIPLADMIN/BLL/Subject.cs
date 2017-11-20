using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace PubEntAdmin.BLL
{
    public class Subject : MultiSelectListBoxItem
    {
        #region Fields
        private bool blnInNCIPL;
        private bool blnInROO;
        private bool blnInExh;
        private bool blnInCatalog;
        private bool blnHasSubCat;
        private bool blnActive;
        private int intSortSeq;
        private bool blnCantRem;
        #endregion

        #region Constructors
        public Subject(int subjId, string subjName, bool inNCIPL, bool inROO,
            bool inExh, bool inCatalog, bool hasSubCat, int sortSeq, bool subjActive)
            : this(subjId,subjName,hasSubCat,sortSeq,subjActive)
        {
            this.blnInNCIPL = inNCIPL;
            this.blnInROO = inROO;
            this.blnInExh = inExh;
            this.blnInCatalog = inCatalog;
            this.blnHasSubCat = hasSubCat;
        }
        /// <summary>
        /// subject
        /// </summary>
        public Subject(bool CantRem, int subjId, string subjName, bool inNCIPL, bool inROO,
            bool inExh, bool inCatalog, bool hasSubCat, bool subjActive)
            : base(subjId, subjName)
        {
            this.blnCantRem = CantRem;
            this.blnInNCIPL = inNCIPL;
            this.blnInROO = inROO;
            this.blnInExh = inExh;
            this.blnInCatalog = inCatalog;
            this.blnHasSubCat = hasSubCat;
            this.blnActive = subjActive;
        }

        public Subject(int subjId, string subjName, bool subjActive)
            : base(subjId, subjName)
        {
            this.blnActive = subjActive;
        }
        
        public Subject(int subjId, string subjName, bool hasSubCat, int sortSeq, bool subjActive)
            : base(subjId, subjName)
        {
            this.blnHasSubCat = hasSubCat;
            this.blnActive = subjActive;
            this.intSortSeq = sortSeq;
        }
        #endregion

        #region Properties
        public bool CannotRem
        {
            get { return this.blnCantRem; }
            
        }

        public int SubjID
        {
            get { return this.ID; }
            set { this.ID = value; }
        }

        public string SubjName
        {
            get { return this.Name; }
            set { this.Name = value; }
        }

        public bool InNCIPL
        {
            get { return this.blnInNCIPL; }
            set { this.blnInNCIPL = value; }
        }

        public bool InROO
        {
            get { return this.blnInROO; }
            set { this.blnInROO = value; }
        }

        public bool InExh
        {
            get { return this.blnInExh; }
            set { this.blnInExh = value; }
        }

        public bool InCatalog
        {
            get { return this.blnInCatalog; }
            set { this.blnInCatalog = value; }
        }

        public bool HasSubCat
        {
            get { return this.blnHasSubCat; }
            set { this.blnHasSubCat = value; }
        }

        public bool Active
        {
            get { return this.blnActive; }
            set { this.blnActive = value; }
        }
        #endregion

    }
}
