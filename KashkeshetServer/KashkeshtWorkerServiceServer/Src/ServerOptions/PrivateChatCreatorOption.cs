using KashkeshetCommon.Enum;
using KashkeshetCommon.Models.ChatData;
using KashkeshtWorkerServiceServer.Src.Models;
using KashkeshtWorkerServiceServer.Src.Models.ChatModel;
using System.Collections.Generic;

namespace KashkeshtWorkerServiceServer.Src.ServerOptions
{
    public class PrivateChatCreatorOption : IOption
    {
        private AllChatDetails _allChatDetails;

        private string Name { get; set; }
        public PrivateChatCreatorOption(string name, AllChatDetails allChatDetails)
        {
            Name = name;
            _allChatDetails = allChatDetails;
        }


        public void Operation(MainRequest chatData)
        {
            var data = chatData as PrivateChatMessageModel;
            ChatModule newChat = new PrivateChat();
            ClientModel senerClient = _allChatDetails.GetClientByName(Name);
            newChat.AddClient(senerClient);

            List<ClientModel> clients = new List<ClientModel>();
            clients.Add(senerClient);

            foreach (var clientName in data.lsUsers)
            {
                if (_allChatDetails.IsClientExist(clientName))
                {
                    ClientModel client = new ClientModel(clientName, _allChatDetails.GetClientByName(clientName).Client);
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
