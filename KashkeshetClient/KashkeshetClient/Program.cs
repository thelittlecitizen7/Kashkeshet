using KashkeshetClient.ClientSocketHandler;
using System;
using System.Net.Http.Headers;

namespace KashkeshetClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Client client = new Client("127.0.0.1", 11111);
             client.Connect();

        }
    }
}
