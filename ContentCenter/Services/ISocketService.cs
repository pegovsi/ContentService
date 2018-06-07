using ContentCenter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSocketSharp;

namespace ContentCenter.Services
{
    public interface ISocketService
    {
        WebSocket connection { get; }
        Task InitializeSocket(string server, string port, string ws, string userId);
        event EventHandler<MessageEventArgs> OnMessageEvent;
        Task Disconnect();
        Task Send(string method, string received, string message, List<Command> commands);
        bool IsConnected { get; }
    }
}
