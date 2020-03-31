using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComosWebSDK;
using ComosWebSDK.Data;
using ComosWebSDK.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using System.Net;
using ModernHttpClient;

namespace XComosMobile.Pages.maintenance
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WorkPackageDetail : PageTemplate
    {
        IComosWeb m_ComosWeb { get { return Services.XServices.Instance.GetService<ComosWebSDK.IComosWeb>(); } }
        ViewModels.ProjectData ProjectData
        {
            get { return Services.XServices.Instance.GetService<ViewModels.ProjectData>(); }
        }

        string SystemUID = null;
        public string Database { get; set; }
        public string Language { get; set; }
        public WorkPackageDetail(CSpecification spec, string objuid)
        {
            InitializeComponent();
            this.BindingContext = this;
            SystemUID = objuid;

            /*
			this.Icon = obj.Picture;
			this.Title = obj.Name;
			this.MainObject = obj;
			this.BindingContext = this;
			this.Database = database;
			this.Language = language;

			this.ShowAttributeView = true;
			this.CheckFavorite();
			NavigationBarControl.OnSaved += NavigationBarControl_OnSaved;

			*/

            UpdateAttributesUI(spec, objuid);
        }

        ComosWebSDK.UI.UIBase[] m_Attributes = null;

        #region Using grid layout

        public async Task UpdateAttributesUI(CSpecification spec, string objUID)
        {
            StackLayout stack = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Vertical,
            };

            string html;
            try
            {
                html = await m_ComosWeb.GetObjectSpecificationAsHtml(
                        this.ProjectData.SelectedDB.Key, spec.ProjectUID, spec.OverlayUID, ProjectData.SelectedLanguage.LCID, objUID, spec.Name);

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Error al cargar atributos: " + ex.Message, Services.TranslateExtension.TranslateText("OK"));
                return;
            }

            if (html == null)
                return;

            m_Attributes = CHtmlParser.ParseAttributesForUI(html);

            foreach (var attribute in m_Attributes)
            {
                var elm = await UpdateAttributesUI(attribute);
                if (elm == null)
                    continue;
                stack.Children.Add(elm);
            }


            stack.Padding = new Thickness(0, 0, 0, 0);
            stack.HorizontalOptions = LayoutOptions.FillAndExpand;
            stack.VerticalOptions = LayoutOptions.FillAndExpand;


            this.m_Panel.Content = stack;
        }

        private async Task<View> UpdateAttributesUI(ComosWebSDK.UI.UIBase b)
        {
            View elm = null;
            if (b is ComosWebSDK.UI.UIFrame)
            {
                elm = await UpdateAttributesUI(b as ComosWebSDK.UI.UIFrame);
            }
            else if (b is ComosWebSDK.UI.UICheckBox)
            {
                elm = UpdateAttributesUI(b as ComosWebSDK.UI.UICheckBox);
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
                elm = await UpdateAttributesUI(b as ComosWebSDK.UI.UIQuery);
            }
            else if (b is ComosWebSDK.UI.UIOptions)
            {
                elm = UpdateAttributesUI(b as ComosWebSDK.UI.UIOptions);
            }
            return elm;
        }

        private async Task<View> UpdateAttributesUI(ComosWebSDK.UI.UIQuery ui)
        {
            StackLayout layout = new StackLayout()
            {
                //Orientation = StackOrientation.Horizontal,
            };

            if (ui.Text != "")
            {
                var lbl = new Label()
                {
                    Text = ui.Text,
                    WidthRequest = ui.WidthLabel,
                    HorizontalTextAlignment = TextAlignment.End,
                };
                layout.Children.Add(lbl);
            }

            controls.QueryControl tmp = new controls.QueryControl()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Start,
            };

            tmp.SetMainObject(MainObject);


            /*
			var task = m_ComosWeb.GetqueriesResult(
			Database, MainObject.ProjectUID, MainObject.OverlayUID, Language, ui.QueryUID, ui.Owner);

			task.Wait();
			var query = task.Result;

			tmp.Query = query;
			await tmp.ShowListQuery();
			layout.Children.Add(tmp);

			AbsoluteLayout.SetLayoutBounds(layout, new Rectangle(
			ui.x, ui.y, ui.width, ui.height));

			tmp.HeightRequest = (ui.height) + 50;

	*/

            return layout;
        }

        public View UpdateAttributesUI(ComosWebSDK.UI.UIMemo ui)
        {
            StackLayout layout = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
            };

            if (ui.ShowLabel)
            {
                var lbl = new Label()
                {
                    Text = ui.Text,
                    WidthRequest = ui.WidthLabel,
                    HorizontalTextAlignment = TextAlignment.End,
                };

                layout.Children.Add(lbl);
            }

            // editable text
            var txt = new Entry()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                BindingContext = ui,
                IsEnabled = !ui.ReadOnly,
            };

            if (ui.CachedValue != "")
            {
                txt.Text = ui.CachedValue;
                txt.BackgroundColor = Color.Blue;
                //lbl.BackgroundColor = Color.Blue;
            }
            else
            {
                txt.Text = ui.Value;
            }

            //txt.TextChanged += Txt_TextChanged;
            layout.Children.Add(txt);

            AbsoluteLayout.SetLayoutBounds(layout, new Rectangle(
                 ui.x, ui.y, ui.width, ui.height));
            return layout;
        }
        private View UpdateAttributesUI(ComosWebSDK.UI.UICheckBox ui)
        {
            StackLayout layout = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
            };
            Switch elm = new Switch()
            {
                IsToggled = ui.Value,
                IsEnabled = !ui.ReadOnly,
            };
            elm.BindingContext = ui;
            elm.Toggled += SwitchToggled;
            var lbl = new Label()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center,
                WidthRequest = ui.WidthLabel / (App.PixelDensity * 0.8),
            };
            lbl.Text = ui.Text;

            layout.Children.Add(lbl);
            layout.Children.Add(elm);
            AbsoluteLayout.SetLayoutBounds(layout, new Rectangle(
                ui.x, ui.y, ui.width, ui.height));
            return layout;
        }
        private View UpdateAttributesUI(ComosWebSDK.UI.UIEdit ui)
        {
            StackLayout layout = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
            };

            var lbl = new Label()
            {
                Text = ui.Text,
                WidthRequest = ui.WidthLabel / (App.PixelDensity * 0.8),
                HorizontalTextAlignment = TextAlignment.End,
                VerticalTextAlignment = TextAlignment.Center,
            };
            layout.Children.Add(lbl);

            if (ui.HasUnit && ui.Unit == "%")
            {
                // Use a slider bar.
                Slider slider = new Slider
                {
                    Minimum = 0,
                    Maximum = 100,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    BindingContext = ui,
                    Value = int.Parse(ui.Value),
                    IsEnabled = !ui.ReadOnly,
                };
                slider.ValueChanged += Slider_ValueChanged;
                layout.Children.Add(slider);
            }
            else
            {
                var txt = new Entry()
                {
                    Text = ui.Value,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    BindingContext = ui,
                    IsEnabled = !ui.ReadOnly,
                };
                txt.TextChanged += Entry_TextChanged;
                layout.Children.Add(txt);
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
        private async Task<View> UpdateAttributesUI(ComosWebSDK.UI.UIFrame frame)
        {
            Frame f = new Frame()
            {
                OutlineColor = Color.Black,
                CornerRadius = 4,
            };
            AbsoluteLayout.SetLayoutBounds(f, new Rectangle(
                    frame.x, frame.y, frame.width, frame.height));
            StackLayout stack = new StackLayout();
            stack.Children.Add(new Label() { Text = frame.Text, FontAttributes = FontAttributes.Bold });
            if (frame.Children != null)
            {
                foreach (var c in frame.Children)
                {
                    var childview = await UpdateAttributesUI(c);
                    stack.Children.Add(childview);
                }
            }
            f.Content = stack;
            return f;
        }
        private View UpdateAttributesUI(ComosWebSDK.UI.UIOptions ui)
        {
            StackLayout stack = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            AbsoluteLayout.SetLayoutBounds(stack, new Rectangle(
                 ui.x, ui.y, ui.width, ui.height));

            stack.Children.Add(new Label()
            {
                Text = ui.Text,
                WidthRequest = ui.WidthLabel / (App.PixelDensity * 0.8),
                HorizontalTextAlignment = TextAlignment.End,
                VerticalTextAlignment = TextAlignment.Center,
            });

            Picker p = new Picker() { HorizontalOptions = LayoutOptions.FillAndExpand, IsEnabled = !ui.ReadOnly, };
            foreach (var c in ui.Options.Keys)
            {
                p.Items.Add(c);
            }
            p.SelectedItem = ui.Value;
            p.SelectedIndexChanged += PickerSelectionChanged;
            stack.Children.Add(p);

            return stack;
        }

        #endregion

        #region User changes implementation

        Dictionary<string, IBRServiceContracts.CWriteValue> Values = new Dictionary<string, IBRServiceContracts.CWriteValue>();

        // Service public TResult<CWriteValueResult[]> WriteComosValues(CWriteValueCollection values, string user, string projectname, string workinglayer, string language)

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            Entry ctrl = (Entry)sender;
            ComosWebSDK.UI.UIEdit b = (ComosWebSDK.UI.UIEdit)ctrl.BindingContext;

            IBRServiceContracts.CWriteValue value = null;
            if (!Values.TryGetValue(b.NestedName, out value))
            {
                value = new IBRServiceContracts.CWriteValue()
                {
                    NestedName = b.NestedName,
                    NewValue = e.NewTextValue,
                    OldValue = b.Value,
                    WebSystemUID = this.SystemUID,
                };
                Values.Add(b.NestedName, value);
            }
            else
            {
                value.NewValue = e.NewTextValue;
            }
        }

        private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            Slider ctrl = (Slider)sender;
            ComosWebSDK.UI.UIEdit b = (ComosWebSDK.UI.UIEdit)ctrl.BindingContext;

            IBRServiceContracts.CWriteValue value = null;
            if (!Values.TryGetValue(b.NestedName, out value))
            {
                value = new IBRServiceContracts.CWriteValue()
                {
                    NestedName = b.NestedName,
                    NewValue = ((int)e.NewValue).ToString(),
                    OldValue = b.Value,
                    WebSystemUID = this.SystemUID,
                };
                Values.Add(b.NestedName, value);
            }
            else
            {
                value.NewValue = ((int)ctrl.Value).ToString();
            }
        }

        private void PickerSelectionChanged(object sender, EventArgs e)
        {
            Picker ctrl = (Picker)sender;
            ComosWebSDK.UI.UIOptions b = (ComosWebSDK.UI.UIOptions)ctrl.BindingContext;

            IBRServiceContracts.CWriteValue value = null;
            if (!Values.TryGetValue(b.NestedName, out value))
            {
                value = new IBRServiceContracts.CWriteValue()
                {
                    NestedName = b.NestedName,
                    NewValue = b.Options[ctrl.SelectedItem.ToString()],
                    OldValue = b.Value,
                    WebSystemUID = this.SystemUID,
                };
                Values.Add(b.NestedName, value);
            }
            else
            {
                value.NewValue = b.Options[ctrl.SelectedItem.ToString()];
            }
        }

        // User changes handling
        private void SwitchToggled(object sender, ToggledEventArgs e)
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
                    WebSystemUID = this.SystemUID,
                };
                Values.Add(b.NestedName, value);
            }
            else
            {
                value.NewValue = e.Value ? "1" : "0";
            }
        }

        public async void OnSaveClicked(object sender, EventArgs e)
        {
            var m_Http = XComosMobile.Services.XServices.Instance.GetService<HttpClient>();

            IBRServiceContracts.CWriteValueCollection collection = new IBRServiceContracts.CWriteValueCollection()
            {
                Values = Values.Values.ToArray(),
            };

            //var http = m_ComosWeb.GetComosHttpClient();
            string user = ProjectData.User;
            string project = ProjectData.SelectedProject.UID;
            string workinglayer = ProjectData.SelectedLayer.UID;
            string language = ProjectData.SelectedLanguage.LCID;

            var db = Services.XServices.Instance.GetService<Services.XDatabase>();
            string servername = db.ReadSetting("ServerName", "http://siemens.southcentralus.cloudapp.azure.com");

            string output = Newtonsoft.Json.JsonConvert.SerializeObject(collection);
            StringContent content = new StringContent(output, Encoding.UTF8, "application/json");
            string url = string.Format(servername + "/BRMobile/v1/WriteComosValues/{0}/{1}/{2}/{3}",
                    user, WebUtility.UrlEncode(project), WebUtility.UrlEncode(workinglayer), language);


            // Working POST
            /*
			string output = Newtonsoft.Json.JsonConvert.SerializeObject(collection);
			StringContent content = new StringContent(output, Encoding.UTF8, "application/json");
			string url = string.Format("http://siemens.southcentralus.cloudapp.azure.com/BRMobile/v1/WriteComosValues/{0}/{1}/{2}/{3}",
					user, WebUtility.UrlEncode(project), WebUtility.UrlEncode(workinglayer), language);
					*/

            var response = await m_Http.PostAsync(url, content);

            var platform = Services.XServices.Instance.GetService<Services.IPlatformSystem>();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                await App.Current.MainPage.DisplayAlert(Services.TranslateExtension.TranslateText("error"), Services.TranslateExtension.TranslateText("save_failed") + response.StatusCode.ToString(), "OK");
                return;
            }

            output = await response.Content.ReadAsStringAsync();
            IBRServiceContracts.TResult<IBRServiceContracts.CWriteValueResult[]> result =
                            Newtonsoft.Json.JsonConvert.DeserializeObject<
                                    IBRServiceContracts.TResult<IBRServiceContracts.CWriteValueResult[]>>(output);
            if (!result.Status)
            {
                await App.Current.MainPage.DisplayAlert(Services.TranslateExtension.TranslateText("error"), Services.TranslateExtension.TranslateText("save_failed") + result.Message, "OK");
                return;
            }

            await App.Current.MainPage.DisplayAlert("", Services.TranslateExtension.TranslateText("done"), "OK");
            return;
        }

        #endregion
    }
}