using System;
using Newtonsoft.Json;

namespace ShariesApp
{
    public class RequestData
    {
        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }  //PK

        [JsonProperty(PropertyName = "requestSource")]
        public int requestSource { get; set; } //FK1

        [JsonProperty(PropertyName = "requestDestination")]
        public int requestDestination { get; set; } //FK2

        [JsonProperty(PropertyName = "requestType")]
        public string requestType { get; set; }

        [JsonProperty(PropertyName = "requestAmount")]
        public double requestAmount{ get; set; }

        [JsonProperty(PropertyName = "notificationStatus")]
        public bool notificationStatus { get; set; } // sent or not sent?
    }
}