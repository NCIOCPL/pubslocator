using Aspensys.GlobalUsers.WebServiceClient;
using Aspensys.GlobalUsers.WebServiceClient.UserService;
using ManagementApplication;
using ManagementApplication.Models;
using MvcContrib.Pagination;
using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;

namespace ManagementApplication.Controllers
{
    [HandleError]
    public class ApplicationController : ManagementController
    {
        public ApplicationController()
        {
        }

        private void BuildLookups()
        {
            base.ViewData["MaxPassword"] = new SelectList(new int[] { 10, 15, 20, 25, 30 });
        }

        [ApplicationAuthorize]
        public ActionResult Create()
        {
            this.BuildLookups();
            Application application = new Application()
            {
                Questions = new ApplicationQuestion[5]
            };
            ApplicationQuestion[] questions = application.Questions;
            ApplicationQuestion applicationQuestion = new ApplicationQuestion();
            applicationQuestion.QuestionID = 0;
            applicationQuestion.QuestionText = "What was the make of your first car?";
            questions[0] = applicationQuestion;
            ApplicationQuestion[] applicationQuestionArray = application.Questions;
            ApplicationQuestion applicationQuestion1 = new ApplicationQuestion();
            applicationQuestion1.QuestionID = 0;
            applicationQuestion1.QuestionText = "In what city or town was your first job?";
            applicationQuestionArray[1] = applicationQuestion1;
            ApplicationQuestion[] questions1 = application.Questions;
            ApplicationQuestion applicationQuestion2 = new ApplicationQuestion();
            applicationQuestion2.QuestionID = 0;
            applicationQuestion2.QuestionText = "What school did you attend for sixth grade?";
            questions1[2] = applicationQuestion2;
            ApplicationQuestion[] applicationQuestionArray1 = application.Questions;
            ApplicationQuestion applicationQuestion3 = new ApplicationQuestion();
            applicationQuestion3.QuestionID = 0;
            applicationQuestion3.QuestionText = "What is the name of your favorite childhood friend?";
            applicationQuestionArray1[3] = applicationQuestion3;
            ApplicationQuestion[] questions2 = application.Questions;
            ApplicationQuestion applicationQuestion4 = new ApplicationQuestion();
            applicationQuestion4.QuestionID = 0;
            applicationQuestion4.QuestionText = "What was your childhood phone number including area code?";
            questions2[4] = applicationQuestion4;
            application.Accounts = new string[0];
            base.ViewData.Model = application;
            return base.View();
        }

