using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace KashkeshtWorkerServiceServer.Src.Models.ChatModel
{
    public class ClientModel
    {
        public string Name { get; set; }

        public TcpClient Client { get; set; }

        public bool LastStatusConnected { get; set; }
        public bool Connected { get; set; }

        public ChatModule CurrentConnectChat { get; set; }

        public ClientModel(string name,TcpClient client)
        {
            CurrentConnectChat = null;
            Name = name;
            Client = client;
            Connected = Client.Connected;
        }
    }
}
