using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Graphics.Imaging;
//using Windows.Web.Http;
using Windows.Storage.Streams;
using System.Net.Http;
using System.Net;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Net.NetworkInformation;

using Plugin.Toasts;
using XComosMobile.Services;
using System.IO;

namespace XComosMobile.UWP
{
    public class PlatformSystem : XComosMobile.Services.IPlatformSystem
    {
        public void UseLogin(string domain, string password, string username)
        {
            var m_HttpHandler = new HttpClientHandler() { UseCookies = false, AllowAutoRedirect = true };
            m_HttpHandler.Credentials = new NetworkCredential()
            {
                Domain = domain,
                Password = password,
                UserName = username
            };
            var m_Http = new HttpClient(m_HttpHandler);
            if (XComosMobile.Services.XServices.Instance.GetService<HttpClient>() != null)
            {
                XComosMobile.Services.XServices.Instance.RemoveService<HttpClient>();
            }
            XComosMobile.Services.XServices.Instance.AddService<HttpClient>(m_Http);
            FFImageLoading.ImageService.Instance.Config.HttpClient = m_Http;
        }
        public async Task<bool> OpenFile(string filename, string contenttype)
        {
            bool success = false;
            var file = await ApplicationData.Current.LocalCacheFolder.GetFileAsync(filename);

            if (file != null)
            {
                success = await Windows.System.Launcher.LaunchFileAsync(file);                
            }
            
            return success;
        }

        bool IsOfflineManual = false;
        public bool IsOnline
        {
            get
            {
                return !IsOfflineManual && this.IsDeviceOnline;
            }
            set
            {
                IsOfflineManual = !value;
            }
        }

        void SendNotification(string title, string text)
        {
            throw new NotImplementedException();
        }
        public bool IsDeviceOnline
        {
            get
            {
                if (Windows.Networking.Connectivity.NetworkInformation.GetInternetConnectionProfile() != null)
                {
                    var connectivityLevel = Windows.Networking.Connectivity.NetworkInformation.GetInternetConnectionProfile().GetNetworkConnectivityLevel();
                    bool online = connectivityLevel == Windows.Networking.Connectivity.NetworkConnectivityLevel.InternetAccess;
                    return online;
                }
                else
                {
                    return false;
                }
            }
        }
        public async void ShowToast(string message)
        {

            var notificator =  Xamarin.Forms.DependencyService.Get<IToastNotificator>();

            var options = new NotificationOptions()
            {
                Description = message,

            };
        options.WindowsOptions.LogoUri = "pack://application:,,,/Assets/ComosLogo.png";

            var result = await notificator.Notify(options);

        }
        public string GetPersonalFolderPath()
        {
            return Windows.Storage.ApplicationData.Current.LocalCacheFolder.Path;
        }

        public void DeleteFile(string filename)
        {
            System.IO.File.Delete(filename);
        }

        public bool FileExists(string filename)
        {
            return System.IO.File.Exists(filename);
        }

        Pages.PageProgressDialog m_Progress = null;
        public void ShowProgressMessage(string message, bool spinner)
        {
            if (m_Progress == null)
            {
                m_Progress = new Pages.PageProgressDialog()
                {
                    WaitingMessage = message,
                    IsWaiting = true,
                };
                Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(m_Progress);

            }
            else
                m_Progress.WaitingMessage = message;
        }

        public void HideProgressMessage()
        {
            if (m_Progress != null)
            {
                Rg.Plugins.Popup.Services.PopupNavigation.PopAsync();
                m_Progress = null;
            }
        }

        public void IncrementProgress(int delta)
        {

        }

        public double GetScreenWidthPixels
        {
            get
            {
                return ((Frame)Window.Current.Content).ActualWidth;
            }
        }
        public double GetScreenHeightPixels
        {
            get
            {
                return ((Frame)Window.Current.Content).ActualHeight;
            }
        }

        public double GetScreenDensity
        {
            get
            {
                return 1; // Did not investigate
            }
        }

        public async Task<string> SaveAndOpenDocument(string filename, System.IO.Stream stream, string mimetype, bool open)
        {
            System.Diagnostics.Debug.WriteLine(ApplicationData.Current.LocalCacheFolder.Path + "\\" + filename);
            var outstream = System.IO.File.Create(ApplicationData.Current.LocalCacheFolder.Path + "\\" + filename);

            stream.CopyTo(outstream);
            outstream.Flush();
            outstream.Dispose();
            outstream = null;

            //var intent = new Intent(
            //    Application.Context,
            //    Intent.ActionView, Android.Net.Uri.FromFile(filename));
            ////intent.SetDataAndType(Android.Net.Uri.FromFile(f), "application/msword");
            //StartActivity(intent);

            var file = await ApplicationData.Current.LocalCacheFolder.GetFileAsync(filename);

            if (open)
            {

                if (file != null)
                {
                    // Launch the retrieved file
                    var success = await Windows.System.Launcher.LaunchFileAsync(file);
                    if (success)
                    {
                        // File launched
                    }
                    else
                    {
                        // File launch failed
                        ShowToast("Failed to open file");
                    }
                }
                else
                {
                    // Could not find file
                    ShowToast("Failed to save file");
                }
            }            
            return filename;
        }

    

        public void GalleryMedia()
        {
            throw new NotImplementedException();
        }

        public void CameraMedia()
        {
            throw new NotImplementedException();
        }

        public byte[] PathToByte(string path)
        {
            throw new NotImplementedException();
        }

        public void ShowPicture(string imageURL)
        {
            throw new NotImplementedException();
        }

        public Stream PathToStream(string path)
        {
            throw new NotImplementedException();
        }

        public void CameraMedia(string cameraform)
        {
            throw new NotImplementedException();
        }

        public Task<Stream> DownScaleImage(string path)
        {
            throw new NotImplementedException();
        }

        public string StartRecordingAudio()
        {
            throw new NotImplementedException();
        }

        public void StopRecordingAudio()
        {
            throw new NotImplementedException();
        }

        public void CancelRecording(string path)
        {
            throw new NotImplementedException();
        }

        public void PlayAudio(string path)
        {
           // throw new NotImplementedException();
        }

        public void StopAudio()
        {
           // throw new NotImplementedException();
        }

        public void startworker(string[] uids)
        {
           // throw new NotImplementedException();
        }

        public void SendNotification(string title, string text, bool sound = false, bool progress = false, int total = 0, int partial = 0, int percentMax = 100)
        {
           // throw new NotImplementedException();
        }

        public void SetCurrentThemme(Theming.Themes theme)
        {
            //throw new NotImplementedException();
        }

        bool IPlatformSystem.StopRecordingAudio()
        {
            throw new NotImplementedException();
        }

        public string AppVersionName
        {
            get {
                return Windows.ApplicationModel.Package.Current.Id.Version.ToString();
            }
        }
    }
}
