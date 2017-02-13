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

        public int GetValidationFailureReason(string username, string password)
        {
            int failCode = 0;
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
                failCode = 106;
            }
            conn.Close();

            return failCode;
        }

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