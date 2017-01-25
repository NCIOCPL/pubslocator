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
    public class NewUpdated : SingleSelectCheckBoxItem
    {
        #region Fields
        //private int intAudId;
        //private string strAudName;
       
        private bool blChecked;
        #endregion

        #region Constructors
        public NewUpdated(int newstatus, int updatedstatus, DateTime datet)
            : base(newstatus, updatedstatus, datet)
        {
            this.dtstart = datet;
        }

        #endregion

        #region Properties
        public int newstatus
        {
            get { return this.newstatus; }
            set { this.newstatus = value; }
        }

        public int updatedstatus
        {
            get { return this.updatedstatus; }
            set { this.updatedstatus = value; }
        }

        public DateTime datest
        {
            get { return this.dtstart; }
            set { this.dtstart = value; }
        }

       
        #endregion

    }
}
