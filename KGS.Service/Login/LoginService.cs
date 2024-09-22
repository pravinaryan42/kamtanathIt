using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KGS.Data;
using KGS.Repo;

namespace KGS.Service
{
    public class LoginService : ILoginService
    {
        #region "Fields"
        private IRepository<User> repoUserMaster;
        #endregion

        #region "Cosntructor"
        public LoginService(IRepository<User> repoUserMaster)
        {
            this.repoUserMaster = repoUserMaster;
        }
        #endregion
        //public UserLogin GetLoginDeatils(string email, string password)
        //{
        //    return repoUserMaster.Query().Filter(x => (x.UserName.ToLower() == email.ToLower() || x.EmailOffice.ToLower() == email.ToLower()) && String.Compare(x.Password, password, false) == 0 && x.IsActive == true).Get().FirstOrDefault();
        //}

        //public UserLogin GetLoginDeatilByEmail(string email)
        //{
        //    return repoUserMaster.Query().Filter(x => x.EmailOffice.ToLower() == email.ToLower()).Get().FirstOrDefault();
        //}

        public User GetUserLoginDetails(string email) {
            return repoUserMaster.Query().Filter(x => x.Email == email).Get().FirstOrDefault();
        }

        public User SaveUser(User usercreate) {
            usercreate.UserId = Guid.NewGuid();
       
            repoUserMaster.Insert(usercreate);
            return usercreate;
        }
        public User UpdateUser(User usercreate)
        {
   
            repoUserMaster.Update(usercreate);
            return usercreate;
        }
        #region "Dispose"
        public void Dispose()
        {
            if (repoUserMaster != null)
            {
                repoUserMaster.Dispose();
                repoUserMaster = null;
            }
        }
        #endregion

    }
}
