using System.ComponentModel;
using System.Web.UI;

using NCI.Web.UI.WebControls.Infrastructure;

namespace NCI.Web.UI.WebControls
{
    /// <summary>
    /// Validated textbox with default of no validation expression.
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:HtmlValidatedInputBox runat=server></{0}:HtmlValidatedInputBox>")]
    public class HtmlValidatedInputBox : ValidatedInputBox
    {
        /// <summary>
        /// Returns the regular expression used to validate the input box.
        /// </summary>
        protected override string ValidationPattern
        {
            get
            {
                return ValidatedInputConfiguration.HtmlValidationExpression;
            }
        }
    }
}
