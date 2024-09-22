using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using KGS.Web.Models.Security;
using KGS.Web.Code.LIBS;
using System;
using KGS.Core;
using System.Linq;
using KGS.Data;

namespace KGS.Web.Code.Attributes
{
    public class CustomAuthorization : AuthorizeAttribute
    {

        private UserRoles[] UserRoless;
        public CustomAuthorization(params UserRoles[] UserRoless)
        {
            this.UserRoless = UserRoless;
        }

        protected virtual CustomPrincipal CurrentUser
        {
            get { return HttpContext.Current.User as CustomPrincipal; }
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var rd = filterContext.HttpContext.Request.RequestContext.RouteData;
            string currentController = rd.GetRequiredString("controller").ToLower();
            string currentAction = rd.GetRequiredString("action").ToLower();
            if (CurrentUser != null && CurrentUser.Identity.IsAuthenticated && UserRoless.Length > 0)
            {
                if (!CurrentUser.IsInRole(UserRoless))
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                    {
                        controller = "error",
                        action = "unauthorized",
                        area = ""
                    }));
                }
             
            }
            else
            {
                base.OnAuthorization(filterContext);
            }
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //filterContext.Co
            string areaName = Convert.ToString(filterContext.RouteData.DataTokens["area"]);
            if (areaName == "admin" || areaName == "Admin")
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "account",
                    action = "index",
                    returnurl = HttpContext.Current.Request.Path,
                    area = "admin"
                }));
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "home",
                    action = "index",
                    returnurl = HttpContext.Current.Request.Path,
                    area = ""
                }));
            }
            //else {
            //    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
            //    {
            //        controller = "Home",
            //        action = "Index",
            //        area = ""
            //    }));

            //}


        }

    }
}
