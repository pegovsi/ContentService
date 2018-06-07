using ContentCenter.Models;
using System.Collections.Generic;

namespace ContentCenter.Services
{
    public interface IUsersManager
    {
        List<UserDb> GetUsers();
        void Insert(UserDb user);
        void Update(UserDb user);
    }
}
