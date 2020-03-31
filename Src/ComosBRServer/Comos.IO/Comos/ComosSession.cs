using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using IBRServiceContracts;
using Plt;
using Chemserv;
using Comos.Global;
using System.IO;
using System.Runtime.InteropServices;
using BRComos.IO.Utilities;
using Comos.IO;
using log4net;

namespace Comos.IO
{
	/// <summary>
	/// Implementação de operações e recuperação da informação do COMOS.
	/// Não utilizam static si em futuras versões caso COMOS permitir que vários worksets em mesmo processo.
	/// </summary>
	[ComVisible(true)]
	public class ComosSession
	{
		private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		ComosClass m_ChemServer = null;
		CommonClass m_CommonClass = null;

		private void HelperAddItem(string key, object elm)
		{
			dynamic globals = Comos.Global.AppGlobal.Workset.Globals();
			typeof(object).InvokeMember(key, System.Reflection.BindingFlags.SetProperty, (System.Reflection.Binder)null, globals, new object[1]
				{
								elm
				});
		}

		public void ReleaseCOMOSObjects()
		{
			try
			{
				AppGlobal.Workset.ReleaseAllObjects();
			}
			catch (Exception ex)
			{
				log.Error("Error releasing commos objects.", ex);
			}
		}

		/// <summary>
		/// Inicializar COMOS para usar banco de dados definido pela conectionstring.
		/// </summary>
		/// <param name="connectionstring">Mesmo valor que COMOS "Object Debuger" retornos resultado de "a.Workset.InitString"</param>
		/// <returns>True é o sucesso, false caso contrário.</returns>
		public bool Open(string connectionstring)
		{
			log.Info($"Openning connection in COMOS. connection string {connectionstring}");
			IComosDWorkset WorkSet = null;
			if (AppGlobal.Workset == null) // !! Comos 10.1, Não é possivel de criar mais que um workset no mesmo processo.                
			{
				// Criar um workset. 
				try
				{
					WorkSet = new CPLTWorkset() as IComosDWorkset;
					log.Info($"Comos IO v1.3 - Successfully created an IComosDWorkset");
				}
				catch (Exception ex)
				{
					log.Error($"Error trying to open the connection.", ex);
				}

				if (WorkSet == null)
				{
					return false;
				}
				//WorkSet.StartMode = 256; // Is in Web server mode
				//AppGlobal.Workset = WorkSet;
			}
			else
			{
				WorkSet = AppGlobal.Workset;
				log.Info($"Comos IO v1.3 - Old Workset assigned");
			}

			// Evite caixas de diálogo Standard COMOS
			WorkSet.MessageBoxHandler = (object)new ComosMsgHandler();

			// Inicializar Workset  em uma thread. Requisito por COMOS 10.2 
			bool error = false;
			System.Threading.Thread thread = new System.Threading.Thread(
					delegate ()
					{
						if (!WorkSet.IsInitialized())
						{
							try
							{
								log.Info($"initialization workset");

								//string value = WorkSet.InitAOWM();
								//WorkSet.FinishAOWM(value);

								//DOWPS wps = new DOWPS(WorkSet);  //Comenté esto. Esto se usaba para cambiar usuarios
								WorksetHack wps = new WorksetHack(WorkSet);

								error = WorkSet.Init(string.Empty, string.Empty, connectionstring);
								CheckWorkFlowManager(WorkSet);
								AppGlobal.Workset = WorkSet;
							}
							catch (Exception ex)
							{
								log.Error($"Error on workset initialization. ", ex);
							}
						}
						if (!AppGlobal.Workset.IsInitialized())
						{
							error = true;
						}
						else
						{
							//Log.WriteLog("WS:ERROR");
							error = false; // @ToDo: Everrything seams to work, but WorkSet Init returned error = true.
						}
						return;
					}
				);
			thread.Start();
			thread.Join();
			if (error)
				return false;

			// Register the chemserver
			try
			{
				// register the ChemServer
				m_ChemServer = new Chemserv.ComosClass();
			}
			catch (Exception ex)
			{
				log.Error($"Error trying to register ChemServer", ex);
				return false;
			}

			return true;
		}

		/// <summary>
		/// Atribuir o usuário para próximos operações Comos.
		/// </summary>
		/// <param name="username">Nome de COMOS Usario.</param>
		/// <returns>True é o sucesso, false caso contrário.</returns>
		public bool SetCurrentUser(string username)
		{
			log.Info($"Setting current user. Name: [{username}]");

			try
			{
				// Definir usuário atual.
				username = username.ToUpper();

				dynamic globals = AppGlobal.Workset.Globals();

				globals.MobileUser = username;

				//username = "@SETUP";
				IComosDCollection collection = AppGlobal.Workset.GetAllUsers();
				int size = collection.Count();
				for (int i = 0; i < size; i++)
				{
					IComosDUser user = (IComosDUser)collection.Item(i + 1);
					if (string.Compare(user.Name, username) == 0)
					{
						AppGlobal.Workset.SetCurrentUser((object)user);
						return true;
					}
				}
			}
			catch (Exception ex)
			{
				log.Error($"Error setting the new user.", ex);
			}

			return false;
		}

		/// <summary>
		/// Obter informações de um usuário(objeto IComosDUser), mas também a partir do projeto Usuários.
		/// </summary>
		/// <param name="username">Nome de COMOS Usario.</param>
		/// <returns>Objeto contendo as informações de COMOS</returns>
		public IComosDUser GetUserInfo(string username)
		{
			log.Info($"Getting user information. Name: [{username}]");

			try
			{
				username = username.ToUpper();

				//Original Line        
				IComosDCollection collection = Comos.Global.AppGlobal.Workset.GetAllUsers();

				int size = collection.Count();
				for (int i = 0; i < size; i++)
				{
					IComosDUser user = (IComosDUser)collection.Item(i + 1);
					if (string.Compare(user.Name.ToUpper(), username) == 0)
					{
						return user;
					}
				}
			}
			catch (Exception e)
			{
				log.Info($"Error trying to get the user information.", e);
			}
			return null;
		}

		/// <summary>
		/// Obter todos os projetos disponíveis para o usuário.
		/// </summary>
		/// <returns>Objeto COMOS Project</returns>
		public IEnumerable<IComosDProject> GetProjects()
		{

			log.Info($"Get all projects");

			var result = new List<IComosDProject>();
			IComosDCollection collection;

			try
			{
				collection = AppGlobal.Workset.GetAllProjects();

				int size = collection.Count();
				for (int i = 0; i < size; i++)
				{
					result.Add((IComosDProject)collection.Item(i + 1));
				}

				return result;
			}
			catch (Exception e)
			{
				log.Error($"Error getting the projects", e);
			}

			return result;
		}

		/// <summary>
		/// Obter todos as camadas disponíveis para o usuário e projeto.
		/// </summary>
		/// <returns>Objeto COMOS camada</returns>
		public IEnumerable<IComosDWorkingOverlay> GetWorkingOverlays(IComosDProject project)
		{
			var result = new List<IComosDWorkingOverlay>();

			try
			{
				log.Info($"Get working overlays. project {project?.Description}");

				IComosDCollection collection = project.WorkingOverlays();

				int size = collection.Count();
				for (int i = 0; i < size; i++)
				{
					IComosDWorkingOverlay wo = (IComosDWorkingOverlay)collection.Item(i + 1);
					result.Add(wo);
				}

			}
			catch (Exception ex)
			{
				log.Error($"Error getting the wroking overlays", ex);
			}

			return result;
		}

