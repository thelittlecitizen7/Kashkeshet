using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace KashkeshetCommon.Models.ChatData
{
    [Serializable]
    public class InsertToChatMessageModel : MainRequest
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public string ChatId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public string MessageChat { get; set; }

    }
}
