using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;
using Newtonsoft.Json.Linq;
using ComosWebSDK.Data;
using static XComosWebSDK.ApplicationProperties;
using XComosWebSDK;

namespace ComosWebSDK
{
    public class ComosWeb : IComosWeb, IDisposable
    {
        #region Fields

        string m_Url = null;
        string serverName;
        CSession m_Session = null;
        int m_IsLocked = 0;
        bool m_LogOutRequested = false;
        ICommunicationManager communicationManager;

        ComosWeb IComosWeb.ComosWeb => this;
        bool m_IsHeartBeatActive = true;
        #endregion

        #region Properties

        public string URL { get { return m_Url; } }

        //public ComosHttp ComosHttp { get { return m_Http; } }

        public bool IsLoggedIn { get { return m_Session != null; } }

        public int HeartBeat { get { return m_Session.HeartBeat; } }

        #endregion

        #region Constructor

        public ComosWeb(string url)
        {
            serverName = url;
            m_Url = url + "/api/webview/v1/";
            communicationManager = new CommunicationManager();
        }

        #endregion

        #region Interface Implementation

        public async Task<HttpStatusCode> Connect(string domain, string username, string password)
        {
            SetApplicationCurrentProperty("domain", domain);
            SetApplicationCurrentProperty("username", username);
            SetApplicationCurrentProperty("password", password);
            return await communicationManager.Connect(m_Url, serverName);
        }

        public async Task<CSession> Login()
        {
            if (m_IsLocked != 0 && m_LogOutRequested)
            {
                m_LogOutRequested = false;
                return m_Session;
            }

            var json = await communicationManager.PostLogin(m_Url, serverName);

            if (json == null)
                return await Task.FromResult<CSession>(null);

            CSession session = new CSession();
            session = JsonConvert.DeserializeObject<CSession>(json);
            m_Session = session;
            return session;
        }

        public void Lock()
        {
            m_IsLocked += 1;
        }

        public void UnLock()
        {
            m_IsLocked -= 1;
            if (m_IsLocked == 0 && m_LogOutRequested)
                Task.Run(async () => { await Logout(); });
        }

        public async Task<bool> Logout()
        {
            string value = "";
            if (m_IsLocked != 0)
            {
                m_LogOutRequested = true;
                return true;
            }

            if (m_Session != null)
            {
                if (m_IsHeartBeatActive)
                {
                    StopHeartbeat();
                }

                value = await communicationManager.PostLogout(m_Url, m_Session.Id, serverName);
                communicationManager.TokenSource.Cancel();
                m_Session = null;
                return value != "";
            }
            return true;
        }

        public async Task<CInfo> GetDatabases()
        {
            var content = await communicationManager.GetDatabases(m_Url, m_Session.Id, serverName);
            if (content == null)
                return await Task.FromResult<CInfo>(null);
            JObject json = JObject.Parse(content);

            if (json == null)
                return await Task.FromResult<CInfo>(null);

            List<CDatabase> databases = new List<CDatabase>();
            JToken dbs = json.SelectTokens("Databases").FirstOrDefault();
            databases = JsonConvert.DeserializeObject<List<CDatabase>>(dbs.ToString());

            List<CLanguage> languages = new List<CLanguage>();
            dbs = json.SelectTokens("Languages").FirstOrDefault();
            languages = JsonConvert.DeserializeObject<List<CLanguage>>(dbs.ToString());

            return new CInfo() { Databases = databases.ToArray(), Languages = languages.ToArray() };
        }

        public async Task<List<CProject>> GetProjects(CDatabase database)
        {
            var content = await communicationManager.GetProjects(database, m_Url, m_Session.Id, serverName);
            if (content == null)
                return null;
            JArray json = JArray.Parse(content);
            if (json == null)
                return null;

            List<CProject> projects = new List<CProject>();
            projects = JsonConvert.DeserializeObject<List<CProject>>(json.ToString());

            foreach (var project in projects)
                project.Database = database.Key;

            return projects;
        }

        public async Task<List<CWorkingLayer>> GetWorkingOverlays(CProject project)
        {
            var content = await communicationManager.GetWorkingOverlays(project.Database, project.ProjectUID, project.ProjectUID, m_Url, m_Session.Id, serverName).ConfigureAwait(false);
            if (content == null)
                return null;
            JObject json = JObject.Parse(content);
            if (json == null)
                return null;

            List<CWorkingLayer> layers = new List<CWorkingLayer>();
            JArray array = (JArray)json.SelectToken("OverlayItems");
            layers = JsonConvert.DeserializeObject<List<CWorkingLayer>>(array.ToString());

            foreach (var layer in layers)
                layer.Database = project.Database;

            return layers;
        }

