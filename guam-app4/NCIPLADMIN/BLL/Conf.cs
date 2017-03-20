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
    public class Conf : MultiSelectListBoxItem
    {
        #region Fields
        private int intConfId;
        private string strConfName;
        private DateTime dateStarted;
        private DateTime dateEnded;
        private bool blnActive;
        private int iMaxIntlOrder;
        #endregion

        #region Constructors
        public Conf(int confid, string confname)
            : base(confid, confname)
        {         
        }
        public Conf(int confid, string confname,int maxIntlOrder, DateTime dateStart, DateTime dateEnd)
            : base(confid, confname)
        {
            this.iMaxIntlOrder = maxIntlOrder;
            this.dateStarted = dateStart;
            this.dateEnded = dateEnd;
        }

        public Conf(int confid, string confname, int maxIntlOrder, DateTime dateStart, DateTime dateEnd, bool confjActive)
            : base(confid, confname)
        {
            this.iMaxIntlOrder = maxIntlOrder;
            this.dateStarted = dateStart;
            this.dateEnded = dateEnd;
            this.blnActive = confjActive;
        }
       
        #endregion

        #region Properties
        public int ConfID
        {
            get { return this.ID; }
            set { this.ID = value; }
        }

        public string ConfName
        {
            get { return this.Name; }
            set { this.Name = value; }
        }

        public DateTime StartDate
        {
            get { return this.dateStarted; }
            set { this.dateStarted = value; }
        }

        public DateTime EndDate
        {
            get { return this.dateEnded; }
            set { this.dateEnded = value; }
        }
        public bool Active
        {
            get { return this.blnActive; }
            set { this.blnActive = value; }
        }

        public int MaxOrder_INTL
        {
            get { return this.iMaxIntlOrder; }
            set { this.iMaxIntlOrder = value; }
        }
        #endregion
    }
    
}
