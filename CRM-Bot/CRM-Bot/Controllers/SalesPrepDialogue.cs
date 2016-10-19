using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis.Models;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Luis;

namespace CRM_Bot.Controllers
{
    [Serializable]
    [LuisModel("9fa88d02-421f-4849-bfca-477600130220", "3a322409fdf246a0b8423ed40df30c94")]
    public class SalesPrepDialogue : LuisDialog<object>
    {
        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = Logic.CRMLookup.CRMLookupDialogue(result.Query.ToString()) ?? $"Sorry, I did not understand '{result.Query}'. Type 'help' if you need assistance.";

            await context.PostAsync(message);

            context.Wait(this.MessageReceived);
        }

        [LuisIntent("Help")]
        public async Task Help(IDialogContext context, LuisResult result)
        {
            var message = context.MakeMessage();
            message.Attachments = new List<Attachment>();
            message.Attachments.Add(GetWelcomeHeroCard());
            await context.PostAsync(message);

            context.Wait(this.MessageReceived);
        }

        [LuisIntent("Prepare")]
        public async Task PrepareDialogue(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Give me a sec.. checking your O365 calendar.");

            await context.PostAsync(Logic.O356CalendarLookup.O356FindAppointment());

            var companyBio = context.MakeMessage();
            companyBio.Attachments = new List<Attachment>();
            companyBio.Attachments.Add(Logic.O356CalendarLookup.GetCompanyCard());
            await context.PostAsync(companyBio);

            context.Wait(this.MessageReceived);
        }

        private static Attachment GetWelcomeHeroCard()
        {
            var heroCard = new HeroCard
            {
                Title = "Parts Unlimited Sales Assistant",
                //Subtitle = "Your bots — wherever your users are talking",
                Text = "Ask me about your accounts! I can help you log information and prepare for a meeting  with your customer by looking through Dynamics CRM and O365!",
                Images = new List<CardImage> { new CardImage("https://sec.ch9.ms/ch9/7ff5/e07cfef0-aa3b-40bb-9baa-7c9ef8ff7ff5/buildreactionbotframework_960.jpg") },
                Buttons = new List<CardAction>
                {
                    new CardAction
                    (
                        ActionTypes.ImBack,
                        "Prepare",
                        value: "Prepare"
                    ),
                    new CardAction
                    (
                        ActionTypes.ImBack,
                        "Log",
                        value: "Log"
                    ),
                    new CardAction
                    (
                        ActionTypes.ImBack,
                        "Sign-In",
                        value: "Sign-In"
                    )
                }
            };

            return heroCard.ToAttachment();
        }

        
    }
}