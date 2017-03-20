using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using System.Web.Routing;

namespace ManagementApplication.Controllers
{
    public static class ControllerExtensions
    {
        private static string GetViewName(Controller controller, string viewName)
        {
            return (!string.IsNullOrEmpty(viewName) ? viewName : controller.RouteData.GetRequiredString("action"));
        }

        public static ViewResult RazorView(this Controller controller)
        {
            return controller.RazorView(null, null);
        }

        public static ViewResult RazorView(this Controller controller, object model)
        {
            return controller.RazorView(null, model);
        }

        public static ViewResult RazorView(this Controller controller, string viewName)
        {
            return controller.RazorView(viewName, null);
        }

        public static ViewResult RazorView(this Controller controller, string viewName, object model)
        {
            if (model != null)
            {
                controller.ViewData.Model = model;
            }
            ((dynamic)controller.ViewBag)._ViewName = ControllerExtensions.GetViewName(controller, viewName);
            ViewResult viewResult = new ViewResult();
            viewResult.ViewName = "RazorView";
            viewResult.ViewData = controller.ViewData;
            viewResult.TempData = controller.TempData;
            return viewResult;
        }

        public static ViewResult RazorViewWithShim(this Controller controller, string shim)
        {
            return controller.RazorViewWithShim(null, null, shim);
        }

        public static ViewResult RazorViewWithShim(this Controller controller, object model, string shim)
        {
            return controller.RazorViewWithShim(null, model, shim);
        }

        public static ViewResult RazorViewWithShim(this Controller controller, string viewName, string shim)
        {
            return controller.RazorViewWithShim(viewName, null, shim);
        }

        public static ViewResult RazorViewWithShim(this Controller controller, string viewName, object model, string shim)
        {
            if (model != null)
            {
                controller.ViewData.Model = model;
            }
            ((dynamic)controller.ViewBag)._ViewName = ControllerExtensions.GetViewName(controller, viewName);
            ViewResult viewResult = new ViewResult();
            viewResult.ViewName = shim;
            viewResult.ViewData = controller.ViewData;
            viewResult.TempData = controller.TempData;
            return viewResult;
        }
    }
}