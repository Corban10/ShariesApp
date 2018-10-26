using System;
using Newtonsoft.Json;

namespace ShariesPrototype
{
    class BlockedAccounts // create own class file? *
    {
        [JsonProperty(PropertyName = "blocker")]
        public int blocker { get; set; }

        [JsonProperty(PropertyName = "blockee")]
        public int blockee { get; set; }
    }
}
