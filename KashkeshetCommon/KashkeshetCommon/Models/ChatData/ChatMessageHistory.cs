using KashkeshetCommon.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace KashkeshetCommon.Models.ChatData
{
    [Serializable]
    public class ChatMessageHistory : MainRequest
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ChatId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public MessageDetails[] AllMessages { get; set; }
    }


    [Serializable]
    public class MessageDetails
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ChatMessageType MessageType { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime Datetime { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SenderName { get; set; }
    }


}
