
using KnowledgeBot.Model;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.FormFlow.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;


namespace KnowledgeBot.Forms
{
    [Serializable]
    public class CaseQuestions
    {

        private static string _createCaseLogicAppHTTPEndpoint = "<YOUR HTTP LISTENER END POINT>";
        private static string _pleaseWaitMessage = "working on it..";
        private static string _createdCaseMessage = "Ok, I've created a new case for you, your case no is {0}";
        public static IForm<JObject> BuildForm()
        {
            OnCompletionAsyncDelegate<JObject> processResult = async (context, state) =>
            {
                var responses = new Dictionary<string, string>();

                Incident incident = new Incident();

                // Iterate the responses and do what you like here
                foreach (JProperty item in (JToken)state)
                {
                    responses.Add(item.Name, item.Value.ToString());

                    if (item.Name.Equals("Email"))
                        incident.email = AzureSearch.AzureSearchHelper.StripHTML((string)item.Value);

                    if (item.Name.Equals("Name"))
                        incident.name = (string)item.Value;

                    if (item.Name.Equals("Phone"))
                        incident.phonenumber = (string)item.Value;

                    if (item.Name.Equals("Title"))
                        incident.title = (string)item.Value; 
                }

                string json = JsonConvert.SerializeObject(incident);
                string response = await LogicApp.LogicAppHelper.MakePost(_createCaseLogicAppHTTPEndpoint, json);

                string caseNo = JObject.Parse(response)["casenumber"].Value<string>();

                var msg = context.MakeMessage();
                msg.Text = string.Format(_createdCaseMessage, caseNo);
                await context.PostAsync(msg);

            };
            
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("KnowledgeBot.Data.CaseQuestions.json"))
            {
                var schema = JObject.Parse(new StreamReader(stream).ReadToEnd());
                return new FormBuilderJson(schema)
                    .AddRemainingFields()
                    .Message(_pleaseWaitMessage)
                    .OnCompletion(processResult)
                    .Build();
            }
        }
    }
}