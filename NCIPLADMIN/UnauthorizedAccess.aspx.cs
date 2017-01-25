using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace PubEntAdmin
{
    public partial class UnauthorizedAccess : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (PubEntAdminManager.TamperProof)
            {
                if (PubEntAdminManager.ContainURLQS(PubEntAdminManager.strUnauthorizedDetail))
                    this.lblreason.Text = PubEntAdminManager.GetURLQS(PubEntAdminManager.strUnauthorizedDetail);
                else
                    this.lblreason.Text = String.Empty;
            }
            else
            {
                if (Request.QueryString[PubEntAdminManager.strUnauthorizedDetail] != null)
                    this.lblreason.Text = Request.QueryString[PubEntAdminManager.strUnauthorizedDetail].ToString();
                else
                    this.lblreason.Text = String.Empty;
            }
        }
    }
}
