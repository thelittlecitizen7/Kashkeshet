using KashkeshetCommon.Enum;
using KashkeshetCommon.Models.ChatData;
using KashkeshtWorkerServiceServer.Src.Models;
using KashkeshtWorkerServiceServer.Src.Models.ChatModel;
using KashkeshtWorkerServiceServer.Src.Models.ChatsModels;
using System.Collections.Generic;

namespace KashkeshtWorkerServiceServer.Src.ServerOptions
{
    public class PrivateChatCreatorOption : IOption
    {
        private AllChatDetails _allChatDetails;

        private IClientModel _userClient { get; set; }

        public PrivateChatCreatorOption(IClientModel userClient, AllChatDetails allChatDetails)
        {
            _userClient = userClient;
            _allChatDetails = allChatDetails;
        }


        public void Operation(MainRequest chatData)
        {
            var data = chatData as PrivateChatMessageModel;
            ChatModule newChat = new PrivateChat();
            newChat.AddClient(_userClient);

            List<IClientModel> clients = new List<IClientModel>();
            clients.Add(_userClient);

            foreach (var clientName in data.lsUsers)
            {
                if (_allChatDetails.IsClientExist(clientName))
                {
                    IClientModel client = _allChatDetails.GetClientByName(clientName);
                    clients.Add(client);
                    newChat.AddClient(client);
                }
            }
            if (!_allChatDetails.IsExistChatWithSamePeaple(clients,ChatType.Private))
            {
                _allChatDetails.AddChat(newChat);
            }
        }
    }
}
