using ContentCenter.Models;
using ContentCenter.Services;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace ContentCenter.Test
{
    public class ConnectionDb
    {
        DbConnector dbConnector = null;
        IOptions<SQLServer> _options; 

        public ConnectionDb()
        {
            SQLServer sqlServer = GetSettings();

            _options = Options.Create<SQLServer>(new SQLServer
            {
                ConnectionStringPG = sqlServer.ConnectionStringPG,
                QueryPG = sqlServer.QueryPG,
                QueryTokenPG = sqlServer.QueryTokenPG,
                Provider = sqlServer.Provider
            });
            
        }
        [Fact(DisplayName = "Проверка на получение юзеров из 1с")]
        public void GetUsers()
        {
            dbConnector = new DbConnector(_options);
            List<UserBox> users = dbConnector.Getv8users().Result;

            Assert.NotNull(users);
            Assert.NotEmpty(users);
        }

        [Fact(DisplayName = "Проверка на получение токенов 1с")]
        public void GetTokens()
        {
            dbConnector = new DbConnector(_options);
            Dictionary<string, string> tokens = dbConnector.GetTokens().Result;

            Assert.NotNull(tokens);
            Assert.NotEmpty(tokens);
        }


        #region private methods
        private SQLServer GetSettings()
        {
            string path = @"D:\new\ContentCenter\ContentCenter\ContentCenter\appsettings.json";
            Settings settings = JsonConvert.DeserializeObject<Settings>(
                File.ReadAllText(path));
            return settings.Logging.SQLServer;
        }
        #endregion
    }
}
