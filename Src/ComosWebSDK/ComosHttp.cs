using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;

namespace ComosWebSDK
{
    public class ComosHttp: IDisposable
    {
        HttpClientHandler m_HttpHandler = null;
        HttpClient m_Http = null;
        bool firstTime = true;

        public ComosHttp()
        {
            ServicePointManager.ServerCertificateValidationCallback += (o, certificate, chain, errors) => true;
        }
        public async Task<HttpStatusCode> Connect(string url)
        {
            m_HttpHandler = new HttpClientHandler() { UseCookies = true, AllowAutoRedirect = true };
            m_HttpHandler.Credentials = CredentialCache.DefaultNetworkCredentials;
            m_Http = new HttpClient(m_HttpHandler);
            m_Http.Timeout = TimeSpan.FromMinutes(5);
            var response = await m_Http.GetAsync(url).ConfigureAwait(false);
            return response.StatusCode;
        }
        public async Task<HttpStatusCode> Connect(string url, string domain, string username, string password)
        {
            m_HttpHandler = new HttpClientHandler() { UseCookies = true, AllowAutoRedirect = true };
            m_HttpHandler.Credentials = new NetworkCredential()
            {
                Domain = domain,
                Password = password,
                UserName = username
            };
            m_Http = new HttpClient(m_HttpHandler);
            m_Http.Timeout = TimeSpan.FromMinutes(5);
            var response = await m_Http.GetAsync(url).ConfigureAwait(false);
            return response.StatusCode;
        }

        public Task<HttpResponseMessage> GetAsync(string url)
        {
            return m_Http.GetAsync(url);
        }

        public Task<HttpResponseMessage> PostAsync(string url)
        {
            var content = new StringContent("{}", Encoding.UTF8, "application/json");            

            return m_Http.PostAsync(url,content);
        }

        public Task<HttpResponseMessage> PostAsync(string url, string serverName)
        {
            var content = new StringContent("{}", Encoding.UTF8, "application/json");
            //var content = new StringContent("{userName : \"caio.barreto\",password: \"\"  }", Encoding.UTF8, "application/json");
            //m_Http.DefaultRequestHeaders.Add("Accept", "application/json, text/plain, */*");
            //m_Http.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
            //m_Http.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9");
            ////m_Http.DefaultRequestHeaders.Add("Content-Length", "0");
            //m_Http.DefaultRequestHeaders.Add("clienttechnology", "html");
            //m_Http.DefaultRequestHeaders.Add("Connection", "keep-alive");
            //m_Http.DefaultRequestHeaders.Add("product", "COMOS");
            //m_Http.DefaultRequestHeaders.Add("Origin", "http://evil.com/");
            //m_Http.DefaultRequestHeaders.Add("Referer", "http://siemens.southcentralus.cloudapp.azure.com/WebView/index.html");            
            //m_Http.DefaultRequestHeaders.Add("X-XSRF-TOKEN", "0148b9ce-c66c-4113-937f-b14ea86b9f44");

            //I make a new Request Header with the XSRF-TOKEN that was received in a cookie in the first Connect.
            //This Request Header will remain configured for future HTTP communications.

            if (firstTime) //&& version == "10.2.4"
            {
                Uri myCookieUri = new Uri(serverName);
                var myCookieContanier = m_HttpHandler.CookieContainer.GetCookies(myCookieUri);
                var myXSRFCookie = myCookieContanier["XSRF-TOKEN"];
                m_Http.DefaultRequestHeaders.Add("X-XSRF-TOKEN", myXSRFCookie.Value);
                firstTime = false;
            }

            return m_Http.PostAsync(url, content);
        }

            public Task<HttpResponseMessage> PostAsync(string url, StreamContent content)
        {            
            return m_Http.PostAsync(url, content);
        }

        public Task<HttpResponseMessage> PostAsync(string url, StringContent content)
        {
            return m_Http.PostAsync(url, content);
        }

        public void UseSession(string id, string serverName)
        {
            //Cookie mySessionID = new Cookie("Comos.Web.Session", id);
            //Uri myCookieUri = new Uri(serverName);
            //m_HttpHandler.CookieContainer.Add(myCookieUri, mySessionID);
            m_Http.DefaultRequestHeaders.Add("Cookie", "_ga=GA1.2.1398315612.1487785879; Comos.Web.Session=" + id);
        }
        public void ClearSession()
        {
            m_Http.DefaultRequestHeaders.Remove("Cookie");
            
        }


        public void Dispose()
        {
            m_Http.Dispose();
            m_HttpHandler.Dispose();
        }
    }
}
