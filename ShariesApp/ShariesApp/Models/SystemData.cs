using System;
using Newtonsoft.Json;

namespace ShariesApp
{
    public class SystemData
    {
        [JsonProperty(PropertyName = "id")]
        public string SystemDataId { get; set; }

        [JsonProperty(PropertyName = "CreditLimit")]
        public double CreditLimit { get; set; }

        [JsonProperty(PropertyName = "TextLimit")]
        public double TextLimit { get; set; }

        [JsonProperty(PropertyName = "DataLimit")]
        public double DataLimit { get; set; }

        [JsonProperty(PropertyName = "MinutesLimit")]
        public double MinutesLimit { get; set; }
    }
}
