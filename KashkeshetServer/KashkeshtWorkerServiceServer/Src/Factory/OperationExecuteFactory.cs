using KashkeshetCommon.Enum;
using KashkeshetCommon.Models.ChatData;
using KashkeshtWorkerServiceServer.Src.Models;
using KashkeshtWorkerServiceServer.Src.Models.ChatModel;
using KashkeshtWorkerServiceServer.Src.RequestsHandler;
using KashkeshtWorkerServiceServer.Src.ResponsesHandler;
using KashkeshtWorkerServiceServer.Src.ServerOptions;
using KashkeshtWorkerServiceServer.Src.ServerOptions.ManagerOptions;
using System;
using System.Collections.Generic;

namespace KashkeshtWorkerServiceServer.Src.Factory
{
    public class OperationExecuteFactory
    {
        public object locker = new object();
        private AllChatDetails _allChatDetails { get; set; }
        public string Name { get; set; }

        private IServerRequestHandler _requestHandler;
        private IServerResponseHandler _responseHandler;
        public OperationExecuteFactory(string name, AllChatDetails allChatDetails)
        {
            _requestHandler = new ServerRequestHandler();
            _responseHandler = new ServerResponseHandler();
            _allChatDetails = allChatDetails;
            Name = name;
        }
        public void Execute(string requestData)
        {
            var obj = Utils.DeSerlizeObject<MainRequest>(requestData);
            switch (obj.RequestType)
            {
                case MessageType.PrivateCreationChat:
                    var request = Utils.DeSerlizeObject<PrivateChatMessageModel>(requestData);
                    new PrivateChatCreatorOption(Name, _allChatDetails).Operation(request);
                    break;
                case MessageType.GetAllChats:
                    new GetAllChatOption(Name, _allChatDetails).Operation(obj);
                    break;

                case MessageType.InsertToChat:
                    var request3 = Utils.DeSerlizeObject<InsertToChatMessageModel>(requestData);
                    //ConnectedToChat(requestData);
                    new InsertToChatOption(_allChatDetails).Operation(request3);
                    break;

                case MessageType.GetAllUserConnected:
                    var request5 = Utils.DeSerlizeObject<MainRequest>(requestData);
                    new GetAllUserConnectedOption(Name, _allChatDetails).Operation(request5);
                    break;
                case MessageType.GroupCreationChat:
                    var request6 = Utils.DeSerlizeObject<GroupChatMessageModel>(requestData);
                    new GroupChatCreatorOption(Name, _allChatDetails).Operation(request6);
                    break;

                case MessageType.AddUserToChat:
                    var request7 = Utils.DeSerlizeObject<GroupChatMessageModel>(requestData);
                    new AddUserToGroupOption(Name, _allChatDetails).Operation(request7);
                    break;
                case MessageType.RemoveUserToChat:
                    var request8 = Utils.DeSerlizeObject<GroupChatMessageModel>(requestData);
                    new RemoveUserFromGroupOption(Name, _allChatDetails).Operation(request8);
                    break;
                case MessageType.AddAdminPermissions:
                    var request9 = Utils.DeSerlizeObject<GroupChatMessageModel>(requestData);
                    new AddAdminPermissionOption(Name, _allChatDetails).Operation(request9);
                    break;
                case MessageType.RemoveAdminPermissions:
                    var request10 = Utils.DeSerlizeObject<GroupChatMessageModel>(requestData);
                    new RemoveAdminPermissionOption(Name, _allChatDetails).Operation(request10);
                    break;
                case MessageType.ExitChat:
                    var request11 = Utils.DeSerlizeObject<GroupChatMessageModel>(requestData);
                    new ExitChatOption(Name, _allChatDetails).Operation(request11);
                    break;//ExitChat

            }
        }

        private void ConnectedToChat(string requestData)
        {

            string response = requestData;
            InsertToChatMessageModel request = Utils.DeSerlizeObject<InsertToChatMessageModel>(response);
            var clientSneder = _allChatDetails.GetClientByName(Name);
            ChatModule foundChat = _allChatDetails.GetChatById(request.ChatId);
            lock (locker)
            {
                _allChatDetails.UpdateCurrentChat(clientSneder, foundChat);
            }
            Console.WriteLine(clientSneder.CurrentConnectChat);

            while (true)
            {
                request = Utils.DeSerlizeObject<InsertToChatMessageModel>(response);

                var model = new NewChatMessage
                {
                    RequestType = MessageType.NewChatMessage,
                    From = request.From,
                    Message = request.MessageChat
                };
                string message = Utils.SerlizeObject(model);
                if (request.MessageChat == "exit")
                {
                    lock (locker)
                    {
                        _allChatDetails.UpdateCurrentChat(clientSneder, null);
                        //clientSneder.CurrentConnectChat = null;
                    }
                    model.Message = $"The user {model.From} disconnect from this chat";
                    SendAll(foundChat, request, $"User");
                    return;
                }

                SendAll(foundChat, request, message);

                foundChat.AddMessage(new MessageModel(ChatMessageType.TextMessage, message, clientSneder, DateTime.Now));
                response = _responseHandler.GetResponse(_allChatDetails.GetClientByName(Name).Client);
            }
        }

        private void SendAll(ChatModule foundChat, InsertToChatMessageModel request, string message)
        {
            var allUserToSend = GetAllConnectedToSend(foundChat, request);

            foreach (var client in allUserToSend)
            {
                if (client.Client.Connected)
                {
                    _requestHandler.SendData(client.Client, message);
                }
            }
        }
        private List<ClientModel> GetAllConnectedToSend(ChatModule chat, InsertToChatMessageModel request)
        {
            //var allUserToSend = chat.Clients.Where(c => c.Name != request.From && c.Connected == true && c.CurrentConnectChat.ChatId == chat.ChatId);
            List<ClientModel> ls = new List<ClientModel>();
            foreach (var client in chat.Clients)
            {
                if ((client.Name != request.From) && (client.Connected == true))
                {
                    if (client.CurrentConnectChat != null)
                    {
                        if (client.CurrentConnectChat.ChatId == chat.ChatId)
                        {
                            lock (locker)
                            {
                                ls.Add(client);
                            }
                        }
                    }

                }
            }

            return ls;

        }


    }
}
