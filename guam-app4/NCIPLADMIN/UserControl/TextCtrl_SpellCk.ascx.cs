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
    public partial class TextCtrl_SpellCk : System.Web.UI.UserControl
    {
        protected string strWidth;
        protected string strHeight;
        protected bool blnEnable;
        protected string strTextMode;
        protected int intMaxLength;

        private bool blnTurnOffValidator = false;

        protected static readonly string strActiveMenuItem = "ActiveMenuItem";
        protected static readonly string strDefaultTextBoxHeight = "22px";
        protected static readonly string strSpellCheckBtnName = "btnspells.gif";

        protected const string strJScriptFormat =
            "<script language=\"javascript\" src=\"{0}\"></script>";

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.SpellCheckTB.Width = (this.Width);
            this.SpellCheckTB.Height = (this.Height);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.txtComment.TextMode == TextBoxMode.SingleLine)
                this.txtComment.Height = Unit.Parse(strDefaultTextBoxHeight);
            this.txtComment.Attributes.Add("onkeypress", "return taLimit(" + this.txtComment.MaxLength + ");");
            this.txtComment.Attributes.Add("onpaste", "return taCount(" + this.txtComment.MaxLength + ");");
            this.txtComment.Attributes.Add("onkeyup", "return taCount(" + this.txtComment.MaxLength + ");");
            this.txtComment.Attributes.Add("onkeydown", "return taCount(" + this.txtComment.MaxLength + ");");
            this.txtComment.Attributes.Add("onblur", "return taGone();");
            AssignComponentSrc();
           RegisterMonitoredChanges();
        }

        protected void RegisterMonitoredChanges()
        {
            PubEntAdminManager.MonitorChanges(this.Page, this.txtComment);
        }

        private void AssignComponentSrc()
        {
            JScriptSheet.Text = String.Format(strJScriptFormat, JScriptPath);
        }

        protected string JScriptPath
        {
            get
            {
                string strJSName;
                strJSName = Page.ResolveUrl("~/JS/charcount.js");
                return strJSName;
            }
        }

        public string Text
        {
            get { return this.txtComment.Text; }
            set { this.txtComment.Text = value; }
        }

        public bool Enabled
        {
            get
            {
                return this.blnEnable;
            }
            set
            {
                this.blnEnable = value;
                this.SpellCheckTB.Visible = value;
            }
        }

        public string Width
        {
            get { return this.strWidth; }
            set
            {
                this.strWidth = value;
                txtComment.Width = Unit.Pixel(Convert.ToInt32(Int32.Parse(value) * 0.90));
            }
        }

        public string Height
        {
            get { return this.strHeight; }
            set
            {
                this.strHeight = value;
            }
        }

        public string TextMode
        {

            get
            {
                return this.strTextMode;
            }
            set
            {
                this.strTextMode = value;
                switch (value)
                {
                    case "SingleLine":
                        txtComment.TextMode = TextBoxMode.SingleLine;
                        break;
                    case "MultiLine":
                        txtComment.TextMode = TextBoxMode.MultiLine;
                        break;
                    case "Password":
                        txtComment.TextMode = TextBoxMode.Password;
                        break;
                }
            }
        }

        public bool TurnOffValidator
        {
            set
            {
                this.blnTurnOffValidator = value;
                this.Required = !(value);
            }
            get { return this.blnTurnOffValidator; }
        }

        public int MaxLength
        {
            get { return this.intMaxLength; }
            set
            {
                this.intMaxLength = value;
                txtComment.MaxLength = value;
            }
        }

        public bool Required
        {
            get { return this.ReqFieldVal.Enabled; }
            set { this.ReqFieldVal.Enabled = value; }
        }

        public string CssClass
        {
            set { this.txtComment.CssClass = value; }
            get { return this.txtComment.CssClass; }
        }

        public string TextCtrl_SpellCheckClentID
        {
            get { return this.txtComment.ClientID; }
        }

        public TextBox TextCtrl_SpellCheckInnerTxtbox
        {
            get { return this.txtComment; }
        }

        public bool ReadOnly
        {
            set {

                TextBoxMode l_tbm = this.txtComment.TextMode;
                this.txtComment.ReadOnly = value;
                //this.txtComment.TextMode = l_tbm;
                this.txtComment.Attributes.CssStyle.Add("color", "#808080");

                if (value)
                {
                    this.txtComment.TextMode = TextBoxMode.MultiLine;
                }
                else
                {
                    this.txtComment.TextMode = l_tbm;
                }

            }
            get { return txtComment.ReadOnly; }
        }
    }
}