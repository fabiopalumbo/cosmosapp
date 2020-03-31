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
    public class TextToBoolean : IValueConverter
    {
        public TextToBoolean()
        {
        }

        public object Convert(object value, Type targetType,
                              object parameter, CultureInfo culture)
        {
            string txt = value as string;
            int result = 0;

            if (int.TryParse(txt,out result))
            {
                return txt == "1";
            }
            else
            {
                return value.ToString().ToUpper() == "TRUE";
            }            
        }

        public object ConvertBack(object value, Type targetType,
                                  object parameter, CultureInfo culture)
        {
            if (!(value is Boolean))
                return "0";

            if((bool)value)
                return "1";
            return "0";
        }
    }
}
