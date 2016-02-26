using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Abb.Cz.Apps.WattCountdown.Helpers
{
    class Watt
    {
        private const string WattBaseUrl = "http://czbrq-s-apl0007:8080/reports/";
        private const string WattLoginUrl = WattBaseUrl + "dologin.jsp";
        private const string WattReportUrl = WattBaseUrl + "report_fetcher.jsp?taskId";

        private CookieContainer cookieJar = new CookieContainer();
        private string userName;
        private string password;

        internal Watt(string userName, string password)
        {
            this.userName = userName;
            this.password = password;
        }

        public UserInformation GetUserInformation()
        {
            try
            {
                var response = GetResponseFromWattRequest(new Uri(WattLoginUrl), RequestMethod.POST, string.Format("app=&username={0}&password={1}", userName, password));

                string responseUrl = response.ResponseUri.ToString();

                if (response != null)
                    response.Close();

                if (responseUrl.Equals(WattLoginUrl))
                    return null;

                return new UserInformation(GetRawStringFromResponse(GetResponseFromWattRequest(new Uri(WattReportUrl + responseUrl.Substring(responseUrl.IndexOf("=-")) + "&repId=1"), RequestMethod.GET)));

            }
            catch
            {
                return null;
            }
        }

        private string GetRawStringFromResponse(HttpWebResponse response)
        {
            using (var sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                var responseString = sr.ReadToEnd();
                if (response != null)
                {
                    response.Close();
                    response = null;
                }
                return responseString;
            }
        }

        private HttpWebResponse GetResponseFromWattRequest(Uri requestUrl, RequestMethod method, string postData = "")
        {
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUrl);

                httpWebRequest.Credentials = CredentialCache.DefaultCredentials;
                httpWebRequest.CookieContainer = cookieJar;
                httpWebRequest.UserAgent = "CSharp HTTP Sample";
                httpWebRequest.KeepAlive = true;
                httpWebRequest.Headers.Set("Pragma", "no-cache");
                httpWebRequest.Timeout = 300000;
                httpWebRequest.Method = method.ToString();
                if (method == RequestMethod.POST)
                {
                    httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                    byte[] bytes = Encoding.ASCII.GetBytes(postData);
                    httpWebRequest.ContentLength = bytes.Length;
                    Stream requestStream = httpWebRequest.GetRequestStream();
                    requestStream.Write(bytes, 0, bytes.Length);
                    requestStream.Close();
                }
                return (HttpWebResponse)httpWebRequest.GetResponse();
            }
            catch
            {
                return null;
            }
        }

        private enum RequestMethod
        {
            POST,
            GET
        }
    }   
}
