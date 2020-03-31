using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XComosMobile.Pages.maintenance;

namespace XComosMobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageMenu : ContentPage
    {
        ComosWebSDK.IComosWeb web;
        public string UserName
        {
            get
            {
                if (Services.XServices.Instance.GetService<ViewModels.ProjectData>() != null)
                {
                    return Services.XServices.Instance.GetService<ViewModels.ProjectData>().User;
                }
                else
                    return "";
            }
            set { }
        }
        public string ProjectName
        {
            get
            {
                if (Services.XServices.Instance.GetService<ViewModels.ProjectData>() != null && Services.XServices.Instance.GetService<ViewModels.ProjectData>().SelectedProject != null)
                {
                    return Services.XServices.Instance.GetService<ViewModels.ProjectData>().SelectedProject.Name;
                }
                else
                    return "";
            }
            set { }
        }

        public string ProjectDescription
        {
            get
            {
                if (Services.XServices.Instance.GetService<ViewModels.ProjectData>() != null && Services.XServices.Instance.GetService<ViewModels.ProjectData>().SelectedProject != null)
                {
                    return Services.XServices.Instance.GetService<ViewModels.ProjectData>().SelectedProject.Description;
                }
                else
                    return "";
            }
            set { }
        }
        public string OverlayName
        {
            get
            {
                if (Services.XServices.Instance.GetService<ViewModels.ProjectData>() != null && Services.XServices.Instance.GetService<ViewModels.ProjectData>().SelectedLayer != null)
                {
                    return Services.XServices.Instance.GetService<ViewModels.ProjectData>().SelectedLayer.Name;
                }
                else
                    return "";
            }
            set { }
        }

        public string OverlayDescription
        {
            get
            {
                if (Services.XServices.Instance.GetService<ViewModels.ProjectData>() != null && Services.XServices.Instance.GetService<ViewModels.ProjectData>().SelectedLayer != null)
                {
                    return Services.XServices.Instance.GetService<ViewModels.ProjectData>().SelectedLayer.Description;
                }
                else
                    return "";
            }
            set { }
        }



        public PageMenu(ComosWebSDK.IComosWeb web)
        {
            InitializeComponent();
            this.BindingContext = this;
            this.web = web;
            //foreach(var item in Enum.GetNames(typeof(Theming.Themes)))
            //    PickerThemes.Items.Add(item);

            this.VersionNumber.Text = "V" + Services.XServices.Instance.GetService<Services.IPlatformSystem>().AppVersionName;
        }

        public void ProjectSettingsChanged()
        {
            OnPropertyChanged("UserName");
            OnPropertyChanged("ProjectName");
            OnPropertyChanged("ProjectDescription");
            OnPropertyChanged("OverlayName");
            OnPropertyChanged("OverlayDescription");
        }

        void OnThemeChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            Theming.ChangeTheme(selectedIndex.ToString());
            var db = Services.XServices.Instance.GetService<Services.IDatabase>();
            db.WriteSetting("Theme", selectedIndex.ToString());
        }
        void OnLanguageChanged(object sender, EventArgs e)
        {
            var web = Services.XServices.Instance.GetService<ComosWebSDK.IComosWeb>();
            try
            {
                App.ResetAppAsync(web).GetAwaiter();
            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert(Services.TranslateExtension.TranslateText("logout"), "Error al cerrar sesion: " + ex.Message, Services.TranslateExtension.TranslateText("OK")).GetAwaiter();
            }
        }

        void OnConnectionModeChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex != -1)
            {
                int comoswebsdk_mode = selectedIndex;
                ComosWebSDK.IComosWeb comosweb = Services.XServices.Instance.GetService<ComosWebSDK.IComosWeb>();
                var db = Services.XServices.Instance.GetService<Services.IDatabase>();

                switch (comoswebsdk_mode)
                {
                    case 0: // Offline only
                        if (comosweb is Services.ComosWebOffline)
                            return; // Nothing tod, is already in this mode.
                        Services.XServices.Instance.RemoveService<ComosWebSDK.IComosWeb>();
                        Services.XServices.Instance.AddService<ComosWebSDK.IComosWeb>(
                            new Services.ComosWebOffline(db, comosweb.ComosWeb.URL));
                        break;
                    case 1: // Online only
                        if (comosweb is ComosWebSDK.ComosWeb)
                            return; // Nothing tod, is already in this mode.
                        Services.XServices.Instance.RemoveService<ComosWebSDK.IComosWeb>();
                        Services.XServices.Instance.AddService<ComosWebSDK.IComosWeb>(
                           comosweb.ComosWeb);
                        break;
                    case 2: //Online first 
                        if (comosweb is Services.ComosWebOnlineFirst)
                            return; // Nothing tod, is already in this mode.
                        Services.XServices.Instance.RemoveService<ComosWebSDK.IComosWeb>();
                        Services.XServices.Instance.AddService<ComosWebSDK.IComosWeb>(
                           new Services.ComosWebOnlineFirst(comosweb.ComosWeb));
                        break;
                    case 3: // Offline first 
                        if (comosweb is Services.ComosWebOfflineFirst)
                            return; // Nothing tod, is already in this mode.
                        Services.XServices.Instance.RemoveService<ComosWebSDK.IComosWeb>();
                        Services.XServices.Instance.AddService<ComosWebSDK.IComosWeb>(
                           new Services.ComosWebOfflineFirst(comosweb.ComosWeb));
                        break;
                    default:
                        break;
                }
            }
        }



        async void Navigator_Clicked(object sender, EventArgs e)
        {

            var projectdata = Services.XServices.Instance.GetService<ViewModels.ProjectData>();
            projectdata.SaveLastSession();

            var m_Client = Services.XServices.Instance.GetService<ComosWebSDK.IComosWeb>();
            m_Client.StartHeartBeat();

            Device.StartTimer(TimeSpan.FromSeconds(
                m_Client.HeartBeat),
                m_Client.DoHeartBeat);


            // Init the image converter based on the database to use.
            object tmp = null;
            if (App.Current.Resources.TryGetValue("PictureToImage", out tmp))
            {
                Converters.PictureRefToImage c = tmp as Converters.PictureRefToImage;
                if (c != null)
                    c.InitConverter(projectdata.SelectedLayer.Database);
            }

            Frame bt = (Frame)sender;
            await FadeOutIn(bt);
            await App.Navigation.PushAsync(new comos.NavigatorTabbed(projectdata.SelectedLayer, projectdata.SelectedLanguage.LCID));
        }

        async void Favorites_Clicked(object sender, EventArgs e)
        {
            var projectdata = Services.XServices.Instance.GetService<ViewModels.ProjectData>();
            projectdata.SaveLastSession();

            var m_Client = Services.XServices.Instance.GetService<ComosWebSDK.IComosWeb>();
            m_Client.StartHeartBeat();

            Device.StartTimer(TimeSpan.FromSeconds(
                m_Client.HeartBeat),
                                m_Client.DoHeartBeat);

            Frame bt = (Frame)sender;
            await FadeOutIn(bt);
            await App.Navigation.PushAsync(new PageFavorites(projectdata.SelectedLayer, projectdata.SelectedLanguage.LCID));
        }

        async void Maintenance_Clicked(object sender, EventArgs e)
        {
            Frame bt = (Frame)sender;
            await FadeOutIn(bt);
            await App.Navigation.PushAsync(new Pages.maintenance.MaintenanceTabs());
        }

        async void Inspections_Clicked(object sender, EventArgs e)
        {
            Frame bt = (Frame)sender;
            await FadeOutIn(bt);
            await App.Navigation.PushAsync(new Pages.inspection.InspectionPage());
        }

        async void Incidents_Clicked(object sender, EventArgs e)
        {
            Frame bt = (Frame)sender;
            await FadeOutIn(bt);
            await App.Navigation.PushAsync(new Pages.maintenance.IncidentsTypes());
            //await App.Navigation.PushAsync(new Pages.maintenance.CachedIncidentsPage());
        }

        async void Upload_Clicked(object sender, EventArgs e)
        {
            Frame bt = (Frame)sender;
            await FadeOutIn(bt);
            await App.Navigation.PushAsync(new Pages.PageUpload());
        }
        async void Search_Clicked(object sender, EventArgs e)
        {
            Frame bt = (Frame)sender;
            await FadeOutIn(bt);
            await App.Navigation.PushAsync(new Pages.PageSearch());
        }

        async void browser_Clicked(object sender, EventArgs e)
        {
            Frame bt = (Frame)sender;
            await FadeOutIn(bt);
            await App.Navigation.PushAsync(new Pages.PageBrowser());
        }

        async void Logoff_Clicked(object sender, EventArgs e)
        {
            Frame bt = (Frame)sender;
            await FadeOutIn(bt);
            try
            {
                await App.ResetAppAsync(web);
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert(Services.TranslateExtension.TranslateText("logout"), "Error al cerrar sesion: " + ex.Message, Services.TranslateExtension.TranslateText("OK"));
            }
        }

        async void Documents_Clicked(object sender, EventArgs e)
        {
            Frame bt = (Frame)sender;
            await FadeOutIn(bt);
            await App.Navigation.PushAsync(new Pages.PageDocuments());
        }

        async void Home_Clicked(object sender, EventArgs e)
        {
            Frame bt = (Frame)sender;
            await FadeOutIn(bt);
            await App.Navigation.PushAsync(new Pages.PageSolutions(web));
        }

        async void Projects_Clicked(object sender, EventArgs e)
        {
            Frame bt = (Frame)sender;
            await FadeOutIn(bt);
            await App.Navigation.PushAsync(new Pages.PageProjects(web));
        }

        async void Theme_Clicked(object sender, EventArgs e)
        {
            Frame bt = (Frame)sender;
            await FadeOutIn(bt);

            //foreach(var item in Enum.GetNames(typeof(Theming.Themes)))
            //    PickerThemes.Items.Add(item);

            var action = await DisplayActionSheet(Services.TranslateExtension.TranslateText("choose_theme"), Services.TranslateExtension.TranslateText("cancel"), null, "Comos"
                , "Siemens", "Light", "Black");

            if (action != Services.TranslateExtension.TranslateText("cancel"))
            {
                Theming.ChangeTheme(action);
                var db = Services.XServices.Instance.GetService<Services.IDatabase>();

                db.WriteSetting("Theme", action);

            }


        }


        public async Task<bool> FadeOutIn(Frame bt)
        {
            try
            {
                if (!bt.AnimationIsRunning("FadeTo"))
                {
                    await bt.FadeTo(0, 130, Easing.Linear);
                }
                if (!bt.AnimationIsRunning("FadeTo"))
                {
                    await bt.FadeTo(1, 130, Easing.Linear);
                }
            }
            catch (Exception)
            {

            }
            return true;
        }
    }
}