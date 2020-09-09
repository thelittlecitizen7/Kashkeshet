using KashkeshetCommon.Enum;
using KashkeshetCommon.Models.ChatData;
using KashkeshtWorkerServiceServer.Src.Models;
using KashkeshtWorkerServiceServer.Src.RequestsHandler;
using KashkeshtWorkerServiceServer.Src.ResponsesHandler;
using System;
using System.Linq;

namespace KashkeshtWorkerServiceServer.Src.ServerOptions.ManagerOptions
{
    public class AddAdminPermissionOption : IOption
    {
        private AllChatDetails _allChatDetails;

        private string Name { get; set; }

        private IServerRequestHandler _requestHandler;
        private IServerResponseHandler _responseHandler;
        public AddAdminPermissionOption(string name, AllChatDetails allChatDetails)
        {
            _requestHandler = new ServerRequestHandler();
            _responseHandler = new ServerResponseHandler();
            Name = name;
            _allChatDetails = allChatDetails;
        }
        public void Operation(MainRequest chatData)
        {
            var data = chatData as GroupChatMessageModel;
            var groupChat = _allChatDetails.GetGroupByName(data.GroupName);
            var client = _allChatDetails.GetClientByName(Name);
            if (!groupChat.IsClientManager(client))
            {
                var errorBody = new ErrorMessage
                {
                    RequestType = MessageType.ErrorResponse,
                    Error = $"The user name {Name} has not permission to give admin permission"
                };
                _requestHandler.SendData(client.Client, Utils.SerlizeObject(errorBody));
                return;
            }
            var alClientsToAdd = data.lsUsers.Where(c => groupChat.IsClientExistInChat(_allChatDetails.GetClientByName(c))).Select(u => _allChatDetails.GetClientByName(u)).ToList();
            groupChat.AddMultiManagrs(alClientsToAdd);
            Console.WriteLine($"Group users updated");
            var successBody = new OkResponseMessage
            {
                RequestType = MessageType.SuccessResponse,
                Message = $"Group {data.GroupName} users updated"
            };
            _requestHandler.SendData(client.Client, Utils.SerlizeObject(successBody));
        }
    }
}
