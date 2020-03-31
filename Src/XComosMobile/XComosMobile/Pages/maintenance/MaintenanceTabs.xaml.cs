using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XComosMobile.Pages.maintenance
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MaintenanceTabs : PageTemplate
    {
        public MaintenanceTabs()
        {
            InitializeComponent();
        }

        async void Tasks_Clicked(object sender, EventArgs e)
        {
            /*Frame bt = (Frame)sender;
            await FadeOutIn(bt);
            await App.Navigation.PushAsync(new Pages.maintenance.TasksPage());*/
        }

        async void Workpackages_Clicked(object sender, EventArgs e)
        {
            Frame bt = (Frame)sender;
            await FadeOutIn(bt);
            await App.Navigation.PushAsync(new Pages.maintenance.WorkPackages());
        }

        async void Inspections_Clicked(object sender, EventArgs e)
        {
            Frame bt = (Frame)sender;
            await FadeOutIn(bt);
            await App.Navigation.PushAsync(new Pages.inspection.InspectionPage());
        }

        protected override bool OnBackButtonPressed()
        {
            var web = Services.XServices.Instance.GetService<ComosWebSDK.IComosWeb>();
            this.Navigation.PushAsync(new PageSolutions(web));
            return true;
        }

    }
}