using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.ComponentModel;
using ComosWebSDK.Data;
using XComosMobile.Pages.controls;

namespace XComosMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageTemplate : ContentPage, INotifyPropertyChanged
    {
        private const string NOT_FAVORITE = "\uf006";
        private const string FAVORITE = "\uf005";


        private string isfavorite = NOT_FAVORITE;
        public CObject MainObject { get; set; }

        public PageTemplate()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);
            OnOpenMenu = new Command(() => { DoOpenMenu(); });
            OnOpenSecondMenu = new Command(() => { DoOpenSecondMenu(); });
            OnBackButtonClicked = new Command(() => { DoBackButtonPressed(); });
            OnFavorite = new Command(() => { DoFavorite(); });
            OnNavigate = new Command(() => { DoNavigate(); });
            OnOnline = new Command(() => { DoOnline(); });

            this.NavigationBarControl = new Pages.controls.NavigationBarControl();
            this.NavigationBarControl.BindingContext = this;
        }

        #region menu masterdetailpage implementation
        bool m_ShowHamburger = true;
        public bool ShowHamburger
        {
            get => m_ShowHamburger;
            set
            {
                m_ShowHamburger = value;
                OnPropertyChanged("ShowHamburger");
            }
        }

        bool m_ShowAttributeView = false;
        public bool ShowAttributeView
        {
            get => m_ShowAttributeView;
            set
            {
                m_ShowAttributeView = value;
                OnPropertyChanged("ShowAttributeView");
            }
        }

        bool m_ShowHome = false;
        public bool ShowHome
        {
            get => m_ShowHome;
            set
            {
                m_ShowHome = value;
                OnPropertyChanged("ShowHome");
            }
        }

        bool m_ShowSearch = false;
        public bool ShowSearch
        {
            get => m_ShowSearch;
            set
            {
                m_ShowSearch = value;
                OnPropertyChanged("ShowSearch");
            }
        }

        bool m_ShowRefresh = false;
        public bool ShowRefresh
        {
            get => m_ShowRefresh;
            set
            {
                m_ShowRefresh = value;
                OnPropertyChanged("ShowRefresh");
            }
        }


        bool m_ShowFilter = false;
        public bool ShowFilter
        {
            get => m_ShowFilter;
            set
            {
                m_ShowFilter = value;
                OnPropertyChanged("ShowFilter");
            }
        }

        public Command OnOpenMenu { get; private set; }
        public Command OnOpenSecondMenu { get; private set; }

        public void DoOpenMenu()
        {
            App.MenuIsPresented = true;
        }
        public void DoOpenSecondMenu()
        {

        }
        #endregion

        #region implemenation of custom back button

        bool m_ShowBackButton = true;
        public bool ShowBackButton
        {
            get => m_ShowBackButton;
            set
            {
                m_ShowBackButton = value;
                OnPropertyChanged("ShowBackButton");
            }
        }
        public Command OnBackButtonClicked { get; private set; }

        public void DoBackButtonPressed()
        {
            //this.OnBackButtonPressed();
            if (!this.OnBackButtonPressed())
            {
                this.Navigation.PopAsync(true);
                //this.Navigation.PopToRootAsync();
            }
        }

        bool showinfo = false;
        public bool ShowProjectInfo
        {
            get { return showinfo; }
            set
            {
                showinfo = value;
                OnPropertyChanged("ShowProjectInfo");
            }
        }

        virtual public void OnNavigateBackAsync()
        {

        }
        #endregion

        public bool CheckOnline()
        {
            var plataform = Services.XServices.Instance.GetService<Services.IPlatformSystem>();
            return plataform.IsOnline;
        }

        public void SendNotification(string title, string message)
        {
            var plataform = Services.XServices.Instance.GetService<Services.IPlatformSystem>();
            plataform.SendNotification(title, message);
        }


        public void ShowSpinner(string message)
        {
            var platform = Services.XServices.Instance.GetService<Services.IPlatformSystem>();
            platform.ShowProgressMessage(message, true);
        }


        public void HideSpinner()
        {
            //if (!forceSpinner)
            //  {
            try
            {
                var platform = Services.XServices.Instance.GetService<Services.IPlatformSystem>();
                platform.HideProgressMessage();
            }
            catch (Exception ex)
            {
                ShowToast(ex.Message);
            }

            //}                        
        }

        public void ShowToast(string message)
        {

            var platform = Services.XServices.Instance.GetService<Services.IPlatformSystem>();
            platform.ShowToast(message);

        }

        //public int NavBarButtonWidth { get { return (int)(100.0 / App.PixelDensity); } }
        public int NavBarButtonWidth { get { return (int)(App.FontSize * 2); } }

        private void OnSearch_Clicked(object sender, EventArgs e)
        {
        }

        StackLayout m_MenuItemsLeft = null;
        public StackLayout MenuItemsLeft
        {
            get { return m_MenuItemsLeft; }
            set
            {
                m_MenuItemsLeft = value;
                OnPropertyChanged("MenuItemsLeft");
            }
        }

        StackLayout m_MenuItemsRight = null;
        public StackLayout MenuItemsRight
        {
            get { return m_MenuItemsRight; }
            set
            {
                m_MenuItemsRight = value;
                OnPropertyChanged("MenuItemsRight");
            }
        }

        // FAVORITE BUTTONS

        public Command OnSave { get; set; }
        public Command OnFavorite { get; private set; }
        public Command OnNavigate { get; private set; }

        public void DoNavigate()
        {
            if (MainObject != null)
            {
                var projectdata = Services.XServices.Instance.GetService<ViewModels.ProjectData>();
                projectdata.SaveLastSession();
                var m_Client = Services.XServices.Instance.GetService<ComosWebSDK.IComosWeb>();
                m_Client.StartHeartBeat();
                Pages.comos.PageNavigator nav = new Pages.comos.PageNavigator(projectdata.SelectedLayer, projectdata.SelectedLanguage.LCID);
                nav.NavigateToObject(MainObject);
                App.Navigation.PushAsync(nav);
            }
        }

        public void DoFavorite()
        {
            if (this.MainObject != null)
            {
                var db = Services.XServices.Instance.GetService<Services.XDatabase>();

                if (isfavorite == FAVORITE)
                {
                    db.RemoveFavorite(MainObject.UID);
                    isfavorite = NOT_FAVORITE;
                }
                else if (isfavorite == NOT_FAVORITE)
                {
                    string output = Newtonsoft.Json.JsonConvert.SerializeObject(this.MainObject);
                    db.SetFavorite(MainObject.UID, output);
                    isfavorite = FAVORITE;
                }
                OnPropertyChanged("IsFavorite");
            }
        }

        #region Online Handling
        static bool m_IsOnline = true;
        public bool IsOnline
        {
            get => m_IsOnline;
            set
            {
                if (m_IsOnline == value) return;
                m_IsOnline = value;
                OnPropertyChanged("OnlineColor");
                // Trigger/Propagate on full page stack
                if (App.Navigation != null)
                {
                    foreach (var page in App.Navigation.Navigation.NavigationStack)
                    {
                        ((PageTemplate)page).OnPropertyChanged("OnlineColor");
                    }
                }
            }
        }
        public Color OnlineColor
        {
            get
            {
                if (m_IsOnline)
                {
                    return Color.White;
                }
                return Color.Red;
            }
        }
        public Command OnOnline { get; private set; }
        public async void DoOnline()
        {
            ComosWebSDK.IComosWeb comosweb = Services.XServices.Instance.GetService<ComosWebSDK.IComosWeb>();
            var db = Services.XServices.Instance.GetService<Services.IDatabase>();
            var platform = Services.XServices.Instance.GetService<Services.IPlatformSystem>();

            if (m_IsOnline)
            {
                if (comosweb is Services.ComosWebOffline)
                {
                    IsOnline = false; // Should never happen
                    return; // Nothing todo, is already in this mode.
                }

                if (comosweb != null)
                {
                    if (await App.Current.MainPage.DisplayAlert("Offline", Services.TranslateExtension.TranslateText("go_offline"), Services.TranslateExtension.TranslateText("yes"), Services.TranslateExtension.TranslateText("no")))
                    {
                        try
                        {
                            bool success = await comosweb.Logout();
                            if (success)
                            {
                                Services.XServices.Instance.RemoveService<ComosWebSDK.IComosWeb>();
                                // Switched to offline in none login screen
                                //var xdb = Services.XServices.Instance.GetService<Services.XDatabase>();
                                string ServerName = Pages.comos.Constants.serverAddress;
                                Services.XServices.Instance.AddService<ComosWebSDK.IComosWeb>(
                                        new Services.ComosWebOffline(db, ServerName));
                                IsOnline = false;
                                platform.IsOnline = false;
                            }
                            else
                            {
                                await App.Current.MainPage.DisplayAlert("Error", "No se pudo cerrar sesion", "OK");
                            }
                        }
                        catch (Exception e)
                        {
                            await App.Current.MainPage.DisplayAlert("Error", "No se pudo cerrar sesion: " + e.Message, "OK");
                        }
                    }

                }
            }
            else
            {
                if (platform.IsDeviceOnline)
                {
                    if (comosweb is Services.ComosWebOnlineFirst)
                    {
                        IsOnline = true; // Should never happen.
                        return; // Nothing todo, is already in this mode.
                    }
                    Services.XServices.Instance.RemoveService<ComosWebSDK.IComosWeb>();

                    try
                    {
                        await App.ResetAppAsync(comosweb);
                        IsOnline = true;
                        platform.IsOnline = true;
                    }
                    catch (Exception ex)
                    {
                        await App.Current.MainPage.DisplayAlert(Services.TranslateExtension.TranslateText("logout"), "Error al cerrar sesion: " + ex.Message, Services.TranslateExtension.TranslateText("OK"));
                    }
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("", Services.TranslateExtension.TranslateText("no_network"), "OK");
                }
            }
        }
        #endregion

        bool m_ShowFavoritesButton = true;
        public bool ShowFavoriteButton
        {
            get => m_ShowFavoritesButton;
            set
            {
                m_ShowFavoritesButton = value;
                OnPropertyChanged("ShowFavoriteButton");
            }
        }

        public void CheckFavorite()
        {
            var db = Services.XServices.Instance.GetService<Services.XDatabase>();

            if (this.MainObject != null)
            {
                if (!db.IsFavorite(this.MainObject.UID))
                {
                    isfavorite = NOT_FAVORITE;
                }
                else
                {
                    isfavorite = FAVORITE;
                }
            }
            else
            {
                isfavorite = NOT_FAVORITE;
            }

            OnPropertyChanged("IsFavorite");
        }
        public string IsFavorite
        {
            get
            {
                return isfavorite;

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

        public NavigationBarControl NavigationBarControl { get; private set; }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            (Application.Current.MainPage as Pages.PageRoot).IsGestureEnabled = true;
        }
    }
}
