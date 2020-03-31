using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComosWebSDK;
using ComosWebSDK.Data;

namespace XComosMobile.Services
{
    public class ComosWebOfflineFirst: IComosWeb
    {
        ComosWeb m_ComosWebOnline = null;
        public string URL => m_Url;
        public ComosWeb ComosWeb => m_ComosWebOnline;

        ComosWebOffline m_ComosWebOffline = null;
        IDatabase m_Database = null;
        string m_Url = null;
        public ComosWebOfflineFirst(ComosWeb comosweb)
        {
            m_ComosWebOnline = comosweb;
            m_Url = comosweb.URL;
            m_Database = Services.XServices.Instance.GetService<Services.IDatabase>();
            m_ComosWebOffline = new ComosWebOffline(m_Database, comosweb.URL);
        }
        public int HeartBeat => m_ComosWebOnline.HeartBeat;

        public void Dispose()
        {
        }

        public bool DoHeartBeat()
        {
            return m_ComosWebOnline.DoHeartBeat();
        }

        public async Task<System.Net.HttpStatusCode> Connect(
                     string domain, string username, string password)
        {
            return await m_ComosWebOnline.Connect(domain, username, password);
        }
        public async Task<CInfo> GetDatabases()
        {
            var tmp = await m_ComosWebOffline.GetDatabases();
            if (tmp == null)
            {
                tmp = await m_ComosWebOnline.GetDatabases();
                if (tmp != null)
                {
                    var url = m_Url + "dbs";
                    string output = Newtonsoft.Json.JsonConvert.SerializeObject(tmp);
                    m_Database.WriteUrlCache(url, output);
                }
            }
            return tmp;
        }

        public async Task<List<CObject>> GetNavigatorNodes_Children(string db, string ProjectUID, string layer, string lcid, string target, string viewstate, string parent = null)
        {
            var tmp = await m_ComosWebOffline.GetNavigatorNodes_Children(db, ProjectUID, layer, lcid, target, viewstate, parent);
            if (tmp == null || tmp.Count == 0)
            {
                tmp = await m_ComosWebOnline.GetNavigatorNodes_Children(db, ProjectUID, layer, lcid, target, viewstate, parent);
                if (tmp != null && tmp.Count > 0)
                {
                    string url = null;
                    if (string.IsNullOrEmpty(parent))
                        url = m_Url + "dbs/" + db + "/projects/" + ProjectUID + "/navigator/nodes/" + target + "/children?lcid=" + lcid + "&overlayid=" + layer + "&view=" + viewstate;
                    else
                        url = m_Url + "dbs/" + db + "/projects/" + ProjectUID + "/navigator/nodes/" + target + "/children?lcid=" + lcid + "&overlayid=" + layer + "&parent=" + parent + "&view=" + viewstate;
                    string output = Newtonsoft.Json.JsonConvert.SerializeObject(tmp);
                    m_Database.WriteUrlCache(url, output);
                }
            }
            return tmp;
        }

        public async Task<CSystemObject> GetObject(string db, string projectuid, string layeruid, string uid, string lcid = "")
        {
            var tmp = await m_ComosWebOffline.GetObject(db, projectuid, layeruid, uid, lcid);
            if (tmp == null)
            {
                tmp = await m_ComosWebOnline.GetObject(db, projectuid, layeruid, uid, lcid);
                if (tmp != null)
                {
                    var url = m_Url + "dbs/" + db + "/projects/" + projectuid + "/objects/" + uid + "?lcid=" + lcid + "&overlayid=" + layeruid;
                    string output = Newtonsoft.Json.JsonConvert.SerializeObject(tmp);
                    m_Database.WriteUrlCache(url, output);
                };
            }
            return tmp;
        }

        public async Task<CSystemObject> GetObject(CWorkingLayer layer, string uid, string lcid = "")
        {
            return await this.GetObject(layer.Database, layer.ProjectUID, layer.UID, uid, lcid);
        }

        public async Task<string> GetObjectSpecificationAsHtml(string database, string project, string layer, string lcid, string target, string spec)
        {
            var tmp = await m_ComosWebOffline.GetObjectSpecificationAsHtml(database, project, layer, lcid, target, spec);
            if (tmp == null)
            {
                tmp = await m_ComosWebOnline.GetObjectSpecificationAsHtml(database, project, layer, lcid, target, spec);
                if (tmp != null)
                {
                    var url = m_Url + "ui/dbs/" + database + "/projects/" + project + "/objects/" + target + "/specifications/" + spec + "/html?lcid=" + lcid + "&overlayid=" + layer;
                    string output = Newtonsoft.Json.JsonConvert.SerializeObject(tmp);
                    m_Database.WriteUrlCache(url, output);
                }
            }
            return tmp;
        }

        public string GetPictureUrl(string database, string key)
        {
            return m_ComosWebOnline.GetPictureUrl(database, key);
        }

        public async Task<List<CProject>> GetProjects(CDatabase database)
        {
            var tmp = await m_ComosWebOffline.GetProjects(database);
            if (tmp == null)
            {
                tmp = await m_ComosWebOnline.GetProjects(database);
                if (tmp != null)
                {
                    string url = m_Url + "dbs/" + database.Key + "/projects";
                    string output = Newtonsoft.Json.JsonConvert.SerializeObject(tmp);
                    m_Database.WriteUrlCache(url, output);
                }
            }
            return tmp;
        }

        public async Task<CQuerieResult> GetqueriesResult(CWorkingLayer layer, string lcid, string target, string start_objid)
        {
            return await this.GetqueriesResult(layer.Database, layer.ProjectUID, layer.UID, lcid, target, start_objid);
        }

