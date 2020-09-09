using ClientChat;
using KashkeshetClient.ClientSocketHandler;
using KashkeshetClient.IOSystem;
using KashkeshetClient.Models;
using System;
using System.Net.Http.Headers;

namespace KashkeshetClient
{
    class Program
    {
        static void Main(string[] args)
        {

            var containerInterfaces = new ContainerInterfaces
            {
                RequestHandler = new RequestHandler(),
                ResponseHandler = new ResponseHandler(),
                SystemInput = new SystemInputHandler(),
                SystemOutput = new SystemOutputHandler()
            };
            Client client = new Client("127.0.0.1", 11111, containerInterfaces);
            client.Connect();

        }
    }
}
