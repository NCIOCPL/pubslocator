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
    public class Language : MultiSelectListBoxItem
    {
        #region Fields
        private bool blnChecked;
        #endregion

        #region Constructors
        public Language(int LngId, string LngName, bool _checked)
            : base(LngId, LngName)
        {
            this.blnChecked = _checked;
        }
        #endregion

        #region Properties
        public int LngID
        {
            get { return this.ID; }
            set { this.ID = value; }
        }

        public string LngName
        {
            get { return this.Name; }
            set { this.Name = value; }
        }

        public bool Checked
        {
            get { return this.blnChecked; }
            set { this.blnChecked = value; }
        }
        #endregion

    }
}
