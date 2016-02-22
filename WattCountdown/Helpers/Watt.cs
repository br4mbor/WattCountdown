using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Abb.Cz.Apps.WattCountdown.Helpers
{
    class Watt
    {
        //private CookieContainer CookieJar = new CookieContainer();
        //private string userName;
        //private string password;

        //Watt(string userName, string password)
        //{
        //    this.userName = userName;
        //    this.password = password;
        //}

        //public UserInformation getUserInformation()
        //{
        //    try
        //    {
        //        HttpWebResponse requestData = this.getRequestData(new Uri("http://czbrq-s-apl0007:8080/reports/dologin.jsp"), "POST", this.proxyUserName, this.proxyPassword, this.proxyAddress, this.proxyPort, "app=&username=" + this.watUserName + "&password=" + this.watPassword);
        //        string str = requestData.ResponseUri.ToString();
        //        if (requestData != null)
        //            requestData.Close();
        //        return new UserInformation(this.getRawStringFromResponse(this.getRequestData(new Uri("http://czbrq-s-apl0007:8080/reports/report_fetcher.jsp?taskId" + str.Substring(str.IndexOf("=-")) + "&repId=1"), "GET", this.proxyUserName, this.proxyPassword, this.proxyAddress, this.proxyPort, "")));
        //    }
        //    catch (Exception ex)
        //    {
        //        return new UserInformation();
        //    }
        //}

        //private string getRawStringFromResponse(HttpWebResponse httpWebResponse)
        //{
        //    StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.ASCII);
        //    string str = streamReader.ReadToEnd();
        //    streamReader.Close();
        //    if (httpWebResponse != null)
        //    {
        //        httpWebResponse.Close();
        //        httpWebResponse = (HttpWebResponse)null;
        //    }
        //    return str;
        //}

        //private HttpWebResponse getRequestData(Uri URL, string Method, string proxyUserName, string proxyPassword, string proxyAddress, string proxyPort, string postData)
        //{
        //    try
        //    {
        //        if (proxyAddress.Length > 0)
        //            GlobalProxySelection.Select = (IWebProxy)new WebProxy(proxyAddress + ":" + proxyPort, true);
        //        HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(URL);
        //        if (proxyUserName.Length > 0)
        //        {
        //            NetworkCredential networkCredential = new NetworkCredential(proxyUserName, proxyPassword);
        //            httpWebRequest.Credentials = (ICredentials)new CredentialCache()
        //  {
        //    {
        //      URL,
        //      "Basic",
        //      networkCredential
        //    }
        //  };
        //        }
        //        else
        //            httpWebRequest.Credentials = CredentialCache.DefaultCredentials;
        //        httpWebRequest.CookieContainer = this.CookieJar;
        //        httpWebRequest.UserAgent = "CSharp HTTP Sample";
        //        httpWebRequest.KeepAlive = true;
        //        httpWebRequest.Headers.Set("Pragma", "no-cache");
        //        httpWebRequest.Timeout = 300000;
        //        httpWebRequest.Method = Method;
        //        if ("POST" == Method)
        //        {
        //            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
        //            byte[] bytes = Encoding.ASCII.GetBytes(postData);
        //            httpWebRequest.ContentLength = (long)bytes.Length;
        //            Stream requestStream = httpWebRequest.GetRequestStream();
        //            requestStream.Write(bytes, 0, bytes.Length);
        //            requestStream.Close();
        //        }
        //        return (HttpWebResponse)httpWebRequest.GetResponse();
        //    }
        //    catch (Exception ex)
        //    {
        //        return (HttpWebResponse)null;
        //    }
        //}
    }
}
