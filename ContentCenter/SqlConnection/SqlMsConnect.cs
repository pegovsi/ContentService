using ContentCenter.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Options;
using Serilog;

namespace ContentCenter.SqlConnection
{
    public class SqlMsConnect : ISqlConnection
    {
        private readonly IOptions<SQLServer> _options;
        public SqlMsConnect(IOptions<SQLServer> options)
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
                    var data = await db.QueryAsync<UserBox>(_options.Value.QueryMS);
                    users = data.ToList();
                    Log.Information($"Получение пользователей из базы 1с по адресу {_options.Value.ConnectionStringMS}. Количество юзеров: {users.Count}");
                }
                catch (Exception ex)
                {
                    Log.Error($"Ошибка при получении пользователей из базы 1с по адресу {_options.Value.ConnectionStringMS}. Ошибка: {ex.Message}");
                }
                return users;
                
            }
        }

        public async Task<Dictionary<string, string>> GetTokens()
        {
            using (var db = OpenConnection())
            {
                try
                {
                    var data = await db.QueryAsync<TokenBox>(_options.Value.QueryTokenMS);
                    Log.Information($"Получение токенов из базы 1с по адресу {_options.Value.ConnectionStringMS}. Количество токенов: {data.Count()}");
                    return data.ToDictionary(x=>x.tokenkey, x=>x.path);
                }
                catch (Exception ex)
                {
                    Log.Error($"Ошибка при получении токенов на контент из базы 1с по адресу {_options.Value.ConnectionStringMS}. Ошибка: {ex.Message}");
                    return new Dictionary<string, string>();                    
                }                
            }
        }

        private IDbConnection OpenConnection()
        {
            if (string.IsNullOrEmpty(_options.Value?.ConnectionStringMS))
            {
                Log.Error($"Пустая строка подключения к БД или не корректная");
                throw new ArgumentNullException("connectionString");
            }

            return new System.Data.SqlClient.SqlConnection(_options.Value.ConnectionStringMS);            
        }
    }
}
