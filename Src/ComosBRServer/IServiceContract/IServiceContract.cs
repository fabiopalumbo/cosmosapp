using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace IBRServiceContracts
{
    /// <summary>
    /// Interface de comunicar entre COMOS Web BR e COMOS IO
    /// </summary>
    [ServiceContract]
    public interface IServiceContract
    {
        #region Old Interface
        /// <summary>
        /// Inicializar COMOS para usar banco de dados definido pela conectionstring.
        /// !!! Este methodo se comporta static
        /// </summary>
        /// <param name="connectionstring"></param>
        /// <returns>True é o sucesso, false caso contrário.</returns>
        [OperationContract]
        bool Open(string connectionstring);

        /// <summary>
        /// Verifice se COMOS usar banco de dados definido.
        /// !!! Este methodo se comporta static.
        /// </summary>
        /// <param name="connectionstring"></param>
        /// <returns>True é o sucesso, false caso contrário.</returns>
        [OperationContract]
        bool IsOpen();

        /// <summary>
        /// Fechar sessão COMOS.
        /// !!! Este methodo se comporta static.
        /// </summary>
        [OperationContract]
        bool Close();

        /// <summary>
        /// Obter todos as projetos e camadas disponíveis para o usuário.
        /// </summary>
        /// <param name="user">Usario registro no COMOS.</param>
        /// <returns>Informação de usario ou nada.</returns>
        [OperationContract]
        CUserInfo ValidateUser(string user);


        /// <summary>
        /// Obter todos as projetos e camadas disponíveis para o usuário.
        /// </summary>
        /// <param name="user">Usario registro no COMOS.</param>
        /// <returns>Todos projetos e Camadas.</returns>
        [OperationContract]
        List<CProject> GetProjectsAndLayers(string user_input);

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
        TResult<CQueryResult> ExecuteQuery(string user, string projectname, int workinglayer, string query_fullname, string startobject_fullname);

        [OperationContract]
        string GetObjectDetails(string user, string projectname, int workinglayer, string object_sysuid);

        [OperationContract]
        TResult<CQueryResult> BuscaString(string stringbuscar, string projectname, int workinglayer);

        [OperationContract]
        String DownloadFile(string uidDocument, string projectname, int workinglayer, bool revision_file);

        [OperationContract]
        TResult<CQueryResult> ExecuteTaskQuery(string user, string projectname, int workinglayer, string opt, string startobject);

        [OperationContract]
        ComosActivity GetComosActivity(string user, string projectname, int workinglayer, string object_sysuid);

        [OperationContract]
        bool UpdateComosValues(IBRServiceContracts.CCell[] specs,string user, string projectname, int workinglayer);

        [OperationContract]
        bool BatchUpdateComosValues(IBRServiceContracts.CRow[] specs, string user, string projectname, int workinglayer);

        [OperationContract]
        bool CreateComosDevice(IBRServiceContracts.CCell[] specs, string user, string projectname, int idworkinglayer, string ownersystemfullname, string cdevsystemfullname);

        [OperationContract]
        bool CreateComosDeviceBySystemUID(IBRServiceContracts.CCell[] specs, string user, string projectname, int idworkinglayer, string ownersystemuid, string cdevsystemfullname);

        [OperationContract]
        String ClickBotaoUid(string user, string systemuid, string nestedname, string project_name, int working_layer);

        [OperationContract]
        CCell[] GetTaskDataByUid(string user, string project_name, int working_layer, string systemuid);

        [OperationContract]
        TResult<bool> AddImageToComosObject(byte[] fileStream, string user, string projectname, int workinglayer, string coSystemFullName, string docname);

        [OperationContract]
      
        TResult<bool> ForceCrash(string ctype);

        #endregion

        #region Version 1 interface 

        [OperationContract]
        TResult<CWriteValueResult[]> WriteComosValues(CWriteValueCollection values, string user, string projectname, string workinglayer, string language);

        [OperationContract]
        TResult<CQueryResult> SearchDevicesByNameAndDescription(string projectname, string workinglayer, string language,string tosearch, string filter = "");

        [OperationContract]
        TResult<CQueryResult> SearchDocumentsByNameAndDescription(string projectname, string workinglayer, string language, string tosearch);

        [OperationContract]
        TResult<CQueryResult> SearchAllByNameAndDescription(string projectname, string workinglayer, string language, string tosearch);

        [OperationContract]
        TResult<string> CreateComosDeviceByWebUID(CWriteValueCollection values, string projectname, string workinglayer, string language, string owner, string cdev, string user,string desc);

        [OperationContract]
        void UploadFileToComosDevice(RemoteFileInfo fileinfo);

        [OperationContract]
        void UploadFileToComosDevice2(RemoteFileInfo fileinfo);

        [OperationContract]
        void CreateRedLinesAndAditionalDocuments(RemoteFileInfo fileinfo);

        [OperationContract]
        bool IsAlive();

        #endregion
    }

    [MessageContract]
    public class RemoteFileInfo : IDisposable
    {        
        [MessageHeader(MustUnderstand = true)]
        public string Project;

        [MessageHeader(MustUnderstand = true)]
        public string Workinglayer;

        [MessageHeader(MustUnderstand = true)]
        public string Owner;

        [MessageHeader(MustUnderstand = true)]
        public string FileName;

        [MessageHeader(MustUnderstand = true)]
        public string FullFileName;

        [MessageHeader(MustUnderstand = true)]
        public string Description;

        [MessageHeader(MustUnderstand = true)]
        public string Extension;

        [MessageHeader(MustUnderstand = true)]
        public string User;


        [MessageBodyMember(Order = 1)]
        public System.IO.Stream FileByteStream;
        public void Dispose()
        {
            if (FileByteStream != null)
            {
                FileByteStream.Close();
                FileByteStream = null;
            }
        }
    }
}