		/// <summary>
		/// Obter todos as camadas disponíveis para o usuário e camada.
		/// </summary>
		/// <returns>Objeto COMOS camada</returns>
		public IEnumerable<IComosDWorkingOverlay> GetWorkingOverlays(IComosDWorkingOverlay overlay)
		{
			var result = new List<IComosDWorkingOverlay>();

			try
			{
				log.Info($"Get working overlays. Overlay {overlay?.Description}");

				IComosDCollection wo_collection = overlay.WorkingOverlays();
				int wo_size = wo_collection.Count();
				for (int j = 0; j < wo_size; j++)
				{
					IComosDWorkingOverlay wo = (IComosDWorkingOverlay)wo_collection.Item(j + 1);
					result.Add(wo);
				}
			}
			catch (Exception ex)
			{
				log.Error($"Error getting the wroking overlays", ex);
			}

			return result;

		}

		public Plt.IComosDProject GetCurrentProject()
		{
			try
			{
				log.Info($"Get current project.");

				if (Comos.Global.AppGlobal.Workset == null)
					return null;
				return Comos.Global.AppGlobal.Workset.GetCurrentProject();
			}
			catch (Exception ex)
			{
				log.Error($"Error getting the current project", ex);
			}
			return null;
		}

		public IEnumerable<IComosBaseObject> GetByClassification(Plt.IComosBaseObject rootobject, string classcode)
		{
			Plt.IComosDSearchManager search;
			IComosDCollection resultSet = null;

			try
			{
				log.Info($"Get items by clasification. ClassCode: [{classcode}]");
				search = Comos.Global.AppGlobal.Workset.GetSearchManager();
				search.RootObjects.Add(rootobject);
				search.SystemType = 8;
				search.IsSearchCaseSensitive = true;
				search.IsUserAbortAllowed = true;
				search.AppendSearchCondition("", "CLASSIFICATION", "1", "=", classcode);
				dynamic searchitem = search.SearchConditions.Item[1];
				searchitem.IsSearchCaseSensitive = false;

				resultSet = search.Start();
			}
			catch (Exception ex)
			{
				log.Error($"Error getting by classification", ex);
				yield break;
			}
			int amountfound = 10000;
			while (amountfound == 10000)
			{
				amountfound = search.RetrieveData(10000);
				for (int i = 1; i <= amountfound; i++)
				{
					Plt.IComosBaseObject o = resultSet.Item(i) as Plt.IComosBaseObject;
					yield return o;
				}
			}
			search.Stop();

		}

		/// <summary>
		/// Atribuir o projeto e camada para próximos operações Comos.
		/// </summary>
		/// <param name="nameproject">Nome COMOS projeto</param>
		/// <param name="overlayid">ID COMOS Camada</param>
		/// <returns>True é o sucesso, false caso contrário.</returns>
		public bool SetCurrentProjectAndWorkingOverlay(string nameproject, int overlayid = 0)
		{
			log.Info($"Set current project and working overlay. project [{nameproject}], overlay [{overlayid}]");

			try
			{
				nameproject = nameproject.Replace("%20", " ");

				if (AppGlobal.Workset.GetAllProjects().ItemExist(nameproject))
				{
					IComosDProject project = (IComosDProject)AppGlobal.Workset.GetAllProjects().Item(nameproject);
					AppGlobal.Workset.SetCurrentProject(project);
					IComosDWorkingOverlay wo = null;

					if (overlayid != 0)
					{
						wo = project.GetWorkingOverlay(overlayid);
						if (wo == null)
							return false;
					}

					project.CurrentWorkingOverlay = wo;

					return true;
				}
			}
			catch (Exception ex)
			{
				log.Error($"Error getting current project and working overlay", ex);
			}

			return false;
		}

		/// <summary>
		/// Atribuir o projeto e camada para próximos operações Comos.
		/// </summary>
		/// <param name="nameproject">Nome COMOS projeto</param>
		/// <param name="overlayid">ID COMOS Camada</param>
		/// <returns>True é o sucesso, false caso contrário.</returns>
		public bool SetCurrentProject(string nameproject)
		{
			try
			{
				log.Info($"Set current project. project [{nameproject}]");

				foreach (var project in GetProjects())
				{
					if (string.Compare(project.Name, nameproject, true) == 0)
					{
						Comos.Global.AppGlobal.Workset.SetCurrentProject(project);
						project.CurrentWorkingOverlay = null;
						return true;
					}
				}
			}
			catch (Exception ex)
			{
				log.Error($"Error setting current project", ex);
			}
			return false;
		}

		/// <summary>
		/// Guardar todos os dados
		/// </summary>
		public void SaveAll()
		{
			try
			{
				log.Info($"Saving data");
				AppGlobal.Workset.SaveAll();
			}
			catch (Exception ex)
			{
				log.Error($"Error saving data", ex);
			}

		}

		/// <summary>
		/// Fechar sessão COMOS.
		/// </summary>
		public void Close()
		{
			try
			{
				log.Info($"Closing connection");

				if (AppGlobal.Workset != null && AppGlobal.Workset.IsInitialized())
				{
					m_CommonClass = null;
					// m_ChemServer.ShutdownComos();
					m_ChemServer = null;

					AppGlobal.Workset.ReleaseAllObjects();
					/*
					if (AppGlobal.Workset.GetCurrentUser() != null)
					{
							AppGlobal.Workset.DisConnectFromComosSystem();
					}
					AppGlobal.Workset.DisposeAllModules();
					AppGlobal.Workset.DisConnectFromDatabase();
					*/

					AppGlobal.Workset.Terminate();
				}
			}
			catch (Exception ex)
			{
				log.Error($"Error closing workset", ex);
			}
		}

		public IBRServiceContracts.TResult<IBRServiceContracts.CQueryResult> BuscaString(string systemuid)
		{
			try
			{
				System.Diagnostics.Debug.WriteLine(AppGlobal.Workset.GetCurrentProject() == null);
				return BRComos.IO.Utilities.ComosObjectSearch.Buscar(systemuid, AppGlobal.Workset.GetCurrentProject());
			}
			catch (Exception ex)
			{
				log.Error($"Error searching on string UID [{systemuid}]", ex);
				return null;
			}

		}

		/// <summary>
		/// Get the Link query object to execute.
		/// </summary>
		/// <param name="specname">specification name</param>
		/// <param name="query">The result if was found. If not will contain an error message.</param>
		/// <returns></returns>
		public bool TryGetLinkQueryFullName(string specname, out string query)
		{
			try
			{
				log.Info($"Getting link query fullname. name [{specname}]");

				IComosDProject projeto = GetCurrentProject();

				//IComosBaseObject projeto = GetCurrentProject();
				IComosDSpecification specification = projeto.spec(specname);

				if (specification != null)
				{
					//                Log.WriteLog("PROJ::" + projeto.Name);
					//                Log.WriteLog("SPEC::" + (specification.LinkObject != null));

					IComosDDevice linkobject = (IComosDDevice)specification.LinkObject;
					if (linkobject != null)
					{
						query = linkobject.SystemFullName();
						return true;
					}
					else
					{
						log.Warn($"Query settings not defined!");
						query = "Query settings not defined!";
						return false;
					}
				}
				else
				{
					query = "Project missing configuration tab";
					return false;
				}
			}
			catch (Exception ex)
			{
				log.Error($"Error trying get link query full name.", ex);
				query = "Internal Query Error";
				return false;
			}

		}

