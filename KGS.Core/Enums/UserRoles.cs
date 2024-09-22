using System.ComponentModel;

namespace KGS.Core
{
  
    public enum UserRoles
    {
        [Description("SuperAdmin")]
        SuperAdmin = 1,
        [Description("CompanyAdmin")]
        CompanyAdmin = 2,
        [Description("User")]
        User = 3,
    }

   
}