        [ApplicationAuthorize]
        [HttpPost]
        public ActionResult Create(Application a, HttpPostedFileBase setting_import)
        {
            ActionResult actionResult;
            this.BuildLookups();
            if (setting_import == null)
            {
                ActionResult action = base.View(a);
                if (base.ModelState.IsValid)
                {
                    Application application = a;
                    Guid guid = Guid.NewGuid();
                    application.ApplicationPassword = guid.ToString().Replace("-", "");
                    ReturnObject returnObject = null;
                    UsingExtension.Using<UserServiceClient>(new UserServiceClient(), (UserServiceClient client) => returnObject = client.CreateApplication(a.ToApplicationInformation()));
                    if (!(returnObject is SuccessReturnObject))
                    {
                        base.ViewData = base.ViewData;
                        base.ViewData["ServiceError"] = returnObject.DefaultErrorMessage;
                    }
                    else
                    {
                        UsingExtension.Using<UserServiceClient>(new UserServiceClient(), (UserServiceClient client) => returnObject = client.AddRole(string.Concat(a.ApplicationName, " Administrator")));
                        action = base.RedirectToAction("Edit", new { id = a.ApplicationName });
                    }
                }
                actionResult = action;
            }
            else
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Application));
                try
                {
                    base.ViewData.Model = xmlSerializer.Deserialize(setting_import.InputStream);
                    base.ViewData["ServiceError"] = "This data has been restored from the provided file.  If you wish to commit it, press the Save button below. If you wish to abort this import, use the Applications link above.";
                }
                catch (Exception exception1)
                {
                    Exception exception = exception1;
                    base.ViewData.Model = a;
                    base.ViewData["ServiceError"] = string.Concat("Import failed.  Error: ", exception.ToString());
                }
                actionResult = base.View();
            }
            return actionResult;
        }

        [ApplicationAuthorize]
        public ActionResult Delete(string id)
        {
            UsingExtension.Using<UserServiceClient>(new UserServiceClient(), (UserServiceClient client) => client.DeleteApplication(id));
            return base.RedirectToAction("Index");
        }

        [ApplicationAuthorize]
        public ActionResult Edit(string id)
        {
            this.BuildLookups();
            UsingExtension.Using<UserServiceClient>(UserService.GetManagementClient(id), (UserServiceClient client) =>
                base.ViewData.Model = client.GetApplicationInformation().ToApplicationInfo()
            );
            //UsingExtension.Using<UserServiceClient>(UserService.GetManagementClient(id), (UserServiceClient client) => (base.get_ViewData().Model = client.GetApplicationInformation().ToApplicationInfo()));
            return base.View();
        }

        [ApplicationAuthorize]
        [HttpPost]
        public ActionResult Edit(Application ai, HttpPostedFileBase setting_import)
        {
            ActionResult action;
            this.BuildLookups();
            if (setting_import == null)
            {
                if (base.ModelState.IsValid)
                {
                    ReturnObject returnObject = null;
                    UsingExtension.Using<UserServiceClient>(UserService.GetManagementClient(ai.ApplicationName), (UserServiceClient client) => returnObject = client.UpdateApplication(ai.ToApplicationInformation()));
                    if (returnObject is SuccessReturnObject)
                    {
                        action = base.RedirectToAction("Index");
                        return action;
                    }
                    base.ViewData["ServiceError"] = returnObject.DefaultErrorMessage;
                }
                action = base.View(ai);
            }
            else
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Application));
                try
                {
                    base.ViewData.Model = xmlSerializer.Deserialize(setting_import.InputStream);
                    base.ViewData["ServiceError"] = "This data has been restored from the provided file.  If you wish to commit it, press the Save button below. You may make changes before doing so.<br/><br/>If you wish to abort this import, use the Applications link above.";
                }
                catch (Exception exception1)
                {
                    Exception exception = exception1;
                    base.ViewData.Model = ai;
                    base.ViewData["ServiceError"] = string.Concat("Import failed.  Error: ", exception.ToString());
                }
                action = base.View();
            }
            return action;
        }

        [ApplicationAuthorize]
        public FileStreamResult Export(string id)
        {
            Application applicationInfo = null;
            UsingExtension.Using<UserServiceClient>(UserService.GetManagementClient(id), (UserServiceClient client) => applicationInfo = client.GetApplicationInformation().ToApplicationInfo());
            base.HttpContext.Response.AddHeader("content-disposition", string.Concat("attachment; filename=", id, ".xml"));
            MemoryStream memoryStream = new MemoryStream();
            XmlSerializer xmlSerializer = new XmlSerializer(applicationInfo.GetType());
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
            xmlSerializer.Serialize(xmlTextWriter, applicationInfo);
            xmlTextWriter.BaseStream.Seek((long)0, SeekOrigin.Begin);
            return new FileStreamResult(xmlTextWriter.BaseStream, "text/xml");
        }

        [AuthenticatedAuthorize]
        public ActionResult Index()
        {
            UsingExtension.Using<UserServiceClient>(new UserServiceClient(), (UserServiceClient client) => base.ViewData.Model =
                from a in client.GetApplicationInformationList()
                where (base.HttpContext.User.IsInRole("Full Administrator") ? true : base.HttpContext.User.IsInRole(string.Concat(a.ApplicationName, " Administrator")))
                select a.ToApplicationInfo() into a
                orderby a.ApplicationName
                select a);
            /*
            UsingExtension.Using<UserServiceClient>(new UserServiceClient(), (UserServiceClient client) => base.get_ViewData().set_Model(
                from a in client.GetApplicationInformationList()
                where (base.HttpContext.User.IsInRole("Full Administrator") ? true : base.HttpContext.User.IsInRole(string.Concat(a.ApplicationName, " Administrator")))
                select a.ToApplicationInfo() into a
                orderby a.ApplicationName
                select a));
            */
            return base.View();
        }

        [ApplicationAuthorize]
        public ActionResult Log(string id, DateTime start, DateTime end, int? page, int? size)
        {
            int? nullable = page;
            int num = (nullable.HasValue ? nullable.GetValueOrDefault() : 1);
            nullable = size;
            int num1 = (nullable.HasValue ? nullable.GetValueOrDefault() : 25);
            base.ViewData["id"] = id;
            base.ViewData["start"] = start;
            base.ViewData["end"] = end;
            ViewDataDictionary viewData = base.ViewData;
            SelectListItem[] selectListItemArray = new SelectListItem[4];
            SelectListItem selectListItem = new SelectListItem();
            selectListItem.Text = "10";
            selectListItem.Value = "10";
            selectListItemArray[0] = selectListItem;
            SelectListItem selectListItem1 = new SelectListItem();
            selectListItem1.Text = "25";
            selectListItem1.Value = "25";
            selectListItemArray[1] = selectListItem1;
            SelectListItem selectListItem2 = new SelectListItem();
            selectListItem2.Text = "50";
            selectListItem2.Value = "50";
            selectListItemArray[2] = selectListItem2;
            SelectListItem selectListItem3 = new SelectListItem();
            selectListItem3.Text = "100";
            selectListItem3.Value = "100";
            selectListItemArray[3] = selectListItem3;
            viewData["size"] = new SelectList(selectListItemArray, "Text", "Value", num1.ToString());
            EventAudit[] returnValue = new EventAudit[0];
            int num2 = 0;
            UsingExtension.Using<UserServiceClient>(UserService.GetManagementClient(id), (UserServiceClient client) => returnValue = (EventAudit[])client.GetApplicationLogs(out num2, start, end, new int?(num - 1), new int?(num1)).ReturnValue);
            base.ViewData.Model = new CustomPagination<EventAudit>(returnValue, num, num1, num2);
            return base.View();

            /*
             *             selectListItem.set_Text("10");
            selectListItem.set_Value("10");

             */
        }
    }
}