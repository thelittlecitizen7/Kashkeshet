using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace KashkeshetClient.ClientRequestsHandler
{
    public interface IRequestHandler
    {
        void SendData(TcpClient client, string data);

    }
}
