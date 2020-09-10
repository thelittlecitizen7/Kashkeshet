using ClientChat;
using KashkeshetClient.Factory;
using KashkeshetClient.Models;
using KashkeshetCommon;
using KashkeshetCommon.Enum;
using KashkeshetCommon.Models.ChatData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;

namespace KashkeshetClient.ServersHandler
{
    public class ServerHandler
    {
        private IUser _user;

        private IContainerInterfaces _containerInterfaces;
        public ServerHandler(IContainerInterfaces containerInterfaces, IUser user)
        {

            _containerInterfaces = containerInterfaces;
            _user = user;
        }

        public string SendCommand(string command)
        {
            var dataChat = new CommandMessage { RequestType = MessageType.CommandMessage, Command = command, From = _user.Name };
            string message = Utils.SerlizeObject(dataChat);
            _containerInterfaces.RequestHandler.SendData(_user.Client, message);

            var transactionResponseStr = _containerInterfaces.ResponseHandler.GetResponse(_user.Client);
            var transactionResponse = new GetResponseFactory().GetResponse(transactionResponseStr);

            _containerInterfaces.SystemOutput.Print(transactionResponse);

            var enumValueNumber = (int)MessageType.ErrorResponse;
            if (transactionResponseStr.Contains(enumValueNumber.ToString())) 
            {
                return null;
            }


            var responseCommandStr = _containerInterfaces.ResponseHandler.GetResponse(_user.Client);
            var responserComand = new GetResponseFactory().GetResponse(responseCommandStr);
            return responserComand;
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
            var dataChat = new MainRequest { RequestType = MessageType.GetAllUserConnected };
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

        public string GetAllOldMessages(string chatId)
        {
            var oldMessages = new ChatMessageHistory
            {
                RequestType = MessageType.HistoryChatMessages,
                ChatId = chatId,
                From = _user.Name
            };
            string oldMessagesStr = Utils.SerlizeObject(oldMessages);
            _containerInterfaces.RequestHandler.SendData(_user.Client, oldMessagesStr);
            string data = _containerInterfaces.ResponseHandler.GetResponse(_user.Client);

            var responseStr = new GetResponseFactory().GetResponse(data);
            if (responseStr == string.Empty)
            {
                return null;
            }
            return responseStr;

        }


        public void InsertToChat(string chatId)
        {
            var allOldMessages = GetAllOldMessages(chatId);
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


            if (allOldMessages != null)
            {
                _containerInterfaces.SystemOutput.Print(allOldMessages);
            }


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
