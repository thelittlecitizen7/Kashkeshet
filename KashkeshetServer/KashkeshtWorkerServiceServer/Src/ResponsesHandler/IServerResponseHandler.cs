using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace KashkeshtWorkerServiceServer.Src.ResponsesHandler
{
    public interface IServerResponseHandler
    {
        string GetResponse(TcpClient client);
    }
}
