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

namespace PubEntAdmin.UserControl
{
    public partial class AdminMenu : System.Web.UI.UserControl
    {
        #region Events Handling
        protected void Page_Load(object sender, EventArgs e)
        {
            this.AssignMenu(); 
        }

        protected void Menu_OnMenuItemDataBound(object sender, MenuEventArgs e)
        {
            if (String.Compare(e.Item.Text.Trim(), PubEntAdminManager.RECORD_MENUITEM, true)==0 ||
                String.Compare(e.Item.Text.Trim(), PubEntAdminManager.REPORTS_MENUITEM, true) == 0 ||
                String.Compare(e.Item.Text.Trim(), PubEntAdminManager.LOOKUPS_MENUITEM, true) == 0 ||
                String.Compare(e.Item.Text.Trim(), "ORDERS") == 0)
            {
                e.Item.Selectable = false;
            }

            if (String.Compare(e.Item.Text.Trim(), PubEntAdminManager.HELP_MENUITEM, true) == 0)
            {
                if (!this.Page.ClientScript.IsClientScriptBlockRegistered("AdminClientScript"))
                {
                    this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "AdminClientScript",
                        @"
                            function OpenHelpInNewWin(address, heigth, width, title)  
                            {  
                             var winl = (screen.width-width)/2;  
                             var wint = (screen.height-heigth)/2;  
                             var options = ""width="" + width;  
                             options += "",height="" + heigth;  
                             options += "",top="" + wint;  
                             options += "",left="" + winl;  
                             options += "",location=no,toolbar=no, menubar=no, scrollbars=1, resizable"";  
                               
                             window.open(address, title, options);  
                            }
                        ", true);
                }
                e.Item.NavigateUrl = "javascript:OpenHelpInNewWin('/ncipladmin/Help/index.html',500,800,'HelpWin')";
            }

            if (String.Compare(e.Item.Text.Trim(), PubEntAdminManager.LOGOUT_MENUITEM, true) == 0)
            {
                //// e.Item.NavigateUrl = "javascript:window.close();";
            }

        }
        #endregion

        #region Methods
        protected void AssignMenu()
        {
            /// Allowing all other users to use AdminMenu.xml as source until authorization is fixed
            this.XmlDataSrcMenu.DataFile = "~/Xml/AdminMenu.xml";

            /*
            if (((CustomPrincipal)Context.User).IsInRole(PubEntAdminManager.AdminRole))
            {
                this.XmlDataSrcMenu.DataFile = "~/Xml/AdminMenu.xml";
            }
            else if (((CustomPrincipal)Context.User).IsInRole(PubEntAdminManager.DWHStaffRole))
            {
                this.XmlDataSrcMenu.DataFile = "~/Xml/DWHStaffMenu.xml";
            }
            else if (((CustomPrincipal)Context.User).IsInRole(PubEntAdminManager.RURole))
            {
                this.XmlDataSrcMenu.DataFile = "~/Xml/RUMenu.xml";
            }
            */

            if (Session[PubEntAdminManager.JS] != null)
            {
                if (System.Convert.ToBoolean(Session[PubEntAdminManager.JS].ToString()))
                {
                    this.AdminMenu1.Visible = true;

                    this.xjsRecordMenu.Visible = this.xjsSearchMenu.Visible = false;
                    this.xjsReportsMenu.Visible = this.xjsLookupsMenu.Visible = false;
                    this.xjsLogoutMenu.Visible = this.xjsHelpMenu.Visible = false;
                    this.xjsCannedMenu.Visible = false;
                }
                else
                {
                    this.AdminMenu1.Visible = false;

                    if (!((CustomPrincipal)Context.User).IsInRole(PubEntAdminManager.AdminRole))
                    {
                        this.xjsRecordMenu.Visible = this.xjsLookupsMenu.Visible = false;
                        this.xjsCannedMenu.Visible = false;
                    }
                    else
                    {
                        this.xjsRecordMenu.Visible = this.xjsLookupsMenu.Visible = true;
                        this.xjsCannedMenu.Visible = true;
                    }

                    this.xjsSearchMenu.Visible = this.xjsReportsMenu.Visible = true;
                    this.xjsLogoutMenu.Visible = this.xjsHelpMenu.Visible = true;

                }
            }
            else
            {
                this.AdminMenu1.Visible = true;

                this.xjsRecordMenu.Visible = this.xjsSearchMenu.Visible = false;
                this.xjsReportsMenu.Visible = this.xjsLookupsMenu.Visible = false;
                this.xjsLogoutMenu.Visible = this.xjsHelpMenu.Visible = false;
                this.xjsCannedMenu.Visible = false;
            }

        }
        #endregion

    }
}