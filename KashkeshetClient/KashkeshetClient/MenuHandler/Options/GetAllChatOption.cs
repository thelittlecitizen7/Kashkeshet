using KashkeshetClient.Models;
using KashkeshetClient.ServersHandler;
using MenuBuilder.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace KashkeshetClient.MenuHandler.Options
{
    public class GetAllChatOption : IOptions
    {
        private ServerHandler _serverHandler { get; set; }

        private IContainerInterfaces _containerInterfaces;

        public GetAllChatOption(IContainerInterfaces containerInterfaces, ServerHandler serverHandler)
        {
            _serverHandler = serverHandler;
            _containerInterfaces = containerInterfaces;
        }
        public void Operation()
        {
            _containerInterfaces.SystemOutput.Print("All chats : ");
            string allChats = _serverHandler.GetAllChats();
            _containerInterfaces.SystemOutput.Print(allChats);
        }
    }
}
