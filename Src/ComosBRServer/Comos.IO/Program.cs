using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BRComos.IO
{
	static class Program
	{
		private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		
		/// <summary>
		/// Application main
		/// </summary>
		[STAThread]
		static int Main(string[] args)
		{
			try
			{
				log.Info("Starting COMO IO... ");

				AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

				IPC.IOServiceHost host = new IPC.IOServiceHost();
				log.Info("Service Host Starting");

				host.Start();

				log.Info("Application Running");
				
				Application.Run();

				log.Info("Service Host Stopping");
				host.Stop();

			}
			catch (Exception ex)
			{
				log.Error("Comos IO exception in Main", ex);
				throw;
			}

			return 0;
		}

		private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			Exception exception = (Exception)e.ExceptionObject;
			log.Error("Unhandled Error - ", exception);
		}
	}
}
