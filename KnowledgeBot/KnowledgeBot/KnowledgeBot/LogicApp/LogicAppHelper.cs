using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace KnowledgeBot.LogicApp
{
    public class LogicAppHelper
    {

        public static async Task<string> MakePost(string requestUrl, string json)
        {

            var client = new HttpClient();

            StringContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(requestUrl, content);

            return await response.Content.ReadAsStringAsync();

        }
    }
}