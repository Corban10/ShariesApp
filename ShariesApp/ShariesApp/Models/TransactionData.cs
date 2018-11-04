using System;
using Newtonsoft.Json;

namespace ShariesPrototype
{
    public class TransactionData
    {
        [JsonProperty(PropertyName = "id")]
        public string id { get; set; } 

        [JsonProperty(PropertyName = "transactionID")]
        public int transactionID { get; set; } //PK

        [JsonProperty(PropertyName = "transactionSource")]
        public int transactionSource { get; set; } //FK1

        [JsonProperty(PropertyName = "transactionDestination")]
        public int transactionDestination { get; set; } //FK2

        [JsonProperty(PropertyName = "transactionType")]
        public string transactionType { get; set; }

        [JsonProperty(PropertyName = "transactionAmount")]
        public double transactionAmount{ get; set; }

        [JsonProperty(PropertyName = "NotificationStatus")]
        public bool notificationStatus { get; set; } // sent or not sent?
    }
}