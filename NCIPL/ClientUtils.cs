using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PubEnt
{
    public class ClientUtils
    {


        public bool ValidateUser(string username, string password)
        {
            bool valid = false;
            Dictionary<string, string> logins = new Dictionary<string,string>();
            logins.Add("testuser1@pubs.cancer.gov", "!Batman11");
            logins.Add("testuser2@pubs.cancer.gov", "!Batman12");
            logins.Add("testuser3@pubs.cancer.gov", "!Batman13");
            logins.Add("testuser4@pubs.cancer.gov", "!Batman14");

            foreach(KeyValuePair<string, string> login in logins)
            {
                if(login.Key == username && login.Value == password)
                {
                    return true;
                }
            }
            return valid;
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

        public int GetValidationFailureReason(string username)
        {
            int valCode = 0;
            if(username == "testuser4@pubs.cancer.gov")
            {
                valCode = 106;
            }
            return valCode;
        }

    }
}