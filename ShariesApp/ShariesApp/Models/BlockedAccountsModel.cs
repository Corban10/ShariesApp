using System;
using Newtonsoft.Json;

namespace ShariesApp
{
    public class BlockedAccounts // create own class file? *
    {
        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }

        [JsonProperty(PropertyName = "blocker")]
        public int blocker { get; set; }

        [JsonProperty(PropertyName = "blockee")]
        public int blockee { get; set; }
    }
}
