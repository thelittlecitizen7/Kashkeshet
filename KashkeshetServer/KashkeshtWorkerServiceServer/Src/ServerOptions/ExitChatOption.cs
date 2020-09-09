using KashkeshetCommon.Enum;
using KashkeshetCommon.Models.ChatData;
using KashkeshtWorkerServiceServer.Src.Models;
using KashkeshtWorkerServiceServer.Src.RequestsHandler;
using KashkeshtWorkerServiceServer.Src.ResponsesHandler;
using System.Linq;

namespace KashkeshtWorkerServiceServer.Src.ServerOptions.ManagerOptions
{
    public class ExitChatOption : IOption
    {
        private AllChatDetails _allChatDetails;

        private string Name { get; set; }

        private IServerRequestHandler _requestHandler;
        private IServerResponseHandler _responseHandler;
        public ExitChatOption(string name, AllChatDetails allChatDetails)
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

            var alClientsToRemove = data.lsUsers.Where(c => groupChat.IsClientExistInChat(_allChatDetails.GetClientByName(c))).Select(u => _allChatDetails.GetClientByName(u)).ToList();
            groupChat.RemoveMultiClients(alClientsToRemove);
            var successBody = new OkResponseMessage
            {
                RequestType = MessageType.SuccessResponse,
                Message = $"Group {data.GroupName} users updated"
            };
            _requestHandler.SendData(client.Client, Utils.SerlizeObject(successBody));
        }
    }
}
