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

namespace PubEntAdmin.CustomControl
{
    public class RdbtnListYesNo : System.Web.UI.WebControls.RadioButtonList
    {
        public override void DataBind()
        {
            MultiSelectListBoxItem one = new MultiSelectListBoxItem(0, "YES");
            MultiSelectListBoxItem two = new MultiSelectListBoxItem(1, "NO");
            MultiSelectListBoxItemCollection col = new MultiSelectListBoxItemCollection();
            col.Add(one);
            col.Add(two);

            this.DataSource = col;
            this.DataTextField = "name";
            this.DataValueField = "id";
           
            base.DataBind();

        }

        public bool Default
        {
            set
            {
                if (this.Items == null)
                    this.DataBind();

                this.Yes = value;
            }
            get
            {
                return this.Items[0].Selected ? true : false;
            }
        }

        public bool Selected()
        {
            if (!this.Items[0].Selected && !this.Items[1].Selected)
                return false;
            else
                return true;
        }

        public bool Yes
        {
            set
            {
                this.Items[0].Selected = value;
                this.Items[1].Selected = !value;
            }
            get
            {
                return this.Items[0].Selected ? true : false;
            }
        }

        public bool No
        {
            set
            {
                this.Items[0].Selected = !value;
                this.Items[1].Selected = value;
            }
            get
            {
                return this.Items[1].Selected ? true : false;
            }
        }
    }

    
}
