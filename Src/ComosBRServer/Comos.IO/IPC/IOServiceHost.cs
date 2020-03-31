using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace BRComos.IO.IPC
{
    /// <summary>
    /// Host para comunicar com o processo externo.
    /// </summary>
    public class IOServiceHost
    {
        ServiceHost m_ServiceHost = null;
        /// <summary>
        /// Iniciar o host.
        /// </summary>
        /// <returns>True é o sucesso, false caso contrário.</returns>
        internal bool Start()
        {
            try
            {
                Log.WriteLog("Starting Service Host", System.Diagnostics.EventLogEntryType.Information);

                m_ServiceHost = new ServiceHost(typeof(IOServiceContract));
                m_ServiceHost.Faulted += ServiceHost_Faulted;
                int processid = System.Diagnostics.Process.GetCurrentProcess().Id;
                NetTcpBinding namedpipebinding = new NetTcpBinding();
                //NetNamedPipeBinding namedpipebinding = new NetNamedPipeBinding();
                // Permitir grande quantidade de dados a ser serializado através NamedPipe
                namedpipebinding.OpenTimeout = TimeSpan.FromHours(13);
                namedpipebinding.CloseTimeout = TimeSpan.FromHours(13);
                namedpipebinding.ReceiveTimeout = TimeSpan.FromHours(13);
                namedpipebinding.SendTimeout = TimeSpan.FromHours(13);
                namedpipebinding.MaxBufferPoolSize = 2147483647;
                namedpipebinding.MaxBufferSize = 2147483647;
                namedpipebinding.MaxReceivedMessageSize = 2147483647;

                //CloseComosIOs();

                //m_ServiceHost.AddServiceEndpoint(typeof(IBRServiceContracts.IServiceContract), namedpipebinding, @"net.tcp://127.0.0.1/comosio/" + processid.ToString());
                //m_ServiceHost.AddServiceEndpoint(typeof(IBRServiceContracts.IServiceContract), namedpipebinding, @"net.tcp://127.0.0.1:807/comosio/" + processid.ToString());

                m_ServiceHost.AddServiceEndpoint(typeof(IBRServiceContracts.IServiceContract), namedpipebinding, @"net.tcp://127.0.0.1:807/comosio/1");

                //m_ServiceHost.AddServiceEndpoint(typeof(IBRServiceContracts.IServiceContract), namedpipebinding, @"net.pipe://127.0.0.1/comosio/"+processid.ToString());

                // Permitir grande quantidade de objetos a ser serializado através NamedPipe
                m_ServiceHost.Description.Endpoints[0].EndpointBehaviors.Add(new IBRServiceContracts.ReaderQuotaExtension());

                m_ServiceHost.Open();
                Log.WriteLog("Service Host started", System.Diagnostics.EventLogEntryType.Information);

            }
            catch (Exception ex)
            {
                Log.WriteLog("Error: " + ex.Message + " : " + ex.StackTrace, System.Diagnostics.EventLogEntryType.Information);
                Console.WriteLine("Error: " + ex.Message);
                m_ServiceHost = null;
                CloseComosIOs();
                return false;
            }
            return true;
        }

        /// <summary>
        /// Detectar falhas e escreve informações.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ServiceHost_Faulted(object sender, EventArgs e)
        {
            try
            {
                Log.WriteLog("Service Host Faulted", System.Diagnostics.EventLogEntryType.Information);

                if (m_ServiceHost != null)
                {
                    m_ServiceHost.Close();
                    m_ServiceHost = null;
                }
            }
            catch (Exception)
            { }
            CloseComosIOs();
            System.Windows.Forms.Application.Exit();
            //killComosIOS();
        }

        /// <summary>
        /// Terminar o Host.
        /// </summary>
        internal void Stop()
        {
            Log.WriteLog("Service Host Stopped", System.Diagnostics.EventLogEntryType.Information);
            if (m_ServiceHost != null)
            {
                m_ServiceHost.Close();
                m_ServiceHost = null;
            }
            CloseComosIOs();
            System.Windows.Forms.Application.Exit();
        }

        internal void CloseComosIOs()
        {
            int curID = System.Diagnostics.Process.GetCurrentProcess().Id;
            System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcessesByName("Comos.IO");

            foreach (var item in processes)
            {
                if (item.Id != curID)
                {
                    Log.WriteLog("Closing existing COMOSIO ProcessID:" + item.Id.ToString());
                    item.WaitForExit(3000);
                    if (!item.HasExited)
                        item.Kill();
                }
                    
            }
        }
    }
}

