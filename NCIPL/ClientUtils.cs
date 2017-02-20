using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PubEnt
{
    public class ClientUtils
    {
        private static String connStr = ConfigurationManager.ConnectionStrings["globalusers"].ConnectionString;

        //Function to get random number
        private static readonly Random getRandom = new Random();
        private static readonly object syncLock = new object();
        private static int GetRandomNumber(int min, int max)
        {
            lock (syncLock)
            { // synchronize
                return getRandom.Next(min, max);
            }
        }

        // Get list of security questions for the NCIPL application
        public Dictionary<String, String> GetAppInfoQuestions()
        {
            Dictionary<string, string> allQuestions = new Dictionary<string, string>();
            SqlConnection conn = new SqlConnection(connStr);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            // Use '3' for application ID 
            cmd.CommandText = @"select QuestionID, QuestionText from Questions where ApplicationID = 3";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = conn;

            conn.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                allQuestions.Add(reader.GetInt32(0).ToString(), reader.GetString(1));
            }
            conn.Close();

            return allQuestions;
        }

        // Get ID for security question
        protected int GetQuestionId(string question)
        {
            int qid = 0;
            SqlConnection conn = new SqlConnection(connStr);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            // Use '3' for application ID
            cmd.CommandText = @"SELECT QuestionID FROM Questions WHERE ApplicationID = 3 AND QuestionText = '" + question + "'";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = conn;

            conn.Open();
            reader = cmd.ExecuteReader();
            while(reader.Read())
            {
                qid = reader.GetInt32(0);
            }
            conn.Close();
            return qid; 
        }

        // Check if username exists
        public bool ExistsUsername(string username)
        {
            bool exists = false;
            SqlConnection conn = new SqlConnection(connStr);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = @"select username from DionDummyUsers where username = '" + username + @"'";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = conn;

            conn.Open();
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                exists = true;
            }
            conn.Close();
            return exists;
        }


        // Add a new user; returns int based on result of add
        public int AddUser(string username)
        {
            int success = 0;
            int fail = 1;

            int uid = GetRandomNumber(1, 9999);
            SqlConnection conn = new SqlConnection(connStr);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = @"INSERT INTO DionDummyUsers (id,username,isDisabled) VALUES (" + uid.ToString() + @",'" + username + @"',0);";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = conn;

            try
            {
                conn.Open();
                reader = cmd.ExecuteReader();
                conn.Close();
                return success;
            }
            catch (Exception ex)
            {
                return fail;
            }

        }

        // Create user metadata
        public void SetUserMetaData(string username)
        {
            SqlConnection conn = new SqlConnection(connStr);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = @"UPDATE DionDummyUsers SET email='" + username + @"' WHERE username ='" + username + "'";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = conn;

            try
            {
                conn.Open();
                reader = cmd.ExecuteReader();
                conn.Close();
            }
            catch (Exception ex)
            {
            }

        }

        // Randomly generate password
        public String GeneratePassword (string username)
        {
            string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            string rand = new string(Enumerable.Repeat(chars, 7)
              .Select(s => s[getRandom.Next(s.Length)]).ToArray());

            SqlConnection conn = new SqlConnection(connStr);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = @"UPDATE DionDummyUsers SET password='" + rand + @"' WHERE username ='" + username + "'";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = conn;

            try
            {
                conn.Open();
                reader = cmd.ExecuteReader();
                conn.Close();
                return rand;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        // Add user roles
        public void AddUserToRole(string username, string[] roles)
        {
            string allRoles = string.Join(",", roles);
            SqlConnection conn = new SqlConnection(connStr);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = @"UPDATE DionDummyUsers SET roles='" + allRoles + @"' WHERE username ='" + username + "'";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = conn;

            conn.Open();
            reader = cmd.ExecuteReader();
            conn.Close();
        }

        // Add user security questions + answers      
        public void SetUserQuestionsAndAnswers(string username, KeyValuePair<string, string> qnas)
        {
            SqlConnection conn = new SqlConnection(connStr);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = @"UPDATE DionDummyUsers SET secQuestion='" + qnas.Key + @"',secAnswer='" + qnas.Value + @"' WHERE username ='" + username + "'";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = conn;

            conn.Open();
            reader = cmd.ExecuteReader();
            conn.Close();
        }

        // Change password    
        public int ChangePassword(string username, string oldPassword, string newPassword)
        {
            int success = 0;
            int oldMatchesNew = 1;
            int notvalid = 2;
            int sqlError = 3;

            bool isUserValid = ValidateUser(username, oldPassword);

            if(oldPassword == newPassword)
            {
                return oldMatchesNew;
            }

            SqlConnection conn = new SqlConnection(connStr);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = @"UPDATE DionDummyUsers 
                                SET password='" + newPassword +
                                @"',isPasswordBad = 0 WHERE username ='" + username + "'";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = conn;

            try
            {
                if (isUserValid)
                {
                    conn.Open();
                    reader = cmd.ExecuteReader();
                    conn.Close();
                    return success;
                }
                else
                {
                    return notvalid;
                }
            }
            catch
            {
                return sqlError;
            }
        }


        // Check if user name and password match and that account is not disabled
        public bool ValidateUser(string username, string password)
        {
            bool isValid = false;
            SqlConnection conn = new SqlConnection(connStr);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = @"select id from DionDummyUsers where username = '" + username + @"' AND password = '" + password + @"' AND isDisabled != 1";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = conn;

            conn.Open();
            reader = cmd.ExecuteReader();
            if(reader.HasRows)
            {
                isValid = true;
            }
            conn.Close();
            return isValid;
        }

        // If validation failed, return a failure code
        public int GetValidationFailureReason(string username, string password)
        {
            int returnCode = 0;
            int failCode = 106;

            SqlConnection conn = new SqlConnection(connStr);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = @"select id from DionDummyUsers where username = '" + username + @"' AND password = '" + password + @"' AND isDisabled = 1";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = conn;

            conn.Open();
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                returnCode = failCode;
            }
            conn.Close();

            return returnCode;
        }

        // Get the security question & question ID for a given user
        public KeyValuePair<string, string> GetUserQuestions(string username)
        {

            KeyValuePair<string, string> question = new KeyValuePair<string, string>("", "");
            SqlConnection conn = new SqlConnection(connStr);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = @"SELECT secQuestion FROM DionDummyUsers WHERE username = '" + username + "'";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = conn;

            try
            {
                conn.Open();
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string questionValue = reader.GetString(0);
                        string questionId = GetQuestionId(questionValue).ToString();
                        question = new KeyValuePair<string, string>(questionId, questionValue);
                    }
                }
                conn.Close();
                return question;
            }
            catch(Exception ex)
            {
                return question;
            }
        }

        // Reset user password and return status code
        public int ResetPassword(string username, string userAnswer, int userQuestionID)
        {
            int success = 0;
            int fail = 1;
            int qid = userQuestionID; // don't know what we're doing with this yet

            SqlConnection conn = new SqlConnection(connStr);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = @"SELECT id from DionDummyUsers where username = '" + username + @"' AND secAnswer = '" + userAnswer + "'";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = conn;
             
            try
            {
                conn.Open();
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    conn.Close();
                    GeneratePassword(username);
                    return success;
                }
                else
                {
                    conn.Close();
                    return fail;
                }
            }
            catch (Exception x)
            {
                return fail;
            }
        }

        // Check for "isPasswordBad" flag
        public bool GetMustChangePasswordFlag(string username)
        {
            bool isPwBad = false;
            SqlConnection conn = new SqlConnection(connStr);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = @"select id from DionDummyUsers where username = '" + username + @"' AND isPasswordBad = 1";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = conn;

            conn.Open();
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                isPwBad = true;
            }
            conn.Close();
            return isPwBad;
        }

        // Get user's roles
        public String[] GetRolesForUser(string username)
        {
            List<string> roles = new List<string>();
            if (!string.IsNullOrEmpty(username))
            {
                roles.Add("NCIPL_PUBLIC");
            }
            return roles.ToArray();
        }



    }
}