using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using Xamarin.Forms;
using System.Threading.Tasks;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using ComosWebSDK;
using System.Net.Http;
using Xamarin.Forms.Xaml;
using XComosMobile.Pages.comos;
using Android.Views;

namespace XComosMobile
{
    public partial class App : Application
    {
        public static NavigationPage Navigation { get; private set; }

        public static double FontSize { get; private set; }

        public static double PixelDensity { get; private set; }

        public static double WidthPixels { get; private set; }

        public static double HeightPixels { get; private set; }

        public Pages.PageRoot PageRoot { get { return m_PageRoot; } }

        private static Pages.PageRoot m_PageRoot;

        public static bool MenuIsPresented
        {
            get
            {
                return m_PageRoot.IsPresented;
            }
            set
            {
                m_PageRoot.IsPresented = value;
            }
        }

        public static async Task ResetAppAsync(ComosWebSDK.IComosWeb web)
        {
          bool answer = false;
					var platform = Services.XServices.Instance.GetService<Services.IPlatformSystem>();
			
					if (web != null) //&& platform.IsOnline && platform.IsDeviceOnline)
					{
						web.StartHeartBeat();
						answer = await web.Logout();
                        Services.XServices.Instance.RemoveService<ComosWebSDK.IComosWeb>();
						web.Dispose();
						//Services.XServices.Instance.GetService<ICommuncationManager>();
						await App.Current.MainPage.DisplayAlert(Services.TranslateExtension.TranslateText("logout"), Services.TranslateExtension.TranslateText("logout_reference"), Services.TranslateExtension.TranslateText("OK"));
						await Navigation.PopToRootAsync();
					}
					else
					{
						await App.Current.MainPage.DisplayAlert(Services.TranslateExtension.TranslateText("logout"), "Error al cerrar sesion", Services.TranslateExtension.TranslateText("OK"));
					}
        }

        public App()
        {
            InitializeComponent();

            if (!DesignMode.IsDesignModeEnabled)
            {
                // Don't run in the Previewer  
            
                //ApplyAccentColor((Color)Resources["UIAccent"]);
                //Theming.ChangeTheme(Theming.Themes.Comos);

                // register some services.
                var db = Services.XServices.Instance.GetService<Services.IDatabase>();
                Services.XServices.Instance.AddService<Services.XDatabase>(
                    new Services.XDatabase(db));
                string theme = db.ReadSetting("Theme", "Siemens");
                Theming.ChangeTheme(theme);

                if (Device.RuntimePlatform != Device.UWP)
                {
                    // For android needs to be done here.
                    App.WidthPixels = Services.XServices.Instance.GetService<Services.IPlatformSystem>().GetScreenWidthPixels;
                    App.HeightPixels = Services.XServices.Instance.GetService<Services.IPlatformSystem>().GetScreenHeightPixels;
                    App.PixelDensity = Services.XServices.Instance.GetService<Services.IPlatformSystem>().GetScreenDensity;
                }
                else
                    App.PixelDensity = 1;
                App.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
                var web = Services.XServices.Instance.GetService<ComosWebSDK.IComosWeb>();
                var menuPage = new Pages.PageMenu(web);
						

						    Navigation = new NavigationPage(new Pages.PageLogin());
                Navigation.Popped += Navigation_Popped;
                Navigation.PoppedToRoot += Navigation_PoppedToRoot;
                m_PageRoot = new Pages.PageRoot();
                m_PageRoot.Master = menuPage;            
                m_PageRoot.Detail = Navigation;            
                MainPage = m_PageRoot;

                m_PageRoot.IsPresentedChanged += M_PageRoot_IsPresentedChanged;

                if (Device.RuntimePlatform == Device.UWP)
                    MainPage.Appearing += MainPage_Appearing;

            }

        }

   

        private void M_PageRoot_IsPresentedChanged(object sender, EventArgs e)
        {
            MasterDetailPage page = sender as MasterDetailPage;
            if (page.IsPresented)
            {
                Pages.PageMenu menu = (Pages.PageMenu)page.Master;
                menu.ProjectSettingsChanged();
            }

        }

        private void Navigation_PoppedToRoot(object sender, NavigationEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void Navigation_Popped(object sender, NavigationEventArgs e)
        {
            PageTemplate p = Navigation.CurrentPage as PageTemplate;
            if (p != null)
                p.OnNavigateBackAsync();
        }

        private void MainPage_Appearing(object sender, EventArgs e)
        {
            // For windows needs to be fetched here.
            App.WidthPixels = Services.XServices.Instance.GetService<Services.IPlatformSystem>().GetScreenWidthPixels;
            App.HeightPixels = Services.XServices.Instance.GetService<Services.IPlatformSystem>().GetScreenHeightPixels;
            App.PixelDensity = Services.XServices.Instance.GetService<Services.IPlatformSystem>().GetScreenDensity;
            MainPage.Appearing -= MainPage_Appearing;
        }

        protected override void OnStart()
        {            

        }
        

        protected override void OnSleep()
        {
                     
            var comosweb = Services.XServices.Instance.GetService<ComosWebSDK.IComosWeb>();
            if (comosweb != null)
            {
                // causing many crashes lets leave licensing issues to comosweb itself
                //comosweb.StopHeartbeat();
                // Make sure we are loging out, so the comos web license is released.
               // Task.Run(async () => { await comosweb.Logout(); });
            }
        }

        protected override void OnResume()
        {
            
            // Handle when your app sleeps
            var comosweb = Services.XServices.Instance.GetService<ComosWebSDK.IComosWeb>();
            if (comosweb != null)
            {
                // check if the license is released and if it is checkout another one
                if (!comosweb.DoHeartBeat())
                {
                    var platform = Services.XServices.Instance.GetService<Services.IPlatformSystem>();
                    platform.ShowProgressMessage(Services.TranslateExtension.TranslateText("reloging"), true);

										try
										{
											var session = comosweb.Login().GetAwaiter().GetResult();

											if (session == null)
												Current.MainPage.DisplayAlert(
														"Advertencia",
														"No se pudo establecer la conexión con el servidor",
														Services.TranslateExtension.TranslateText("OK")).GetAwaiter();

											comosweb.StartHeartBeat();
											platform.HideProgressMessage();
										}
										catch(Exception e)
										{
											Current.MainPage.DisplayAlert(
														"Advertencia",
														"No se pudo establecer la conexión con el servidor: " + e.Message,
														Services.TranslateExtension.TranslateText("OK")).GetAwaiter();
										}          
                }                    
            }
        }
       
        // Reference: https://forums.xamarin.com/discussion/63935/how-to-make-color-accent-match-your-android-apps-theme
        public static void ApplyAccentColor(Color newAccentColor)
        {
            if (Device.Android == Device.RuntimePlatform)
            {
                // Access the setter of the 'Xamarin.Forms.Color.Accent' property
                var prop = typeof(Color).GetRuntimeProperty(nameof(Color.Accent));
                var setter = prop?.SetMethod;
                // Invoke the setter method of the Color.Accent property to overwrite it with the custom accent color
                setter?.Invoke(Color.Accent, new object[] { newAccentColor });
            }
        }
    }
}
