using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KGS.Data;

namespace KGS.Service
{
    public interface ILoginService 
    {
        User GetUserLoginDetails(string email);
        User SaveUser(User usercreate);
        User UpdateUser(User usercreate);
    }
}
