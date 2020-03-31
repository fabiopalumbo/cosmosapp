using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ComosWebSDK.Data;
using XComosMobile.Pages.maintenance;
using Rg.Plugins.Popup.Services;
using XComosMobile.PopUps;
using Android.App;
using Android.Content;

namespace XComosMobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageLogin : PageTemplate
    {
        CLanguage[] langs = new ComosWebSDK.Data.CLanguage[3];
        //string myVersionNumber = "V" + Services.XServices.Instance.GetService<Services.IPlatformSystem>().AppVersionName;

        public PageLogin()
        {
            InitializeComponent();
            Services.XDatabase db = Services.XServices.Instance.GetService<Services.XDatabase>();

            this.ShowHamburger = false;
            this.ShowBackButton = false;
            this.BindingContext = this;

            this.UserName = db.ReadSetting("UserName", "");
            this.DomainName = comos.Constants.userDomain;
            //this.ServerName = db.ReadSetting("ServerName", "http://siemens.southcentralus.cloudapp.azure.com");
            this.ServerName = comos.Constants.serverAddress;
            //this.ServerName = db.ReadSetting("ServerName", "http://10.0.2.2:81");
            //this.ServerName = db.ReadSetting("ServerName", "http://comos.industrysoftware.automation.siemens.com");
            this.RememberPassword = db.ReadSetting("RememberPassword", false);
            //this.ServerNameBR = db.ReadSetting("ServerNameBR", "http://siemens.southcentralus.cloudapp.azure.com:5109/Service.svc");            
            this.ServerNameBR = comos.Constants.webServiceAddress;


            if (this.RememberPassword)
                this.Password = db.ReadSetting("Password", "");

            var platform = Services.XServices.Instance.GetService<Services.IPlatformSystem>();
            this.IsOnline = platform.IsOnline;

            this.codeVersion = "v" + platform.AppVersionName;
        }

        string m_ServerNameBR = "";
        public string ServerNameBR { get => m_ServerNameBR; set { m_ServerNameBR = value; OnPropertyChanged("ServerNameBR"); } }

        string m_ServerName = "";
        public string ServerName { get => m_ServerName; set { m_ServerName = value; OnPropertyChanged("ServerName"); } }
        string m_UserName = "";
        public string UserName { get => m_UserName; set { m_UserName = value; OnPropertyChanged("UserName"); } }
        string m_Password = "";
        public string Password { get => m_Password; set { m_Password = value; OnPropertyChanged("Password"); } }
        string m_DomainName = "";
        public string DomainName { get => m_DomainName; set { m_DomainName = value; OnPropertyChanged("DomainName"); } }

        bool m_RememberPassword = false;
        public bool RememberPassword { get => m_RememberPassword; set { m_RememberPassword = value; OnPropertyChanged("RememberPassword"); } }

        string m_codeVersion = "";
        public string codeVersion { get => m_codeVersion; set { m_codeVersion = value; OnPropertyChanged("codeVersion"); } }


        ComosWebSDK.IComosWeb m_ComosWeb = null;
        ComosWebSDK.ComosHttp http = null;

        public async void OnLoginClicked(object sender, EventArgs e)
        {
            try
            {
                this.UserName = this.UserName.Trim();  //Trim name, in case a space is added accidentally.

                if (this.IsOnline)
                {

                    this.ShowSpinner(Services.TranslateExtension.TranslateText("connecting"));

                    if (m_ComosWeb == null)
                    {
                        var comosweb = new ComosWebSDK.ComosWeb(this.ServerName);
                        m_ComosWeb = new XComosMobile.Services.ComosWebOnlineFirst(comosweb);
                    }

                    try
                    {
                        await m_ComosWeb.Connect(this.DomainName, this.UserName, this.Password);
                    }
                    catch (Exception ex)
                    {
                        await App.Current.MainPage.DisplayAlert(Services.TranslateExtension.TranslateText("error"), "No se pudo conectar: " + ex.Message, "OK");
                        return;
                    }

                    try
                    {
                        var session = await m_ComosWeb.Login();
                        if (session != null)
                        {
                            Services.XServices.Instance.AddService<ComosWebSDK.IComosWeb>(m_ComosWeb);
                            //Services.XServices.Instance.AddService<ComosWebSDK.ComosHttp>(http);

                            var db = Services.XServices.Instance.GetService<Services.XDatabase>();
                            db.WriteSetting("UserName", this.UserName);
                            db.WriteSetting("DomainName", this.DomainName);
                            db.WriteSetting("ServerName", this.ServerName);
                            db.WriteSetting("ServerNameBR", this.m_ServerNameBR);
                            db.WriteSetting("RememberPassword", this.RememberPassword);
                            if (this.RememberPassword)
                            {
                                db.WriteSetting("Password", this.Password);
                            }

                            int selectedIndex = PickerLanguages.SelectedIndex;

                            if (selectedIndex != -1)
                            {
                                CLanguage lan = langs[selectedIndex];
                                Services.TranslateExtension.Language = lan.Key;
                                db.WriteSetting("Language", lan);
                            }

                            Services.XServices.Instance.GetService<Services.IPlatformSystem>().UseLogin(
                            this.DomainName, this.Password, this.UserName);

                            var web = Services.XServices.Instance.GetService<ComosWebSDK.IComosWeb>();
                            await App.Navigation.PushAsync(new PageProjects(web));
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert(Services.TranslateExtension.TranslateText("error"), "No se pudo establecer la conexión con el servidor", "OK");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        await App.Current.MainPage.DisplayAlert(Services.TranslateExtension.TranslateText("error"), "No se pudo establecer la conexión con el servidor: " + ex.Message, "OK");
                        return;
                    }
                }
                else
                {
                    CSession session = null;
                    if (m_ComosWeb == null)
                    {
                        var db = Services.XServices.Instance.GetService<Services.IDatabase>();
                        m_ComosWeb = new XComosMobile.Services.ComosWebOffline(db, this.ServerName);

                        try
                        {
                            session = await m_ComosWeb.Login();
                            if (session != null)
                            {
                                Services.XServices.Instance.AddService<ComosWebSDK.IComosWeb>(m_ComosWeb);
                                Services.XServices.Instance.GetService<Services.IPlatformSystem>().UseLogin(this.DomainName, this.Password, this.UserName);
                            }
                            else
                            {
                                await App.Current.MainPage.DisplayAlert(Services.TranslateExtension.TranslateText("error"), "No se pudo loguear sin conexion", "OK");
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            await App.Current.MainPage.DisplayAlert(Services.TranslateExtension.TranslateText("error"), "No se pudo loguear sin conexion: " + ex.Message, "OK");
                            return;
                        }
                    }
                    var web = Services.XServices.Instance.GetService<ComosWebSDK.IComosWeb>();
                    await App.Navigation.PushAsync(new PageProjects(web));
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert(Services.TranslateExtension.TranslateText("error"), Services.TranslateExtension.TranslateText("login_error") + ex.Message, "OK");
                return;
            }
            finally
            {
                m_ComosWeb = null;
                this.HideSpinner();
            }
        }

        void StartLanguages()
        {
            langs[0] = new CLanguage() { Key = "en", LCID = "1033", Value = "English" };
            langs[1] = new CLanguage() { Key = "es", LCID = "1033", Value = "Español" };
            langs[2] = new CLanguage() { Key = "pt-BR", LCID = "1046", Value = "Português" };

            PickerLanguages.ItemsSource = langs;
            PickerLanguages.ItemDisplayBinding = new Binding("Value");
            PickerLanguages.SelectedIndexChanged += PickerLanguages_SelectedIndexChanged;

            //Set default espanol
            CLanguage lan = langs[1];   //default en español
            Services.XDatabase db = Services.XServices.Instance.GetService<Services.XDatabase>();
            Services.TranslateExtension.Language = lan.Key;
            db.WriteSetting("Language", lan);

        }

        private void PickerLanguages_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = PickerLanguages.SelectedIndex;

            //if (firstTime)
            //{
            //    firstTime = false;
            //    return;
            //}

            if (selectedIndex != -1)
            {
                CLanguage lan = langs[selectedIndex];
                Services.XDatabase db = Services.XServices.Instance.GetService<Services.XDatabase>();
                Services.TranslateExtension.Language = lan.Key;
                db.WriteSetting("Language", lan);

                // refresh page                
                //InitializeComponent();
                StartLanguages();

            }
        }

        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();

                (Xamarin.Forms.Application.Current.MainPage as PageRoot).IsGestureEnabled = false;

                Services.XDatabase db = Services.XServices.Instance.GetService<Services.XDatabase>();
                CLanguage lang = null;
                lang = db.ReadSetting("Language", lang);

                if (lang != null)
                    Services.TranslateExtension.Language = lang.Key;

                //InitializeComponent();

                //#if !DEBUG
                //this.StackLayoutSavePassword.IsVisible = false;
                //#endif

                //Set default espanol
                StartLanguages();

                /*if (lang != null)
                {
                    for (int i = 0; i < langs.Length; i++)
                    {
                        if (langs[i].LCID == lang.LCID)
                        {
                            PickerLanguages.SelectedIndex = i;
                        }
                    }
                }*/

                //Hide unnecessary UI elements
                PickerLanguages.IsVisible = false;
                StackLayoutSavePassword.IsVisible = false;
                LanguageLabel.IsVisible = false;
                //avoid password save if this is tablet

                RememberPassword = false;
                m_RememberPassword = false;
                //force blank password
                passWordEntry.Text = "";
            }
            catch (Exception e)
            {
                App.Current.MainPage.DisplayAlert("Error", e.Message, Services.TranslateExtension.TranslateText("OK"));
            }
        }

        protected override bool OnBackButtonPressed()
        {
            CloseApp();
            return true;
        }

        private async void CloseApp()
        {
            var result = await App.Current.MainPage.DisplayAlert("Salir", "Desea salir de la app?", "Si", "No");
            if (result)
            {
                if (Device.RuntimePlatform.Equals(Device.Android))
                    Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
            }
        }

    }

}