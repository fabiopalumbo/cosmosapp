using ComosWebSDK.Data;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace XComosWebSDK
{
    public interface ICommunicationManager
    {
        CancellationTokenSource TokenSource { get; set; }

        string XsrfToken { get; set; }

        Task<HttpStatusCode> Connect(string url, string serverName);

        Task<string> PostLogin(string url, string serverName);

        Task<string> PostLogout(string url, string sessionId, string serverName);

        Task<string> GetDatabases(string url, string sessionId, string serverName);

        Task<string> GetProjects(CDatabase db, string url, string sessionId, string serverName);

        Task<string> GetWorkingOverlays(string db, string projectUID, string layerUID, string url, string sessionId, string serverName);

        Task<string> GetObject(string url, string db, string projectUID, string uid, string lcid, string layerUID, string sessionId, string serverName);

        Task<string> GetDefaultLanguage(string url, string db, string projectUID, string layerUID, string sessionId, string serverName);

        Task<string> GetNavigatorNodes_Children(string url, string db, string projectUID, string target, string lcid, string layer, string parent, string viewstate, string sessionId, string serverName);

        Task<string> GetNavigatorNodes_TreeNodes(string url, CWorkingLayer layer, string target, string lcid, string viewstate, string sessionId, string serverName);

        Task<string> GetObjectSpecification(string url, string db, string projectUID, string target, string lcid, string layer, string sessionId, string serverName);

        Task<string> GetObjectSpecificationAsHtml(string url, string db, string projectUID, string target, string spec, string lcid, string layer, string sessionId, string serverName);

        Task<string> GetqueriesResult(string url, string db, string projectUID, string target, string lcid, string layer, string start_objid, string sessionId, string serverName);

        string GetPictureUrl(string url, string database, string key);

        Task<HttpResponseMessage> DoHeartbeat(string url, string sessionId, string serverName);

        Task<HttpResponseMessage> GetDocumentStream(string url, string database, string projectuid, string layer, string docuid, bool exportpdf, string sessionId, string serverName);
    }
}