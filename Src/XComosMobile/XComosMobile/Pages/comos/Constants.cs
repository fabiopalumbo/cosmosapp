using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XComosMobile.Pages.comos
{
	public class Constants
	{
		#region UID's
		public const string IncidentCDevUID = "U:13:A3KE1WDCID:I";
		public const string IncidentCDevFalloUID = "U:13:A4A8TRC02J:I";
		public const string IncidentCDevRojoUID = "U:13:A4A88FR9BH:I";
		#endregion

		#region Queries
		public const string QueryTasksSystemFullName = "A10|A40|A10";
		public const string QueryIncidentTypesFullName = "A10|A40|A40";
		public const string QueryEventContainerFullName = "A10|A40|A50";
		public const string QueryInspectionsSystemFullName = "A10|A40|A30";
		public const string QueryWorkPackagesSystemFullName = "A10|A40|A20";
		public const string QueryWorkPackagesAnFailureSystemFullName = "A10|A40|A60";
		public const string QueryLogBookSystemFullName = "A10|A40|A70";
		public const string QueryLogBookTypesFullName = "A10|A40|A80";
		#endregion

		#region Mobile Tab & Icons
		public const string MobileTabName = "Z10T00002";

		public const string IncidentIcon = "\uf071";
		public const string PlusIcon = "\uf067";
		public const string CancelIcon = "\uf00d";
		public const string QRIcon = "\uf029";
		public const string RightArrowIcon = "\uf105";
		public const string OkIcon = "\uf00c";

		public const string WP_OTCode_Description = "Numero de OT";
		#endregion

		#region Project Data
		public const string defaultDB = "DBName1";
		public const string defaultProject = "STO";
		public const string defaultWorkingLayer = "PruebaCarga8Niveles|Partes|OTs|interfaz|STO";
		#endregion

		#region Pañoles
		public static readonly string[] panolSTO = { "1001400", "1001430" };
		public static readonly string[] panolCYA = { "0201400" };
		public static readonly string[] panolARR = { "9906400", "9906401", "9906430" };

		public static Dictionary<string, List<string>> panolDictionary = new Dictionary<string, List<string>>()
		{
			["STO"] = new List<string>(panolSTO),
			["CYA"] = new List<string>(panolCYA),
			["ACD"] = new List<string>(panolARR)
		};
		#endregion

		#region External Links & Endpoints
			#if PROD_TABLET
				public const string URL_FindMaterials = "https://industria40prod.arcorgroup.com/services/I4.0/comos/";
				public const string Endpoints_FindMaterials = "searchItems";

				public const string URL_StockInformation = "https://industria40prod.arcorgroup.com/services/I4.0/comos/";
				public const string Endpoints_StockInformation = "checkstock";

				public const string URL_MaterialRequest = "https://industria40prod.arcorgroup.com/services/I4.0/comos/";
				public const string Endpoints_MaterialRequest = "ordermaterials";

				public const string URL_VerifyExistence = "https://cldsrv2020.arcorgroup.com:5000/";
				public const string Endpoints_ExistenceVerification = "/query_asset";
			
			#elif PROD_CELULAR
				public const string URL_FindMaterials = "https://comos.webaccess.arcor.com:8443/services/I4.0/comos/";
				public const string Endpoints_FindMaterials = "searchItems";

				public const string URL_StockInformation = "https://comos.webaccess.arcor.com:8443/services/I4.0/comos/";
				public const string Endpoints_StockInformation = "checkstock";

				public const string URL_MaterialRequest = "https://comos.webaccess.arcor.com:8443/services/I4.0/comos/";
				public const string Endpoints_MaterialRequest = "ordermaterials";

				public const string URL_VerifyExistence = "https://cldsrv2020.arcorgroup.com:5000/";
				public const string Endpoints_ExistenceVerification = "/query_asset";
			
			#elif TEST_TABLET
				public const string URL_FindMaterials = "https://industria40test.arcorgroup.com/services/I4.0/comos/";
				public const string Endpoints_FindMaterials = "searchItems";

				public const string URL_StockInformation = "https://industria40test.arcorgroup.com/services/I4.0/comos/";
				public const string Endpoints_StockInformation = "checkstock";

				public const string URL_MaterialRequest = "https://industria40test.arcorgroup.com/services/I4.0/comos/";
				public const string Endpoints_MaterialRequest = "ordermaterials";

				public const string URL_VerifyExistence = "https://cldsrv2020.arcorgroup.com:5000/";
				public const string Endpoints_ExistenceVerification = "/query_asset";
			#else
				public const string URL_FindMaterials = "http://demo3249751.mockable.io/";
				public const string Endpoints_FindMaterials = "/find/";

				public const string URL_StockInformation = "http://demo3249751.mockable.io/";
				public const string Endpoints_StockInformation = "/material/";

				public const string URL_MaterialRequest = "http://demo3249751.mockable.io/";
				public const string Endpoints_MaterialRequest = "";

				public const string URL_VerifyExistence = "http://demo3249751.mockable.io/";
				public const string Endpoints_ExistenceVerification = "/query_asset";
#endif
		#endregion

        #region Server Links
            #if PROD_TABLET
				public const string webServiceAddress = "https://cldsrv1056.arcorgroup.com:8444/ARMobile";
				public const string serverAddress = "https://cldsrv1056.arcorgroup.com:8443";
				public const string userDomain = "arcorgroup";
            #elif PROD_CELULAR
				public const string webServiceAddress = "https://comos.webaccess.arcor.com:8444/ARMobile";
				public const string serverAddress = "https://comos.webaccess.arcor.com:8443";
				public const string userDomain = "arcorgroup";
            #elif TEST_TABLET
				public const string webServiceAddress = "https://comostst.webaccess.arcor.com:8444/ARMobile";
				public const string serverAddress = "https://comostst.webaccess.arcor.com:8443";
				public const string userDomain = "arcorgroup";
			#else
				// Publico 
				public const string webServiceAddress = "http://comossrv1024.digi2g.com.ar/BRMobile";
				public const string serverAddress = "http://comosweb1024.digi2g.com.ar";
				public const string userDomain = "i2g0";
				// COMOS.IO
				/*public const string webServiceAddress = "http://192.168.0.63:8080/Service.svc";
				public const string serverAddress = "http://192.168.0.63";
				public const string userDomain = "i2g0";*/

				// COMOS.SERVER
				/*public const string webServiceAddress = "http://192.168.0.63:7886/api";
				public const string serverAddress = "http://192.168.0.63";
				public const string userDomain = "i2g0";*/
			#endif
		#endregion

	}
}
