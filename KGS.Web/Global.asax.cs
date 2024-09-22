using FluentValidation.Mvc;
using KGS.Web.Code.Attributes;
using KGS.Web.Code.Validation;
using KGS.Web.Models.Security;
using Newtonsoft.Json;
using OfficeOpenXml;
using SB.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace KGS.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            GlobalFilters.Filters.Add(new HandleErrorAttribute());
            GlobalFilters.Filters.Add(new ManageExceptionFilter());
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ValidationConfiguration();
            System.Globalization.CultureInfo.DefaultThreadCurrentCulture = new System.Globalization.CultureInfo("en-US");
        }
        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = this.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                try
                {
                    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                    var serializeModel = JsonConvert.DeserializeObject<CustomPrincipal>(authTicket.UserData);
                    CustomPrincipal newUser = new CustomPrincipal(authTicket.Name);
                    newUser.UserId = serializeModel.UserId;
                    newUser.FirstName = serializeModel.FirstName;
                    newUser.LastName = serializeModel.LastName;
                    newUser.CreatedDate = serializeModel.CreatedDate;
                    newUser.Roles = serializeModel.Roles;
                    newUser.SinglePatientIntakeId = serializeModel.SinglePatientIntakeId;
                    newUser.DoctorId = serializeModel.DoctorId;
                    newUser.SubscriptionPlanId = serializeModel.SubscriptionPlanId;
                    newUser.SubscriptionExpireIn = serializeModel.SubscriptionExpireIn;
                    newUser.IsSubscriptionExpired = serializeModel.IsSubscriptionExpired;
                    newUser.UserModulePermissions = serializeModel.UserModulePermissions;
                    newUser.IsTwoNumberIdentity = serializeModel.IsTwoNumberIdentity;
                    newUser.IsFreeSubscription = serializeModel.IsFreeSubscription;
                    newUser.IsSubscriptionExpired = serializeModel.IsSubscriptionExpired;
                    HttpContext.Current.User = newUser;
                }
                catch (CryptographicException)
                {
                    FormsAuthentication.SignOut();
                }
            }
        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
            if (Code.LIBS.SiteKeys.IsLive == "1")
            {
                if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("http:"))
                {
                    Response.Redirect(HttpContext.Current.Request.Url.ToString().ToLower().Replace("http:", "https:"));
                }
            }

        }
        private void ValidationConfiguration()
        {
            FluentValidationModelValidatorProvider.Configure(provider =>
            {
                provider.ValidatorFactory = new ValidatorFactory();
            });
        }
    }
}
