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
        private ServerHandler _serverHandler;
        private string _clientname;
        public GroupChatCreatorOption(string name, ServerHandler serverHandler)
        {
            _clientname = name;
            _serverHandler = serverHandler;
        }
        public void Operation()
        {
            Console.WriteLine("All connected users");
            string allConnectedUser = _serverHandler.GetAllUserConnected();
            Console.WriteLine(allConnectedUser);

            Console.WriteLine("Please enter the name Of the group you want to open");
            string groupName = Console.ReadLine();


            Console.WriteLine("Please enter the name you want to open chat with him | CLICK --stop-- EXIT");
            string name = Console.ReadLine();


            List<string> userNames = new List<string>();
            while (name != "stop") 
            {
                bool IsNameValidate = ValidateName(allConnectedUser,name,userNames);
                if (IsNameValidate) 
                {
                    userNames.Add(name);
                }
                Console.WriteLine("Please enter the name you want to open chat with him | CLICK --stop-- EXIT");
                name = Console.ReadLine();
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
                Console.WriteLine($"The user {name} already in user to add");
                return false;
            }
            if (name == _clientname)
            {
                Console.WriteLine($"You cannot create private chat with yourself");
                return false; ;
            }

            if (!allConnectedUser.Contains(name))
            {
                Console.WriteLine($"The user {name} is not in user list");
                return false;
            }

            return true;
        }
      
    }
}
