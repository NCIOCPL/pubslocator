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

namespace PubEntAdmin.UserControl
{
    public partial class SpellCkr : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //this.Visible = false;
        }

        //public new bool Visible
        //{
        //    set
        //    {
        //        this.spellchrfieldset.Visible = value;
        //        //this.drpLanguage.Enabled = value;
        //        //if (!value)
        //        //    this.imgSpelChksrc.Attributes.Clear();
        //    }
        //    //get
        //    //{
        //    //    return this.spellchrfieldset.Visible;
        //    //}
        //}

        public HtmlImage RefImgSplChk()
        {
            return this.imgSpelChksrc;
        }

        public DropDownList LangSel()
        {
            return this.drpLanguage;
        }

        public string LangSelClientID
        {
            get
            {
                return this.drpLanguage.ClientID;
            }
        }
    }
}