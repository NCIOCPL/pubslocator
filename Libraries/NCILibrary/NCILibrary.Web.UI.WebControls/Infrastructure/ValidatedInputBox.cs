using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NCI.Web.UI.WebControls.Infrastructure
{
    [ValidationProperty("Text")]
    public abstract class ValidatedInputBox : CompositeControl
    {
        // Default message.  Overridden by setting RegexValidationMessage.
        private const string DefaultValidationMessage = "Invalid input.";

        // Does the field default to being required?  Overriden by setting
        // the RequiredField property.
        private const bool RequireByDefault = false;
        private bool _requiredHasBeenSet = false;

        // The inner controls.
        private TextBox _textbox = new TextBox();
        private RegularExpressionValidator _regexValidator = new RegularExpressionValidator();
        private RequiredFieldValidator _reqFieldValidator = new RequiredFieldValidator();

        /// <summary>
        /// Provides access to the validator control's message.
        /// </summary>
        [Bindable(true)]
        [DefaultValue("")]
        public virtual String RegexValidationMessage
        {
            get
            {
                EnsureChildControls();
                return _regexValidator.Text;
            }

            set
            {
                EnsureChildControls();
                _regexValidator.Text = value;
            }
        }

        /// <summary>
        /// Gets the inner textbox's unique control ID.
        /// </summary>
        [Bindable(false)]
        public virtual String TextboxID
        {
            get
            {
                EnsureChildControls();
                return _textbox.UniqueID;
            }
        }

        /// <summary>
        /// Provides access to the inner textbox's value.
        /// Use the SanitizedText property for a safe HTML-encoded
        /// version of the text.
        /// </summary>
        [Bindable(true)]
        public virtual String Text
        {
            get
            {
                return TextValueInternal;
            }
            set
            {
                TextValueInternal = value;
            }
        }

        /// <summary>
        /// Provides access to an HTML-encoded version of the textbox's value.
        /// Use the Text property for a plaintext version of the value.
        /// </summary>
        /// <remarks> This property is meant to be used by inherting classes to 
        /// provide HTML-encoded access to the textbox value when plain text is not
        /// a safe option (e.g. When receiving user-input which is intended to be
        /// displayed on a web page).
        /// 
        /// Inheriting classes are responsible for guaranteeing that only Text
        /// or SanitizedText is used and not both.
        /// </remarks>
        protected string SanitizedText
        {
            get
            {
                return HtmlEncoder.HtmlEncode(TextValueInternal);
            }
            set
            {
                if (HtmlEncoder.IsHtmlEncoded(value))
                {
                    TextValueInternal = HtmlEncoder.HtmlDecode(value);
                }
                else
                {
                    TextValueInternal = value;
                }
            }
        }

        /// <summary>
        /// Infrastructure.  Get/Set the actual textbox control.
        /// </summary>
        private String TextValueInternal
        {
            get
            {
                EnsureChildControls();
                return _textbox.Text;
            }
            set
            {
                EnsureChildControls();
                _textbox.Text = value;
            }
        }


        /// <summary>
        /// Allows access to the maximum number of characters allowed in the textbox.
        /// </summary>
        [Bindable(true)]
        public virtual int MaxLength
        {
            get
            {
                EnsureChildControls();
                return _textbox.MaxLength;
            }
            set
            {
                EnsureChildControls();
                _textbox.MaxLength = value;
            }
        }

        /// <summary>
        /// Allows access to the TextMode of the textbox.
        /// </summary>
        [Bindable(true)]
        public virtual TextBoxMode TextMode
        {
            get
            {
                EnsureChildControls();
                return _textbox.TextMode;
            }
            set
            {
                EnsureChildControls();
                _textbox.TextMode = value;
            }
        }

        /// <summary>
        /// Allows access to the Enabled property of the RequiredFieldValidator
        /// </summary>
        [Bindable(true)]
        [DefaultValue("false")]
        public virtual bool RequiredField
        {
            /// Validators are enabled by default, so if we do nothing, fields using
            /// controls derived from ValidatedInputBox will always be required.
            /// What we do to get around this is use a flag value (_requiredHasBeenSet)
            /// to let us know whether this property has been set externally.  This allows
            /// us to provide a default value until we know one has been passed in, without
            /// having to worry about whether ViewState has been loaded yet.
            /// The sole assumption here is that derived classes aren't sensitive to
            /// whether the RequiredField is set prior CreateChildControls being called.
            /// 
            /// Incidentally, the DefaultValue attribute doesn't provide a default value for
            /// this property, instead, it tells Asp.Net what the default value is.
            /// Among other uses, this seems to mean that if a value is specified, and the
            /// value provided is the same as what's in the DefaultValue attribute, then the
            /// the RequiredField property won't be called.
            get
            {
                bool retval = _requiredHasBeenSet ? _reqFieldValidator.Enabled : RequireByDefault;
                return retval;
            }
            set
            {
                EnsureChildControls();
                _reqFieldValidator.Enabled = value;
                _requiredHasBeenSet = true;
            }
        }


        /// <summary>
        /// Allows access to the Text property of RequiredFieldValidator
        /// </summary>
        [Bindable(true)]
        public virtual string RequiredFieldMessage
        {
            get
            {
                EnsureChildControls();
                return _reqFieldValidator.Text;
            }
            set
            {
                EnsureChildControls();
                _reqFieldValidator.Text = value;
            }
        }

        /// <summary>
        /// Returns the regular expression used to validate the input box.
        /// </summary>
        protected virtual string ValidationPattern
        {
            get { return "[^<>]*"; }
        }

        protected override void OnInit(EventArgs e)
        {
            EnsureChildControls();

            // Do I actually need this?

            base.OnInit(e);
        }

        protected override void CreateChildControls()
        {
            // Set up text box
            _textbox.ID = "Textbox";
            this.Controls.Add(_textbox);

            // set up the validator
            _regexValidator.ControlToValidate = _textbox.ID;
            _regexValidator.ValidationExpression = ValidationPattern;
            _regexValidator.Text = DefaultValidationMessage; 
            this.Controls.Add(_regexValidator);

            // See the RequiredField property for an explanation of
            // how we know whether to enable/disable this validator.
            _reqFieldValidator.ControlToValidate = _textbox.ID;
            _reqFieldValidator.Text = DefaultValidationMessage;
            _reqFieldValidator.Enabled = RequiredField;
            this.Controls.Add(_reqFieldValidator);

            base.CreateChildControls();
        }
    }
}