using KashkeshetCommon.Enum;
using KashkeshtWorkerServiceServer.Src.Models.ChatModel;
using KashkeshtWorkerServiceServer.Src.Models.ChatsModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace KashkeshtWorkerServiceServer.Src.Models
{
    public class MessageModel
    {
        public ChatMessageType MessageType { get; set; }

        public string Message { get; set; }

        public IClientModel ClientSender { get; set; }

        public DateTime DateSended { get; set; }
        public MessageModel(ChatMessageType messageType, string message, IClientModel clientSender, DateTime dateSended)
        {
            MessageType = messageType;
            Message = message;
            ClientSender = clientSender;
            DateSended = dateSended;
        }
    }
}
