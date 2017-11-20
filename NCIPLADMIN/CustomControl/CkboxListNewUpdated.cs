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
    public class CkboxListNewUpdated : System.Web.UI.WebControls.CheckBoxList
    {

        public override void DataBind()
        {
            MultiSelectListBoxItem one = new MultiSelectListBoxItem(0, "New");
            MultiSelectListBoxItem two = new MultiSelectListBoxItem(1, "Updated");
            MultiSelectListBoxItemCollection col = new MultiSelectListBoxItemCollection();
            col.Add(one);
            col.Add(two);

            this.DataSource = col;
            this.DataTextField = "name";
            this.DataValueField = "id";

            base.DataBind();

        }

        public void DataBinder(int pubid)
        {
            this.DataSource = PE_DAL.GetNewUpdatedByPubID(pubid,true);
            this.DataTextField = "name";
            this.DataValueField = "id";

            base.DataBind();
        }

        public bool New
        {
            set
            {
                this.Items[0].Selected = value;
            }
            get
            {
                return this.Items[0].Selected ? true : false;
            }
        }

        public bool Updated
        {
            set
            {
                this.Items[1].Selected = value;
            }
            get
            {
                return this.Items[1].Selected ? true : false;
            }
        }
    }
}
