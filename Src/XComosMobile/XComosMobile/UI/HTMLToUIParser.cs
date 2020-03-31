using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComosWebSDK;
using ComosWebSDK.Data;
using ComosWebSDK.UI;
using Xamarin.Forms;
using XComosMobile.Pages.comos;
using XComosMobile.Pages.controls;
using static Android.Widget.TimePicker;

namespace XComosMobile.UI
{
	public class HTMLToUIParser
	{
		Converters.PictureRefToImage converter = new Converters.PictureRefToImage();
		Converters.IsComosDocumentUIDToBool docconverter = new Converters.IsComosDocumentUIDToBool();
		Converters.IsComosDeviceUIDToBool devconverter = new Converters.IsComosDeviceUIDToBool();

		ViewModels.ProjectData ProjectData = Services.XServices.Instance.GetService<ViewModels.ProjectData>();
		Services.XDatabase cachedb = Services.XServices.Instance.GetService<Services.XDatabase>();
		Services.IPlatformSystem plataform = Services.XServices.Instance.GetService<Services.IPlatformSystem>();

		Dictionary<string, IBRServiceContracts.CWriteValue> values = new Dictionary<string, IBRServiceContracts.CWriteValue>();

		private List<ComosWebSDK.UI.UIBase> editablevalues = new List<ComosWebSDK.UI.UIBase>();
		public List<ComosWebSDK.UI.UIBase> EditableValues { get { return editablevalues; } }

		public CCachedDevice CachedDevice { get; set; }
		public Dictionary<string, IBRServiceContracts.CWriteValue> Values { get; set; }
		IComosWeb m_ComosWeb { get { return Services.XServices.Instance.GetService<ComosWebSDK.IComosWeb>(); } }

		Page m_Page = null;
		public CObject MainObject { get; set; }

		public string WebSystemUID
		{
			get
			{
				if (this.MainObject != null)
				{
					return this.MainObject.UID;
				}
				return "";
			}
		}

		public HTMLToUIParser(Page page)
		{
			m_Page = page;
			this.InitParser();
		}

		public HTMLToUIParser()
		{
			this.InitParser();
		}

		private void InitParser()
		{
			Values = new Dictionary<string, IBRServiceContracts.CWriteValue>();
			converter.InitConverter(ProjectData.SelectedDB.Key);
		}

