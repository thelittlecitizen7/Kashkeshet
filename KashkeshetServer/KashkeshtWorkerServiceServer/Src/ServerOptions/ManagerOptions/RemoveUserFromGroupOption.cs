using KashkeshetCommon.Models.ChatData;
using KashkeshtWorkerServiceServer.Src.Models;
using KashkeshtWorkerServiceServer.Src.RequestsHandler;
using KashkeshtWorkerServiceServer.Src.ResponsesHandler;
using System.Linq;

namespace KashkeshtWorkerServiceServer.Src.ServerOptions.ManagerOptions
{
    public class RemoveUserFromGroupOption : IOption
    {
        private AllChatDetails _allChatDetails;

        private string Name { get; set; }

        private IServerRequestHandler _requestHandler;
        private IServerResponseHandler _responseHandler;
        public RemoveUserFromGroupOption(string name, AllChatDetails allChatDetails)
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
                    RequestType = "ErrorResponse",
                    Error = $"The user name {Name} cannot remove user to group beacuse he has not permission"
                };
                _requestHandler.SendData(client.Client, Utils.SerlizeObject(errorBody));
                return;
            }
            var alClientsToAdd = data.lsUsers.Where(c => groupChat.IsClientExistInChat(_allChatDetails.GetClientByName(c))).Select(u => _allChatDetails.GetClientByName(u)).ToList();
            groupChat.RemoveMultiClients(alClientsToAdd);
            var successBody = new OkResponseMessage
            {
                RequestType = "SuccessResponse",
                Message = $"Group {data.GroupName} users updated"
            };
            _requestHandler.SendData(client.Client, Utils.SerlizeObject(successBody));
        }
    }
}
