using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NCI.Web.UI.WebControls
{
    /// <summary>
    /// It is a compostie control having a third party editonpro embeded in it.
    /// It serves an WYSISYG HTML editor with powerful editing functions.
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:EditOnProControl runat=server></{0}:EditOnProControl>")]
    public class EditOnProControl : CompositeControl
    {
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]

        #region Fields
        private HiddenField _HtmlText = new HiddenField();
        private System.Web.UI.HtmlControls.HtmlGenericControl g = new System.Web.UI.HtmlControls.HtmlGenericControl();
        private string _editOnProBase = "/Resources/EditPro/eopro/";
        private string _jsFileLocation = null;
        private const string _jsFileName = "editonpro.js";
        private string _UIConfigFile = "uiconfig.xml";
        private string _configFile = "config.xml";
        private string _editOnProName = "MyEditor";
        //private int _editOnProWidth = 0;
        //private int _editOnProHeight = 0;        
        public EventHandler Click;
        #endregion

        /// <summary>
        /// Name for initilize an editonpro object
        /// </summary>
        public string EditOnProName
        {
            get { return _editOnProName; }
            set { _editOnProName = value; }
        }

        /// <summary>
        /// The path for javascript file
        /// </summary>
        public string JsFileLocation
        {
            get { return _jsFileLocation; }
            set { _jsFileLocation = value; }
        }

        /// <summary>
        /// Config file name
        /// </summary>
        public string ConfigFile
        {
            get { return _configFile; }
            set { _configFile = value; }
        }

        /// <summary>
        /// UI config file name
        /// </summary>
        public string UIConfigFile
        {
            get { return _UIConfigFile; }
            set { _UIConfigFile = value; }
        }

        /// <summary>
        /// Hidden field for move the HTML data from editonpro to database or from db to editonpro
        /// </summary>
        public string Text
        {
            get
            {
                String s = (String)_HtmlText.Value;
                return ((s == null) ? String.Empty : s);
            }

            set
            {
                _HtmlText.Value = value;
            }
        }

        public virtual void OnClick(EventArgs e)
        {
            if (Click != null)
            {
                Click(this, e);
            }

        }

        /// <summary>
        /// Create editoronpro control layout
        /// </summary>
        protected override void CreateChildControls()
        {

            _HtmlText.ID = "HTMLText";
            //_HtmlText.Value = "";
            this.Controls.Add(_HtmlText);
            this.Controls.Add(g);

        }

        /// <summary>
        /// Register the javascript block of Form.Onsubmit and Form.OnLoad
        /// Put the content from hiddenfield to editonpro in Form.Onload
        /// Put the contrent from editonpro to hiddenfield in Form.Onsubmit
        /// </summary>
        protected override void OnPreRender(EventArgs e)
        {
            _jsFileLocation = _editOnProBase + _jsFileName;
            Page.ClientScript.RegisterClientScriptInclude("EditProJS", _jsFileLocation);
            Page.ClientScript.RegisterOnSubmitStatement(typeof(Page), "scriptForm_onsubmit", "document.getElementById(\"" + _HtmlText.ClientID + "\").value = eop.getHTMLData();");
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "StartupScript", LoadDataFromHiddenField());
            g.InnerHtml = LoadApplet();
        }

        /// <summary>
        /// Javascript for Form.OnLoad and also initilize the instance of editonpro object
        /// </summary>
        private string LoadDataFromHiddenField()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\n\n");


            sb.Append("\r\n<script language=\"javascript\" type=\"text/javascript\">\r\n");
            //when load the page, put text into applet from hidden field.
            sb.Append("function scriptForm_loadData()\n");
            sb.Append("{\n");

            sb.Append("if(document.getElementById(\"" + _HtmlText.ClientID + "\").value!=\"\"){\n");
            sb.Append("eop.setHTMLData(document.getElementById(\"" + _HtmlText.ClientID + "\").value);\n");
            //sb.Append("  eop.setStyleSheet(document.SubmitContent.CSSText.value);\n");
            sb.Append("}else{\n");
            sb.Append("eop.setHTMLData(\"\");}\n");
            sb.Append("eop.pumpEvents();\n");
            sb.Append("}\n");

            sb.Append("eop = new editOnPro(" + this.Width.ToString() + ", " + this.Height.ToString() + ", '" + this.ClientID + "_" + _editOnProName + "' , '" + this.ClientID + "_myID', 'eop');\n");
            sb.Append("eop.setCodebase('" + _editOnProBase + "');\n");
            sb.Append("eop.setUIConfigURL('" + _UIConfigFile + "');\n");
            sb.Append("eop.setConfigURL('" + _configFile + "');\n");
            sb.Append("eop.setBase(document.URL);\n");
            sb.Append("eop.setLocaleCode(eop.getLocaleFromBrowser());\n");
            sb.Append("eop.setOnEditorLoaded('scriptForm_loadData');\n");
            sb.Append("</script>\n");


            return sb.ToString();
        }

        /// <summary>
        /// Create Content of GenericHtmlcontrol which is defined in the CreateChildClass
        /// The content is a line of javascript which calls for loading the editonpro.
        /// </summary>
        private string LoadApplet()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\n\n");
            //load the editor          
            sb.Append("<script language='javascript' type='text/javascript'> eop.loadEditor(); </script>\n");
            return sb.ToString();

        }
    }
}

