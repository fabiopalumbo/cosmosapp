using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Comos.IO;
using IBRServiceContracts;
using Plt;

namespace BRComos.IO.IPC
{
    /// <summary>
    /// Implementação de servico por comunicar entre COMOS Web BR e COMOS IO
    /// </summary>
    public class IOServiceContract : IServiceContract
    {
        #region Old interface

        /// <summary>
        /// Interface na COMOS.
        /// </summary>
        private static ComosSession m_ComosSession = null;

        /// <summary>
        /// Inicializar COMOS para usar banco de dados definido pela conectionstring.
        /// !!! Este methodo se comporta static
        /// </summary>
        /// <param name="connectionstring"></param>
        /// <returns>True é o sucesso, false caso contrário.</returns>
        public bool Open(string connectionstring)
        {
            try
            {
                Console.WriteLine("COMOS.IO Opening : " + connectionstring);
                Log.WriteLog("COMOS.IO Opening : " + connectionstring, System.Diagnostics.EventLogEntryType.Information);

                if (m_ComosSession != null)
                    return false;
                m_ComosSession = new ComosSession();
                Log.WriteLog("Session Created. Will Call Open in Comos IO", System.Diagnostics.EventLogEntryType.Information);

                if (!m_ComosSession.Open(connectionstring))
                {
                    Console.WriteLine("Invalid <connection string> or <no license> argument: " + connectionstring);
                    Log.WriteLog("Invalid <connection string> or <no license> argument: " + connectionstring, System.Diagnostics.EventLogEntryType.Error);
                    return false;
                }
                else
                {
                    Log.WriteLog("Success opening COMOS DB:"+connectionstring, System.Diagnostics.EventLogEntryType.Information);
                    m_ComosSession.ReleaseCOMOSObjects();
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog("Exception opening COMOS DB:" + ex.Message, System.Diagnostics.EventLogEntryType.Error);
                //ComosBRWeb.LogHandler.WriteLog(ex.Message + ":Open");
                System.Diagnostics.Debug.Assert(false);
            }
            return true;
        }

        /// <summary>
        /// Verifice se COMOS usar banco de dados definido.
        /// !!! Este methodo se comporta static.
        /// </summary>
        /// <param name="connectionstring"></param>
        /// <returns>True é o sucesso, false caso contrário.</returns>
        public bool IsOpen()
        {
            try
            {
                return (m_ComosSession != null);
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.Message + ":IsOpen", System.Diagnostics.EventLogEntryType.Warning);
                System.Diagnostics.Debug.Assert(false);
            }
            return false;
        }

        /// <summary>
        /// Fechar sessão COMOS e terminar applicação comos.io.
        /// !!! Este methodo se comporta static.
        /// </summary>
        public bool Close()
        {
            try
            {
                return this.Close(true);
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.Message + ":Close", System.Diagnostics.EventLogEntryType.Warning);
                System.Diagnostics.Debug.Assert(false);
            }
            return false;
        }

        /// <summary>
        /// Fechar sessão COMOS
        /// </summary>
        /// <param name="doexit">Terminar applicação sim=true/não=false</param>
        public bool Close(bool doexit)
        {
            try
            {
                Log.WriteLog("Closing service and exit app = {0}",doexit);
                if (m_ComosSession == null)
                    return false;
                m_ComosSession.Close();
                m_ComosSession = null;
                if (doexit)
                {
                    // Can not imediatky exit as it would close also the service host.
                    // We need to delay it.
                    //System.Windows.Forms.Application.Exit();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.Message + ":Close", System.Diagnostics.EventLogEntryType.Warning);
                System.Diagnostics.Debug.Assert(false);
            }
            return false;
        }
        

        /// <summary>
        /// Executar quando o objeto é excluído.
        /// @ToDo: Não tenho certeza se necessário.
        /// </summary>
        ~IOServiceContract()
        {
            try
            {
                if (m_ComosSession != null)
                    m_ComosSession.Close();
                m_ComosSession = null;
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.Message, System.Diagnostics.EventLogEntryType.Warning);
                System.Diagnostics.Debug.Assert(false);
            }
        }

