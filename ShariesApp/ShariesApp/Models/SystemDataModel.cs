using System;
using Newtonsoft.Json;

namespace ShariesPrototype
{
    class SystemDataModel
    {
        [JsonProperty(PropertyName = "creditLimit")]
        public double creditLimit { get; set; }

        [JsonProperty(PropertyName = "textLimit")]
        public double textLimit { get; set; }

        [JsonProperty(PropertyName = "dataLimit")]
        public double dataLimit { get; set; }

        [JsonProperty(PropertyName = "minutesLimit")]
        public double minutesLimit { get; set; }
    }
}
