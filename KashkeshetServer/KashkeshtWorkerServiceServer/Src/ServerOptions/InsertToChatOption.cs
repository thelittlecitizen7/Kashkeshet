using KashkeshetCommon;
using KashkeshetCommon.Enum;
using KashkeshetCommon.Models.ChatData;
using KashkeshtWorkerServiceServer.Src.Models;
using KashkeshtWorkerServiceServer.Src.Models.ChatModel;
using KashkeshtWorkerServiceServer.Src.RequestsHandler;
using KashkeshtWorkerServiceServer.Src.ResponsesHandler;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KashkeshtWorkerServiceServer.Src.ServerOptions
{
    public class InsertToChatOption : IOption
    {
        private object locker = new object();
        private AllChatDetails _allChatDetails { get; set; }
        private IServerRequestHandler _requestHandler;
        private IServerResponseHandler _responseHandler;

        public InsertToChatOption(AllChatDetails allChatDetails)
        {
            _requestHandler = new ServerRequestHandler();
            _responseHandler = new ServerResponseHandler();
            _allChatDetails = allChatDetails;
        }

        public void Operation(MainRequest request)
        {

            var data = request as InsertToChatMessageModel;
            ChatModule foundChat = _allChatDetails.GetChatById(data.ChatId);
            SendToAll(request, foundChat);

        }

        private void SendToAll(MainRequest request, ChatModule chat)
        {

            var data = request as InsertToChatMessageModel;
            var clientSneder = _allChatDetails.GetClientByName(data.From);

            if (!ValidateFirstConnectionToChat(clientSneder, chat, data)) 
            {
                return;
            }

            try
            {
                lock (locker)
                {
                    _allChatDetails.UpdateCurrentChat(clientSneder, chat);
                }

                var model = new NewChatMessage
                {
                    RequestType = MessageType.NewChatMessage,
                    From = request.From,
                    Message = data.MessageChat
                };
                string message = Utils.SerlizeObject(model);
                if (data.MessageChat == "exit")
                {
                    ExitFromChat(clientSneder, chat, request);
                    return;
                }

                SendToAll(chat, request, message);


                chat.AddMessage(new MessageModel(ChatMessageType.TextMessage, message, clientSneder, DateTime.Now));
            }
            catch (Exception e)
            {
                ExitFromChat(clientSneder, chat, request);
                Console.WriteLine("Error" + e.Message);
            }
        }

        private bool ValidateFirstConnectionToChat(ClientModel clientSneder,ChatModule chat, InsertToChatMessageModel data)
        {
            if (clientSneder.CurrentConnectChat == null)
            {
                if (chat == null)
                {
                    var errorBody = new ErrorMessage
                    {
                        RequestType = MessageType.ErrorResponse,
                        Error = $"There is not chat with id {data.ChatId}"
                    };
                    _requestHandler.SendData(clientSneder.Client, Utils.SerlizeObject(errorBody));
                    return false;
                }
                else
                {
                    var successBody = new OkResponseMessage
                    {
                        RequestType = MessageType.SuccessResponse,
                        Message = $"user {data.From} in chat with {data.ChatId}"
                    };
                    _requestHandler.SendData(clientSneder.Client, Utils.SerlizeObject(successBody));
                    return true;
                }
            }
            return true;
        }

        private void ExitFromChat(ClientModel clientSneder, ChatModule chat, MainRequest request)
        {
            lock (locker)
            {
                _allChatDetails.UpdateCurrentChat(clientSneder, null);
            }
            var model = new NewChatMessage
            {
                RequestType = MessageType.NewChatMessage,
                From = request.From
            };

            model.Message = $"The user {clientSneder.Name} disconnect from server";
            SendToAll(chat, request, Utils.SerlizeObject(model));
            model.Message = $"exit";
            _requestHandler.SendData(clientSneder.Client, Utils.SerlizeObject(model));
        }


        private void SendToAll(ChatModule chat, MainRequest request, string message)
        {
            var allUserToSend = GetAllConnectedToSend(chat, request);
            foreach (var client in allUserToSend)
            {
                if (client.Client.Connected)
                {
                    _requestHandler.SendData(client.Client, message);
                }
            }
        }


        private List<ClientModel> GetAllConnectedToSend(ChatModule chat, MainRequest requestData)
        {
            var request = requestData as InsertToChatMessageModel;
            List<ClientModel> ls = new List<ClientModel>();
            foreach (var client in chat.Clients)
            {
                if ((client.Name != request.From) && (client.Connected == true))
                {
                    if (client.CurrentConnectChat != null)
                    {
                        if (client.CurrentConnectChat.ChatId == chat.ChatId)
                        {
                            ls.Add(client);

                        }
                    }
                }
            }

            return ls;

        }




    }
}
