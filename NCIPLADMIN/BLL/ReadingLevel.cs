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
    public class ReadingLevel : MultiSelectListBoxItem
    {
        #region Fields
        private bool blnChecked;
        #endregion

        #region Constructors
        public ReadingLevel(int RdlId, string RdlName, bool _checked)
            : base(RdlId, RdlName)
        {
            this.blnChecked = _checked;
        }
        #endregion

        #region Properties
        public int RdlID
        {
            get { return this.ID; }
            set { this.ID = value; }
        }

        public string RdlName
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
