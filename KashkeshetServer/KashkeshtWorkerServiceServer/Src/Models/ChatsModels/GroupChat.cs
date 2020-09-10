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

        public List<IClientModel> Managers { get; set; }

        public GroupChat(string groupName) : base(ChatType.Group)
        {
            GroupName = groupName;
            Managers = new List<IClientModel>();
        }

        public List<string> GetAllManagersNames() 
        {
            return Managers.Select(m => m.Name).ToList();
        }

        public void AddMultiClient(List<IClientModel> clients)
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

        public bool IsClientManager(IClientModel client) 
        {
            return Managers.Any(m => m.Name == client.Name && base.IsClientExistInChat(client));
        }

        public void AddManager(IClientModel client) 
        {
            if (!IsClientManager(client)) 
            {
                Managers.Add(client);
            }
        }

        public void AddMultiManagrs(List<IClientModel> clients) 
        {
            foreach (var client in clients)
            {
                AddManager(client);
            }
        }

        public void RemoveManager(IClientModel client)
        {
            if (IsClientManager(client))
            {
                Managers.Remove(client);
            }
        }


        public void RemoveMultiManagrs(List<IClientModel> clients)
        {
            foreach (var client in clients)
            {
                RemoveManager(client);
            }
        }

        public override void RemoveClient(IClientModel client)
        {
            if (IsClientExistInChat(client))
            {
                base.RemoveClient(client);
                RemoveManager(client);
            }
        }

        public override void RemoveMultiClients(List<IClientModel> clients)
        {
            foreach (var client in clients)
            {
                RemoveClient(client);
            }
        }
    }
}