        public async Task<List<CWorkingLayer>> GetWorkingOverlays(CWorkingLayer layer)
        {
            var content = await communicationManager.GetWorkingOverlays(layer.Database, layer.ProjectUID, layer.UID, m_Url, m_Session.Id, serverName).ConfigureAwait(false);
            if (content == null)
                return null;
            JObject json = JObject.Parse(content);
            if (json == null)
                return null;

            List<CWorkingLayer> layers = new List<CWorkingLayer>();
            JArray array = (JArray)json.SelectToken("OverlayItems");
            layers = JsonConvert.DeserializeObject<List<CWorkingLayer>>(array.ToString());

            return layers;
        }

        public async Task<CSystemObject> GetObject(string db, string projectuid, string layeruid, string uid, string lcid = "")
        {
            var content = await communicationManager.GetObject(m_Url, db, projectuid, uid, lcid, layeruid, m_Session.Id, serverName);
            if (content == null)
                return null;
            JObject json = JObject.Parse(content);
            if (json == null)
                return await Task.FromResult<CSystemObject>(null);

            CSystemObject result = new CSystemObject();
            result = JsonConvert.DeserializeObject<CSystemObject>(json.ToString());

            var doctypejson = json.SelectToken("DocumentType");
            if (doctypejson != null)
            {
                result.DocumentType = new CDocumentType();
                result.DocumentType = JsonConvert.DeserializeObject<CDocumentType>(doctypejson.ToString());
            }
            return result;
        }

        public async Task<CSystemObject> GetObject(CWorkingLayer layer, string uid, string lcid = "")
        {
            return await this.GetObject(layer.Database, layer.ProjectUID, layer.UID, uid, lcid);
        }

        public async Task<CObject> GetDefaultLanguage(string database, string project, string layer)
        {
            var content = await communicationManager.GetDefaultLanguage(m_Url, database, project, layer, m_Session.Id, serverName);
            if (content == null)
                return null;
            JObject json = JObject.Parse(content);
            if (json == null)
                return null;

            CObject o = new CObject();
            o = JsonConvert.DeserializeObject<CObject>(json.ToString());

            if (o.OverlayUID == null)
                o.OverlayUID = layer;

            if (o.ProjectUID == null)
                o.ProjectUID = project;

            return o;
        }

        public async Task<List<CObject>> GetNavigatorNodes_Children(string db, string ProjectUID, string layer, string lcid, string target, string viewstate, string parent = null)
        {
            var content = await communicationManager.GetNavigatorNodes_Children(m_Url, db, ProjectUID, target, lcid, layer, parent, viewstate, m_Session.Id, serverName);
            if (content == null)
                return null;
            JArray jarray = JArray.Parse(content);
            List<CObject> result = new List<CObject>();

            if (jarray == null)
                return result;

            result = JsonConvert.DeserializeObject<List<CObject>>(jarray.ToString());

            foreach (var o in result)
            {
                if (o.OverlayUID == null)
                {
                    o.OverlayUID = layer;
                }
                if (o.ProjectUID == null)
                {
                    o.ProjectUID = ProjectUID;
                }
            }
            return result;
        }

        public async Task<List<CNode>> GetNavigatorNodes_TreeNodes(CWorkingLayer layer, string lcid, string target, string viewstate)
        {
            var content = await communicationManager.GetNavigatorNodes_TreeNodes(m_Url, layer, target, lcid, viewstate, m_Session.Id, serverName);
            if (content == null)
                return null;
            JArray jarray = JArray.Parse(content);
            List<CNode> result = new List<CNode>();

            if (jarray == null)
                return result;

            foreach (var token in jarray)
            {
                var item = token.SelectToken("Item");
                CNode o = new CNode()
                {
                    Item = new CObject()
                    {
                        ClassType = item.SelectToken("ClassType").Value<string>(),
                        Description = item.SelectToken("Description").Value<string>(),
                        IsClientPicture = item.SelectToken("IsClientPicture").Value<bool>(),
                        Picture = item.SelectToken("Picture").Value<string>(),
                        Name = item.SelectToken("Name").Value<string>(),
                        OverlayUID = item.SelectToken("OverlayUID").Value<string>(),
                        ProjectUID = item.SelectToken("ProjectUID").Value<string>(),
                        UID = item.SelectToken("UID").Value<string>(),
                        SystemFullName = item.SelectToken("SystemFullName").Value<string>()
                    }
                };
                if (o.Item.OverlayUID == null)
                {
                    o.Item.OverlayUID = layer.UID;
                }
                if (o.Item.ProjectUID == null)
                {
                    o.Item.ProjectUID = layer.ProjectUID;
                }
                List<CObject> Items = new List<CObject>();
                foreach (var child in (JArray)token.SelectToken("Items"))
                {
                    var Item = new CObject()
                    {
                        ClassType = child.SelectToken("ClassType").Value<string>(),
                        Description = child.SelectToken("Description").Value<string>(),
                        IsClientPicture = child.SelectToken("IsClientPicture").Value<bool>(),
                        Picture = child.SelectToken("Picture").Value<string>(),
                        Name = child.SelectToken("Name").Value<string>(),
                        OverlayUID = child.SelectToken("OverlayUID").Value<string>(),
                        ProjectUID = child.SelectToken("ProjectUID").Value<string>(),
                        UID = child.SelectToken("UID").Value<string>(),
                        SystemFullName = child.SelectToken("SystemFullName").Value<string>()
                    };
                    if (Item.OverlayUID == null)
                    {
                        Item.OverlayUID = layer.UID;
                    }
                    if (Item.ProjectUID == null)
                    {
                        Item.ProjectUID = layer.ProjectUID;
                    }
                    Items.Add(Item);
                }
                o.Items = Items;

                result.Add(o);
            }
            return result;
        }

