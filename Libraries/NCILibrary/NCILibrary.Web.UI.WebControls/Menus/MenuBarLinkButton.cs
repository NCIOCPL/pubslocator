using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NCI.Web.UI.WebControls.Menus
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:MenuBarLinkButton runat=server></{0}:MenuBarLinkButton>")]
    public class MenuBarLinkButton : MenuBarButton
    {
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string NavigateUrl
        {
            get
            {
                String s = (String)ViewState["NavigateUrl"];
                return ((s == null) ? String.Empty : s);
            }

            set
            {
                ViewState["NavigateUrl"] = value;
            }
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            output.AddAttribute(HtmlTextWriterAttribute.Href, NavigateUrl);
            output.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID+"_lnk");
            output.RenderBeginTag(HtmlTextWriterTag.A);

            output.RenderBeginTag(HtmlTextWriterTag.Span);
            output.Write(this.Text);
            output.RenderEndTag();
            output.RenderEndTag();
        }

    }
}
