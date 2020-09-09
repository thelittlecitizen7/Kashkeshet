using KashkeshetCommon.Enum;
using KashkeshtWorkerServiceServer.Src.Factory;
using KashkeshtWorkerServiceServer.Src.Models;
using KashkeshtWorkerServiceServer.Src.Models.ChatModel;
using KashkeshtWorkerServiceServer.Src.RequestsHandler;
using KashkeshtWorkerServiceServer.Src.ResponsesHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;

namespace KashkeshtWorkerServiceServer.Src.SocketsHandler
{
    public class SeverHandler : ISeverHandler
    {
        public object locker = new object();

        private TcpClient _client;

        private IServerRequestHandler _requestHandler;
        private IServerResponseHandler _responseHandler;

        private ClientModel _userClient { get; set; }

        public Thread ListenThread { get; set; }

        private AllChatDetails _allChatDetails;

        public SeverHandler(TcpClient client, AllChatDetails allChatDetails)
        {
            _allChatDetails = allChatDetails;
            _client = client;
            _requestHandler = new ServerRequestHandler();
            _responseHandler = new ServerResponseHandler();
        }
        public void Run()
        {
            string name = _responseHandler.GetResponse(_client);

            if (!_allChatDetails.IsClientExist(name))
            {
                _userClient = new ClientModel(name, _client);
                _allChatDetails.AddClient(_userClient);
                var globalChat = _allChatDetails.GetAllChatByType(ChatType.Globaly)[0];
                globalChat.AddClient(_userClient);

            }
            else
            {
                _userClient = _allChatDetails.GetClientByName(name);
            }
            Console.WriteLine($"Client with name : {name} connected to server");

            _userClient.Client = _client;
            _userClient.Connected = true;
            _userClient.LastStatusConnected = true;


            ListenThread = new Thread(() =>
            {
                ListenReciveMesages();
            });
            ListenThread.Start();
        }



        public ClientModel GetClient(string name)
        {
            var globalChat = _allChatDetails.GetAllChatByType(ChatType.Globaly)[0];
            return globalChat.Clients.FirstOrDefault(s => s.Name == name);

        }
        private void ListenReciveMesages()
        {
            while (true)
            {
                try
                {
                    string data = _responseHandler.GetResponse(_client);

                    OperationExecuteFactory operationExecuteFactory = new OperationExecuteFactory(_userClient.Name, _allChatDetails);

                    operationExecuteFactory.Execute(data);

                    Console.WriteLine($"Received from {_userClient.Name}: " + data);

                    if (data == "Close")
                    {
                        CloseSocket();
                        break;
                    }

                }
                catch (Exception e)
                {
                    CloseSocket();
                    break;
                }
            }
        }

        private void CloseSocket()
        {
            _userClient.Connected = false;
            _client.Close();
            Console.WriteLine($"User {_userClient.Name} disconnected");
        }

    }
}
