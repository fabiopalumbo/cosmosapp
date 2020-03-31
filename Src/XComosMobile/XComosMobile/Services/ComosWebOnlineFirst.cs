using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComosWebSDK;
using ComosWebSDK.Data;

namespace XComosMobile.Services
{
	public class ComosWebOnlineFirst : IComosWeb
	{
		public string URL => m_Url;
		public ComosWeb ComosWeb => m_ComosWebOnline;

		ComosWeb m_ComosWebOnline = null;
		ComosWebOffline m_ComosWebOffline = null;
		IDatabase m_Database = null;
		string m_Url = null;
		IPlatformSystem m_Platform;
		public ComosWebOnlineFirst(ComosWeb comosweb)
		{
			m_Platform = Services.XServices.Instance.GetService<Services.IPlatformSystem>();
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
			if (m_Platform.IsOnline)
			{
				return m_ComosWebOnline.DoHeartBeat();
			}
			return true;
		}

		public async Task<System.Net.HttpStatusCode> Connect(
										 string domain, string username, string password)
		{
			return await m_ComosWebOnline.Connect(domain, username, password);
		}

		public async Task<CInfo> GetDatabases()
		{
			if (m_Platform.IsOnline)
			{
				var tmp = await m_ComosWebOnline.GetDatabases();
				if (tmp != null)
				{
					var url = m_Url + "dbs";
					string output = Newtonsoft.Json.JsonConvert.SerializeObject(tmp);
					m_Database.WriteUrlCache(url, output);
					return tmp;
				}
			}
			return await m_ComosWebOffline.GetDatabases();
		}

		public async Task<List<CObject>> GetNavigatorNodes_Children(string db, string ProjectUID, string layer, string lcid, string target, string viewstate, string parent = null)
		{
			if (m_Platform.IsOnline)
			{
				var tmp = await m_ComosWebOnline.GetNavigatorNodes_Children(db, ProjectUID, layer, lcid, target, viewstate, parent);
				if (tmp != null && tmp.Count > 0)
				{
					string url = null;
					if (string.IsNullOrEmpty(parent))
						url = m_Url + "dbs/" + db + "/projects/" + ProjectUID + "/navigator/nodes/" + target + "/children?lcid=" + lcid + "&overlayid=" + layer + "&view=" + viewstate;
					else
						url = m_Url + "dbs/" + db + "/projects/" + ProjectUID + "/navigator/nodes/" + target + "/children?lcid=" + lcid + "&overlayid=" + layer + "&parent=" + parent + "&view=" + viewstate;
					string output = Newtonsoft.Json.JsonConvert.SerializeObject(tmp);
					m_Database.WriteUrlCache(url, output);
					return tmp;
				}
			}
			return await m_ComosWebOffline.GetNavigatorNodes_Children(db, ProjectUID, layer, lcid, target, viewstate, parent);
		}

		public async Task<CSystemObject> GetObject(string db, string projectuid, string layeruid, string uid, string lcid = "")
		{
			if (m_Platform.IsOnline)
			{
				var tmp = await m_ComosWebOnline.GetObject(db, projectuid, layeruid, uid, lcid);
				if (tmp != null)
				{
					var url = m_Url + "dbs/" + db + "/projects/" + projectuid + "/objects/" + uid + "?lcid=" + lcid + "&overlayid=" + layeruid;
					string output = Newtonsoft.Json.JsonConvert.SerializeObject(tmp);
					m_Database.WriteUrlCache(url, output);
					return tmp;
				}
			}
			return await m_ComosWebOffline.GetObject(db, projectuid, layeruid, uid, lcid);
		}

		public async Task<CSystemObject> GetObject(CWorkingLayer layer, string uid, string lcid = "")
		{
			return await this.GetObject(layer.Database, layer.ProjectUID, layer.UID, uid, lcid);
		}

		public async Task<string> GetObjectSpecificationAsHtml(string database, string project, string layer, string lcid, string target, string spec)
		{
			string tmp = null;
			if (m_Platform.IsOnline)
			{
				tmp = await m_ComosWebOnline.GetObjectSpecificationAsHtml(database, project, layer, lcid, target, spec);
				if (tmp != null)
				{
					var url = m_Url + "ui/dbs/" + database + "/projects/" + project + "/objects/" + target + "/specifications/" + spec + "/html?lcid=" + lcid + "&overlayid=" + layer;
					m_Database.WriteUrlCache(url, tmp);
					return tmp;
				}
			}
			string result = await m_ComosWebOffline.GetObjectSpecificationAsHtml(database, project, layer, lcid, target, spec);
			//if (string.Compare(result, tmp) != 0)
			//{
			//    throw new Exception();
			//}
			return result;
		}

		public string GetPictureUrl(string database, string key)
		{
			return m_ComosWebOnline.GetPictureUrl(database, key);
		}

		public async Task<List<CProject>> GetProjects(CDatabase database)
		{
			if (m_Platform.IsOnline)
			{
				var tmp = await m_ComosWebOnline.GetProjects(database);
				if (tmp != null)
				{
					string url = m_Url + "dbs/" + database.Key + "/projects";
					string output = Newtonsoft.Json.JsonConvert.SerializeObject(tmp);
					m_Database.WriteUrlCache(url, output);
					return tmp;
				}
			}
			return await m_ComosWebOffline.GetProjects(database);
		}

		public async Task<CQuerieResult> GetqueriesResult(CWorkingLayer layer, string lcid, string target, string start_objid)
		{
			return await this.GetqueriesResult(layer.Database, layer.ProjectUID, layer.UID, lcid, target, start_objid);
		}

