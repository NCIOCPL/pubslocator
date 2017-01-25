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

using PubEntAdmin.BLL;
using PubEntAdmin.DAL;

namespace PubEntAdmin.CustomControl
{
    public class CkboxListDisplayStatus : System.Web.UI.WebControls.CheckBoxList
    {
        public override void DataBind()
        {
            this.DataSource = PE_DAL.GetAllDisplayStatus(true);
            this.DataTextField = "name";
            this.DataValueField = "id";

            base.DataBind();
        }

        public bool IsOnline
        {
            set
            {
                bool done = false;
                for (int i = 0; i < this.Items.Count && !done; i++)
                {
                    if (this.Items[i].Text == PubEntAdminManager.strDisplayStatus_ONLINE)
                    {
                        done = true;
                        this.Items[i].Selected = true;
                    }
                }
            }
            get
            {
                int i = 0;
                bool done = false;
                for (; i < this.Items.Count && !done; i++)
                {
                    if (this.Items[i].Text == PubEntAdminManager.strDisplayStatus_ONLINE)
                    {
                        done = true;
                    }
                }
                return this.Items[i].Selected;

            }
        }

        public bool IsOrder
        {
            set
            {

                bool done = false;
                for (int i = 0; i < this.Items.Count && !done; i++)
                {
                    if (this.Items[i].Text == PubEntAdminManager.strDisplayStatus_ORDER)
                    {
                        done = true;
                        this.Items[i].Selected = true;
                    }
                }
            }
            get
            {
                int i = 0;
                bool done = false;
                for (; i < this.Items.Count && !done; i++)
                {
                    if (this.Items[i].Text == PubEntAdminManager.strDisplayStatus_ORDER)
                    {
                        done = true;
                        
                    }
                }
                return this.Items[i].Selected;
            }
        }

        public bool IsNERDO
        {
            set
            {
                bool done = false;
                for (int i = 0; i < this.Items.Count && !done; i++)
                {
                    if (this.Items[i].Text == PubEntAdminManager.strDisplayStatus_ContentAndCover)
                    {
                        done = true;
                        this.Items[i].Selected = true;
                    }
                }
            }
            get
            {
                int i = 0;
                bool done = false;
                for (; i < this.Items.Count && !done; i++)
                {
                    if (this.Items[i].Text == PubEntAdminManager.strDisplayStatus_ContentAndCover)
                    {
                        done = true;
                        
                    }
                }
                return this.Items[i].Selected;
            }
        }

        public string SelectedValueToString()
        {
            return SelectedValueToString(",");
        }

        public string SelectedValueToString(string sep)
        {
            string strDisplayStatusSelection = "";
            for (int i = 0; i < this.Items.Count; i++)
            {
                if (this.Items[i].Selected)
                {
                    if (strDisplayStatusSelection.Length > 0)
                        strDisplayStatusSelection += "," + this.Items[i].Value;
                    else
                        strDisplayStatusSelection += this.Items[i].Value;
                }
            }

            return strDisplayStatusSelection;
        }
    }
}
