using ContentCenter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using System.Data;
using Microsoft.Extensions.Options;
using Serilog;

namespace ContentCenter.SqlConnection
{
    public class SqlPgConnect : ISqlConnection
    {
        private readonly IOptions<SQLServer> _options;
        
        public SqlPgConnect(IOptions<SQLServer> options)
        {
            _options = options;            
        }
        public async Task<List<UserBox>> GetUsers()
        {            
            List<UserBox> users = new List<UserBox>();
            using (var db = OpenConnection())
            {
                try
                {
                    var data = await db.QueryAsync<UserBox>(_options.Value.QueryPG);
                    users = data.ToList();
                    Log.Information($"Sets users from database 1c. Address: {_options.Value.ConnectionStringPG}. Count users: {users.Count}");
                }
                catch (Exception ex)
                {
                    Log.Error($"Error sets user by address: {_options.Value.ConnectionStringPG}. Error: {ex.Message}");
                }                
            }

            return users;
        }

        public async Task<Dictionary<string, string>> GetTokens()
        {
            using (var db = OpenConnection())
            {
                try
                {
                    var data = await db.QueryAsync<TokenBox>(_options.Value.QueryTokenPG);
                    Log.Information($"Sets tokens from database 1c. Address {_options.Value.ConnectionStringPG}. Count tokens: {data.Count()}");
                    return data.ToDictionary(x => x.tokenkey, x => x.path);
                }
                catch (Exception ex)
                {
                    Log.Error($"Error sets tokens by address {_options.Value.ConnectionStringPG}. Error: {ex.Message}");
                    return new Dictionary<string, string>();    
                }                
            }
        }

        private IDbConnection OpenConnection()
        {
            if (string.IsNullOrEmpty(_options.Value?.ConnectionStringPG))
            {
                Log.Error($"This is connection string empty or not correct!");
                throw new ArgumentNullException("connectionString");
            }

            return new NpgsqlConnection(_options.Value.ConnectionStringPG);                        
        }
    }
}
