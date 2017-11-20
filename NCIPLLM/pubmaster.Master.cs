using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PubEnt
{
    public partial class pubmaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {


        }
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            //Label1.Visible = false;
            LinkButton1.Visible = false;
            LinkButton2.Visible = false;
            lnkEditAcc.Visible = false;
            if (GlobalUtils.Utils.isLoggedin())
            {
                //Label1.Visible = true;
                LinkButton2.Visible = true;
                //Label1.Text = GlobalUtils.Utils.LoggedinUser();
                lnkEditAcc.Visible = true;
                lnkEditAcc.Text = GlobalUtils.Utils.LoggedinUser();
               // this.LogoImage1Visible = true; //NCIPL_CC
                //this.LogoImage2Visible = false; //NCIPL_CC
            }
            else
            {
                LinkButton1.Visible = true;
              //  this.LogoImage1Visible = false; //NCIPL_CC
             //   this.LogoImage2Visible = true; //NCIPL_CC
              //  this.lnkHome.NavigateUrl = "~/login.aspx"; //NCIPL_CC
              //  this.lnkHome.Text = "Login"; //NCIPL_CC
            }
            base.Render(writer);
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

        public bool TopRightPanelVisible
        {
            get { return this.pnlTopRight.Visible; }
            set { this.pnlTopRight.Visible = value; }
        }

        //public bool LogoImageVisible
        //{
        //    get { return this.divLogo.Visible; }
        //    set { this.divLogo.Visible = value; }
        //}

        //public bool LoginLinkBtnVisible
        //{
        //    get { return this.LinkButton1.Visible;}
        //    set {this.LinkButton1.Visible = value;}
        //}

        //public bool LogoImage1Visible
       // {
       //     get { return this.logoImage.Visible; }
       //     set { this.logoImage.Visible = value; }
       // }
       // public bool LogoImage2Visible
       // {
       //     get { return this.logoImage2.Visible; }
       //     set { this.logoImage2.Visible = value; }
       // }
       
        protected void LinkButton1_Click(object sender, EventArgs e)
        {
           
        }

        protected void LinkButton2_Click(object sender, EventArgs e)
        {            
            PubEnt.GlobalUtils.Utils.LogoutCurrentUser();
            Response.Redirect("~/default.aspx",true);
        }
    }
}
