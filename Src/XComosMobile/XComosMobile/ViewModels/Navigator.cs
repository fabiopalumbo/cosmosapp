using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComosWebSDK.Data;
using System.ComponentModel;

namespace XComosMobile.ViewModels
{
	public class Navigator : Observable
	{

		private string navmode = "U";
		public string NavigationMode { get { return navmode; } set { navmode = value; } }

		ComosWebSDK.IComosWeb m_ComosWeb
		{
			get
			{
				return Services.XServices.Instance.GetService<ComosWebSDK.IComosWeb>();
			}
		}

		CWorkingLayer m_Layer;
		string m_Language;
		Stack<CObject> m_Parent = new Stack<CObject>();
		Pages.controls.NavigationBarControl nav;

		public Navigator(CWorkingLayer layer, string language)
		{
			m_Layer = layer;
			m_Language = language;
			//NavigateToRoot();
			Task myTask = StartNavigationInObject("U:8:A49XFBSUP5:");
		}

		public string Database { get { return m_Layer.Database; } }
		public string Language { get { return m_Language; } }

		public async Task StartNavigationInObject(string objUID)
		{
			CSystemObject sysobj;
			try
			{
				sysobj = await m_ComosWeb.GetObject(m_Layer, objUID, m_Language);
			}
			catch(Exception ex)
			{
				await App.Current.MainPage.DisplayAlert("Error", "Error al cargar objetos de Comos Web: " + ex.Message, Services.TranslateExtension.TranslateText("OK"));
				return;
			}

			if (sysobj == null)
				return;

			CObject obj = new CObject()
			{
				ClassType = sysobj.SystemType,
				Description = sysobj.Description,
				IsClientPicture = sysobj.IsClientPicture,
				Name = sysobj.Name,
				UID = sysobj.UID,
				OverlayUID = m_Layer.UID,
				Picture = sysobj.Picture,
				ProjectUID = m_Layer.ProjectUID,
				SystemFullName = sysobj.Name,
			};
			await this.NavigateToChild(obj);
		}

		public async Task NavigateToRoot()
		{
			CSystemObject sysobj;
			try
			{
				sysobj = await m_ComosWeb.GetObject(m_Layer, m_Layer.ProjectUID, m_Language);
			}
			catch (Exception ex)
			{
				await App.Current.MainPage.DisplayAlert("Error", "Error al cargar objetos de Comos Web: " + ex.Message, Services.TranslateExtension.TranslateText("OK"));
				return;
			}
			
			CObject obj = new CObject()
			{
				ClassType = sysobj.SystemType,
				Description = sysobj.Description,
				IsClientPicture = sysobj.IsClientPicture,
				Name = sysobj.Name,
				UID = sysobj.UID,
				OverlayUID = m_Layer.UID,
				Picture = sysobj.Picture,
				ProjectUID = m_Layer.ProjectUID,
				SystemFullName = sysobj.Name,
			};
			await this.NavigateToChild(obj);
		}

		public bool IsNotRoot
		{
			get { return m_Parent.Count() != 0; }
		}

		public CObject Parent
		{
			get
			{
				if (m_Parent.Count > 0)
				{
					return m_Parent.Peek();
				}
				else
				{
					return null;
				}
			}
		}

		CObject m_Current = null;
		public CObject Current
		{
			get { return m_Current; }
			set { m_Current = value; OnPropertyChanged("Current"); }
		}

		CObject[] m_Children = null;
		public CObject[] Children
		{
			get { return m_Children; }
			set { m_Children = value; OnPropertyChanged("Children"); Nav.FullList = (GetListForFilter(m_Children)); }
		}

		private List<Pages.controls.NavigationBarControl.FilterItem> GetListForFilter(CObject[] children)
		{
			List<Pages.controls.NavigationBarControl.FilterItem> filterList = new List<Pages.controls.NavigationBarControl.FilterItem>();

			foreach (var child in children)
			{
				filterList.Add(new Pages.controls.NavigationBarControl.FilterItem() { FilterValue = child.Name + child.Description, FilterObject = child });
			}
			return filterList;
		}

		public Pages.controls.NavigationBarControl Nav
		{
			get { return nav; }
			set { nav = value; }
		}

		private void AddParrent(CObject o)
		{
			m_Parent.Push(o);
			OnPropertyChanged("Parent");
			OnPropertyChanged("IsNotRoot");
		}

		private void RemoveParrent()
		{
			m_Parent.Pop();
			OnPropertyChanged("Parent");
			OnPropertyChanged("IsNotRoot");
		}

		public async Task NavigateToChild(CObject item)
		{
			if (this.Current != null)
				this.AddParrent(this.Current);

			try
			{
				var objects = await m_ComosWeb.GetNavigatorNodes_Children(m_Layer.Database, item.ProjectUID, item.OverlayUID, m_Language, item.UID, navmode);
				this.Children = objects.ToArray();
				this.Current = item;
			}
			catch(Exception e)
			{
				await App.Current.MainPage.DisplayAlert("Error", "Error al obtener objetos de Comos Web: " + e.Message, Services.TranslateExtension.TranslateText("OK"));
			}
		}

		public async Task MoveUp()
		{
			if (!this.IsNotRoot)
				return;
			CObject item = this.Parent;
			this.RemoveParrent();

			try
			{
				var objects = await m_ComosWeb.GetNavigatorNodes_Children(m_Layer.Database, item.ProjectUID, item.OverlayUID, m_Language, item.UID, navmode);
				this.Children = objects.ToArray();
				this.Current = item;
			}
			catch (Exception e)
			{
				await App.Current.MainPage.DisplayAlert("Error", "Error al obtener objetos de Comos Web: " + e.Message, Services.TranslateExtension.TranslateText("OK"));
			}
		}

	}
}
