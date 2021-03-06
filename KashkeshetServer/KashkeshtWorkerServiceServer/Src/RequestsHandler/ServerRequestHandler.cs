﻿using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

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

        public void SendDataMultiClients(List<TcpClient> clients, string data)
        {
            foreach (var client in clients)
            {
                Thread thread = new Thread(() => 
                {
                    if (client.Connected)
                    {
                        SendData(client, data);
                    }
                });
                thread.Start();
                
            }
        }
    }
}
