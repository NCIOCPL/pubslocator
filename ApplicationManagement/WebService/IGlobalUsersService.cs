using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace WebService
{
    [ServiceContract]
    public interface IGlobalUsersService
    {
        [OperationContract]
        ReturnObject AddRole(string RoleName);

        [OperationContract]
        ReturnObject AddUser(string Username);

        [OperationContract]
        ReturnObject AddUsersToRoles(string[] Usernames, string[] RoleNames);

        [OperationContract]
        ReturnObject AddUserToRole(string Username, string RoleName);

        [OperationContract]
        ReturnObject AssignPassword(string Username, string NewPassword);

        [OperationContract]
        ReturnObject ChangePassword(string Username, string CurrentPassword, string NewPassword);

        [OperationContract]
        ReturnObject CreateApplication(ApplicationInformation ai);

        [OperationContract]
        ReturnObject CreateUserAndPassword(string Username, string password);

        [OperationContract]
        ReturnObject CreateUserTicket(string username);

        [OperationContract]
        ReturnObject DeleteApplication(string name);

        [OperationContract]
        ReturnObject DeleteRole(string RoleName, bool HaltIfUsers);

        [OperationContract]
        ReturnObject DeleteUser(string Username);

        [OperationContract]
        ReturnObject EnableUser(string Username);

        [OperationContract]
        ReturnObject ExistsRole(string RoleName);

        [OperationContract]
        ReturnObject ExistsUsername(string Username);

        [OperationContract]
        ReturnObject FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords);

        [OperationContract]
        ReturnObject FindUsersByMetaData(string MetaDataKey, string MetaDataValue);

        [OperationContract]
        ReturnObject FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords);

        [OperationContract]
        ReturnObject FindUsersInRole(string roleName, string usernameToMatch);

        [OperationContract]
        ReturnObject GeneratePassword(string Username);

        [OperationContract]
        ReturnObject GetAllRoles();

        [OperationContract]
        ReturnObject GetAllUsers(out int totalRecords, int pageIndex, int pageSize);

        [OperationContract]
        ApplicationInformation GetApplicationInformation();

        [OperationContract]
        List<ApplicationInformation> GetApplicationInformationList();

        [OperationContract]
        ReturnObject GetApplicationLogs(out int total_logs, DateTime start, DateTime end, int? page, int? size);

        [OperationContract]
        ReturnObject GetApplicationLogsForUser(out int total_logs, string username, int? page, int? size);

        [OperationContract]
        ReturnObject GetAvailableQuestions();

        [OperationContract]
        ReturnObject GetInactivityDaysLeft(string Username);

        [OperationContract]
        ReturnObject GetMustChangePasswordFlag(string Username);

        [OperationContract]
        ReturnObject GetPasswordExpirationDaysLeft(string Username);

        [OperationContract]
        ReturnObject GetPasswordPolicyStatement();

        [OperationContract]
        ReturnObject GetRolesForUser(string Username);

        [OperationContract]
        ReturnObject GetUser(string Username);

        [OperationContract]
        ReturnObject GetUserMetaData(string Username, string MetaDataKey);

        [OperationContract]
        ReturnObject GetUserMetaDataAll(string Username);

        [OperationContract]
        ReturnObject GetUserQuestions(string Username);

        [OperationContract]
        ReturnObject GetUsersByMetaData(string key, List<string> values);

        [OperationContract]
        ReturnObject GetUsersInRole(string roleName);

        [OperationContract]
        ReturnObject GetValidationFailureReason(string Username);

        [OperationContract]
        ReturnObject GetVersionInformation();

        [OperationContract]
        ReturnObject IsIPLockedOut();

        [OperationContract]
        ReturnObject IsPasswordExpired(string Username);

        [OperationContract]
        ReturnObject IsUserInRole(string Username, string RoleName);

        [OperationContract]
        ReturnObject IsValidPassword(string Username, string Password);

        [OperationContract]
        ReturnObject IsValidUsername(string Username);

        [OperationContract]
        ReturnObject RemoveUserFromRole(string Username, string RoleName);

        [OperationContract]
        ReturnObject RemoveUsersFromRoles(string[] Usernames, string[] RoleNames);

        [OperationContract]
        ReturnObject ResetPassword(string Username, UserQuestion[] answers);

        [OperationContract]
        ReturnObject ResetPasswordAndSendEmail(string Username, string FromEmail, string Subject, string EmailTemplate, string[] BCC);

        [OperationContract]
        ReturnObject SearchByMetaData(Dictionary<string, string> MetaData, int pageIndex, int pageSize, out int totalRecords);

        [OperationContract]
        ReturnObject SendNewUserEmail(string Username);

        [OperationContract]
        ReturnObject SetAvailableQuestions(ApplicationQuestion[] Questions);

        [OperationContract]
        ReturnObject SetMustChangePasswordFlag(string Username, bool Flag);

        [OperationContract]
        ReturnObject SetUserMetaData(string Username, string MetaDataKey, string MetaDataValue);

        [OperationContract]
        ReturnObject SetUserMetaDataAll(string Username, Dictionary<string, string> MetaData);

        [OperationContract]
        ReturnObject SetUserQuestionsAndAnswers(string Username, UserQuestion[] question);

        [OperationContract]
        ReturnObject ShouldShowPasswordExpirationWarning(string Username);

        [OperationContract]
        ReturnObject UpdateApplication(ApplicationInformation ai);

        [OperationContract]
        ReturnObject UpdateUser(User user);

        [OperationContract]
        ReturnObject ValidateUser(string Username, string Password);

        [OperationContract]
        ReturnObject ValidateUserAnswers(string Username, UserQuestion[] question);

        [OperationContract]
        ReturnObject ValidateUserTicket(string username, string ticket);
    }
}