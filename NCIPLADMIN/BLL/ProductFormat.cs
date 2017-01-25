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
    public class ProductFormat : MultiSelectListBoxItem
    {
        #region Fields
        private bool blnChecked;
        #endregion

        #region Constructors
        public ProductFormat(int PftId, string PftName, bool _checked)
            : base(PftId, PftName)
        {
            this.blnChecked = _checked;
        }
        #endregion

        #region Properties
        public int PftID
        {
            get { return this.ID; }
            set { this.ID = value; }
        }

        public string PftName
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
