using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComosWebSDK.Data;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XComosMobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageSolutions : PageTemplate
    {
        ComosWebSDK.IComosWeb web;

        public PageSolutions(ComosWebSDK.IComosWeb web)
        {
            InitializeComponent();
            this.ShowHamburger = false;
            this.ShowProjectInfo = true;
            this.web = web;
            new CheckAndDownloadAditionalContent();

            this.TileWidth = (int)((App.WidthPixels / 2.0f) - 20.0f / App.PixelDensity);
            this.TileHeight = this.TileWidth;
            this.BindingContext = this;

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            (Application.Current.MainPage as PageRoot).IsGestureEnabled = false;

        }

        public int TileHeight { get; private set; }
        public int TileWidth { get; private set; }

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
            await App.Navigation.PushAsync(new Pages.maintenance.WorkPackages());
        }

        async void WPAnFailure_Clicked(object sender, EventArgs e)
        {
            Frame bt = (Frame)sender;
            await FadeOutIn(bt);
            await App.Navigation.PushAsync(new Pages.maintenance.WorkPackagesAnFailure());
        }

        async void Inspections_Clicked(object sender, EventArgs e)
        {
            Frame bt = (Frame)sender;
            await FadeOutIn(bt);
            await App.Navigation.PushAsync(new Pages.inspection.InspectionPage());
        }

        async void browser_Clicked(object sender, EventArgs e)
        {
            Frame bt = (Frame)sender;
            await FadeOutIn(bt);
            await App.Navigation.PushAsync(new Pages.PageBrowser());
        }


        async void Incidents_Clicked(object sender, EventArgs e)
        {
            Frame bt = (Frame)sender;
            await FadeOutIn(bt);
            await App.Navigation.PushAsync(new Pages.maintenance.IncidentPage());
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

        async void Documents_Clicked(object sender, EventArgs e)
        {
            Frame bt = (Frame)sender;
            await FadeOutIn(bt);
            await App.Navigation.PushAsync(new Pages.PageDocuments());
        }

        async void Projects_Clicked(object sender, EventArgs e)
        {
            Frame bt = (Frame)sender;
            await FadeOutIn(bt);
            await App.Navigation.PushAsync(new Pages.PageProjects(web));
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

        async void Search_Material_Clicked(object sender, EventArgs e)
        {
            Frame bt = (Frame)sender;
            await FadeOutIn(bt);
            await App.Navigation.PushAsync(new PageSearchMaterial());
        }


        async void Log_Book_Clicked(object sender, EventArgs e)
        {
          Frame bt = (Frame)sender;
          await FadeOutIn(bt);
          await App.Navigation.PushAsync(new Pages.maintenance.LogBookPage());
        }
    

        async void Theme_Clicked(object sender, EventArgs e)
        {
            Frame bt = (Frame)sender;
            await FadeOutIn(bt);

            var action = await DisplayActionSheet(Services.TranslateExtension.TranslateText("choose_theme"), Services.TranslateExtension.TranslateText("cancel"), null, "Comos"
                , "Siemens", "Light", "Black");

            if (action != Services.TranslateExtension.TranslateText("cancel"))
            {
                Theming.ChangeTheme(action);
                var db = Services.XServices.Instance.GetService<Services.IDatabase>();

                db.WriteSetting("Theme", action);

            }

        }

        protected override bool OnBackButtonPressed()
        {
            //this.Navigation.PushAsync(new PageProjects(web));
            return true;
        }

    }
}