		public View UpdateAttributesUI(ComosWebSDK.UI.UIQuery ui)
		{

			int rowIndex = 0;
			int columnIndex = 0;

			if (ui.Result == null || ui.Result.Rows == null || ui.Result.Rows.Length == 0)
				return null;

			var table = new Grid();

			foreach (var column in ui.Result.Columns)
			{
				table.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
			}

			foreach (var row in ui.Result.Rows)
			{
				table.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
			}

			if (ui.Result.Columns.Length > 1)
			{
				table.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

				foreach (var column in ui.Result.Columns)
				{
					UIGrid.AddChild(table, new Label()
					{
						Text = column.DisplayDescription,
						HorizontalOptions = LayoutOptions.FillAndExpand,
						VerticalOptions = LayoutOptions.FillAndExpand,
						FontAttributes = FontAttributes.Bold,
					}, rowIndex, columnIndex);
					columnIndex++;
				}

				rowIndex++;
				columnIndex = 0;
			}

			foreach (var row in ui.Result.Rows)
			{
				if (row.Items[0].Text != "")
				{
					var viewcell = new StackLayout()
					{
						Orientation = StackOrientation.Horizontal,
						VerticalOptions = LayoutOptions.FillAndExpand,
						HorizontalOptions = LayoutOptions.Fill,
						//Padding = new Thickness(10, 5),
						//BackgroundColor = (Color)Application.Current.Resources["ComosColorModuleCard"],
					};
					viewcell.SetDynamicResource(StackLayout.StyleProperty, "ComosQueryResult");

					FFImageLoading.Forms.CachedImage img = new FFImageLoading.Forms.CachedImage()
					{
						HeightRequest = 20,
						WidthRequest = 20,
						Source = converter.GetAsUrl("?keyid=" + row.Items[0].Picture),
					};

					Label lbl = new Label()
					{
						Text = row.Items[0].Text,
						HorizontalOptions = LayoutOptions.FillAndExpand,
						VerticalOptions = LayoutOptions.FillAndExpand,
					};

					viewcell.Children.Add(img);
					viewcell.Children.Add(lbl);
					viewcell.BindingContext = row;
					TapGestureRecognizer gesture = new TapGestureRecognizer();
					gesture.Tapped += ViewCell_DocumentTapped;
					viewcell.GestureRecognizers.Add(gesture);

					UIGrid.AddChild(table, viewcell, rowIndex, columnIndex);
					columnIndex++;

					for (int i = 1; i < row.Items.Length; i++)
					{
						UIGrid.AddChild(table, new Label()
						{
							Text = row.Items[i].Text,
							HorizontalOptions = LayoutOptions.FillAndExpand,
							VerticalOptions = LayoutOptions.FillAndExpand,
						}, rowIndex, columnIndex);
						columnIndex++;
					}

				}

				rowIndex++;
				columnIndex = 0;
			}

			table.SetDynamicResource(StackLayout.StyleProperty, "ComosQueryResult");
			table.ColumnSpacing = 20;
			table.BackgroundColor = (Color)Application.Current.Resources["ComosColorNavBarText"];

			ScrollView scroll = new ScrollView
			{
				Orientation = ScrollOrientation.Horizontal,
			};

			scroll.BackgroundColor = (Color)Application.Current.Resources["ComosColorNavBarText"];
			scroll.Content = table;
			/*var table = new StackLayout() {
					HorizontalOptions =LayoutOptions.Fill,
					VerticalOptions =LayoutOptions.StartAndExpand,
					BackgroundColor = (Color)Application.Current.Resources["ComosColorNavBar"],
					Spacing = 1,
			};

			if (ui.Result == null || ui.Result.Rows == null || ui.Result.Rows.Length == 0)
					return null;

			foreach (var row in ui.Result.Rows)
			{
					if (row.Items[0].Text != "")
					{                    
							var viewcell = new StackLayout()
							{
									Orientation = StackOrientation.Horizontal,
									VerticalOptions = LayoutOptions.FillAndExpand,
									HorizontalOptions = LayoutOptions.Fill,
									Padding = new Thickness(10,5),                 
									//BackgroundColor = (Color)Application.Current.Resources["ComosColorModuleCard"],
							};
							viewcell.SetDynamicResource(StackLayout.StyleProperty, "ComosQueryResult");

							FFImageLoading.Forms.CachedImage img = new FFImageLoading.Forms.CachedImage()
							{
									HeightRequest = 20,
									WidthRequest = 20,
									Source = converter.GetAsUrl("?keyid=" + row.Items[0].Picture),
							};
							viewcell.Children.Add(img);

							for(int i=0; i < row.Items.Length;i++)
							{
									viewcell.Children.Add(new Label()
									{
											Text = row.Items[i].Text,
											HorizontalOptions = LayoutOptions.FillAndExpand,
											VerticalOptions = LayoutOptions.FillAndExpand,
									});
							}

							viewcell.BindingContext = row;
							TapGestureRecognizer gesture = new TapGestureRecognizer();
							gesture.Tapped += ViewCell_DocumentTapped;
							viewcell.GestureRecognizers.Add(gesture);

							table.Children.Add(viewcell);
					}
			}*/
			//table.SizeChanged += Table_SizeChanged;
			return scroll;
		}

		private void Table_SizeChanged(object sender, EventArgs e)
		{
			TableView table = sender as TableView;
			table.HeightRequest = 20.0 + (App.FontSize + 3.0) * table.Root[0].Count;
		}

