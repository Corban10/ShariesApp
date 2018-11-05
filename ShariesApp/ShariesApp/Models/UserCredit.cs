using System;
using Newtonsoft.Json;

namespace ShariesApp
{
    public class UserCredit
    // This is the only model that isn't included in our ERD and class diagram
    // because it represents the main systems database
    {
        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }

        [JsonProperty(PropertyName = "accountNumber")]
        public int accountNumber { get; set; }

        [JsonProperty(PropertyName = "creditAmount")]
        public double creditAmount { get; set; }

        [JsonProperty(PropertyName = "textAmount")]
        public double textAmount { get; set; }

        [JsonProperty(PropertyName = "dataAmount")]
        public double dataAmount { get; set; }

        [JsonProperty(PropertyName = "minutesAmount")]
        public double minutesAmount { get; set; }
    }
}
