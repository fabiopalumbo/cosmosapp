using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComosWebSDK.Data;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XComosMobile.Pages.comos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NavigatorTabbed : TabbedPage
    {
        public NavigatorTabbed(CWorkingLayer layer, string language)
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            PageNavigator unit = new PageNavigator(layer, language) { Title = "Units" };
            PageNavigator location = new PageNavigator(layer, language, "L") { Title = "Locations" };
            PageNavigator docs = new PageNavigator(layer, language, "D") { Title = "Documents" };

            Children.Add(unit);
            Children.Add(location);
            Children.Add(docs);

        }
    }
}