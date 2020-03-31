using System;
using Xamarin.Forms.Platform.Android;
using Android.Widget;
using Android.Graphics;
using Xamarin.Forms;
using XComosMobile.Droid.Renderer;
using XComosMobile.Pages.controls;
using Android.OS;
using Android.App;
using Android.Graphics.Drawables;
using Android.Webkit;
using Android.Annotation;



[assembly: ExportRenderer(typeof(CustomWebView), typeof(CustomWebViewRenderer))]
[assembly: ExportRenderer(typeof(AFLabel), typeof(AFLabelRenderer))]
[assembly: ExportRenderer(typeof(AFButton), typeof(AFButtonRenderer))]
[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
[assembly: ExportRenderer(typeof(CustomEditor), typeof(CustomEditorRenderer))]
[assembly: ExportRenderer(typeof(CustomPicker), typeof(CustomPickerRenderer))]
[assembly: ExportRenderer(typeof(CustomStackLayout), typeof(CustomStackLayoutRenderer))]
[assembly: ExportRenderer(typeof(CustomFrame), typeof(CustomFrameRenderer))]

namespace XComosMobile.Droid.Renderer
{

    public class CustomWebViewRenderer : WebViewRenderer
    {
        Activity mContext;
        //public CustomWebViewRenderer(Android.Content.Context context)  : base(context)
        //{
        //    this.mContext = context as Activity;
        //}
        protected async override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
        {
            base.OnElementChanged(e);

            var custom = e.NewElement as Pages.controls.CustomWebView;
            await custom.RequestPermissions();

            Control.Settings.JavaScriptEnabled = true;
            Control.ClearCache(true);
            Control.SetWebChromeClient(new MyWebClient(((Activity)Forms.Context)));
        }
        public class MyWebClient : WebChromeClient
        {
            Activity mContext;
            public MyWebClient(Activity context)
            {
                this.mContext = context;                
                //this.OnPermissionRequest();
            }
            [TargetApi(Value = 21)]
            public override void OnPermissionRequest(PermissionRequest request)
            {

                string[] PermissionsAndroid =
                {
                  Android.Manifest.Permission.CaptureAudioOutput,
                  Android.Manifest.Permission.Vibrate,
                  Android.Manifest.Permission.ModifyAudioSettings,                  
                  Android.Manifest.Permission.RecordAudio,
                  Android.Manifest.Permission.WriteExternalStorage,
                  Android.Manifest.Permission.AccessNetworkState,
                  Android.Manifest.Permission.MediaContentControl,
                  Android.Manifest.Permission.Camera,
                  PermissionRequest.ResourceVideoCapture,
                  PermissionRequest.ResourceAudioCapture
                };

                mContext.RunOnUiThread(() => {
                    request.Grant(PermissionsAndroid);

                });

                
                    //var statusCamera = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
                    //var statusMicrophone = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Microphone);
                    //return statusCamera == PermissionStatus.Granted && statusMicrophone == PermissionStatus.Granted;
                


                /*
                mContext.RunOnUiThread(() => {
                    request.Grant(request.GetResources());

                });
                */

            }
        }

    }
    public class CustomFrameRenderer : FrameRenderer
    {
        CustomFrame custom;
        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);
            custom = e.NewElement as CustomFrame;
        }
        public CustomFrameRenderer()
        {
            this.LongClick += (object sender, LongClickEventArgs args) => 
            {
                if (custom != null)
                    custom.OnLongClick();
            };
        }
    }
    public class CustomStackLayoutRenderer : VisualElementRenderer<StackLayout>
    {
        CustomStackLayout custom;
        protected override void OnElementChanged(ElementChangedEventArgs<StackLayout> e)
        {
            base.OnElementChanged(e);
            custom = e.NewElement as CustomStackLayout;            
        }
        public CustomStackLayoutRenderer()
        {
            this.LongClick += (object sender, LongClickEventArgs args) =>
            {
                if(custom != null)
                custom.OnLongClick();
            };
        }
    }
    public class CustomPickerRenderer : PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.Background = Resources.GetDrawable(Resource.Layout.CustomEntryDrawable);
                                
            }
        }
    }
    public class CustomEditorRenderer : EditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {                
                Control.Background = Resources.GetDrawable(Resource.Layout.CustomEntryDrawable);
            }

        }
    }
        public class CustomEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                //Control.SetBackgroundColor(global::Android.Graphics.Color.LightGreen);

                Control.Background = Resources.GetDrawable(Resource.Layout.CustomEntryDrawable);
            }
        }
    }
    public class AFLabelRenderer : LabelRenderer
    {
        
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            var label = (TextView)Control;
            var text = label.Text;
            if (text.Length > 1 || text[0] < 0xf000)
            {
                return;
            }
            var font = Typeface.CreateFromAsset(Xamarin.Forms.Forms.Context.ApplicationContext.Assets, "fontawesome.ttf");
            label.Typeface = font;
        }
    }

    public class AFButtonRenderer : ButtonRenderer
    {

        private bool basecalled = false;
        bool animate = false;

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            try
            {                
                if (!basecalled)
                {
                    base.OnElementChanged(e);                    
                    basecalled = true;
                }

                var customButton = e.NewElement as Pages.controls.AFButton;
                var thisButton = Control as Android.Widget.Button;

                //e.NewElement.BorderRadius = 150;

                if (!customButton.IsJustClick)
                {

                    float position = 0;

                    thisButton.Touch += async (object sender, TouchEventArgs args) =>
                    {
                        if (args.Event.Action == Android.Views.MotionEventActions.Down)
                        {
                            DoAnimate(customButton);
                            await customButton.ScaleTo(1.5, 5);
                            customButton.OnPressed();
                        }
                        else if (args.Event.Action == Android.Views.MotionEventActions.Up)
                        {
                            animate = false;
                            await customButton.ScaleTo(1, 5);

                            if (position > args.Event.GetX() + customButton.Width)
                            {
                                var x = customButton.X;

                                var activity = (Activity)Forms.Context;
                                var vibrator = (Vibrator)activity.GetSystemService("vibrator");
                                vibrator.Vibrate(20);

                                customButton.OnDragLeft();
                            }
                            else
                            {
                                customButton.OnReleased();
                            }
                            position = 0;
                        }
                    };
                }

                var label = (TextView)Control;
                var text = label.Text;
                if (text.Length > 1 || text[0] < 0xf000)
                {
                    return;
                }
                var font = Typeface.CreateFromAsset(Xamarin.Forms.Forms.Context.ApplicationContext.Assets, "fontawesome.ttf");
                label.Typeface = font;
                label.SetPadding(0, 0, 0, 0);
            }
            catch (Exception)
            {
                
            }
        }

        private async void DoAnimate(AFButton bt)
        {
            while (animate)
            {
                await bt.FadeTo(0, 300, Easing.Linear);
                await bt.FadeTo(1, 750, Easing.Linear);
            }
        }
    }


}