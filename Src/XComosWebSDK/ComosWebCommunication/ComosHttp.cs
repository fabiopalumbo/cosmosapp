using System.Net.Http;
using System.Net;
using static XComosWebSDK.ApplicationProperties;

namespace ComosWebSDK
{
    public class ComosHttp : HttpClient
    {
        public ComosHttp(HttpClientHandler handler) : base(handler)
        {
            handler.Credentials = new NetworkCredential()
            {
                Domain = GetApplicationCurrentProperty("domain").ToString(),
                Password = GetApplicationCurrentProperty("password").ToString(),
                UserName = GetApplicationCurrentProperty("username").ToString()
            };
        }
    }
}