        /// <summary>
        /// Obter todos as projetos e camadas disponíveis para o usuário.
        /// </summary>
        /// <param name="user">Usario registro no COMOS.</param>
        /// <returns>Todos projetos e Camadas.</returns>
        public List<IBRServiceContracts.CProject> GetProjectsAndLayers(string user)
        {
            try
            {
                if (m_ComosSession == null)
                {
                    return null;
                }

                m_ComosSession.SetCurrentUser(user);
                List<IBRServiceContracts.CProject> cProjects = new List<IBRServiceContracts.CProject>();
                foreach (var project in m_ComosSession.GetProjects())
                {
                    if (project.Name.StartsWith("@"))
                        continue;

                    IBRServiceContracts.CProject cProject = new IBRServiceContracts.CProject() { Description = project.Description, Name = project.Name };
                    List<IBRServiceContracts.CLayer> layers = new List<IBRServiceContracts.CLayer>();
                    foreach (var layer in m_ComosSession.GetWorkingOverlays(project))
                    {
                        IBRServiceContracts.CLayer cLayer = new IBRServiceContracts.CLayer() { Description = layer.Description, ID = layer.ID(), Name = layer.Name, };
                        cLayer.WorkingLayers = GetWorkingLayers(layer);
                        layers.Add(cLayer);
                    }
                    cProject.WorkingLayers = layers;
                    cProjects.Add(cProject);
                }
                return cProjects;
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.Message + ":GetProjectsAndLayers", System.Diagnostics.EventLogEntryType.Warning);
                System.Diagnostics.Debug.Assert(false);
            }
            return null;
        }

        /// <summary>
        /// Obter todos as e camadas em camda.
        /// </summary>
        /// <param name="parentlayer">Camada de trabalho</param>
        /// <returns>Todos Camadas.</returns>
        private List<IBRServiceContracts.CLayer> GetWorkingLayers(Plt.IComosDWorkingOverlay parentlayer)
        {
            try
            {
                List<IBRServiceContracts.CLayer> layers = new List<IBRServiceContracts.CLayer>();
                foreach (var layer in m_ComosSession.GetWorkingOverlays(parentlayer))
                {
                    IBRServiceContracts.CLayer clayer = new IBRServiceContracts.CLayer() { Name = layer.Name, Description = layer.Description, ID = layer.ID() };
                    clayer.WorkingLayers = GetWorkingLayers(layer);
                    layers.Add(clayer);
                }
                return layers;
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.Message + ":GetWorkingLayers", System.Diagnostics.EventLogEntryType.Warning);
                System.Diagnostics.Debug.Assert(false);
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="projectname"></param>
        /// <param name="workinglayer"></param>
        /// <param name="query_fullname"></param>
        /// <param name="startobject_fullname"></param>
        /// <returns></returns>
        public TResult<CQueryResult> ExecuteQuery(string user, string projectname, int workinglayer, string query_fullname, string startobject_fullname)
        {
            try
            {
                if (m_ComosSession == null)
                {
                    return new TResult<CQueryResult>()
                    {
                        Status = false,
                        Message = "Session is NULL",
                    };
                }

                m_ComosSession.SetCurrentUser(user);
                m_ComosSession.SetCurrentProjectAndWorkingOverlay(projectname, workinglayer);
                var device = m_ComosSession.GetDeviceByFullName(query_fullname);
                Plt.IComosBaseObject startobject = null;
                if (!string.IsNullOrEmpty(startobject_fullname))
                {
                    startobject = m_ComosSession.GetDeviceByFullName(startobject_fullname);
                }
                var table = m_ComosSession.GetQueryData(device, startobject);

                return new TResult<CQueryResult>()
                {
                    data = table,
                    Status = true,
                };
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.Message + ":ExecuteQuery", System.Diagnostics.EventLogEntryType.Warning);
                System.Diagnostics.Debug.Assert(false);
            }
            return null;
        }



        public CUserInfo ValidateUser(string user)
        {
            CUserInfo userinfo = new CUserInfo();

            try
            {
                Log.WriteLog(user, System.Diagnostics.EventLogEntryType.Information);

                Plt.IComosDUser userobject = m_ComosSession.GetUserInfo(user);

                if (userobject != null)
                {
                    userinfo.UserUID = userobject.SystemUID();
                    userinfo.Name = userobject.Name;

                    if (userobject.GetRemark(5) != "")
                    {
                        Plt.IComosDWorkset workset = (Plt.IComosDWorkset)userobject.Workset();
                        Plt.IComosDDevice objuser = (IComosDDevice)workset.LoadObjectByType(ComosSystemTypes.SystemTypeDevice, userobject.GetRemark(5));

                        if (objuser != null)
                        {
                            userinfo.SystemFullName = objuser.SystemFullName();
                        }

                    }

                    m_ComosSession.SetCurrentUser(user);
                }
                else
                {
                    return null;
                }
              
                return userinfo;
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.ToString(), System.Diagnostics.EventLogEntryType.Information);
                System.Diagnostics.Debug.WriteLine(ex.Message + ":ValidateUser");
                System.Diagnostics.Debug.Assert(false);
                return null;
            }

        }

        public string GetObjectDetails(string user, string projectname, int workinglayer, string object_sysuid)
        {
            try
            {
                m_ComosSession.SetCurrentUser(user);
                m_ComosSession.SetCurrentProjectAndWorkingOverlay(projectname, workinglayer);
                Plt.IComosBaseObject device = m_ComosSession.GetComosDeviceBySystemUID(object_sysuid);
                //return ComosSpecsEngine.RetornaHTMLTela(device);
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.Message + ":GetObjectDetails", System.Diagnostics.EventLogEntryType.Warning);
                System.Diagnostics.Debug.Assert(false);
            }
            return "";
        }

        public string GetSessionId()
        {
            return Guid.NewGuid().ToString();
        }

        public TResult<CQueryResult> BuscaString(string stringbuscar, string projectname, int workinglayer)
        {

            try
            {

                m_ComosSession.SetCurrentProjectAndWorkingOverlay(projectname, workinglayer);
                TResult<CQueryResult> table = m_ComosSession.BuscaString(stringbuscar);
                return table;
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.Message + ":BuscaString", System.Diagnostics.EventLogEntryType.Warning);
                System.Diagnostics.Debug.Assert(false);
                return null;

            }

        }

        public string ClickBotaoUid(string user, string systemuid, string nestedname, string project_name, int working_layer)
        {

            m_ComosSession.SetCurrentUser(user);
            m_ComosSession.SetCurrentProjectAndWorkingOverlay(project_name, working_layer);

            return m_ComosSession.ClickBotaoUid(systemuid, nestedname);            
        }

        public string DownloadFile(string systemuid, string projectname, int workinglayer, bool isrevision)
        {
            
           m_ComosSession.SetCurrentProjectAndWorkingOverlay(projectname, workinglayer);

            //TResult<string> result = new TResult<string>();
            
            string pathToFile = m_ComosSession.DownloadFile(systemuid, isrevision);

            //if (pathToFile != "")
            //{
            //    result.Status = true;
            //    result.data = pathToFile;
            //}else
            //{
            //    result.Status = false;
            //}

            return pathToFile;
        }


        public TResult<CQueryResult> ExecuteTaskQuery(string user, string projectname, int workinglayer, string opt, string startobject)
        {

            try
            {
                m_ComosSession.SetCurrentUser(user);
                m_ComosSession.SetCurrentProjectAndWorkingOverlay(projectname, workinglayer);
                string query = null;
                if (m_ComosSession.TryGetLinkQueryFullName(opt, out query))
                {
                    TResult<CQueryResult> result = new TResult<CQueryResult>();
                    result = ExecuteQuery(user, projectname, workinglayer, query, startobject);
                    result.Status = true;
                    return result;
                }
                else
                {
                    TResult<CQueryResult> result = new TResult<CQueryResult>();
                    result.Message = query;
                    result.Status = false;
                    return result;
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.Message + ":ExecuteTaskQuery" + ex.StackTrace, System.Diagnostics.EventLogEntryType.Warning);
                System.Diagnostics.Debug.Assert(false);
                return new TResult<CQueryResult>()
                {
                    Message = ex.Message,
                    Status = false,
                    data = null,
                };
            }
        }

        //public IBRServiceContracts.ComosActivity GetComosActivityBySysUID(string user, string projectname, int workinglayer, string object_sysuid)
        //{

        //    Plt.IComosBaseObject device = null;

        //    try
        //    {
        //        m_ComosSession.SetCurrentUser(user);
        //        m_ComosSession.SetCurrentProjectAndWorkingOverlay(projectname, workinglayer);

        //        device = m_ComosSession.GetComosDeviceBySystemUID(object_sysuid);

        //        return ComosSpecsEngine.GetDeviceSpecifications(device);
        //    }
        //    catch (Exception ex)
        //    {
        //        ComosBRWeb.LogHandler.WriteLog(ex.Message + ":GetComosActivityBySysUID");
        //        System.Diagnostics.Debug.Assert(false);
        //    }
        //    return null;
        //}

        //public IBRServiceContracts.ComosActivity GetComosActivityBySysFullName(string user, string projectname, int workinglayer, string object_sysuid)
        //{

        //    Plt.IComosBaseObject device = null;

        //    try
        //    {
        //        m_ComosSession.SetCurrentUser(user);
        //        m_ComosSession.SetCurrentProjectAndWorkingOverlay(projectname, workinglayer);

        //        device = m_ComosSession.GetDeviceByFullName(object_sysuid);

        //        return ComosSpecsEngine.GetDeviceSpecifications(device);
        //    }
        //    catch (Exception ex)
        //    {
        //        ComosBRWeb.LogHandler.WriteLog(ex.Message + ":GetComosActivityBySysFullName");
        //        System.Diagnostics.Debug.Assert(false);
        //    }
        //    return null;
        //}

        // @ToDo : Split in Sys and fullname. Avoid exception.
        public IBRServiceContracts.ComosActivity GetComosActivity(string user, string projectname, int workinglayer, string object_sysuid)
        {

            Plt.IComosBaseObject device = null;

            try
            {
                m_ComosSession.SetCurrentUser(user);
                m_ComosSession.SetCurrentProjectAndWorkingOverlay(projectname, workinglayer);

                if (object_sysuid.Contains("|")){

                    device = m_ComosSession.GetDeviceByFullName(object_sysuid);
                }
                else
                {
                    device = m_ComosSession.GetComosDeviceBySystemUID(object_sysuid);

                    if (device == null)
                    {
                        device = m_ComosSession.GetComosDocumentBySystemUID(object_sysuid);
                    }
                }

                if (device != null)
                {
                    
                }
            }
            catch (Exception)
            {
               // ComosBRWeb.LogHandler.WriteLog(ex.Message + ":GetComosActivity");
              //  System.Diagnostics.Debug.Assert(false);
            }
            return null;
        }

        public bool UpdateComosValues(IBRServiceContracts.CCell[] specs, string user, string projectname, int workinglayer)
        {
            try
            {
                m_ComosSession.SetCurrentUser(user);
                m_ComosSession.SetCurrentProjectAndWorkingOverlay(projectname, workinglayer);
                //Log.WriteLog(":UpdateComosValues", System.Diagnostics.EventLogEntryType.Information);
                return m_ComosSession.ChangeComosValues(specs);

            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.Message + ":ChangeComosValues", System.Diagnostics.EventLogEntryType.Warning);
                System.Diagnostics.Debug.Assert(false);
            }
            return false;
        }

        public bool BatchUpdateComosValues(IBRServiceContracts.CRow[] specs, string user, string projectname, int workinglayer)
        {
            try
            {
                m_ComosSession.SetCurrentUser(user);
                m_ComosSession.SetCurrentProjectAndWorkingOverlay(projectname, workinglayer);
                return m_ComosSession.BatchChangeComosValues(specs);
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.Message + ":BatchChangeComosValues", System.Diagnostics.EventLogEntryType.Warning);
                System.Diagnostics.Debug.Assert(false);
            }
            return false;
        }

        public bool CreateComosDevice(IBRServiceContracts.CCell[] specs, string user, string projectname, int idworkinglayer, string ownersystemfullname, string cdevsystemfullname)
        {
            try
            {
                m_ComosSession.SetCurrentUser(user);
                m_ComosSession.SetCurrentProjectAndWorkingOverlay(projectname, idworkinglayer);
                return m_ComosSession.CreateDevice(ownersystemfullname, cdevsystemfullname, specs);
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.Message + ":createComosDevice", System.Diagnostics.EventLogEntryType.Warning);
                System.Diagnostics.Debug.Assert(false);
            }
            return false;
        }

        public bool CreateComosDeviceBySystemUID(IBRServiceContracts.CCell[] specs, string user, string projectname, int idworkinglayer, string ownersystemuid, string cdevsystemfullname)
        {
            try
            {
                m_ComosSession.SetCurrentUser(user);
                m_ComosSession.SetCurrentProjectAndWorkingOverlay(projectname, idworkinglayer);
                return m_ComosSession.createDeviceBySystemUID(ownersystemuid, cdevsystemfullname, specs);
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.Message + ":createComosDevice", System.Diagnostics.EventLogEntryType.Warning);
                System.Diagnostics.Debug.Assert(false);
            }
            return false;
        }

        public IBRServiceContracts.CCell[] GetTaskDataByUid(string user, string projectname, int workinglayer,string systemuid)            
        {
            try
            {
                m_ComosSession.SetCurrentUser(user);
                m_ComosSession.SetCurrentProjectAndWorkingOverlay(projectname, workinglayer);

                return m_ComosSession.GetTaskDataByUid(systemuid);

            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.Message + ":GetTaskDataByUid", System.Diagnostics.EventLogEntryType.Warning);
                System.Diagnostics.Debug.Assert(false);
            }
            return null;
        }
        

        public TResult<bool> AddImageToComosObject(byte[] imagestream, string user, string projectname, int workinglayer, string coSystemFullName, string docname)
        {
            try
            {
                m_ComosSession.SetCurrentUser(user);
                m_ComosSession.SetCurrentProjectAndWorkingOverlay(projectname, workinglayer);
                var memstream = new System.IO.MemoryStream(imagestream);
                var img = System.Drawing.Bitmap.FromStream(memstream);
                string error = m_ComosSession.AddImageToComosObject(coSystemFullName, docname, img);
                return new TResult<bool>()
                {
                    data = error == null,
                    Message = error,
                    Status = error == null
                };
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.Message + ":AddImageToComosObject", System.Diagnostics.EventLogEntryType.Warning);
                System.Diagnostics.Debug.Assert(false);
                return new TResult<bool>()
                {
                    data = true,
                    Message = ex.Message + ":AddImageToComosObject",
                    Status = true
                };
            }
        }

        public TResult<bool> ForceCrash(string ctype)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Interface Version 1

        public TResult<CWriteValueResult[]> WriteComosValues(CWriteValueCollection values, string user, string projectname, string workinglayer, string language)
        {
            try
            {
                Log.WriteLog("WriteComosValues: Set Current User = {0}",user);
                if (!m_ComosSession.SetCurrentUser(user))
                {
                    Log.WriteLog("WriteComosValues: Can not set user.", System.Diagnostics.EventLogEntryType.Warning);
                    return new TResult<CWriteValueResult[]>()
                    {
                        Status = false,
                        data = null,
                        Message = "Failed settings current user",
                    };
                }
                Log.WriteLog("WriteComosValues: SetCurrentProjectAndWorkingOverlay = {0} - {1} - {2}",projectname, workinglayer, language);
                if (!m_ComosSession.SetCurrentProjectAndWorkingOverlay(projectname, workinglayer, language))
                {
                    Log.WriteLog("WriteComosValues: Can not set project and working layer.", System.Diagnostics.EventLogEntryType.Warning);
                    return new TResult<CWriteValueResult[]>()
                    {
                        Status = false,
                        data = null,
                        Message = "Failed settings working layer",
                    };
                }
                Log.WriteLog("WriteComosValues: WriteComosValues");
                Log.WriteLog(string.Format("\tUser = {0}, projectname = {1}, workinglayer={2}, language={3}", user, projectname, workinglayer, language), System.Diagnostics.EventLogEntryType.Information);
                foreach (var att in values.Values)
                {
                    Log.WriteLog(string.Format("\t\tSystemUID = {0}, NestedName = {1}, New={2}, Old={3}",
                        att.WebSystemUID, att.NestedName, att.NewValue, att.OldValue), System.Diagnostics.EventLogEntryType.Information);
                }
                var result = m_ComosSession.WriteComosValues(values);
                return new TResult<CWriteValueResult[]>()
                {
                    Status = true,
                    data = result.ToArray(),
                };
            }
            catch (Exception ex)
            {
                Log.WriteLog("WriteComosValues: Exception = {0}, Stack = {1}", ex.Message, ex.StackTrace);
                return new TResult<CWriteValueResult[]>()
                {
                    Status = false,
                    data = null,
                    Message = ex.Message,
                };
            }
        }

        public TResult<CQueryResult> SearchAllByNameAndDescription(string projectname, string workinglayer, string language, string tosearch)
        {
            m_ComosSession.SetCurrentUser("@SETUP");
            m_ComosSession.SetCurrentProjectAndWorkingOverlay(projectname, workinglayer, language);

            return m_ComosSession.SearchAllByNameAndDescription(projectname, workinglayer, language, tosearch);
        }

        public TResult<CQueryResult> SearchDocumentsByNameAndDescription(string projectname, string workinglayer, string language, string tosearch)
        {
            m_ComosSession.SetCurrentUser("@SETUP");
            m_ComosSession.SetCurrentProjectAndWorkingOverlay(projectname, workinglayer, language);

            return m_ComosSession.SearchDocumentsByNameAndDescription(projectname, workinglayer, language, tosearch);
        }

        public TResult<CQueryResult> SearchDevicesByNameAndDescription(string projectname, string workinglayer, string language, string tosearch, string filter = "")
        {
            m_ComosSession.SetCurrentUser("@SETUP");
            m_ComosSession.SetCurrentProjectAndWorkingOverlay(projectname, workinglayer, language);

            return m_ComosSession.SearchDevicesByNameAndDescription(projectname, workinglayer, language, tosearch, filter);
        }

        public TResult<string> CreateComosDeviceByWebUID(CWriteValueCollection values, string projectname, string workinglayer, string language, string owner, string cdev, string user,string desc)
        {
            m_ComosSession.SetCurrentUser(user);
            m_ComosSession.SetCurrentProjectAndWorkingOverlay(projectname, workinglayer, language);

            string newuid = m_ComosSession.CreateComosDeviceByWebUID(owner, cdev,desc);
            bool created = false;

            foreach (var att in values.Values)
            {
                att.WebSystemUID = newuid;
            }

            if (newuid != "")
            {
                var result = m_ComosSession.WriteComosValues(values);
                created = true;
            }
           
            return new TResult<string>
            {
                data = newuid,
                Status = created
            };
        }

        public void UploadFileToComosDevice(RemoteFileInfo fileinfo)
        {
            string fullFileName = fileinfo.FullFileName;

            Log.WriteLog("UploadFileToComosDevice::" + (fileinfo == null));
            Log.WriteLog(string.Format("Project: {0} , Overlay: {1}, Owner: {2}, FileName ={3}, User = {4}", fileinfo.Project,
                fileinfo.Workinglayer, fileinfo.Owner, fileinfo.FileName,fileinfo.User));

            m_ComosSession.SetCurrentUser(fileinfo.User);
            m_ComosSession.SetCurrentProjectAndWorkingOverlay(fileinfo.Project, fileinfo.Workinglayer,"");

            Log.WriteLog("FromStream::" + (fileinfo.FileByteStream == null));
            var img = System.Drawing.Bitmap.FromStream(fileinfo.FileByteStream);
            
            m_ComosSession.AddPictureToComosObject(fileinfo.Owner, fileinfo.FileName, fileinfo.Description, img,fileinfo.FullFileName);
            Log.WriteLog("PIC:SAVED:");
         
        }

        public void UploadFileToComosDevice2(RemoteFileInfo fileinfo)
        {

            Log.WriteLog("UploadFileToComosDevice2::" + (fileinfo == null));

            Log.WriteLog(string.Format("Project: {0} , Overlay: {1}, Owner: {2}, FileName ={3}, User = {4}", fileinfo.Project,
                   fileinfo.Workinglayer, fileinfo.Owner, fileinfo.FileName, fileinfo.User));

            m_ComosSession.SetCurrentUser(fileinfo.User);
            m_ComosSession.SetCurrentProjectAndWorkingOverlay(fileinfo.Project, fileinfo.Workinglayer, "");

            Log.WriteLog("FromStream::" + (fileinfo.FileByteStream == null));
            Log.WriteLog("owner::" + (fileinfo.Owner));
            Log.WriteLog("filename::" + (fileinfo.FullFileName));

            m_ComosSession.AddFileToComosObject(fileinfo.Owner, fileinfo.FileName, fileinfo);

            Log.WriteLog("FILE:SAVED:");          

        }

        public void CreateRedLinesAndAditionalDocuments(RemoteFileInfo fileinfo)
        {
            Log.WriteLog("CreateRedLinesAndAditionalDocuments::" + (fileinfo == null));

            Log.WriteLog(string.Format("Project: {0} , Overlay: {1}, Owner: {2}, FileName ={3}, User = {4}", fileinfo.Project,
            fileinfo.Workinglayer, fileinfo.Owner, fileinfo.FileName, fileinfo.User));

            m_ComosSession.SetCurrentUser(fileinfo.User);
            m_ComosSession.SetCurrentProjectAndWorkingOverlay(fileinfo.Project, fileinfo.Workinglayer, "");

            Log.WriteLog("FromStream::" + (fileinfo.FileByteStream == null));
            Log.WriteLog("owner::" + (fileinfo.Owner));
            Log.WriteLog("filename::" + (fileinfo.FileName));

            m_ComosSession.LoadDocumentAndCreateRedLines(fileinfo.Owner, fileinfo.FileName, fileinfo);

            Log.WriteLog("FILE:SAVED:");

        }

        public bool IsAlive()
        {
            return true;
        }

        #endregion
    }
}
