using KashkeshetClient.Models;
using KashkeshetClient.ServersHandler;
using MenuBuilder.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace KashkeshetClient.MenuHandler.Options
{
    public class CommandRequestOption : IOptions
    {

        private IContainerInterfaces _containerInterfaces;

        private ServerHandler _serverHandler { get; set; }
        public CommandRequestOption(IContainerInterfaces containerInterfaces, ServerHandler serverHandler)
        {
            _serverHandler = serverHandler;
            _containerInterfaces = containerInterfaces;
        }
        public void Operation()
        {
            _containerInterfaces.SystemOutput.Print("Please enter the command ou want to do | for exit press exit");
            string command = _containerInterfaces.SystemInput.StringInput();
            string responseCommand = _serverHandler.SendCommand(command);

            
            if (responseCommand == null)  // when there  is error from server
            {
                return;
            }
            _containerInterfaces.SystemOutput.Print(responseCommand);
        }
    }
}
