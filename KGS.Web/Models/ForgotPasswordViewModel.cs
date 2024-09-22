using System.ComponentModel;

namespace KGS.Web.Models
{
    public class ForgotPasswordViewModel
    {
       
        public string UserName { get; set; }

        [DisplayName("New Password")]
        public string NewPassword { get; set; }
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}