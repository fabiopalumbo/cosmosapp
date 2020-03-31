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
    public class PictureRefToImage : IValueConverter
    {
        string m_DatabaseKey = null;
       public PictureRefToImage()
        {
        }

        private ComosWebSDK.IComosWeb ComosWeb
        {
            get
            {
                return XServices.Instance.GetService<ComosWebSDK.IComosWeb>();
            }
        }
        public void InitConverter(string db)
        {
            m_DatabaseKey = db;
        }

        public object Convert(object value, Type targetType,
                              object parameter, CultureInfo culture)
        {
            string pictureref = value as string;

            if (pictureref == null)
                return null;

						try
						{
							return this.ComosWeb.GetPictureUrl(m_DatabaseKey, pictureref);
						}
						catch (Exception ex)
						{
							App.Current.MainPage.DisplayAlert("Error", "Error al cargar imagen: " + ex.Message, Services.TranslateExtension.TranslateText("OK"));
							return null;
						}
			
        }

        public string GetAsUrl(string picref)
        {
					try
					{
						return this.ComosWeb.GetPictureUrl(m_DatabaseKey, picref);
					}
					catch (Exception ex)
					{
						App.Current.MainPage.DisplayAlert("Error", "Error al cargar imagen: " + ex.Message, Services.TranslateExtension.TranslateText("OK"));
						return null;
					}
        }

        public object ConvertBack(object value, Type targetType,
                                  object parameter, CultureInfo culture)
        {
            // Not supported.
            return null;
        }
    }
}
