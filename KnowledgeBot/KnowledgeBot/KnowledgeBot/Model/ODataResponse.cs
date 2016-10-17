using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KnowledgeBot.Model
{
    public class ODataResponse
    {
        [JsonProperty("odata.context")]
        public string Context { get; set; }
        public List<SearchResult> Value { get; set; }
    }
}