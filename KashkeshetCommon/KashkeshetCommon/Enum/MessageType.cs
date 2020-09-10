using System;
using System.Collections.Generic;
using System.Text;

namespace KashkeshetCommon.Enum
{
    public enum MessageType
    {
        PrivateCreationChat,
        GetAllChats,
        InsertToChat,
        UserChatStatus,
        GetAllUserConnected,
        GroupCreationChat,
        AddUserToChat,
        RemoveUserToChat,
        AddAdminPermissions,
        RemoveAdminPermissions,
        ExitChat,
        NewChatMessage,
        UserStatus,
        SuccessResponse,
        ErrorResponse,
        HistoryChatMessages
    }
}
