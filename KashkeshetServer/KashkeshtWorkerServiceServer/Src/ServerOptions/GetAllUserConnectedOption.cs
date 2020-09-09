using KashkeshetCommon.Models.ChatData;
using KashkeshtWorkerServiceServer.Src.Models;
using KashkeshtWorkerServiceServer.Src.RequestsHandler;
using KashkeshtWorkerServiceServer.Src.ResponsesHandler;
using System.Collections.Generic;
using System.Linq;

namespace KashkeshtWorkerServiceServer.Src.ServerOptions
{
    public class GetAllUserConnectedOption : IOption
    {
        private AllChatDetails _allChatDetails { get; set; }

        private IServerRequestHandler _requestHandler;
        private IServerResponseHandler _responseHandler;

        private string _name;

        public GetAllUserConnectedOption(string name, AllChatDetails allChatDetails)
        {
            _requestHandler = new ServerRequestHandler();
            _responseHandler = new ServerResponseHandler();
            _allChatDetails = allChatDetails;
            _name = name;
        }

        public void Operation(MainRequest chatData)
        {
            List<string> ls = _allChatDetails.GetAllUsers().Select(u => u.Name).ToList();
            var body = new AllUsersMessage
            { 
                From = _name,
                RequestType = "GetAllUserConnected",
                Names = ls
            };
            var client = _allChatDetails.GetClientByName(_name).Client;
            string message = Utils.SerlizeObject(body);
            _requestHandler.SendData(client, message);
        }
    }
}
