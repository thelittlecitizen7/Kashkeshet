using KashkeshetClient.Models;
using KashkeshetClient.ServersHandler;
using KashkeshetCommon.Enum;
using KashkeshetCommon.Models.ChatData;
using MenuBuilder.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KashkeshetClient.MenuHandler.Options
{
    public class ExitChatOption : IOptions
    {
        private IContainerInterfaces _containerInterfaces;
        private ServerHandler _serverHandler;
        private string _clientname;
        public ExitChatOption(IContainerInterfaces containerInterfaces, IUser _user, ServerHandler serverHandler)
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


            _containerInterfaces.SystemOutput.Print("Please enter the group name you want to remove to | CLICK --stop for stop type OR exit for exit option-- EXIT");
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

            var body = new GroupChatMessageModel
            {
                RequestType = MessageType.ExitChat,
                lsUsers = new List<string>() { _clientname},
                GroupName = groupName
            };

            _serverHandler.UpdateChat(body);


        }



    }
}
