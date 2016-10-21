using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CRM_Bot.Logic
{
    public class CRMLookup
    {
        public static IList<Attachment> GetKeyContacts(string accountName)
        {

            //Task task = Task.Delay(2000);
            //task.Wait();

            return new List<Attachment>()
            {
                GetHeroCard(
                    "Alan Steiner",
                    "CTO",
                    new CardImage(url: "https://dxnz.blob.core.windows.net/share/Ignite-2016/Alan-Steiner.jpg"),
                    new List<CardAction>
                    {
                        new CardAction(ActionTypes.OpenUrl, "Call: +6422222", value: "tel:+6422222"),
                        new CardAction(ActionTypes.OpenUrl, "Email: alans@contoso.com", value: "mailto:alans@contoso.com")

                    }),
                GetHeroCard(
                    "Amy Alberts",
                    "CEO",
                    new CardImage(url: "https://dxnz.blob.core.windows.net/share/Ignite-2016/Amy-Alberts.jpg"),
                    new List<CardAction>
                    {
                        new CardAction(ActionTypes.OpenUrl, "Call: +6433333", value: "tel:+6433333"),
                        new CardAction(ActionTypes.OpenUrl, "Email: amya@contoso.com", value: "mailto:amya@contoso.com")

                    }),
                GetHeroCard(
                    "Jeff Hay",
                    "CFO",
                    new CardImage(url: "https://dxnz.blob.core.windows.net/share/Ignite-2016/Jeff-Hay.jpg"),
                    new List<CardAction>
                    {
                        new CardAction(ActionTypes.OpenUrl, "Call: +6466666", value: "tel:+6466666"),
                        new CardAction(ActionTypes.OpenUrl, "Email: jeffh@contoso.com", value: "mailto:jeffh@contoso.com")

                    }),
                GetHeroCard(
                    "Renee Lo",
                    "CMO",
                    new CardImage(url: "https://dxnz.blob.core.windows.net/share/Ignite-2016/Renee-Lo.jpg"),
                    new List<CardAction>
                    {
                        new CardAction(ActionTypes.OpenUrl, "Call: +6499999", value: "tel:+6499999"),
                        new CardAction(ActionTypes.OpenUrl, "Email: reneel@contoso.com", value: "mailto:reneel@contoso.com")

                    })
            };
        }

        private static Attachment GetHeroCard(string title, string subtitle, CardImage cardImage, List<CardAction> cardActions)
        {
            var heroCard = new HeroCard
            {
                Title = title,
                Subtitle = subtitle,
                Images = new List<CardImage>() { cardImage },
                Buttons = cardActions,
            };

            return heroCard.ToAttachment();
        }

        public static Attachment GetSalesTrend(string accountName)
        {
            var attachment = new Attachment()
            {
                ContentUrl = "https://dxnz.blob.core.windows.net/share/Ignite-2016/sales-trend.png",
                ContentType = "image/png",
                Name = "Sales Trend for Contoso Mechanics Ltd."
            };

            return attachment;
        }

    }
}