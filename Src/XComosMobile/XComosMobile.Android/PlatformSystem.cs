using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using System.Net.Http;
using System.Net;
using Android.Net;
using System.Threading.Tasks;
using Environment = Android.OS.Environment;
using Uri = Android.Net.Uri;
using Android.Provider;
using System.IO;
using FFImageLoading;
using XComosMobile.Services;
using ComosWebSDK;
using NtlmHttpHandler;

namespace XComosMobile.Droid
{
    public class PlatformSystem: XComosMobile.Services.IPlatformSystem
    {
        Activity m_Activity = null;
        ConnectivityManager m_ConnectivityManager;
        bool saveAudio = false;
        System.Timers.Timer timer = new System.Timers.Timer();

        private string _recordingFile = "";
        Android.Media.MediaRecorder _recorder;
        Android.Media.MediaPlayer _player;

        int spinners = 0;

        private Theming.Themes theme = Theming.Themes.Siemens;
        public void SetCurrentThemme(Theming.Themes theme)
        {
            this.theme = theme;
        }


        public void PlayAudio(string filepath)
        {
            _player.Stop();
            _player.Reset();
            if (FileExists(filepath))
            {
                _player.SetDataSource(Application.Context, Uri.Parse(filepath));
                _player.Prepare();
                _player.Start();
            }            
        }
        public string StartRecordingAudio()
        {
            string fullfilename = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath
                + "/" + Android.OS.Environment.DirectoryDownloads;            
            fullfilename += "/" + String.Format("myAudio_{0}.mp4", Guid.NewGuid());

            try
            {
                _recorder = new Android.Media.MediaRecorder();
                _recorder.SetAudioSource(Android.Media.AudioSource.Mic);
                _recorder.SetOutputFormat(Android.Media.OutputFormat.Mpeg4);
                _recorder.SetAudioEncoder(Android.Media.AudioEncoder.AmrNb);
                _recorder.SetOutputFile(fullfilename);
                _recorder.Prepare();
                _recorder.Start();
                timer.Start();
                saveAudio = false;
                _recordingFile = fullfilename;
                return fullfilename;
            }
            catch (Exception)
            {

                return "";
            }

            
        }

        public bool StopRecordingAudio()
        {
            bool saved = false;
            try
            {
                _recorder.Stop();
                _recorder.Reset();
                                
            }
            catch (Exception)
            {                
            }
            finally
            {
                if (!saveAudio)
                    CancelRecording(_recordingFile);
                else
                    saved = true;

                timer.Stop();
                _recordingFile = "";                
                saveAudio = false;                
            }

            return saved;

        }

        public void CancelRecording(string path)
        {
            //StopRecordingAudio();
            Java.IO.File f = new Java.IO.File(path);
            if (f.Exists())
            f.Delete();
        }

        public PlatformSystem(Activity activity)
        {
            m_Activity = activity;
            this.m_ConnectivityManager = (ConnectivityManager)Application.Context.GetSystemService(Context.ConnectivityService);

            //record audios
            timer.Interval = 1000;
            timer.Elapsed += Timer_Elapsed;
            

            _recorder = new Android.Media.MediaRecorder();            
            _player = new Android.Media.MediaPlayer();

            _player.Completion += (sender, e) => {
                _player.Reset();                
            };

        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            saveAudio = true;
            timer.Stop();
        }

        public void StopAudio()
        {
            _player.Stop();
            _player.Reset();

        }

        public async Task<bool> OpenFile(string filename, string contenttype)
        {

            Java.IO.File f = new Java.IO.File(filename);

            if (!f.Exists())
            {
                string fullfilename = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath
                + "/" + Android.OS.Environment.DirectoryDownloads;
                fullfilename += "/" + filename;
                f = new Java.IO.File(fullfilename);
            }
            
                if (f.Exists())
                {
                    Intent intent = new Intent(Intent.ActionView);
                    intent.SetDataAndType(Android.Net.Uri.FromFile(f), contenttype);
                    Intent intent1 = Intent.CreateChooser(intent, Services.TranslateExtension.TranslateText("open_with"));
                    intent1.SetFlags(ActivityFlags.NewTask);
                    Application.Context.StartActivity(intent1);
                return true;
                }
            return false;
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
        public bool IsDeviceOnline
        {
            get
            {
                var networkinfo = this.m_ConnectivityManager.ActiveNetworkInfo;
                if ((networkinfo == null) || !networkinfo.IsConnected)
                    return false;
                return true;
            }
        }

        public void UseLogin(string domain, string password, string username)
        {
            var m_HttpHandler = NtlmHttpHandlerFactory.Create();
            m_HttpHandler.UseCookies = true;
            m_HttpHandler.AllowAutoRedirect = true;
            m_HttpHandler.Credentials = new NetworkCredential()
            {
                Domain = domain,
                Password = password,
                UserName = username
            };
            var m_Http = new HttpClient(m_HttpHandler);
            m_Http.Timeout = TimeSpan.FromMinutes(2);
            if (XComosMobile.Services.XServices.Instance.GetService<HttpClient>() != null)
            {
                XComosMobile.Services.XServices.Instance.RemoveService<HttpClient>();
            }
            XComosMobile.Services.XServices.Instance.AddService<HttpClient>(m_Http);
            FFImageLoading.ImageService.Instance.Config.HttpClient = m_Http;
        }

        public void ShowToast(string message)
        {
            Toast.MakeText(m_Activity, message, ToastLength.Long).Show();
        }
        public string GetPersonalFolderPath()
        {
            return System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        }

        public void DeleteFile(string filename)
        {
            System.IO.File.Delete(filename);
        }

        public bool FileExists(string filename)
        {
            return System.IO.File.Exists(filename);
        }

        private Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            return imageBitmap;
        }

