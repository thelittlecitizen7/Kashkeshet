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
        private ServerHandler _serverHandler;
        private string _clientname;
        public ExitChatOption(string name, ServerHandler serverHandler)
        {
            _clientname = name;
            _serverHandler = serverHandler;
        }
        public void Operation()
        {
            Console.WriteLine("All chats group chats");
            List<ChatMessageModel> allChats = _serverHandler.GetAllChatGroupModels();
            Console.WriteLine(GetChatsResponse(allChats));


            Console.WriteLine("Please enter the group name you want to remove to | CLICK --stop for stop type OR exit for exit option-- EXIT");
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

            var body = new GroupChatMessageModel
            {
                RequestType = MessageType.ExitChat,
                lsUsers = new List<string>() { _clientname},
                GroupName = groupName
            };

            _serverHandler.UpdateChat(body);


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



        private bool IsvalidateGroupName(string groupName, List<ChatMessageModel> chats)
        {
            return chats.Any(c => c.GroupName == groupName);
        }
    }
}
