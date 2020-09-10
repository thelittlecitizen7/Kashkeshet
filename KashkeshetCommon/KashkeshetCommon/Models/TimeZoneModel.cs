using KashkeshetCommon.Models.ChatData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace KashkeshetCommon.Models
{
    [Serializable]
    public class TimeZoneModel
    {
        [JsonProperty("datetime", NullValueHandling = NullValueHandling.Ignore)]
        public string Datatime { get; set; }


        [JsonProperty("day_of_week", NullValueHandling = NullValueHandling.Ignore)]
        public string DayOfWeekNumber { get; set; }
    }
}
