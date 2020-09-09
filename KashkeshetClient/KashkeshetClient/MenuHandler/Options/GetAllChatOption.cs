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
        public GetAllChatOption(ServerHandler serverHandler)
        {
            _serverHandler = serverHandler;
        }
        public void Operation()
        {
            Console.WriteLine("All chats : ");
            string allChats = _serverHandler.GetAllChats();
            Console.WriteLine(allChats);
        }
    }
}