        public async Task<List<CSpecification>> GetObjectSpecification(string database, string projectuid, string layer, string lcid, string target)
        {
            var content = await communicationManager.GetObjectSpecification(m_Url, database, projectuid, target, lcid, layer, m_Session.Id, serverName);
            if (content == null)
                return null;
            JArray jarray = JArray.Parse(content);
            List<CSpecification> result = new List<CSpecification>();

            if (jarray == null)
                return result;

            foreach (var token in jarray)
            {
                CSpecification s = new CSpecification()
                {
                    IsEditable = token.SelectToken("IsEditable").Value<bool>(),
                    IsReauthenticationRequired = token.SelectToken("IsReauthenticationRequired").Value<bool>(),
                    NestedName = token.SelectToken("NestedName").Value<string>(),
                    ObjectUID = token.SelectToken("ObjectUID").Value<string>(),
                    OverlayName = token.SelectToken("OverlayName").Value<string>(),
                    ProjectName = token.SelectToken("ProjectName").Value<string>(),
                    ClassType = token.SelectToken("ClassType").Value<string>(),
                    Description = token.SelectToken("Description").Value<string>(),
                    IsClientPicture = token.SelectToken("IsClientPicture").Value<bool>(),
                    Name = token.SelectToken("Name").Value<string>(),
                    OverlayUID = token.SelectToken("OverlayUID").Value<string>(),
                    Picture = token.SelectToken("Picture").Value<string>(),
                    ProjectUID = token.SelectToken("ProjectUID").Value<string>(),
                    SystemFullName = token.SelectToken("SystemFullName").Value<string>(),
                    UID = token.SelectToken("UID").Value<string>(),
                };

                List<CSpecification> children = new List<CSpecification>();
                var json_children = token.SelectToken("children");
                if (json_children != null && json_children.HasValues)
                {
                    foreach (var child in (JArray)json_children)
                    {
                        CSpecification v = new CSpecification()
                        {
                            ClassType = child.SelectToken("ClassType").Value<string>(),
                            ProjectName = child.SelectToken("ProjectName").Value<string>(),
                            OverlayName = child.SelectToken("OverlayName").Value<string>(),
                            Description = child.SelectToken("Description").Value<string>(),
                            IsClientPicture = child.SelectToken("IsClientPicture").Value<bool>(),
                            IsEditable = child.SelectToken("IsEditable").Value<bool>(),
                            IsReauthenticationRequired = child.SelectToken("IsReauthenticationRequired").Value<bool>(),
                            Name = child.SelectToken("Name").Value<string>(),
                            NestedName = child.SelectToken("NestedName").Value<string>(),
                            ObjectUID = child.SelectToken("ObjectUID").Value<string>(),
                            OverlayUID = child.SelectToken("OverlayUID").Value<string>(),
                            Picture = child.SelectToken("Picture").Value<string>(),
                            ProjectUID = child.SelectToken("ProjectUID").Value<string>(),
                            SystemFullName = child.SelectToken("SystemFullName").Value<string>(),
                            UID = child.SelectToken("UID").Value<string>(),
                            children = null,
                        };

                        v.CachedValue = "";
                        children.Add(v);
                    }
                    s.children = children;
                }
                result.Add(s);
            }
            return result;
        }

