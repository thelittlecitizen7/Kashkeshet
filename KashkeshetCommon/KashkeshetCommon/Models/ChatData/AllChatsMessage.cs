using KashkeshetCommon.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace KashkeshetCommon.Models.ChatData
{
    [Serializable]
    public class AllChatsMessage : MainRequest
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public List<ChatMessageModel> Chats { get; set; }

    }

    [Serializable]
    public class ChatMessageModel 
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public ChatType ChatType { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public string GroupName { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public string ChatId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public List<string> Names { get; set; }
    }
}
