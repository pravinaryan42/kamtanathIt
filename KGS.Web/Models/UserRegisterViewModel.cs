using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KGS.Web.Models
{
    public class UserRegisterViewModel
    {
       
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [DisplayName("Email")]
        public string Email { get; set; }
        [DisplayName("Password")]
        public string Password { get; set; }
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }
         public bool AcceptTermsAndCondition { get; set; }

        public int RoleId { get; set; }
        [DisplayName("License Number")]
        public string LicenseNumber { get; set; }
        public bool IsDoctor { get; set; }

    }
}