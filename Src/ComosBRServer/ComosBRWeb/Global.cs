using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Web;

namespace ComosBRWeb
{
    public class Global : System.Web.HttpApplication
    {
        public Global()
        {
        }

        IBRServiceContracts.IServiceContract m_ComosAPIService;
        System.Diagnostics.Process m_Process;


        private void OpenConnString()
        {
            string fullname = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            string connectionstring = System.Configuration.ConfigurationManager.AppSettings.Get("ComosConnectionString");

            if (System.Configuration.ConfigurationManager.AppSettings.Get("ComosConnectionString").ToUpper().Contains(".MDB"))            
                connectionstring = fullname + connectionstring;            

            bool success = false;
            try
            {
                LogHandler.WriteLog("Comos.Web APIService opening connectionstring : " + connectionstring, System.Diagnostics.EventLogEntryType.Information);
                success = m_ComosAPIService.Open(connectionstring);
                LogHandler.WriteLog("Comos.Web APIService opened connectionstring : " + success.ToString(), System.Diagnostics.EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                LogHandler.WriteLog("Failed opening comos.io connection at startup::" + ex.Message);
            }
            if (success)
            {
                LogHandler.WriteLog("Comos.Web Database Opened with success", System.Diagnostics.EventLogEntryType.Information);
                this.Application["ComosAPI"] = m_ComosAPIService;
                ((IClientChannel)m_ComosAPIService).Faulted -= ComosIO_FaultedOnStartup;
                ((IClientChannel)m_ComosAPIService).Faulted += ComosIO_FaultedDuringRequest;
            }


        }
        private bool TryeRecoverProcess()
        {

            //NetNamedPipeBinding binding = new NetNamedPipeBinding();
            NetTcpBinding binding = new NetTcpBinding();

            // Permitir grande quantidade de dados a ser serializado através NamedPipe
            binding.OpenTimeout = TimeSpan.FromHours(12);
            binding.CloseTimeout = TimeSpan.FromHours(12);
            binding.ReceiveTimeout = TimeSpan.FromHours(12);
            binding.SendTimeout = TimeSpan.FromHours(12);
            binding.MaxBufferPoolSize = 2147483647;
            binding.MaxBufferSize = 2147483647;
            binding.MaxReceivedMessageSize = 2147483647;

            //ChannelFactory<IBRServiceContracts.IServiceContract> m_ChannelFactory = new ChannelFactory<IBRServiceContracts.IServiceContract>(binding, new EndpointAddress(@"net.pipe://127.0.0.1/comosio/" + m_Process.Id.ToString()));
            ChannelFactory<IBRServiceContracts.IServiceContract> m_ChannelFactory = new ChannelFactory<IBRServiceContracts.IServiceContract>(binding, new EndpointAddress(@"net.tcp://127.0.0.1:807/comosio/1"));

            // Permitir grande quantidade de objetos a ser serializado através NamedPipe
            m_ChannelFactory.Endpoint.EndpointBehaviors.Add(new IBRServiceContracts.ReaderQuotaExtension());

            m_ComosAPIService = m_ChannelFactory.CreateChannel();
            

            try
            {
                if (m_ComosAPIService.IsAlive())
                {
                    ((IClientChannel)m_ComosAPIService).OperationTimeout = TimeSpan.FromHours(12);
                    this.Application["ComosAPI"] = m_ComosAPIService;
                    return true;
                }
                else
                {                    
                    return false;
                }
                    
            }
            catch (Exception)
            {                
                return false;
            }

        }

        protected void Application_Start()
        {        
          
            Application.Lock();

            string fullname = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;

            try
            {

               string path = fullname + System.Configuration.ConfigurationManager.AppSettings.Get("ComosBinFolder");
                if (!System.IO.Directory.Exists(path + "/../Log/"))
                {
                    System.IO.Directory.CreateDirectory(path + "/../Log/");
                }

                LogHandler.logfile = path + "/../Log/" + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".ComosBRWeb.log";
                LogHandler.WriteLog("Application_Start!", System.Diagnostics.EventLogEntryType.Error);



                if (TryeRecoverProcess())
                {

                    if(!m_ComosAPIService.IsOpen())
                        OpenConnString();

                    Application.UnLock();
                    LogHandler.WriteLog("Comos.IO.Exe was recovered!!");
                    return;
                }
                else
                {
                    LogHandler.WriteLog("Comos.IO.Exe could not be recovered!! Will start new!");
                    CloseComosIOs();
                }                

                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.CreateNoWindow = true;
                startInfo.UseShellExecute = false;
                startInfo.FileName = path + "\\Comos.IO.exe";

                LogHandler.WriteLog("Comos.IO.Exe.Path::" + startInfo.FileName);

                startInfo.WorkingDirectory = path;
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                m_Process = System.Diagnostics.Process.Start(startInfo);
                m_Process.WaitForInputIdle();

                if (m_Process.HasExited)
                {
                    LogHandler.WriteLog("Comos.Web APIService Exited before it could be opening a database!", System.Diagnostics.EventLogEntryType.Error);
                    return;
                }

            }
            catch (Exception ex)
            {

                LogHandler.WriteLog("Error at:" + ex.Message, System.Diagnostics.EventLogEntryType.Error);
                throw;
            }


            //NetNamedPipeBinding binding = new NetNamedPipeBinding();
            NetTcpBinding binding = new NetTcpBinding();

            // Permitir grande quantidade de dados a ser serializado através NamedPipe
            binding.OpenTimeout = TimeSpan.FromHours(12);
            binding.CloseTimeout = TimeSpan.FromHours(12);
            binding.ReceiveTimeout = TimeSpan.FromHours(12);
            binding.SendTimeout = TimeSpan.FromHours(12);
            binding.MaxBufferPoolSize = 2147483647;
            binding.MaxBufferSize = 2147483647;
            binding.MaxReceivedMessageSize = 2147483647;

             //ChannelFactory<IBRServiceContracts.IServiceContract> m_ChannelFactory = new ChannelFactory<IBRServiceContracts.IServiceContract>(binding, new EndpointAddress(@"net.pipe://127.0.0.1/comosio/" + m_Process.Id.ToString()));
            ChannelFactory<IBRServiceContracts.IServiceContract> m_ChannelFactory = new ChannelFactory<IBRServiceContracts.IServiceContract>(binding, new EndpointAddress(@"net.tcp://127.0.0.1:807/comosio/1"));

            // Permitir grande quantidade de objetos a ser serializado através NamedPipe
            m_ChannelFactory.Endpoint.EndpointBehaviors.Add(new IBRServiceContracts.ReaderQuotaExtension());

            m_ComosAPIService = m_ChannelFactory.CreateChannel();
            ((IClientChannel)m_ComosAPIService).OperationTimeout = TimeSpan.FromHours(12);
            ((IClientChannel)m_ComosAPIService).Faulted += ComosIO_FaultedOnStartup;
            ((IClientChannel)m_ComosAPIService).Opened += ComosIO_HasBeenOpened;

            // @ToDo: ComosConnectionString, Deve ser um local para banco de dados access em relação ao pasts do site.
            // Isso deve ser alterado assim que o usuário pode colocar o nome de arquivo completo, parente ou uma nome de conexão SQL.

            string connectionstring = System.Configuration.ConfigurationManager.AppSettings.Get("ComosConnectionString");

            if (System.Configuration.ConfigurationManager.AppSettings.Get("ComosConnectionString").ToUpper().Contains(".MDB")){
                connectionstring = fullname + connectionstring;
            }

            bool success = false;
            try
            {
                LogHandler.WriteLog("Comos.Web APIService opening connectionstring : " + connectionstring, System.Diagnostics.EventLogEntryType.Information);
                 success = m_ComosAPIService.Open(connectionstring);
                LogHandler.WriteLog("Comos.Web APIService opened connectionstring : " + success.ToString(), System.Diagnostics.EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                LogHandler.WriteLog("Failed opening comos.io connection at startup");
            }
            if (success)
            {
                LogHandler.WriteLog("Comos.Web Database Opened with success", System.Diagnostics.EventLogEntryType.Information);
                this.Application["ComosAPI"] = m_ComosAPIService;
                ((IClientChannel)m_ComosAPIService).Faulted -= ComosIO_FaultedOnStartup;
                ((IClientChannel)m_ComosAPIService).Faulted += ComosIO_FaultedDuringRequest;
            }
            else
            {
                LogHandler.WriteLog("Comos.Web APIService failed to open : " + connectionstring, System.Diagnostics.EventLogEntryType.Error);
                if (!m_Process.HasExited)
                {
                    m_Process.Kill();
                    m_Process.Dispose();
                }
            }
            //((IClientChannel)m_ComosAPIService).Opened -= ComosAPIService_Opened;
            Application.UnLock();
        }

        protected void Application_End()
        {
            Application.Lock();

            LogHandler.WriteLog("Application Ending");
            if (m_ComosAPIService != null)
            {
                try
                {
                    ((IClientChannel)m_ComosAPIService).Faulted -= ComosIO_FaultedDuringRequest;
                    LogHandler.WriteLog("Closing APIService");
                    m_ComosAPIService.Close();
                }
                catch(Exception ex) {
                    LogHandler.WriteLog("\tException: " + ex.Message);
                }
                ((IClientChannel)m_ComosAPIService).Close();
                m_Process.WaitForExit(2000);
            }
            else
            {
                LogHandler.WriteLog("\tAPIService = null");
            }
        

            if (m_Process != null)
            {
                if (!m_Process.HasExited)
                {
                    LogHandler.WriteLog("\tKilling IO Process");
                    m_Process.Kill();
                }
            }
            else
                LogHandler.WriteLog("\tm_Process = null");

            Application.UnLock();
        }

        private void ComosIO_HasBeenOpened(object sender, EventArgs e)
        {

        }

        /// This will be called when there was a crash on COMOS IO when starting up the process.
        private void ComosIO_FaultedOnStartup(object sender, EventArgs e)
        {
            LogHandler.WriteLog("APIService Faulted! <Startup>", System.Diagnostics.EventLogEntryType.Error);
        }

        /// This will be called when there was a crash on COMOS IO when the process was handlign a request.
        /// !!!!! This should never happen.
        private void ComosIO_FaultedDuringRequest(object sender, EventArgs e)
        {
            var api = m_ComosAPIService;
            m_ComosAPIService = null;
            this.Application["ComosAPI"] = null;
            LogHandler.WriteLog("APIService Faulted! <Request>", System.Diagnostics.EventLogEntryType.Error);
            try
            {
                ((IClientChannel)api).Close(TimeSpan.FromSeconds(10));
            }
            catch (Exception)
            {
            }
            if (!m_Process.HasExited)
            {
                m_Process.Kill();
            }
            m_Process.Close();
            m_Process.Dispose();
            m_Process = null;
            LogHandler.WriteLog("Start new Comos.IO");
            Application_Start(); // Launch new Comos.IO in case of failure.
        }

        internal void CloseComosIOs()
        {
            int curID = System.Diagnostics.Process.GetCurrentProcess().Id;
            System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcessesByName("Comos.IO");

            foreach (var item in processes)
            {
                if (item.Id != curID)
                {
                    LogHandler.WriteLog("Closing existing COMOSIO ProcessID:" + item.Id.ToString());
                    item.WaitForExit(3000);
                    if (!item.HasExited)
                        item.Kill();
                }

            }
        }
    }
}
