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
using PubEntAdmin.DAL;
using System.Web.SessionState;

namespace PubEntAdmin.Guam
{
    public partial class edituser : System.Web.UI.Page
    {
        private int _userid
        {
            get
            {
                object o = ViewState["_userid"];
                if (o == null)
                {
                    return 0;
                }
                return (int)o;
            }

            set
            {
                ViewState["_userid"] = value;
            }
        }
        KVPairCollection kvpaircoll;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session.IsNewSession)
            {
                Response.Redirect("~/Home.aspx");
            }
            System.Web.UI.UserControl userControl = (System.Web.UI.UserControl)this.LoadControl("~/UserControl/AdminMenu.ascx");
            this.plcHldMenu.Controls.Add(userControl);
            if (!IsPostBack)
            {
                if (Request.QueryString["userid"] != null)
                {
                    _userid = Int32.Parse(Request.QueryString["userid"]);
                    CISUser c = PubEntAdmin.BLL.CISUser.GetGuamUserById(_userid);

                    lblUsername.Text = c.Email;

                    kvpaircoll = DAL.PE_DAL.GetGuamRoles();
                    drpRoles.DataSource = kvpaircoll;
                    drpRoles.DataTextField = "Val";
                    drpRoles.DataValueField = "Val";
                    drpRoles.DataBind();
                    drpRoles.SelectedValue = c.Role;
                    //foreach (KVPair kvpair in kvpaircoll)
                    //{
                    //    ListItem lstItem = new ListItem();
                    //    lstItem.Text = kvpair.Val;
                    //    lstItem.Value = kvpair.Key;
                    //    if (kvpair.IsSelected == "1")
                    //        lstItem.Selected = true;
                    //    else
                    //        lstItem.Selected = false;
                    //    drpRoles.Items.Add(lstItem);
                    //}
                }
            }

        }

        protected void btnSave1_Click(object sender, EventArgs e)
        {
            //***EAC SAVE CURRENT RECORD
            if (Page.IsValid && _userid > 0)
            {
                //***EAC we only save ROLE for now ...
                CISUser c = new CISUser(this._userid, "dummy", "dummy", drpRoles.SelectedValue, "dummy", "dummy", "dummy", "dummy");

                if (PubEntAdmin.DAL.PE_DAL.SaveGuamUser(c))
                    lblMessage.Text = "Your changes have been saved";
                else
                    lblMessage.Text = "Error! There was a problem saving";
            }

        }

        protected void btnSave2_Click(object sender, EventArgs e)
        {
            btnSave1_Click(sender, e);
        }


    }
}
