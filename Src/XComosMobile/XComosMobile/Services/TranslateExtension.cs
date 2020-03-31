using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Reflection;
using System.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XComosMobile.Services
{
    // You exclude the 'Extension' suffix when using in Xaml markup
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        const string ResourceId = "XComosMobile.Properties.Resources";

        private static CultureInfo m_CI = null;
        public static string Language
        {
            get { return m_CI.Name; }
            set {
                if(value == "-")
                    m_CI = new CultureInfo("");
                else
                    m_CI = new CultureInfo(value);
            }
        }

        public static string TranslateText(string text)
        {
            if (text == null)
                return "";

            ResourceManager resmgr = new ResourceManager(ResourceId
                                , typeof(TranslateExtension).GetTypeInfo().Assembly);

            var translation = resmgr.GetString(text.Replace(' ', '_'), m_CI);

            if (translation == null)
            {
                translation = text;
            }

            return translation;
        }

        public TranslateExtension()
        {
            
            if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.Android || Device.RuntimePlatform == Device.UWP)
            {
                //ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
                //ci = new CultureInfo("pt-BR");
                if(m_CI == null)
                    m_CI = new CultureInfo("");
            }
            else if(m_CI == null)
                m_CI = new CultureInfo("");

        }

        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return "";

            ResourceManager resmgr = new ResourceManager(ResourceId
                                , typeof(TranslateExtension).GetTypeInfo().Assembly);

            var translation = resmgr.GetString(Text.Replace(' ','_'), m_CI);

            if (translation == null)
            {
#if DEBUG_IDIOMAS
                //throw new ArgumentException(
                //    String.Format("Key '{0}' was not found in resources '{1}' for culture '{2}'.", Text, ResourceId, ci.Name),
                //    "Text");
                translation = Text.Replace('e', 'è').Replace('S', '$').Replace('o', '0').Replace('a', 'â');
                translation = new string(
                                    translation.ToCharArray().Select(c => char.IsLetter(c) ? (char.IsUpper(c) ?
                                    char.ToLower(c) : char.ToUpper(c)) : c).ToArray());
#else
                translation = Text; // HACK: returns the key, which GETS DISPLAYED TO THE USER
#endif
            }
            return translation;
        }
    }
}
