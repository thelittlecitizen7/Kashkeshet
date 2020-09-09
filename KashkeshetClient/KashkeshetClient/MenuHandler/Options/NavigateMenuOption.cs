using MenuBuilder.Menus;
using MenuBuilder.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace KashkeshetClient.MenuHandler.Options
{
    public class NavigateMenuOption : IOptions
    {
        private IMenu _menu;
        public NavigateMenuOption(IMenu menu)
        {
            _menu = menu;
        }
        public void Operation()
        {
            _menu.Run();
        }
    }
}
