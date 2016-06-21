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
        public void Parse()
        {
            FileInfo file = new FileInfo("D:\\WATTSestavy.htm");
            HtmlAgilityPack.HtmlDocument html = new HtmlAgilityPack.HtmlDocument();
            html.OptionDefaultStreamEncoding = Encoding.UTF8;
            
            html.Load("D:\\WATT.htm");
            var nodes = html.DocumentNode.SelectNodes("//td[@class='day']"); //[class='page']");
            //var today = nodes.Single(td => td.InnerText.Contains($"&nbsp;{DateTime.Now.Date.Day}."));
            var today = nodes.Single(td => td.InnerText.Contains($"&nbsp;17."));
            //var entries = today.SelectNodes(".//td");
            var todayEntries = today.ParentNode.ParentNode.ParentNode.SelectNodes(".//td");

            var times = new List<string>();
            var prichodShouldFollow = true;
            for (int i = 0; i< todayEntries.Count; i++)
            {
                var todayEntry = todayEntries[i];
                if (todayEntry.GetAttributeValue("class", "") == "d4" && !string.IsNullOrEmpty(todayEntry.InnerText))
                {
                    var typ = todayEntries[i + 2].InnerText;
                    if (prichodShouldFollow)
                    {
                        if (!typ.Contains("Př"))
                        {
                            Debugger.Break();
                            break;
                        }
                    }
                    else
                    {
                        if (!typ.Contains("Od"))
                        {
                            Debugger.Break();
                            break;
                        }
                    }

                    times.Add(todayEntry.InnerText);
                    prichodShouldFollow = !prichodShouldFollow;
                }
                
            }
            
        }
    }
}
