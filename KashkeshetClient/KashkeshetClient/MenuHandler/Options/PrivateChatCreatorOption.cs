using KashkeshetClient.Models;
using KashkeshetClient.ServersHandler;
using KashkeshetCommon.Enum;
using KashkeshetCommon.Models.ChatData;
using MenuBuilder.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KashkeshetClient.MenuHandler.Options
{
    public class PrivateChatCreatorOption : IOptions
    {
        private IContainerInterfaces _containerInterfaces;
        private ServerHandler _serverHandler;
        private string _clientname;
        public PrivateChatCreatorOption(IContainerInterfaces containerInterfaces, IUser _user, ServerHandler serverHandler)
        {
            _clientname = _user.Name;
            _serverHandler = serverHandler;
            _containerInterfaces = containerInterfaces;
        }
        public void Operation()
        {
            _containerInterfaces.SystemOutput.Print("All connected users");
            string allConnectedUser = _serverHandler.GetAllUserConnected();
            _containerInterfaces.SystemOutput.Print(allConnectedUser);

            _containerInterfaces.SystemOutput.Print("Please enter the name you want to open chat with him");
            string name = _containerInterfaces.SystemInput.StringInput();

            if (name == _clientname) 
            {
                _containerInterfaces.SystemOutput.Print($"You cannot create private chat with yourself");
                return;
            }

            if (!allConnectedUser.Contains(name)) 
            {
                _containerInterfaces.SystemOutput.Print($"The user {name} is not in user list");
                return;
            }

            var body = new PrivateChatMessageModel
            {
                RequestType = MessageType.PrivateCreationChat,
                lsUsers = new List<string>() { name }
            };

            _serverHandler.CreateChat(body);

        }
    }
}
