using System;
using System.Collections.Generic;
using System.Text;

using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;


namespace NCI.Web.UI.WebControls.Infrastructure
{
    /// <summary>
    /// Abstract validator to not allow tags or script tags in textboxes.
    /// </summary>
    public abstract class TagsBaseValidator : BaseValidator
    {
        /// <summary>
        /// Returns the regular expression used for validation.
        /// </summary>
        public abstract string ValidationExpression { get; }


        /// <summary>
        /// Determines whether the value in the input control is valid.
        /// </summary>
        /// <returns></returns>
        protected override bool EvaluateIsValid()
        {
            string controlValidationValue = this.GetControlValidationValue(this.ControlToValidate);
            
            if (controlValidationValue == null)
                return true;
            if (controlValidationValue.Trim() == String.Empty)
                return true;

            Match match = Regex.Match(controlValidationValue, ValidationExpression);
            bool result = match.Success && match.Index == 0 && match.Length == controlValidationValue.Length;
            return result;
        }
    }
}