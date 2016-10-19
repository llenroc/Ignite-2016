using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;

namespace CRM_Bot.Logic
{
    public class O356CalendarLookup
    {
        public static string O356FindAppointment()
        {
            Task task = Task.Delay(5000);
            task.Wait();

            return "I see that you have a meeting with Contoso Mechanics Ltd. in 7 mins.";
        }

        public static Attachment GetCompanyCard()
        {

            Task task = Task.Delay(2000);
            task.Wait();

            var heroCard = new HeroCard
            {
                Title = "Contoso Mechanics Ltd.",
                Subtitle = "Because so much is riding on your tires.",
                Text = "Contoso Mechanics Ltd. was established in 1961. The sole owner/director is a qualified panel beater with over fourty years of experience. Twelve staff members operate the Body Shop. Their primary focus is the client and achieving total client satisfaction.",
                Images = new List<CardImage> { new CardImage("https://dxnz.blob.core.windows.net/share/Ignite-2016/Contoso-Mechanics.jpg") },
                Buttons = new List<CardAction>
                {
                    new CardAction
                    (
                        ActionTypes.ImBack,
                        "Meeting Minutes",
                        value: "Meeting Minutes"
                    ),
                    new CardAction
                    (
                        ActionTypes.ImBack,
                        "Last Order",
                        value: "Sales Trend for Contoso Mechanics"
                    ),
                    new CardAction
                    (
                        ActionTypes.ImBack,
                        "Any Dues",
                        value: "Any Dues"
                    ),
                    new CardAction
                    (
                        ActionTypes.ImBack,
                        "Key Contacts",
                        value: "Key Contacts for Contoso Mechanics"
                    )
                }
            };

            return heroCard.ToAttachment();
        }
    }
}