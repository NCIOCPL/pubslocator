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
    public class Race : MultiSelectListBoxItem
    {
        #region Fields
        //private int intAudId;
        //private string strAudName;
        private bool blnChecked;
        #endregion

        #region Constructors
        public Race(int audId, string audName, bool _checked)
            : base(audId, audName)
        {
            this.blnChecked = _checked;
        }

        #endregion

        #region Methods
        public static bool SetRace(int pubid, string selectedValue, char delim)
        {
            return PE_DAL.SetRaceByPubID(pubid, selectedValue, delim);
        }
        #endregion

        #region Properties
        public int RaceID
        {
            get { return this.ID; }
            set { this.ID = value; }
        }

        public string RaceName
        {
            get { return this.Name; }
            set { this.Name = value; }
        }

        //from ls
        public int RceID
        {
            get { return this.ID; }
            set { this.ID = value; }
        }

        public string RceName
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
        #endregion

    }
}
