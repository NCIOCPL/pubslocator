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
    public partial class LiveNewUpStatus : System.Web.UI.UserControl
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
            set { this.ckboxLiveNewUpdateStatus.RepeatDirection = value; }
            get { return this.ckboxLiveNewUpdateStatus.RepeatDirection; }
        }

        public bool SelectedAny
        {
            get
            {
                for (int i = 0; i < this.ckboxLiveNewUpdateStatus.Items.Count; i++)
                {
                    if (this.ckboxLiveNewUpdateStatus.Items[i].Selected)
                        return true;
                }
                return false;
            }
        }

        public bool InNew
        {
            get { return this.ckboxLiveNewUpdateStatus.Items.FindByValue("New").Selected; }
            set { this.ckboxLiveNewUpdateStatus.Items.FindByValue("New").Selected = value; }
        }

        public bool InUpdated
        {
            get { return this.ckboxLiveNewUpdateStatus.Items.FindByValue("Updated").Selected; }
            set { this.ckboxLiveNewUpdateStatus.Items.FindByValue("Updated").Selected = value; }
        }

 

        public ListItemCollection Items
        {
            get { return this.ckboxLiveNewUpdateStatus.Items; }
        }
        #endregion

        #region Methods
        protected void BindData_Helper()
        {
            bool active = true;
            LiveNewUpd l_liveInt = PE_DAL.GetLiveNewUpdatedByPubID(this.PubID);
            this.InNew = System.Convert.ToBoolean(l_liveInt.New);
            this.InUpdated = System.Convert.ToBoolean(l_liveInt.Updated);
        }

        protected void BindData()
        {
            this.BindData_Helper();

           
            
        }

        public void clearAll()
        {
            for (int i = 0; i < this.ckboxLiveNewUpdateStatus.Items.Count; i++)
            {
                this.ckboxLiveNewUpdateStatus.Items[i].Selected = false;
            }
        }

        public bool Save()
        {
            return PE_DAL.SetLiveNewUpdatePubID(this.PubID);
        }
        #endregion
    }
}