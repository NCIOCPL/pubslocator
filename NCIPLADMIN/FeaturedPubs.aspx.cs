using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//added
using PubEntAdmin.BLL;

namespace PubEntAdmin
{
    public partial class FeaturedPubs : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session.IsNewSession)
            {
                Response.Redirect("Home.aspx");
            }
            this.Title = "Featured Publications Setup";
            this.PageTitle = "Featured Publications Setup";
            //this.AddJS(Server.MapPath("JS/LUMgmt.js"));

            if (1 == 2) // temp auth fix
            {
                PubEntAdminManager.UnathorizedAccess();
            }

            this.plcHldContent.Controls.Clear();

            System.Web.UI.UserControl usrCtrl = null;
            usrCtrl = (System.Web.UI.UserControl)this.LoadControl("UserControl/FeaturedPubs.ascx");
            this.plcHldContent.Controls.Add(usrCtrl);
        }

        #region Properties
        public string PageTitle
        {
            set
            {
                ((Label)this.Master.FindControl("lblPageTitle")).Text = value;
            }
            get
            {
                return ((Label)this.Master.FindControl("lblPageTitle")).Text;
            }
        }
        #endregion

    }
}
