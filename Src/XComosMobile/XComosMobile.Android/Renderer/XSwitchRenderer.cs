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

[assembly: ExportRenderer(typeof(Xamarin.Forms.Switch), typeof(XSwitchRenderer))]
namespace XComosMobile.Droid.Renderer
{
    class XSwitchRenderer : SwitchRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Switch> e)
        {
            base.OnElementChanged(e);

            if (Control == null || e.NewElement == null) return;
            ChangeColorSwitchCompat();
            e.NewElement.Toggled += SwitchCompat_CheckedChange;

            //if (Control != null)
            //{
            //    if (Control == null || e.NewElement == null) return;

            //    if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            //        Control.BackgroundTintList = ColorStateList.ValueOf(Xamarin.Forms.Color.Accent.ToAndroid());
            //    else
            //        Control.Background.SetColorFilter(Xamarin.Forms.Color.Accent.ToAndroid(), PorterDuff.Mode.SrcAtop);
            //}
        }

        private void SwitchCompat_CheckedChange(object sender, ToggledEventArgs e)
        {
            ChangeColorSwitchCompat();
        }

        private void ChangeColorSwitchCompat()
        {
            //var swicthcompat = Control as Android.Support.V7.Widget.SwitchCompat;

            if (Control.Checked)
            {
                if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                {
                    Control.ThumbDrawable.SetTint(Xamarin.Forms.Color.Accent.ToAndroid());
                    Control.TrackDrawable.SetTint(Xamarin.Forms.Color.Accent.MultiplyAlpha(0.6f).ToAndroid());
                }
                else
                {
                    Control.ThumbDrawable.SetColorFilter(Xamarin.Forms.Color.Accent.ToAndroid(), PorterDuff.Mode.SrcAtop);
                    Control.TrackDrawable.SetColorFilter(Xamarin.Forms.Color.Accent.MultiplyAlpha(0.6f).ToAndroid(), PorterDuff.Mode.SrcAtop);
                }
            }
            else
            {
                if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                {
                    Control.TrackDrawable.SetTint(Android.Graphics.Color.LightGray);
                    Control.ThumbDrawable.SetTint(Android.Graphics.Color.Gray);
                }
                else
                {
                    Control.TrackDrawable.SetColorFilter(Android.Graphics.Color.LightGray, PorterDuff.Mode.SrcAtop);
                    Control.ThumbDrawable.SetColorFilter(Android.Graphics.Color.Gray, PorterDuff.Mode.SrcAtop);
                }
            }
        }
    }
}