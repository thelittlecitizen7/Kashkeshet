using ClientChat;
using KashkeshetClient.Factory;
using KashkeshetCommon.Enum;
using KashkeshetCommon.Models.ChatData;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
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
            var dataChat = new MainRequest { RequestType = "GetAllChats" };
            string message = Utils.SerlizeObject(dataChat);
            _requestHandler.SendData(Client, message);
            var response = _responseHandler.GetResponse(Client);
            var responserStr = new GetResponseFactory().GetResponse(response);

            return responserStr;
        }

        public List<ChatMessageModel> GetAllChatGroupModels() 
        {
            var dataChat = new MainRequest { RequestType = "GetAllChats" };
            string message = Utils.SerlizeObject(dataChat);
            _requestHandler.SendData(Client, message);
            var response = _responseHandler.GetResponse(Client);
            var allChatsResponse = Utils.DeSerlizeObject<AllChatsMessage>(response);
            return allChatsResponse.Chats.Where(c => c.ChatType != ChatType.Private).ToList();

        }

        public string GetAllUserConnected()
        {
            var dataChat = new MainRequest { RequestType = "GetAllUserConnected" };
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
                RequestType = "InsertToChat",
                ChatId = chatId,
                From = Name
            };

            Thread thread = new Thread(() => { ListenAnswerTCP(Client); });
            thread.Start();


            dataChat.MessageChat = $"User {Name} connected to chat";
            _requestHandler.SendData(Client, Utils.SerlizeObject(dataChat));

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
