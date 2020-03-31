using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Net;

namespace ComosBRWeb
{
    [ServiceContract]
    public interface IWebService
    {
        #region Old Interface

        /// <summary>
        /// Obter todos as projetos e camadas disponíveis para o usuário.
        /// </summary>
        /// <param name="user">Usario registro no COMOS.</param>
        /// <returns>Todos projetos e Camadas.</returns>
        [OperationContract]
        [WebGet(UriTemplate = "ValidateUser/{user_input}/{user_password}", ResponseFormat = WebMessageFormat.Json)]
        IBRServiceContracts.TResult<IBRServiceContracts.CUserInfo> ValidateUser(string user_input, string user_password);

        /// <summary>
        /// Obter todos as projetos e camadas disponíveis para o usuário.
        /// </summary>
        /// <param name="user">Usario registro no COMOS.</param>
        /// <returns>Todos projetos e Camadas.</returns>
        [OperationContract]
        [WebGet(UriTemplate = "GetProjectsAndLayers/{user_input}", ResponseFormat = WebMessageFormat.Json)]
        IBRServiceContracts.TResult<List<IBRServiceContracts.CProject>> GetProjectsAndLayers(string user_input);

        /// <summary>
        /// Run a qeury and return the results as a CQueryResult.
        /// </summary>
        /// <param name="user">Usario registro no COMOS.</param>
        /// <param name="projectname">Nome da projeto</param>
        /// <param name="workinglayer">ID da camada</param>
        /// <param name="query_fullname">System Full Name da Query object.</param>
        /// <param name="startobject_fullname"></param>
        /// <returns></returns>
        [OperationContract]
        [WebGet(UriTemplate = "ExecuteQuery/{user}/{projectname}/{workinglayer}/{query_fullname}/{startobject_fullname}", ResponseFormat = WebMessageFormat.Json)]
        IBRServiceContracts.TResult<IBRServiceContracts.CQueryResult> ExecuteQuery(string user, string projectname, string workinglayer, string query_fullname, string startobject_fullname);

        [OperationContract]
        [WebGet(UriTemplate = "ExecuteLinkedQuery/{user}/{projectname}/{workinglayer}/{spec}/{startobject}", ResponseFormat = WebMessageFormat.Json)]
        IBRServiceContracts.TResult<IBRServiceContracts.CQueryResult> ExecuteLinkedQuery(string user, string projectname, string workinglayer, string spec, string startobject);


        /// <summary>
        /// Run a qeury and return the results as a CQueryResult.
        /// </summary>
        /// <param name="user">Usario registro no COMOS.</param>
        /// <param name="projectname">Nome da projeto</param>
        /// <param name="workinglayer">ID da camada</param>
        /// <param name="query_fullname">System Full Name da Query object.</param>
        /// <param name="startobject_fullname"></param>
        /// <returns></returns>
        [OperationContract]
        [WebGet(UriTemplate = "GetObjectDetails/{user}/{projectname}/{workinglayer}/{object_sysuid}", ResponseFormat = WebMessageFormat.Json)]
        IBRServiceContracts.TResult<string> GetObjectDetails(string user, string projectname, string workinglayer, string object_sysuid);

        [OperationContract]
        [WebGet(UriTemplate = "GetTaskDataByUid/{user}/{projectname}/{workinglayer}/{object_sysuid}", ResponseFormat = WebMessageFormat.Json)]
        IBRServiceContracts.TResult<IBRServiceContracts.CRow> GetTaskDataByUid(string user, string projectname, string workinglayer, string object_sysuid);


        [OperationContract]
        [WebGet(UriTemplate = "ClickBotaoUid/{user}/{project_name}/{working_layer}/{systemuid}/{nestedname}", ResponseFormat = WebMessageFormat.Json)]
        string ClickBotaoUid(string user, string systemuid, string nestedname, string project_name, string working_layer);

        [OperationContract]
        [WebGet(UriTemplate = "Busca/{user_input}/{project_name}/{working_layer}", ResponseFormat = WebMessageFormat.Json)]
        IBRServiceContracts.TResult<IBRServiceContracts.CQueryResult> BuscaString(string user_input, string project_name, string working_layer);

