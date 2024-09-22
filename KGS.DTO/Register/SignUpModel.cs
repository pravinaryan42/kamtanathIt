using System;
using System.Collections.Generic;
using System.Text;

namespace KGS.Dto
{
    public class SignUpModel
    {
     
            public SignUpModel()
            {
               // UserList = new List<SelectListItem>();
            }
            public int UserId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int Gender { get; set; }
            public string EmailAddress { get; set; }
            public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public int Salt { get; set; }


    }
}
