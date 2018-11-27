using System;
using Newtonsoft.Json;

namespace ShariesApp
{
    public class UserCredit
    // This is the only model that isn't included in our ERD and class diagram
    // because it represents the main systems _database
    {
        [JsonProperty(PropertyName = "id")]
        public string UserCreditId { get; set; }

        [JsonProperty(PropertyName = "AccountNumber")]
        public int AccountNumber { get; set; }

        [JsonProperty(PropertyName = "CreditAmount")]
        public double CreditAmount { get; set; }

        [JsonProperty(PropertyName = "TextAmount")]
        public double TextAmount { get; set; }

        [JsonProperty(PropertyName = "DataAmount")]
        public double DataAmount { get; set; }

        [JsonProperty(PropertyName = "MinutesAmount")]
        public double MinutesAmount { get; set; }
    }
}
