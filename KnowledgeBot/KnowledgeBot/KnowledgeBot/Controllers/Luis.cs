using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System.Threading.Tasks;
using KnowledgeBot.AzureSearch;
using KnowledgeBot.Model;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;

namespace KnowledgeBot.Controllers
{
    [LuisModel("<LUIS APP ID>", "<LUIS SECRET>")]
    [Serializable]

    public class Luis : LuisDialog<object>
    {
        #region Constants
        private const string _domain = "trbotazuresearch.search.windows.net";
        private const string _key = "81EC28AC03C0195BDECAB1E7AE7A3FE4";
        private const string _index = "kb";
        private const string _apiVersion = "2015-02-28";
        private const decimal _scoreMinimum = 0.05M;
        private const string _noResults = "Sorry, I couldn't find any help";

        private const string _confirmCaseNo = "Sure thing, what's your case no?";
        private const string _closeRequestSent = "Ok, I've sent an update to close your case.";

        private static string _closeCaseLogicAppHTTPEndpoint = "https://prod-07.northeurope.logic.azure.com:443/workflows/d01471365f894b22b548b891c4d9e393/triggers/manual/run?api-version=2016-06-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=V8Gslfelc0ybDFvHepArih3j-oYdEj-0aLQKJhEC9k0";
        #endregion

        // This class is serialized so need to make sure everything supports serialization!!    
        private ResumptionCookie resumptionCookie;

        protected async override Task MessageReceived(IDialogContext context, IAwaitable<IMessageActivity> item)
        {

            Activity activity = (Activity)await item;
            this.resumptionCookie = new ResumptionCookie(activity);

            await base.MessageReceived(context, item);
        }

        [LuisIntent("CaseStatus")]
        public async Task GetCaseStatus(IDialogContext context, LuisResult result)
        {
            PromptDialog.Text(context, AfterConfirming_caseNumberStatus, _confirmCaseNo);
        }

        public async Task AfterConfirming_caseNumberStatus(IDialogContext context, IAwaitable<string> casenoconfirmation)
        {
            string caseno = await casenoconfirmation;
            // GET CASE STATUS ...
            context.Wait(MessageReceived);

        }
        /// <summary>
        ///     Will run when a CloseCase intent is Triggered -- We'll test this one
        /// </summary>
        /// <param name="context"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        [LuisIntent("CloseCase")]
        public async Task CloseCase(IDialogContext context, LuisResult result)
        {
            PromptDialog.Text(context, AfterConfirming_caseNumberClose, _confirmCaseNo);
        }

       

        /// <summary>
        ///     Will run once we have the case number from the close case intent -- We'll test this one
        /// </summary>
        /// <param name="context"></param>
        /// <param name="casenoconfirmation"></param>
        /// <returns></returns>
        public async Task AfterConfirming_caseNumberClose(IDialogContext context, IAwaitable<string> casenoconfirmation)
        {
            // GET THE CASE NO FROM THE CONFIRMATION
            string caseno = await casenoconfirmation;
            Incident incident = new Incident()
            {
                casenumber = caseno
            };

            // SERIALISE INTO THE INCIDENT OBJECT AND SEND TO LOGIC APP
            string json = JsonConvert.SerializeObject(incident);
            string response = await LogicApp.LogicAppHelper.MakePost(_closeCaseLogicAppHTTPEndpoint, json);

            // REPLY TO THE USER
            string message = _closeRequestSent;
            await context.PostAsync(message);

            // AWAIT THE NEXT MESSAGE
            context.Wait(MessageReceived);

        }

        //Will run when no intent is triggered
        [LuisIntent("")]
        public async Task NoIntent(IDialogContext context, LuisResult result)
        {
            bool sentReply = false;
            AzureSearchHelper azureSearch = new AzureSearch.AzureSearchHelper(_domain, _key, _index, _apiVersion);
            List<SearchResult> searchResults = await azureSearch.Search(result.Query) as List<SearchResult>;

            if (searchResults != null && searchResults.Count > 0)
            {

                foreach (SearchResult searchResult in searchResults)
                {

                    if (searchResult.score > _scoreMinimum)
                    {
                        sentReply = true;
                        string replyContent = AzureSearch.AzureSearchHelper.StripHTML(searchResult.content);
                        await context.PostAsync(replyContent);
                        context.Wait(MessageReceived);
                    }
                }


            }
        }
    }
    
   

}