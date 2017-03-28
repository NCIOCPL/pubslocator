using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace WebService
{
    [DataContract]
    public class User
    {
        [DataMember]
        public string Application
        {
            get;
            set;
        }

        [DataMember]
        public UserAttribute[] Attributes
        {
            get;
            set;
        }

        [DataMember]
        public string Email
        {
            get;
            set;
        }

        [DataMember]
        public bool IsActive
        {
            get;
            set;
        }

        [DataMember]
        public bool IsEnabled
        {
            get;
            set;
        }

        [DataMember]
        public bool IsLockedOut
        {
            get;
            set;
        }

        [DataMember]
        public bool IsPasswordExpired
        {
            get;
            set;
        }

        [DataMember]
        public bool MustChangePassword
        {
            get;
            set;
        }

        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public string NewUsername
        {
            get;
            set;
        }

        [DataMember]
        public string[] Roles
        {
            get;
            set;
        }

        [DataMember]
        public string Username
        {
            get;
            set;
        }

        public User()
        {
        }
    }
}