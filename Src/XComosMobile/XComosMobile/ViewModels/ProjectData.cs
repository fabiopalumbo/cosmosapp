using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComosWebSDK;
using ComosWebSDK.Data;
using System.ComponentModel;

namespace XComosMobile.ViewModels
{
	public class ProjectData : Observable
	{
		Services.XDatabase XDatabase { get { return Services.XServices.Instance.GetService<Services.XDatabase>(); } }
		IComosWeb m_Client
		{
			get
			{
				return Services.XServices.Instance.GetService<ComosWebSDK.IComosWeb>();
			}
		}

		private IComosWeb ComosClient { get { return m_Client; } }

		public string User { get; private set; }
		public ProjectData()
		{
			var db = Services.XServices.Instance.GetService<Services.XDatabase>();
			this.User = db.ReadSetting("UserName", (string)null);
			this.RememberLayer = db.ReadSetting("Automatic", false);
		}

		#region Data

		private CDatabase[] m_Databases;
		public CDatabase[] Databases
		{
			get => m_Databases;
			private set { m_Databases = value; OnPropertyChanged("Databases"); }
		}

		private CLanguage[] m_Languages;
		public CLanguage[] Languages
		{
			get => m_Languages;
			private set { m_Languages = value; OnPropertyChanged("Languages"); }
		}

		private CProject[] m_Projects;
		public CProject[] Projects
		{
			get => m_Projects;
			private set { m_Projects = value; OnPropertyChanged("Projects"); }
		}

		private CWorkingLayer[] m_Layers;

		public CWorkingLayer[] Layers
		{
			get => m_Layers;
			set
			{
				m_Layers = value;
				OnPropertyChanged("Layers");
			}
		}

		bool m_IsProjectReady = false;
		public bool IsProjectReady
		{
			get => m_IsProjectReady;
			private set
			{
				if (m_IsProjectReady == value)
					return;
				m_IsProjectReady = value; OnPropertyChanged("IsProjectReady");
			}
		}


		bool m_IsDBReady = false;
		public bool IsDBReady
		{
			get => m_IsDBReady;
			private set
			{
				if (m_IsDBReady == value)
					return;
				m_IsDBReady = value;
				OnPropertyChanged("IsDBReady");
			}
		}

		CDatabase m_SelectedDB = null;
		public CDatabase SelectedDB
		{
			get
			{
				return m_SelectedDB;
			}
			private set
			{
				if (m_SelectedDB == value)
					return;
				m_SelectedDB = value;
				if (m_SelectedDB == null || m_SelectedLanguage == null)
				{
					this.IsDBReady = false;
				}
				else
				{
					this.IsDBReady = true;
				}
				OnPropertyChanged("SelectedDB");
			}
		}

		CLanguage m_SelectedLanguage = null;
		public CLanguage SelectedLanguage
		{
			get => m_SelectedLanguage;
			set
			{
				if (m_SelectedLanguage == value)
					return;
				m_SelectedLanguage = value;
				if (m_SelectedDB == null || m_SelectedLanguage == null)
				{
					this.IsDBReady = false;
				}
				else
				{
					this.IsDBReady = true;
				}

				OnPropertyChanged("SelectedLanguage");
			}
		}

		public void OverideSelectedLanguage(CLanguage language)
		{
			m_SelectedLanguage = language;
		}

		//public void SelectLanguage(CLanguage language)
		//{
		//    if (this.SelectedLanguage == language)
		//        return; // this language is already selected.
		//    this.SelectedLanguage = language;
		//}


		public async Task<bool> OpenDatabase(CDatabase database)
		{
			if (this.SelectedDB == database)
				return false; // database is already opened

			List<CProject> projects = null;
			try
			{
				projects = await m_Client.GetProjects(database);
			}
			catch(Exception e)
			{
				await App.Current.MainPage.DisplayAlert("Error", "Error al obtener Proyectos: " + e.Message, Services.TranslateExtension.TranslateText("OK"));
			}
			
			if (projects == null)
			{
				return false;
			}
			else
			{
				this.SelectedDB = database;
				this.Projects = projects.ToArray();
			}
			return true;
		}

