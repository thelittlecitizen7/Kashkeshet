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
                
                byte[] tmpBuff = new byte[client.ReceiveBufferSize];
                int readOut = nts.Read(tmpBuff, 0, client.ReceiveBufferSize);
                if (readOut > 0)
                {
                    return Encoding.ASCII.GetString(tmpBuff, 0, readOut);
                }
            }
            return null;

        }
    }
}
