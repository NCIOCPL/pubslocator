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
    public class RotationPubs : MultiSelectListBoxItem
    {
        #region Fields
        private int intConfId;
        private string strConfName;
        private string strConfdate;       
        private string strProdid;
        private string strLongTitle;
        #endregion

        #region Constructors
        public RotationPubs(int confid, string confname)
            : base(confid, confname)
        {         
        }
        public RotationPubs(int confid, string confname, string confDates, string prodId, string longTitle)
            : base(confid, confname)
        {
            this.strConfdate = confDates;
            this.strProdid = prodId;
            this.strLongTitle = longTitle;
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

        public string ConfDates
        {
            get { return this.strConfdate; }
            set { this.strConfdate = value; }
        }
       
        public string ProdID
        {
            get { return this.strProdid; }
            set { this.strProdid = value; }
        }

        public string LongTitle
        {
            get
            {
                if (this.strLongTitle != null)
                    return this.strLongTitle;
                else
                    return String.Empty;
            }
            set { this.strLongTitle = value; }
        }
       
        #endregion
    }
}
