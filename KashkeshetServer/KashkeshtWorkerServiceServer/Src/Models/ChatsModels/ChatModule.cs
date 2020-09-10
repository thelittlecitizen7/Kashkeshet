using KashkeshetCommon.Enum;
using KashkeshtWorkerServiceServer.Src.Models.ChatsModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KashkeshtWorkerServiceServer.Src.Models.ChatModel
{
    public class ChatModule
    {
        public List<IClientModel> Clients { get; set; }

        public List<MessageModel> Messages { get; set; }

        public ChatType ChatType { get; set; }

        public string ChatId { get; set; }


        public ChatModule(ChatType chatType)
        {
            ChatId = Guid.NewGuid().ToString();
            Clients = new List<IClientModel>();
            Messages = new List<MessageModel>();
            ChatType = chatType;
        }

        public IClientModel GetClient(string name)
        {
            return Clients.FirstOrDefault(c => c.Name == name);
        }

        public void AddClient(IClientModel client)
        {
            if (!IsClientExistInChat(client))
            {
                Clients.Add(client);
            }
        }


        public List<string> GetAllNamesInChat()
        {
            List<string> names = new List<string>();
            foreach (var user in Clients)
            {
                names.Add(user.Name);
            }
            return names;
        }


        public virtual void RemoveClient(IClientModel client)
        {
            if (IsClientExistInChat(client))
            {
                Clients.Remove(client);
            }
        }

        public virtual void RemoveMultiClients(List<IClientModel> clients)
        {
            foreach (var client in clients)
            {
                RemoveClient(client);
            }
        }


        public bool IsClientExistInChat(IClientModel client)
        {
            return Clients.Any(c => c.Name == client.Name);
        }

        public bool IsClientExistInChat(string name)
        {
            return Clients.Any(c => c.Name == name);
        }



        public void AddMessage(MessageModel message)
        {
            Messages.Add(message);
        }

        public void RemoveMessage(MessageModel message)
        {
            Messages.Remove(message);
        }
    }
}
