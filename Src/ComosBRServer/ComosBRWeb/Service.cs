using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using IBRServiceContracts;
using System.Web.Hosting;
using System.ServiceModel.Web;
using System.Web;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ComosBRWeb
{
    public class Service : IWebService
    {
        private IServiceContract GetComosAPI()
        {
            try
            {
                object comosobject = HttpContext.Current.ApplicationInstance.Application["ComosAPI"];
                IServiceContract m_ComosAPIService = (IServiceContract)comosobject;
                //if (m_ComosAPIService.IsAlive())
                    return m_ComosAPIService;
            }
            catch (Exception)
            {
                throw;
                //ComosBRWeb.Global
            }
            
            //return m_ComosAPIService;
        }
        public System.IO.Stream Pages(string filename)
        {
            try
            {
                if (filename.EndsWith(".html",true,System.Globalization.CultureInfo.InvariantCulture))
                {
                    WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
                }
                if (filename.EndsWith(".css", true, System.Globalization.CultureInfo.InvariantCulture))
                {
                    WebOperationContext.Current.OutgoingResponse.ContentType = "text/css";
                }

                return System.IO.File.OpenRead(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "/" + filename);
            }
            catch (Exception ex)
            {
                string result = ex.Message;
                byte[] resultBytes = Encoding.UTF8.GetBytes(result);
                WebOperationContext.Current.OutgoingResponse.ContentType = "text/plain";
                return new MemoryStream(resultBytes);
            }
        }

        public TResult<CUserInfo> ValidateUser(string user_input,string password)
        {
            try
            {
                LogHandler.WriteLog("Comos.Web Validating user. V1.2", System.Diagnostics.EventLogEntryType.Information);

                //object comosobject = System.Web.HttpContext.Current.ApplicationInstance.Application["ComosAPI"];
                //IBRServiceContracts.IServiceContract m_ComosAPIService = (IBRServiceContracts.IServiceContract)comosobject;
                IServiceContract m_ComosAPIService = GetComosAPI();
                if (m_ComosAPIService.IsAlive())
                {
                    LogHandler.WriteLog("Comos.Web Validating user. V1.2. COMOS API is ALIVE", System.Diagnostics.EventLogEntryType.Information);

                }         
                CUserInfo userinfo = m_ComosAPIService.ValidateUser(user_input);
                if (userinfo == null)
                {
                    return new TResult<CUserInfo>()
                    {
                        Status = false,
                        data = userinfo,
                        Message = "Not registered",
                    };
                }
                return new TResult<CUserInfo>()
                {
                    Status = true,
                    data = userinfo,
                    Message = null,
                };
            }
            catch (Exception ex)
            {

                LogHandler.WriteLog(ex.Message);

                return new TResult<CUserInfo>()
                {
                    Status = false,
                    data = null,
                    Message = ex.Message,
                };
            }
        }

        public TResult<List<CProject>> GetProjectsAndLayers(string userinput)
        {
            try
            {
                //object comosobject = System.Web.HttpContext.Current.ApplicationInstance.Application["ComosAPI"];
                //IBRServiceContracts.IServiceContract m_ComosAPIService = (IBRServiceContracts.IServiceContract)comosobject;
                IServiceContract m_ComosAPIService = GetComosAPI();
                List<CProject> projects = m_ComosAPIService.GetProjectsAndLayers(userinput);
                return new TResult<List<CProject>>()
                {
                    Status = true,
                    data = projects,
                    Message = null,
                };
            }
            catch(Exception ex)
            {
                LogHandler.WriteLog(ex.Message);

                return new TResult<List<CProject>>()
                {
                    Status = false,
                    data = null,
                    Message = ex.Message,
                };
            }
        }

        /// <summary>
        /// Run a qeury and return the results as a CQueryResult.
        /// </summary>
        /// <param name="user">Usario registro no COMOS.</param>
        /// <param name="projectname">Nome da projeto</param>
        /// <param name="workinglayer">ID da camada</param>
        /// <param name="query_fullname">System Full Name da Query object.</param>
        /// <param name="startobject_fullname"></param>
        /// <returns></returns>
        public TResult<CQueryResult> ExecuteQuery(string user, string projectname, string workinglayer, string query_fullname, string startobject_fullname)
        {
            try
            {
                int wo_id = 0;
                if (!int.TryParse(workinglayer, out wo_id))
                    return null;
                //object comosobject = System.Web.HttpContext.Current.ApplicationInstance.Application["ComosAPI"];
                //IBRServiceContracts.IServiceContract m_ComosAPIService = (IBRServiceContracts.IServiceContract)comosobject;
                IServiceContract m_ComosAPIService = GetComosAPI();
                TResult<CQueryResult> result = m_ComosAPIService.ExecuteQuery(user,projectname,wo_id,query_fullname,startobject_fullname);
                return result;
            }
            catch (Exception ex)
            {
                LogHandler.WriteLog(ex.Message);
                return new TResult<CQueryResult>()
                {
                    Status = false,
                    data = null,
                    Message = ex.Message,
                };
            }
        }

        public TResult<CQueryResult> ExecuteLinkedQuery(string user, string projectname, string workinglayer, string opt, string startobject)
        {

            //object comosobject = System.Web.HttpContext.Current.ApplicationInstance.Application["ComosAPI"];
            //IBRServiceContracts.IServiceContract m_ComosAPIService = (IBRServiceContracts.IServiceContract)comosobject;
            IServiceContract m_ComosAPIService = GetComosAPI();

            int idworkinglayer = 0;

            if (workinglayer != "")
            {
                idworkinglayer = int.Parse(workinglayer);
            }


            TResult<CQueryResult> result = m_ComosAPIService.ExecuteTaskQuery(user, projectname, idworkinglayer, opt, startobject);

            return result;

        }


        public TResult<string> GetObjectDetails(string user, string projectname, string workinglayer, string object_sysuid)
        {
            try
            {
                int wo_id = 0;
                if (!int.TryParse(workinglayer, out wo_id))
                    return null;
                //object comosobject = System.Web.HttpContext.Current.ApplicationInstance.Application["ComosAPI"];
                //IBRServiceContracts.IServiceContract m_ComosAPIService = (IBRServiceContracts.IServiceContract)comosobject;
                IServiceContract m_ComosAPIService = GetComosAPI();
                string result = m_ComosAPIService.GetObjectDetails(user, projectname, wo_id, object_sysuid);
                return new TResult<string>()
                {
                    Status = true,
                    data = result,
                    Message = null,
                };
            }
            catch (Exception ex)
            {
                LogHandler.WriteLog(ex.Message);
                return new TResult<string>()
                {
                    Status = false,
                    data = null,
                    Message = ex.Message,
                };
            }
        }

        public TResult<CQueryResult> BuscaString(string user_input, string project, string camada)
        {
            try
            {
                //object comosobject = System.Web.HttpContext.Current.ApplicationInstance.Application["ComosAPI"];
                //IBRServiceContracts.IServiceContract m_ComosAPIService = (IBRServiceContracts.IServiceContract)comosobject;
                IServiceContract m_ComosAPIService = GetComosAPI();
                return m_ComosAPIService.BuscaString(user_input, project, int.Parse(camada));
            }
            catch (Exception ex)
            {
                LogHandler.WriteLog(ex.Message);
                return null;
            }

        }
				    
            
         public string ClickBotaoUid(string user, string systemuid, string nestedname, string project_name, string working_layer)
        {
            //object comosobject = System.Web.HttpContext.Current.ApplicationInstance.Application["ComosAPI"];
            //IBRServiceContracts.IServiceContract m_ComosAPIService = (IBRServiceContracts.IServiceContract)comosobject;
            IServiceContract m_ComosAPIService = GetComosAPI();

            int idworkinglayer = 0;

            if (working_layer != "")
            {
                idworkinglayer = int.Parse(working_layer);
            }

            return m_ComosAPIService.ClickBotaoUid( user,  systemuid,  nestedname,  project_name, idworkinglayer);
            
        }
        
        public System.IO.Stream DownloadFile(string uidDocument, string project_name, string working_layer, string revision_file)
        {

            string downloadFilePath = "";
            

            try
            {
                IServiceContract m_ComosAPIService = GetComosAPI();

                int id_working_layer = int.Parse(working_layer);
                System.IO.FileInfo fileInfo = null;

                bool isrev = false;

                if (revision_file.ToUpper() == "TRUE")                
                    isrev = true;                            

            downloadFilePath = m_ComosAPIService.DownloadFile(uidDocument, project_name, id_working_layer, isrev);

             LogHandler.WriteLog("Service.CS:Download::" + downloadFilePath);

            fileInfo = new System.IO.FileInfo(downloadFilePath);
                
                if (fileInfo.Exists)
                {
                    string fileName = fileInfo.Name;
                    String headerInfo = "attachment; filename=" + fileName;
                    WebOperationContext.Current.OutgoingResponse.Headers["Content-Disposition"] = headerInfo;
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/octet-stream";

                    /*result.Status = true;
                    result.data = System.IO.File.OpenRead(downloadFilePath);
                    return result;
                    */
                    return System.IO.File.OpenRead(downloadFilePath);
                }
                else
                {                     
                    LogHandler.WriteLog("Arquivo não encontrado!::" + downloadFilePath);
                    return null;

                    //    result.Message = "Arquivo não encontrado!";
                    //  return result;
                }

            }
            catch (Exception ex)
            {
                LogHandler.WriteLog(ex.Message + "::" + downloadFilePath);
                //result.Message = "Erro: " + ex.Message;
                //return result;
                return null;
            }
            

        }

        public TResult<ComosActivity> GetComosActivity(string user, string projectname, string workinglayer, string object_sysuid)
        {

            //object comosobject = System.Web.HttpContext.Current.ApplicationInstance.Application["ComosAPI"];
            //IBRServiceContracts.IServiceContract m_ComosAPIService = (IBRServiceContracts.IServiceContract)comosobject;
            IServiceContract m_ComosAPIService = GetComosAPI();
            int idworkinglayer = 0;

            if (workinglayer != "")
            {
                idworkinglayer = int.Parse(workinglayer);
            }

            ComosActivity result = m_ComosAPIService.GetComosActivity(user, projectname, idworkinglayer, object_sysuid);

            return new TResult<ComosActivity>()
            {
                Status = true,
                data = result,
                Message = null,
            };

        }

        public TResult<IBRServiceContracts.CRow> GetTaskDataByUid(string user, string projectname, string workinglayer, string systemuid)
        {

            //object comosobject = System.Web.HttpContext.Current.ApplicationInstance.Application["ComosAPI"];
            //IBRServiceContracts.IServiceContract m_ComosAPIService = (IBRServiceContracts.IServiceContract)comosobject;
            IServiceContract m_ComosAPIService = GetComosAPI();
            int idworkinglayer = 0;

            if (workinglayer != "")
            {
                idworkinglayer = int.Parse(workinglayer);
            }

            IBRServiceContracts.CCell[] result = m_ComosAPIService.GetTaskDataByUid(user, projectname, idworkinglayer, systemuid);

            CRow row = new CRow();
            row.Values = result;

            return new TResult<IBRServiceContracts.CRow >()
            {
                Status = (result.Length > 0),
                data = row,
                Message = null,
            };

        }

        public TResult<bool> ChangeComosValues(IBRServiceContracts.CCell[] specs, string user, string projectname, string workinglayer,string systemfullname)
        {
            //object comosobject = System.Web.HttpContext.Current.ApplicationInstance.Application["ComosAPI"];
            //IBRServiceContracts.IServiceContract m_ComosAPIService = (IBRServiceContracts.IServiceContract)comosobject;
            IServiceContract m_ComosAPIService = GetComosAPI();
            int idworkinglayer = 0;

            if (workinglayer != "")
            {
                idworkinglayer = int.Parse(workinglayer);
            }
            ComosBRWeb.LogHandler.WriteLog(
                string.Format("WEB :ChangeComosValues {0}",specs[0].SystemFullName), System.Diagnostics.EventLogEntryType.Information);
            bool result = m_ComosAPIService.UpdateComosValues(specs,user, projectname, idworkinglayer);

            return new TResult<bool>()
            {
                Status = result,
                data = result,
                Message = null,
            };


        }

        public TResult<bool> BatchChangeComosValues(IBRServiceContracts.CRow[] specs, string user, string projectname, string workinglayer)
        {

            //object comosobject = System.Web.HttpContext.Current.ApplicationInstance.Application["ComosAPI"];
            //IBRServiceContracts.IServiceContract m_ComosAPIService = (IBRServiceContracts.IServiceContract)comosobject;
            IServiceContract m_ComosAPIService = GetComosAPI();
            int idworkinglayer = 0;

            if (workinglayer != "")
            {
                idworkinglayer = int.Parse(workinglayer);
            }
            

            bool result = m_ComosAPIService.BatchUpdateComosValues(specs, user, projectname, idworkinglayer);

            return new TResult<bool>()
            {
                Status = result,
                data = result,
                Message = null,
            };

        }

        public TResult<bool> CreateComosDevice(IBRServiceContracts.CCell[] specs, string user, string projectname, string workinglayer, string ownersystemfullname,string cdevsystemfullname)
        {

            //object comosobject = System.Web.HttpContext.Current.ApplicationInstance.Application["ComosAPI"];
            //IBRServiceContracts.IServiceContract m_ComosAPIService = (IBRServiceContracts.IServiceContract)comosobject;
            IServiceContract m_ComosAPIService = GetComosAPI();
            int idworkinglayer = 0;

            if (workinglayer != "")
            {
                idworkinglayer = int.Parse(workinglayer);
            }

            bool result = m_ComosAPIService.CreateComosDevice(specs, user, projectname, idworkinglayer, ownersystemfullname, cdevsystemfullname);

            return new TResult<bool>()
            {
                Status = result,
                data = result,
                Message = null,
            };


        }


        public TResult<bool> CreateComosDeviceBySystemUID(IBRServiceContracts.CCell[] specs, string user, string projectname, string workinglayer, string ownersystemuid, string cdevsystemfullname)
        {

            //object comosobject = System.Web.HttpContext.Current.ApplicationInstance.Application["ComosAPI"];
            //IBRServiceContracts.IServiceContract m_ComosAPIService = (IBRServiceContracts.IServiceContract)comosobject;
            IServiceContract m_ComosAPIService = GetComosAPI();
            int idworkinglayer = 0;

            if (workinglayer != "")
            {
                idworkinglayer = int.Parse(workinglayer);
            }

            bool result = m_ComosAPIService.CreateComosDeviceBySystemUID(specs, user, projectname, idworkinglayer, ownersystemuid, cdevsystemfullname);

            return new TResult<bool>()
            {
                Status = result,
                data = result,
                Message = null,
            };


        }


        public TResult<bool> AddImageToComosObject(Stream fileStream, string user, string projectname, string workinglayer, string coSystemFullName, string docname)
        {
            try
            {
                //object comosobject = System.Web.HttpContext.Current.ApplicationInstance.Application["ComosAPI"];
                //IBRServiceContracts.IServiceContract m_ComosAPIService = (IBRServiceContracts.IServiceContract)comosobject;
                IServiceContract m_ComosAPIService = GetComosAPI();
                int idworkinglayer = 0;

                if (workinglayer != "")
                {
                    idworkinglayer = int.Parse(workinglayer);
                }

                //fileStream.Position = 0;
                //byte[] buffer = new byte[fileStream.Length];
                //for (int totalBytesCopied = 0; totalBytesCopied < fileStream.Length;)
                //    totalBytesCopied += fileStream.Read(buffer, totalBytesCopied, Convert.ToInt32(fileStream.Length) - totalBytesCopied);
                MemoryStream memstream = new MemoryStream();
                fileStream.CopyTo(memstream);
                m_ComosAPIService.AddImageToComosObject(memstream.ToArray(), user, projectname, idworkinglayer, coSystemFullName, docname);

                return new TResult<bool>()
                {
                    Status = true,
                    data = true,
                    Message = null,
                };
            }
            catch (Exception ex)
            {
                return new TResult<bool>()
                {
                    Status = false,
                    data = false,
                    Message = ex.Message,
                };
            }
        }

        public TResult<bool> ForceCrash(string ctype)
        {
            if (ctype == "full")
            {
                //object comosobject = System.Web.HttpContext.Current.ApplicationInstance.Application["ComosAPI"];
                //IBRServiceContracts.IServiceContract m_ComosAPIService = (IBRServiceContracts.IServiceContract)comosobject;
                IServiceContract m_ComosAPIService = GetComosAPI();
                TResult<bool> result = m_ComosAPIService.ForceCrash(ctype);
                return result;
            }

            if (ctype == "internal")
            {
                try {
                    //object comosobject = System.Web.HttpContext.Current.ApplicationInstance.Application["ComosAPI"];
                    //IBRServiceContracts.IServiceContract m_ComosAPIService = (IBRServiceContracts.IServiceContract)comosobject;
                    IServiceContract m_ComosAPIService = GetComosAPI();
                    TResult<bool> result = m_ComosAPIService.ForceCrash(ctype);
                    return result;
                }
                catch (Exception)
                {
                   
                }

            }
            return new TResult<bool>()
            {
                data = false,
                Message = "",
                Status = false,
            };
        }

        public TResult<CWriteValueResult[]> WriteComosValues(CWriteValueCollection values, string user, string projectname, string workinglayer, string language)
        {
            try
            {
                //object comosobject = System.Web.HttpContext.Current.ApplicationInstance.Application["ComosAPI"];
                //IBRServiceContracts.IServiceContract m_ComosAPIService = (IBRServiceContracts.IServiceContract)comosobject;
                IServiceContract m_ComosAPIService = GetComosAPI();
                if (string.IsNullOrEmpty(workinglayer))
                {
                    LogHandler.WriteLog("Service.cs: WriteComosValues: Bad request workinglayer argument is empty", EventLogEntryType.Error);
                    return new TResult<CWriteValueResult[]>()
                    {
                        data = null,
                        Message = "Working Layer is empty",
                        Status = false,
                    };
                }

                LogHandler.WriteLog("Service.cs: WriteComosValues:", EventLogEntryType.Information);
                LogHandler.WriteLog(string.Format("\tUser = {0}, projectname = {1}, workinglayer={2}, language={3}", user, projectname, workinglayer, language), EventLogEntryType.Information);
                foreach (var att in values.Values)
                {
                    LogHandler.WriteLog(string.Format("\t\tSystemUID = {0}, NestedName = {1}, New={2}, Old={3}",
                        att.WebSystemUID, att.NestedName, att.NewValue, att.OldValue), EventLogEntryType.Information);
                }
                var result = m_ComosAPIService.WriteComosValues(values, user, projectname, workinglayer, language);
                return result;
            }
            catch (Exception ex)
            {
                LogHandler.WriteLog("Service.cs: WriteComosValues.  " + ex.Message, EventLogEntryType.Error);
                return new TResult<CWriteValueResult[]>()
                {
                    Status = false,
                    data = null,
                    Message = ex.Message,
                };
            }
        }

        public TResult<CWriteValueResult[]> WriteComosValuesTest(string user, string projectname, string workinglayer, string language)
        {
            return new TResult<CWriteValueResult[]>()
            {
                Status = false,
                data = null,
                Message = "Test:" + user + " " + projectname + " " + workinglayer + " " + language,
            };
        }
        
        public TResult<CWriteValueResult[]> WriteComosValuesTest2(CWriteValueCollection values, string user, string projectname, string workinglayer, string language)
        {
            return new TResult<CWriteValueResult[]>()
            {
                Status = false,
                data = null,
                Message = "Test:" + user + " " + projectname + " " + workinglayer + " " + language,
            };
        }

        public TResult<CQueryResult> SearchDevicesByNameAndDescription(string projectname, string workinglayer, string language, string tosearch)
        {
            //object comosobject = System.Web.HttpContext.Current.ApplicationInstance.Application["ComosAPI"];
            //IBRServiceContracts.IServiceContract m_ComosAPIService = (IBRServiceContracts.IServiceContract)comosobject;
            IServiceContract m_ComosAPIService = GetComosAPI();
            var result = m_ComosAPIService.SearchDevicesByNameAndDescription(projectname, workinglayer, language, tosearch);

            return result;
        }

        public TResult<CQueryResult> SearchAllByNameAndDescription(string projectname, string workinglayer, string language, string tosearch)
        {
            //object comosobject = System.Web.HttpContext.Current.ApplicationInstance.Application["ComosAPI"];
            //IBRServiceContracts.IServiceContract m_ComosAPIService = (IBRServiceContracts.IServiceContract)comosobject;
            IServiceContract m_ComosAPIService = GetComosAPI();
            var result = m_ComosAPIService.SearchAllByNameAndDescription(projectname, workinglayer, language, tosearch);

            return result;
        }

        public TResult<CQueryResult> SearchDocumentsByNameAndDescription(string projectname, string workinglayer,string language, string tosearch)
        {

            //object comosobject = System.Web.HttpContext.Current.ApplicationInstance.Application["ComosAPI"];
            //IBRServiceContracts.IServiceContract m_ComosAPIService = (IBRServiceContracts.IServiceContract)comosobject;
            IServiceContract m_ComosAPIService = GetComosAPI();
            var result = m_ComosAPIService.SearchDocumentsByNameAndDescription(projectname, workinglayer, language, tosearch);

            return result;
        }

        public TResult<CQueryResult> SearchDevicesByNameAndDescriptionWithFilter(string projectname, string workinglayer, string language, string tosearch,string filter)
        {

            //object comosobject = System.Web.HttpContext.Current.ApplicationInstance.Application["ComosAPI"];
            //IBRServiceContracts.IServiceContract m_ComosAPIService = (IBRServiceContracts.IServiceContract)comosobject;
            IServiceContract m_ComosAPIService = GetComosAPI();
            var result = m_ComosAPIService.SearchDevicesByNameAndDescription(projectname, workinglayer, language, tosearch, filter);

            return result;
        }


        public TResult<string> CreateComosDeviceByWebUID(CWriteValueCollection values, string projectname, string workinglayer, string language,string owner, string cdev, string user, string desc)
        {
            try
            {
                //object comosobject = System.Web.HttpContext.Current.ApplicationInstance.Application["ComosAPI"];
                //IBRServiceContracts.IServiceContract m_ComosAPIService = (IBRServiceContracts.IServiceContract)comosobject;
                IServiceContract m_ComosAPIService = GetComosAPI();
                if (string.IsNullOrEmpty(workinglayer))
                {
                    LogHandler.WriteLog("Service.cs: WriteComosValues: Bad request workinglayer argument is empty", EventLogEntryType.Error);
                    return new TResult<string>()
                    {
                        data = null,
                        Message = "Working Layer is empty",
                        Status = false,
                    };
                }

                return m_ComosAPIService.CreateComosDeviceByWebUID(values, projectname, workinglayer, language, owner, cdev, user, desc);

            }
            catch (Exception ex)
            {
                LogHandler.WriteLog("Service.cs: CreateComosDeviceByWebUID.  " + ex.Message, EventLogEntryType.Error);
                return new TResult<string>()
                {
                    Status = false,
                    data = null,
                    Message = ex.Message,
                };
            }
        }

        public TResult<bool> UploadFile(string filename, System.IO.Stream reader)
        {          
            System.Diagnostics.Debug.Assert(false);
            try
            {

                LogHandler.WriteLog("Service.cs: UploadFile.  " + filename, EventLogEntryType.Information);

                string user = System.Web.HttpContext.Current.Request.Headers["user"];
                string project = System.Web.HttpContext.Current.Request.Headers["projectname"];
                string layer = System.Web.HttpContext.Current.Request.Headers["layer"];
                string uid = System.Web.HttpContext.Current.Request.Headers["owner"];

                //object comosobject = System.Web.HttpContext.Current.ApplicationInstance.Application["ComosAPI"];
                //IBRServiceContracts.IServiceContract m_ComosAPIService = (IBRServiceContracts.IServiceContract)comosobject;
                IServiceContract m_ComosAPIService = GetComosAPI();

                RemoteFileInfo fileInfo = new RemoteFileInfo()
                {
                    FileByteStream = reader,
                    Project = project,
                    FullFileName = "",
                    Owner = uid,
                    Workinglayer = layer,                    
                    FileName = filename,
                    Description = "",
                };

                m_ComosAPIService.UploadFileToComosDevice(fileInfo);

                //using (var write = File.OpenWrite("c:\\temp\\"+filename))
                //{
                //    byte[] buffer = new byte[1024];
                //    int size = 0;
                //    //var reader = System.Web.HttpContext.Current.Request.GetBufferedInputStream();
                //    while ((size = reader.Read(buffer, 0, 1024)) != 0)
                //    {
                //        write.Write(buffer, 0, size);
                //    }
                //}
            }
            catch (Exception ex)
            {
                return new TResult<bool>() {
                    Status = false,
                    Message = ex.Message,
                    data = false
                };
            }
            //return null;
            return new TResult<bool>() {
            data = true,
            Status = true,
            };
        }

        public TResult<bool> UploadFile2(string filename, System.IO.Stream stream)
        {
            //used for uploading images

            System.Diagnostics.Debug.Assert(false);
            try
            {
                string user = System.Web.HttpContext.Current.Request.Headers["user"];
                string project = System.Web.HttpContext.Current.Request.Headers["projectname"];
                string layer = System.Web.HttpContext.Current.Request.Headers["layer"];
                string uid = System.Web.HttpContext.Current.Request.Headers["owner"];
                string description = System.Web.HttpContext.Current.Request.Headers["description"];

                string fullFilename = "c:\\temp\\" + filename + ".jpeg";

                LogHandler.WriteLog("Service.cs: UploadFile2.  : Starting "+filename, EventLogEntryType.Information);
                //var parser = new HttpMultipartParser.MultipartFormDataParser(stream);
                var parser = new HttpMultipartParser.StreamingMultipartFormDataParser(stream);
                parser.ParameterHandler += parameter => { };
                // Write the part of the file we've recieved to a file stream. (Or do something else)
                
                //filename = filename + "." + extension;
                var write = File.OpenWrite(fullFilename);

                LogHandler.WriteLog("Service.cs: UploadFile2.  : Creating File " + filename, EventLogEntryType.Information);
                IServiceContract m_ComosAPIService = GetComosAPI();

                parser.FileHandler += (name, fileName, type, disposition, buffer, bytes) =>
                {
                    LogHandler.WriteLog("   Service.cs: UploadFile2.  : Write block to File " + filename, EventLogEntryType.Information);
                    write.Write(buffer, 0, bytes);
                };
                parser.StreamClosedHandler += () =>
                {
                    LogHandler.WriteLog("   Service.cs: UploadFile2.  :  File stream closed" + filename, EventLogEntryType.Information);
                    // Do things when my input stream is closed
                    write.Flush();
                    write.Close();
                    write.Dispose();

                    using (var reader = System.IO.File.OpenRead(fullFilename))
                    {                        
                        RemoteFileInfo fileInfo = new RemoteFileInfo()
                        {
                            User = user,
                            FullFileName = fullFilename,
                            FileByteStream = reader,
                            Project = project,
                            Owner = uid,
                            Workinglayer = layer,                            
                            FileName = filename,
                            Description = description,
                        };
                        m_ComosAPIService.UploadFileToComosDevice(fileInfo);
                    }
                    
                };
                parser.Run();
                
            }
            catch (Exception ex)
            {
                LogHandler.WriteLog("Service.cs: UploadFile2.  : Exception = " + ex.Message, EventLogEntryType.Error);

                return new TResult<bool>()
                {
                    Status = false,
                    Message = ex.Message,
                    data = false
                };
            }
            //return null;
            return new TResult<bool>()
            {
                data = true,
                Status = true,
            };
        }

        public TResult<bool> UploadFile3(string filename, System.IO.Stream stream)
        {
            System.Diagnostics.Debug.Assert(false);
            try
            {
                string user = System.Web.HttpContext.Current.Request.Headers["user"];
                string project = System.Web.HttpContext.Current.Request.Headers["projectname"];
                string layer = System.Web.HttpContext.Current.Request.Headers["layer"];
                string uid = System.Web.HttpContext.Current.Request.Headers["owner"];
                string description = System.Web.HttpContext.Current.Request.Headers["description"];
                string extension = System.Web.HttpContext.Current.Request.Headers["extension"];

                LogHandler.WriteLog("Service.cs: UploadFile3.  : Starting" + filename, EventLogEntryType.Information);
                //var parser = new HttpMultipartParser.MultipartFormDataParser(stream);
                var parser = new HttpMultipartParser.StreamingMultipartFormDataParser(stream);
                parser.ParameterHandler += parameter => { };
                // Write the part of the file we've recieved to a file stream. (Or do something else)

                filename = filename + "." + extension;
                LogHandler.WriteLog("Service.cs: UploadFile3.  : Creating File " + filename, EventLogEntryType.Information);
                var write = File.OpenWrite("c:\\temp\\" + filename);


                //object comosobject = System.Web.HttpContext.Current.ApplicationInstance.Application["ComosAPI"];
                //IBRServiceContracts.IServiceContract m_ComosAPIService = (IBRServiceContracts.IServiceContract)comosobject;
                IServiceContract m_ComosAPIService = GetComosAPI();


                parser.FileHandler += (name, fileName, type, disposition, buffer, bytes) =>
                {
                    LogHandler.WriteLog("   Service.cs: UploadFile3.  : Write block to File " + filename, EventLogEntryType.Information);
                    write.Write(buffer, 0, bytes);
                };
                parser.StreamClosedHandler += () =>
                {
                    LogHandler.WriteLog("   Service.cs: UploadFile3.  :  File stream closed" + filename, EventLogEntryType.Information);
                    // Do things when my input stream is closed
                    write.Flush();
                    write.Close();
                    write.Dispose();

                    using (var reader = System.IO.File.OpenRead("c:\\temp\\" + filename))
                    {
                        RemoteFileInfo fileInfo = new RemoteFileInfo()
                        {
                            User = user,
                            FileByteStream = reader,
                            Project = project,
                            Owner = uid,
                            Workinglayer = layer,
                            FullFileName = "c:\\temp\\" + filename,
                            FileName = filename,
                            Extension = extension,
                            Description = description,
                        };
                        m_ComosAPIService.UploadFileToComosDevice2(fileInfo);
                    }

                };
                parser.Run();
                
            }
            catch (Exception ex)
            {
                LogHandler.WriteLog("Service.cs: UploadFile3.  : Exception = " + ex.Message, EventLogEntryType.Error);

                return new TResult<bool>()
                {
                    Status = false,
                    Message = ex.Message,
                    data = false
                };
            }
            //return null;
            return new TResult<bool>()
            {
                data = true,
                Status = true,
            };
        }



        public TResult<bool> UploadRedLinesAddiotionalFiles(string filename, System.IO.Stream stream)
        {
            System.Diagnostics.Debug.Assert(false);
            try
            {
                string user = System.Web.HttpContext.Current.Request.Headers["user"];
                string project = System.Web.HttpContext.Current.Request.Headers["projectname"];
                string layer = System.Web.HttpContext.Current.Request.Headers["layer"];
                string uid = System.Web.HttpContext.Current.Request.Headers["owner"];
                string description = System.Web.HttpContext.Current.Request.Headers["description"];
                string extension = System.Web.HttpContext.Current.Request.Headers["extension"];

                LogHandler.WriteLog("Service.cs: UploadRedLinesAddiotionalFiles.  : Starting" + filename, EventLogEntryType.Information);
                //var parser = new HttpMultipartParser.MultipartFormDataParser(stream);
                var parser = new HttpMultipartParser.StreamingMultipartFormDataParser(stream);
                parser.ParameterHandler += parameter => { };
                // Write the part of the file we've recieved to a file stream. (Or do something else)

                filename = filename + "." + extension;
                LogHandler.WriteLog("Service.cs: UploadRedLinesAddiotionalFiles.  : Creating File " + filename, EventLogEntryType.Information);
                var write = File.OpenWrite("c:\\temp\\" + filename);


                //object comosobject = System.Web.HttpContext.Current.ApplicationInstance.Application["ComosAPI"];
                //IBRServiceContracts.IServiceContract m_ComosAPIService = (IBRServiceContracts.IServiceContract)comosobject;
                IServiceContract m_ComosAPIService = GetComosAPI();


                parser.FileHandler += (name, fileName, type, disposition, buffer, bytes) =>
                {
                    LogHandler.WriteLog("   Service.cs: UploadRedLinesAddiotionalFiles.  : Write block to File " + filename, EventLogEntryType.Information);
                    write.Write(buffer, 0, bytes);
                };
                parser.StreamClosedHandler += () =>
                {
                    LogHandler.WriteLog("   Service.cs: UploadRedLinesAddiotionalFiles.  :  File stream closed" + filename, EventLogEntryType.Information);
                    // Do things when my input stream is closed
                    write.Flush();
                    write.Close();
                    write.Dispose();

                    using (var reader = System.IO.File.OpenRead("c:\\temp\\" + filename))
                    {
                        RemoteFileInfo fileInfo = new RemoteFileInfo()
                        {
                            FileByteStream = reader,                            
                            Project = project,
                            User = user,
                            Owner = uid,
                            Workinglayer = layer,
                            FileName = filename,
                            Extension = extension,
                            Description = description,
                        };
                        m_ComosAPIService.CreateRedLinesAndAditionalDocuments(fileInfo);
                    }

                };
                parser.Run();

            }
            catch (Exception ex)
            {
                LogHandler.WriteLog("Service.cs: UploadRedLinesAddiotionalFiles.  : Exception = " + ex.Message, EventLogEntryType.Error);

                return new TResult<bool>()
                {
                    Status = false,
                    Message = ex.Message,
                    data = false
                };
            }
            //return null;
            return new TResult<bool>()
            {
                data = true,
                Status = true,
            };
        }
    }
}
