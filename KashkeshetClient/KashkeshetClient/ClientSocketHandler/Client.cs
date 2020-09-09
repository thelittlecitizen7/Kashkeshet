using ClientChat;
using KashkeshetClient.ClientRequestsHandler;
using KashkeshetClient.MenuHandler;
using KashkeshetClient.Models;
using MenuBuilder;
using MenuBuilder.IO.Input;
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
    public class Client : IClient
    {
        public int Port { get; set; }
        public string Adress { get; set; }
        public string Name { get; set; }

        private IUser _user;

        private IContainerInterfaces _containerInterfaces;

        public Client(string adress, int port,IContainerInterfaces containerInterfaces)
        {
            Adress = IPAddress.Parse(adress).ToString();
            Port = port;
            _containerInterfaces = containerInterfaces;
        }


        public void Connect()
        {
            _containerInterfaces.SystemOutput.Print("Please enter your Name");
            string name = Console.ReadLine();
            Name = name;


            string hostname = Adress;
            var client = new TcpClient();
            client.Connect(hostname, Port);

            _user = new User(name,client);
            
            _containerInterfaces.SystemOutput.Print("Socket connected to");
            _containerInterfaces.RequestHandler.SendData(client, name);

            Menu menu = new Menu(_containerInterfaces,_user);
            menu.Run();
        }

    }
}
