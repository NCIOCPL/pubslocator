using System;
using System.Collections.Generic;
using System.Text;

using System.Web.UI;
using NCI.Web.UI.WebControls.Infrastructure;

namespace NCI.Web.UI.WebControls
{
    /// <summary>
    /// Validator that does not allow script tags in a textbox.
    /// </summary>
    [ToolboxData("<{0}:NoScriptValidator runat=\"server\"></{0}:NoScriptValidator>")]
    public class NoScriptValidator : TagsBaseValidator
    {
        /// <summary>
        /// Returns the regular expression used for validation..
        /// </summary>
        public override string ValidationExpression
        {
            get
            {
                return ValidatedInputConfiguration.NoScriptValidationExpression;
            }
        }
    }
}