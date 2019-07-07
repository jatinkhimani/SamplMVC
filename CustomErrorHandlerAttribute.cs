﻿using AppCustomer.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppCustomer.Common
{
    public class CustomErrorHandlerAttribute:HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            var resultView = new ErrorController() { ControllerContext = filterContext }.Index();
            resultView.ExecuteResult(filterContext);
            filterContext.ExceptionHandled = true;
        }
    }
}