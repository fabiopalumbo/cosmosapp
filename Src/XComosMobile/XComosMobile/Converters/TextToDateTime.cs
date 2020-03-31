using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Xamarin.Forms;
using XComosMobile.Services;
using FFImageLoading;

namespace XComosMobile.Converters
{
    public class TextToDateTime : IValueConverter
    {
        public TextToDateTime()
        {
        }

        public object Convert(object value, Type targetType,
                              object parameter, CultureInfo culture)
        {
            string txt = value as string;
            DateTime dt = new DateTime();
            DateTime.TryParse(txt, out dt);
            return dt;
        }

        public object ConvertBack(object value, Type targetType,
                                  object parameter, CultureInfo culture)
        {
            if (!(value is DateTime))
                return new DateTime();

            return ((DateTime)value).ToString();
        }
    }
}
