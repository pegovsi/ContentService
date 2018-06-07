using ContentCenter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentCenter.SqlConnection
{
    public class SqlConnect
    {
        public ISqlConnection _sqlConnection { private get; set; }

        public SqlConnect(ISqlConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }

        public async Task<List<UserBox>> GetUsers()
        {
            return await _sqlConnection.GetUsers();
        }

        public async Task<Dictionary<string, string>> GetTokens()
        {
            return await _sqlConnection.GetTokens();
        }
    }
}