		private async void ViewCell_DocumentTapped(object sender, EventArgs e)
		{
			var viewcell = sender as StackLayout;
			var row = viewcell.BindingContext as CRow;

			await viewcell.ScaleTo(1.2, 50, Easing.Linear);
			await viewcell.ScaleTo(1, 50, Easing.Linear);

			if ((bool)docconverter.Convert(row.Items[0].UID, null, null, null))
			{
				if (plataform.IsOnline)
				{
					plataform.ShowProgressMessage(Services.TranslateExtension.TranslateText("downloading_documents"), true);
					//string[] filename_andtype = await DownloadDocument(row.Items[0].UID);
					CDocument filename_andtype = await DownloadDocument(row.Items[0].UID);

					if (filename_andtype != null && !string.IsNullOrEmpty(filename_andtype.FileName))
						cachedb.CacheDocumentFilePath(filename_andtype.FileName, filename_andtype.MimeType, row.Items[0].UID, ProjectData.SelectedProject.UID, ProjectData.SelectedLayer.UID, filename_andtype.Name, filename_andtype.Description, filename_andtype.Picture);
					if (filename_andtype == null)
					{
						plataform.ShowToast(Services.TranslateExtension.TranslateText("document_not_found"));
					}
					if (string.IsNullOrEmpty(filename_andtype.FileName))
					{
						// Can happen in emulator. When folder /SDCard/Storage/Download not exists.
						plataform.ShowToast(Services.TranslateExtension.TranslateText("save_not_possible"));
					}

					plataform.HideProgressMessage();
				}
				else
				{
					string[] path = cachedb.GetDocumentFilePath(row.Items[0].UID, ProjectData.SelectedProject.UID, ProjectData.SelectedLayer.UID);
					if (path != null && path[0] != "")
					{
						bool result = await plataform.OpenFile(path[0], path[1]);
					}
					//else doc not found in cache

				}
			}
			else if ((bool)devconverter.Convert(row.Items[0].UID, null, null, null))
			{
				// open device properties

				if (m_Page != null)
				{
					CSystemObject sysobj;
					try
					{
						sysobj = await m_ComosWeb.GetObject(ProjectData.SelectedLayer, row.Items[0].UID, ProjectData.SelectedLanguage.LCID);
					}
					catch (Exception ex)
					{
						await App.Current.MainPage.DisplayAlert("Error", "Error al cargar objetos de Comos Web: " + ex.Message, Services.TranslateExtension.TranslateText("OK"));
						return;
					}

					if (sysobj != null)
					{

						CObject o = new CObject()
						{
							ClassType = sysobj.SystemType,
							Description = sysobj.Description,
							IsClientPicture = sysobj.IsClientPicture,
							Name = sysobj.Name,
							UID = sysobj.UID,
							OverlayUID = ProjectData.SelectedLayer.UID,
							Picture = sysobj.Picture,
							ProjectUID = ProjectData.SelectedProject.ProjectUID,
							SystemFullName = sysobj.Name,
						};
						PageSpecifications page = new PageSpecifications(ProjectData.SelectedDB.Key, o, ProjectData.SelectedLanguage.LCID);

						await m_Page.Navigation.PushAsync(page);

					}
				}

			}

		}

		private async Task<CDocument> DownloadDocument(string UID)
		{
			CheckAndDownloadAditionalContent downloader = new CheckAndDownloadAditionalContent();
			return await downloader.DownloadDocument(UID, true);

		}

