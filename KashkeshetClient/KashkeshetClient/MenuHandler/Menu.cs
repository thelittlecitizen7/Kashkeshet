using KashkeshetClient.MenuHandler.Options;
using KashkeshetClient.MenuHandler.Options.ManagerOptions;
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
        public void Run(string name,TcpClient client)
        {
            IOutputSystem outputSystem = new ConsoleOutput();
            ISystemInput systemInput = new SystemInput();

            //IMenuBuilder launcRocket = new TextMenuBuilder("Launch Menu : ", outputSystem, systemInput);

            ServerHandler serverHandler = new ServerHandler(name, client);
            IMenuBuilder managerMenuBuilder = new NumberMenuBuilder("Manager options : ", outputSystem, systemInput)
            .AddOptions("Add user from group", new AddUserToChatOption(name, serverHandler))
            .AddOptions("Remove user from group", new RemoveUserFromChatOption(name, serverHandler))
            .AddOptions("Add user as admin", new AddManagerPermissionOption(name,serverHandler))
            .AddOptions("Remove user as admin", new RemoveManagerPermissionOption(name, serverHandler));



            IMenu numberMenuBuilder = new NumberMenuBuilder("Chat options : ", outputSystem, systemInput).
                AddOptions("Get All Chats", new GetAllChatOption(serverHandler)).
                AddOptions("Create Private Chat", new PrivateChatCreatorOption(name,serverHandler)).
                AddOptions("Create Group Chat", new GroupChatCreatorOption(name,serverHandler)).
                AddOptions("Manager Options", new NavigateMenuOption(managerMenuBuilder.Build())).
                AddOptions("Go into chat", new InsertToChatOption(serverHandler)).
                AddOptions("Exit from Group", new ExitChatOption(name,serverHandler)).
                AddOptions("Exit from chat",new MenuExitOption())
                .Build();

            managerMenuBuilder.AddOptions("MoveBack", new NavigateMenuOption(numberMenuBuilder));


            //launcRocket.AddOptions("MoveBack", new NavigateMenu(numberMenuBuilder))


            numberMenuBuilder.Run();
        }
    }
}
