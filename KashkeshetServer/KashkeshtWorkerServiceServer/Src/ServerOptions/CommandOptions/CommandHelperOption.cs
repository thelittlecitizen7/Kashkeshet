using KashkeshetCommon;
using KashkeshetCommon.Enum;
using KashkeshetCommon.Models.ChatData;
using KashkeshtWorkerServiceServer.Src.Models;
using KashkeshtWorkerServiceServer.Src.Models.ChatsModels;
using KashkeshtWorkerServiceServer.Src.Models.CommandRequest;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace KashkeshtWorkerServiceServer.Src.ServerOptions.CommandOptions
{
    public class CommandHelperOption : IOption
    {
        private IClientModel _userClient;

        private IContainerInterfaces _containerInterfaces;
        private CommandHandler _commandHandler;
        public CommandHelperOption(IClientModel userClient, IContainerInterfaces containerInterfaces, CommandHandler commandHandler)
        {
            _userClient = userClient;
            _commandHandler = commandHandler;
            _containerInterfaces = containerInterfaces;
        }
        public void Operation(MainRequest chatData)
        {
            try
            {
                string commands = $"Commands : {Environment.NewLine}";
                foreach (var command in _commandHandler.Commands)
                {
                    commands += $"Name : {command.Name} - {command.Description} {Environment.NewLine}";

                }
                var successBody = new OkResponseMessage
                {
                    RequestType = MessageType.SuccessResponse,
                    Message = commands
                };
                _containerInterfaces.RequestHandler.SendData(_userClient.Client, Utils.SerlizeObject(successBody));
                _containerInterfaces.Logger.LogInformation("Success send all commands");
            }
            catch (Exception e) 
            {
                var errorBody = new ErrorMessage
                {
                    RequestType = MessageType.ErrorResponse,
                    Error = $"Error when get all commands {e.Message}"
                };
                _containerInterfaces.RequestHandler.SendData(_userClient.Client, Utils.SerlizeObject(errorBody));
                _containerInterfaces.Logger.LogError($"Error when get all commands {e.Message}");
            }
        }
    }
}
