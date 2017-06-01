using System.ComponentModel;
using System.Web.UI;

using NCI.Web.UI.WebControls.Infrastructure;

namespace NCI.Web.UI.WebControls
{
    /// <summary>
    /// Validated textbox with default of allowing anything except scripts.
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:NoScriptValidatedInputBox runat=\"server\"></{0}:NoScriptValidatedInputBox>")]
    public class NoScriptValidatedInputBox : ValidatedInputBox
    {
        public override string RegexValidationMessage
        {
            get
            {
                return "Invalid input: Script tags not allowed.";
            }
        }

        /// <summary>
        /// Returns the regular expression used to validate the input box.
        /// </summary>
        protected override string ValidationPattern
        {
            get
            {
                return ValidatedInputConfiguration.NoScriptValidationExpression;
            }
        }

        /// <summary>
        /// Provides access to an HTML-encoded version of the inner textbox's value.
        /// </summary>
        [Bindable(true)]
        public override string Text
        {
            get
            {
                return base.SanitizedText;
            }
            set
            {
                base.SanitizedText = value;
            }
        }
    }
}
