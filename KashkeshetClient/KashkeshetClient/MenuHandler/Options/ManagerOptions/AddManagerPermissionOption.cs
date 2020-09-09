
using KashkeshetClient.Models;
using KashkeshetClient.ServersHandler;
using KashkeshetCommon.Enum;
using KashkeshetCommon.Models.ChatData;
using MenuBuilder.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KashkeshetClient.MenuHandler.Options.ManagerOptions
{
    public class AddManagerPermissionOption : IOptions
    {
        private IContainerInterfaces _containerInterfaces;
        private ServerHandler _serverHandler;
        private string _clientname;
        public AddManagerPermissionOption(IContainerInterfaces containerInterfaces, IUser _user, ServerHandler serverHandler)
        {
            _containerInterfaces = containerInterfaces;
            _clientname = _user.Name;
            _serverHandler = serverHandler;
        }
        public void Operation()
        {
            _containerInterfaces.SystemOutput.Print("All chats group chats");
            List<ChatMessageModel> allChats = _serverHandler.GetAllChatGroupModels();
            _containerInterfaces.SystemOutput.Print(_serverHandler.ParseChatsToString(allChats));


            _containerInterfaces.SystemOutput.Print("Please enter the group name you want to add to | CLICK --stop for stop type OR exit for exit option-- EXIT");
            string groupName = _containerInterfaces.SystemInput.StringInput();

            while (groupName != "stop")
            {
                if (groupName == "exit")
                {
                    return;
                }
                if (allChats.Any(c => c.GroupName == groupName))
                {
                    break;
                }
                _containerInterfaces.SystemOutput.Print("Please enter the group name you want to add to | CLICK --stop for stop type OR exit for exit option-- EXIT");
                groupName = _containerInterfaces.SystemInput.StringInput();
            }



            _containerInterfaces.SystemOutput.Print("All connected users");
            string allConnectedUser = _serverHandler.GetAllUserConnected();
            _containerInterfaces.SystemOutput.Print(allConnectedUser);


            _containerInterfaces.SystemOutput.Print("Please enter which user you want to give hime admin permission | CLICK --stop for stop type OR exit for exit option-- EXIT");
            string name = _containerInterfaces.SystemInput.StringInput();

            List<string> userNames = new List<string>();
            while (name != "stop")
            {
                if (name == "exit")
                {
                    return;
                }
                bool IsNameValidate = ValidateName(allConnectedUser, name, userNames, allChats.First(c => c.GroupName == groupName));
                if (IsNameValidate)
                {
                    userNames.Add(name);
                }
                _containerInterfaces.SystemOutput.Print("Please enter which user you want to give hime admin permission | CLICK --stop for stop type OR exit for exit option-- EXIT");
                name = _containerInterfaces.SystemInput.StringInput();
            }


            var body = new GroupChatMessageModel
            {
                RequestType = MessageType.AddAdminPermissions,
                lsUsers = userNames,
                GroupName = groupName
            };

            _serverHandler.UpdateChat(body);
        }

       


        private bool ValidateName(string allConnectedUser, string name, List<string> userNamesToAdd, ChatMessageModel chat)
        {
            if (!chat.Names.Contains(name))
            {
                _containerInterfaces.SystemOutput.Print($"The user {name} already exist in group");
                return false;
            }
            if (userNamesToAdd.Contains(name))
            {
                _containerInterfaces.SystemOutput.Print($"The user {name} already in user to add");
                return false;
            }
            if (name == _clientname)
            {
                _containerInterfaces.SystemOutput.Print($"You cannot create Group chat with yourself");
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
