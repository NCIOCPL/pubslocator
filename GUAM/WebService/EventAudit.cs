using System;
using System.Runtime.Serialization;

namespace WebService
{
    [DataContract]
    public class EventAudit
    {
        [DataMember]
        public string Username;

        [DataMember]
        public string EventType;

        [DataMember]
        public DateTime Timestamp;

        [DataMember]
        public string IP;

        [DataMember]
        public string Comment;

        public EventAudit()
        {
        }
    }
}