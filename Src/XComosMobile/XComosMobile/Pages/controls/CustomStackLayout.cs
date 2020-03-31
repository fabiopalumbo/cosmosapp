using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XComosMobile.Pages.controls
{
    public class CustomStackLayout : StackLayout
    {

        public event EventHandler LongClick;
        public object CustomData { get; set; }

        public bool IsPicture { get; set; }

        public bool IsAudio { get; set; }

        public virtual void OnLongClick()
        {
            LongClick?.Invoke(this, EventArgs.Empty);
        }

    }

    public class CustomFrame : Frame
    {

        public event EventHandler LongClick;
        public object CustomData { get; set; }

        public bool IsPicture { get; set; }

        public bool IsAudio { get; set; }

        public virtual void OnLongClick()
        {
            LongClick?.Invoke(this, EventArgs.Empty);
        }

    }
}
