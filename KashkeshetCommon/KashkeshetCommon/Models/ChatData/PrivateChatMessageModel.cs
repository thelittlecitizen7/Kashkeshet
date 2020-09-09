using KashkeshetCommon.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace KashkeshetCommon.Models.ChatData
{
    [Serializable]
    public class PrivateChatMessageModel : MainRequest
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> lsUsers { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ChatType ChatType { get; set; }

    }
}
