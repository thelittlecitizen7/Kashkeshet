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
        private List<ChatModule> _allChats;

        private Dictionary<string, TcpClient> _allSockets;

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
                _allChatDetails.AddClient(new ClientModel(name,_client));
                
            }
            Console.WriteLine($"Client with name : {name} connected to server");

            var clientName = GetClient(name);
            
            if (clientName != null)
            {
                if (!clientName.Connected)
                {
                    var globalChat = _allChatDetails.GetAllChatByType(ChatType.Globaly)[0];
                    globalChat.RemoveClient(clientName);
                    globalChat.AddClient(clientName);
           
                 }
                
                _userClient = clientName;
                _userClient.Client = _client;
                _userClient.Connected = true;
                _userClient.LastStatusConnected = true;
            }
            else
            {
                var globalChat = _allChatDetails.GetAllChatByType(ChatType.Globaly)[0];
                _userClient = new ClientModel(name, _client);
                globalChat.AddClient(_userClient);
            }
            //_userClient.CurrentConnectChat = _allChatDetails.GetAllChatByType(ChatType.Globaly)[0];

            //SendToAll($"user {_userClient.Name} connect to chat");


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

                    OperationExecuteFactory operationExecuteFactory = new OperationExecuteFactory(_userClient.Name,_allChatDetails);

                    operationExecuteFactory.Execute(data);

                    Console.WriteLine($"Received from {_userClient.Name}: " + data);

                    if (data == "Close")
                    {
                        CloseSocket();
                        break;
                    }
                    //SendToAll(data);

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

        private void SendToAllGlobal(string message)
        {
            var globalChat = _allChatDetails.GetAllChatByType(ChatType.Globaly)[0];
            var allUserToSend = globalChat.Clients.Where(c => c.Name != _userClient.Name && c.Connected == true);

            foreach (var client in allUserToSend)
            {
                if (client.Client.Connected)
                {
                    _requestHandler.SendData(client.Client, $"client  {_userClient.Name} : {message}");
                }
            }
        }



        private void SendToAll(string message)
        {
            var globalChat = _allChatDetails.GetAllChatByType(ChatType.Globaly)[0];
            var allUserToSend = globalChat.Clients.Where(c => c.Name != _userClient.Name && c.Connected == true);
            
            foreach (var client in allUserToSend)
            {
                if (client.Client.Connected)
                {
                    _requestHandler.SendData(client.Client, $"client  {_userClient.Name} : {message}");
                }
            }
        }
    }
}