        public async Task<string> GetObjectSpecificationAsHtml(string database, string project, string layer, string lcid, string target, string spec)
        {
            var content = await communicationManager.GetObjectSpecificationAsHtml(m_Url, database, project, target, spec, lcid, layer, m_Session.Id, serverName);
            if (content == null)
                return null;
            JObject json = JObject.Parse(content);
            if (json == null)
                return null;

            string result = json.SelectToken("Result").Value<string>();
            return result;
        }

        public async Task<CQuerieResult> GetqueriesResult(string database, string projectuid, string layer, string lcid, string target, string start_objid)
        {
            var content = await communicationManager.GetqueriesResult(m_Url, database, projectuid, target, lcid, layer, start_objid, m_Session.Id, serverName);
            if (content == null)
                return null;
            JObject json = JObject.Parse(content);
            if (json == null)
                return null;

            CQuerieResult q = new CQuerieResult()
            {
                UID = json.SelectToken("UID").Value<string>(),
            };
            List<int> invisiblecolumns = new List<int>();
            int index = 0;
            List<CColumn> columns = new List<CColumn>();

            foreach (var jcol in (JArray)json.SelectToken("Columns"))
            {
                CColumn c = new CColumn()
                {
                    Alignment = jcol.SelectToken("Alignment").Value<int>(),
                    DisplayDescription = jcol.SelectToken("DisplayDescription").Value<string>(),
                    Id = jcol.SelectToken("Id").Value<int>(),
                    IsDate = jcol.SelectToken("IsDate").Value<bool>(),
                    Numeric = jcol.SelectToken("Numeric").Value<bool>(),
                    Visible = jcol.SelectToken("Visible").Value<bool>(),
                    Width = jcol.SelectToken("Width").Value<int>(),
                    WithPicture = jcol.SelectToken("WithPicture").Value<bool>(),
                    WrapText = jcol.SelectToken("WrapText").Value<bool>(),
                };

                if (c.Width > 0)
                {
                    columns.Add(c);
                }
                else
                {
                    invisiblecolumns.Add(index);
                }
                index++;
            }
            q.Columns = columns.ToArray();
            List<CRow> rows = new List<CRow>();

            foreach (var jrow in (JArray)json.SelectToken("Rows"))
            {
                CRow c = new CRow()
                {
                    UID = jrow.SelectToken("UID").Value<string>(),
                };
                List<CCell> cells = new List<CCell>();
                index = 0;

                foreach (var jcell in (JArray)jrow.SelectToken("Items"))
                {
                    CCell cell = new CCell()
                    {
                        IsClientPicture = jcell.SelectToken("IsClientPicture").Value<bool>(),
                        NumericValue = jcell.SelectToken("NumericValue").Value<double>(),
                        Picture = jcell.SelectToken("Picture").Value<string>(),
                        Text = jcell.SelectToken("Text").Value<string>(),
                        UID = jcell.SelectToken("UID").Value<string>(),
                    };
                    if (!invisiblecolumns.Contains(index))
                    {
                        cells.Add(cell);
                    }
                    index++;
                }
                c.Items = cells.ToArray();
                rows.Add(c);
            }
            q.Rows = rows.ToArray();

            return q;
        }

        #region data links

        public string GetPictureUrl(string database, string key)
        {
            return communicationManager.GetPictureUrl(m_Url, database, key);
        }

        public async Task<HttpResponseMessage> GetDocumentStream(string database, string projectuid, string layer, string docuid, bool exportpdf)
        {
            return await communicationManager.GetDocumentStream(m_Url, database, projectuid, layer, docuid, exportpdf, m_Session.Id, serverName);
        }

        #endregion

        #region Heartbeat support implementation

        public bool DoHeartBeat()
        {
            if (!m_IsHeartBeatActive)
                return m_IsHeartBeatActive;

            try
            {
                var response = communicationManager.DoHeartbeat(m_Url, m_Session.Id, serverName).Result;
                if (response == null)
                {
                    m_IsHeartBeatActive = false;
                }
                else
                {
                    m_IsHeartBeatActive = response.IsSuccessStatusCode;
                }
            }
            catch (Exception e)
            {
                m_IsHeartBeatActive = false;
            }

            return m_IsHeartBeatActive;
        }

        public void StartHeartBeat()
        {
            m_IsHeartBeatActive = true;
        }

        public void StopHeartbeat()
        {
            m_IsHeartBeatActive = false;
        }

        #endregion
        #endregion

        #region IDisposable implementation

        public void Dispose()
        {
            if (m_Session != null)
            {
                Task.Run(async () => { await this.Logout(); });
            }
        }

        #endregion
    }
}
