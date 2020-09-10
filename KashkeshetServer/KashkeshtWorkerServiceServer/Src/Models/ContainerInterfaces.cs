using KashkeshtWorkerServiceServer.Src.RequestsHandler;
using KashkeshtWorkerServiceServer.Src.ResponsesHandler;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace KashkeshtWorkerServiceServer.Src.Models
{
    public class ContainerInterfaces : IContainerInterfaces
    {
        public IServerRequestHandler RequestHandler { get ; set ; }
        public IServerResponseHandler ResponseHandler { get ; set ; }
        public ILogger<Worker> Logger { get ; set ; }

        public ContainerInterfaces(IServerRequestHandler requestHandler , IServerResponseHandler responseHandler, ILogger<Worker> logger)
        {
            RequestHandler = requestHandler;
            ResponseHandler = responseHandler;
            Logger = logger;
        }
    }
}
