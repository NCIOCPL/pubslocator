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
    public class Owner : MultiSelectListBoxItem
    {
        #region Fields

        private string strFname;
        private string strLName;
        private string strLongName;
        private string StrMiddleInt;       
        private bool blnChecked;

        #endregion

        #region Constructors
        public Owner(int ownerid, string fullname)
            : base(ownerid, fullname)
        {         
        }
        public Owner(int ownerid, string firstname, string lastName, string middleInt, string longname)
            : base(ownerid, longname)
        {
            this.strFname = firstname;
            this.strLName = lastName;
            this.StrMiddleInt = middleInt;
            this.strLongName = longname;                       
        }

        public Owner(int ownerid, string firstname, string lastName, string middleInt, string longname, bool _checked)
            : base(ownerid, longname)
        {
            this.strFname = firstname;
            this.strLName = lastName;
            this.StrMiddleInt = middleInt;
            this.strLongName = longname;            
            this.blnChecked = _checked;
        }

        #endregion

        #region Properties
        public int OwnerID
        {
            get { return this.ID; }
            set { this.ID = value; }
        }

        public string FullName
        {
            get { return this.Name; }
            set { this.Name = value; }
        }

        public string LongName
        {
            get { return this.strLongName; }
            set { this.strLongName = value; }
        }

        public string FirstName
        {
            get { return this.strFname; }
            set { this.strFname = value; }
        }

        public string LastName
        {
            get { return this.strLName; }
            set { this.strLName = value; }
        }

        public string MiddleInitial
        {
            get { return this.StrMiddleInt; }
            set { this.StrMiddleInt = value; }
        }

        public bool Checked
        {
            get { return this.blnChecked; }
            set { this.blnChecked = value; }
        }
        #endregion
     
    }
}
