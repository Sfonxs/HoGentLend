using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HoGentLendTests.Controllers
{
    public static class Extensions
    {
        public static void AssertViewWasReturned(this ActionResult result, string viewName, string defaultViewName)
        {
            Assert.IsInstanceOfType(result, typeof(ViewResult), "Result is not an instance of ViewResult");
            var viewResult = (ViewResult)result;

            var actualViewName = viewResult.ViewName;

            if (actualViewName == "")
                actualViewName = defaultViewName;

            Assert.AreEqual(viewName, actualViewName, string.Format("Expected a View named {0}, got a View named {1}", viewName, actualViewName));
        }

        public static void AssertRedirectActionTo(this ActionResult result, string actionName, string controllerName)
        {
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult), "result is not an instance of RedirectToRouteResult");
            var redirectResult = (RedirectToRouteResult)result;

            var actualActionName = redirectResult.RouteValues["Action"];
            var actualControllerName = redirectResult.RouteValues["Controller"];

            Assert.AreEqual(actionName, actualActionName, string.Format("Expected a Action named {0}, got a Action named {1}", actionName, actualActionName));
            Assert.AreEqual(controllerName, actualControllerName, string.Format("Expected a Controller named {0}, got a Controller named {1}", controllerName, actualControllerName));

        }
    }
}