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
using Android.Content.Res;
using Android.Graphics;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using XComosMobile.Droid.Renderer;

[assembly: ExportRenderer(typeof(Entry), typeof(XEntryRenderer))]
namespace XComosMobile.Droid.Renderer
{
    class XEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            
            if (Control != null)
            {
                if (Control == null || e.NewElement == null) return;

                if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                    Control.BackgroundTintList = ColorStateList.ValueOf(Xamarin.Forms.Color.Accent.ToAndroid());
                else
                    Control.Background.SetColorFilter(Xamarin.Forms.Color.Accent.ToAndroid(), PorterDuff.Mode.SrcAtop);
            }
        }
    }
}