using ClientChat;
using KashkeshetClient.Factory;
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
        

        public object locker = new object();
        public static bool KeepAliveThread = true;
        private RequestHandler _requestHandler { get; set; }

        private ResponseHandler _responseHandler { get; set; }

        public TcpClient Client { get; set; }

        public string Name { get; set; }
        public ServerHandler(string name, TcpClient client)
        {
            Client = client;
            _requestHandler = new RequestHandler();
            _responseHandler = new ResponseHandler();
            Name = name;
        }


        public string GetAllChats()
        {
            var dataChat = new MainRequest { RequestType = MessageType.GetAllChats };
            string message = Utils.SerlizeObject(dataChat);
            _requestHandler.SendData(Client, message);
            var response = _responseHandler.GetResponse(Client);
            var responserStr = new GetResponseFactory().GetResponse(response);

            return responserStr;
        }

        public AllChatsMessage GetAllChatModels() 
        {
            var dataChat = new MainRequest { RequestType = MessageType.GetAllChats };
            string message = Utils.SerlizeObject(dataChat);
            _requestHandler.SendData(Client, message);
            var response = _responseHandler.GetResponse(Client);
            return Utils.DeSerlizeObject<AllChatsMessage>(response);
        }

        public List<ChatMessageModel> GetAllChatGroupModels() 
        {
            var dataChat = new MainRequest { RequestType = MessageType.GetAllChats };
            string message = Utils.SerlizeObject(dataChat);
            _requestHandler.SendData(Client, message);
            var response = _responseHandler.GetResponse(Client);
            var allChatsResponse = Utils.DeSerlizeObject<AllChatsMessage>(response);
            return allChatsResponse.Chats.Where(c => c.ChatType != ChatType.Private).ToList();

        }

        public string GetAllUserConnected()
        {
            var dataChat = new MainRequest { RequestType = MessageType.GetAllUserConnected};
            string message = Utils.SerlizeObject(dataChat);
            _requestHandler.SendData(Client, message);
            var response = _responseHandler.GetResponse(Client);
            var responserStr = new GetResponseFactory().GetResponse(response);
            return responserStr;
        }

        public void CreateChat(MainRequest request)
        {
            string message = Utils.SerlizeObject(request);
            _requestHandler.SendData(Client, message);
        }


        public void UpdateChat(MainRequest request)
        {
            string message = Utils.SerlizeObject(request);
            _requestHandler.SendData(Client, message);
            var response = _responseHandler.GetResponse(Client);
            var responserStr = new GetResponseFactory().GetResponse(response);
            Console.WriteLine(responserStr);
        }

        public void InsertToChat(string chatId)
        {
            var dataChat = new InsertToChatMessageModel
            {
                RequestType = MessageType.InsertToChat,
                ChatId = chatId,
                From = Name
            };

            

            dataChat.MessageChat = $"User {Name} connected to chat";
            _requestHandler.SendData(Client, Utils.SerlizeObject(dataChat));

            string data = _responseHandler.GetResponse(Client);
            var successSwithChat = Utils.DeSerlizeObject<MainRequest>(data);

            if (successSwithChat.RequestType == MessageType.ErrorResponse) 
            {
                var error = Utils.DeSerlizeObject<ErrorMessage>(data);
                Console.WriteLine(error.Error);
                return;
            }
            var successChat = Utils.DeSerlizeObject<OkResponseMessage>(data);
            Console.WriteLine(successChat.Message);

            Thread thread = new Thread(() => { ListenAnswerTCP(Client); });
            thread.Start();

            while (true)
            {
                Console.WriteLine("Please enter message to send");
                string inputMessage = Console.ReadLine();
                Console.WriteLine($"You : {inputMessage}");
                dataChat.MessageChat = inputMessage;

                _requestHandler.SendData(Client, Utils.SerlizeObject(dataChat));
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
                string response = _responseHandler.GetResponse(client);

                if (response != null)
                {
                    string responserStr = new GetResponseFactory().GetResponse(response);
                    if (responserStr.Contains($"{Name} sent : exit")) 
                    {
                        return;
                    }
                    Console.WriteLine(responserStr);
                }
                
               
            }
        }

    }
}
