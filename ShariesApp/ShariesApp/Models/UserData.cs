using System;
using Newtonsoft.Json;

namespace ShariesApp
{
    public class UserData
    {
        [JsonProperty(PropertyName = "id")]
        public string UserId { get; set; }

        [JsonProperty(PropertyName = "AccountNumber")]
        public int AccountNumber { get; set; }

        [JsonProperty(PropertyName = "Password")]
        public string Password { get; set; }

        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }
    }
}
