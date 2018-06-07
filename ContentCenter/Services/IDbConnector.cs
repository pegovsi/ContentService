using ContentCenter.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContentCenter.Services
{
    public interface IDbConnector
    {
        Task<List<UserBox>> Getv8users();
        Task<Dictionary<string, string>> GetTokens();
    }
}
