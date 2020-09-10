using KashkeshetCommon.Models.ChatData;
using KashkeshtWorkerServiceServer.Src.Models;
using KashkeshtWorkerServiceServer.Src.Models.ChatModel;
using KashkeshtWorkerServiceServer.Src.Models.ChatsModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace KashkeshtWorkerServiceServer.Src.ServerOptions
{
    public class GroupChatCreatorOption : IOption
    {
        private AllChatDetails _allChatDetails;

        private IClientModel _userClient;

        private IContainerInterfaces _containerInterfaces;
        public GroupChatCreatorOption(IClientModel userClient, AllChatDetails allChatDetails , IContainerInterfaces containerInterfaces)
        {
            _userClient = userClient;
            _containerInterfaces = containerInterfaces;
            
            _allChatDetails = allChatDetails;
        }


        public void Operation(MainRequest chatData)
        {
           var data =  chatData as GroupChatMessageModel;
            GroupChat newGroupChat = new GroupChat(data.GroupName);
            
            List<IClientModel> clients = new List<IClientModel>();
            clients.Add(_userClient);
            if (data.lsUsers.Count ==  0) 
            {
                return;
            }
            newGroupChat.AddManager(_userClient);
            newGroupChat.AddClient(_userClient);
            foreach (var clientName in data.lsUsers)
            {
                if (_allChatDetails.IsClientExist(clientName))
                {
                    IClientModel client = _allChatDetails.GetClientByName(clientName);
                    clients.Add(client);
                    newGroupChat.AddClient(client);
                }
            }

            if (!_allChatDetails.IsExistChatWithName(data.GroupName)) 
            {
                _allChatDetails.AddChat(newGroupChat);
                _containerInterfaces.Logger.LogInformation($"Group {data.GroupName} added");
            }
        }
    }
}
