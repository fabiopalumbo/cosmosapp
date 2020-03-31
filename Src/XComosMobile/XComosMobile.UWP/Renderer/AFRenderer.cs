using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Xamarin.Forms.Platform.UWP;
using Xamarin.Forms;
using XComosMobile.UWP.Renderer;
using XComosMobile.Pages.controls;

[assembly: ExportRenderer(typeof(AFLabel), typeof(AFLabelRenderer))]
[assembly: ExportRenderer(typeof(AFButton), typeof(AFButtonRenderer))]

namespace XComosMobile.UWP.Renderer
{
    public class AFLabelRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                var font = new FontFamily(@"/Assets/Fonts/fontawesome.ttf#FontAwesome");

                //if (e.NewElement != null)
                //{
                //    switch (e.NewElement.FontAttributes)
                //    {
                //        case FontAttributes.None:
                //            break;
                //        case FontAttributes.Bold:
                //            //set bold font etc
                //            break;
                //        case FontAttributes.Italic:
                //            //set italic font etc
                //            break;
                //        default:
                //            break;
                //    }
                //}
                Control.FontFamily = font;
            }
        }
    }

    public class AFButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                var font = new FontFamily(@"/Assets/Fonts/fontawesome.ttf#FontAwesome");

                //if (e.NewElement != null)
                //{
                //    switch (e.NewElement.FontAttributes)
                //    {
                //        case FontAttributes.None:
                //            break;
                //        case FontAttributes.Bold:
                //            //set bold font etc
                //            break;
                //        case FontAttributes.Italic:
                //            //set italic font etc
                //            break;
                //        default:
                //            break;
                //    }
                //}
                Control.FontFamily = font;
            }
        }
    }
}
