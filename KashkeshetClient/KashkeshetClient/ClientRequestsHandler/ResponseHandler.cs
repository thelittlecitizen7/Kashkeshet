using KashkeshetClient.ClientRequestsHandler;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace ClientChat
{
    public class ResponseHandler : IResponseHandler
    {
        public object locker = new object();

        public string GetResponse(TcpClient client)
        {
            lock (locker)
            {
                NetworkStream nts = client.GetStream();
                byte[] tmpBuff = new byte[1024];
                int readOut = nts.Read(tmpBuff, 0, 1024);
                if (readOut > 0)
                {
                    return Encoding.ASCII.GetString(tmpBuff, 0, readOut);
                }
            }
            return null;

        }
    }
}
