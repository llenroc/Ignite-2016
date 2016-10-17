using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;

// LOCAL
using KnowledgeBot.AzureSearch;
using KnowledgeBot.Model;
using System.Collections.Generic;
using KnowledgeBot.Forms;
using Newtonsoft.Json.Linq;
using KnowledgeBot.Controllers;

namespace KnowledgeBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        #region constants
        private string _domain = "kbbotsearch.search.windows.net";
        private string _key = "07D8A85ADE68C6A389BA389E61B74A8A";
        private string _index = "knowledgebase";
        private string _apiVersion = "2015-02-28";
        private decimal _scoreMinimum = 0.05M; // TWEAK THIS SCORE BASED ON YOUR KB ARTICLES

        private string _noResults = "Sorry, I couldn't find any help";
        #endregion

        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                // TECHREADY23 MSDY315 - PART THREE
                // ** Switching to natural language **
                //await Conversation.SendAsync(activity, () => new Luis());

                // TECHREADY23 MSDY315 - PART ONE
                //**Finding knowledge through Azure Search **

                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));

                bool sentReply = false;
                AzureSearchHelper azureSearch = new AzureSearch.AzureSearchHelper(_domain, _key, _index, _apiVersion);
                List<SearchResult> searchResults = await azureSearch.Search(activity.Text) as List<SearchResult>;

                if (searchResults != null && searchResults.Count > 0)
                {

                    foreach (SearchResult searchResult in searchResults)
                    {

                        if (searchResult.score > _scoreMinimum)
                        {
                            sentReply = true;
                            string replyContent = AzureSearch.AzureSearchHelper.StripHTML(searchResult.content);
                            Activity reply = activity.CreateReply(replyContent);
                            await connector.Conversations.ReplyToActivityAsync(reply);
                        }
                    }


                }

                if (!sentReply)
                {
                    // TECHREADY23 MSDY315 - PART ONE
                    // ** When we don't get a result, return a message **
                    Activity reply = activity.CreateReply(_noResults);
                    await connector.Conversations.ReplyToActivityAsync(reply);

                    // TECHREADY23 MSDY315 - PART TWO
                    // ** Including the ability to create a case if no results found **
                    //await Conversation.SendAsync(activity, MakeRootDialog);
                }

            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        internal static IDialog<JObject> MakeRootDialog()
        {
            return Chain.From(() => FormDialog.FromForm(CaseQuestions.BuildForm));
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}