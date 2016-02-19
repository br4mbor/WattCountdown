using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abb.Cz.Apps.WattCountdown.Models
{
    public class CountdownModel
    {
        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public DateTime Lunch { get; set; }

        public double WorkTime { get; set; }

        public CountdownModel()
        {
            Start = DateTime.Now;
            WorkTime = 8;
            Lunch = new DateTime(1, 1, 1, 0, 30, 0);
            End = Start.AddHours(WorkTime).AddHours(Lunch.Hour).AddMinutes(Lunch.Minute);
        }
    }
}
