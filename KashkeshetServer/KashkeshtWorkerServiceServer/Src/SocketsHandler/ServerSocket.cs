using KashkeshtWorkerServiceServer.Src.Models;
using KashkeshtWorkerServiceServer.Src.Models.ChatModel;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Sockets;
using System.Threading;

namespace KashkeshtWorkerServiceServer.Src.SocketsHandler
{
    public class ServerSocket : IServer
    {
        public TcpListener Listener { get; set; }
        public int Port { get; set; }

        private ChatModule _globalChatModel { get; set; }

        public AllChatDetails AllChatDetails { get; set; }

        private IContainerInterfaces _containerInterfaces;

        public ServerSocket(int port , IContainerInterfaces containerInterfaces)
        {
            _containerInterfaces = containerInterfaces;
            _globalChatModel = new GlobalChat();
            AllChatDetails = new AllChatDetails();
            AllChatDetails.AddChat(_globalChatModel);

            Port = port;
            Listener = new TcpListener(Port);
        }


        public void Close()
        {
            Listener.Stop();
        }

        public void Listen()
        {
            try
            {
                Listener.Start();
                _containerInterfaces.Logger.LogInformation("Server Start listen");
                while (true)
                {
                    
                    Thread thread = new Thread(() =>
                    {
                        TcpClient client = Listener.AcceptTcpClient();
                        SeverHandler socketHandler = new SeverHandler(client, AllChatDetails,_containerInterfaces);
                        socketHandler.Run();
                    });
                    thread.Start();
                }
            }
            catch (Exception e)
            {
                Listener.Stop();
            }
        }


    }
}
