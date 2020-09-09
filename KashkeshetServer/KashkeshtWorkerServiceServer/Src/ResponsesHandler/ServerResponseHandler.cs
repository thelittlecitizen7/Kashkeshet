using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace KashkeshtWorkerServiceServer.Src.ResponsesHandler
{
    public class ServerResponseHandler : IServerResponseHandler
    {
        public ServerResponseHandler()
        {
        }
        public string GetResponse(TcpClient client)
        {
            NetworkStream nwStream = client.GetStream();
            byte[] buffer = new byte[client.ReceiveBufferSize];

            int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);

            string data = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            return data;
        }
    }
}
