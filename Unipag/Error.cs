using System.Collections.Generic;
using Newtonsoft.Json;

namespace Unipag
{
    public class Error
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("params")]
        public Dictionary<string, string> Params { get; set; }
    }
}
