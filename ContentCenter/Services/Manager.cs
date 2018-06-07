using ContentCenter.Models;
using System;
using System.Collections.Generic;

namespace ContentCenter.Services
{
    public class Manager
    {
        private static readonly Lazy<Manager> lazy =
                        new Lazy<Manager>(() => new Manager());

        public List<UserDb> Users { get; private set; }

        private Manager()
        {
            Users = new List<UserDb>();
        }

        public static Manager GetInstance()
        {
            return lazy.Value;
        }

        public void InsertUser(UserDb user)
        {
            Users.Add(user);
        }
    }
}
