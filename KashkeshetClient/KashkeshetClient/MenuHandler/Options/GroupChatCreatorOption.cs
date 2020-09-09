using KashkeshetClient.Models;
using KashkeshetClient.ServersHandler;
using KashkeshetCommon.Enum;
using KashkeshetCommon.Models.ChatData;
using MenuBuilder.Options;
using System;
using System.Collections.Generic;

namespace KashkeshetClient.MenuHandler.Options
{
    public class GroupChatCreatorOption : IOptions
    {
        private IContainerInterfaces _containerInterfaces;
        private ServerHandler _serverHandler;
        private string _clientname;
        public GroupChatCreatorOption(IContainerInterfaces containerInterfaces, IUser _user, ServerHandler serverHandler)
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

            _containerInterfaces.SystemOutput.Print("Please enter the name Of the group you want to open");
            string groupName = _containerInterfaces.SystemInput.StringInput();


            _containerInterfaces.SystemOutput.Print("Please enter the name you want to open chat with him | CLICK --stop-- EXIT");
            string name = _containerInterfaces.SystemInput.StringInput();


            List<string> userNames = new List<string>();
            while (name != "stop")
            {
                bool IsNameValidate = ValidateName(allConnectedUser, name, userNames);
                if (IsNameValidate)
                {
                    userNames.Add(name);
                }
                _containerInterfaces.SystemOutput.Print("Please enter the name you want to open chat with him | CLICK --stop-- EXIT");
                name = _containerInterfaces.SystemInput.StringInput();
            }


            var body = new GroupChatMessageModel
            {
                RequestType = MessageType.GroupCreationChat,
                lsUsers = userNames,
                GroupName = groupName
            };

            _serverHandler.CreateChat(body);

        }

        public bool ValidateName(string allConnectedUser, string name, List<string> userNamesToAdd)
        {
            if (userNamesToAdd.Contains(name))
            {
                _containerInterfaces.SystemOutput.Print($"The user {name} already in user to add");
                return false;
            }
            if (name == _clientname)
            {
                _containerInterfaces.SystemOutput.Print($"You cannot create private chat with yourself");
                return false; ;
            }

            if (!allConnectedUser.Contains(name))
            {
                _containerInterfaces.SystemOutput.Print($"The user {name} is not in user list");
                return false;
            }

            return true;
        }

    }
}
