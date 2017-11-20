using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Xml;
using System.Collections;

namespace NCI.Web.UI.WebControls.Infrastructure
{
    public class ValidatedInputConfiguration : ConfigurationSection
    {
        const string NoTagsDefault = "^(?!.*[<>]).*$";  //"[^<>]*";
        const string NoScriptDefault = "^(?!.*</?[sS][cC][rR][iI][pP][tT]).*$";
        const string HtmlDefault = ".*";
        const string UserExpressionDefault = "[a-zA-Z0-9.]*";

        // (Mostly) RFC 2822 compliant, from http://regexlib.com/(A(W3kNi4pFFhlG18xsn5SToQaHTmv69BRJxCoPiWfzEloliI2huj7_nfxo4NV-9nbihd5T8wRlREesre8lQWpGJLC3FaK3JS-kbe7Q95oN-BFNgQGl9I661RVHSWVHTKlXWLIzOaemn7zMqjixIx4fVsmGRWqzt0WIH0N9yXHy78ZGie4mfaZa0cT21iQjSFgw0))/UserPatterns.aspx?authorId=9b7a4540-cf6a-4ae2-a883-5f0e6c6cb76c
        // Note: contains quotation marks escaped as "".
        const string EmailDefault = @"^((?>[a-zA-Z\d!#$%&'*+\-/=?^_`{|}~]+\x20*|""((?=[\x01-\x7f])[^""\\]|\\[\x01-\x7f])*""\x20*)*(?<angle><))?((?!\.)(?>\.?[a-zA-Z\d!#$%&'*+\-/=?^_`{|}~]+)+|""((?=[\x01-\x7f])[^""\\]|\\[\x01-\x7f])*"")@(((?!-)[a-zA-Z\d\-]+(?<!-)\.)+[a-zA-Z]{2,}|\[(((?(?<!\[)\.)(25[0-5]|2[0-4]\d|[01]?\d?\d)){4}|[a-zA-Z\d\-]*[a-zA-Z\d]:((?=[\x01-\x7f])[^\\\[\]]|\\[\x01-\x7f])+)\])(?(angle)>)$";

        /// <summary>
        /// [Internal use]
        /// </summary>
        [ConfigurationProperty("noTagsValidationExpression", IsRequired = false)]
        public ConfigValue NoTagsExpression
        {
            get
            {
                return (ConfigValue)base["noTagsValidationExpression"];
            }
        }

        /// <summary>
        /// [Internal use]
        /// </summary>
        [ConfigurationProperty("noScriptValidationExpression", IsRequired = false)]
        public ConfigValue NoScriptExpression
        {
            get
            {
                return (ConfigValue)base["noScriptValidationExpression"];
            }
        }

        /// <summary>
        /// [Internal use]
        /// </summary>
        [ConfigurationProperty("htmlValidationExpression", IsRequired = false)]
        public ConfigValue HtmlExpression
        {
            get
            {
                return (ConfigValue)base["htmlValidationExpression"];
            }
        }

        /// <summary>
        /// [Internal use]
        /// </summary>
        [ConfigurationProperty("usernameValidationExpression", IsRequired = false)]
        public ConfigValue UsernameExpression
        {
            get
            {
                return (ConfigValue)base["usernameValidationExpression"];
            }
        }

        /// <summary>
        /// [Internal use]
        /// </summary>
        [ConfigurationProperty("emailValidationExpression", IsRequired = false)]
        public ConfigValue EmailExpression
        {
            get
            {
                return (ConfigValue)base["emailValidationExpression"];
            }
        }

        /// <summary>
        /// Helper property to retrieve an instance of ValidatedInputConfiguration. 
        /// </summary>
        private static ValidatedInputConfiguration Instance
        {
            get
            {
                return (ValidatedInputConfiguration)System.Configuration.ConfigurationManager.GetSection("validatedInputBoxConfig");
            }
        }

        /// <summary>
        /// Regular expression to eliminate all tags.  Used with NoTagsValidatedInputBox
        /// </summary>
        public static String NoTagsValidationExpression
        {
            get
            {
                ValidatedInputConfiguration config = Instance;
                String retval;
                
                if (config != null && config.NoTagsExpression.ElementInformation.IsPresent)
                    retval = config.NoTagsExpression.Value;
                else 
                    retval = NoTagsDefault;
 
                return retval;
            }
        }

        /// <summary>
        /// Regular expression to eliminate only script tags.  Used with NoScriptValidatedInputBox
        /// </summary>
        public static String NoScriptValidationExpression
        {
            get
            {
                ValidatedInputConfiguration config = Instance;
                String retval;

                if (config != null && config.NoScriptExpression.ElementInformation.IsPresent)
                    retval = config.NoScriptExpression.Value;
                else
                    retval = NoScriptDefault;

                return retval;
            }
        }

        /// <summary>
        /// Regular expression to eliminate only script tags.  Used with HtmlValidatedInputBox
        /// </summary>
        public static String HtmlValidationExpression
        {
            get
            {
                ValidatedInputConfiguration config = Instance;
                String retval;

                if (config != null && config.HtmlExpression.ElementInformation.IsPresent)
                    retval = config.HtmlExpression.Value;
                else
                    retval = HtmlDefault;

                return retval;
            }
        }

        /// <summary>
        /// Regular expression for validating usernames.
        /// </summary>
        public static String UsernameValidationExpression
        {
            get
            {
                ValidatedInputConfiguration config = Instance;
                String retval;

                if (config != null && config.UsernameExpression.ElementInformation.IsPresent)
                    retval = config.UsernameExpression.Value;
                else
                    retval = UserExpressionDefault;

                return retval;
            }
        }

        /// <summary>
        /// Regular expression for validating email addresses.
        /// </summary>
        public static String EmailValidationExpression
        {
            get
            {
                ValidatedInputConfiguration config = Instance;
                String retval;

                if (config != null && config.EmailExpression.ElementInformation.IsPresent)
                    retval = config.EmailExpression.Value;
                else
                    retval = EmailDefault;

                return retval;
            }
        }
    }
}
