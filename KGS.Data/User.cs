//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KGS.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        public System.Guid UserId { get; set; }
        public string SaltKey { get; set; }
        public Nullable<int> UserRoleId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Password { get; set; }
        public string ProfileImage { get; set; }
        public string IpAddress { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
}
