using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace KashkeshetClient.Models
{
    public interface IUser
    {
        string Name { get; set; }

        TcpClient Client { get; set; }
    }
}
