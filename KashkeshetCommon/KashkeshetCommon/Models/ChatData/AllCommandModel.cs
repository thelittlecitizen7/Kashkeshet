using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace KashkeshetCommon.Models.ChatData
{
    [Serializable]
    public class AllCommandModel : MainRequest
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<Command> Commands { get; set; }
    }

    public class Command
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Name{ get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
    }
}
