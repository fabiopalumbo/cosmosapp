using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComosWebSDK.Data;

namespace ComosWebSDK
{
    public interface IComosWeb: IDisposable
    {
        string URL { get; }

        void Lock();

        void UnLock();

        ComosWeb ComosWeb { get; }

        Task<System.Net.HttpStatusCode> Connect(string DomainName, string UserName, string Password);

        Task<bool> Logout();

        void StartHeartBeat();

        void StopHeartbeat();

        int HeartBeat { get; }

        bool DoHeartBeat();

        string GetPictureUrl(string database, string key);

        Task<CQuerieResult> GetqueriesResult(string database, string projectuid, string layer, string lcid, string target, string start_objid);

        Task<CSystemObject> GetObject(string db, string projectuid, string layeruid, string uid, string lcid = "");

        Task<CSystemObject> GetObject(CWorkingLayer layer, string uid, string lcid = "");

        Task<List<CObject>> GetNavigatorNodes_Children(string db, string ProjectUID, string layer, string lcid, string target, string viewstate, string parent = null);

        Task<List<CProject>> GetProjects(CDatabase database);

        Task<List<CWorkingLayer>> GetWorkingOverlays(CProject project);

        Task<List<CWorkingLayer>> GetWorkingOverlays(CWorkingLayer layer);

        Task<CInfo> GetDatabases();

        Task<string> GetObjectSpecificationAsHtml(string database, string project, string layer, string lcid, string target, string spec);

        Task<List<CSpecification>> GetObjectSpecification(string database, string projectuid, string layer, string lcid, string target);

        Task<List<CNode>> GetNavigatorNodes_TreeNodes(CWorkingLayer layer, string lcid, string target, string viewstate);

        Task<CObject> GetDefaultLanguage(string database, string project, string layer);        

        Task<CSession> Login();

    }
}
