using KashkeshetCommon.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace KashkeshetCommon.Models.ChatData
{
    [Serializable]
    public class MainRequest
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public MessageType RequestType { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public string From { get; set; }


    }
}