		public string DownloadFile(string systemuid, bool isrevision)
		{
			string fullfilename = string.Empty;

			try
			{
				log.Info($"Downloading file UID [{systemuid}] IsRevision [{isrevision}].");

				IComosDDocument document = null;
				IComosDDevice revision = null;

				document = GetComosDocumentBySystemUID(systemuid);

				if (document != null)
				{
					log.Info($"Document [{document.SystemFullName()}]");

					//if (document.IsNameSystemReference())
					//{
					//    document = document.NameSystemReference();
					//}

					if (document.OrgDocument != null)
					{
						log.Info($"Origin document [{document.OrgDocument.SystemFullName()}]");
						document = document.OrgDocument;
					}

					if (!isrevision)
					{
						if (document.IsDdmContainer())
						{
							IComosDOwnCollection col = document.Documents();
							document = (IComosDDocument)col.Item(col.Count());
						}

						fullfilename = document.FullFileName();
						log.Info($"fullfield name [{fullfilename}]");
					}
					else
					{
						if (document.Revisions().Count() > 0)
						{
							//revision = document.LastReleasedRevision();
							revision = document.LastCreatedRevision();
							fullfilename = revision.GetRevisionFolder() + @"\" + revision.Name + ".pdf";
						}
					}

				}
			}
			catch (Exception ex)
			{
				log.Error($"Error downloading file.", ex);
			}

			return fullfilename;
		}

		#region Codigo de Query

		internal IComosBaseObject GetComosDeviceBySystemUID(int systype, string systemuid)
		{
			IComosBaseObject comosBaseObject = null;

			try
			{
				log.Info($"Getting comos device UID [{systemuid}]");

				IComosDWorkset workset = AppGlobal.Workset;

				comosBaseObject = (IComosBaseObject)workset.LoadObjectByType(systype, systemuid);
			}
			catch (Exception ex)
			{
				log.Error($"Error getting device.", ex);
			}

			return comosBaseObject;
		}

		internal IComosBaseObject GetComosDeviceBySystemUID(string systemuid)
		{
			IComosDWorkset workset = AppGlobal.Workset;
			IComosBaseObject comosBaseObject = null;

			try
			{
				log.Info($"Get comos device by system UID [{systemuid}]");

				//comosBaseObject = (IComosBaseObject)workset.LoadObjectByType(Plt.ComosSystemTypes.SystemTypeDevice, systemuid);
				comosBaseObject = (IComosBaseObject)workset.LoadObjectByType(ComosSystemTypes.SystemTypeDevice, systemuid);
				if (comosBaseObject == null)
					comosBaseObject = (IComosBaseObject)workset.LoadObjectByType(ComosSystemTypes.SystemTypeCDevice, systemuid);
				if (comosBaseObject == null)
					return null; // Nothing found.
			}
			catch (Exception ex)
			{
				log.Error($"Error getting comos device by system UID [{systemuid}]", ex);
			}

			return comosBaseObject;
		}

		internal IComosDSpecification GetComosSpecificationBySystemUID(string systemuid)
		{
			IComosDSpecification comosSpec = null;

			try
			{
				log.Info($"Get comos specification by system UID [{systemuid}]");
				IComosDWorkset workset = AppGlobal.Workset;
				comosSpec = (IComosDSpecification)workset.LoadObjectByType(ComosSystemTypes.SystemTypeSpecification, systemuid);
			}
			catch (Exception ex)
			{
				log.Error($"Error getting comos device by system UID [{systemuid}]", ex);
				throw;
			}

			return comosSpec;
		}

		public IComosDDocument GetComosDocumentBySystemUID(string systemuid)
		{

			IComosDDocument comosDocumentObject = null;
			try
			{
				log.Info($"Get comos document by system UID [{systemuid}]");
				IComosDWorkset workset = AppGlobal.Workset;
				comosDocumentObject = (IComosDDocument)workset.LoadObjectByType(ComosSystemTypes.SystemTypeDocument, systemuid);
			}
			catch (Exception ex)
			{
				log.Error($"Error getting comos document by system UID [{systemuid}]", ex);
			}

			return comosDocumentObject;
		}

		internal Plt.IComosDCDevice GetCDeviceByFullName(string fullname)
		{
			IComosDCDevice cdev = null;
			try
			{
				log.Info($"Get comos device by full name [{fullname}]");
				cdev = AppGlobal.Workset.GetCurrentProject().GetCDeviceBySystemFullname(fullname, 3);
			}
			catch (Exception ex)
			{
				log.Error($"Error getting comos device by full name [{fullname}]", ex);
			}
			return cdev;
		}

		public Plt.IComosDDevice GetDeviceByFullName(string fullname)
		{
			try
			{
				log.Info($"Get device by full name [{fullname}]");

				string[] names = fullname.Split('|');
				Plt.IComosDDevice bo = (Plt.IComosDDevice)AppGlobal.Workset.GetCurrentProject().Devices().Item(names[0]);
				if (bo == null)
					return null;
				for (int i = 1; i < names.Length; i++)
				{
					if (bo.Devices().ItemExist(names[i]))
					{
						bo = (Plt.IComosDDevice)bo.Devices().Item(names[i]);
					}
					else
					{
						if (bo.CObject.Elements().ItemExist(names[i]))
						{
							IComosDCDevice cdev = GetCObjectToCreate((IComosDCDevice)bo.CObject.Elements().Item(names[i]));
							bo = (IComosDDevice)bo.Devices().CreateNewWithNameAndCDevice(names[i], cdev);
						}
					}

				}
				return bo;
			}
			catch (Exception ex)
			{
				log.Error($"Error getting device.", ex);
			}

			return null;
		}

		static public CQueryResult GetCOMOSQueryData(IComosBaseObject device, IComosBaseObject start)
		{
			CQueryResult result = null;
			try
			{
				log.Info($"Get comos query data. Device [{device?.Name}]");

				AppGlobal.Workset.ReleaseAllObjects();
				AppGlobal.Workset.ReleaseObjects();

				result = new CQueryResult();
				result.Date = DateTime.Now;

				if (device == null)
				{
					log.Error($"Device is null");
					result.QueryName = "Query not found";
					return result;
				}

				result.QueryName = device.SystemFullName();

				dynamic queryxObj = device.XObj;
				if (queryxObj == null)
				{
					log.Warn($"Failed getting xobject");
					return result;
				}

				ComosQueryInterface.ITopQuery topQuery = queryxObj.TopQuery;

				if (start != null)
					topQuery.MainObject = (object)start;

				log.Info($"Execute main object query");
				topQuery.Execute();

				ComosQueryInterface.IQuery query = topQuery.Query;
				result.Columns = new List<IBRServiceContracts.CColumn>();

				log.Info($"Getting column information. Count [{query.ColumnCount}]");

				// Get the column information, and how to evaluate the values.
				for (short key = 1; key <= query.ColumnCount; ++key)
				{
					int num = (int)query.ColumnBaseIndex(key);
					ComosQueryInterface.IColumnDef columndef = query.BaseQuery.Columns.Item((object)num);

					CColumn column = new CColumn();
					column.Name = columndef.Name;
					column.Description = columndef.DisplayDescription;
					column.Visible = columndef.Visible;
					column.ShowIcon = columndef.withPicture;
					//column.ReadOnly = !columndef.Editable;

					switch (columndef.DefaultValueType)
					{
						case ComosQueryInterface.qeValueType.qcBoolean:
							column.ValueType = CColumn.DataType.qcBoolean;
							break;
						case ComosQueryInterface.qeValueType.qcDate:
							column.ValueType = CColumn.DataType.qcDate;
							break;
						case ComosQueryInterface.qeValueType.qcDouble:
							column.ValueType = CColumn.DataType.qcDouble;
							break;
						case ComosQueryInterface.qeValueType.qcLong:
							column.ValueType = CColumn.DataType.qcLong;
							break;
						case ComosQueryInterface.qeValueType.qcString:
							column.ValueType = CColumn.DataType.qcString;
							break;
						case ComosQueryInterface.qeValueType.qcVariant:
						default:
							column.ValueType = CColumn.DataType.qcVariant;
							break;
					}
					result.Columns.Add(column);
				}

				log.Info($"Execute query for rows");

				topQuery.Execute();

				int rowCount = query.RowCount;
				int columnsize = query.ColumnCount;

				log.Info($"Getting row information. Row Count [{rowCount}], column size [{columnsize}]");

				result.Rows = new List<CRow>();
				for (int index = 1; index <= rowCount; ++index)
				{
					CRow row = new CRow();
					row.Values = new CCell[columnsize];
					IComosBaseObject rowobject = query.get_RowObject(index) as IComosBaseObject;
					row.SystemUid = rowobject.SystemUID();
					row.SystemFullName = rowobject.SystemFullName();

					if (rowobject.XObjExist())
						row.IsQuery = true;

					for (short key = 1; key <= query.ColumnCount; ++key)
					{
						ComosQueryInterface.ICell cell = query.Cell(index, key);
						if (cell != null)
						{
							IComosBaseObject cellobject = cell.Object as IComosBaseObject;
							if (cellobject == null)
							{
								row.Values[key - 1] = new CCell()
								{
									Value = cell.Text,
									DisplayValue = cell.Text,
								};
							}
							else
							{
								// Getting the value of the cell.

								string SystemFullName = cellobject.SystemFullName();
								string SystemUid = cellobject.SystemUID();

								if (cellobject.SystemType() == 10)
								{
									IComosDSpecification spec = (IComosDSpecification)(cellobject);
									IComosBaseObject specowner = (IComosBaseObject)spec.GetSpecOwner();
									SystemFullName = specowner.SystemFullName();
									SystemUid = specowner.SystemUID();
								}

								CCell newcell = row.Values[key - 1] = new CCell()
								{
									Value = cell.Text,
									DisplayValue = cellobject.Name + "   " + cellobject.Description,
									SystemUID = SystemUid,
									SystemFullName = SystemFullName,
									Icon = GetQueryIcon(cellobject),
									SystemType = cellobject.SystemType()
								};

								if (cellobject.XObjExist())
								{
									newcell.IsQuery = true;
									newcell.Icon = "list";
								}
							}
						}
					}
					result.Rows.Add(row);
				}

				log.Info($"Closing device connection");
				device.ShutDownXObj();
			}
			catch (Exception ex)
			{
				log.Error($"Error getting comos query data.", ex);
			}

			return result;
		}

