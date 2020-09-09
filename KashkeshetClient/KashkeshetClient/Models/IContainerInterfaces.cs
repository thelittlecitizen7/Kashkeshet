using KashkeshetClient.ClientRequestsHandler;
using MenuBuilder;
using MenuBuilder.IO.Input;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace KashkeshetClient.Models
{
    public interface IContainerInterfaces
    {
        IRequestHandler RequestHandler { get; set; }

        IResponseHandler ResponseHandler { get; set; }

        ISystemInput SystemInput { get; set; }
        IOutputSystem SystemOutput { get; set; }
    }
}
