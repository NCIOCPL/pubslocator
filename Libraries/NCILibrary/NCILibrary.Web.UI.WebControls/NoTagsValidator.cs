using System;
using System.Collections.Generic;
using System.Text;

using System.Web.UI;
using NCI.Web.UI.WebControls.Infrastructure;

namespace NCI.Web.UI.WebControls
{
    /// <summary>
    /// Validator that does not allow tags in a textbox.
    /// </summary>
    [ToolboxData("<{0}:NoTagsValidator runat=\"server\"></{0}:NoTagsValidator>")]
    public class NoTagsValidator : TagsBaseValidator
    {
        /// <summary>
        /// Returns the regular expression used for validation.
        /// </summary>
        public override string ValidationExpression
        {
            get
            {
                return ValidatedInputConfiguration.NoTagsValidationExpression;
            }
        }
    }
}