		private static string GetQueryIcon(IComosBaseObject obj)
		{
			string returnIcon = string.Empty;
			try
			{

				int SystemType = obj.SystemType();

				switch (SystemType)
				{
					case 8:
						{
							returnIcon = "cube";
						}
						break;
					case 10:
						{
							returnIcon = "alert";
						}
						break;
					case 29:
						{
							returnIcon = "document";
						}
						break;
				}
			}
			catch (Exception ex)
			{
				log.Error($"Error getting icon", ex);
			}

			return returnIcon;
		}

		public CQueryResult GetQueryData(IComosDDevice device, IComosBaseObject start)
		{
			try
			{
				IComosBaseObject obj = (IComosBaseObject)device;

				return GetCOMOSQueryData(obj, start);
			}
			catch (Exception ex)
			{
				log.Error($"Error getting query data", ex);
				return null;
			}

		}

		public bool BatchChangeComosValues(CRow[] specs)
		{
			bool changed = true;

			try
			{
				foreach (CRow item in specs)
				{
					if (!ChangeComosValues(item.Values))
					{
						changed = false;
					}
				}
			}
			catch (Exception ex)
			{
				log.Error($"Error changing comos values.", ex);
				changed = false;
			}

			return changed;
		}

		public bool ChangeComosValues(CCell[] specs)
		{
			try
			{
				log.Info($"Changing comos values");

				IComosDDevice device = null;
				IComosDProject projeto = GetCurrentProject();

				int qtdspecs = specs.Count();
				int specsOk = 0;

				//System.Diagnostics.Debug.Assert(false);                        
				log.Info($"fill [{qtdspecs}]");
				foreach (var spec in specs)
				{
					string systemUid = spec.SystemUID;
					string newValue = spec.Value;

					log.Info($"Value UID [{systemUid}] new Value [{newValue}]");

					if ((device == null) || (device.SystemFullName() != spec.SystemFullName))
					{
						device = GetDeviceByFullName(spec.SystemFullName);
						if (device == null)
						{
							device = (IComosDDevice)GetComosDeviceBySystemUID(spec.SystemFullName);
						}
					}

					IComosDSpecification currentSpec = device.spec(spec.NestedName);

					if (currentSpec != null)
					{

						//TO DO!
						if (fillSpec(newValue, currentSpec))
						{
							specsOk = specsOk + 1;
						}

					}
				}

				if (qtdspecs == specsOk)
					projeto.SaveAll();

				return (qtdspecs == specsOk);
			}
			catch (Exception ex)
			{
				log.Error($"Error changing spec.", ex);
				return false;
			}
			

		}

		bool fillSpec(string newValue, IComosDSpecification spec, bool forceFill = false)
		{
			try
			{
				log.Info($"Filling Spec. Value [{newValue}] ForceFill [{forceFill}]");

				bool filled = false;
				DateTime newdate = DateTime.Now;

				if ((spec.Inheritance == 0 || spec.Inheritance == 1) || (forceFill))
				{
					if (spec.IsValueValid(newValue))
					{
						if (spec.ControlType.ToUpper().Contains("MEMO"))
						{
							spec.SetXValue(0, newValue);
						}
						else if (spec.ControlType.ToUpper().Contains("EDIT"))
						{
							if (spec.StandardTable == null)
							{
								spec.value = newValue;
							}
							else
							{
								log.Warn($"There is not a standar table. getting values");

								spec.value = GetValueForStdValue(newValue, spec);
							}
						}

						else if (spec.ControlType.ToUpper().Contains("CHECK"))
						{
							if (newValue == "true")
							{
								spec.value = "1";
							}
							else
							{
								spec.value = "0";
							}
						}

						//spec.SaveAll();       
						filled = true;

						if (spec.ControlType.ToUpper().Contains("DATE"))
						{
							newValue = newValue.Replace(",", "");

							System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
							System.Globalization.DateTimeStyles styles = System.Globalization.DateTimeStyles.None;

							if (DateTime.TryParse(newValue, culture, styles, out newdate))
							{
								spec.SIValue = newdate.ToOADate();
							}
							else
							{
								filled = false;
							}
						}

					}
					else
					{
						log.Warn($"Spec value is invalid");
					}
				}
				else
				{
					log.Warn($"Spec not set inheritance is [{spec.Inheritance == 0 || spec.Inheritance == 1}]");
				}

				return filled;
			}
			catch (Exception ex)
			{
				log.Error($"Error filling spec.", ex);
			}
			return false;
		}

		//@20|C40|A40|A10!A50!A10
		private string GetValueForStdValue(string value, IComosDSpecification spec)
		{

			IComosDStandardTable table = (IComosDStandardTable)spec.StandardTable;
			IComosDStandardValue curvalue = null;

			for (int i = 1; i < table.StandardValues().Count(); i++)
			{
				curvalue = (IComosDStandardValue)table.StandardValues().Item(i);

				if (curvalue.Description == value)
				{
					return curvalue.GetXValue(0);
				}
			}

			return value;
		}

		private IComosDCDevice GetCObjectToCreate(IComosDCDevice cdev)
		{
			while (cdev.CObject != null)
			{
				cdev = cdev.CObject;
			}

			return cdev;

		}

