using System;
using System.Collections.Generic;
using System.Text;
using KashkeshetCommon.Enum;

namespace KashkeshtWorkerServiceServer.Src.Models.ChatModel
{
    public class GlobalChat : ChatModule
    {
        public GlobalChat() : base(ChatType.Globaly)
        {
        }
    }
}
