using ContentCenter.Models;
using ContentCenter.Services;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using WebSocketSharp;

namespace ContentCenter.Services
{
    public class SocketService : ISocketService
    {
        public string URL { get; private set; }
        public WebSocket connection { get; private set; }
        public event EventHandler<MessageEventArgs> OnMessageEvent;

        private string connectionId = string.Empty;

        private bool _isConnected = false;
        public bool IsConnected
        {
            get => _isConnected;
            set => _isConnected = value;
        }
        private System.Timers.Timer timer = null;

        #region constructor
        public SocketService()
        {
            timer = new System.Timers.Timer();
            timer.Interval = 10000;
            timer.Elapsed += OpenConnectByTimer;
            timer.Stop();
        }
        #endregion

        #region public methods
        public async Task InitializeSocket(string server, string port, string ws, string userId)
        {
            if (string.IsNullOrEmpty(server))
                throw new ArgumentNullException(server);
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(userId);

            connectionId = userId;
            SetUrl(server, port, ws);
            await Initialize();
        }

        public async Task Initialize()
        {
            connection = new WebSocket(URL);
            Uri uri = new Uri(URL);
            if (uri.Scheme.ToLower()=="wss")
            {
                connection.SslConfiguration.EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12;
                connection.SslConfiguration.ServerCertificateValidationCallback =
                  (senderz, certificate, chain, sslPolicyErrors) => true;
            }
            connection.OnOpen += OnOpen;
            connection.OnError += OnError;
            connection.OnMessage += OnMessage;
            connection.OnClose += OnClose;

            await Connect();
        }

        public async Task Send(string method, string received, string message, List<Command> commands)
        {
            Transport transport = new Transport
            {
                Message = message,
                Method = method,
                TypeMessage = new TypeMessage
                {
                    id = connectionId,
                    type = "user",
                    commands = commands
                }
            };
            connection.Send(JsonConvert.SerializeObject(transport));
        }

        public async Task Disconnect()
        {
            connection.CloseAsync();
        }

        #endregion

        #region private methods
        private async void OpenConnectByTimer(object sender, ElapsedEventArgs e)
        {
            await Connect();
        }

        private void SetUrl(string server, string port, string ws)
        {
            URL = $"{server}:{port}/{ws}";
        }

        private async Task Connect()
        {
            connection.Connect();
            await Task.Delay(2000);

            Transport transport = new Transport
            {
                Message = string.Empty,
                Method = string.Empty,
                TypeMessage = new TypeMessage
                {
                    commands = new List<Command> { },
                    id = connectionId,
                    type = "service"
                }
            };
            connection.Send(JsonConvert.SerializeObject(transport));
        }
        #endregion

        #region Socket Event
        private void OnMessage(object sender, MessageEventArgs e)
        {
            IsConnected = true;
            OnMessageEvent?.Invoke(sender, e);
        }
        private void OnOpen(object sender, EventArgs e)
        {
            IsConnected = true;
            timer.Stop();
            Log.Information($"Connected to websocket by address: {URL}");
        }
        private void OnError(object sender, ErrorEventArgs e)
        {
            Log.Error($"Error conncecting to websocket by address: {URL} status: closed. Description: {e.Message}");
            IsConnected = false;
        }
        private void OnClose(object sender, CloseEventArgs e)
        {
            Log.Information($"Connect websocket was closed: {URL}");
            IsConnected = false;
            if (!timer.Enabled)
                timer.Start();
        }

        #endregion
    }
}