using KGS.Core;
using KGS.Data;
using KGS.Dto;
using KGS.Service;
using KGS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace KGS.Web.Controllers
{
    public class AccountController : BaseController
    {
        private readonly ILoginService loginService;
        #region [Method]
        // GET: Account

        public AccountController(ILoginService loginService)
        {
            this.loginService = loginService;
        }

        public ActionResult Login()
        {
            UserLoginViewModel model = new UserLoginViewModel();
            return View(model);
        }

        public ActionResult RegisterUser()
        {
            SignUpModel model = new SignUpModel();
            return View(model);
        }

        [HttpGet]
        public ActionResult Logout()
        {
            RemoveAuthentication();
            if (CurrentUser != null)
            {
                CurrentUser.FirstName = null;
                CurrentUser.LastName = null;
            }
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            return RedirectToAction("Login", "Account");
        }

        #endregion

        #region [ SIGNIN ]

        [ChildActionOnly]
        public ActionResult SignIn()
        {
            UserLoginViewModel model = new UserLoginViewModel();
            return PartialView("_UserSignIn", model);
        }


        /// <summary>
        /// Sign In User
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult SignIn(UserLoginViewModel model)
        {
            var Redirecturl = "";
            if (Request.UrlReferrer != null && Request.UrlReferrer.Query.Contains("="))
                Redirecturl = Request.UrlReferrer.Query.Split('=')[1].Replace("%2F", "/");

            if (ModelState.IsValid)
            {
                var user = loginService.GetUserLoginDetails(model.Email);
                if (user != null && PasswordEncryption.PasswordsMatch(user.Password, model.Password, user.SaltKey))
                {
                    //Check whether user is not active
                    if (user.IsActive == null || !user.IsActive.Value)
                    {
                        ShowErrorMessage("Error", "Your account is not active." + Environment.NewLine + "This may be because your account is not verified. Our support team can help. Please email:  support@eprogresstracker.com.", false);
                        if (Redirecturl != "")
                            return NewtonSoftJsonResult(new RequestOutcome<dynamic> { RedirectUrl = Redirecturl });
                        else
                            return NewtonSoftJsonResult(new RequestOutcome<string> { RedirectUrl = @Url.Action("Index", "home") });
                    }

            


                  

                    byte userRoleId = (byte)(user.UserRoleId.HasValue ? user.UserRoleId.Value : 0);
                   
                    //Update Authentication Ticket
                    CreateUserAuthenticationTicket(user,false);

                    //Redirect user as per user role
                    string actionName = string.Empty, controllerName = string.Empty;
                    if (user.UserRoleId != null && user.UserRoleId.HasValue)
                    {
                     
                            return NewtonSoftJsonResult(new RequestOutcome<dynamic> { RedirectUrl = @Url.Action("Index", "Dashboard") });
                    }
              
                }
            }

            ShowErrorMessage("Error", "Email id or password is incorrect.", false);
            return NewtonSoftJsonResult(new RequestOutcome<dynamic> { RedirectUrl = @Url.Action("login", "account") });
        }

        #endregion [ SIGNIN ]

        #region [ SIGNUP ]
        /// <summary>
        /// Method to call user signup partial view
        /// </summary>
        /// <param name="id">this is role type i.e. for doctor 2 and for patient 3 </param>
        /// <returns></returns>
        

        [HttpPost]
        [ValidateInput(false)]
    
        public ActionResult Registeration(SignUpModel model)
        {
            try
            {
                User user = loginService.GetUserLoginDetails(model.EmailAddress);

                if (user != null && user.UserId != null)
                {
                    return NewtonSoftJsonResult(new RequestOutcome<string> { ErrorMessage = "Error! Email id already registered.", RedirectUrl = @Url.Action("Register", "Home") });
                }

                User Usercreate = new User();
                Usercreate.FirstName = model.FirstName.Trim();
                Usercreate.LastName = model.LastName.Trim();
                Usercreate.Email = model.EmailAddress.Trim();
                Usercreate.Mobile = model.PhoneNumber.Trim();
                var saltKey = PasswordEncryption.CreateSaltKey(5);
                var encryptedPassword = PasswordEncryption.CreatePasswordHash(model.Password, saltKey, "MD5");
                Usercreate.Password = encryptedPassword;
                Usercreate.SaltKey = saltKey;
                 
                Usercreate.IsActive = true;
            
                Usercreate.UserRoleId = (int)UserRoles.User;

                User saveduser = loginService.SaveUser(Usercreate);


                 
                    ShowSuccessMessage("Success!", "Success! Registration has been successfully. We have sent a verification link to registered email. Please verify to login", false);
                    return NewtonSoftJsonResult(new RequestOutcome<string> { RedirectUrl = @Url.Action("login", "account"), Message = "Success! Registration has been successfully. We have sent a verification link to registered email. Please verify to login" });

                

            }
            catch (Exception ex)
            {
                return NewtonSoftJsonResult(new RequestOutcome<string> { ErrorMessage = ex.ToString() });
            }

            //return NewtonSoftJsonResult(new RequestOutcome<string> { ErrorMessage = "Error! Registration has been failed." });
        }
        #endregion
    }
}