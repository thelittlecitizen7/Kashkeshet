using KashkeshtWorkerServiceServer.Src.Models;
using KashkeshtWorkerServiceServer.Src.Models.ChatsModels;
using KashkeshtWorkerServiceServer.Src.Models.CommandRequest;
using KashkeshtWorkerServiceServer.Src.ServerOptions;
using KashkeshtWorkerServiceServer.Src.ServerOptions.CommandOptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace KashkeshtWorkerServiceServer.Src.Factory
{
    public class OperationCommandFactory
    {
        private IClientModel _userClient;

        private IContainerInterfaces _containerInterfaces;

        private CommandHandler _commandHandler;
        public OperationCommandFactory(IClientModel userClient, IContainerInterfaces containerInterfaces, CommandHandler commandHandler)
        {
            _commandHandler = commandHandler;
            _userClient = userClient;
            _containerInterfaces = containerInterfaces;
        }
        public IOption GetOption(string command) 
        {
            switch (command) 
            {
                case "/timeZone":
                    return new GetTimeZoneOption(_userClient, _containerInterfaces);
                case "/GetHelp":
                    return new CommandHelperOption(_userClient, _containerInterfaces,_commandHandler);
                default:
                    return null;
            }
        }
    }
}
