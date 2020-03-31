

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ComosWebSDK.Data;
using ComosWebSDK.Extensions;
using XComosMobile.Pages.comos;
using System.Collections;
using System.Dynamic;

namespace XComosMobile.Pages.controls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class QueryControl : ContentView
	{

		private int pickerIndex = -1;
		private QueryViewMode view = 0;

		private const string LIST = "\uf03a";
		private const string TABLE = "\uf0ce";

		string m_Language;

		Converters.PictureRefToImage converter = new Converters.PictureRefToImage();
		Converters.IsComosDocumentUIDToBool docconverter = new Converters.IsComosDocumentUIDToBool();
		Converters.IsComosDeviceUIDToBool devconverter = new Converters.IsComosDeviceUIDToBool();

		CObject m_Mainobject;
		CWorkingLayer m_Layer;
		ViewModels.ProjectData m_ProjectData;
		PageTemplate template = null;
		List<CColumn> columns = new List<CColumn>();
		ListView listQuery = null;

		public event EventHandler OnCellTaped;

		protected virtual void CellTaped(object sender, EventArgs e)
		{
			EventHandler handler = OnCellTaped;
			if (handler != null)
			{
				handler(sender, e);
			}
		}


		ComosWebSDK.IComosWeb m_ComosWeb
		{
			get
			{
				return Services.XServices.Instance.GetService<ComosWebSDK.IComosWeb>();
			}
		}

		public QueryViewMode ViewMode
		{

			get { return view; }
			set
			{
				view = value;
			}
		}

		bool expandable = true;
		public bool Expandable
		{
			get
			{
				return expandable;
			}
			set
			{
				expandable = value;
				OnPropertyChanged("Expandable");
			}
		}

		bool filtervisible = true;
		public bool FilterVisible
		{
			get
			{
				return filtervisible;
			}
			set
			{
				filtervisible = value;
				OnPropertyChanged("FilterVisible");
			}
		}

		public void SetMainObject(CObject o)
		{

			converter.InitConverter(m_Navigator);
			m_Mainobject = o;

		}

		List<CRow> mainList { get { return m_Query.Rows.ToList(); } }

		public controls.NavigationBarControl nav { private get; set; }

		public List<NavigationBarControl.FilterItem> GetListToFilter()
		{
			List<NavigationBarControl.FilterItem> list = new List<NavigationBarControl.FilterItem>();


			foreach (var item in mainList)
			{
				NavigationBarControl.FilterItem obj = new NavigationBarControl.FilterItem()
				{
					FilterValue = item.Items[pickerFilter.SelectedIndex].Text,
					FilterObject = item
				};
				list.Add(obj);
			}
			return list;
		}

		public enum QueryViewMode
		{
			Accordion,
			List
		}

		string m_Navigator;


		public QueryControl()
		{
			InitializeComponent();
			m_ProjectData = Services.XServices.Instance.GetService<ViewModels.ProjectData>();
			m_Layer = m_ProjectData.SelectedLayer;
			m_Language = m_ProjectData.SelectedLanguage.LCID;
			m_Navigator = m_ProjectData.SelectedDB.Key;
			filter.BindingContext = this;
		}

		public void SetNavigator(string nav)
		{
			m_Navigator = nav;
		}
		public async Task StartQuery(string navigator, CObject o, QueryViewMode mode = QueryViewMode.Accordion)
		{
			ViewModels.ProjectData m_ProjectData = Services.XServices.Instance.GetService<ViewModels.ProjectData>();
			m_Layer = m_ProjectData.SelectedLayer;
			m_Language = m_ProjectData.SelectedLanguage.LCID;
			m_Mainobject = o;

			m_Navigator = navigator;

			SetNavigator(navigator);
			SetMainObject(o);

			template = new PageTemplate();

			await UpdateQuery(o, m_Language);

			switch (view)
			{
				case QueryViewMode.Accordion:
					await ShowListQuery();
					break;
				case QueryViewMode.List:
					await ShowTableQuery();
					break;
			}

		}


		public async Task UpdateQuery(CObject obj, string language)
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				template.ShowSpinner(Services.TranslateExtension.TranslateText("fetch_query"));
			});

			try
			{
				this.Query = await m_ComosWeb.GetqueriesResult(
					m_Navigator, obj.ProjectUID, obj.OverlayUID, language, obj.UID, null);
			}
			catch(Exception e)
			{
				await App.Current.MainPage.DisplayAlert("Error", "Error al cargar query: " + e.Message, Services.TranslateExtension.TranslateText("OK"));
				return;
			}

			columns.Clear();

			// Create the page content in code.
			for (int i = 0; i < m_Query.Columns.Length; i++)
			{
				var column = m_Query.Columns[i];
				columns.Add(column);
			}

			pickerFilter.ItemsSource = columns;

			mainList.Clear();
			Device.BeginInvokeOnMainThread(() =>
					{
						template.HideSpinner();
					});
		}

		public async Task ShowListQuery()
		{
			if (this.Query == null)
			{
				//template.HideSpinner();
				string text = Services.TranslateExtension.TranslateText("query_not_found");
				text = text.Replace("@", m_Mainobject.SystemFullName);

				await App.Current.MainPage.DisplayAlert("", text, "OK");
				return;
			}

			Device.BeginInvokeOnMainThread(() =>
			{
				// do the rest inside the event
				pickerFilter.SelectedIndex = 0;
				UpdateAccordion();

				if (template != null)
					template.HideSpinner();
			});
		}
		private DataTemplate CreateTemplateForRow(CColumn[] columns, ListView listQuery, string language)
		{

			DataTemplate template = new DataTemplate(() =>
			{

				var layout = new StackLayout() { Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.StartAndExpand };
				var headerlayout = new StackLayout() { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };

				Label lblHeader = new Label();

				lblHeader.VerticalOptions = LayoutOptions.FillAndExpand;
				lblHeader.HorizontalOptions = LayoutOptions.FillAndExpand;
				lblHeader.SetBinding(Label.TextProperty, "Items[ " + pickerIndex + "].Text");
				lblHeader.FontAttributes = FontAttributes.Bold;

				FFImageLoading.Forms.CachedImage img = new FFImageLoading.Forms.CachedImage();
				Binding binding = new Binding("Items[ " + pickerIndex + "].Picture");
				binding.Converter = converter;
				img.SetBinding(FFImageLoading.Forms.CachedImage.SourceProperty, binding);

				img.HeightRequest = 20;
				img.WidthRequest = 20;

				// document download

				/*

				AFButton btdoc = new AFButton();
				Binding bindinguid = new Binding("Items[ " + pickerIndex + "].UID");
				bindinguid.Converter = docconverter;
				btdoc.SetBinding(AFButton.IsVisibleProperty, bindinguid);
				btdoc.Text = "\uf019";
				btdoc.HorizontalOptions = LayoutOptions.EndAndExpand;
				btdoc.BackgroundColor = Color.Transparent;
				btdoc.BorderColor = Color.Transparent;
				btdoc.Clicked += (object sender, EventArgs e) =>
				{
						AFButton af = (AFButton)sender;                    
						 Services.ComosDocumentHandler.DownloadDocument(m_ProjectData, "");
				};

				*/

				// incident creation

				/*
				AFButton btdev = new AFButton();
				Binding bindinguiddev = new Binding("Items[ " + pickerIndex + "].UID");
				bindinguiddev.Converter = devconverter;
				btdev.SetBinding(AFButton.IsVisibleProperty, bindinguiddev);
				btdev.Text = "\uf071";
				btdev.HorizontalOptions = LayoutOptions.EndAndExpand;
				btdev.BackgroundColor = Color.Transparent;
				btdev.BorderColor = Color.Transparent;
				btdev.Clicked += async (object sender, EventArgs e) =>
				{
						AFButton af = (AFButton)sender;
						CRow cell = (CRow)af.BindingContext;
						string uid = cell.Items[pickerIndex].UID;
						string pic = cell.Items[pickerIndex].Picture;
						string ownername = cell.Items[pickerIndex].Text;

						var db = Services.XServices.Instance.GetService<Services.XDatabase>();
						ComosWebSDK.UI.UICachedScreen screen = db.GetCachedScreen(comos.Constants.IncidentCDevUID);            
						await this.Navigation.PushAsync(new PageNewDevice(screen, uid, pic, ownername));

				};
				*/

				var contentlayout = new StackLayout()
				{
					Orientation = StackOrientation.Vertical,
					IsVisible = false,
				};

				contentlayout.FadeTo(0, 300, Easing.Linear);

				int colid = 0;

				foreach (CColumn column in columns)
				{

					AFButton bt = new AFButton();
					Label lbl = new Label();
					Label lblvalue = new Label();
					StackLayout stackRow = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, Spacing = 5 };

					lbl.VerticalOptions = LayoutOptions.Center;
					lbl.HorizontalOptions = LayoutOptions.Start;
					lbl.FontAttributes = FontAttributes.Bold;
					lbl.Text = column.DisplayDescription;

					FFImageLoading.Forms.CachedImage imginterior = new FFImageLoading.Forms.CachedImage();
					Binding bindinginterior = new Binding("Items[ " + colid.ToString() + "].Picture");
					bindinginterior.Converter = converter;
					imginterior.SetBinding(FFImageLoading.Forms.CachedImage.SourceProperty, bindinginterior);
					imginterior.HeightRequest = 15;
					imginterior.WidthRequest = 15;

					lblvalue.VerticalOptions = LayoutOptions.Center;
					lblvalue.HorizontalOptions = LayoutOptions.End;
					lblvalue.SetBinding(Label.TextProperty, "Items[ " + colid.ToString() + "].Text");

					colid = colid + 1;

					bt.Text = "\uf054";
					bt.HorizontalOptions = LayoutOptions.EndAndExpand;
					bt.BackgroundColor = Color.Transparent;
					bt.BorderColor = Color.Transparent;
					bt.StyleId = colid.ToString();

					//stackRow.Children.Add(imginterior);
					stackRow.Children.Add(lbl);
					stackRow.Children.Add(lblvalue);
					//stackRow.Children.Add(bt);

					// store index to click

					contentlayout.Children.Add(stackRow);

					#region "Old arrow for each column stuff"
					//stackRow.StyleId = colid.ToString();
					//TapGestureRecognizer tap = new TapGestureRecognizer();
					//tap.Tapped += async (object sender, EventArgs e) =>
					//{                        
					//    AFButton item = (AFButton)sender;
					//    CRow qcell = (CRow)item.BindingContext;

					//    //CSystemObject sysobj = await m_ComosWeb.GetObject(m_Layer, qcell.Items[int.Parse(item.StyleId)].UID, language);
					//    //aways the first object to avoid arrows at all lines?
					//    CSystemObject sysobj = await m_ComosWeb.GetObject(m_Layer, qcell.Items[0].UID, language);
					//    CObject o = new CObject()
					//    {
					//        ClassType = sysobj.SystemType,
					//        Description = sysobj.Description,
					//        IsClientPicture = sysobj.IsClientPicture,
					//        Name = sysobj.Name,
					//        UID = sysobj.UID,
					//        OverlayUID = m_ProjectData.SelectedLayer.UID,
					//        Picture = sysobj.Picture,
					//        ProjectUID = m_ProjectData.SelectedProject.ProjectUID,
					//        SystemFullName = sysobj.Name,
					//    };

					//    PageSpecifications page = new PageSpecifications(m_Navigator, o, language);
					//    await this.Navigation.PushAsync(page);

					//};

					//contentlayout.GestureRecognizers.Add(tap);

					//stackRow.GestureRecognizers.Add(tap);
					//bt.GestureRecognizers.Add(tap);                    

					#endregion
				}


				TapGestureRecognizer tap = new TapGestureRecognizer();

				tap.Tapped += async (object sender, EventArgs e) =>
							{
								if (OnCellTaped == null)
								{
									StackLayout item = (StackLayout)sender;
									CRow qcell = (CRow)item.BindingContext;
									//aways the first object to avoid arrows at all lines?
									CSystemObject sysobj;
									try
									{
										sysobj = await m_ComosWeb.GetObject(m_Layer, qcell.Items[0].UID, language);
									}
									catch (Exception ex)
									{
										await App.Current.MainPage.DisplayAlert("Error", "Error al cargar objetos de Comos Web: " + ex.Message, Services.TranslateExtension.TranslateText("OK"));
										return;
									}

									if (sysobj == null)
										return;

									CObject o = new CObject()
									{
										ClassType = sysobj.SystemType,
										Description = sysobj.Description,
										IsClientPicture = sysobj.IsClientPicture,
										Name = sysobj.Name,
										UID = sysobj.UID,
										OverlayUID = m_ProjectData.SelectedLayer.UID,
										Picture = sysobj.Picture,
										ProjectUID = m_ProjectData.SelectedProject.ProjectUID,
										SystemFullName = sysobj.Name,
									};
									PageSpecifications page = new PageSpecifications(m_Navigator, o, language);
									await this.Navigation.PushAsync(page);
								}
								else
								{
									CellTaped(sender, e);
								}
							};

				contentlayout.GestureRecognizers.Add(tap);

				headerlayout.Children.Add(img);
				headerlayout.Children.Add(lblHeader);
				//headerlayout.Children.Add(btdoc);
				//headerlayout.Children.Add(btdev);


				layout.Children.Add(headerlayout);
				layout.Children.Add(contentlayout);

				TapGestureRecognizer headertap = new TapGestureRecognizer();
				headertap.Tapped += async (object sender, EventArgs e) =>
							{
								if (expandable)
								{
									contentlayout.IsVisible = !contentlayout.IsVisible;
									if (contentlayout.IsVisible)
									{
										contentlayout.FadeTo(1, 750, Easing.Linear);
									}
									else
									{
										await contentlayout.FadeTo(0, 300, Easing.Linear);
									}
									OnPropertyChanged("IsVisible");
								}
							};

				Frame frm = new Frame()
				{
					OutlineColor = (Color)Application.Current.Resources["ComosColorNavBarButton"],
					HasShadow = true,
					BackgroundColor = (Color)Application.Current.Resources["ComosColorModuleCard"],
					Margin = new Thickness(0, 0, 0, 5)
				};

				frm.Content = layout;
				frm.GestureRecognizers.Add(headertap);

				return new ViewCell { View = frm };

			});

			return template;

		}

		public void Nav_OnFiltered(object sender, EventArgs e)
		{
			if (view == QueryViewMode.Accordion)
			{
				listQuery.ItemsSource = nav.FilteredList;
			}
			else
			{
				CRow[] newRows = new CRow[nav.FilteredList.Count];
				int i = 0;

				foreach (CRow item in nav.FilteredList)
				{
					newRows[i] = new CRow { Items = item.Items, UID = item.UID };
					i = i + 1;
				}
				m_Query.Rows = newRows;
				ShowTableQuery();
			}

		}

		private void pickerFilter_SelectedIndexChanged(object sender, EventArgs e)
		{
			bool doRefresh = true;

			if (pickerIndex == -1)
				doRefresh = false;

			pickerIndex = pickerFilter.SelectedIndex;

			if (view == QueryViewMode.Accordion && doRefresh)
			{
				this.UpdateAccordion();
			}

			if (nav != null)
				nav.FullList = GetListToFilter();
		}
		private void UpdateAccordion()
		{
			listQuery = new ListView() { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, IsPullToRefreshEnabled = true };

			/* causing crash at android
			listQuery.RefreshCommand = new Command(async () =>
			{
					await UpdateQuery(m_Mainobject,m_Language);
					await ShowListQuery();
			});
			*/

			listQuery.ItemsSource = mainList;
			listQuery.HasUnevenRows = true;
			listQuery.ItemTemplate = CreateTemplateForRow(m_Query.Columns, listQuery, m_Language);
			queryGrid.Children.Clear();

			if (mainList.Count > 0)
				queryGrid.Children.Add(listQuery);

			if (m_Query.Columns.Count() == 1)
			{
				filter.IsVisible = false;
				filterdefinition.Height = 0;
			}
			else
			{
				filter.IsVisible = true;
				filterdefinition.Height = 35;
			}
		}

		private void Bt_Clicked(object sender, EventArgs e)
		{
			throw new NotImplementedException();
		}

		private async void listQuery_ItemTapped(object sender, ItemTappedEventArgs e)
		{

			CRow tapped = (CRow)e.Item;
			if (tapped.Items[0].UID != "")
			{
				CSystemObject sysobj;
				try
				{
					sysobj = await m_ComosWeb.GetObject(m_Layer, tapped.Items[0].UID, m_Language);
				}
				catch (Exception ex)
				{
					await App.Current.MainPage.DisplayAlert("Error", "Error al cargar objetos de Comos Web: " + ex.Message, Services.TranslateExtension.TranslateText("OK"));
					return;
				}

				if (sysobj == null)
					return;

				CObject o = new CObject()
				{
					ClassType = sysobj.SystemType,
					Description = sysobj.Description,
					IsClientPicture = sysobj.IsClientPicture,
					Name = sysobj.Name,
					UID = sysobj.UID,
					OverlayUID = m_Mainobject.OverlayUID,
					Picture = sysobj.Picture,
					ProjectUID = m_Mainobject.ProjectUID,
					SystemFullName = sysobj.Name,
				};

				PageSpecifications page = new PageSpecifications(m_Navigator, o, m_Language);
				this.Navigation.PushAsync(page);
			}

		}

		#region "Table Query"

		public async Task ShowTableQuery()
		{
			/*
			Device.BeginInvokeOnMainThread(() =>
			{
					//this.ShowSpinner("FetchingQuery Result ...");
			});

			this.Query = await m_ComosWeb.GetqueriesResult(
					m_Navigator, obj.ProjectUID, obj.OverlayUID, language, obj.UID, null);
			*/

			if (this.Query == null)
			{
				//this.HideSpinner();
				string text = Services.TranslateExtension.TranslateText("query_not_found");
				text = text.Replace("@", m_Mainobject.SystemFullName);
				await App.Current.MainPage.DisplayAlert("", text, "OK");
				return;
			}

			Device.BeginInvokeOnMainThread(() =>
			{
				Grid grid = new Grid();

				// Create the page content in code.
				for (int j = 0; j < m_Query.Columns.Length; j++)
				{
					ColumnDefinition def = new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto) };
					var column = m_Query.Columns[j];
					grid.ColumnDefinitions.Add(def);
				}

				grid.RowDefinitions.Add(
										 new RowDefinition()
										 { Height = new GridLength(1, GridUnitType.Auto) }
										 );
				for (int i = 0; i < m_Query.Columns.Length; i++)
				{
					var column = m_Query.Columns[i];

					grid.Children.Add(
								new ContentView()
								{
									Content = new Label()
									{
										Text = column.DisplayDescription,
										FontAttributes = FontAttributes.Bold,
									},
									Padding = new Thickness(10, 10, 10, 10),
									Margin = new Thickness(1, 1, 0, 0),
								}, i, 0);
				}

				//mainList.Clear();

				for (int i = 0; i < m_Query.Rows.Length; i++)
				{
					var row = m_Query.Rows[i];
					//mainList.Add(new QCell { Cells = row.Items, UID = row.UID });

					grid.RowDefinitions.Add(
						new RowDefinition()
						{
							Height = new GridLength(1, GridUnitType.Auto),
						});
				}
				for (int rowid = 0; rowid < m_Query.Rows.Length; rowid++)
				{
					var row = m_Query.Rows[rowid];
					for (int colid = 0; colid < m_Query.Columns.Length; colid++)
					{

						var cell = row.Items[colid];
						ContentView cellcontent = new ContentView();

						cellcontent.Content = new Label()
						{
							Text = cell.Text,
						};
						cellcontent.Margin = new Thickness(1, 1, 0, 0);
						cellcontent.Padding = new Thickness(10, 10, 10, 10);
						cellcontent.BackgroundColor = Color.White;

						TapGestureRecognizer tap = new TapGestureRecognizer();

						tap.Tapped += async (object sender, EventArgs e) =>
									{
										CSystemObject sysobj;
										try
										{
											sysobj = await m_ComosWeb.GetObject(m_Layer, cell.UID, m_Language);
										}
										catch (Exception ex)
										{
											await App.Current.MainPage.DisplayAlert("Error", "Error al cargar objetos de Comos Web: " + ex.Message, Services.TranslateExtension.TranslateText("OK"));
											return;
										}
										
										if (sysobj == null)
											return;

										CObject o = new CObject()
										{
											ClassType = sysobj.SystemType,
											Description = sysobj.Description,
											IsClientPicture = sysobj.IsClientPicture,
											Name = sysobj.Name,
											UID = sysobj.UID,
											OverlayUID = m_ProjectData.SelectedLayer.UID,
											Picture = sysobj.Picture,
											ProjectUID = m_ProjectData.SelectedProject.UID,
											SystemFullName = sysobj.Name,
										};

										PageSpecifications page = new PageSpecifications(m_Navigator, o, m_Language);
										this.Navigation.PushAsync(page);

									};
						cellcontent.GestureRecognizers.Add(tap);
						grid.Children.Add(cellcontent, colid, rowid + 1);

					}
				}
				grid.ColumnSpacing = 1;
				grid.RowSpacing = 1;
				grid.BackgroundColor = Color.FromRgb(190, 205, 215);
				grid.Padding = new Thickness(0, 0, 0, 0);
				grid.HorizontalOptions = LayoutOptions.StartAndExpand;
				grid.VerticalOptions = LayoutOptions.StartAndExpand;

				queryGrid.Children.Clear();
				queryGrid.Children.Add(grid);

				//  this.HideSpinner();
			});
		}

		#endregion

		CQuerieResult m_Query = null;
		public CQuerieResult Query
		{
			set
			{
				m_Query = value;

				columns.Clear();

				// Create the page content in code.
				for (int i = 0; i < m_Query.Columns.Length; i++)
				{
					var column = m_Query.Columns[i];
					columns.Add(column);
				}

				pickerFilter.ItemsSource = columns;
				pickerFilter.SelectedIndex = 0;
				m_Query.Rows = m_Query.Rows.OrderBy(x => x.Items[0].Text).ToArray();

				Device.BeginInvokeOnMainThread(() =>
				{
					OnPropertyChanged("Query");
				});
			}
			get
			{
				return m_Query;
			}
		}

		private async void Button_Clicked(object sender, EventArgs e)
		{


			switch (ViewMode)
			{
				case QueryViewMode.Accordion:
					await this.ShowTableQuery();
					ViewMode = controls.QueryControl.QueryViewMode.List;
					lbshow.Text = TABLE;
					break;
				case QueryViewMode.List:
					await this.ShowListQuery();
					ViewMode = controls.QueryControl.QueryViewMode.Accordion;
					lbshow.Text = LIST;
					break;
				default:
					break;
			}

		}
	}
}