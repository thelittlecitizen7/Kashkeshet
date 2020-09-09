using MenuBuilder.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace KashkeshetClient.MenuHandler.Options
{
    public class MenuExitOption : IOptions
    {
        
        public void Operation()
        {
            Environment.Exit(1);
        }
    }
}
