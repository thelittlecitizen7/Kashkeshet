using ClientChat;
using KashkeshetClient.MenuHandler;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace KashkeshetClient.ClientSocketHandler
{
    public class Client
    {
        public int Port { get; set; }
        public string Adress { get; set; }
        public string Name { get; set; }
        private RequestHandler _requestHandler { get; set; }

        private ResponseHandler _responseHandler { get; set; }

        public Client(string adress, int port)
        {
            Adress = IPAddress.Parse(adress).ToString();
            Port = port;
            _requestHandler = new RequestHandler();
            _responseHandler = new ResponseHandler();
        }


        public void Connect()
        {
            Console.WriteLine("Please enter your Name");
            string name = Console.ReadLine();
            Name = name;

            string hostname = Adress;
            var client = new TcpClient();
            client.Connect(hostname, Port);            
            Console.WriteLine("Socket connected to");
            _requestHandler.SendData(client, name);

            Menu menu = new Menu();
            menu.Run(Name,client);

            while (true)
            {
                Thread thread = new Thread(() => { ListenAnswerTCP(client); });
                thread.Start();

                while (true)
                {
                    Console.WriteLine("Please enter Input");
                    string input = Console.ReadLine();

                    
                    _requestHandler.SendData(client, input);
                    Console.WriteLine($"You : {input}");
                }
            }
        }



        private void ListenAnswerTCP(TcpClient client)
        {
            while (true)
            {
                var response = _responseHandler.GetResponse(client);

                if (response != null)
                {
                    Console.WriteLine($"from server : {response}");
                }
            }
        }
    }
}
