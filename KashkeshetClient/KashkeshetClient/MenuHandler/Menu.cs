using KashkeshetClient.ClientRequestsHandler;
using KashkeshetClient.IOSystem;
using KashkeshetClient.MenuHandler.Options;
using KashkeshetClient.MenuHandler.Options.ManagerOptions;
using KashkeshetClient.Models;
using KashkeshetClient.ServersHandler;
using MenuBuilder;
using MenuBuilder.IO.Input;
using MenuBuilder.Menus;
using MenuBuilder.Menus.NumberMenu;
using MenuBuilder.output;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace KashkeshetClient.MenuHandler
{
    public class Menu
    {
        private IContainerInterfaces _containerInterfaces;

        private IUser _user;
        public Menu(IContainerInterfaces containerInterfaces , IUser user)
        {
            _containerInterfaces = containerInterfaces;
            _user = user;
        }

        public void Run()
        {
            ServerHandler serverHandler = new ServerHandler(_containerInterfaces,_user);
            IMenuBuilder managerMenuBuilder = new NumberMenuBuilder("Manager options : ", _containerInterfaces.SystemOutput, _containerInterfaces.SystemInput)
            .AddOptions("Add user from group", new AddUserToChatOption(_containerInterfaces, _user, serverHandler))
            .AddOptions("Remove user from group", new RemoveUserFromChatOption(_containerInterfaces, _user, serverHandler))
            .AddOptions("Add user as admin", new AddManagerPermissionOption(_containerInterfaces, _user, serverHandler))
            .AddOptions("Remove user as admin", new RemoveManagerPermissionOption(_containerInterfaces, _user, serverHandler));

            IMenu numberMenuBuilder = new NumberMenuBuilder("Chat options : ", _containerInterfaces.SystemOutput, _containerInterfaces.SystemInput).
                AddOptions("Get All Chats", new GetAllChatOption(_containerInterfaces,serverHandler)).
                AddOptions("Create Private Chat", new PrivateChatCreatorOption(_containerInterfaces, _user, serverHandler)).
                AddOptions("Create Group Chat", new GroupChatCreatorOption(_containerInterfaces, _user, serverHandler)).
                AddOptions("Manager Options", new NavigateMenuOption(managerMenuBuilder.Build())).
                AddOptions("Go into chat", new InsertToChatOption(_containerInterfaces,serverHandler)).
                AddOptions("Exit from Group", new ExitChatOption(_containerInterfaces, _user, serverHandler)).
                AddOptions("Exit from chat", new MenuExitOption())
                .Build();

            managerMenuBuilder.AddOptions("MoveBack", new NavigateMenuOption(numberMenuBuilder));

            numberMenuBuilder.Run();
        }
    }
}
