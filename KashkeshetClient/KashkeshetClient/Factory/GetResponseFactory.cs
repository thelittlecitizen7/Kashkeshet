using KashkeshetCommon.Models.ChatData;
using System;
using System.Collections.Generic;
using System.Text;

namespace KashkeshetClient.Factory
{
    public class GetResponseFactory
    {
        public string GetResponse(string response)
        {
            var obj = Utils.DeSerlizeObject<MainRequest>(response);
            switch (obj.RequestType.ToString())
            {
                case "NewChatMessage":
                    var model = Utils.DeSerlizeObject<NewChatMessage>(response);
                    return $"{DateTime.Now.ToString("MM/dd/yyyy")} : {model.From} sent : {model.Message} ";
                case "UserStatus":
                    var userStatusModel = Utils.DeSerlizeObject<StatusClientMessage>(response);
                    return $"{DateTime.Now.ToString("MM/dd/yyyy")} : {userStatusModel.From} {userStatusModel.StatusClient}";
                case "GetAllChats":
                    var allChatsResponse = Utils.DeSerlizeObject<AllChatsMessage>(response);
                    return GetChatsResponse(allChatsResponse);
                case "GetAllUserConnected":
                    var allUsersResponse = Utils.DeSerlizeObject<AllUsersMessage>(response);
                    string allUsersStr = "";
                    allUsersResponse.Names.ForEach(n => allUsersStr += $"{n} {Environment.NewLine}");
                    return allUsersStr;
                case "SuccessResponse":
                    var successResponse = Utils.DeSerlizeObject<OkResponseMessage>(response);
                    return $"request Sucess : {successResponse.Message}";
                case "ErrorResponse":
                    var errorResponse = Utils.DeSerlizeObject<ErrorMessage>(response);
                    return $"request Failed : {errorResponse.Error}";
                default:
                    return null;
            }
        }

        private string GetChatsResponse(AllChatsMessage obj)
        {
            string msg = "";
            foreach (var chat in obj.Chats)
            {
                string memebersStr = "|";
                foreach (var memeber in chat.Names)
                {
                    memebersStr += $" {memeber} |";
                }
                if (chat.GroupName != null)
                {
                    msg += $"{chat.ChatType.ToString()} with name {chat.GroupName} chat id : {chat.ChatId} , with memebers : {memebersStr} {Environment.NewLine}";
                }
                else
                {
                    msg += $"{chat.ChatType.ToString()} chat id : {chat.ChatId} , with memebers : {memebersStr} {Environment.NewLine}";
                }
            }
            return msg;
        }
    }
}
