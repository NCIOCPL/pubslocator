using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Exhibit.BLL;
using PubEnt.DAL;

namespace PubEnt
{
    public partial class default2 : System.Web.UI.Page
    {
        public string kioskparams = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //-- Retrive list of conference
                this.BindOptions();

                //*** Display the values of the timeouts for debugging purposes only
                int temp;

                temp = DAL2.DAL.GetTimeout(1, 1);
                Label2.Text = temp.ToString();

                temp = DAL2.DAL.GetTimeout(1, 2);
                Label3.Text = temp.ToString();

                temp = DAL2.DAL.GetTimeout(1, 3);
                Label4.Text = temp.ToString();
            }

        }

        protected void BindOptions()
        {
            this.ddlConfName.DataSource = Conf.GetAllConf();
            this.ddlConfName.DataTextField = "ConfName";
            this.ddlConfName.DataValueField = "ConfID";
            this.ddlConfName.DataBind();
        }

        /*protected void btnContinue_Click(object sender, EventArgs e)
        {
            String strKioskURL = "attract.aspx?ConfID=" + this.ddlConfName.SelectedValue;
            Response.Redirect(strKioskURL, true);
        }*/
    }
}
