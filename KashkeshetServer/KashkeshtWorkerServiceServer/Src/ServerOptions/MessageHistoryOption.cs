using KashkeshetCommon;
using KashkeshetCommon.Enum;
using KashkeshetCommon.Models.ChatData;
using KashkeshtWorkerServiceServer.Src.Models;
using KashkeshtWorkerServiceServer.Src.Models.ChatModel;
using KashkeshtWorkerServiceServer.Src.Models.ChatsModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace KashkeshtWorkerServiceServer.Src.ServerOptions
{
    public class MessageHistoryOption : IOption
    {
        private AllChatDetails _allChatDetails;

        private IClientModel _userClient { get; set; }


        public IContainerInterfaces _containerInterfaces { get; set; }

        public MessageHistoryOption(IClientModel userClient, AllChatDetails allChatDetails , IContainerInterfaces containerInterfaces)
        {
            _userClient = userClient;
            _allChatDetails = allChatDetails;
            _containerInterfaces = containerInterfaces;
        }

        public void Operation(MainRequest chatData)
        {
            var data  = chatData as ChatMessageHistory;

            var chat = _allChatDetails.GetChatById(data.ChatId);

            if (chat == null) {
                var errorBody = new ErrorMessage
                {
                    RequestType = MessageType.ErrorResponse,
                    Error = $"The chat with id {data.ChatId} not found"
                };
                _containerInterfaces.RequestHandler.SendData(_userClient.Client, Utils.SerlizeObject(errorBody));
                return;
            }

            List<MessageDetails> messages = new List<MessageDetails>();
            foreach (var message in chat.Messages)
            {
                var msg = new MessageDetails
                {
                    SenderName = message.ClientSender.Name,
                    Message = message.Message,
                    Datetime = message.DateSended,
                    MessageType = message.MessageType
                };
                messages.Add(msg);
            }



            var allChatMessages = new ChatMessageHistory
            {
                RequestType = MessageType.HistoryChatMessages,
                From = _userClient.Name,
                AllMessages = messages.ToArray()
            };
            string data5 = Utils.SerlizeObject(allChatMessages);
            Console.WriteLine(data5);
            _containerInterfaces.RequestHandler.SendData(_userClient.Client,Utils.SerlizeObject(allChatMessages));

            _containerInterfaces.Logger.LogInformation("Send all history of chat "+ data.ChatId);

            
        }
    }
}