		CProject m_SelectedProject = null;
		public CProject SelectedProject
		{
			get
			{
				return m_SelectedProject;
			}
			private set
			{
				m_SelectedProject = value;
				OnPropertyChanged("SelectedProject");
			}
		}
		public async Task SelectProject(CProject project)
		{
			if (this.SelectedProject == project)
				return; // this project is already opened.
			this.SelectedLayer = null;
			if (project != null)
			{
				try
				{
					var layers = await m_Client.GetWorkingOverlays(project);
					this.Layers = layers.ToArray();
				}
				catch(Exception e)
				{
					await App.Current.MainPage.DisplayAlert("Error", "Error al obtener Capas de trabajo: " + e.Message, Services.TranslateExtension.TranslateText("OK"));
					this.Layers = null;
				}
			}
			else
				this.Layers = null;
			this.SelectedProject = project;
			OnPropertyChanged("BreadCrumbLayers");
		}


		bool m_RememberLayer = false;
		public bool RememberLayer
		{
			get => m_RememberLayer;
			set
			{
				m_RememberLayer = value;
				OnPropertyChanged("RememberLayer");
			}
		}


		CWorkingLayer m_SelectedLayer = null;
		public CWorkingLayer SelectedLayer
		{
			private set
			{
				m_SelectedLayer = value;
				OnPropertyChanged("SelectedLayer");
				this.IsProjectReady = m_SelectedLayer != null;
			}
			get
			{
				return m_SelectedLayer;
			}
		}

		public async Task SelectLayer(CWorkingLayer layer)
		{
			try
			{
				var Layers = await m_Client.GetWorkingOverlays(layer);
				this.Layers = Layers.ToArray();
			}
			catch(Exception e)
			{
				await App.Current.MainPage.DisplayAlert("Error", "Error al obtener Capas de Trabajo: " + e.Message, Services.TranslateExtension.TranslateText("OK"));
				this.Layers = null;
			}
			
			layer.Layers = this.Layers;
			this.SelectedLayer = layer;
		}

		#endregion

		public bool SaveLastSession()
		{
			if (this.RememberLayer)
			{
				var db = Services.XServices.Instance.GetService<Services.XDatabase>();
				db.WriteSetting("Database", this.SelectedDB);
				db.WriteSetting("Language", this.SelectedLanguage);
				db.WriteSetting("Project", this.SelectedProject);
				db.WriteSetting("Layer", this.SelectedLayer);
				db.WriteSetting("Automatic", this.RememberLayer);
				return true;
			}
			return false;
		}

		public async Task<bool> LoadLastSession()
		{
			var xdb = this.XDatabase;
			CInfo infos;
			try
			{
				infos = await m_Client.GetDatabases();
			}
			catch(Exception e)
			{
				await App.Current.MainPage.DisplayAlert("Error", "Error al obtener Base de Datos: " + e.Message, Services.TranslateExtension.TranslateText("OK"));
				return false;
			}

			if (infos == null)
				return false;

			this.Databases = infos.Databases;
			this.Languages = infos.Languages;


			if (xdb.ReadSetting("Automatic", false))
			{
				this.RememberLayer = true;
				// Try to restore the old settings, of last time opened layer.
				var prevlcid = xdb.ReadSetting("Language", (CLanguage)null);
				if (prevlcid != null)
				{
					this.SelectedLanguage = this.Languages.Where(x =>
							string.Compare(x.LCID, prevlcid.LCID) == 0).FirstOrDefault();

					if (this.SelectedLanguage == null)
					{
						this.SelectedLanguage = this.Languages[0];
					}

				}
				else
					return false;
				var prevdb = xdb.ReadSetting("Database", (CDatabase)null);
				if (prevdb != null)
				{
					// Check if database exists in the available once.
					var db = this.Databases.Where(x =>
							string.Compare(x.Key, prevdb.Key) == 0).FirstOrDefault();
					if (db == null)
						return false; // maybe database has been removed or renamed since last time.
													// Open the database
					var success = await OpenDatabase(db);
					if (!success)
						return false;
				}
				else
					return false;

				// Select the project last time opened
				var prevProj = xdb.ReadSetting("Project", (CProject)null);
				if (prevProj != null)
				{
					// Check if project exists in the available ones.
					var proj = this.Projects.Where(x =>
							string.Compare(x.UID, prevProj.UID) == 0).FirstOrDefault();
					if (proj == null)
						return false; // maybe project has been deleted since last time.
					await SelectProject(proj);
				}
				else
					return false;

				var prevLayer = xdb.ReadSetting("Layer", (CWorkingLayer)null);
				if (prevLayer != null)
				{
					this.SelectedLayer = prevLayer;
				}
				return true;
			}
			return false;
		}
	}
}


