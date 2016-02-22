using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abb.Cz.Apps.WattCountdown.Models
{
    public class CountdownModel
    {
        public TimeSpan StartTime { get; set; }

        public TimeSpan Lunch { get; set; }

        public double WorkTime { get; set; }

        public DateTime EndDate { get; set; }

        public bool LunchVoucher { get; set; }

        public TimeSpan Countdown { get; set; }

        public CountdownModel()
        {
            SetDefaultData();
        }

        private void SetDefaultData()
        {
            var now = DateTime.Now;
            StartTime = now.TimeOfDay.Subtract(new TimeSpan(0, 0, now.TimeOfDay.Seconds));
            WorkTime = 8;
            Lunch = new TimeSpan(0, 30, 0);
            LunchVoucher = true;
        }
    }
}
