using KashkeshetClient.ServersHandler;
using KashkeshetCommon.Models.ChatData;
using MenuBuilder.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KashkeshetClient.MenuHandler.Options.ManagerOptions
{
    public class AddUserToChatOption : IOptions
    {
        private ServerHandler _serverHandler;
        private string _clientname;
        public AddUserToChatOption(string name, ServerHandler serverHandler)
        {
            _clientname = name;
            _serverHandler = serverHandler;
        }
        public void Operation()
        {
            Console.WriteLine("All chats group chats");
            List<ChatMessageModel> allChats =  _serverHandler.GetAllChatGroupModels();
            Console.WriteLine(GetChatsResponse(allChats));
            
            
            Console.WriteLine("Please enter the group name you want to add to | CLICK --stop for stop type OR exit for exit option-- EXIT");
            string groupName = Console.ReadLine();

            while (groupName != "stop")
            {
                if (groupName == "exit") 
                {
                    return;
                }
                if (IsvalidateGroupName(groupName, allChats))
                {
                    break;
                }
                Console.WriteLine("Please enter the group name you want to add to | CLICK --stop for stop type OR exit for exit option-- EXIT");
                groupName = Console.ReadLine();
            }



            Console.WriteLine("All connected users");
            string allConnectedUser = _serverHandler.GetAllUserConnected();
            Console.WriteLine(allConnectedUser);


            Console.WriteLine("Please enter which user you want to add to chat");
            string name = Console.ReadLine();

            List<string> userNames = new List<string>();
            while (name != "stop")
            {
                if (name == "exit")
                {
                    return;
                }
                bool IsNameValidate = ValidateName(allConnectedUser, name, userNames,allChats.First(c => c.GroupName == groupName));
                if (IsNameValidate)
                {
                    userNames.Add(name);
                }
                Console.WriteLine("Please enter the name you want to open chat with him | CLICK --stop-- EXIT");
                name = Console.ReadLine();
            }


            var body = new GroupChatMessageModel
            {
                RequestType = "AddUserToChat",
                lsUsers = userNames,
                GroupName = groupName
            };

            _serverHandler.UpdateChat(body);

        }

        private bool IsvalidateGroupName(string groupName , List<ChatMessageModel> chats) 
        {
            return chats.Any(c => c.GroupName == groupName);
        }

        private string GetChatsResponse(List<ChatMessageModel> chats)
        {
            string msg = "";
            foreach (var chat in chats)
            {
                string memebersStr = "|";
                foreach (var memeber in chat.Names)
                {
                    memebersStr += $" {memeber} |";
                }
                if (chat.GroupName != null)
                {
                    msg += $"{chat.ChatType.ToString()} with name {chat.GroupName} chat id : {chat.ChatId} , with memebers : {memebersStr} {Environment.NewLine}";
                }
                else
                {
                    msg += $"{chat.ChatType.ToString()} chat id : {chat.ChatId} , with memebers : {memebersStr} {Environment.NewLine}";
                }
            }
            return msg;
        }


        private bool ValidateName(string allConnectedUser, string name, List<string> userNamesToAdd,ChatMessageModel chat)
        {
            if (chat.Names.Contains(name)) 
            {
                Console.WriteLine($"The user {name} already exist in group");
                return false;
            }
            if (userNamesToAdd.Contains(name))
            {
                Console.WriteLine($"The user {name} already in user to add");
                return false;
            }
            if (name == _clientname)
            {
                Console.WriteLine($"You cannot create Group chat with yourself");
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
