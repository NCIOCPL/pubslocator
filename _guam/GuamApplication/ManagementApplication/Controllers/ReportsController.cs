using Aspensys.GlobalUsers.WebServiceClient;
using Aspensys.GlobalUsers.WebServiceClient.UserService;
using ManagementApplication.Models;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;

namespace ManagementApplication.Controllers
{
    public class ReportsController : ManagementController
    {
        public ReportsController()
        {
        }

        [Authorize(Roles = "Full Administrator")]
        public ActionResult Applications(bool? excel)
        {
            bool flag;

            UsingExtension.Using<UserServiceClient>(new UserServiceClient(), (UserServiceClient client) => base.ViewData.Model = (
                from a in client.GetApplicationInformationList()
                select a.ToApplicationInfo() into a
                orderby a.ApplicationName
                select a).ToArray<Application>());
            /*
            UsingExtension.Using<UserServiceClient>(new UserServiceClient(), (UserServiceClient client) => base.get_ViewData().set_Model((
                from a in client.GetApplicationInformationList()
                select a.ToApplicationInfo() into a
                orderby a.ApplicationName
                select a).ToArray<Application>()));
            */
            dynamic viewBag = base.ViewBag;
            flag = (!excel.HasValue ? false : excel.Value);
            viewBag.Excel = flag;
            if (((dynamic)base.ViewBag).Excel)
            {
                base.Response.AddHeader("Content-Type", "application/vnd.ms-excel");
                base.Response.AddHeader("Content-Disposition", "attachment; filename='ApplicationReport.xls'");
            }
            return base.View();
        }
    }
}