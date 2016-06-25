using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abb.Cz.Apps.WattCountdown.Services
{
    public class WattParser
    {

        //public DateTime DateToParse { get; set; }
        public string ReportHtml { get; set; }

        public WattParser(string reportHtml)
        {
            
            ReportHtml = reportHtml;
        }

        public List<WattEntry> ParseEntries(DateTime dateToParse)
        {
            HtmlAgilityPack.HtmlDocument html = new HtmlAgilityPack.HtmlDocument();
            html.OptionDefaultStreamEncoding = Encoding.UTF8;
            
            html.LoadHtml(ReportHtml);
            var nodes = html.DocumentNode.SelectNodes("//td[@class='day']"); //[class='page']");
            var today = nodes.Single(td => td.InnerText.Contains($"&nbsp;{dateToParse.Day}."));
            var todayEntries = today.ParentNode.ParentNode.ParentNode.SelectNodes(".//td");

            var entries = new List<WattEntry>();
            for (int i = 0; i< todayEntries.Count; i++)
            {
                var todayEntry = todayEntries[i];
                if (todayEntry.GetAttributeValue("class", "") == "d4" && !string.IsNullOrEmpty(todayEntry.InnerText))
                {
                    var time = todayEntry.InnerText;
                    var typ = todayEntries[i + 2].InnerText;
                    var entry = new WattEntry(time, typ);
                    entries.Add(entry);
                }
            }

            return entries;
        }


    }

    public class WattEntry
    {
        public DateTime EntryTime { get; set; }
        public EntryType Type { get; set; }

        public WattEntry(string rawTime, string type)
        {
            EntryTime = DateTime.Parse(rawTime);
            Type = ParseEntryType(type);
        }

        private EntryType ParseEntryType(string type)
        {
            if(type.Contains("Př"))
                return EntryType.Prichod;
            else if(type.Contains("Od"))
                return EntryType.Odchod;

            throw new NotSupportedException($"Unknown entry type {type}");
        }
    }

    public enum EntryType
    {
        Prichod,
        Odchod
    }
}
