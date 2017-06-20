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
    public class Sponsor : MultiSelectListBoxItem
    {
        #region Fields
       
        private string strDesc;
        private string strSponsorCode;
        private string strLongDesc;
        private bool blnChecked;
        #endregion

        #region Constructors
        public Sponsor(int sponsorid, string desc)
            : base(sponsorid, desc)
        {         
        }
        public Sponsor(int sponsorid, string desc, string code, string longdesc)
            : base(sponsorid, desc)
        {
            this.strDesc = desc;
            this.strSponsorCode = code;
            this.strLongDesc = longdesc;
            
        }

        public Sponsor(int sponsorid, string desc, string code, string longdesc, bool _checked)
            : base(sponsorid, desc)
        {
            this.strDesc = desc;
            this.strSponsorCode = code;
            this.strLongDesc = longdesc;
            this.blnChecked = _checked;
        }
       
        #endregion

        #region Properties
        public int SponsorID
        {
            get { return this.ID; }
            set { this.ID = value; }
        }

        public string Description
        {
            get { return this.Name; }
            set { this.Name = value; }
        }

        public string LongDescription
        {
            get { return this.strLongDesc; }
            set { this.strLongDesc = value; }
        }

        public string SponsorCode
        {
            get { return this.strSponsorCode; }
            set { this.strSponsorCode = value; }
        }
       
        public bool Checked
        {
            get { return this.blnChecked; }
            set { this.blnChecked = value; }
        }
        #endregion
    }
}
