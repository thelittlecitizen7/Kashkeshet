using KashkeshetCommon;
using KashkeshetCommon.Enum;
using KashkeshetCommon.Models.ChatData;
using KashkeshtWorkerServiceServer.Src.Models;
using KashkeshtWorkerServiceServer.Src.Models.ChatsModels;
using KashkeshtWorkerServiceServer.Src.RequestsHandler;
using KashkeshtWorkerServiceServer.Src.ResponsesHandler;
using System.Linq;

namespace KashkeshtWorkerServiceServer.Src.ServerOptions.ManagerOptions
{
    public class ExitChatOption : IOption
    {
        private AllChatDetails _allChatDetails;

        private IClientModel _userClient;

        private IContainerInterfaces _containerInterfaces;
        public ExitChatOption(IClientModel userClient, AllChatDetails allChatDetails , IContainerInterfaces containerInterfaces)
        {
            _userClient = userClient;
            _containerInterfaces = containerInterfaces;
            
            _allChatDetails = allChatDetails;
        }
        public void Operation(MainRequest chatData)
        {
            var data = chatData as GroupChatMessageModel;
            var groupChat = _allChatDetails.GetGroupByName(data.GroupName);

            if (groupChat == null)
            {
                var errorBody = new ErrorMessage
                {
                    RequestType = MessageType.ErrorResponse,
                    Error = $"There is not group chat with name {data.GroupName}"
                };
                _containerInterfaces.RequestHandler.SendData(_userClient.Client, Utils.SerlizeObject(errorBody));
                return;
            }

            var alClientsToRemove = data.lsUsers.Where(c => groupChat.IsClientExistInChat(_allChatDetails.GetClientByName(c))).Select(u => _allChatDetails.GetClientByName(u)).ToList();
            groupChat.RemoveMultiClients(alClientsToRemove);
            var successBody = new OkResponseMessage
            {
                RequestType = MessageType.SuccessResponse,
                Message = $"Group {data.GroupName} users updated"
            };
            _containerInterfaces.RequestHandler.SendData(_userClient.Client, Utils.SerlizeObject(successBody));
        }
    }
}
