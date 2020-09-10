using KashkeshetCommon;
using KashkeshetCommon.Enum;
using KashkeshetCommon.Models.ChatData;
using KashkeshtWorkerServiceServer.Src.Models;
using KashkeshtWorkerServiceServer.Src.Models.ChatModel;
using KashkeshtWorkerServiceServer.Src.Models.ChatsModels;
using KashkeshtWorkerServiceServer.Src.RequestsHandler;
using KashkeshtWorkerServiceServer.Src.ResponsesHandler;
using KashkeshtWorkerServiceServer.Src.ServerOptions;
using KashkeshtWorkerServiceServer.Src.ServerOptions.ManagerOptions;
using System;
using System.Collections.Generic;

namespace KashkeshtWorkerServiceServer.Src.Factory
{
    public class OperationExecuteFactory
    {
        public object locker = new object();
        private AllChatDetails _allChatDetails { get; set; }
        
        private IContainerInterfaces _containerInterfaces;

        private IClientModel _userClient { get; set; }

        public OperationExecuteFactory(IClientModel userClient, AllChatDetails allChatDetails , IContainerInterfaces containerInterfaces)
        {
            _containerInterfaces = containerInterfaces;
            _allChatDetails = allChatDetails;
            _userClient = userClient;
        }
        public void Execute(string requestData)
        {
            var obj = Utils.DeSerlizeObject<MainRequest>(requestData);
            switch (obj.RequestType)
            {
                case MessageType.PrivateCreationChat:
                    var request = Utils.DeSerlizeObject<PrivateChatMessageModel>(requestData);
                    new PrivateChatCreatorOption(_userClient, _allChatDetails).Operation(request);
                    break;
                case MessageType.GetAllChats:
                    new GetAllChatOption(_userClient, _allChatDetails,_containerInterfaces).Operation(obj);
                    break;

                case MessageType.InsertToChat:
                    var request3 = Utils.DeSerlizeObject<InsertToChatMessageModel>(requestData);
                    new InsertToChatOption(_allChatDetails, _containerInterfaces).Operation(request3);
                    break;

                case MessageType.GetAllUserConnected:
                    var request5 = Utils.DeSerlizeObject<MainRequest>(requestData);
                    new GetAllUserConnectedOption(_userClient, _allChatDetails,_containerInterfaces).Operation(request5);
                    break;
                case MessageType.GroupCreationChat:
                    var request6 = Utils.DeSerlizeObject<GroupChatMessageModel>(requestData);
                    new GroupChatCreatorOption(_userClient, _allChatDetails, _containerInterfaces).Operation(request6);
                    break;

                case MessageType.AddUserToChat:
                    var request7 = Utils.DeSerlizeObject<GroupChatMessageModel>(requestData);
                    new AddUserToGroupOption(_userClient, _allChatDetails, _containerInterfaces).Operation(request7);
                    break;
                case MessageType.RemoveUserToChat:
                    var request8 = Utils.DeSerlizeObject<GroupChatMessageModel>(requestData);
                    new RemoveUserFromGroupOption(_userClient, _allChatDetails,_containerInterfaces).Operation(request8);
                    break;
                case MessageType.AddAdminPermissions:
                    var request9 = Utils.DeSerlizeObject<GroupChatMessageModel>(requestData);
                    new AddAdminPermissionOption(_userClient, _allChatDetails,_containerInterfaces).Operation(request9);
                    break;
                case MessageType.RemoveAdminPermissions:
                    var request10 = Utils.DeSerlizeObject<GroupChatMessageModel>(requestData);
                    new RemoveAdminPermissionOption(_userClient, _allChatDetails,_containerInterfaces).Operation(request10);
                    break;
                case MessageType.ExitChat:
                    var request11 = Utils.DeSerlizeObject<GroupChatMessageModel>(requestData);
                    new ExitChatOption(_userClient, _allChatDetails,_containerInterfaces).Operation(request11);
                    break;

                case MessageType.HistoryChatMessages:
                    var request12 = Utils.DeSerlizeObject<ChatMessageHistory>(requestData);
                    new MessageHistoryOption(_userClient, _allChatDetails, _containerInterfaces).Operation(request12);
                    break;
                    
            }
        }
    }
}
