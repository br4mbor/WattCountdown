using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abb.Cz.Apps.WattCountdown.Models
{
    public class CountdownModel
    {
        private DateTime start;

        public DateTime Start
        {
            get { return start; }
            set { start = value.Subtract(new TimeSpan(0, 0, 0, value.Second, value.Millisecond)); }
        }

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
            Start = DateTime.Now;
            WorkTime = 8;
            Lunch = new TimeSpan(0, 30, 0);
            LunchVoucher = true;
        }
    }
}
