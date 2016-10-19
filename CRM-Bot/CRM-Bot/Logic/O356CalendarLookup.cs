using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Threading.Tasks;

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
    }
}