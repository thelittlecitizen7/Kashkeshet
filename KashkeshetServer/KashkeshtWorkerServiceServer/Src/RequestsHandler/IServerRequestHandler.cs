using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace KashkeshtWorkerServiceServer.Src.RequestsHandler
{
    public interface IServerRequestHandler
    {
        void SendData(TcpClient client, string data);
    }
}
