using KashkeshetClient.ServersHandler;
using MenuBuilder.Options;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace KashkeshetClient.MenuHandler.Options
{
    public class InsertToChatOption : IOptions
    {
        private ServerHandler _serverHandler { get; set; }
        public InsertToChatOption(ServerHandler serverHandler)
        {
            _serverHandler = serverHandler;
        }
        public void Operation()
        { 
            Console.WriteLine("Please enter the chat id you want to get in");
            string chatId = Console.ReadLine();
            _serverHandler.InsertToChat(chatId);
        }
    }
}