		public View UpdateAttributesUI(ComosWebSDK.UI.UIBase b)
		{
			View elm = null;
			//creating ui items

			if (b == null)
				return null;

			if (CachedDevice != null)
			{
				IBRServiceContracts.CWriteValue value = CachedDevice.ValueCollection.Values.FirstOrDefault(x => x.NestedName == b.NestedName);
				if (value != null)
				{
					b.CachedValue = value.NewValue;
					Values.Add(b.NestedName, value);
				}
			}
			else if (MainObject != null)
			{
				string project = ProjectData.SelectedProject.UID;
				string workinglayer = ProjectData.SelectedLayer.UID;
				b.CachedValue = cachedb.ReadCacheSpecValue(MainObject.UID, b.NestedName, project, workinglayer, b.Text);
			}

			if (b is ComosWebSDK.UI.UIFrame)
			{
				elm = UpdateAttributesUI(b as ComosWebSDK.UI.UIFrame);
			}
			else if (b is ComosWebSDK.UI.UICheckBox)
			{
				elm = UpdateAttributesUI(b as ComosWebSDK.UI.UICheckBox);
			}
			else if (b is ComosWebSDK.UI.UIDate)
			{
				elm = UpdateAttributesUI(b as ComosWebSDK.UI.UIDate);
			}
			else if (b is ComosWebSDK.UI.UIDateTime)
			{
				elm = UpdateAttributesUI(b as ComosWebSDK.UI.UIDateTime);
			}
			else if (b is ComosWebSDK.UI.UIEdit)
			{
				elm = UpdateAttributesUI(b as ComosWebSDK.UI.UIEdit);
			}
			else if (b is ComosWebSDK.UI.UIMemo)
			{
				elm = UpdateAttributesUI(b as ComosWebSDK.UI.UIMemo);
			}
			else if (b is ComosWebSDK.UI.UIQuery)
			{
				elm = UpdateAttributesUI(b as ComosWebSDK.UI.UIQuery);
			}
			else if (b is ComosWebSDK.UI.UIOptions)
			{
				elm = UpdateAttributesUI(b as ComosWebSDK.UI.UIOptions);
			}
			else if (b is ComosWebSDK.UI.UILabel)
			{
				elm = UpdateAttributesUI(b as ComosWebSDK.UI.UILabel);
			}
			return elm;
		}
		public View UpdateAttributesUI(ComosWebSDK.UI.UICheckBox ui)
		{
			StackLayout layout = new StackLayout()
			{
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand
			};

			if (!ui.ReadOnly)
				editablevalues.Add(ui);

			Converters.TextToBoolean c = new Converters.TextToBoolean();

			if (ui.CachedValue == null)
			{
				ui.CachedValue = ui.Value.ToString();
			}

			bool cachedvalue = (bool)c.Convert(ui.CachedValue, null, null, null);
			bool value = (bool)c.Convert(ui.Value, null, null, null);

			if ((cachedvalue != value) && (ui.CachedValue != ""))
			{
				value = cachedvalue;
				CheckValue(ui.NestedName, ui.CachedValue, c.ConvertBack(value, null, null, null).ToString(), ui.Text);
			}

			Switch elm = new Switch()
			{
				IsToggled = value,
				BindingContext = ui,
				IsEnabled = !ui.ReadOnly,
				HorizontalOptions = LayoutOptions.Fill,

			};

			// color?
			//if (value != ui.Value)
			//    elm.BackgroundColor = Color.Blue;

			var lbl = new Label()
			{
				HorizontalTextAlignment = TextAlignment.Start,
				VerticalTextAlignment = TextAlignment.Center,
				WidthRequest = App.WidthPixels / 3,
			};
			lbl.Text = ui.Text;

			elm.Toggled += SwitchToggled;

			layout.Children.Add(lbl);
			layout.Children.Add(elm);
			AbsoluteLayout.SetLayoutBounds(layout, new Rectangle(
				ui.x, ui.y, ui.width, ui.height));
			return layout;
		}

		public View UpdateAttributesUI(ComosWebSDK.UI.UILabel ui)
		{
			StackLayout layout = new StackLayout()
			{
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,

			};

			var lbl = new Label()
			{
				Text = ui.Text,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Start,
			};

			layout.Children.Add(lbl);

			AbsoluteLayout.SetLayoutBounds(layout, new Rectangle(
				 ui.x, ui.y, ui.width, ui.height));

			return layout;
		}
		public View UpdateAttributesUI(ComosWebSDK.UI.UIMemo ui)
		{
			StackLayout layout = new StackLayout()
			{
				Orientation = StackOrientation.Vertical,
			};

			if (!ui.ReadOnly)
				editablevalues.Add(ui);

			if (ui.ShowLabel)
			{
				var lbl = new Label()
				{
					Text = ui.Text,
					WidthRequest = App.WidthPixels / 3,
					VerticalTextAlignment = TextAlignment.Center,
					HorizontalTextAlignment = TextAlignment.Start,
				};

				layout.Children.Add(lbl);
			}

			if (ui.ReadOnly)
			{
				var txt = new Label()
				{
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.FillAndExpand,
					Text = ui.Value,
				};
				layout.Children.Add(txt);
			}
			else
			{
				var txt = new CustomEditor()
				{
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.FillAndExpand,
					BindingContext = ui,
					Keyboard = Keyboard.Chat,
				};

				//txt.HeightRequest = ui.height /  (App.PixelDensity * 3);

				txt.HeightRequest = 120.00;

				if (ui.CachedValue != "" && ui.CachedValue != ui.Value)
				{
					txt.Text = ui.CachedValue;
					txt.TextColor = (Color)App.Current.Resources["spec-only-cache"];

					CheckValue(ui.NestedName, ui.CachedValue, ui.Value, ui.Text);
				}
				else
				{
					txt.Text = ui.Value;
				}

				txt.TextChanged += Txt_TextChangedEditor;
				layout.Children.Add(txt);
			}

			AbsoluteLayout.SetLayoutBounds(layout, new Rectangle(
				 ui.x, ui.y, ui.width, ui.height));
			return layout;
		}