		public async Task<CQuerieResult> GetqueriesResult(string database, string projectuid, string layer, string lcid, string target, string start_objid)
		{
			if (m_Platform.IsOnline)
			{
				var tmp = await m_ComosWebOnline.GetqueriesResult(database, projectuid, layer, lcid, target, start_objid);
				if (tmp != null)
				{
					string url = m_Url + "dbs/" + database + "/projects/" + projectuid + "/queries/" + target + "/result?lcid=" + lcid + "&overlayid=" + layer;
					if (start_objid != null)
						url += "&start_objid=" + start_objid;
					string output = Newtonsoft.Json.JsonConvert.SerializeObject(tmp);
					m_Database.WriteUrlCache(url, output);
					return tmp;
				}
			}
			return await m_ComosWebOffline.GetqueriesResult(database, projectuid, layer, lcid, target, start_objid);
		}

		public async Task<List<CWorkingLayer>> GetWorkingOverlays(CProject project)
		{
			if (m_Platform.IsOnline)
			{
				var tmp = await m_ComosWebOnline.GetWorkingOverlays(project);
				if (tmp != null)
				{
					string url = m_Url + "dbs/" + project.Database + "/projects/" + project.ProjectUID + "/overlays?overlayid=" + project.ProjectUID;
					string output = Newtonsoft.Json.JsonConvert.SerializeObject(tmp);
					m_Database.WriteUrlCache(url, output);
					return tmp;
				}
			}
			return await m_ComosWebOffline.GetWorkingOverlays(project);

		}
		public async Task<List<CWorkingLayer>> GetWorkingOverlays(CWorkingLayer layer)
		{
			if (m_Platform.IsOnline)
			{
				var tmp = await m_ComosWebOnline.GetWorkingOverlays(layer);
				if (tmp != null)
				{
					List<CWorkingLayer> newTmp = tmp;  //New layers have same Database
					foreach (CWorkingLayer wl in newTmp)
					{
						wl.Database = layer.Database;
					}
					string url = m_Url + "dbs/" + layer.Database + "/projects/" + layer.ProjectUID + "/overlays?overlayid=" + layer.UID;
					string output = Newtonsoft.Json.JsonConvert.SerializeObject(tmp);
					m_Database.WriteUrlCache(url, output);
					return tmp;
				}
			}
			return await m_ComosWebOffline.GetWorkingOverlays(layer);
		}

		public async Task<List<CSpecification>> GetObjectSpecification(string database, string projectuid, string layer, string lcid, string target)
		{
			if (m_Platform.IsOnline)
			{
				var tmp = await m_ComosWebOnline.GetObjectSpecification(database, projectuid, layer, lcid, target);
				if (tmp != null)
				{
					string url = m_Url + "dbs/" + database + "/projects/" + projectuid + "/objects/" + target + "/specifications?lcid=" + lcid + "&overlayid=" + layer;
					string output = Newtonsoft.Json.JsonConvert.SerializeObject(tmp);
					m_Database.WriteUrlCache(url, output);
					return tmp;
				}
			}
			return await m_ComosWebOffline.GetObjectSpecification(database, projectuid, layer, lcid, target);
		}

		public async Task<List<CNode>> GetNavigatorNodes_TreeNodes(CWorkingLayer layer, string lcid, string target, string viewstate)
		{
			if (m_Platform.IsOnline)
			{
				var tmp = await m_ComosWebOnline.GetNavigatorNodes_TreeNodes(layer, lcid, target, viewstate);
				if (tmp != null && tmp.Count > 0)
				{
					string url = m_Url + "dbs/" + layer.Database + "/projects/" + layer.ProjectUID + "/navigator/nodes/" + target + "/treenodes?lcid=" + lcid + "&overlayid=" + layer.UID + "&view=" + viewstate;
					string output = Newtonsoft.Json.JsonConvert.SerializeObject(tmp);
					m_Database.WriteUrlCache(url, output);
					return tmp;
				}
			}
			return await m_ComosWebOffline.GetNavigatorNodes_TreeNodes(layer, lcid, target, viewstate);
		}

		public async Task<bool> Logout()
		{
			if (m_Platform.IsOnline)
			{
				return await m_ComosWebOnline.Logout();
			}
			return await Task.FromResult<bool>(true);
		}

		public void StartHeartBeat()
		{
			if (m_Platform.IsOnline)
			{
				m_ComosWebOnline.StartHeartBeat();
			}
		}

		public void StopHeartbeat()
		{
			if (m_Platform.IsOnline)
			{
				m_ComosWebOnline.StopHeartbeat();
			}
		}

		public async Task<CObject> GetDefaultLanguage(string database, string project, string layer)
		{
			if (m_Platform.IsOnline)
			{
				var tmp = await m_ComosWebOnline.GetDefaultLanguage(database, project, layer);
				if (tmp != null)
				{
					string url = m_Url + "dbs/" + database + "/projects/" + project + "/languages/default?lcid=&overlayid=" + layer;
					string output = Newtonsoft.Json.JsonConvert.SerializeObject(tmp);
					m_Database.WriteUrlCache(url, output);
					return tmp;
				}
			}
			return await m_ComosWebOffline.GetDefaultLanguage(database, project, layer);
		}

		public async Task<CSession> Login()
		{
			if (m_Platform.IsOnline)
			{
				var tmp = await m_ComosWebOnline.Login();
				if (tmp != null)
				{
					string url = m_Url + "sessions/actions/Login";
					string output = Newtonsoft.Json.JsonConvert.SerializeObject(tmp);
					m_Database.WriteUrlCache(url, output);
					return tmp;
				}
			}
			return await m_ComosWebOffline.Login();
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
