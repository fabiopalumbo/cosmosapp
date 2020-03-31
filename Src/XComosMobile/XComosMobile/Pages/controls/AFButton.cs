using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XComosMobile.Pages.controls
{
    public class AFButton: Button
    {

        public event EventHandler Pressed;
        public event EventHandler Released;
        public event EventHandler DragLeft;

        public bool IsJustClick { get; set; }

        public AFButton()
        {
            IsJustClick = true;
        }

        public virtual void OnPressed()
        {
            Pressed?.Invoke(this, EventArgs.Empty);
        }

        public virtual void OnReleased()
        {
            Released?.Invoke(this, EventArgs.Empty);
        }

        public virtual void OnDragLeft()
        {
            DragLeft?.Invoke(this, EventArgs.Empty);
        }
    }
}
