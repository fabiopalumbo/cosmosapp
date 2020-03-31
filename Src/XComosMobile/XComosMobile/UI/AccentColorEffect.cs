using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XComosMobile.UI
{
    public static class SAccentColorEffect
    {
        public static readonly BindableProperty UseProperty =
            BindableProperty.CreateAttached("Use", typeof(bool), typeof(SAccentColorEffect), false, propertyChanged: OnUseChanged);

        public static bool GetUse(BindableObject view)
        {
            return (bool)view.GetValue(UseProperty);
        }
        public static void SetUse(BindableObject view, bool value)
        {
            view.SetValue(UseProperty, value);
        }
        static void OnUseChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as View;
            if (view == null)
            {
                return;
            }

            bool use = (bool)newValue;
            if (use)
            {
                view.Effects.Add(new AccentColorEffect());
            }
            else
            {
                var toRemove = view.Effects.FirstOrDefault(e => e is AccentColorEffect);
                if (toRemove != null)
                {
                    view.Effects.Remove(toRemove);
                }
            }
        }
    }

    public class AccentColorEffect : RoutingEffect
    {

        public bool Use { get; set; }

        public AccentColorEffect() : base("XComosMobile.AccentColorEffect")
        {
        }
    }
}