		public View UpdateAttributesUI(ComosWebSDK.UI.UIDate ui)
		{
			if (!ui.ReadOnly)
				editablevalues.Add(ui);

			StackLayout layout = new StackLayout()
			{
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.FillAndExpand
			};

			var lbl = new Label()
			{
				Text = ui.Text,
				WidthRequest = App.WidthPixels / 3,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Start,
			};
			layout.Children.Add(lbl);

			if (ui.ReadOnly)
			{
				var txt = new Label()
				{
					Text = ui.Value,
				};
				layout.Children.Add(txt);
			}
			else
			{

				var date = new DatePicker()
				{
					HorizontalOptions = LayoutOptions.FillAndExpand,
					BindingContext = ui,
				};
				

				if (ui.CachedValue != "" && ui.CachedValue != ui.Value && ui.CachedValue != null)
				{
					date.Date = Convert.ToDateTime(ui.CachedValue);
					date.TextColor = (Color)App.Current.Resources["spec-only-cache"];
					CheckValue(ui.NestedName, ui.CachedValue, ui.Value, ui.Text);
				}
				else
				{
					if (!ui.Value.Equals(""))
					{
						date.Date = Convert.ToDateTime(ui.Value);
						DateTime dateTime = new DateTime(date.Date.Year, date.Date.Month, date.Date.Day);
						string comosDate = dateTime.ToOADate().ToString();
						CheckValue(ui.NestedName, comosDate, ui.Value, ui.Text);
					}
					else
					{
						CheckValue(ui.NestedName, "", ui.Value, ui.Text);
					}
				}

				date.DateSelected += Date_ValueChanged;

				layout.Children.Add(date);
			}

			AbsoluteLayout.SetLayoutBounds(layout, new Rectangle(
			   ui.x, ui.y, ui.width, ui.height));
			return layout;
		}

		public View UpdateAttributesUI(ComosWebSDK.UI.UIDateTime ui)
		{
			if (!ui.ReadOnly)
				editablevalues.Add(ui);

			StackLayout layout = new StackLayout()
			{
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.FillAndExpand
			};

			var lbl = new Label()
			{
				Text = ui.Text,
				WidthRequest = App.WidthPixels / 3,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Start,
			};
			layout.Children.Add(lbl);

			if (ui.ReadOnly)
			{
				var txt = new Label()
				{
					Text = ui.Value,
				};
				layout.Children.Add(txt);
			}
			else
			{

				var date = new DatePicker()
				{
					HorizontalOptions = LayoutOptions.FillAndExpand,
					BindingContext = ui,
				};

				var time = new TimePicker()
				{
					HorizontalOptions = LayoutOptions.End,
					BindingContext = ui,
				};

				if (ui.CachedValue != "" && ui.CachedValue != ui.Value && ui.CachedValue != null)
				{
					string[] dateAndTime = ui.CachedValue.Split(' ');
					date.Date = Convert.ToDateTime(dateAndTime[0]);
					time.Time = TimeSpan.Parse(dateAndTime[1]);
					date.TextColor = (Color)App.Current.Resources["spec-only-cache"];
					time.TextColor = (Color)App.Current.Resources["spec-only-cache"];

					DateTime dateTime = new DateTime(date.Date.Year, date.Date.Month, date.Date.Day, time.Time.Hours, time.Time.Minutes, time.Time.Seconds);
					string comosDate = dateTime.ToOADate().ToString();
					CheckValue(ui.NestedName, comosDate, "", ui.Text);
				}
				else
				{
					if (!ui.Value.Equals(""))
					{
						string[] dateAndTime = ui.Value.Split(' ');
						DateTime _date = Convert.ToDateTime(dateAndTime[0]);
						TimeSpan _time = TimeSpan.Parse(dateAndTime[1]);

						DateTime dateTime = (new DateTime(_date.Year, _date.Month, _date.Day, _time.Hours, _time.Minutes, _time.Seconds)).ToLocalTime();
						date.Date = dateTime.Date;
						time.Time = dateTime.TimeOfDay;
						string comosDate = dateTime.ToOADate().ToString();
						CheckValue(ui.NestedName, comosDate, comosDate, ui.Text);
					}
					else
					{
						CheckValue(ui.NestedName, "", ui.Value, ui.Text);
					}
				}

				date.DateSelected += Date_DateTime_ValueChanged;
				time.PropertyChanged += Time_DateTime_ValueChanged;
				layout.Children.Add(date);
				layout.Children.Add(time);
			}

			AbsoluteLayout.SetLayoutBounds(layout, new Rectangle(
			   ui.x, ui.y, ui.width, ui.height));
			return layout;
		}

