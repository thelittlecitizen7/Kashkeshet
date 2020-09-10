using KashkeshetCommon;
using KashkeshetCommon.Enum;
using KashkeshetCommon.Models.ChatData;
using KashkeshtWorkerServiceServer.Src.Models;
using KashkeshtWorkerServiceServer.Src.Models.ChatsModels;
using KashkeshtWorkerServiceServer.Src.RequestsHandler;
using KashkeshtWorkerServiceServer.Src.ResponsesHandler;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace KashkeshtWorkerServiceServer.Src.ServerOptions.ManagerOptions
{
    public class AddAdminPermissionOption : IOption
    {
        private AllChatDetails _allChatDetails;

        private IClientModel _userClient;

        private IContainerInterfaces _containerInterfaces;

        public AddAdminPermissionOption(IClientModel userClient, AllChatDetails allChatDetails, IContainerInterfaces containerInterfaces)
        {
            _userClient = userClient;
            _containerInterfaces = containerInterfaces;
            _allChatDetails = allChatDetails;
        }
        public void Operation(MainRequest chatData)
        {
            var data = chatData as GroupChatMessageModel;
            var groupChat = _allChatDetails.GetGroupByName(data.GroupName);

            if (!groupChat.IsClientManager(_userClient))
            {
                var errorBody = new ErrorMessage
                {
                    RequestType = MessageType.ErrorResponse,
                    Error = $"The user name {_userClient.Name} has not permission to give admin permission"
                };
                _containerInterfaces.RequestHandler.SendData(_userClient.Client, Utils.SerlizeObject(errorBody));
                return;
            }
            var alClientsToAdd = data.lsUsers.Where(c => groupChat.IsClientExistInChat(_allChatDetails.GetClientByName(c))).Select(u => _allChatDetails.GetClientByName(u)).ToList();
            groupChat.AddMultiManagrs(alClientsToAdd);
            _containerInterfaces.Logger.LogInformation($"Group users updated");
            var successBody = new OkResponseMessage
            {
                RequestType = MessageType.SuccessResponse,
                Message = $"Group {data.GroupName} users updated"
            };
            _containerInterfaces.RequestHandler.SendData(_userClient.Client, Utils.SerlizeObject(successBody));
        }
    }
}
