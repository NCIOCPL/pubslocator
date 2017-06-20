using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using PubEntAdmin.BLL;
using PubEntAdmin.DAL;

namespace PubEntAdmin.UserControl
{
    public partial class cslistbox : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        public ListBox listBoxCtrl
        {
            get { return LookUpList; }
            set { LookUpList = value; } 
        }

        public string SelValues
        {
            get
            {
                string ret = "";
                string sep = "";
                foreach (ListItem item in LookUpList.Items)
                {
                    if (item.Selected)
                    {
                        ret += sep + item.Value;
                        sep = ",";
                    }
                }
                return ret;
            }
        }

        public void ClearValues()
        {
            foreach (ListItem item in LookUpList.Items)
                item.Selected = false;
        }

        public void SecurityCheck()
        {
            foreach (ListItem li in LookUpList.Items)
            {
                //if (PubEntAdminManager.OtherVal(li.Text) || PubEntAdminManager.OtherVal(li.Value))
                if (!PubEntAdminManager.ContentNumVal(li.Value))
                {
                    Response.Redirect("InvalidInput.aspx");
                }
            }
        }
    }
}