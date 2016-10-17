using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KnowledgeBot.Model
{
    public class SearchResult
    {
        [JsonProperty("@search.score")]
        public decimal score { get; set; }
        public string id { get; set; }
        public string title { get; set; }
        public string content { get; set; }

    }
}