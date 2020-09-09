using KashkeshetCommon.Enum;
using KashkeshtWorkerServiceServer.Src.Models.ChatModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace KashkeshtWorkerServiceServer.Src.Models
{
    public class MessageModel
    {
        public MessageType MessageType { get; set; }

        public string Message { get; set; }

        public ClientModel ClientSender { get; set; }

        public DateTime DateSended { get; set; }
        public MessageModel(MessageType messageType, string message, ClientModel clientSender, DateTime dateSended)
        {
            MessageType = messageType;
            Message = message;
            ClientSender = clientSender;
            DateSended = dateSended;
        }
    }
}
