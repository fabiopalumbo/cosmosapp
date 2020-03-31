using ComosWebSDK;
using ComosWebSDK.Data;
using Newtonsoft.Json;
using NtlmHttpHandler;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XComosWebSDK
{
    public class CommunicationManager : ICommunicationManager
    {
        #region Properties
        public string XsrfToken { get; set; }

        public CancellationTokenSource TokenSource { get; set; }
        #endregion

        #region Private Method's
        private async Task<string> GetFromComosWeb(string url, string sessionId, string serverName)
        {
            using (var handler = NtlmHttpHandlerFactory.Create())
            {
                AddSession(handler, sessionId, serverName);
                using (var comosHttpClient = new ComosHttp(handler))
                {
                    Uri myCookieUri = new Uri(serverName);
                    handler.CookieContainer.Add(myCookieUri, new Cookie("XSRF-TOKEN", XsrfToken));
                    comosHttpClient.DefaultRequestHeaders.Add("X-XSRF-TOKEN", XsrfToken);
                    var response = await comosHttpClient.GetAsync(url, TokenSource.Token).ConfigureAwait(false);

                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        throw new Exception(response.ReasonPhrase);
                    }
                }
            }
        }

        private async Task<HttpResponseMessage> PostToComosWeb(string url, string serverName)
        {
            var content = new StringContent("{}", Encoding.UTF8, "application/json");

            using (var handler = NtlmHttpHandlerFactory.Create())
            {
                using (var comosHttpClient = new ComosHttp(handler))
                {
                    Uri myCookieUri = new Uri(serverName);
                    handler.CookieContainer.Add(myCookieUri, new Cookie("XSRF-TOKEN", XsrfToken));
                    comosHttpClient.DefaultRequestHeaders.Add("X-XSRF-TOKEN", XsrfToken);
                    return await comosHttpClient.PostAsync(url, content).ConfigureAwait(false);
                }
            }
        }

        private void AddSession(HttpClientHandler handler, string sessionId, string serverName)
        {
            Cookie mySessionID = new Cookie("Comos.Web.Session", sessionId);
            Uri myCookieUri = new Uri(serverName);
            handler.CookieContainer.Add(myCookieUri, mySessionID);
        }
        #endregion

        #region Constructor
        public CommunicationManager()
        {
            TokenSource = new CancellationTokenSource();
        }
        #endregion

        #region Interface Implementations
        public async Task<HttpStatusCode> Connect(string url, string serverName)
        {
            using (var handler = NtlmHttpHandlerFactory.Create())
            {
                using (var comosHttpClient = new ComosHttp(handler))
                {
                    var response = comosHttpClient.GetAsync($"{url}/settings").GetAwaiter().GetResult();
                    if (response.IsSuccessStatusCode)
                    {
                        Uri myCookieUri = new Uri(serverName);
                        var myCookieContanier = handler.CookieContainer.GetCookies(myCookieUri);
                        var myXSRFCookie = myCookieContanier["XSRF-TOKEN"];
                        XsrfToken = myXSRFCookie.Value;
                        return response.StatusCode;
                    }
                    else
                    {
                        throw new Exception(response.ReasonPhrase);
                    }

                }
            }
        }

        public async Task<string> PostLogin(string url, string serverName)
        {

            var response = await PostToComosWeb($"{url}/sessions/actions/Login", serverName).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }

        public async Task<string> PostLogout(string url, string sessionId, string serverName)
        {
            var content = new StringContent("{}", Encoding.UTF8, "application/json");

            using (var handler = NtlmHttpHandlerFactory.Create())
            {
                AddSession(handler, sessionId, serverName);
                using (var comosHttpClient = new ComosHttp(handler))
                {
                    Uri myCookieUri = new Uri(serverName);
                    handler.CookieContainer.Add(myCookieUri, new Cookie("XSRF-TOKEN", XsrfToken));
                    comosHttpClient.DefaultRequestHeaders.Add("X-XSRF-TOKEN", XsrfToken);
                    var response = await comosHttpClient.PostAsync(url + "sessions/actions/logout", content, TokenSource.Token).ConfigureAwait(false);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        throw new Exception(response.ReasonPhrase);
                    }
                }
            }
        }

        public async Task<string> GetDatabases(string url, string sessionId, string serverName)
        {
            return await GetFromComosWeb($"{url}/dbs", sessionId, serverName);
        }

        public async Task<string> GetProjects(CDatabase db, string url, string sessionId, string serverName)
        {
            return await GetFromComosWeb($"{url}/dbs/{db.Key}/projects", sessionId, serverName);
        }

        public async Task<string> GetWorkingOverlays(string db, string projectUID, string layerUID, string url, string sessionId, string serverName)
        {
            return await GetFromComosWeb($"{url}/dbs/{db}/projects/{projectUID}/overlays?overlayid={layerUID}", sessionId, serverName);
        }

        public async Task<string> GetObject(string url, string db, string projectUID, string uid, string lcid, string layerUID, string sessionId, string serverName)
        {
            return await GetFromComosWeb($"{url}/dbs/{db}/projects/{projectUID}/objects/{uid}?lcid={lcid}&overlayid={layerUID}", sessionId, serverName);
        }

        public async Task<string> GetDefaultLanguage(string url, string db, string projectUID, string layerUID, string sessionId, string serverName)
        {
            return await GetFromComosWeb($"{url}/dbs/{db}/projects/{projectUID}/languages/default?lcid=&overlayid={layerUID}", sessionId, serverName);
        }

        public async Task<string> GetNavigatorNodes_Children(string url, string db, string projectUID, string target, string lcid, string layer, string parent, string viewstate, string sessionId, string serverName)
        {
            if (string.IsNullOrEmpty(parent))
                return await GetFromComosWeb($"{url}/dbs/{db}/projects/{projectUID}/navigator/nodes/{target}/children?lcid={lcid}&overlayid={layer}&view={viewstate}", sessionId, serverName);
            else
                return await GetFromComosWeb($"{url}/dbs/{db}/projects/{projectUID}/navigator/nodes/{target}/children?lcid={lcid}&overlayid={layer}&parent={parent}&view={viewstate}", sessionId, serverName);
        }

        public async Task<string> GetNavigatorNodes_TreeNodes(string url, CWorkingLayer layer, string target, string lcid, string viewstate, string sessionId, string serverName)
        {
            return await GetFromComosWeb($"{url}/dbs/{layer.Database}/projects/{layer.ProjectUID}/navigator/nodes/{target}/treenodes?lcid={lcid}&overlayid={layer.UID}&view={viewstate}", sessionId, serverName);
        }

        public async Task<string> GetObjectSpecification(string url, string db, string projectUID, string target, string lcid, string layer, string sessionId, string serverName)
        {
            return await GetFromComosWeb($"{url}/dbs/{db}/projects/{projectUID}/objects/{target}/specifications?lcid={lcid}&overlayid={layer}", sessionId, serverName);
        }

        public async Task<string> GetObjectSpecificationAsHtml(string url, string db, string projectUID, string target, string spec, string lcid, string layer, string sessionId, string serverName)
        {
            return await GetFromComosWeb($"{url}/ui/dbs/{db}/projects/{projectUID}/objects/{target}/specifications/{spec}/html?lcid={lcid}&overlayid={layer}", sessionId, serverName);
        }

        public async Task<string> GetqueriesResult(string url, string db, string projectUID, string target, string lcid, string layer, string start_objid, string sessionId, string serverName)
        {
            string completeUrl = $"{url}/dbs/{db}/projects/{projectUID}/queries/{target}/result?lcid={lcid}&overlayid={layer}";
            if (start_objid != null)
                completeUrl += $"&start_objid={start_objid}";

            return await GetFromComosWeb(completeUrl, sessionId, serverName);
        }

        public string GetPictureUrl(string url, string database, string key)
        {
            return $"{url}/dbs/{database}/pictures/{key}";
        }

        public async Task<HttpResponseMessage> GetDocumentStream(string url, string database, string projectuid, string layer, string docuid, bool exportpdf, string sessionId, string serverName)
        {
            string completeUrl = $"{url}/dbs/{database}/projects/{projectuid}/documents/{docuid}?overlayid={layer}";
            if (exportpdf)
                completeUrl += "&export_type=report2pdf";

            using (var handler = NtlmHttpHandlerFactory.Create())
            {
                AddSession(handler, sessionId, serverName);
                using (var comosHttpClient = new ComosHttp(handler))
                {
                    return await comosHttpClient.GetAsync(completeUrl);
                }
            }
        }

        public async Task<HttpResponseMessage> DoHeartbeat(string url, string sessionId, string serverName)
        {
            using (var handler = NtlmHttpHandlerFactory.Create())
            {
                AddSession(handler, sessionId, serverName);
                using (var comosHttpClient = new ComosHttp(handler))
                {
                    var json = JsonConvert.SerializeObject(
                            new
                            {
                                text = sessionId
                            });

                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    Uri myCookieUri = new Uri(serverName);
                    handler.CookieContainer.Add(myCookieUri, new Cookie("XSRF-TOKEN", XsrfToken));
                    comosHttpClient.DefaultRequestHeaders.Add("X-XSRF-TOKEN", XsrfToken);
                    return await comosHttpClient.PostAsync(url + "sessions/actions/send_heartbeat", content).ConfigureAwait(false);
                }
            }
        }
        #endregion
    }
}

