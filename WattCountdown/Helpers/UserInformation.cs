using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Abb.Cz.Apps.WattCountdown.Helpers
{
    internal class UserInformation
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string Title { get; set; }

        public TimeSpan StartTime { get; set; } = DateTime.Now.TimeOfDay;

        public TimeSpan TotalTime { get; set; }

        public bool Valid { get; set; }

        public bool MissingStartDate { get; set; }

        public bool? OutOfOffice { get; set; }

        public UserInformation(string wattHtml)
        {
            CreateUserInfromationFromWattHtml(wattHtml);
        }

        private static void CreateUserInfromationFromWattHtml(string wattHtml)
        {
            var html = new HtmlDocument();
            html.LoadHtml(wattHtml);
            var row = html.DocumentNode.SelectSingleNode("//td[@class='d1' and text()='" + DateTime.Today.Day + ".']")?.ParentNode;
            //try
            //{
            //    string str1 = "<TD class=\"pc1\">";
            //    string str2 = "<TD class=\"d1\">" + (object)DateTime.Now.Day + ".</TD>";
            //    string str3 = wattHtml.Substring(wattHtml.IndexOf(str1) + str1.Length);
            //    var watTimes = new List<WatTime>();
            //    string str4 = str3.Substring(0, str3.IndexOf("</TD>"));
            //    this.surname = str4.Substring(0, str4.IndexOf(" ")).Trim();
            //    int num = str4.IndexOf(",");
            //    this.name = num == -1 ? str4.Substring(this.surname.Length).Trim() : str4.Substring(this.surname.Length, num - this.surname.Length).Trim();
            //    if (str4.IndexOf(",") > 0)
            //        this.title = str4.Substring(str4.IndexOf(",") + 1).Trim();
            //    string str5 = wattHtml.Substring(wattHtml.IndexOf(str2) + str2.Length);
            //    string html = str5.Substring(0, str5.IndexOf("</TR>")).Trim();
            //    for (WatTime watTime = this.getWatTime(ref html); watTime.enter.HasValue; watTime = this.getWatTime(ref html))
            //    {
            //        watTimes.Add(watTime);
            //        bool? nullable = watTime.enter;
            //        this.outOfOffice = nullable.HasValue ? new bool?(!nullable.GetValueOrDefault()) : new bool?();
            //    }
            //    this.startTime = this.getStartTime(watTimes);
            //    //this.totalTime = this.getTotalTime(watTimes);
            //    this.valid = true;
            //}
            //catch
            //{
            //    this.valid = false;
            //}
        }

        private TimeSpan GetStartTime(IEnumerable<WattTime> watTimes)
        {
            this.MissingStartDate = true;
            foreach (var watTime in watTimes)
            {
                var nullable = watTime.enter;
                if ((!nullable.GetValueOrDefault() ? 0 : (nullable.HasValue ? 1 : 0)) != 0)
                    return watTime.time;
            }
            this.MissingStartDate = true;
            return DateTime.Now.TimeOfDay;
        }

        //private TimeSpan getTotalTime(List<WatTime> watTimes)
        //{
        //    TimeSpan timeSpan = new TimeSpan(0L);
        //    DateTime dateTime = DateTime.Now;
        //    bool flag = false;
        //    foreach (var watTime in watTimes)
        //    {
        //        bool? nullable = watTime.enter;
        //        if ((!nullable.GetValueOrDefault() ? 0 : (nullable.HasValue ? 1 : 0)) != 0)
        //        {
        //            if (flag)
        //            {
        //                timeSpan += watTime.time.Subtract(dateTime);
        //                flag = false;
        //            }
        //        }
        //        else
        //        {
        //            dateTime = watTime.time;
        //            flag = true;
        //        }
        //    }
        //    return timeSpan;
        //}

        private WattTime getWatTime(ref string html)
        {
            string str1 = "<TD class=\"d3\">";
            string str2 = "<TD class=\"d5\">";
            DateTime now = DateTime.Now;
            WattTime watTime = new WattTime();
            try
            {
                html = html.Substring(html.IndexOf(str1) + str1.Length);
                string str3 = html.Substring(0, html.IndexOf("</TD>"));
                html = html.Substring(str3.Length);
                watTime.time = str3.Length != 0 ? new TimeSpan((int)Convert.ToInt16(str3.Substring(0, str3.IndexOf(":"))), (int)Convert.ToInt16(str3.Substring(str3.IndexOf(":") + 1), 0), 0) : now.TimeOfDay;
                html = html.Substring(html.IndexOf(str2) + str2.Length);
                string str4 = html.Substring(0, html.IndexOf("</TD>")).Trim();
                html = html.Substring(str4.Length);
                watTime.enter = str4.Length != 0 ? new bool?(str4.StartsWith("P")) : new bool?();
            }
            catch
            {
                watTime.time = now.TimeOfDay;
                watTime.enter = new bool?();
            }
            return watTime;
        }

        private struct WattTime
        {
            public TimeSpan time;
            public bool? enter;
        }
    }
}
