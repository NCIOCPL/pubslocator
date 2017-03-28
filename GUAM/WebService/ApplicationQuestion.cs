using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace WebService
{
    [DataContract]
    public class ApplicationQuestion
    {
        [DataMember]
        public int QuestionID
        {
            get;
            set;
        }

        [DataMember]
        public string QuestionText
        {
            get;
            set;
        }

        public ApplicationQuestion()
        {
        }
    }
}