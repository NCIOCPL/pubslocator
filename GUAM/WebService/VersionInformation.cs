using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace WebService
{
    [DataContract]
    public class VersionInformation
    {
        [DataMember]
        public string ActualWebServicesVersion
        {
            get;
            set;
        }

        [DataMember]
        public string DatabaseExpectedWebServicesVersion
        {
            get;
            set;
        }

        [DataMember]
        public int DatabaseSchemaVersion
        {
            get;
            set;
        }

        public VersionInformation()
        {
        }
    }
}