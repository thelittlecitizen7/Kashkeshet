using KashkeshetCommon;
using KashkeshetCommon.Enum;
using KashkeshetCommon.Models.ChatData;
using KashkeshtWorkerServiceServer.Src.Models;
using KashkeshtWorkerServiceServer.Src.Models.ChatModel;
using KashkeshtWorkerServiceServer.Src.Models.ChatsModels;
using KashkeshtWorkerServiceServer.Src.Models.CommandRequest;
using KashkeshtWorkerServiceServer.Src.RequestsHandler;
using KashkeshtWorkerServiceServer.Src.ResponsesHandler;
using KashkeshtWorkerServiceServer.Src.ServerOptions;
using KashkeshtWorkerServiceServer.Src.ServerOptions.CommandOptions;
using KashkeshtWorkerServiceServer.Src.ServerOptions.ManagerOptions;
using System;
using System.Collections.Generic;

namespace KashkeshtWorkerServiceServer.Src.Factory
{
    public class OperationExecuteFactory
    {
        private AllChatDetails _allChatDetails { get; set; }
        
        private IContainerInterfaces _containerInterfaces;

        private IClientModel _userClient { get; set; }

        private CommandHandler _commandHandler; 

        public OperationExecuteFactory(IClientModel userClient, AllChatDetails allChatDetails , IContainerInterfaces containerInterfaces)
        {
            _containerInterfaces = containerInterfaces;
            _allChatDetails = allChatDetails;
            _userClient = userClient;
            BuildCommands();
        }

        private void BuildCommands() 
        {
            var timeZoneCommand = new CommandModel("/timeZone", "Get the timezone of isreal", new GetTimeZoneOption(_userClient, _containerInterfaces));
            var helpCommand = new CommandModel("/GetHelp", "Get help See all commands", new GetTimeZoneOption(_userClient, _containerInterfaces));
            _commandHandler = new CommandBuilder()
                .AddCommand(timeZoneCommand)
                .AddCommand(helpCommand)
                .Build();
        }

        public void Execute(string requestData)
        {
            var mainRequestData = Utils.DeSerlizeObject<MainRequest>(requestData);
            switch (mainRequestData.RequestType)
            {
                case MessageType.PrivateCreationChat:
                    var privateChatCreationDetaild = Utils.DeSerlizeObject<PrivateChatMessageModel>(requestData);
                    new PrivateChatCreatorOption(_userClient, _allChatDetails).Operation(privateChatCreationDetaild);
                    break;
                case MessageType.GetAllChats:
                    new GetAllChatOption(_userClient, _allChatDetails,_containerInterfaces).Operation(mainRequestData);
                    break;

                case MessageType.InsertToChat:
                    var sendMessageDetails = Utils.DeSerlizeObject<InsertToChatMessageModel>(requestData);
                    new InsertToChatOption(_allChatDetails, _containerInterfaces).Operation(sendMessageDetails);
                    break;

                case MessageType.GetAllUserConnected:
                    var getAllConnectedUsersDetails = Utils.DeSerlizeObject<MainRequest>(requestData);
                    new GetAllUserConnectedOption(_userClient, _allChatDetails,_containerInterfaces).Operation(getAllConnectedUsersDetails);
                    break;
                case MessageType.GroupCreationChat:
                    var groupCreationChatDetails = Utils.DeSerlizeObject<GroupChatMessageModel>(requestData);
                    new GroupChatCreatorOption(_userClient, _allChatDetails, _containerInterfaces).Operation(groupCreationChatDetails);
                    break;

                case MessageType.AddUserToChat:
                    var addUserChatDetails = Utils.DeSerlizeObject<GroupChatMessageModel>(requestData);
                    new AddUserToGroupOption(_userClient, _allChatDetails, _containerInterfaces).Operation(addUserChatDetails);
                    break;
                case MessageType.RemoveUserToChat:
                    var RemoveUserToChatDetails = Utils.DeSerlizeObject<GroupChatMessageModel>(requestData);
                    new RemoveUserFromGroupOption(_userClient, _allChatDetails,_containerInterfaces).Operation(RemoveUserToChatDetails);
                    break;
                case MessageType.AddAdminPermissions:
                    var AddAdminPermissionsDetails = Utils.DeSerlizeObject<GroupChatMessageModel>(requestData);
                    new AddAdminPermissionOption(_userClient, _allChatDetails,_containerInterfaces).Operation(AddAdminPermissionsDetails);
                    break;
                case MessageType.RemoveAdminPermissions:
                    var RemoveAdminPermissionsDetails = Utils.DeSerlizeObject<GroupChatMessageModel>(requestData);
                    new RemoveAdminPermissionOption(_userClient, _allChatDetails,_containerInterfaces).Operation(RemoveAdminPermissionsDetails);
                    break;
                case MessageType.ExitChat:
                    var ExitChatDetails = Utils.DeSerlizeObject<GroupChatMessageModel>(requestData);
                    new ExitChatOption(_userClient, _allChatDetails,_containerInterfaces).Operation(ExitChatDetails);
                    break;

                case MessageType.HistoryChatMessages:
                    var HistoryChatMessagesDetails = Utils.DeSerlizeObject<ChatMessageHistory>(requestData);
                    new MessageHistoryOption(_userClient, _allChatDetails, _containerInterfaces).Operation(HistoryChatMessagesDetails);
                    break;

                case MessageType.CommandMessage:
                    var CommandMessageDetails = Utils.DeSerlizeObject<CommandMessage>(requestData);
                    new MainCommandOption(_userClient, _containerInterfaces,_commandHandler).Operation(CommandMessageDetails);
                    break;
            }
        }
    }
}
