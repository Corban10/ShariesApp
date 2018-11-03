using System;
using Newtonsoft.Json;

namespace ShariesPrototype
{
    public class TransactionDataModel
    {
        [JsonProperty(PropertyName = "transactionID")]
        public int transactionID { get; set; } //PK

        [JsonProperty(PropertyName = "transactionSource")]
        public int transactionSource { get; set; } //FK1

        [JsonProperty(PropertyName = "transactionDestination")]
        public int transactionDestination { get; set; } //FK2

        [JsonProperty(PropertyName = "transactionType")]
        public int transactionType { get; set; } // enumeration of types?

        [JsonProperty(PropertyName = "transactionAmount")]
        public double transactionAmount{ get; set; }

        [JsonProperty(PropertyName = "NotificationStatus")]
        public bool NotificationStatus { get; set; } // sent or not sent?
    }
}