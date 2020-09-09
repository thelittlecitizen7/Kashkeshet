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
        private ServerHandler _serverHandler;
        private string _clientname;
        public PrivateChatCreatorOption(string name ,ServerHandler serverHandler)
        {
            _clientname = name;
            _serverHandler = serverHandler;
        }
        public void Operation()
        {
            Console.WriteLine("All connected users");
            string allConnectedUser = _serverHandler.GetAllUserConnected();
            Console.WriteLine(allConnectedUser);

            Console.WriteLine("Please enter the name you want to open chat with him");
            string name = Console.ReadLine();

            if (name == _clientname) 
            {
                Console.WriteLine($"You cannot create private chat with yourself");
                return;
            }

            if (!allConnectedUser.Contains(name)) 
            {
                Console.WriteLine($"The user {name} is not in user list");
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
