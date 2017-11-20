using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using NCIPLex.GlobalUtils;

namespace NCIPLex.help
{
    public partial class nciplexhelp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Utils.ValidateRedirect().Length > 0) //Important check
                Response.Redirect(Utils.ValidateRedirect(), true);
            
            if (!Page.IsPostBack)
            {
                //Display the master page tabs 
                Utils UtilMethod = new GlobalUtils.Utils();
                if (Session["NCIPLEX_Pubs"] != null)
                    Master.LiteralText = UtilMethod.GetTabHtmlMarkUp(Session["NCIPLEX_Qtys"].ToString(), "");
                else
                    Master.LiteralText = UtilMethod.GetTabHtmlMarkUp("", "");
                UtilMethod = null;
            }
        }
    }
}
