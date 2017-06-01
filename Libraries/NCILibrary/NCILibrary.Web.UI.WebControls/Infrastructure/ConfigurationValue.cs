using System;
using System.Collections.Generic;
using System.Configuration;

namespace NCI.Web.UI.WebControls.Infrastructure
{
    public class ConfigValue : ConfigurationElement
    {
        [ConfigurationProperty("value", IsRequired = true)]
        public String Value
        {
            get { return (String)this["value"]; }
        }
    }
}
