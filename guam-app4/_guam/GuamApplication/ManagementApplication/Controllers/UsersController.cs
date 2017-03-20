using Aspensys.GlobalUsers.WebServiceClient;
using Aspensys.GlobalUsers.WebServiceClient.UserService;
using ManagementApplication;
using MvcContrib.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace ManagementApplication.Controllers
{
    [HandleError]
    public class UsersController : ManagementController
    {
        public UsersController()
        {
        }

        [AuthenticatedAuthorize]
        public ActionResult Add(string id)
        {
            User user = new User();
            user.Application = id;
            user.IsActive = true;
            user.IsEnabled = true;
            this.UserEditor(user, true);
            return base.View();
        }

        [AuthenticatedAuthorize]
        [HttpPost]
        public ActionResult Add(User u)
        {
            string str;
            string str1;
            ActionResult actionResult;
            ReturnObject returnObject = null;
            //u.Username(u.get_NewUsername());
            //u.set_NewUsername(string.Empty);
            UsingExtension.Using<UserServiceClient>(UserService.GetManagementClient(u.Application), (UserServiceClient client) => returnObject = client.AddUser(u.Username));
            if (returnObject.ReturnCode != 0)
            {
                this.UserEditor(u, true, "Username", returnObject.DefaultErrorMessage);
                actionResult = base.View();
            }
            else if (!this.ResetPassword(u.Username, u.Application, out str1, out str))
            {
                this.UserEditor(u, true, "", returnObject.DefaultErrorMessage);
                actionResult = base.View();
            }
            else
            {
                base.ViewData["password"] = str1;
                base.ViewData.Model = u;
                UsingExtension.Using<UserServiceClient>(UserService.GetManagementClient(u.Application), (UserServiceClient client) => returnObject = client.UpdateUser(u));
                if (returnObject.ReturnCode == 0)
                {
                    actionResult = base.View("Password");
                }
                else
                {
                    this.UserEditor(u, true, "", returnObject.DefaultErrorMessage);
                    actionResult = base.View();
                }
            }
            return actionResult;
        }

        [AuthenticatedAuthorize]
        public ActionResult Delete(string id, string application)
        {
            ReturnObject returnObject = null;
            UsingExtension.Using<UserServiceClient>(UserService.GetManagementClient(application), (UserServiceClient client) => returnObject = client.DeleteUser(id));
            if (returnObject.ReturnCode != 0)
            {
                base.ViewData.ModelState.AddModelError("", returnObject.DefaultErrorMessage);
            }
            ActionResult action = base.RedirectToAction("Index", new { id = application });
            return action;
        }

        [AuthenticatedAuthorize]
        public ActionResult Edit(string id, string application)
        {
            ReturnObject user = null;
            UsingExtension.Using<UserServiceClient>(UserService.GetManagementClient(application), (UserServiceClient client) => user = client.GetUser(id));
            if (user.ReturnCode == 0)
            {
                base.ViewData.Model = (User)user.ReturnValue;
                this.UserEditor((User)user.ReturnValue, false);
            }
            return base.View();
        }

        [AuthenticatedAuthorize]
        [HttpPost]
        public ActionResult Edit(User u)
        {
            ActionResult action;
            if (u.Roles == null)
            {
                u.Roles = new string[0];
            }
            ReturnObject returnObject = null;
            UsingExtension.Using<UserServiceClient>(UserService.GetManagementClient(u.Application), (UserServiceClient client) => returnObject = client.UpdateUser(u));
            if (returnObject.ReturnCode != 0)
            {
                this.UserEditor(u, false, "", returnObject.DefaultErrorMessage);
                action = base.View();
            }
            else
            {
                action = base.RedirectToAction("Index", new { id = u.Application });
            }
            return action;
        }

        private int? ExpirationDays(User u)
        {
            int? nullable = null;
            UsingExtension.Using<UserServiceClient>(UserService.GetManagementClient(u.Application), (UserServiceClient client) =>
            {
                ReturnObject passwordExpirationDaysLeft = client.GetPasswordExpirationDaysLeft(u.Username);
                if (passwordExpirationDaysLeft.ReturnCode == 0)
                {
                    nullable = new int?((int)passwordExpirationDaysLeft.ReturnValue);
                }
            });
            return nullable;
        }

        private string[] GetRoles(string application)
        {
            string[] returnValue = null;
            UsingExtension.Using<UserServiceClient>(UserService.GetManagementClient(application), (UserServiceClient client) => returnValue = (string[])client.GetAllRoles().ReturnValue);
            return returnValue;
        }

        private int? InactivityDays(User u)
        {
            int? nullable = null;
            UsingExtension.Using<UserServiceClient>(UserService.GetManagementClient(u.Application), (UserServiceClient client) =>
            {
                ReturnObject inactivityDaysLeft = client.GetInactivityDaysLeft(u.Username);
                if (inactivityDaysLeft.ReturnCode == 0)
                {
                    nullable = new int?((int)inactivityDaysLeft.ReturnValue);
                }
            });
            return nullable;
        }

        [AuthenticatedAuthorize]
        public ActionResult Index(string id, int? page, int? size)
        {
            base.ViewData["id"] = id;
            int? nullable = page;
            int num1 = (nullable.HasValue ? nullable.GetValueOrDefault() : 1);
            nullable = size;
            int num2 = (nullable.HasValue ? nullable.GetValueOrDefault() : 25);
            UsingExtension.Using<UserServiceClient>(UserService.GetManagementClient(id), (UserServiceClient client) =>
            {
                int num = 0;
                IEnumerable<User> returnValue = (IEnumerable<User>)client.GetAllUsers(out num, num1 - 1, num2).ReturnValue;
                if (num == 0)
                {
                    num = returnValue.Count<User>();
                }
                base.ViewData.Model = new CustomPagination<User>(returnValue, num1, num2, num);
            });
            return base.View();
        }

        private EventAudit[] Logs(User u)
        {
            EventAudit[] returnValue = new EventAudit[0];
            int num = 0;
            UsingExtension.Using<UserServiceClient>(UserService.GetManagementClient(u.Application), (UserServiceClient client) => returnValue = (EventAudit[])client.GetApplicationLogsForUser(out num, u.Username, new int?(0), new int?(10)).ReturnValue);
            return returnValue;
        }

        [AuthenticatedAuthorize]
        public JsonResult ResetPassword(string id, string application)
        {
            string str;
            string str1;
            this.ResetPassword(id, application, out str1, out str);
            return base.Json(new { password = str1 ?? str });
        }

        private bool ResetPassword(string username, string application, out string password, out string error_message)
        {
            bool flag = false;
            string str = null;
            string defaultErrorMessage = null;
            UsingExtension.Using<UserServiceClient>(UserService.GetManagementClient(application), (UserServiceClient client) =>
            {
                ReturnObject user = client.GeneratePassword(username);
                if (user.ReturnCode != 0)
                {
                    defaultErrorMessage = user.DefaultErrorMessage;
                }
                else
                {
                    str = user.ReturnValue.ToString();
                    user = client.AssignPassword(username, str);
                    if (user.ReturnCode != 0)
                    {
                        defaultErrorMessage = user.DefaultErrorMessage;
                    }
                    else
                    {
                        user = client.GetUser(username);
                        if (user.ReturnCode != 0)
                        {
                            defaultErrorMessage = user.DefaultErrorMessage;
                        }
                        else
                        {
                            User returnValue = (User)user.ReturnValue;
                            returnValue.MustChangePassword = true;
                            user = client.UpdateUser(returnValue);
                            if (user.ReturnCode != 0)
                            {
                                defaultErrorMessage = user.DefaultErrorMessage;
                            }
                            else
                            {
                                flag = true;
                            }
                        }
                    }
                }
            });
            password = str;
            error_message = defaultErrorMessage;
            return flag;
        }

        private void UserEditor(User u, bool new_user)
        {
            //u.set_NewUsername(u.Username);
            base.ViewData.Model = u;
            base.ViewData["NewUser"] = new_user;
            base.ViewData["Roles"] = new MultiSelectList(this.GetRoles(u.Application));
            base.ViewData["UsernamePattern"] = this.UsernamePattern(u.Application);
            if (!new_user)
            {
                base.ViewData["InactivityDays"] = this.InactivityDays(u);
                base.ViewData["ExpirationDays"] = this.ExpirationDays(u);
                base.ViewData["Logs"] = this.Logs(u);
            }
        }

        private void UserEditor(User u, bool new_user, string error_attribute, string error_message)
        {
            this.UserEditor(u, new_user);
            base.ModelState.AddModelError(error_attribute, error_message);
        }

        private string UsernamePattern(string application)
        {
            string str = "";
            UsingExtension.Using<UserServiceClient>(UserService.GetManagementClient(application), (UserServiceClient client) =>
            {
                ApplicationInformation applicationInformation = client.GetApplicationInformation();
                str = (string.IsNullOrEmpty(applicationInformation.ValidUsernameRegex) ? "No pattern" : string.Format("{0} {1}", applicationInformation.ValidUsernameRegex, applicationInformation.ValidUsernameExplanation));
            });
            return str;
        }
    }
}