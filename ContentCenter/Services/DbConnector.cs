using System.Collections.Generic;
using System.Threading.Tasks;
using ContentCenter.Models;
using ContentCenter.SqlConnection;
using Microsoft.Extensions.Options;


namespace ContentCenter.Services
{
    public sealed class DbConnector : IDbConnector
    {
        private readonly IOptions<SQLServer> _options;        

        public DbConnector(IOptions<SQLServer> options)
        {
            _options = options;            
        }

        public async Task<List<UserBox>> Getv8users()
        {            
            SqlConnect sqlConnect = new SqlConnect(GetConnect());
            return await sqlConnect.GetUsers();                        
        }

        public async Task<Dictionary<string, string>> GetTokens()
        {            
            SqlConnect sqlConnect = new SqlConnect(GetConnect());
            return await sqlConnect.GetTokens();
        }


        private ISqlConnection GetConnect()
        {
            ISqlConnection sqlConnection = null;

            switch (_options.Value.Provider)
            {
                case Provider.pg:
                    sqlConnection = new SqlPgConnect(_options);
                    break;
                    
                case Provider.ms:
                    sqlConnection= new SqlMsConnect(_options);
                    break;                    
            }

            return sqlConnection;
        }
    }
}
