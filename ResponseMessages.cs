using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace AppCustomer.Common
{
    public static class ResponseMessages
    {
        public static JsonResult GetCustomMessage(bool isSuccess, string message)
        {
            if (isSuccess)
            {
                return new JsonResult() { Data= new { Status = "Success", Message = message }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                return new JsonResult() { Data= new { Status = "Failed", Message = message }, JsonRequestBehavior=JsonRequestBehavior.AllowGet };
            }
        }

        public static JsonResult GetGenericUnknonFailed()
        {
            return GetCustomMessage(false, "Some error occured please try again later");
        }

        public static JsonResult GetGenericModalFailed()
        {
            return GetCustomMessage(false, "Please check all the fields and try again.");
        }

        public static JsonResult GetGenericCreatedFailed(string entity,string uniqueKey)
        {
            return GetCustomMessage(false,string.Format("Failed to create the {0} as it is {1} unique",entity,uniqueKey));
        }

        public static JsonResult GetGenericEditFailed(string entity, string uniqueKey)
        {
            return GetCustomMessage(false, string.Format("Failed to update the {0} as it is {1} unique", entity, uniqueKey));
        }

        public static JsonResult GetGenericDeleteFailed(string entity)
        {
            return GetCustomMessage(false, string.Format("Failed to delete the {0} as it in use", entity));
        }
    }
}