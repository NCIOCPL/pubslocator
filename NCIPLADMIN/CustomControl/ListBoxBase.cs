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
using System.ComponentModel;
using System.Xml.Linq;

namespace PubEntAdmin.CustomControl
{
    [DefaultProperty("Text"),
        ToolboxData("<{0}:LinkBoxBase runat=server></{0}:LinkBoxBase>")]
    public class ListBoxBase : System.Web.UI.WebControls.ListBox
    {
        public event EventHandler SelectedIndexesChanged;

        protected void OnSelectedIndexesChanged(EventArgs e)
        {
            if (this.SelectedIndexesChanged != null)
            {
                SelectedIndexesChanged(this, e);
            }
        }

        
    }
}