		public CCell[] GetTaskDataByUid(string systemuid)
		{
			log.Info($"Get taks by UID [{systemuid}]");

			CCell[] cells = new CCell[0];

			if (systemuid.Length == 10)
			{

				var task = GetComosDeviceBySystemUID(systemuid);

				if (task != null)
				{
					log.Info($"Taks found. Filling data");

					cells = new CCell[13];

					for (int i = 0; i < cells.Length - 1; i++)
					{
						cells[i] = new CCell();
					}

					IComosBaseObject owner = (IComosBaseObject)task.owner();
					IComosBaseObject unit = (IComosBaseObject)task.OwnerByClass("U");

					cells[0].Value = unit.Name;
					cells[1].Value = owner.Name;
					cells[2].Value = task.Name;
					cells[3].Value = task.Description;
					cells[4].Value = task.spec("Y00T00222.Y00A02889").value + " h";
					cells[5].Value = task.spec("Y00T00233.Y00A03028").DisplayValue();
					cells[8].Value = task.spec("Y00T00236.Y00A02885").value;
					cells[9].Value = task.spec("Y00T00235.Y00A00726").value;
					cells[10].Value = task.spec("Y00T00236.Y00A00738").value;
					cells[11].Value = task.spec("Y00T00058.Y00A00647").DisplayValue();

				}

			}

			return cells;
		}

		public bool CreateDevice(string owner, string cdevSystemfullname, CCell[] specs = null)
		{

			bool created = false;
			log.Info($"Create device. Owner [{owner}] Device name [{cdevSystemfullname}]");

			try
			{
				var ownerDev = GetDeviceByFullName(owner);
				var cDev = GetCDeviceByFullName(cdevSystemfullname);

				if ((ownerDev != null) && (cDev != null))
				{
					log.Info($"Getting device new name");

					string devname = ownerDev.Devices().NextName("??");

					log.Info($"Getting device based on name [{devname}]");
					var newDev = ownerDev.Devices().CreateNewWithNameAndCDevice(devname, cDev);

					log.Info($"Device created.");

					created = true;
					IComosDDevice device = (IComosDDevice)newDev;

					if (specs != null)
					{
						log.Info($"Filling specs");

						foreach (var spec in specs)
						{
							IComosDSpecification currentSpec = device.spec(spec.NestedName);
							if (currentSpec != null)
							{
								fillSpec(spec.Value, currentSpec, true);
							}
						}
					}

					log.Info($"Commit device data");

					device.SaveAll();
				}

			}
			catch (Exception ex)
			{
				log.Error($"Error creating device.", ex);
			}

			return created;
		}

		public bool createDeviceBySystemUID(string ownersystemuid, string cdevSystemfullname, CCell[] specs = null)
		{
			bool created = false;
			try
			{
				log.Info($"Create device by system UID. Owner system uid [{ownersystemuid}] Device name [{cdevSystemfullname}]");

				var ownerDev = (IComosDDevice)GetComosDeviceBySystemUID(ownersystemuid);
				var cDev = GetCDeviceByFullName(cdevSystemfullname);

				if ((ownerDev != null) && (cDev != null))
				{
					string devname = ownerDev.Devices().NextName("??");

					log.Info($"Getting device based on name [{devname}]");
					var newDev = ownerDev.Devices().CreateNewWithNameAndCDevice(devname, cDev);
					created = true;
					IComosDDevice device = (IComosDDevice)newDev;

					if (specs != null)
					{
						log.Info($"Filling specs");

						foreach (var spec in specs)
						{
							IComosDSpecification currentSpec = device.spec(spec.NestedName);
							if (currentSpec != null)
							{
								fillSpec(spec.Value, currentSpec, true);
							}
						}
					}

					log.Info($"Commit device data");
					device.SaveAll();
				}
			}
			catch (Exception ex)
			{
				log.Error($"Error creating device.", ex);
			}

			return created;
		}

		#endregion

		#region Check WorkflowManager

		static public void CheckWorkFlowManager(IComosDWorkset workset)
		{
			try
			{
				log.Info($"Checking workflow for workset");

				Comos.Workflow.IWorkflowManager wfmanager = null;

				IPLTVarstorage globals = null;

				globals = (IPLTVarstorage)workset.Globals();


				if (globals.Exist("ComosWorkflowManager") == 0)
				{
					wfmanager = new Comos.Workflow.ComosWorkflowManager();

					wfmanager.Init(workset);

					typeof(object).InvokeMember("ComosWorkflowManager", System.Reflection.BindingFlags.SetProperty, (System.Reflection.Binder)null, globals, new object[1] { wfmanager });

				}
			}
			catch (Exception ex)
			{
				log.Error($"Error checking workfow", ex);
			}

		}

		#endregion

		#region Atributos COMOS

		public string ClickBotaoUid(string uidownerbotao, string nestedname)
		{
			string msgRetorno = string.Empty;

			try
			{
				IComosDSpecification botao = null;

				IComosBaseObject botaoowner = null;

				IComosDWorkset workset = Comos.Global.AppGlobal.Workset;

				//botaoowner = GetComosDeviceBySystemUID(uidownerbotao);

				botaoowner = GetDeviceByFullName(uidownerbotao);

				botao = botaoowner.spec(nestedname);

				msgRetorno = ClickBotao(botao);

			}
			catch (Exception ex)
			{
				log.Error($"Error on button click by UID", ex);
			}

			return msgRetorno;
		}


		static public string ClickBotao(IComosDSpecification botao)
		{
			string strRetorno = string.Empty;

			try
			{

				MSScriptControl.ScriptControl ObjSC = new MSScriptControl.ScriptControl();

				IComosDWorkset workset = Comos.Global.AppGlobal.Workset;

				System.Diagnostics.Debug.WriteLine(botao.SystemFullName());

				ObjSC.Language = "VBScript";
				ObjSC.AddCode("On Error Resume Next");
				ObjSC.AddCode(botao.Script.Replace("oXStdMod.ComosMsgBox", "MsgBox"));

				ObjSC.AddCode("Dim StrRetornoMsgBox \n" +
											"Function MsgBox(Str1,Int1,Str2) \n" +
											"StrRetornoMsgBox = Cstr(Str1) \n" +
											"MsgBox = vbyes \n" +
											"End Function");

				ObjSC.AddObject("Project", workset.GetCurrentProject(), true);
				ObjSC.AddObject("Workset", workset, true);
				ObjSC.AddObject("ThisObj", botao, true);
				ObjSC.AddObject("Me", botao, true);

				ObjSC.Run("OnClick");

				strRetorno = ObjSC.Eval("StrRetornoMsgBox");
			}
			catch (Exception ex)
			{
				log.Error($"Error on button click by UID", ex);
				strRetorno = "Error: " + ex.Message;
			}

			return strRetorno;
		}

		#endregion

		private System.Drawing.Imaging.ImageCodecInfo GetEncoder(System.Drawing.Imaging.ImageFormat format)
		{
			System.Drawing.Imaging.ImageCodecInfo[] codecs = System.Drawing.Imaging.ImageCodecInfo.GetImageDecoders();

			foreach (System.Drawing.Imaging.ImageCodecInfo codec in codecs)
			{
				if (codec.FormatID == format.Guid)
				{
					return codec;
				}
			}
			return null;
		}

		public string AddImageToComosObject(string systemfullname_owner, string name, System.Drawing.Image image)
		{
			try
			{
				log.Info($"Add image to comos object. Owner [{systemfullname_owner}] Name [{name}]");

				IComosDDevice m_ComosObject = GetDeviceByFullName(systemfullname_owner);
				if (m_ComosObject == null)
				{
					log.Warn($"Invalid system fullname [{systemfullname_owner}]");

					return "systemfullname is not valid. : AddImageToComosObject";
				}

				Plt.IComosDDocument doc = (Plt.IComosDDocument)m_ComosObject.Documents().CreateNewWithName(name);
				doc.DocumentType = (Plt.IComosDDocumentType)Comos.Global.AppGlobal.Workset.GetCurrentProject().DocumentTypes().Item("GeneralDocument");
				string fullfilename = doc.FullFileName();
				if (fullfilename.EndsWith(".*"))
				{
					fullfilename = fullfilename.Replace(".*", "");
				}

				System.Drawing.Imaging.ImageCodecInfo jpgEncoder = GetEncoder(System.Drawing.Imaging.ImageFormat.Jpeg);
				System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
				System.Drawing.Imaging.EncoderParameter myEncoderParameter = new System.Drawing.Imaging.EncoderParameter(myEncoder, 100L);
				System.Drawing.Imaging.EncoderParameters myEncoderParameters = new System.Drawing.Imaging.EncoderParameters(1);
				myEncoderParameters.Param[0] = myEncoderParameter;
				image.Save(fullfilename + ".jpeg", jpgEncoder, myEncoderParameters);
				doc.Filename = doc.Filename + ".jpeg";
				doc.Save();
				log.Info($"Data saved");

			}
			catch (Exception ex)
			{
				log.Error($"Error on add image to comos.", ex);
			}

			return null;
		}

