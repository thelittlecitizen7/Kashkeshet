using KashkeshetCommon.Enum;
using KashkeshtWorkerServiceServer.Src.Factory;
using KashkeshtWorkerServiceServer.Src.Models;
using KashkeshtWorkerServiceServer.Src.Models.ChatModel;
using KashkeshtWorkerServiceServer.Src.Models.ChatsModels;
using KashkeshtWorkerServiceServer.Src.RequestsHandler;
using KashkeshtWorkerServiceServer.Src.ResponsesHandler;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;

namespace KashkeshtWorkerServiceServer.Src.SocketsHandler
{
    public class SeverHandler : ISeverHandler
    {

        private TcpClient _client;

        private IClientModel _userClient { get; set; }

        private AllChatDetails _allChatDetails;

        private IContainerInterfaces _containerInterfaces;

        public SeverHandler(TcpClient client, AllChatDetails allChatDetails , IContainerInterfaces containerInterfaces)
        {
            _allChatDetails = allChatDetails;
            _client = client;
            _containerInterfaces = containerInterfaces;
        }
        public void Run()
        {

            StartClientConnection();

            Thread ListenThread = new Thread(() =>
            {
                ListenReciveMesages();
            });
            ListenThread.Start();
        }

        private void StartClientConnection()
        {
            string name = _containerInterfaces.ResponseHandler.GetResponse(_client);

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
            _containerInterfaces.Logger.LogInformation($"Client with name : {name} connected to server");

            _userClient.Client = _client;
            _userClient.Connected = true;
        }


        private void ListenReciveMesages()
        {
            while (true)
            {
                try
                {
                    string data = _containerInterfaces.ResponseHandler.GetResponse(_client);

                    OperationExecuteFactory operationExecuteFactory = new OperationExecuteFactory(_userClient, _allChatDetails,_containerInterfaces);

                    operationExecuteFactory.Execute(data);

                    _containerInterfaces.Logger.LogInformation($"Received from {_userClient.Name}: " + data);

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
