using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace KashkeshetClient.Models
{
    public class User : IUser
    {
        public string Name { get; set; }

        public TcpClient Client { get; set; }

        public User(string name , TcpClient client)
        {
            Name = name;
            Client = client;
        }
    }
}
