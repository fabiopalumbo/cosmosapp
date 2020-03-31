using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Net;
using Android.Support.V7.AppCompat;
using Android.Content;
using Android.Database;
using Android.Provider;
using System.IO;
using Android.Graphics;
using Plugin.CurrentActivity;
using Xamarin.Forms;
using XComosMobile.Services;

namespace XComosMobile.Droid
{
    public static class AppClass
    {
        public static Java.IO.File _file;
        public static Java.IO.File _dir;
        public static Android.Graphics.Bitmap bitmap;
    }

    [Activity(Label = "XComosMobile", Theme = "@style/splashscreen", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        
        public bool IsCamera;
        protected override void OnCreate(Bundle bundle)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironment_UnhandledExceptionRaiser;

            // bug with URI at android 7 :: https://stackoverflow.com/questions/44434150/android-application-crashes-while-launch-camera-activity-visual-studio-xamarin
            StrictMode.VmPolicy.Builder builder = new StrictMode.VmPolicy.Builder();
            StrictMode.SetVmPolicy(builder.Build());

            base.Window.RequestFeature(WindowFeatures.ActionBar);
            base.SetTheme(Resource.Style.MainTheme);

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            FFImageLoading.Forms.Droid.CachedImageRenderer.Init();
            base.OnCreate(bundle);
            Rg.Plugins.Popup.Popup.Init(this, bundle);
            global::Xamarin.Forms.Forms.Init(this, bundle);

            ServicePointManager.ServerCertificateValidationCallback += (o, certificate, chain, errors) => true;
            if (null == XComosMobile.Services.XServices.Instance.GetService<XComosMobile.Services.IPlatformSystem>())
            {
                XComosMobile.Services.XServices.Instance.AddService<XComosMobile.Services.IPlatformSystem>(new PlatformSystem(this));
                XComosMobile.Services.XServices.Instance.AddService<XComosMobile.Services.IDatabase>(
                    (new Database()).Open("xcomos.db"));
            }
            ZXing.Mobile.MobileBarcodeScanner.Initialize(Application);
            CrossCurrentActivity.Current.Init(this, bundle);
            DependencyService.Register<IStatusBarStyleManager, StatusBarStyleManager>();
            LoadApplication(new App());

            // https://blog.xamarin.com/requesting-runtime-permissions-in-android-marshmallow/
            if ((int)Build.VERSION.SdkInt >= 23)
            {
                string[] PermissionsAndroid =
                {
                  Android.Manifest.Permission.CaptureAudioOutput,
                  Android.Manifest.Permission.Vibrate,
                  Android.Manifest.Permission.RecordAudio,
                  Android.Manifest.Permission.WriteExternalStorage,
                  Android.Manifest.Permission.AccessNetworkState,
                  Android.Manifest.Permission.ModifyAudioSettings,
                  Android.Manifest.Permission.MediaContentControl,                  
                  Android.Manifest.Permission.Camera,
                  
                };
                for (int i = 0; i < PermissionsAndroid.Length; i++)
                {
                    if (CheckSelfPermission(PermissionsAndroid[i]) == (int)Permission.Granted)
                    {
                        continue;
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("No Permission to " + PermissionsAndroid[i]);
                        RequestPermissions(PermissionsAndroid, i);
                    }
                }
            }
        }


        protected override void OnDestroy()
        {
            base.OnDestroy();

            var comosweb = Services.XServices.Instance.GetService<ComosWebSDK.IComosWeb>();
            if (comosweb != null)
            {
                comosweb.StopHeartbeat();
                // Make sure we are loging out, so the comos web license is released.
                System.Threading.Tasks.Task.Run(async () => { await comosweb.Logout(); });
            }


        }
        public override async void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            //if (grantResults[requestCode] == Permission.Granted)
            //{
            //    //Permission granted
            //    //var snack = Snackbar.Make(layout, "Location permission is available, getting lat/long.", Snackbar.LengthShort);
            //    //snack.Show();
            //    System.Diagnostics.Debug.WriteLine("Permission granted for " + permissions[requestCode]);
            //}
            //else
            //{
            //    //Permission Denied :(
            //    //Disabling location functionality
            //    //var snack = Snackbar.Make(layout, "Location permission is denied.", Snackbar.LengthShort);
            //    //snack.Show();
            //    System.Diagnostics.Debug.WriteLine("Permission NOT GRANTED for " + permissions[requestCode]);
            //}
        }


        private void AndroidEnvironment_UnhandledExceptionRaiser(object sender, RaiseThrowableEventArgs e)
        {
            //throw new NotImplementedException();
            Toast.MakeText(this, e.Exception.Message, ToastLength.Long).Show();
            

        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //throw new NotImplementedException();
            Toast.MakeText(this, e.ToString(), ToastLength.Long).Show();
        }

        public static string CameraForm = null;
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {

            base.OnActivityResult(requestCode, resultCode, data);

            if (CameraForm != null)
            {
                Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
                Android.Net.Uri contentUri = Android.Net.Uri.FromFile(AppClass._file);
                mediaScanIntent.SetData(contentUri);
                SendBroadcast(mediaScanIntent);

                //int height = Resources.DisplayMetrics.HeightPixels;
                int width = Resources.DisplayMetrics.WidthPixels;
                AppClass.bitmap = AppClass._file.Path.LoadAndResizeBitmap(width, width);

                byte[] bitmapData = new byte[0];

                if (AppClass.bitmap != null)
                {
                    using (var stream = new MemoryStream())
                    {
                        AppClass.bitmap.Compress(Bitmap.CompressFormat.Png, 50, stream);
                        bitmapData = stream.ToArray();
                    }

                    AppClass.bitmap = null;
                }
              


                if (string.Compare(CameraForm, "PageNewDevice") == 0)
                {
                    XComosMobile.Pages.comos.PageNewDevice.SaveImage(AppClass._file.AbsolutePath);
                    XComosMobile.Pages.comos.PageNewDevice.Cameraimage(bitmapData, AppClass._file.AbsolutePath, DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss"));
                }
                else if (string.Compare(CameraForm, "PageAttributes") == 0)
                {
                    XComosMobile.Pages.comos.PageAttributes.SaveImage(AppClass._file.AbsolutePath);
                    XComosMobile.Pages.comos.PageAttributes.Cameraimage(bitmapData, AppClass._file.AbsolutePath, DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss"));
                }

                CameraForm = null;
            }
            else
            {
                if (requestCode == 1)
                {
                    if (resultCode == Result.Ok)
                    {
                        if (data.Data != null)
                        {
                            Android.Net.Uri uri = data.Data;

                            int orientation = getOrientation(uri);
                            BitmapWorkerTask task = new BitmapWorkerTask(this.ContentResolver, uri);
                            //task.Execute(orientation);
                            task.Execute();
                        }
                    }
                }
            }
        }

        public int getOrientation(Android.Net.Uri photoUri)
        {
            ICursor cursor = Application.ApplicationContext.ContentResolver.Query(photoUri, new String[] { MediaStore.Images.ImageColumns.Orientation }, null, null, null);

            if (cursor.Count != 1)
            {
                return -1;
            }

            cursor.MoveToFirst();
            return cursor.GetInt(0);
        }
    }
}


