using KGS.Data;
using KGS.Web.Code.LIBS;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Security.Principal;
using KGS.Core;
using System.Collections.Generic;
using System.Web.Mvc;

namespace KGS.Web.Models.Security
{
    public class CustomPrincipal : IPrincipal
    {

        public Guid UserId { get; set; }
        public Guid DoctorId { get; set; }
        public string UserName { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? RoleId { get; set; }
        public string Email { get; set; }
        //public string ProfileImage { get; set; }
        public byte[] Roles { get; set; }
        public Guid? SinglePatientIntakeId { get; set; }

        public bool IsTwoNumberIdentity { get; set; }
        public string CreatedDate { get; set; }
        [JsonIgnore]
        public IIdentity Identity { get; private set; }

        public Guid? SubscriptionPlanId { get; set; }
        public bool IsSubscriptionExpired { get; set; }
        public int SubscriptionExpireIn { get; set; }
        public byte[] UserModulePermissions { get; set; }

        public Guid PrimaryDoctor { get; set; }
        public bool CanScheduleAppointment { get; set; }
        public bool CanManageCategory { get; set; }
        public bool CanCheckInCheckOut { get; set; }
        public bool CanManageFAQ { get; set; }
        public bool IsFreeSubscription { get; set; }
        public CustomPrincipal() { }

        public CustomPrincipal(string userName, params byte[] userRoles)
        {
            this.Identity = new GenericIdentity(userName);
            this.UserName = userName;
            this.Roles = userRoles;
        }

        public CustomPrincipal(User user, params byte[] userRoles)
        {
            this.Identity = new GenericIdentity(user.Email);
            this.UserId = user.UserId;
            this.UserName = string.Format("{0} {1}", user.FirstName, user.LastName).Trim();
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.RoleId = user.UserRoleId;
            this.Email = user.Email;
            this.IsFreeSubscription = true;
         

            this.Roles = userRoles;
        }


        public bool IsInRole(UserRoles userRole)
        {
            return Roles.Contains((byte)userRole);
        }

        public bool IsInRole(params UserRoles[] userRoles)
        {
            return userRoles.Any(r => Roles.Contains((byte)r));
        }

        public bool IsInRole(string role)
        {
            //Check with enum
            UserRoles userRole;
            if (Enum.TryParse(role, out userRole)) { return IsInRole(userRole); }
            return false;

        }

     
    }
}
