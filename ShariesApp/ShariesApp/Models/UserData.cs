using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ShariesApp
{
    public class UserData
    {
        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }

        [JsonProperty(PropertyName = "accountNumber")]
        public int accountNumber { get; set; }

        [JsonProperty(PropertyName = "password")]
        public string password { get; set; }

        [JsonProperty(PropertyName = "pame")]
        public string name { get; set; }
    }
}
