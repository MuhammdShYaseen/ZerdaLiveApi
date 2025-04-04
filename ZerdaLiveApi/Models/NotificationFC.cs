using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZerdaLiveApi.Models
{
    public class NotificationFC
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("body")]
        public string Body { get; set; }
        [JsonProperty("image")]
        public string ImageUrl { get; set; }
        public Dictionary<string, string> Dataa { get; set; }
    }
}
