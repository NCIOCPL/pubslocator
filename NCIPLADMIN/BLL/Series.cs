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

using PubEntAdmin.DAL;

namespace PubEntAdmin.BLL
{
    public class Series : MultiSelectListBoxItem
    {
        #region Fields
        //private int intAudId;
        //private string strAudName;
        private bool blnChecked;
        private bool blnInNCIPL;
        private bool blnInNCIPl_CC;
        #endregion

        #region Constructors
        public Series(int audId, string audName, bool _checked)
            : base(audId, audName)
        {
            this.blnChecked = _checked;
        }

        //NCIPL_CC
        public Series(int audId,
                        string audName,
                        bool _checked,
                        bool inNCIPL,
                        bool inNCIPL_CC)
            : base(audId, audName)
        {
            this.blnChecked = _checked;
            this.blnInNCIPL = inNCIPL;
            this.blnInNCIPl_CC = inNCIPL_CC;
        }
        #endregion

        #region Methods
        public static bool SetSeries(int pubid, string selectedValue, char delim)
        {
            return PE_DAL.SetSeriesByPubID(pubid, selectedValue, delim);
        }
        #endregion

        #region Properties
        public int SeriesID
        {
            get { return this.ID; }
            set { this.ID = value; }
        }

        public string SeriesName
        {
            get { return this.Name; }
            set { this.Name = value; }
        }
        //from ls
        public int SreID
        {
            get { return this.ID; }
            set { this.ID = value; }
        }

        public string SreName
        {
            get { return this.Name; }
            set { this.Name = value; }
        }
        //from ls
        public bool Checked
        {
            get { return this.blnChecked; }
            set { this.blnChecked = value; }
        }
        public bool InNCIPL
        {
            get { return this.blnInNCIPL; }
            set { this.blnInNCIPL = value; }
        }
        public bool InNCIPL_CC
        {
            get { return this.blnInNCIPl_CC; }
            set { this.blnInNCIPl_CC = value; }
        }
        #endregion

    }
}