        Android.App.ProgressDialog m_Progress;


        public void ShowProgressMessage(string message, bool spinner)
        {
            spinners++;

            if (m_Progress == null)
            {
                switch (theme)
                {
                    case Theming.Themes.Comos:
                        m_Progress = new ProgressDialog(m_Activity, ProgressDialog.ThemeHoloLight);
                        break;
                    case Theming.Themes.Siemens:
                        m_Progress = new ProgressDialog(m_Activity, ProgressDialog.ThemeDeviceDefaultLight);
                        break;
                    case Theming.Themes.Light:
                        
                        m_Progress = new ProgressDialog(m_Activity, ProgressDialog.ThemeHoloDark);
                        break;
                    case Theming.Themes.Black:
                        m_Progress = new ProgressDialog(m_Activity, ProgressDialog.ThemeDeviceDefaultDark);
                        break;
                    default:
                        m_Progress = new ProgressDialog(m_Activity);
                        break;
                }
                

                m_Progress.Indeterminate = true;
                if(spinner)
                    m_Progress.SetProgressStyle(Android.App.ProgressDialogStyle.Spinner);
                else
                    m_Progress.SetProgressStyle(Android.App.ProgressDialogStyle.Horizontal);
                m_Progress.SetMessage(message);
                m_Progress.SetCancelable(false);
                m_Progress.SetMessage(message);
                m_Progress.Show();
            }
            else
                m_Progress.SetMessage(message);
        }

        public void SendNotification(string title, string text, bool sound = false, bool progress = false, int total = 0, int partial = 0, int percentMax = 100)
        {

            // Instantiate the builder and set notification elements:
            Notification.Builder builder = new Notification.Builder(Application.Context)
            .SetContentTitle(title).SetContentText(text)
            .SetSmallIcon(Resource.Drawable.icon);//.SetTicker("");

            // Build the notification:
            //Notification notification = builder.Build();

            // Get the notification manager:
            NotificationManager notificationManager =
            Application.Context.GetSystemService(Context.NotificationService) as NotificationManager;

            // Publish the notification:
            const int notificationId = 2468;

            if (progress)
                builder.SetProgress(percentMax, percentMax * partial / total, false);

            if (sound)
                builder.SetSound(Android.Media.RingtoneManager.GetDefaultUri(Android.Media.RingtoneType.Notification));

            notificationManager.Notify(notificationId, builder.Build());

            builder.SetWhen(DateTime.Now.Millisecond);

    }
       
        public void HideProgressMessage()
        {
            if (m_Progress != null)
            {
                if(spinners ==1)
                {
                    m_Progress.Dismiss();
                    spinners = 0;
                    m_Progress = null;
                }
                else
                {
                    spinners--;
                }
               
            }
        }

        public void IncrementProgress(int delta)
        {
            if (m_Progress != null)
            {
                m_Progress.IncrementProgressBy(delta);
            }
        }

        public double GetScreenWidthPixels
        {
            get
            {
                return m_Activity.Resources.DisplayMetrics.WidthPixels / m_Activity.Resources.DisplayMetrics.Density;
            }
        }
        public double GetScreenHeightPixels
        {
            get
            {
                return m_Activity.Resources.DisplayMetrics.HeightPixels / m_Activity.Resources.DisplayMetrics.Density;
            }
        }

        public double GetScreenDensity
        {
            get
            {
                return m_Activity.Resources.DisplayMetrics.Density;
            }
        }

        public void CameraMedia(string CameraForm)
        {



            Intent intent = new Intent(MediaStore.ActionImageCapture);
            AppClass._file = new Java.IO.File(AppClass._dir, String.Format("myPhoto_{0}.jpg", Guid.NewGuid()));

            string fullfilename = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath
                + "/" + Android.OS.Environment.DirectoryDownloads;
            fullfilename += "/" + String.Format("myPhoto_{0}.jpg", Guid.NewGuid());

            AppClass._file = new Java.IO.File(fullfilename);
            
            intent.PutExtra(MediaStore.ExtraOutput, Uri.FromFile(AppClass._file));
            MainActivity.CameraForm = CameraForm;

            m_Activity.StartActivityForResult(intent, 0);            

        }

        public void GalleryMedia()
        {
            var imageIntent = new Intent();
            imageIntent.SetType("image/*");
            imageIntent.SetAction(Intent.ActionGetContent);
            ((Activity)Xamarin.Forms.Forms.Context).StartActivityForResult(Intent.CreateChooser(imageIntent, "Select photo"), 1);
        }

