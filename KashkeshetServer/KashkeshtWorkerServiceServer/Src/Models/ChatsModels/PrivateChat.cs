using System;
using System.Collections.Generic;
using System.Text;
using KashkeshetCommon.Enum;

namespace KashkeshtWorkerServiceServer.Src.Models.ChatModel
{
    public class PrivateChat : ChatModule
    {
        public PrivateChat() : base(ChatType.Private)
        {
        }
    }
}
