using KashkeshetCommon.Enum;
using KashkeshtWorkerServiceServer.Src.Models.ChatModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KashkeshtWorkerServiceServer.Src.Models.ChatsModels
{
    public class GroupChat : ChatModule
    {
        public string GroupName { get; set; }
        public string ChatName { get; set; }

        public List<ClientModel> Managers { get; set; }

        public GroupChat(string groupName) : base(ChatType.Group)
        {
            GroupName = groupName;
            Managers = new List<ClientModel>();
        }

        public List<string> GetAllManagersNames() 
        {
            return Managers.Select(m => m.Name).ToList();
        }

        public void AddMultiClient(List<ClientModel> clients)
        {
            foreach (var client in clients)
            {
                base.AddClient(client);
            }
        }

        public void ChangeGroupName(string groupName) 
        {
            GroupName = groupName;
        }

        public bool IsClientManager(ClientModel client) 
        {
            return Managers.Any(m => m.Name == client.Name && base.IsClientExistInChat(client));
        }

        public void AddManager(ClientModel client) 
        {
            if (!IsClientManager(client)) 
            {
                Managers.Add(client);
            }
        }

        public void AddMultiManagrs(List<ClientModel> clients) 
        {
            foreach (var client in clients)
            {
                AddManager(client);
            }
        }

        public void RemoveManager(ClientModel client)
        {
            if (IsClientManager(client))
            {
                Managers.Remove(client);
            }
        }


        public void RemoveMultiManagrs(List<ClientModel> clients)
        {
            foreach (var client in clients)
            {
                RemoveManager(client);
            }
        }

        public override void RemoveClient(ClientModel client)
        {
            if (IsClientExistInChat(client))
            {
                base.RemoveClient(client);
                RemoveManager(client);
            }
        }

        public override void RemoveMultiClients(List<ClientModel> clients)
        {
            foreach (var client in clients)
            {
                RemoveClient(client);
            }
        }
    }
}