        [OperationContract]
        [WebGet(UriTemplate = "Download/{request}/{project_name}/{working_layer}/{revision_file}", ResponseFormat = WebMessageFormat.Json)]
        System.IO.Stream DownloadFile(string request, string project_name, string working_layer, string revision_file);


        [OperationContract]
        [WebGet(UriTemplate = "GetComosActivity/{user}/{projectname}/{workinglayer}/{object_sysuid}", ResponseFormat = WebMessageFormat.Json)]
        IBRServiceContracts.TResult<IBRServiceContracts.ComosActivity> GetComosActivity(string user, string projectname, string workinglayer, string object_sysuid);


        [OperationContract]
        [WebInvoke(UriTemplate = "/ChangeComosValues/{user}/{projectname}/{workinglayer}/{systemfullname}",
           Method = "POST",
           RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json)]
        IBRServiceContracts.TResult<bool> ChangeComosValues(IBRServiceContracts.CCell[] specs, string user, string projectname, string workinglayer, string systemfullname);

        [OperationContract]
        [WebInvoke(UriTemplate = "/BatchChangeComosValues/{user}/{projectname}/{workinglayer}",
          Method = "POST",
          RequestFormat = WebMessageFormat.Json,
          ResponseFormat = WebMessageFormat.Json)]
        IBRServiceContracts.TResult<bool> BatchChangeComosValues(IBRServiceContracts.CRow[] specs, string user, string projectname, string workinglayer);

        [OperationContract]
        [WebInvoke(UriTemplate = "/CreateComosDevice/{user}/{projectname}/{workinglayer}/{owner}/{cdevsystemfullname}",
        Method = "POST",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json)]
        IBRServiceContracts.TResult<bool> CreateComosDevice(IBRServiceContracts.CCell[] specs, string user, string projectname, string workinglayer, string owner, string cdevsystemfullname);

        [OperationContract]
        [WebInvoke(UriTemplate = "/CreateComosDeviceBySystemUID/{user}/{projectname}/{workinglayer}/{ownersystemuid}/{cdevsystemfullname}",
        Method = "POST",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json)]
        IBRServiceContracts.TResult<bool> CreateComosDeviceBySystemUID(IBRServiceContracts.CCell[] specs, string user, string projectname, string workinglayer, string ownersystemuid, string cdevsystemfullname);


        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/AddImageToComosObject/{user}/{projectname}/{workinglayer}/{coSystemFullName}/{docname}",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json)]
        IBRServiceContracts.TResult<bool> AddImageToComosObject(System.IO.Stream fileStream, string user, string projectname, string workinglayer, string coSystemFullName, string docname);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/Crash/{ctype}",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json)]
        IBRServiceContracts.TResult<bool> ForceCrash(string ctype);

        #endregion


        #region Interface Version 1 - Voltalia

        [OperationContract]
        [WebInvoke(UriTemplate = "/v1/WriteComosValues/{user}/{projectname}/{workinglayer}/{language}",
           Method = "POST",
           RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json)]
        IBRServiceContracts.TResult<IBRServiceContracts.CWriteValueResult[]> WriteComosValues(
            IBRServiceContracts.CWriteValueCollection values,
            string user,
            string projectname,
            string workinglayer,
            string language);

        [OperationContract]
        [WebInvoke(UriTemplate = "/v1/WriteComosValuesTest/{user}/{projectname}/{workinglayer}/{language}",
           Method = "GET",
           ResponseFormat = WebMessageFormat.Json)]
        IBRServiceContracts.TResult<IBRServiceContracts.CWriteValueResult[]> WriteComosValuesTest(
            string user,
            string projectname,
            string workinglayer,
            string language);

        [OperationContract]
        [WebInvoke(UriTemplate = "/v1/WriteComosValuesTest2/{user}/{projectname}/{workinglayer}/{language}",
           Method = "POST",
           RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json)]
                IBRServiceContracts.TResult<IBRServiceContracts.CWriteValueResult[]> WriteComosValuesTest2(
            IBRServiceContracts.CWriteValueCollection values,
            string user,
            string projectname,
            string workinglayer,
            string language);

