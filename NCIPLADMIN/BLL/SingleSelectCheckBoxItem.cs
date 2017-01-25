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
    [Serializable()]
    public class SingleSelectCheckBoxItem
    {
        public SingleSelectCheckBoxItem()
        {

        }

        public SingleSelectCheckBoxItem(int intnew, int intupdated)
        {
            this.intnewstatus = intnew;
            this.intupdatedstatus = intupdated;
        }

        public SingleSelectCheckBoxItem(int intnew, int intupdated, bool enabled)
        {
            this.intnewstatus = intnew;
            this.intupdatedstatus = intupdated;
            this.blnEnabled = enabled;
        }


        public SingleSelectCheckBoxItem(int intnew, int intupdated, DateTime expdate)
        {
            this.intnewstatus = intnew;
            this.intupdatedstatus = intupdated;
            this.dtstart = expdate;
        }

        #region Properties
        private int intnew;
        private int intupdated;
        private DateTime expdate;
        private bool blnSelected;
        private bool blnEnabled = true;

        public int intnewstatus
        {
            set { this.intnew = value; }
            get { return this.intnew; }
        }

        public int intupdatedstatus
        {
            set { this.intupdated = value; }
            get { return this.intupdated; }
        }

        public DateTime dtstart
        {
            set { this.expdate = value; }
            get { return this.expdate; }
        }

        public bool Selected
        {
            set { this.blnSelected = value; }
            get { return this.blnSelected; }
        }

        public bool Enabled
        {
            set { this.blnEnabled = value; }
            get { return this.blnEnabled; }
        }
        #endregion
    }
}
