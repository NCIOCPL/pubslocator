using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//added
using NCIPLex.DAL;

namespace NCIPLex
{
    public partial class pubmaster : System.Web.UI.MasterPage
    {
        public string idletimeout = "";
        public string idle2timeout = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            idletimeout = SQLDataAccess.GetTimeout(1, 2).ToString() + "000";
            idle2timeout = SQLDataAccess.GetTimeout(1, 3).ToString();
        }
        
        public string LiteralText
        {
            get
            {
                return LiteralListItem.Text;
            }
            set
            {
                LiteralListItem.Text = value;
            }
        }
    }
}
