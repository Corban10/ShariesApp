using System;
using Newtonsoft.Json;

namespace ShariesApp
{
    public class RequestData
    {
        [JsonProperty(PropertyName = "id")]
        public string RequestId { get; set; }  //PK

        [JsonProperty(PropertyName = "RequestSource")]
        public int RequestSource { get; set; } //FK1

        [JsonProperty(PropertyName = "RequestDestination")]
        public int RequestDestination { get; set; } //FK2

        [JsonProperty(PropertyName = "RequestType")]
        public string RequestType { get; set; }

        [JsonProperty(PropertyName = "RequestAmount")]
        public double RequestAmount{ get; set; }
    }
}