        public async Task<CQuerieResult> GetqueriesResult(string database, string projectuid, string layer, string lcid, string target, string start_objid)
        {
            var tmp = await m_ComosWebOffline.GetqueriesResult(database, projectuid, layer, lcid, target, start_objid);
            if (tmp == null)
            {
                tmp = await m_ComosWebOnline.GetqueriesResult(database, projectuid, layer, lcid, target, start_objid);
                if (tmp != null)
                {
                    string url = m_Url + "dbs/" + database + "/projects/" + projectuid + "/queries/" + target + "/result?lcid=" + lcid + "&overlayid=" + layer;
                    if (start_objid != null)
                        url += "&start_objid=" + start_objid;
                    string output = Newtonsoft.Json.JsonConvert.SerializeObject(tmp);
                    m_Database.WriteUrlCache(url, output);
                }
            }
            return tmp;
        }

        public async Task<List<CWorkingLayer>> GetWorkingOverlays(CProject project)
        {
            var tmp = await m_ComosWebOffline.GetWorkingOverlays(project);
            if (tmp == null)
            {
                tmp = await m_ComosWebOnline.GetWorkingOverlays(project); 
                if (tmp != null)
                {
                    string url = m_Url + "dbs/" + project.Database + "/projects/" + project.ProjectUID + "/overlays?overlayid=" + project.ProjectUID;
                    string output = Newtonsoft.Json.JsonConvert.SerializeObject(tmp);
                    m_Database.WriteUrlCache(url, output);
                }
            }
            return tmp;

        }

        public async Task<List<CWorkingLayer>> GetWorkingOverlays(CWorkingLayer layer)
        {
            var tmp = await m_ComosWebOffline.GetWorkingOverlays(layer);
            if (tmp == null)
            {
                tmp = await m_ComosWebOnline.GetWorkingOverlays(layer);
                if (tmp != null)
                {
                    string url = m_Url + "dbs/" + layer.Database + "/projects/" + layer.ProjectUID + "/overlays?overlayid=" + layer.UID;
                    string output = Newtonsoft.Json.JsonConvert.SerializeObject(tmp);
                    m_Database.WriteUrlCache(url, output);
                }
            }
            return tmp;
        }

        public async Task<List<CSpecification>> GetObjectSpecification(string database, string projectuid, string layer, string lcid, string target)
        {
            var tmp = await m_ComosWebOffline.GetObjectSpecification(database, projectuid, layer, lcid, target);
            if (tmp == null)
            {
                tmp = await m_ComosWebOnline.GetObjectSpecification(database, projectuid, layer, lcid, target);
                if (tmp != null)
                {
                    string url = m_Url + "dbs/" + database + "/projects/" + projectuid + "/objects/" + target + "/specifications?lcid=" + lcid + "&overlayid=" + layer;
                    string output = Newtonsoft.Json.JsonConvert.SerializeObject(tmp);
                    m_Database.WriteUrlCache(url, output);
                }
            }
            return tmp;
        }

        public async Task<List<CNode>> GetNavigatorNodes_TreeNodes(CWorkingLayer layer, string lcid, string target, string viewstate)
        {
            var tmp = await m_ComosWebOffline.GetNavigatorNodes_TreeNodes( layer,  lcid,  target,  viewstate);
            if (tmp == null)
            {
                tmp = await m_ComosWebOnline.GetNavigatorNodes_TreeNodes( layer,  lcid,  target,  viewstate);
                if (tmp != null)
                {
                    string url = m_Url + "dbs/" + layer.Database + "/projects/" + layer.ProjectUID + "/navigator/nodes/" + target + "/treenodes?lcid=" + lcid + "&overlayid=" + layer.UID + "&view=" + viewstate;
                    string output = Newtonsoft.Json.JsonConvert.SerializeObject(tmp);
                    m_Database.WriteUrlCache(url, output);
                }
            }
            return tmp;
        }


        public async Task<bool> Logout()
        {
            return await m_ComosWebOnline.Logout();
        }

        public void StartHeartBeat()
        {
            m_ComosWebOnline.StartHeartBeat();
        }

        public void StopHeartbeat()
        {
            m_ComosWebOnline.StopHeartbeat();
        }

        public async Task<CObject> GetDefaultLanguage(string database, string project, string layer)
        {
            var tmp = await m_ComosWebOffline.GetDefaultLanguage(database, project, layer);
            if (tmp == null)
            {
                tmp = await m_ComosWebOnline.GetDefaultLanguage(database, project, layer);
                if (tmp != null)
                {
                    string url = m_Url + "dbs/" + database + "/projects/" + project + "/languages/default?lcid=&overlayid=" + layer;
                    string output = Newtonsoft.Json.JsonConvert.SerializeObject(tmp);
                    m_Database.WriteUrlCache(url, output);
                }
            }
            return tmp;
        }

        public async Task<CSession> Login()
        {
            return await Task.FromResult<CSession>(null); 
            //var result = await m_ComosWebOnline.Login();
            //string url = m_Url + "dbs/" + database + "/projects/" + project + "/languages/default?lcid=&overlayid=" + layer;
            //string output = Newtonsoft.Json.JsonConvert.SerializeObject(tmp);
            //m_Database.WriteUrlCache(url, output);
            //return result;
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
            m_ComosWebOnline.Lock();
        }

        public void UnLock()
        {
            m_ComosWebOnline.UnLock();
        }
    }
}
