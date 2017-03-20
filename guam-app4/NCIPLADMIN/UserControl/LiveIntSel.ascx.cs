using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using PubEntAdmin.BLL;
using PubEntAdmin.DAL;

namespace PubEntAdmin.UserControl
{
    public partial class LiveIntSel : System.Web.UI.UserControl
    {
        #region Fields
        private int intPubID;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            if (this.PubID > 0)
                this.BindData();
            
        }

        #region Properties
        public int PubID
        {
            set
            {
                this.intPubID = value;
            }
            get
            {
                return this.intPubID;
            }
        }
        public RepeatDirection RepeatDirection
        {
            set {this.ckboxLiveIntSel.RepeatDirection = value;}
            get { return this.ckboxLiveIntSel.RepeatDirection; }
        }

        public bool SelectedAny
        {
            get
            {
                for (int i = 0; i < this.ckboxLiveIntSel.Items.Count; i++)
                {
                    if (this.ckboxLiveIntSel.Items[i].Selected)
                        return true;
                }
                return false;
            }
        }

        public bool InNCIPL
        {
            get { return this.ckboxLiveIntSel.Items.FindByValue("NCIPL").Selected; }
            set { this.ckboxLiveIntSel.Items.FindByValue("NCIPL").Selected = value; }
        }

        public bool InROO
        {
            get { return this.ckboxLiveIntSel.Items.FindByValue("ROO").Selected; }
            set { this.ckboxLiveIntSel.Items.FindByValue("ROO").Selected = value; }
        }

        public bool InExh
        {
            get { return this.ckboxLiveIntSel.Items.FindByValue("Exhibit").Selected; }
            set { this.ckboxLiveIntSel.Items.FindByValue("Exhibit").Selected = value; }
        }

        public bool InCatalog
        {
            get { return this.ckboxLiveIntSel.Items.FindByValue("Catalog").Selected; }
            set { this.ckboxLiveIntSel.Items.FindByValue("Catalog").Selected = value; }
        }

        public ListItemCollection Items
        {
            get { return this.ckboxLiveIntSel.Items; }
        }
        #endregion

        #region Methods
        protected void BindData_Helper()
        {
            LiveInt l_liveInt = PE_DAL.GetLiveIntByPubID(this.PubID);
            this.InNCIPL = l_liveInt.NCIPL == 0?false:true;
            this.InROO = l_liveInt.ROO == 0 ? false : true;
            this.InExh = l_liveInt.Exhibit == 0 ? false : true;
            this.InCatalog = l_liveInt.Catalog == 0 ? false : true;
        }

        protected void BindData()
        {
            this.BindData_Helper();

            //if (Session[PubEntAdminManager.strPubGlobalMode] != null)
            //{
                

            //    string selMode = Session[PubEntAdminManager.strPubGlobalMode].ToString();
            //    if (selMode == PubEntAdminManager.strPubGlobalVMode)
            //    {

            //    }
            //    else if (selMode == PubEntAdminManager.strPubGlobalEMode ||
            //        selMode == PubEntAdminManager.strPubGlobalAMode)
            //    {

            //    }
                
                       
                
            //}
            
        }

        public void clearAll()
        {
            for (int i = 0; i < this.ckboxLiveIntSel.Items.Count; i++)
            {
                this.ckboxLiveIntSel.Items[i].Selected = false;
            }
        }

        public bool Save()
        {
            return PE_DAL.SetLiveIntByPubID(this.PubID,
                this.InNCIPL ? 1 : 0, this.InROO ? 1 : 0, this.InExh ? 1 : 0, this.InCatalog ? 1 : 0);
        }

        //01-09-2012 - Added for NCIPL_CC. Hide specific list items if needed
        public void SetInterfacesToShow(string LookupName)
        {
            switch (LookupName)
            {
                case "Series":
                    ckboxLiveIntSel.Items.Remove("Exhibit");
                    ckboxLiveIntSel.Items.Remove("Catalog");
                    break;
                default:
                    //Do not do anything
                    break;
            }
        }
        //Added for NCIPL_CC
        public void selectAll()
        {
            for (int i = 0; i < this.ckboxLiveIntSel.Items.Count; i++)
            {
                this.ckboxLiveIntSel.Items[i].Selected = true;
            }
        }

        #endregion
    }
}