using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PubEnt.usercontrols
{
    public partial class steps : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public void Activate(string s)
        {
            ((TableCell)this.FindControl("cell1")).CssClass = "progressCurrent";
            ((TableCell)this.FindControl("cell3")).CssClass = "progressInactive";
            ((TableCell)this.FindControl("cell4")).CssClass = "progressInactive";
            ((TableCell)this.FindControl("cell5")).CssClass = "progressInactive";

            switch (s)
            {
                case "cell3"://on Shipping
                    ((TableCell)this.FindControl("cell1")).Attributes.Add("onclick", "window.location.href='" + Page.ResolveUrl("cart.aspx") + "'");
                    ((TableCell)this.FindControl("cell1")).CssClass = "progressPrev";
                    ((TableCell)this.FindControl("cell3")).CssClass = "progressCurrent";
                    break;
                case "cell4"://on Verify
                    ((TableCell)this.FindControl("cell1")).Attributes.Add("onclick", "window.location.href='" + Page.ResolveUrl("cart.aspx") + "'");
                    ((TableCell)this.FindControl("cell3")).Attributes.Add("onclick", "window.location.href='" + Page.ResolveUrl("shipping.aspx") + "'");
                    ((TableCell)this.FindControl("cell1")).CssClass = "progressPrev";
                    ((TableCell)this.FindControl("cell3")).CssClass = "progressPrev";
                    ((TableCell)this.FindControl("cell4")).CssClass = "progressCurrent";
                    break;
                case "cell5"://on Conf
                    //***EAC no links activated ---THIS IS BY DESIGN
                    break;
                default: //on Shopping Cart
                    break;
            }
        }
    }
}