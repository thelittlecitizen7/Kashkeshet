using KashkeshetCommon.Models.ChatData;
using System;
using System.Collections.Generic;
using System.Text;

namespace KashkeshetClient.ServersHandler
{
    public class ChatUtils
    {
        public static string GetChatsResponse(List<ChatMessageModel> chats)
        {
            string msg = "";
            foreach (var chat in chats)
            {
                string memebersStr = "|";
                foreach (var memeber in chat.Names)
                {
                    if ((chat.GroupName != null) && (chat.AdminUsersNames != null))
                    {
                        if (chat.AdminUsersNames.Contains(memeber))
                        {
                            memebersStr += $" {memeber} - admin |";
                        }
                        else
                        {
                            memebersStr += $" {memeber} |";
                        }
                    }
                    else
                    {
                        memebersStr += $" {memeber} |";
                    }
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
    }
}
