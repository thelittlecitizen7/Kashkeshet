using KashkeshetClient.Models;
using KashkeshetClient.ServersHandler;
using MenuBuilder.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace KashkeshetClient.MenuHandler.Options
{
    public class InsertToChatOption : IOptions
    {
        private IContainerInterfaces _containerInterfaces;
        private ServerHandler _serverHandler { get; set; }
        public InsertToChatOption(IContainerInterfaces containerInterfaces,ServerHandler serverHandler)
        {
            _containerInterfaces = containerInterfaces;
            _serverHandler = serverHandler;
        }

        public void Operation()
        {
            _containerInterfaces.SystemOutput.Print("Please enter the chat id you want to get in");
            string chatId = _containerInterfaces.SystemInput.StringInput();
            _serverHandler.InsertToChat(chatId);
        }
    }
}
