using System;
using System.Runtime.Serialization;

namespace WebService
{
    [DataContract]
    public class UserAttribute
    {
        [DataMember]
        public string Key;

        [DataMember]
        public string Value;

        public UserAttribute()
        {
        }
    }
}