		public View UpdateAttributesUI(ComosWebSDK.UI.UIEdit ui)
		{
			if (!ui.ReadOnly)
				editablevalues.Add(ui);

			StackLayout layout = new StackLayout()
			{
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				//VerticalOptions = LayoutOptions.FillAndExpand,
			};

			var lbl = new Label()
			{
				Text = ui.Text,
				WidthRequest = App.WidthPixels / 3,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Start,
			};
			layout.Children.Add(lbl);

			if (ui.HasUnit && ui.Unit == "%")
			{
				if (ui.Value == "")
					ui.Value = "0";

				// Use a slider bar.
				Slider slider = new Slider
				{
					Minimum = 0,

					Maximum = 100,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					BindingContext = ui,
					// work around xamarin slider bug https://bugzilla.xamarin.com/show_bug.cgi?id=54887
					Value = double.Parse(ui.Value),
					IsEnabled = !ui.ReadOnly,
				};
				slider.ValueChanged += Slider_ValueChanged;
				var lblpercent = new Label()
				{
					Text = ui.Value,
					VerticalTextAlignment = TextAlignment.Center,
					BindingContext = slider,
				};
				lblpercent.SetBinding(
						Label.TextProperty,
						new Binding("Value", BindingMode.OneWay,
						null, null, "{0}"));
				// Try out work around xamarin slider bug 
				// https://bugzilla.xamarin.com/show_bug.cgi?id=54887
				// Does not work
				//   slider.Value = slider.Value + 0.1;

				if (ui.CachedValue != "" && ui.CachedValue != ui.Value)
				{
					slider.Value = int.Parse(ui.CachedValue);
				}

				layout.Children.Add(slider);
				layout.Children.Add(lblpercent);
			}
			else
			{
				if (ui.ReadOnly)
				{
					var txt = new Label()
					{
						Text = ui.Value,
					};
					layout.Children.Add(txt);
				}
				else
				{
					var txt = new CustomEntry()
					{
						Text = ui.Value,
						WidthRequest = App.WidthPixels*(2/3),
						HorizontalOptions = LayoutOptions.FillAndExpand,
						BindingContext = ui,
					};

					if (ui.CachedValue != "" && ui.CachedValue != ui.Value)
					{
						txt.Text = ui.CachedValue;
						txt.TextColor = (Color)App.Current.Resources["spec-only-cache"];
						CheckValue(ui.NestedName, ui.CachedValue, ui.Value, ui.Text);
					}
					else
					{
						txt.Text = ui.Value;
					}

					txt.TextChanged += Txt_TextChanged;
					layout.Children.Add(txt);
				}

				if (ui.HasUnit)
				{
					layout.Children.Add(new Label()
					{
						Text = ui.Unit,
					});
				}
			}


			AbsoluteLayout.SetLayoutBounds(layout, new Rectangle(
				 ui.x, ui.y, ui.width, ui.height));
			return layout;
		}

		public void SwitchToggled(object sender, ToggledEventArgs e)
		{
			Switch ctrl = (Switch)sender;
			ComosWebSDK.UI.UICheckBox b = (ComosWebSDK.UI.UICheckBox)ctrl.BindingContext;

			IBRServiceContracts.CWriteValue value = null;
			if (!Values.TryGetValue(b.NestedName, out value))
			{
				value = new IBRServiceContracts.CWriteValue()
				{
					NestedName = b.NestedName,
					NewValue = e.Value ? "1" : "0",
					OldValue = b.Value ? "1" : "0",
				};
				Values.Add(b.NestedName, value);
			}
			else
			{
				value.NewValue = e.Value ? "1" : "0";
			}

			CheckValue(b.NestedName, value.NewValue, value.OldValue, b.Text);
		}

