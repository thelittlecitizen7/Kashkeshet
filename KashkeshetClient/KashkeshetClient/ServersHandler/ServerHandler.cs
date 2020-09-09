using ClientChat;
using KashkeshetClient.Factory;
using KashkeshetClient.Models;
using KashkeshetCommon;
using KashkeshetCommon.Enum;
using KashkeshetCommon.Models.ChatData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;

namespace KashkeshetClient.ServersHandler
{
    public class ServerHandler
    {
        
        
        private IUser _user;

        private IContainerInterfaces _containerInterfaces;
        public ServerHandler(IContainerInterfaces containerInterfaces,IUser user)
        {
            _containerInterfaces = containerInterfaces;
            _user = user;
        }

        public string ParseChatsToString(List<ChatMessageModel> chats)
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


        public string GetAllChats()
        {
            var dataChat = new MainRequest { RequestType = MessageType.GetAllChats };
            string message = Utils.SerlizeObject(dataChat);
            _containerInterfaces.RequestHandler.SendData(_user.Client, message);
            var response = _containerInterfaces.ResponseHandler.GetResponse(_user.Client);
            var responserStr = new GetResponseFactory().GetResponse(response);

            return responserStr;
        }

        public List<ChatMessageModel> GetAllChatGroupModels() 
        {
            var dataChat = new MainRequest { RequestType = MessageType.GetAllChats };
            string message = Utils.SerlizeObject(dataChat);
            _containerInterfaces.RequestHandler.SendData(_user.Client, message);
            var response = _containerInterfaces.ResponseHandler.GetResponse(_user.Client);
            var allChatsResponse = Utils.DeSerlizeObject<AllChatsMessage>(response);
            return allChatsResponse.Chats.Where(c => c.ChatType != ChatType.Private).ToList();

        }

        public string GetAllUserConnected()
        {
            var dataChat = new MainRequest { RequestType = MessageType.GetAllUserConnected};
            string message = Utils.SerlizeObject(dataChat);
            _containerInterfaces.RequestHandler.SendData(_user.Client, message);
            var response = _containerInterfaces.ResponseHandler.GetResponse(_user.Client);
            var responserStr = new GetResponseFactory().GetResponse(response);
            return responserStr;
        }

        public void CreateChat(MainRequest request)
        {
            string message = Utils.SerlizeObject(request);
            _containerInterfaces.RequestHandler.SendData(_user.Client, message);
        }


        public void UpdateChat(MainRequest request)
        {
            string message = Utils.SerlizeObject(request);
            _containerInterfaces.RequestHandler.SendData(_user.Client, message);
            var response = _containerInterfaces.ResponseHandler.GetResponse(_user.Client);
            var responserStr = new GetResponseFactory().GetResponse(response);
            Console.WriteLine(responserStr);
        }

        public void InsertToChat(string chatId)
        {
            var dataChat = new InsertToChatMessageModel
            {
                RequestType = MessageType.InsertToChat,
                ChatId = chatId,
                From = _user.Name
            };

            dataChat.MessageChat = $"User {_user.Name} connected to chat";
            _containerInterfaces.RequestHandler.SendData(_user.Client, Utils.SerlizeObject(dataChat));

            string data = _containerInterfaces.ResponseHandler.GetResponse(_user.Client);
            var successSwithChat = Utils.DeSerlizeObject<MainRequest>(data);

            if (successSwithChat.RequestType == MessageType.ErrorResponse) 
            {
                var error = Utils.DeSerlizeObject<ErrorMessage>(data);
                Console.WriteLine(error.Error);
                return;
            }
            var successChat = Utils.DeSerlizeObject<OkResponseMessage>(data);
            Console.WriteLine(successChat.Message);

            Thread thread = new Thread(() => { ListenAnswerTCP(_user.Client); });
            thread.Start();

            while (true)
            {
                Console.WriteLine("Please enter message to send");
                string inputMessage = Console.ReadLine();
                Console.WriteLine($"You : {inputMessage}");
                dataChat.MessageChat = inputMessage;

                _containerInterfaces.RequestHandler.SendData(_user.Client, Utils.SerlizeObject(dataChat));
                if (inputMessage == "exit")
                {
                    break;
                }
            }
            
        }
        private void ListenAnswerTCP(TcpClient client)
        {
            while (true)
            {
                string response = _containerInterfaces.ResponseHandler.GetResponse(client);

                if (response != null)
                {
                    string responserStr = new GetResponseFactory().GetResponse(response);
                    if (responserStr.Contains($"{_user.Name} sent : exit")) 
                    {
                        return;
                    }
                    Console.WriteLine(responserStr);
                }
                
               
            }
        }

    }
}
