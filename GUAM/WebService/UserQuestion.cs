using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace WebService
{
    [DataContract]
    public class UserQuestion
    {
        [DataMember]
        public string Answer
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

        [DataMember]
        public int UserID
        {
            get;
            set;
        }

        [DataMember]
        public int UserQuestionID
        {
            get;
            set;
        }

        public UserQuestion()
        {
        }
    }
}