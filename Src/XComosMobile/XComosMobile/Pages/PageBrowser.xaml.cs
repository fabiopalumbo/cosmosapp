using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XComosMobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageBrowser : PageTemplate
    {
        private Services.XDatabase db { get { return Services.XServices.Instance.GetService<Services.XDatabase>(); } }
        public PageBrowser()
        {
            InitializeComponent();
            webView.Navigated += WebView_Navigated;
            urlEntry.Text = db.ReadSetting("URL", @"http://www.siemens.com");

            if (urlEntry.Text != "")
                NavigateToPage(urlEntry.Text);
        }

        private void WebView_Navigated(object sender, WebNavigatedEventArgs e)
        {
            //HideSpinner();
        }

        private void searchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            if (urlEntry.Text != "")
            {
                NavigateToPage(urlEntry.Text);
            }
        }

        private void NavigateToPage(string url)
        {
            string text = url;

            //ShowSpinner(Services.TranslateExtension.TranslateText("searching"));

            if (!text.Contains(@"http://") && !text.Contains(@"https://"))
                text = @"http://" + text;

            webView.Source = new UrlWebViewSource() { Url = text };
            urlEntry.Text = text;
            db.WriteSetting("URL", text);

        }
    }
}