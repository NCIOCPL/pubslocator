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
using System.Collections.Generic;

using PubEntAdmin.BLL;
using PubEntAdmin.DAL;
using AjaxControlToolkit;

namespace PubEntAdmin
{
    public partial class Announcement : System.Web.UI.Page
    {

        #region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session.IsNewSession)//cross-site request forgery
            {
                Response.Redirect("Home.aspx");
            }



            this.Title = "Announcement";
            this.PageTitle = "Announcement";
            this.AddJS("../JS/Announcement.js");



            if (1 == 2) // temp auth fix
            {
                PubEntAdminManager.UnathorizedAccess();
            }

            if (Session[PubEntAdminManager.JS] != null)
            {
                System.Web.UI.UserControl c = null;

                if (System.Convert.ToBoolean(Session[PubEntAdminManager.JS].ToString()))
                {
                    c = (System.Web.UI.UserControl)this.LoadControl("~/UserControl/Announcementx.ascx");
                    c.ID = "Anouncementx1";

                }
                else
                {
                    c = (System.Web.UI.UserControl)this.LoadControl("~/UserControl/Announcement.ascx");
                    c.ID = "Anouncement1";
                }
                this.Master.FindControl("BodyContent").Controls.Clear();
                this.Master.FindControl("BodyContent").Controls.Add(c);
            }


        }
        #endregion

        #region Methods
        public void AddJS(string strJSPath)
        {
            ((System.Web.UI.ScriptManager)this.Master.FindControl("ScriptManager_Default2Master")).
                Scripts.Add(new ScriptReference(strJSPath));
        }
        #endregion

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