		public void PickerSelectionChanged(object sender, EventArgs e)
		{
			Picker ctrl = (Picker)sender;
			ComosWebSDK.UI.UIOptions b = (ComosWebSDK.UI.UIOptions)ctrl.BindingContext;

			string myPickerNewValue = ctrl.SelectedItem.ToString();
			//If picked option does not exist, create new entry in b.Options
			if (!b.Options.TryGetValue(myPickerNewValue, out myPickerNewValue))
			{
				myPickerNewValue = ctrl.SelectedItem.ToString();
				CheckValue(b.NestedName, myPickerNewValue, b.Value, b.Text);
			}
			else
			{
				CheckValue(b.NestedName, b.Options[ctrl.SelectedItem.ToString()], b.Value, b.Text);
			}

		}

		public void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
		{
			Slider ctrl = (Slider)sender;
			ComosWebSDK.UI.UIEdit b = (ComosWebSDK.UI.UIEdit)ctrl.BindingContext;

			var step = Math.Round(e.NewValue / 20);
			ctrl.Value = step * 20;

			CheckValue(b.NestedName, ((int)e.NewValue).ToString(), b.Value, b.Text);

		}

		public void Txt_TextChanged(object sender, TextChangedEventArgs e)
		{
			CustomEntry ctrl = (CustomEntry)sender;
			string nestedname = "";
			string oldvalue = "";
			string description = "";

			ComosWebSDK.UI.UIEdit b = (ComosWebSDK.UI.UIEdit)ctrl.BindingContext;
			nestedname = b.NestedName;
			oldvalue = b.Value;
			description = b.Text;

			CheckValue(nestedname, e.NewTextValue, oldvalue, description);
		}

		public void Txt_TextChangedEditor(object sender, TextChangedEventArgs e)
		{
			Editor ctrl = (Editor)sender;
			string nestedname = "";
			string oldvalue = "";
			string description = "";

			ComosWebSDK.UI.UIMemo b = (ComosWebSDK.UI.UIMemo)ctrl.BindingContext;
			nestedname = b.NestedName;
			oldvalue = b.Value;
			description = b.Text;

			CheckValue(nestedname, e.NewTextValue, oldvalue, description);
		}

		public void Date_ValueChanged(object sender, DateChangedEventArgs e)
		{
			DatePicker ctrl = (DatePicker)sender;
			string nestedname = "";
			string oldvalue = "";
			string description = "";

			ComosWebSDK.UI.UIDate b = (ComosWebSDK.UI.UIDate)ctrl.BindingContext;
			nestedname = b.NestedName;
			oldvalue = b.Value;
			description = b.Text;

			string comosDate = e.NewDate.ToOADate().ToString();

			//if (ctrl.NullableDate == null)
				//CheckValue(nestedname, "", oldvalue, description);
			//else
				CheckValue(nestedname, comosDate, oldvalue, description);
		}

		public void Date_DateTime_ValueChanged(object sender, DateChangedEventArgs e)
		{
			DatePicker date = (DatePicker)sender;
			string nestedname = "";
			string oldvalue = "";
			string description = "";
			TimePicker time = new TimePicker();

			StackLayout stackLayout = (StackLayout)date.Parent;
			
			foreach(var element in stackLayout.Children)
			{
				if (element.GetType() == typeof(TimePicker))
					time = (TimePicker)element;
			}

			ComosWebSDK.UI.UIDateTime b = (ComosWebSDK.UI.UIDateTime)date.BindingContext;
			nestedname = b.NestedName;
			oldvalue = b.Value;
			description = b.Text;

			DateTime dateTime = new DateTime(e.NewDate.Year, e.NewDate.Month, e.NewDate.Day, time.Time.Hours, time.Time.Minutes, time.Time.Seconds);
			string comosDate = dateTime.ToOADate().ToString();

			CheckValue(nestedname, comosDate, oldvalue, description);
		}

