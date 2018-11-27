using System;
using Newtonsoft.Json;

namespace ShariesApp
{
    public class BlockedAccounts // create own class file? *
    {
        [JsonProperty(PropertyName = "id")]
        public string BlockId { get; set; }

        [JsonProperty(PropertyName = "Blocker")]
        public int Blocker { get; set; }

        [JsonProperty(PropertyName = "Blockee")]
        public int Blockee { get; set; }
    }
}
