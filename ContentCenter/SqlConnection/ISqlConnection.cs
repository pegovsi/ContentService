using ContentCenter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentCenter.SqlConnection
{
    public interface ISqlConnection
    {
        Task<List<UserBox>> GetUsers();
        Task<Dictionary<string, string>> GetTokens();
    }
}
