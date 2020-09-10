using System;
using System.Collections.Generic;
using System.Text;

namespace KashkeshtWorkerServiceServer.Src.Models.CommandRequest
{
    public class CommandBuilder
    {
        public List<CommandModel> Commands { get; set; }
        public CommandBuilder()
        {
            Commands = new List<CommandModel>();
        }

        public CommandBuilder AddCommand(CommandModel command)
        {
            Commands.Add(command);
            return this;
        }

        public CommandHandler Build()
        {
            return new CommandHandler(this);
        }
    }
}
