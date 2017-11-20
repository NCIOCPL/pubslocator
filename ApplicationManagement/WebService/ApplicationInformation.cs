using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace WebService
{
    [DataContract]
    public class ApplicationInformation
    {
        [DataMember]
        public string[] Accounts
        {
            get;
            set;
        }

        [DataMember]
        public int ApplicationID
        {
            get;
            set;
        }

        [DataMember]
        public string ApplicationName
        {
            get;
            set;
        }

        [DataMember]
        public string ApplicationPassword
        {
            get;
            set;
        }

        [DataMember]
        public bool CheckPasswordAgainstWordlist
        {
            get;
            set;
        }

        [DataMember]
        public string DailyExpirationEmailSubject
        {
            get;
            set;
        }

        [DataMember]
        public string DailyExpirationEmailTemplate
        {
            get;
            set;
        }

        [DataMember]
        public string DailyInactivityEmailSubject
        {
            get;
            set;
        }

        [DataMember]
        public string DailyInactivityEmailTemplate
        {
            get;
            set;
        }

        [DataMember]
        public int InactivityDays
        {
            get;
            set;
        }

        [DataMember]
        public byte InactivityWarningWindow
        {
            get;
            set;
        }

        [DataMember]
        public int IPAttemptsBeforeLockout
        {
            get;
            set;
        }

        [DataMember]
        public string IPLockoutEmailRecipients
        {
            get;
            set;
        }

        [DataMember]
        public int IPLockoutWindow
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
        public byte NumberOfPasswordResetQuestions
        {
            get;
            set;
        }

        [DataMember]
        public string PasswordAllowedSpecialCharacters
        {
            get;
            set;
        }

        [DataMember]
        public bool PasswordAllowUsername
        {
            get;
            set;
        }

        [DataMember]
        public int PasswordExpirationDays
        {
            get;
            set;
        }

        [DataMember]
        public byte PasswordExpirationWarningWindow
        {
            get;
            set;
        }

        [DataMember]
        public int PasswordHistoryMinimumAgeDays
        {
            get;
            set;
        }

        [DataMember]
        public int PasswordHistoryMinimumIterations
        {
            get;
            set;
        }

        [DataMember]
        public byte PasswordMaxLength
        {
            get;
            set;
        }

        [DataMember]
        public byte PasswordMinLength
        {
            get;
            set;
        }

        [DataMember]
        public string ProjectAccountManagerPointOfContact
        {
            get;
            set;
        }

        [DataMember]
        public string ProjectManager
        {
            get;
            set;
        }

        [DataMember]
        public string ProjectPointOfContact
        {
            get;
            set;
        }

        [DataMember]
        public ApplicationQuestion[] Questions
        {
            get;
            set;
        }

        [DataMember]
        public Dictionary<int, string> Roles
        {
            get;
            set;
        }

        [DataMember]
        public bool SendDailyExpirationEmails
        {
            get;
            set;
        }

        [DataMember]
        public bool SendDailyInactivityEmails
        {
            get;
            set;
        }

        [DataMember]
        public string SendEmailsBCC
        {
            get;
            set;
        }

        [DataMember]
        public string SendEmailsFrom
        {
            get;
            set;
        }

        [DataMember]
        public int TemporaryPasswordExpirationHours
        {
            get;
            set;
        }

        [DataMember]
        public int TicketLifespan
        {
            get;
            set;
        }

        [DataMember]
        public Dictionary<int, string> TrustedApplications
        {
            get;
            set;
        }

        [DataMember]
        public bool UseCustomPasswordValidation
        {
            get;
            set;
        }

        [DataMember]
        public byte UserAttemptsBeforeLockout
        {
            get;
            set;
        }

        [DataMember]
        public int UserCount
        {
            get;
            set;
        }

        [DataMember]
        public string UserCreatedEmailRecipients
        {
            get;
            set;
        }

        [DataMember]
        public string UserCreateEmailSubject
        {
            get;
            set;
        }

        [DataMember]
        public string UserCreateEmailTemplate
        {
            get;
            set;
        }

        [DataMember]
        public string UserLockoutEmailRecipients
        {
            get;
            set;
        }

        [DataMember]
        public string UserLockoutEmailSubject
        {
            get;
            set;
        }

        [DataMember]
        public string UserLockoutEmailTemplate
        {
            get;
            set;
        }

        [DataMember]
        public int UserLockoutWindow
        {
            get;
            set;
        }

        [DataMember]
        public string UserPasswordResetEmailSubject
        {
            get;
            set;
        }

        [DataMember]
        public string UserPasswordResetEmailTemplate
        {
            get;
            set;
        }

        [DataMember]
        public string UserPasswordResetRecipients
        {
            get;
            set;
        }

        [DataMember]
        public string ValidUsernameExplanation
        {
            get;
            set;
        }

        [DataMember]
        public string ValidUsernameRegex
        {
            get;
            set;
        }

        public ApplicationInformation()
        {
        }
    }
}