using KashkeshetCommon.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KashkeshtWorkerServiceServer.Src.Models.ChatModel
{
    public class ChatModule
    {
        public List<ClientModel> Clients { get; set; }

        public List<MessageModel> Messages { get; set; }

        public ChatType ChatType { get; set; }

        public string ChatId { get; set; }
        public Dictionary<ClientModel,int> MapNumberOfEntries { get; set; }


        public ChatModule(ChatType chatType)
        {
            MapNumberOfEntries = new Dictionary<ClientModel, int>();
            ChatId = Guid.NewGuid().ToString();
            Clients = new List<ClientModel>();
            Messages = new List<MessageModel>();
            ChatType = chatType;
        }

        public ClientModel GetClient(string name) 
        {
            return Clients.FirstOrDefault(c => c.Name == name);
        }

        public void AddClient(ClientModel client)
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


        public virtual void RemoveClient(ClientModel client)
        {
            if (IsClientExistInChat(client))
            {
                Clients.Remove(client);
            }
        }

        public virtual void RemoveMultiClients(List<ClientModel> clients)
        {
            foreach (var client in clients)
            {
                RemoveClient(client);
            }
        }


        public bool IsClientExistInChat(ClientModel client)
        {
            return Clients.Any(c => c.Name == client.Name);
        }

        public bool IsClientExistInChat(string name)
        {
            return Clients.Any(c => c.Name == name);
        }

        public void GetInsideChat(ClientModel client) 
        {
            if (MapNumberOfEntries.ContainsKey(client))
            {
                MapNumberOfEntries[client]++;
            }
            else
            {
                MapNumberOfEntries.Add(client, 0);
            }
        }

        public int GetAmountClinetConnection(ClientModel client) 
        {
            return MapNumberOfEntries[client];
        }

        public void SetAmountConnection(int value, ClientModel client) 
        {
            MapNumberOfEntries[client] = value;
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
