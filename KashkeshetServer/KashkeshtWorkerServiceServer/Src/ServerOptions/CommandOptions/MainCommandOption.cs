using KashkeshetCommon;
using KashkeshetCommon.Enum;
using KashkeshetCommon.Models.ChatData;
using KashkeshtWorkerServiceServer.Src.Factory;
using KashkeshtWorkerServiceServer.Src.Models;
using KashkeshtWorkerServiceServer.Src.Models.ChatsModels;
using KashkeshtWorkerServiceServer.Src.Models.CommandRequest;
using System;
using System.Collections.Generic;
using System.Text;

namespace KashkeshtWorkerServiceServer.Src.ServerOptions.CommandOptions
{
    public class MainCommandOption : IOption
    {
        private IClientModel _userClient;

        private IContainerInterfaces _containerInterfaces;

        private OperationCommandFactory _operationCommandFactory;

        private CommandHandler _commandHandler;
        public MainCommandOption(IClientModel userClient, IContainerInterfaces containerInterfaces, CommandHandler commandHandler)
        {
            _commandHandler = commandHandler;
            _userClient = userClient;
            _containerInterfaces = containerInterfaces;
            _operationCommandFactory = new OperationCommandFactory(_userClient, _containerInterfaces,_commandHandler);
        }

        public void Operation(MainRequest chatData)
        {
            var data = chatData as CommandMessage;

            
            if (!data.Command.StartsWith("/"))
            {
                var errorBody = new ErrorMessage
                {
                    RequestType = MessageType.ErrorResponse,
                    Error = $"not valid command , must start with /"
                };
                _containerInterfaces.RequestHandler.SendData(_userClient.Client, Utils.SerlizeObject(errorBody));
                return;
            }
            else
            {
                
                IOption optionCommand = _operationCommandFactory.GetOption(data.Command);

                if (optionCommand == null) 
                {
                    var errorBody = new ErrorMessage
                    {
                        RequestType = MessageType.ErrorResponse,
                        Error = $"There is not command like"
                    };
                    _containerInterfaces.RequestHandler.SendData(_userClient.Client, Utils.SerlizeObject(errorBody));
                    return;
                }


                var successBody = new OkResponseMessage
                {
                    RequestType = MessageType.SuccessResponse,
                    Message = $"The request for command {data.Command} starting .. "
                };
                _containerInterfaces.RequestHandler.SendData(_userClient.Client, Utils.SerlizeObject(successBody));



                optionCommand.Operation(data);
            }
        }

    }
}
