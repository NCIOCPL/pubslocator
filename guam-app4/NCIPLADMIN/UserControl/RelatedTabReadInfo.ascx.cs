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

namespace PubEntAdmin.UserControl
{
    public partial class RelatedTabReadInfo : System.Web.UI.UserControl
    {
        #region Fields
        private int intPubID;
        #endregion

        #region Events Handling
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.PubID>0)
            this.BindData();

            PubEntAdminManager.AssignMonitorTabChangeValuesOnPageLoadInReadMode(this.Page);
        }
        #endregion

        #region Methods
        //public void ReBindData()
        //{
        //    this.BindData();
        //}

        protected void BindData()
        {
            this.gvResult.DataSource = PE_DAL.GetRelatedPubDisplay(this.PubID);
            this.gvResult.DataBind();
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