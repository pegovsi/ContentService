using ContentCenter.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

namespace ContentCenter.Services
{
    public class MiracleService : IMiracleService
    {
        private readonly IDbConnector _dbConnector;
        private readonly IUsersManager _usersManager;
        private readonly ITokenManager _tokenManager;
        private readonly IConvertManager _convertManager;
        private readonly IParserStringService _parserStringService;
        private readonly ISocketService _socketService;
        private readonly IOptions<SocketServer> _options;

        public MiracleService(IDbConnector dbConnector,
            IUsersManager usersManager,
            IConvertManager convertManager,
            IParserStringService parserStringService,
            ITokenManager tokenManager,
            ISocketService socketService,
            IOptions<SocketServer> options)
        {
            _dbConnector = dbConnector;
            _usersManager = usersManager;
            _tokenManager = tokenManager;
            _convertManager = convertManager;
            _parserStringService = parserStringService;
            _socketService = socketService;
            _options = options;

            CreateConnection();
        }

        public async Task FillData()
        {            
            //получаем данные с БД
            var users = await _dbConnector.Getv8users();
            //Собираем в правильный класс
            foreach (var item in users)
            {
                int i = 0;
                byte[] b = new byte[0];
                var tmp_str = _convertManager.DecodePasswordStructure(item.data, ref i, ref b);
                List<object> _list = _parserStringService.ParseString(tmp_str);
                if (_list.Count != 0)
                {
                    foreach (var obj in (List<object>)_list[0])
                    {
                        var ss = ((List<object>)obj).ToArray();
                        //заполняем в хранилище
                        _usersManager.Insert(new UserDb
                        {
                            Name = item.name,
                            Hash = ss[12].ToString()
                        });                          
                    }
                }
            }           
            
        }

        public async Task FillTokenCollection()
        {
            //получаем данные с БД
            var tokens = await _dbConnector.GetTokens();
            _tokenManager.Insert(tokens);
        }

        private void CreateConnection()
        {
            SocketServer socketServer = _options.Value;

            _socketService.InitializeSocket(socketServer.Address, socketServer.Port, socketServer.Ws, Guid.NewGuid().ToString());
            _socketService.OnMessageEvent += new EventHandler<MessageEventArgs>(OnMessage);

        }

        private async void OnMessage(object sender, MessageEventArgs e)
        {
            Transport transport = JsonConvert.DeserializeObject<Transport>(
                Encoding.UTF8.GetString(e.RawData));

            await ExecuteCommand(transport);                
        }

        private async Task ExecuteCommand(Transport transport)
        {
            List<Command> commands = transport.TypeMessage.commands;
            foreach (var item in commands)
            {
                switch (item.command)
                {
                    case "update_users":
                        Log.Information($"Socket command: {item.command}");
                        await FillData();
                        break;
                    case "update_tokens":
                        Log.Information($"Socket command: {item.command}");
                        await FillTokenCollection();
                        break;
                }
            }
        }
    }
}
