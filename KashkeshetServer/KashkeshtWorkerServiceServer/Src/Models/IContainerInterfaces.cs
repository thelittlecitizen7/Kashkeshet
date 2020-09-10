using KashkeshtWorkerServiceServer.Src.RequestsHandler;
using KashkeshtWorkerServiceServer.Src.ResponsesHandler;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace KashkeshtWorkerServiceServer.Src.Models
{
    public interface IContainerInterfaces
    {
        IServerRequestHandler RequestHandler { get; set; }

        IServerResponseHandler ResponseHandler { get; set; }

        ILogger<Worker> Logger { get; set; }
    }
}
