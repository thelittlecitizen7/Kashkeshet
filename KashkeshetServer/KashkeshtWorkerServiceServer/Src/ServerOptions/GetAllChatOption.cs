using KashkeshetCommon;
using KashkeshetCommon.Enum;
using KashkeshetCommon.Models.ChatData;
using KashkeshtWorkerServiceServer.Src.Models;
using KashkeshtWorkerServiceServer.Src.Models.ChatsModels;
using KashkeshtWorkerServiceServer.Src.RequestsHandler;
using KashkeshtWorkerServiceServer.Src.ResponsesHandler;
using System;
using System.Collections.Generic;

namespace KashkeshtWorkerServiceServer.Src.ServerOptions
{
    public class GetAllChatOption : IOption
    {
        private AllChatDetails _allChatDetails { get; set; }

        private IServerRequestHandler _requestHandler;
        private IServerResponseHandler _responseHandler;

        private string _name;

        public GetAllChatOption(string name, AllChatDetails allChatDetails)
        {
            _requestHandler = new ServerRequestHandler();
            _responseHandler = new ServerResponseHandler();
            _allChatDetails = allChatDetails;
            _name = name;
        }

        public void Operation(MainRequest request)
        {
            var allChatsMessageModel = new AllChatsMessage
            {
                RequestType = MessageType.GetAllChats,
                Chats = new List<ChatMessageModel>()
            };
            var allChats = _allChatDetails.GetAllChatThatClientExist(_name);
            foreach (var chat in allChats)
            {
                allChatsMessageModel.Chats.Add(new ChatMessageModel
                {
                    ChatId = chat.ChatId,
                    Names = chat.GetAllNamesInChat(),
                    ChatType = chat.ChatType,
                    GroupName = (chat.GetType() == typeof(GroupChat)) ? ((GroupChat)chat).GroupName : null,
                    AdminUsersNames = (chat.GetType() == typeof(GroupChat)) ? ((GroupChat)chat).GetAllManagersNames() : null
                });
            }
            string msg = Utils.SerlizeObject(allChatsMessageModel);
            _requestHandler.SendData(_allChatDetails.GetClientByName(_name).Client, msg);
        }
    }
}
