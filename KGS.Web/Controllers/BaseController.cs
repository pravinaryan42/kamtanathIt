using DataTables.AspNet.Core;
using DataTables.AspNet.Mvc5;
using KGS.Data;
using KGS.Web.Code.LIBS;
using KGS.Web.Code.Serialization;
using KGS.Web.Models.Others;
using KGS.Web.Models.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace KGS.Web.Controllers
{
    public class BaseController : Controller
    {

        #region "Authentication"

        public CustomPrincipal CurrentUser
        {
            get { return HttpContext.User as CustomPrincipal; }
        }

        public void CreateUserAuthenticationTicket(User user,bool isPersist)
        {
            if (user != null)
            {
                byte[] roles = { Convert.ToByte(user.UserRoleId) };
                CustomPrincipal principal = new CustomPrincipal(user, roles);
                principal.UserId = user.UserId;
                var authTicket = new FormsAuthenticationTicket(1,
                    user.Email,
                    DateTime.Now,
                    DateTime.Now.AddDays(1),
                    isPersist,
                    JsonConvert.SerializeObject(principal));

                string encTicket = FormsAuthentication.Encrypt(authTicket);
                HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                Response.Cookies.Add(faCookie);
            }
        }

        public void UpdateAuthenticationTicket(User user, byte[] defaultUserModulePermissions, bool isPersist, bool isRemind, Guid? doctorId = null, Guid? singlePatientIntakeId = null)
        {
            if (user != null)
            {
                HttpCookie authCookie = this.Request.Cookies[FormsAuthentication.FormsCookieName];
                if (authCookie != null)
                {
                    byte[] roles = { Convert.ToByte(user.UserRoleId) };
                    CustomPrincipal principal = new CustomPrincipal(user, roles);
                    principal.UserId = user.UserId;
                    var authTicket = new FormsAuthenticationTicket(1,
                        user.Email,
                        DateTime.Now,
                        DateTime.Now.AddDays(1),
                        isPersist,
                        JsonConvert.SerializeObject(principal));

                    string encTicket = FormsAuthentication.Encrypt(authTicket);
                    HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                    Response.Cookies.Add(faCookie);
                }
            }
        }

        public void RemoveAuthentication()
        {

            FormsAuthentication.SignOut();
        }

        #endregion "Authentication"

        #region "Notificatons"

        private void ShowMessages(string title, string message, MessageType messageType, bool isCurrentView)
        {
            Notification model = new Notification
            {
                Heading = title,
                Message = message,
                Type = messageType
            };

            if (isCurrentView)
                this.ViewData.AddOrReplace("NotificationModel", model);
            else
                this.TempData.AddOrReplace("NotificationModel", model);
        }

        protected void ShowErrorMessage(string title, string message, bool isCurrentView = true)
        {
            ShowMessages(title, message, MessageType.Danger, isCurrentView);
        }

        protected void ShowSuccessMessage(string title, string message, bool isCurrentView = true)
        {
            ShowMessages(title, message, MessageType.Success, isCurrentView);
        }

        protected void ShowWarningMessage(string title, string message, bool isCurrentView = true)
        {
            ShowMessages(title, message, MessageType.Warning, isCurrentView);
        }

        protected void ShowInfoMessage(string title, string message, bool isCurrentView = true)
        {
            ShowMessages(title, message, MessageType.Info, isCurrentView);
        }

        #endregion "Notificatons"

        #region "HTTP Errors"

        protected ActionResult Redirect404()
        {
            return Redirect("~/error/pagenotfound");
        }

        protected ActionResult Redirect500()
        {
            return Redirect("~/error/servererror");
        }

        protected ActionResult Redirect401()
        {
            return View();
        }

        #endregion "HTTP Errors"

        #region "Exception Handling"

        public PartialViewResult CreateModelStateErrors()
        {
            return PartialView("_ValidationSummary", ModelState.Values.SelectMany(x => x.Errors));
        }

        #endregion "Exception Handling"

        #region "Serialization"

        //public ActionResult NewtonSoftJsonResult(object data)
        //{
        //    return Json(data);
        //}
        public ActionResult NewtonSoftJsonResult(object data)
        {
            //Override IIS default status code message structure
            Response.TrySkipIisCustomErrors = true;
            return new JsonNetResult
            {
                Data = data,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
                //Use this value to set your maximum size for all of your Requests
            };
        }

        #endregion "Serialization"

        #region "DataTables Response"

        public DataTablesJsonResult DataTablesJsonResult(int total, IDataTablesRequest request, IEnumerable<object> data)
        {
            var response = DataTablesResponse.Create(request, total, total, data);
            return new DataTablesJsonResult(response, JsonRequestBehavior.AllowGet);
        }

        protected override JsonResult Json(object data, string contentType,
         System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = Int32.MaxValue
            };
        }

        public string GetModelValidateMessage()
        {
            string message = string.Empty;
            foreach (ModelState modelState in ViewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    message = String.Format("{0}, {1}", message, error.ErrorMessage);
                }
            }
            return message;
        }

        #endregion "DataTables Response"

        public string RenderViewAsString(string viewName, object model)
        {
            // create a string writer to receive the HTML code
            StringWriter stringWriter = new StringWriter();

            // get the view to render
            ViewEngineResult viewResult = ViewEngines.Engines.FindView(ControllerContext, viewName, null);
            // create a context to render a view based on a model
            ViewContext viewContext = new ViewContext(
                    ControllerContext,
                    viewResult.View,
                    new ViewDataDictionary(model),
                    new TempDataDictionary(),
                    stringWriter
                    );

            // render the view to a HTML code
            viewResult.View.Render(viewContext, stringWriter);

            // return the HTML code
            return stringWriter.ToString();
        }
        #region "Dispose"

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        #endregion "Dispose"
    }
}