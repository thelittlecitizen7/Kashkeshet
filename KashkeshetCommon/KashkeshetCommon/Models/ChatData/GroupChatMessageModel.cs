using KashkeshetCommon.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace KashkeshetCommon.Models.ChatData
{
    public class GroupChatMessageModel : MainRequest
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string GroupName { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> lsUsers { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ChatType ChatType { get; set; }
    }
}
