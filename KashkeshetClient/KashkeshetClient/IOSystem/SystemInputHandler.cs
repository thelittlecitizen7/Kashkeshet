using MenuBuilder.IO.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace KashkeshetClient.IOSystem
{
    public class SystemInputHandler : ISystemInput
    {
        public string StringInput()
        {
            return Console.ReadLine();
        }
    }
}
