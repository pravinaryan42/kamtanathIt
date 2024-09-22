using KGS.Core;
using KGS.Web.Code.LIBS;
using KGS.Web.Code.Serialization;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace KGS.Web.Code.Attributes
{
    public class Captcha : System.Web.Mvc.ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var response = HttpContext.Current.Request["g-recaptcha-response"];
            bool IsCaptchaValid = (ReCaptchaClass.Validate(response) == "true" ? true : false);
            if (!IsCaptchaValid)
            {
                filterContext.Result = new JsonNetResult
                {
                    Data = new RequestOutcome<string> { ErrorMessage = "Incorrect captcha!" },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };

            }
            base.OnActionExecuting(filterContext);
        }
    }
}