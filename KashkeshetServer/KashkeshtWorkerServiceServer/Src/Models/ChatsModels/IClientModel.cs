using KashkeshtWorkerServiceServer.Src.Models.ChatModel;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace KashkeshtWorkerServiceServer.Src.Models.ChatsModels
{
    public interface IClientModel
    {
        string Name { get; set; }

        TcpClient Client { get; set; }

        bool Connected { get; set; }

        ChatModule CurrentConnectChat { get; set; }
    }
}
