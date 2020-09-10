using System;
using System.Collections.Generic;
using System.Text;

namespace KashkeshtWorkerServiceServer.Src.Models.CommandRequest
{
    public class CommandHandler
    {
        public List<CommandModel> Commands { get; set; }
        public CommandHandler(CommandBuilder commandBuilder)
        {
            Commands = commandBuilder.Commands;
        }
    }
}
