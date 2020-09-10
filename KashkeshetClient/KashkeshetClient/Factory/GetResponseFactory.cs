using KashkeshetClient.ServersHandler;
using KashkeshetCommon;
using KashkeshetCommon.Enum;
using KashkeshetCommon.Models.ChatData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KashkeshetClient.Factory
{
    public class GetResponseFactory
    {
        public string GetResponse(string response)
        {

            var obj = Utils.DeSerlizeObject<MainRequest>(response);
            switch (obj.RequestType)
            {
                case MessageType.NewChatMessage:
                    var model = Utils.DeSerlizeObject<NewChatMessage>(response);
                    return $"{DateTime.Now.ToString("MM/dd/yyyy")} : {model.From} sent : {model.Message} ";
                case MessageType.UserStatus:
                    var userStatusModel = Utils.DeSerlizeObject<StatusClientMessage>(response);
                    return $"{DateTime.Now.ToString("MM/dd/yyyy")} : {userStatusModel.From} {userStatusModel.StatusClient}";
                case MessageType.GetAllChats:
                    var allChatsResponse = Utils.DeSerlizeObject<AllChatsMessage>(response);
                    return ChatUtils.GetChatsResponse(allChatsResponse.Chats);
                case MessageType.GetAllUserConnected:
                    var allUsersResponse = Utils.DeSerlizeObject<AllUsersMessage>(response);
                    string allUsersStr = "";
                    allUsersResponse.Names.ForEach(n => allUsersStr += $"{n} {Environment.NewLine}");
                    return allUsersStr;
                case MessageType.SuccessResponse:
                    var successResponse = Utils.DeSerlizeObject<OkResponseMessage>(response);
                    return $"request Sucess : {successResponse.Message}";
                case MessageType.ErrorResponse:
                    var errorResponse = Utils.DeSerlizeObject<ErrorMessage>(response);
                    return $"request Failed : {errorResponse.Error}";
                case MessageType.HistoryChatMessages:
                    var chatMessageHistory = Utils.DeSerlizeObject<ChatMessageHistory>(response);
                    string msg = "";
                    chatMessageHistory.AllMessages.ToList().ForEach(m => msg += $"{m.Datetime.ToString("MM/dd/yyyy")} : {m.SenderName} sent : {m.Message} {Environment.NewLine}");
                    return msg;
                default:
                    return null;
            }
        }
    }
}
