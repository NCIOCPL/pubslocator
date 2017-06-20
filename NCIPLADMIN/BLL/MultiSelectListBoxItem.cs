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
    public class MultiSelectListBoxItem
    {
        public MultiSelectListBoxItem()
        {

        }

        public MultiSelectListBoxItem(int intId, string strName)
        {
            this.ID = intId;
            this.Name = strName;
        }

        public MultiSelectListBoxItem(int intId, string strName, bool enabled)
        {
            this.ID = intId;
            this.Name = strName;
            this.blnEnabled = enabled;
        }


        #region Properties
        private int intId;
        private string strName;
        private bool blnSelected;
        private bool blnEnabled = true;

        public int ID
        {
            set { this.intId = value; }
            get { return this.intId; }
        }

        public string Name
        {
            set { this.strName = value; }
            get { return this.strName; }
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
