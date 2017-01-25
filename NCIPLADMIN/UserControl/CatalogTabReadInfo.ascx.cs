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

using PubEntAdmin.DAL;
using GlobalUtils;

namespace PubEntAdmin.UserControl
{
    public partial class CatalogTabReadInfo : System.Web.UI.UserControl
    {
        #region Fields
        private int intPubID;
        #endregion

        #region Events Handling
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.PubID > 0)
            this.BindData();

            PubEntAdminManager.AssignMonitorTabChangeValuesOnPageLoadInReadMode(this.Page);
        }

        protected void dgInfoView_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
        }
        #endregion

        #region Methods
        protected void BindData()
        {
            DataSet ds = PE_DAL.GetCatalogInterfaceView(this.PubID);
            this.dgInfoView.DataSource = DataSetRoutines.FlipDataSet(ds);
            this.dgInfoView.DataBind();
        }

        protected bool Save()
        {
            return true;
        }
        #endregion

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
        #endregion
    }
}