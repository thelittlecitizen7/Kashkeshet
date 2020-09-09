using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace KashkeshetCommon.Models.ChatData
{
    [Serializable]
    public class ChatData<T>
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public string RequestType { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public string ChatId { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public T Message { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public string From { get; set; }
    }
}
