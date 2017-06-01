using System.ComponentModel;
using System.Web.UI;

using NCI.Web.UI.WebControls.Infrastructure;

namespace NCI.Web.UI.WebControls
{
    /// <summary>
    /// Validated textbox with no tags.
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:NoTagsValidatedInputBox runat=server></{0}:NoTagsValidatedInputBox>")]
    public class NoTagsValidatedInputBox : ValidatedInputBox
    {
        public override string RegexValidationMessage
        {
            get
            {
                return "Invalid input: Tags not allowed.";
            }
        }

        /// <summary>
        /// Returns the regular expression used to validate the input box.
        /// </summary>
        protected override string ValidationPattern
        {
            get
            {
                return ValidatedInputConfiguration.NoTagsValidationExpression;
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
