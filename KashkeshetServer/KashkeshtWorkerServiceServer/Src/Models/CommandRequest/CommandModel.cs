using KashkeshtWorkerServiceServer.Src.ServerOptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace KashkeshtWorkerServiceServer.Src.Models.CommandRequest
{
    public class CommandModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public IOption Option { get; set; }


        public CommandModel(string name ,string description , IOption option)
        {
            Name = name;
            Description = description;
            Option = option;
        }
    }
}
