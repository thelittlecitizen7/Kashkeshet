using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace KashkeshtWorkerServiceServer.Src.RequestsHandler
{
    public class ServerRequestHandler : IServerRequestHandler
    {
        public ServerRequestHandler()
        {
        }
        public void SendData(TcpClient client, string data)
        {
            NetworkStream nts = client.GetStream();
            
            byte[] bytesName = Encoding.ASCII.GetBytes(data);

            string inputBytesName = Encoding.UTF8.GetString(bytesName, 0, bytesName.Length);

            nts.Write(Encoding.ASCII.GetBytes(inputBytesName), 0, inputBytesName.Length);
        }
    }
}