		private bool IsFileLocked(string path)
		{

			FileInfo file = new FileInfo(path);
			FileStream stream = null;

			try
			{
				stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
			}
			catch (IOException ex)
			{
				//the file is unavailable because it is:
				//still being written to
				//or being processed by another thread
				//or does not exist (has already been processed)
				log.Error($"File ios unavailable. Message [{ex.Message}]");
				return true;
			}
			finally
			{
				if (stream != null)
					stream.Close();
			}

			//file is not locked
			return false;
		}

		public string AddFileToComosObject(string uid, string name, RemoteFileInfo fi)
		{
			log.Info($"Add file to comos object. UId [{uid}] Name [{name}]");

			try
			{
				int devsystemtype = 0;
				string devsystemuid = "";
				string cdevclass = "";
				ComosXStdMod.XStdMod stdMode = new ComosXStdMod.XStdMod();

				Type managerType = Type.GetTypeFromProgID("ComosDocumentManager.DocumentManager");
				dynamic docManager = Activator.CreateInstance(managerType);

				if (SystemUIDHandler.GetComosSystemUID(uid, out cdevclass, out devsystemtype, out devsystemuid))
				{

					IComosDDevice m_ComosObject = (IComosDDevice)GetComosDeviceBySystemUID(devsystemtype, devsystemuid);

					if (m_ComosObject == null)
					{
						log.Warn($"Get device is not valid.");

						return "GetComosDeviceBySystemUID is not valid. : AddFileToComosObject";
					}

					Plt.IComosDDocument doc = (Plt.IComosDDocument)m_ComosObject.Documents().CreateNewWithName(name);
					doc.DocumentType = (Plt.IComosDDocumentType)Comos.Global.AppGlobal.Workset.GetCurrentProject().DocumentTypes().Item("GeneralDocument");
					string fullfilename = doc.FullFileName();

					if (fullfilename.EndsWith(".*"))
						fullfilename = fullfilename.Replace(".*", "");

					string filePath = "c:\\temp\\" + fi.FileName;

					log.Info($"Document [{doc.SystemFullName()}] Name [{fullfilename}]");
					log.Info($"Move [{filePath}] to [{fullfilename}.{fi.Extension}]");
					log.Info($"Is File Locked? [{IsFileLocked(filePath)}]");

					//while (IsFileLocked(filePath))
					//{
					//    System.Threading.Thread.Sleep(1000);
					//}

					log.Info($"Copy and calculate [{filePath}]");

					docManager.CopyAndCalcutlateDocument(filePath, doc);
					doc.Description = fi.Description;
					//doc.Filename = doc.Filename + "." + fi.Extension;
					doc.Save();
				}
			}
			catch (Exception ex)
			{
				log.Error($"Error to add file to comos object.", ex);
			}

			return null;
		}

		#region Interface support V1

		public bool SetCurrentProjectAndWorkingOverlay(string nameproject, string overlay, string language)
		{
			try
			{
				log.Info($"Set current project and working overlay. Project [{nameproject}] Overlay [{overlay}] Language [{language}]");

				string[] tokens = overlay.Split(':');
				int overlay_systype = 0;
				if (!int.TryParse(tokens[1], out overlay_systype))
				{
					log.Warn($"Failed parsing working layer id. data [{tokens[1]}]");

					return false;
				}

				string overlay_sysuid = tokens[2];

				foreach (var project in GetProjects())
				{
					if (string.Compare("U:2:" + project.SystemUID() + ":", nameproject, true) == 0)
					{
						AppGlobal.Workset.SetCurrentProject(project);
						object chemproject = AppGlobal.Workset.GetCurrentProject();
						m_ChemServer.RegisterProject(ref chemproject);
						IComosDWorkingOverlay wo = GetComosDeviceBySystemUID(overlay_systype, overlay_sysuid) as IComosDWorkingOverlay;
						if (wo == null)
						{
							return false;
						}
						project.CurrentWorkingOverlay = wo;
						object chemwo = project.CurrentWorkingOverlay;
						m_ChemServer.RegisterWorkingOverlay(ref chemwo);
						// Get the common class
						object comclass = m_ChemServer.GetiAgeObject("chemserv.Commonclass");
						if (comclass != null)
						{
							m_CommonClass = (Chemserv.CommonClass)comclass;
						}
						log.Warn($"Failed getting chemserver comon class");
						return true;
					}
				}

				log.Warn($"Failed finding required project. Please check BRComosServices web.config is using correct connection string.");
				return false;
			}
			catch (Exception ex)
			{
				log.Error($"Error setting current project and working overlay.", ex);
				throw;
			}
		}

		private IComosDDevice GetIncidentDummyFolder()
		{
			string pathtofolder = "A10|A90";
			return GetDeviceByFullName(pathtofolder);
		}


		public string CreateComosDeviceByWebUID(string owner, string cdevwebuid, string desc)
		{
			string classname = "";
			string systemuid = "";
			int systemtype = 0;

			int cdevsystemtype = 0;
			string cdevclass = "";
			string cdevsystemuid = "";

			IComosBaseObject ownerobj = null;
			IComosDCDevice cDev = null;

			try
			{
				log.Info($"Create comos device by web UID. Web ui [{cdevwebuid}] Owner [{owner}]");

				if (owner == "DUMMY_INCIDENT")
					ownerobj = GetIncidentDummyFolder();

				if (ownerobj == null && SystemUIDHandler.GetComosSystemUID(owner, out classname, out systemtype, out systemuid))
				{
					log.Info($"Device UID. [0A{systemuid}]");

					ownerobj = GetComosDeviceBySystemUID(systemuid);
				}

				log.Info($"Device UID. [0B{ownerobj.SystemFullName()}]");

				if (SystemUIDHandler.GetComosSystemUID(cdevwebuid, out cdevclass, out cdevsystemtype, out cdevsystemuid))
				{
					cDev = (IComosDCDevice)GetComosDeviceBySystemUID(cdevsystemtype, cdevsystemuid);
					log.Info($"Device UID. [A]");
				}

				if ((ownerobj != null) && (cDev != null))
				{
					log.Info($"Device UID. [B]");

					IComosDDevice ownerDev = (IComosDDevice)ownerobj;
					string devname = ownerDev.Devices().NextName("??");

					var newDev = ownerDev.Devices().CreateNewWithNameAndCDevice(devname, cDev);
					IComosDDevice device = (IComosDDevice)newDev;
					device.Description = desc;
					device.SaveAll();

					return SystemUIDHandler.GetComosWebSystemUID(device);

				}
			}
			catch (Exception ex)
			{
				log.Error($"Error creating comos device by web UID.", ex);
			}

			log.Info($"Device UID. [Z]");
			return string.Empty;
		}

		#endregion

		public TResult<CQueryResult> SearchDevicesByNameAndDescription(string projectname, string workinglayer, string language, string tosearch, string filter = "")
		{
			try
			{
				log.Info($"Searching devices by name and description. Project [{projectname}] Working player [{workinglayer}] Language [{language}] Filter [{filter}]");
				SetCurrentProjectAndWorkingOverlay(projectname, workinglayer, language);
				return ComosObjectSearch.BuscarDevices(tosearch, filter);
			}
			catch (Exception ex)
			{
				log.Error($"Error searching devices by name and description", ex);
				return null;
			}
		}

