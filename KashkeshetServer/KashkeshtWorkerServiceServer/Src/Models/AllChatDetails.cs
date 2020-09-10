using KashkeshetCommon.Enum;
using KashkeshtWorkerServiceServer.Src.Models.ChatModel;
using KashkeshtWorkerServiceServer.Src.Models.ChatsModels;
using System.Collections.Generic;
using System.Linq;

namespace KashkeshtWorkerServiceServer.Src.Models
{
    public class AllChatDetails
    {
        public List<ChatModule> AllChats { get; set; }

        public List<IClientModel> AllClients { get; set; }
        
        public AllChatDetails()
        {
            AllChats = new List<ChatModule>();
            AllClients = new List<IClientModel>();
        }

        public bool IsExistChatWithSamePeaple(List<IClientModel> users,ChatType chatType) 
        {
            var allChats = AllChats.Where(c => c.ChatType == chatType).ToList();
            if (allChats.Count() == 0) 
            {
                return false;
            }
            foreach (var chat in allChats)
            {
                foreach (var client in users)
                {
                    if (!chat.IsClientExistInChat(client)) 
                    {
                        return false;
                    }
                }
                
            }
            return true;
        }

        public GroupChat GetGroupByName(string groupName) 
        {
            var allGroupChats = AllChats.Where(c => c.ChatType == ChatType.Group).ToList();
            return (GroupChat)allGroupChats.FirstOrDefault(g => ((GroupChat)g).GroupName == groupName);

        }

        public bool IsExistChatWithName(string groupName) 
        {
            var allGroupChats = AllChats.Where(c => c.ChatType == ChatType.Group).ToList();
            return allGroupChats.Any(g => ((GroupChat)g).GroupName == groupName);
        } 

        public bool IsClientExist(string name) 
        {
            return AllClients.Any(c => c.Name == name);
        }

        public void UpdateCurrentChat(IClientModel clientModel,ChatModule chat) 
        {
            var clientFromClients = GetClientByName(clientModel.Name);
            var clientFromChat = GetClientByNameFromChat(clientModel.Name);
            if (chat != null)
            {
                var clientFromChat2 = chat.GetClient(clientModel.Name);
                if (clientFromChat2 != null)
                {
                    clientFromChat2.CurrentConnectChat = chat;
                }
            }

            clientFromClients.CurrentConnectChat = chat;
            clientFromChat.CurrentConnectChat = chat;
        }

        public ChatModule GetChatById(string chatId) 
        {
            return AllChats.FirstOrDefault(chat => chat.ChatId == chatId);
        }

        public List<IClientModel> GetAllUsers() 
        {
            return AllClients;
        }

        public List<ChatModule> GetAllChatThatClientExist(string name) 
        {
            return AllChats.Where(c => c.IsClientExistInChat(name)).ToList();
        }


        public List<ChatModule> GetAllChatByType(ChatType chatType) 
        {
            return AllChats.Where(c => c.ChatType == chatType).ToList();
        }

        public IClientModel GetClientByName(string name) 
        {
            return AllClients.FirstOrDefault(c => c.Name == name);
        }

        public IClientModel GetClientByNameFromChat(string name)
        {
            foreach (var chat in AllChats)
            {
                foreach (var client in chat.Clients)
                {
                    if (client.Name == name) 
                    {
                        return client; 
                    }
                }
            }
            return null;
        }

        public void AddClient(IClientModel client) 
        {
            AllClients.Add(client);
        }

        public void AddChat(ChatModule chat) 
        {
            AllChats.Add(chat);
        }


        
    }
}
