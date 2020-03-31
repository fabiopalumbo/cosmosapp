using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XComosMobile.Behaviors
{
    public class NumericEntryLengthValidatorBehavior : Behavior<Entry>
    {
        public int MaxLength { get; set; }

        protected override void OnAttachedTo(Entry bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.TextChanged += OnEntryTextChanged;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.TextChanged -= OnEntryTextChanged;
        }

        void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = (Entry)sender;
            string entryText = entry.Text;

            if (entry.Text.Contains("."))
            {
                entryText = entryText.Remove(entryText.IndexOf(".", StringComparison.InvariantCulture), 1);
                entry.Text = entryText;
                ShowToast($"Sólo se pueden ingresar números enteros positivos");
            }
            else if(entry.Text.Contains("-"))
            {
                entryText = entryText.Remove(entryText.IndexOf("-", StringComparison.InvariantCulture), 1);
                entry.Text = entryText;
                ShowToast($"Sólo se pueden ingresar números enteros positivos");
            }
            else if (entry.Text.Length > this.MaxLength)
            {
                entryText = entryText.Remove(entryText.Length - 1); // remove last char
                entry.Text = entryText;
                ShowToast($"Se pueden ingresar un máximo de {MaxLength} números");
            }
        }

        public void ShowToast(string message)
        {
            var platform = Services.XServices.Instance.GetService<Services.IPlatformSystem>();
            platform.ShowToast(message);
        }
    }
}
