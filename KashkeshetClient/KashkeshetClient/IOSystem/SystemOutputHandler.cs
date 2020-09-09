using MenuBuilder;
using System;
using System.Collections.Generic;
using System.Text;

namespace KashkeshetClient.IOSystem
{
    public class SystemOutputHandler : IOutputSystem
    {
        public void Print(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}
