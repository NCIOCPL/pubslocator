using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WebService
{
    [DataContract]
    [KnownType(typeof(Dictionary<int, string>))]
    [KnownType(typeof(Dictionary<string, string>))]
    [KnownType(typeof(string[]))]
    [KnownType(typeof(ApplicationQuestion))]
    [KnownType(typeof(ApplicationQuestion[]))]
    [KnownType(typeof(ErrorReturnObject))]
    [KnownType(typeof(EventAudit))]
    [KnownType(typeof(EventAudit[]))]
    [KnownType(typeof(SuccessReturnObject))]
    [KnownType(typeof(User))]
    [KnownType(typeof(User[]))]
    [KnownType(typeof(UserAttribute))]
    [KnownType(typeof(UserAttribute[]))]
    [KnownType(typeof(UserQuestion))]
    [KnownType(typeof(UserQuestion[]))]
    [KnownType(typeof(VersionInformation))]
    public class ReturnObject
    {
        [DataMember]
        public int ReturnCode;

        [DataMember]
        public string DefaultErrorMessage;

        [DataMember]
        public object ReturnValue;

        public ReturnObject(int p_ReturnCode, string p_DefaultErrorMessage, object p_ReturnValue)
        {
        }

        public ReturnObject()
        {
        }
    }
}