		public TResult<CQueryResult> SearchAllByNameAndDescription(string projectname, string workinglayer, string language, string tosearch)
		{
			try
			{
				log.Info($"Searching all by name and description. Project [{projectname}] Working player [{workinglayer}] Language [{language}]");
				SetCurrentProjectAndWorkingOverlay(projectname, workinglayer, language);
				return ComosObjectSearch.BuscarTudo(tosearch);
			}
			catch (Exception ex)
			{
				log.Error($"Error searching all by name and description", ex);
				return null;
			}
		}

		public TResult<CQueryResult> SearchDocumentsByNameAndDescription(string projectname, string workinglayer, string language, string tosearch)
		{
			try
			{
				log.Info($"Searching documents by name and description. Project [{projectname}] Working player [{workinglayer}] Language [{language}]");
				SetCurrentProjectAndWorkingOverlay(projectname, workinglayer, language);
				return ComosObjectSearch.BuscarDocuments(tosearch);
			}
			catch (Exception ex)
			{
				log.Error($"Error searching documents by name and description", ex);
				return null;
			}

		}

		internal List<CWriteValueResult> WriteComosValues(CWriteValueCollection values)
		{
			List<CWriteValueResult> result = new List<CWriteValueResult>();

			try
			{
				log.Info($"Write comos values. Count [{values?.Values?.Count()}]");

				IComosDProject projeto = GetCurrentProject();
				IComosDDevice device = null;

				string current_web_systemuid = null;
				string classname;
				int systemtype = 0;
				string systemuid;

				foreach (var spec in values.Values)
				{
					log.Info($"Current System UID [{current_web_systemuid}] Web System UID [{spec.WebSystemUID}]");

					if (current_web_systemuid != spec.WebSystemUID)
					{
						log.Info($"Transfrom the Web type SystemUID to a native Comos SystemUID");

						if (SystemUIDHandler.GetComosSystemUID(spec.WebSystemUID, out classname, out systemtype, out systemuid))
						{
							log.Info($"Class [{classname}] System Type [{systemtype}] SystemUID [{systemuid}]");

							device = (IComosDDevice)AppGlobal.Workset.LoadObjectByType(systemtype, systemuid);
							if (device == null)
							{
								log.Warn($"Device not found");

								// Do error handling
								result.Add(new CWriteValueResult()
								{
									NestedName = spec.NestedName,
									SystemUID = spec.WebSystemUID,
									ErrorMsg = string.Format("Device {0} not found.", spec.WebSystemUID),
								});
								continue;
							}

							IComosDSpecification currentSpec = device.spec(spec.NestedName);
							if (currentSpec == null)
							{
								log.Warn($"Current spec not found");

								result.Add(new CWriteValueResult()
								{
									SystemUID = spec.WebSystemUID,
									NestedName = spec.NestedName,
									ErrorMsg = string.Format("Specification {0} not Found for Device {1}", spec.NestedName, spec.WebSystemUID),
								});
							}

							// Write the value.
							var error = WriteSpecValue(currentSpec, spec.NewValue, spec.OldValue, false);
							if (error != WriteSpecError.None)
							{
								log.Warn($"Specification [{spec.NestedName}] Write error for Device [{spec.WebSystemUID}] Error code [{error}]");

								result.Add(new CWriteValueResult()
								{
									SystemUID = spec.WebSystemUID,
									NestedName = spec.NestedName,
									ErrorMsg = string.Format("Specification {0} Write error for Device {1}: ErrorCode= ", spec.NestedName, spec.WebSystemUID, error),
								});
							}
							else
							{
								log.Info($"Write succeeded");
							}
						}
					}

					log.Info($"Saving data");
					projeto.SaveAll();
					log.Info($"Data saved");
				}
			}
			catch (Exception ex)
			{
				log.Error($"Error writting comos values.", ex);
			}
			
			return result;
		}
		WriteSpecError WriteSpecValue(IComosDSpecification spec, string newvalue, string oldvalue, bool detect_syncerror)
		{
			try
			{
				log.Info($"Write spec value. Spec [{spec.Name}] Old Value [{oldvalue}] New Value [{newvalue}]");

				if ((spec.Inheritance != 0 && spec.Inheritance != 1))
				{
					log.Warn($"No Inheritance");
				}

				var linkinfo = spec.LinkInfo();
				if (linkinfo != null && linkinfo.LinkType == 6)
				{
					log.Info($"Spec has link info, getting specification");
					spec = spec.GetLinkedSpecification();
				}
				else if (linkinfo != null)
				{
					log.Warn($"Unsupported link");
					return WriteSpecError.UnsupportedLinkInfo;
				}

				if (!spec.IsValueValid(newvalue))
				{
					log.Warn($"Unsupported value");
					return WriteSpecError.ValueIsNotValid;
				}

				if (spec.ControlType.ToUpper().Contains("MEMO"))
				{
					log.Info($"Spec is MEMO type");
					if (detect_syncerror && (oldvalue != spec.GetXValue(0)))
					{
						return WriteSpecError.SyncError;
					}
					spec.SetXValue(0, newvalue);
					spec.Save();
				}
				// equals because some attributes does not have controltype and they are edit fields
				else if ((spec.ControlType.ToUpper().Contains("EDIT")) || spec.ControlType.Equals(""))
				{
					if (spec.StandardTable == null)
					{
						log.Info($"Spec is EDIT (none standard table) type");
						if (detect_syncerror && (oldvalue != spec.value))
							return WriteSpecError.SyncError;
						spec.value = newvalue;
						spec.Save();
					}
					else
					{
						log.Info($"Spec is EDIT (standard table) type");
						string value = GetValueForStdValue(newvalue, spec);
						if (detect_syncerror && (value != spec.value))
							return WriteSpecError.SyncError;

						spec.value = value;
						spec.Save();
					}
				}
				else if (spec.ControlType.ToUpper().Contains("CHECK"))
				{
					log.Info($"Spec is CHECK type");
					string value = spec.value;
					if (detect_syncerror && (oldvalue != value))
						return WriteSpecError.SyncError;
					spec.value = newvalue;
					spec.Save();
				}

				else if (spec.ControlType.ToUpper().Contains("DATE"))
				{
					log.Info($"Spec is DATE type");
					/*DateTime newdate = DateTime.Now;
					DateTime olddate = DateTime.Now;
					System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
					System.Globalization.DateTimeStyles styles = System.Globalization.DateTimeStyles.None;*/

					/*if (!DateTime.TryParse(newvalue, culture, styles, out newdate))
					{
						return WriteSpecError.DateTimeConversionError;
					}
					else if (!DateTime.TryParse(oldvalue, culture, styles, out olddate))
					{
						return WriteSpecError.DateTimeConversionError;
					}*/
					if (detect_syncerror && (Convert.ToDouble(oldvalue) != spec.SIValue))
						return WriteSpecError.SyncError;
					spec.SIValue = Convert.ToDouble(newvalue);
					spec.Save();
				}
			}
			catch (Exception ex)
			{
				log.Error($"Error writing spec value.", ex);
			}
			return WriteSpecError.None;
		}

