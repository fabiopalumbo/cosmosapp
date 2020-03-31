using System;
using Android.Graphics;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Content.Res;
using XComosMobile.Droid.Renderer;

[assembly: ResolutionGroupName("XComosMobile")]
[assembly: ExportEffect(typeof(AccentColorEffect), "AccentColorEffect")]
namespace XComosMobile.Droid.Renderer
{

    class AccentColorEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            try
            {
                string name = Control.GetType().Name;
                switch (name)
                {
                    case "EditorEditText":
                    case "EditText":
                        {
                            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                                Control.BackgroundTintList = ColorStateList.ValueOf(Xamarin.Forms.Color.Accent.ToAndroid());
                            else
                                Control.Background.SetColorFilter(Xamarin.Forms.Color.Accent.ToAndroid(), PorterDuff.Mode.SrcAtop);
                        }
                        break;
                    case "SearchView":
                        {
                            var tmp = Control as Android.Widget.SearchView;
                            if (tmp != null)
                            {
                                var iconId = Control.Resources.GetIdentifier("android:id/search_mag_icon", null, null);
                                var icon = tmp.FindViewById<ImageView>(iconId);
                                if(icon != null)
                                    icon.ImageTintList = ColorStateList.ValueOf(Xamarin.Forms.Color.Accent.ToAndroid());

                                var plateId = Control.Resources.GetIdentifier("android:id/search_plate", null, null);
                                var plate = tmp.FindViewById<Android.Widget.LinearLayout>(plateId);
                                if (plate != null)
                                    plate.ForegroundTintList = ColorStateList.ValueOf(Xamarin.Forms.Color.Accent.ToAndroid());
                                if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                                {
                                    //ImageView searchButton = (ImageView)tmp.findViewById(android.support.v7.appcompat.R.id.search_button);

                                    tmp.BackgroundTintList = ColorStateList.ValueOf(Xamarin.Forms.Color.Accent.ToAndroid());
                                    tmp.ForegroundTintList = ColorStateList.ValueOf(Xamarin.Forms.Color.Accent.ToAndroid());
                                }
                                else
                                {
                                    tmp.Background.SetColorFilter(Xamarin.Forms.Color.Accent.ToAndroid(), PorterDuff.Mode.SrcAtop);
                                }
                            }
                        }

                        break;
                    case "FormsSeekBar":
                        {
                            var tmp = Control as Android.Widget.SeekBar;
                            if (tmp != null)
                            {
                                tmp.ProgressTintList =  ColorStateList.ValueOf(Xamarin.Forms.Color.Accent.ToAndroid());
                                tmp.ThumbTintList = ColorStateList.ValueOf(Xamarin.Forms.Color.Accent.ToAndroid());
                                tmp.BackgroundTintList = ColorStateList.ValueOf(Xamarin.Forms.Color.Accent.ToAndroid());
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        protected override void OnDetached()
        {
        }


    }
}