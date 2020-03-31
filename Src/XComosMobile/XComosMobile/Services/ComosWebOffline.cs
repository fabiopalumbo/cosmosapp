using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComosWebSDK;
using ComosWebSDK.Data;

namespace XComosMobile.Services
{
    public class ComosWebOffline : IComosWeb
    {
        string m_Url;
        public string URL => m_Url;
        IDatabase m_DataBase = null;
        public ComosWeb ComosWeb => null;
        public ComosWebOffline(IDatabase db, string url)
        {
            m_Url = url + "/api/webview/v1/";
            m_DataBase = db;
        }

        #region heartbeat implementation
        public int HeartBeat => -1;

        public bool DoHeartBeat()
        {
            return true;
        }

        public void StartHeartBeat()
        {
        }

        public void StopHeartbeat()
        {
        }
        #endregion

        public async Task<System.Net.HttpStatusCode> Connect(
            string domain, string username, string password)
        {
            return await Task.FromResult<System.Net.HttpStatusCode>(System.Net.HttpStatusCode.OK);
        }

        public Task<bool> Logout()
        {
            return Task.FromResult<bool>(true);
        }

        public void Dispose()
        {

        }

        public Task<CInfo> GetDatabases()
        {
            var url = m_Url + "dbs";
            string content = m_DataBase.ReadUrlCache(url);
            if (content == null)
                return Task.FromResult<CInfo>(null);

            var result = Task.FromResult<CInfo>(Newtonsoft.Json.JsonConvert.DeserializeObject<CInfo>(content));
            return result;
        }

        public Task<List<CObject>> GetNavigatorNodes_Children(string db, string ProjectUID, string layer, string lcid, string target, string viewstate, string parent = null)
        {
            string url = null;
            if (string.IsNullOrEmpty(parent))
                url = m_Url + "dbs/" + db + "/projects/" + ProjectUID + "/navigator/nodes/" + target + "/children?lcid=" + lcid + "&overlayid=" + layer + "&view=" + viewstate;
            else
                url = m_Url + "dbs/" + db + "/projects/" + ProjectUID + "/navigator/nodes/" + target + "/children?lcid=" + lcid + "&overlayid=" + layer + "&parent=" + parent + "&view=" + viewstate;

            string content = m_DataBase.ReadUrlCache(url);
            if (content == null)
                return Task.FromResult<List<CObject>>(null);
            var result = Task.FromResult<List<CObject>>(Newtonsoft.Json.JsonConvert.DeserializeObject<List<CObject>>(content));
            return result;
        }

        public Task<CSystemObject> GetObject(string db, string projectuid, string layeruid, string uid, string lcid = "")
        {
            var url = m_Url + "dbs/" + db + "/projects/" + projectuid + "/objects/" + uid + "?lcid=" + lcid + "&overlayid=" + layeruid;
            string content = m_DataBase.ReadUrlCache(url);
            if (content == null)
                return Task.FromResult<CSystemObject>(null);

            var result = Task.FromResult<CSystemObject>(Newtonsoft.Json.JsonConvert.DeserializeObject<CSystemObject>(content));
            return result;
        }

        public Task<CSystemObject> GetObject(CWorkingLayer layer, string uid, string lcid = "")
        {
            return this.GetObject(layer.Database, layer.ProjectUID, layer.UID, uid, lcid);
        }

        public Task<string> GetObjectSpecificationAsHtml(string database, string project, string layer, string lcid, string target, string spec)
        {
            var url = m_Url + "ui/dbs/" + database + "/projects/" + project + "/objects/" + target + "/specifications/" + spec + "/html?lcid=" + lcid + "&overlayid=" + layer;
            string content = m_DataBase.ReadUrlCache(url);
            if (content == null)
                return Task.FromResult<string>(null);

            var result = Task.FromResult<string>(content);
            return result;
        }

        public string GetPictureUrl(string database, string key)
        {
            string url = m_Url + "dbs/" + database + "/pictures/" + key;
            return url;
        }

        public Task<List<CProject>> GetProjects(CDatabase database)
        {
            string url = m_Url + "dbs/" + database.Key + "/projects";
            string content = m_DataBase.ReadUrlCache(url);
            if (content == null)
                return Task.FromResult<List<CProject>>(null);

            var result = Task.FromResult<List<CProject>>(Newtonsoft.Json.JsonConvert.DeserializeObject<List<CProject>>(content));
            return result;
        }