		public string AddPictureToComosObject(string uid, string name, string description, System.Drawing.Image image, string fullFileName)
		{
			try
			{
				log.Info($"Add picture to comos object UID [{uid}] Name [{name}] Description [{description}] FileName [{fullFileName}]");

				int devsystemtype = 0;
				string devsystemuid = "";
				string cdevclass = "";

				Type managerType = Type.GetTypeFromProgID("ComosDocumentManager.DocumentManager");
				dynamic docManager = Activator.CreateInstance(managerType);

				if (SystemUIDHandler.GetComosSystemUID(uid, out cdevclass, out devsystemtype, out devsystemuid))
				{

					IComosDDevice m_ComosObject = (IComosDDevice)GetComosDeviceBySystemUID(devsystemtype, devsystemuid);

					if (m_ComosObject == null)
					{
						log.Warn($"System UID and Type not valid");
						return "GetComosDeviceBySystemUID is not valid. : AddPictureToComosObject";
					}

					Plt.IComosDDocument doc = (Plt.IComosDDocument)m_ComosObject.Documents().CreateNewWithName(name);
					doc.DocumentType = (Plt.IComosDDocumentType)Comos.Global.AppGlobal.Workset.GetCurrentProject().DocumentTypes().Item("GeneralDocument");
					string comosFullFileName = doc.FullFileName();
					doc.Description = description;

					if (comosFullFileName.EndsWith(".*"))
						comosFullFileName = comosFullFileName.Replace(".*", "");

					log.Warn($"Document name [{doc.SystemFullName()}]");
					
					if (image != null && !System.IO.File.Exists(fullFileName))
					{
						System.Drawing.Imaging.ImageCodecInfo jpgEncoder = GetEncoder(System.Drawing.Imaging.ImageFormat.Jpeg);
						System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
						System.Drawing.Imaging.EncoderParameter myEncoderParameter = new System.Drawing.Imaging.EncoderParameter(myEncoder, 100L);
						System.Drawing.Imaging.EncoderParameters myEncoderParameters = new System.Drawing.Imaging.EncoderParameters(1);
						myEncoderParameters.Param[0] = myEncoderParameter;
						image.Save(comosFullFileName + ".jpeg", jpgEncoder, myEncoderParameters);
						doc.Filename = doc.Filename + ".jpeg";
					}
					else if (System.IO.File.Exists(fullFileName))
					{
						log.Warn($"Copy and calculate file. Name [{fullFileName}] Document [{doc.SystemFullName()}]");
						docManager.CopyAndCalcutlateDocument(fullFileName, doc);
						//ComosXStdMod.XStdMod stdMode = new ComosXStdMod.XStdMod();
						//stdMode.ComosCopyFile(fullFileName, comosFullFileName + ".jpeg", true);                    
					}
					else
					{
						log.Error($"Could not save file. Name [{comosFullFileName}]");
						return null;
					}

					log.Warn($"Saving data");
					doc.Save();

					log.Info($"Document saved. Full FileName [{doc.FullFileName()}]. Document Saved [{System.IO.File.Exists(comosFullFileName)}]");
					
					return doc.SystemFullName();
				}

			}
			catch (Exception ex)
			{
				log.Error($"Error adding Picture to comos object.", ex);
			}

			return null;
		}

		public void LoadDocumentAndCreateRedLines(string uid, string name, RemoteFileInfo fi)
		{
			try
			{
				log.Info($"Load Document and create red lines. UID [{uid}] Name [{name}]");

				int devsystemtype = 0;
				string devsystemuid = "";
				string cdevclass = "";

				string filePath = "c:\\temp\\" + fi.FileName;

				if (SystemUIDHandler.GetComosSystemUID(uid, out cdevclass, out devsystemtype, out devsystemuid))
				{

					IComosDDocument doc = (IComosDDocument)GetComosDeviceBySystemUID(devsystemtype, devsystemuid);
					if (doc != null && doc.Revisions().Count() > 0)
					{
						CreateDocumentForRevision((IComosDDevice)doc.Revisions().Item(doc.Revisions().Count()), filePath, fi.User, fi.Extension.ToUpper().Equals("PDF"));
					}
					else
					{
						log.Warn($"Doc does not have revisions");
					}
				}
			}
			catch (Exception ex)
			{
				log.Error($"Error Loading Document and create red lines", ex);
			}
			
		}
		public void CreateDocumentForRevision(IComosDDevice rev, string filePath, string user, bool isRedLine = true)
		{
			try
			{
				log.Info($"Create document for revision. File Path [{filePath}] User [{user}] IsReadLine [{isRedLine}]");

				IComosDDocument pack = null;
				string systemFullNameCObjectPack = "";
				string systemFullNameCObjectItem = "";
				string namePack = "";


				//Set obj = createobject("Comos.RedliningViewer.Script")
				//obj.ShowRedlinings a.revisions.item(1)

				if (isRedLine)
				{
					systemFullNameCObjectPack = "@99|A30|M00|A80|Y00R00066";
					systemFullNameCObjectItem = "@99|A30|M00|A80|A10";
					namePack = "Y00R00066";
				}
				else
				{
					systemFullNameCObjectPack = "@99|A30|M00|A80|Y00R00067";
					systemFullNameCObjectItem = "@99|A30|M00|A80|A20";
					namePack = "Y00R00067";
				}

				log.Info($"Creating Redline for revision [{rev.SystemFullName()}]");
				log.Info($"PackName [{namePack}]");

				if (rev.Documents().ItemExist(namePack))
				{
					pack = (IComosDDocument)rev.Documents().Item(namePack);
				}
				else
				{
					pack = (IComosDDocument)rev.Documents().CreateNewWithName(namePack);
					pack.CObject = AppGlobal.Workset.GetCurrentProject().GetCDeviceBySystemFullname(systemFullNameCObjectPack, 3);
					pack.DocumentType = (IComosDDocumentType)pack.Project().DocumentTypes().Item("Package");
					pack.SaveAll();

				}

				IComosDDocument item = (IComosDDocument)pack.Documents().CreateNewWithName(rev.Project().Workset().NewComosUID());
				item.CObject = AppGlobal.Workset.GetCurrentProject().GetCDeviceBySystemFullname(systemFullNameCObjectItem, 3);
				item.DocumentType = (IComosDDocumentType)AppGlobal.Workset.GetCurrentProject().DocumentTypes().Item("GeneralDocument");
				string fullfilename = item.FullFileName();
				if (fullfilename.EndsWith(".*"))
				{
					fullfilename = fullfilename.Replace(".*", "");
				}

				item.SaveAll();
				string extension = System.IO.Path.GetExtension(filePath);
				
				log.Info($"Item [{item.FullFileName()}] Filepath [{filePath}] Copy To [{fullfilename + extension}]");

				//AppGlobal.Workset.CopyFile(filePath, fullfilename + extension, false);
				//System.IO.File.Copy(filePath, fullfilename + extension);
				ComosXStdMod.XStdMod stdMode = new ComosXStdMod.XStdMod();
				stdMode.ComosCopyFile(filePath, fullfilename + extension, true);

				item.Filename = item.Filename + extension;

				log.Info($"ItemName [{item.FullFileName()}] PackName [{pack.SystemFullName()}]");
				
				item.spec("Y00T00274.Y00A00869").value = filePath;
				item.spec("Y00T00274.Y00A00785").value = user;
				item.spec("Y00T00274.Y00A00710").SIValue = DateTime.Now.ToOADate();

				item.SaveAll();

				log.Info($"Item created [{item.SystemFullName()}]");

			}
			catch (Exception ex)
			{
				log.Error($"Error creating document for revision.", ex);
			}
		}

		/*

		 'PackAge = @99|A30|M00|Y00R00066 
		'1 name = Y00R00066 = redlines // @99|A30|M00|A10

		'PacAge = @99|A30|M00|Y00R00067 
		'2 name = Y00R00067 = additional docs // @99|A30|M00|A20
		'
		'
		'a.documents.item(2).documents.item(1).Name = A42DAIJF04
		'
		'
		'CreateDate Y00T00274.Y00A00710 
		'CreatedBy Y00T00274.Y00A00785
		'File Name = Y00T00274.Y00A00869

		 */

	}
}

