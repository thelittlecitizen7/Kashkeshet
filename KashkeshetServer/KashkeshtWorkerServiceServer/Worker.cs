using KashkeshtWorkerServiceServer.Src.Models;
using KashkeshtWorkerServiceServer.Src.RequestsHandler;
using KashkeshtWorkerServiceServer.Src.ResponsesHandler;
using KashkeshtWorkerServiceServer.Src.SocketsHandler;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace KashkeshtWorkerServiceServer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            IServerRequestHandler requestHandler = new ServerRequestHandler();
            IServerResponseHandler responseHandler = new ServerResponseHandler();


            IContainerInterfaces containerInterfaces = new ContainerInterfaces(requestHandler,responseHandler,_logger);
            ServerSocket serverSocket = new ServerSocket(11111, containerInterfaces);
            serverSocket.Listen();
           
        }
    }
}
