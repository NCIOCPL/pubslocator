using System;

namespace WebService
{
    [Serializable]
    public class ErrorReturnObject : ReturnObject
    {
        public ErrorReturnObject()
        {
            this.ReturnCode = 1;
            this.DefaultErrorMessage = "General error.";
        }

        public ErrorReturnObject(ErrorReturnObject.Error ee)
        {
            this.ReturnCode = (int)ee;
            switch (ee)
            {
                case ErrorReturnObject.Error.General:
                    {
                        this.DefaultErrorMessage = "General error.";
                        break;
                    }
                case ErrorReturnObject.Error.ApplicationDoesNotExist:
                    {
                        this.DefaultErrorMessage = "Application does not exist.";
                        break;
                    }
                case ErrorReturnObject.Error.UsernameNotFound:
                    {
                        this.DefaultErrorMessage = "Username not found.";
                        break;
                    }
                case ErrorReturnObject.Error.RoleNameNotFound:
                    {
                        this.DefaultErrorMessage = "Role name not found.";
                        break;
                    }
            }
        }

        public ErrorReturnObject(int p_ReturnCode, string p_DefaultErrorMessage)
        {
            this.ReturnCode = p_ReturnCode;
            this.DefaultErrorMessage = p_DefaultErrorMessage;
        }

        public enum Error
        {
            General = 1,
            ApplicationDoesNotExist = 2,
            UsernameNotFound = 3,
            RoleNameNotFound = 4
        }
    }
}