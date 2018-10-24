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

        [JsonProperty(PropertyName = "Password")]
        public string Password { get; set; }

        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }
    }
}