        public async System.Threading.Tasks.Task<string> SaveAndOpenDocument(
            string filename, System.IO.Stream stream, string mimetype, bool open)
        {
            try
            {
                string fullfilename = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath
                    + "/" + Android.OS.Environment.DirectoryDownloads;
                fullfilename += "/" + filename;

                var outstream = System.IO.File.Create(fullfilename);
                stream.CopyTo(outstream);
                outstream.Flush();
                outstream.Dispose();
                outstream = null;

                if (open)
                {
                    Java.IO.File f = new Java.IO.File(fullfilename);

                    if (f.Exists())
                    {
                        Intent intent = new Intent(Intent.ActionView);
                        intent.SetDataAndType(Android.Net.Uri.FromFile(f), mimetype);
                        Intent intent1 = Intent.CreateChooser(intent, Services.TranslateExtension.TranslateText("open_with"));
                        intent1.SetFlags(ActivityFlags.NewTask);
                        Application.Context.StartActivity(intent1);
                    }
                }

                return fullfilename;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return await Task.FromResult<string>(null);
        }


        public void ShowPicture(string imageURL)
        {
            Intent intent = new Intent();
            intent.AddFlags(ActivityFlags.NewTask);
            intent.AddFlags(ActivityFlags.GrantReadUriPermission);
            intent.SetAction(Intent.ActionView);
            intent.SetDataAndType(Android.Net.Uri.Parse("file://" + imageURL), "image/*");
            Application.Context.StartActivity(intent);

        }

        public Stream PathToStream(string path)
        {

            FileStream file = File.Open(path, FileMode.Open);

            return file;

        }
        public byte[] PathToByte(string path)
        {
            byte[] bitmapData = new byte[0];

            if (File.Exists(path))
            {
                using (var stream = new MemoryStream())
                {
                    Bitmap bmp = path.LoadAndResizeBitmap(70, 70);
                    bmp.Compress(Bitmap.CompressFormat.Png, 50, stream);
                    bitmapData = stream.ToArray();
                }

            }
            //bitmapData = System.IO.File.ReadAllBytes(path);
            return bitmapData;
        }

        public async Task<Stream> DownScaleImage(string path)
        {
            var photo = await FFImageLoading.ImageService.Instance
               .LoadFile(path)
               .DownSample(width: 600)
               .AsJPGStreamAsync(quality: 80);
            return photo;
        }

        public string AppVersionName
        {
            get {
                return Application.Context.ApplicationContext.PackageManager.GetPackageInfo(Application.Context.ApplicationContext.PackageName, 0).VersionName;
            }
        }

        public void startworker(string[] uids)
        {
           
            var activity = (Activity)Xamarin.Forms.Forms.Context;
            var intent = new Intent(activity, typeof(PeriodicService));
            intent.PutExtra("uids", uids);
            activity.StartService(intent);
        }

    }

    [Service]
    public class PeriodicService : Service
    {
        public override IBinder OnBind(Intent intent)
        {
            //using Newtonsoft.Json;
            //intent.PutExtra("User",JsonConvert.SerializeObject(user));

            return null;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {

            CheckAndDownloadAditionalContent downloader = new CheckAndDownloadAditionalContent();
            IComosWeb m_ComosWeb = Services.XServices.Instance.GetService<IComosWeb>();
            var platform = Services.XServices.Instance.GetService<Services.IPlatformSystem>();

            platform.SendNotification("COMOS MRO", Services.TranslateExtension.TranslateText("downloading_tasks"));

            Task.Run(async () =>
            {

                string[] uids = intent.GetStringArrayExtra("uids");                

                m_ComosWeb.Lock();
                int i = 0;
                foreach (var item in uids)
                {
                    i++;
                    platform.SendNotification("COMOS MRO", Services.TranslateExtension.TranslateText("downloading_tasks"), false, true, uids.Length, i);

                    try
                    {
                        var page = await downloader.DownloadDeviceContent(item, false);

                        if (page == null)
                        {
                            platform.SendNotification("COMOS MRO", TranslateExtension.TranslateText("download_failed") + ": " + TranslateExtension.TranslateText("task_not_found"), true, true, uids.Length, 0, 0);
                            m_ComosWeb.UnLock();
                            return StartCommandResult.NotSticky;
                        }
                    }
                    catch (Exception ex)
                    {

                        platform.SendNotification("COMOS MRO", TranslateExtension.TranslateText("download_failed") + ": " + ex.Message, true, true, uids.Length, 0, 0);
                        m_ComosWeb.UnLock();
                        return StartCommandResult.NotSticky;
                    }
                   
                }
            
                m_ComosWeb.UnLock();
                platform.SendNotification("COMOS MRO", Services.TranslateExtension.TranslateText("download_tasks_done"),true,true, uids.Length, 0,0);

                return StartCommandResult.NotSticky;

            });

            return StartCommandResult.NotSticky;

        }   

    }
}