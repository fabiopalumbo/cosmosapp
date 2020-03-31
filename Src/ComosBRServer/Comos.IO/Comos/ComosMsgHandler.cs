using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using log4net;
using Plt;

namespace Comos.IO
{
	
	/// <summary>
	/// Lidar com Comos mensagens evitando diálogos pop-up.
	/// </summary>
	[ClassInterface(ClassInterfaceType.None)]
	[ComDefaultInterface(typeof(IMessageBoxHandler))]
	[ComVisible(true)]
	[Guid("6bf7cd16-1b2b-47d8-88d6-664e30d9bfea")]
	public class ComosMsgHandler : IMessageBoxHandler, _IMessageBoxHandler
	{
		private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		public short ShowMessageBox(string txt, string caption, short Mode)
		{
			log.Debug(txt);
			log.Debug(caption);
			log.Debug(Mode);
			return (short)0;
		}
	}
}