		public void Time_DateTime_ValueChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			TimePicker time = (TimePicker)sender;
			string nestedname = "";
			string oldvalue = "";
			string description = "";
			DatePicker date = new DatePicker();

			StackLayout stackLayout = (StackLayout)time.Parent;

			foreach (var element in stackLayout.Children)
			{
				if (element.GetType() == typeof(DatePicker))
					date = (DatePicker)element;
			}

			ComosWebSDK.UI.UIDateTime b = (ComosWebSDK.UI.UIDateTime)time.BindingContext;
			nestedname = b.NestedName;
			oldvalue = b.Value;
			description = b.Text;

			DateTime dateTime = new DateTime(date.Date.Year, date.Date.Month, date.Date.Day, time.Time.Hours, time.Time.Minutes, time.Time.Seconds);
			string comosDate = dateTime.ToOADate().ToString();

			CheckValue(nestedname, comosDate, oldvalue, description);
		}

		public void CheckValue(string nestedname, string newvalue, string oldvalue, string description = "")
		{
			if (newvalue == null)
				newvalue = "";

			if (oldvalue == null)
				oldvalue = "";

			IBRServiceContracts.CWriteValue value = null;
			if (!Values.TryGetValue(nestedname, out value))
			{
				value = new IBRServiceContracts.CWriteValue()
				{
					NestedName = nestedname,
					NewValue = newvalue,
					OldValue = oldvalue,
					Description = description,
					WebSystemUID = this.WebSystemUID
				};
				Values.Add(nestedname, value);
			}
			else
			{
				value.NewValue = newvalue;
				value.NestedName = nestedname;
				value.OldValue = oldvalue;
				value.Description = description;
				value.WebSystemUID = this.WebSystemUID;
			}
		}

		public View UpdateAttributesUI(ComosWebSDK.UI.UIFrame frame)
		{

			if (frame.Children == null || frame.Children.Length == 0)
				return null;

			Frame f = new Frame()
			{
				BackgroundColor = (Color)App.Current.Resources["ComosColorModuleCard"],
				OutlineColor = (Color)App.Current.Resources["ComosColorNavBar"],
				CornerRadius = 8,
				VerticalOptions = LayoutOptions.FillAndExpand,
			};
			//AbsoluteLayout.SetLayoutBounds(f, new Rectangle(
			//    frame.x, frame.y, frame.width, frame.height));
			StackLayout stack = new StackLayout();

			if (frame.Children != null)
			{
				var lbl = new Label()
				{
					Text = frame.Text,
					FontAttributes = FontAttributes.Bold
				};
				lbl.SetDynamicResource(Label.TextColorProperty, "ComosColorNavBar");
				stack.Children.Add(lbl);
				foreach (var c in frame.Children)
				{
					var childview = UpdateAttributesUI(c);
					if (childview != null)
						stack.Children.Add(childview);

				}
			}

			if (stack.Children.Count == 1)
				return null;

			f.Content = stack;

			return f;
		}

		public View UpdateAttributesUI(ComosWebSDK.UI.UIOptions ui)
		{
			StackLayout stack = new StackLayout()
			{
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.FillAndExpand,
			};

			if (!ui.ReadOnly)
				editablevalues.Add(ui);

			AbsoluteLayout.SetLayoutBounds(stack, new Rectangle(
				 ui.x, ui.y, ui.width, ui.height));

			stack.Children.Add(new Label()
			{
				Text = ui.Text,
				WidthRequest = App.WidthPixels / 3,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Start,
			});

			CustomPicker p = new CustomPicker()
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				IsEnabled = !ui.ReadOnly,
			};
			foreach (var c in ui.Options.Keys)
			{
				p.Items.Add(c);
			}

			p.BindingContext = ui;

			if (ui.CachedValue != "" && ui.CachedValue != ui.Value)
			{
				p.SelectedItem = ui.CachedValue;
				p.TextColor = (Color)App.Current.Resources["spec-only-cache"];

				CheckValue(ui.NestedName, ui.CachedValue, ui.Value);
			}
			else
			{
				p.SelectedItem = ui.Value;
			}

			p.SelectedIndexChanged += PickerSelectionChanged;
			stack.Children.Add(p);

			return stack;
		}

	}
}
