using KashkeshetCommon;
using KashkeshetCommon.Enum;
using KashkeshetCommon.Models.ChatData;
using KashkeshtWorkerServiceServer.Src.Models;
using KashkeshtWorkerServiceServer.Src.Models.ChatsModels;
using KashkeshtWorkerServiceServer.Src.RequestsHandler;
using KashkeshtWorkerServiceServer.Src.ResponsesHandler;
using System.Collections.Generic;
using System.Linq;

namespace KashkeshtWorkerServiceServer.Src.ServerOptions
{
    public class GetAllUserConnectedOption : IOption
    {
        private AllChatDetails _allChatDetails { get; set; }

        private IContainerInterfaces _containerInterfaces;

        private IClientModel _userClient;

        public GetAllUserConnectedOption(IClientModel userClient, AllChatDetails allChatDetails , IContainerInterfaces containerInterfaces)
        {
            _containerInterfaces = containerInterfaces;
            _allChatDetails = allChatDetails;
            _userClient = userClient;
        }

        public void Operation(MainRequest chatData)
        {
            List<string> ls = _allChatDetails.GetAllUsers().Select(u => u.Name).ToList();
            var body = new AllUsersMessage
            { 
                From = _userClient.Name,
                RequestType = MessageType.GetAllUserConnected,
                Names = ls
            };
            var client = _allChatDetails.GetClientByName(_userClient.Name).Client;
            string message = Utils.SerlizeObject(body);
            _containerInterfaces.RequestHandler.SendData(client, message);
        }
    }
}