        [OperationContract]
        [WebInvoke(UriTemplate = "/v1/SearchDevicesByNameAndDescription/{project}/{layer}/{language}/{tosearch}",
        Method = "GET",
        ResponseFormat = WebMessageFormat.Json)]
        IBRServiceContracts.TResult<IBRServiceContracts.CQueryResult> SearchDevicesByNameAndDescription(
         string project,
         string layer,
         string language,
         string tosearch);

        [OperationContract]
        [WebInvoke(UriTemplate = "/v1/SearchDevicesByNameAndDescriptionWithFilter/{project}/{layer}/{language}/{tosearch}/{filter}",
      Method = "GET",
      ResponseFormat = WebMessageFormat.Json)]
        IBRServiceContracts.TResult<IBRServiceContracts.CQueryResult> SearchDevicesByNameAndDescriptionWithFilter(
       string project,
       string layer,
       string language,
       string tosearch,
       string filter);

        [OperationContract]
        [WebInvoke(UriTemplate = "/v1/SearchDocumentsByNameAndDescription/{project}/{layer}/{language}/{tosearch}",
         Method = "GET",
         ResponseFormat = WebMessageFormat.Json)]
                IBRServiceContracts.TResult<IBRServiceContracts.CQueryResult> SearchDocumentsByNameAndDescription(
          string project,
          string layer,
          string language,
          string tosearch);

        [OperationContract]
        [WebInvoke(UriTemplate = "/v1/SearchAllByNameAndDescription/{project}/{layer}/{language}/{tosearch}",
               Method = "GET",
               ResponseFormat = WebMessageFormat.Json)]
        IBRServiceContracts.TResult<IBRServiceContracts.CQueryResult> SearchAllByNameAndDescription(
                string project,
                string layer,
                string language,
                string tosearch);

        [OperationContract]
        [WebInvoke(UriTemplate = "/v1/CreateComosDeviceByWebUID/{project}/{layer}/{language}/{owner}/{cdev}/{user}/{desc}",
       Method = "POST",
       RequestFormat = WebMessageFormat.Json,
       ResponseFormat = WebMessageFormat.Json)]
        IBRServiceContracts.TResult<string> CreateComosDeviceByWebUID(
            IBRServiceContracts.CWriteValueCollection values,
            string project, 
            string layer,
            string language, 
            string owner,
            string cdev,
            string user,
            string desc
            );

        [OperationContract]
        [WebGet(UriTemplate = "/Pages/{filename}", BodyStyle = WebMessageBodyStyle.Bare)]
        System.IO.Stream Pages(string filename);

        [OperationContract]
        //[WebInvoke(UriTemplate = "/UploadFile/{filename}/{user}/{projectname}/{workinglayer}/{language}",
        [WebInvoke(UriTemplate = "/UploadFile/{filename}",
            Method = "POST",
            ResponseFormat = WebMessageFormat.Json)]
        IBRServiceContracts.TResult<bool> UploadFile(string filename, System.IO.Stream reader);
        #endregion

        [OperationContract]
        //[WebInvoke(UriTemplate = "/UploadFile/{filename}/{user}/{projectname}/{workinglayer}/{language}",
        [WebInvoke(UriTemplate = "/UploadFile2/{filename}",
           Method = "POST",
           ResponseFormat = WebMessageFormat.Json)]
        IBRServiceContracts.TResult<bool> UploadFile2(string filename, System.IO.Stream reader);

        [OperationContract]
        //[WebInvoke(UriTemplate = "/UploadFile/{filename}/{user}/{projectname}/{workinglayer}/{language}",
        [WebInvoke(UriTemplate = "/UploadFile3/{filename}",
           Method = "POST",
           ResponseFormat = WebMessageFormat.Json)]
        IBRServiceContracts.TResult<bool> UploadFile3(string filename, System.IO.Stream reader);

        [OperationContract]        
        [WebInvoke(UriTemplate = "/UploadRedLinesAddiotionalFiles/{filename}",
          Method = "POST",
          ResponseFormat = WebMessageFormat.Json)]
        IBRServiceContracts.TResult<bool> UploadRedLinesAddiotionalFiles(string filename, System.IO.Stream reader);

    }



}

