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
    public class Award : MultiSelectListBoxItem
    {
        #region Fields
        //private int intAudId;
        //private string strAudName;
        private string strAwdName;
        private string strAwdCate;
        private string strAwdYear;
        private bool blnChecked;
        #endregion

        #region Constructors
        public Award(int awdId, string awdDesc)
            : base(awdId, awdDesc)
        {
        }
       
        public Award(int awdId, string awdDesc, bool _checked)
            : this(awdId, awdDesc)
        {
            this.blnChecked = _checked;
        }

        public Award(int awId, string awdDesc, string awdName, string awdCategory, string awdYear, bool _checked)
            : this(awId, awdDesc)
        {
            this.strAwdName=awdName;
            this.strAwdCate=awdCategory;
            this.strAwdYear = awdYear;
            this.blnChecked = _checked;
        }
        #endregion

        #region Methods
        public static bool SetAward(int pubid, string selectedValue, char delim)
        {
            return PE_DAL.SetAwardByPubID(pubid, selectedValue, delim);
        }
        #endregion

        #region Properties
        public int AwardID
        {
            get { return this.ID; }
            set { this.ID = value; }
        }

        public string AwardDescription
        {
            get { return this.Name; }
            set { this.Name = value; }
        }
       
        public string AwdName
        {
            get { return this.strAwdName; }
            set { this.strAwdName = value; }
        }

        public string AwdCategory
        {
            get { return this.strAwdCate; }
            set { this.strAwdCate = value; }
        }

        public string AwdYear
        {
            get { return this.strAwdYear; }
            set { this.strAwdYear = value; }
        }

        public bool Checked
        {
            get { return this.blnChecked; }
            set { this.blnChecked = value; }
        }
        //from ls
        #endregion

    }
}
