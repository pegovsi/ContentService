using ContentCenter.Models;
using System.Collections.Generic;
using System.Linq;

namespace ContentCenter.Services
{
    public class UsersManager: IUsersManager
    {
        Manager manager = Manager.GetInstance();
        public UsersManager()
        {

        }

        public List<UserDb> GetUsers()
        {
            return manager.Users; 
        }
        public void Insert(UserDb user)
        {
            var _user = manager.Users.FirstOrDefault(x => x.Name.ToUpper() == user.Name.ToUpper());
            if (_user == null)
                manager.Users.Add(user);
        }
        public void Update(UserDb user)
        {
            var _user = manager.Users.FirstOrDefault(x => x.Name.ToUpper() == user.Name.ToUpper());
            if(_user ==null)
                manager.Users.Add(user);
        }

    }
}