        public Task<CQuerieResult> GetqueriesResult(CWorkingLayer layer, string lcid, string target, string start_objid)
        {
            return this.GetqueriesResult( layer.Database,layer.ProjectUID, layer.UID, lcid,target,start_objid);
        }

        public Task<CQuerieResult> GetqueriesResult(string database, string projectuid, string layer, string lcid, string target, string start_objid)
        {
            string url = m_Url + "dbs/" + database + "/projects/" + projectuid + "/queries/" + target + "/result?lcid=" + lcid + "&overlayid=" + layer;
            if (start_objid != null)
                url += "&start_objid=" + start_objid;
            string content = m_DataBase.ReadUrlCache(url);
            if (content == null)
                return Task.FromResult<CQuerieResult>(null);

            var result = Task.FromResult<CQuerieResult>(Newtonsoft.Json.JsonConvert.DeserializeObject<CQuerieResult> (content));
            return result;

        }

        public Task<List<CWorkingLayer>> GetWorkingOverlays(CProject project)
        {
            string url = m_Url + "dbs/" + project.Database + "/projects/" + project.ProjectUID + "/overlays?overlayid=" + project.ProjectUID;
            string content = m_DataBase.ReadUrlCache(url);
            if (content == null)
                return Task.FromResult<List<CWorkingLayer>>(null);

            var result = Task.FromResult<List<CWorkingLayer>>(Newtonsoft.Json.JsonConvert.DeserializeObject<List<CWorkingLayer>>(content));
            return result;
        }

        public Task<List<CWorkingLayer>> GetWorkingOverlays(CWorkingLayer layer)
        {
            string url = m_Url + "dbs/" + layer.Database + "/projects/" + layer.ProjectUID + "/overlays?overlayid=" + layer.UID;
            string content = m_DataBase.ReadUrlCache(url);
            if (content == null)
                return Task.FromResult<List<CWorkingLayer>>(null);

            var result = Task.FromResult<List<CWorkingLayer>>(Newtonsoft.Json.JsonConvert.DeserializeObject<List<CWorkingLayer>>(content));
            return result;
       }

        public Task<List<CSpecification>> GetObjectSpecification(string database, string projectuid, string layer, string lcid, string target)
        {
            string url = m_Url + "dbs/" + database + "/projects/" + projectuid + "/objects/" + target + "/specifications?lcid=" + lcid + "&overlayid=" + layer;
            string content = m_DataBase.ReadUrlCache(url);
            if (content == null)
                return Task.FromResult<List<CSpecification>>(null);

            var result = Task.FromResult<List<CSpecification>>(Newtonsoft.Json.JsonConvert.DeserializeObject<List<CSpecification>>(content));
            return result;
        }

        public Task<List<CNode>> GetNavigatorNodes_TreeNodes(CWorkingLayer layer, string lcid, string target, string viewstate)
        {
            string url = m_Url + "dbs/" + layer.Database + "/projects/" + layer.ProjectUID + "/navigator/nodes/" + target + "/treenodes?lcid=" + lcid + "&overlayid=" + layer.UID + "&view=" + viewstate;
            string content = m_DataBase.ReadUrlCache(url);
            if (content == null)
                return Task.FromResult<List<CNode>>(null);
            var result = Task.FromResult<List<CNode>>(Newtonsoft.Json.JsonConvert.DeserializeObject<List<CNode>>(content));
            return result;
        }

        public Task<CObject> GetDefaultLanguage(string database, string project, string layer)
        {
            string url = m_Url + "dbs/" + database + "/projects/" + project + "/languages/default?lcid=&overlayid=" + layer;
            string content = m_DataBase.ReadUrlCache(url);
            if (content == null)
                return Task.FromResult<CObject>(null);

            var result = Task.FromResult<CObject>(Newtonsoft.Json.JsonConvert.DeserializeObject<CObject>(content));
            return result;
        }

        public async Task<CSession> Login()
        {
            string url = m_Url + "sessions/actions/Login";
            string content = m_DataBase.ReadUrlCache(url);
            if (string.IsNullOrWhiteSpace(content))
                return null;
            var result = await Task.FromResult<CSession>(Newtonsoft.Json.JsonConvert.DeserializeObject<CSession>(content));
            return result;
        }

        public string GetDocumentUrl(string database, string projectuid, string layer, string docuid, bool exportpdf)
        {
            string url = m_Url + "dbs/" + database + "/projects/" + projectuid + "/documents/" + docuid + "?overlayid=" + layer;
            if (exportpdf)
                url += "&export_type=report2pdf";
            return url;
        }

        public void Lock()
        {
        }

        public void UnLock()
        {
        }
    }
}
