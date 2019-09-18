using System;
using System.IO;
using System.Net;
using System.Text;

namespace Abb.Cz.Apps.WattCountdown.Helpers
{
    class Watt
    {
        private const string WattBaseUrl = "http://czbrq-s-apl0007:8080/reports/";
        private const string WattLoginUrl = WattBaseUrl + "dologin.jsp";
        private const string WattReportUrl = WattBaseUrl + "report_fetcher.jsp?taskId";

        private readonly CookieContainer _cookieJar = new CookieContainer();
        private readonly string _userName;
        private readonly string _password;

        internal Watt(string userName, string password)
        {
            _userName = userName;
            _password = password;
        }

        public string LoginAndGetReportHtml()
        {
            var responseUrl = LoginAndGetResponseUrl();

            var reportUrl = new Uri(WattReportUrl + responseUrl.Substring(responseUrl.IndexOf("=-")) + "&repId=2");
            var reportResponse = GetResponseFromWattRequest(reportUrl, RequestMethod.Get);
            var reportRawHtml = GetRawStringFromResponse(reportResponse);
            return reportRawHtml;
        }

        private string LoginAndGetResponseUrl()
        {
            var response = GetResponseFromWattRequest(new Uri(WattLoginUrl), RequestMethod.Post, $"app=&username={_userName}&password={_password}");
            if (response == null)
                throw new Exception("Unable to pass the login process.");
            var responseUrl = response.ResponseUri.ToString();

            response.Close();

            return responseUrl;
        }


        public UserInformation GetUserInformation()
        {
            try
            {
                var response = GetResponseFromWattRequest(new Uri(WattLoginUrl), RequestMethod.Post, $"app=&username={_userName}&password={_password}");
                var responseUrl = response.ResponseUri.ToString();

                response.Close();

                if (responseUrl.Equals(WattLoginUrl))
                    return null;

                return new UserInformation(GetRawStringFromResponse(GetResponseFromWattRequest(new Uri(WattReportUrl + responseUrl.Substring(responseUrl.IndexOf("=-")) + "&repId=1"), RequestMethod.Get)));

            }
            catch
            {
                return null;
            }
        }

        private string GetRawStringFromResponse(HttpWebResponse response)
        {
            if (response == null)
                throw new Exception("Unable to get report html.");

            using (var sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                var responseString = sr.ReadToEnd();
                response.Close();

                return responseString;
            }
        }

        private HttpWebResponse GetResponseFromWattRequest(Uri requestUrl, RequestMethod method, string postData = "")
        {
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUrl);

                httpWebRequest.Credentials = CredentialCache.DefaultCredentials;
                httpWebRequest.CookieContainer = _cookieJar;
                httpWebRequest.UserAgent = "CSharp HTTP Sample";
                httpWebRequest.KeepAlive = true;
                httpWebRequest.Headers.Set("Pragma", "no-cache");
                httpWebRequest.Timeout = 300000;
                httpWebRequest.Method = method.ToString();
                if (method == RequestMethod.Post)
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
            Post,
            Get
        }
    }
}
