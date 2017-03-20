using Aspensys.GlobalUsers.WebServiceClient;
using Aspensys.GlobalUsers.WebServiceClient.UserService;
using ManagementApplication;
using System;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace ManagementApplication.Controllers
{
    public class RolesController : Controller
    {
        public RolesController()
        {
        }

        [ApplicationAuthorize]
        public JsonResult Add(string id, string role)
        {
            JsonResult jsonResult = null;
            UsingExtension.Using<UserServiceClient>(UserService.GetManagementClient(id), (UserServiceClient client) =>
            {
                ErrorReturnObject errorReturnObject = client.AddRole(role) as ErrorReturnObject;
                jsonResult = (errorReturnObject == null ? base.Json(new { result_success = "true", errormessage = "" }) : base.Json(new { result_success = "false", errormessage = errorReturnObject.DefaultErrorMessage }));
            });
            return jsonResult;
        }

        [ApplicationAuthorize]
        public JsonResult Delete(string id, string role)
        {
            JsonResult jsonResult = null;
            UsingExtension.Using<UserServiceClient>(UserService.GetManagementClient(id), (UserServiceClient client) =>
            {
                ErrorReturnObject errorReturnObject = client.DeleteRole(role, false) as ErrorReturnObject;
                jsonResult = (errorReturnObject == null ? base.Json(new { result_success = "true", errormessage = "" }) : base.Json(new { result_success = "false", errormessage = errorReturnObject.DefaultErrorMessage }));
            });
            return jsonResult;
        }

        [ApplicationAuthorize]
        public ActionResult Index(string id)
        {
            base.ViewData["id"] = id;
            UsingExtension.Using<UserServiceClient>(UserService.GetManagementClient(id), (UserServiceClient client) =>
            {
                ReturnObject allRoles = client.GetAllRoles();
                base.ViewData.Model = (string[])allRoles.ReturnValue;
            });
            return base.View();
        }
    }
}