using KashkeshetClient.ClientRequestsHandler;
using MenuBuilder;
using MenuBuilder.IO.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace KashkeshetClient.Models
{
    public class ContainerInterfaces : IContainerInterfaces
    {
        public IRequestHandler RequestHandler { get ; set ; }
        public IResponseHandler ResponseHandler { get ; set ; }
        public ISystemInput SystemInput { get ; set ; }
        public IOutputSystem SystemOutput { get ; set ; }
    }
}
