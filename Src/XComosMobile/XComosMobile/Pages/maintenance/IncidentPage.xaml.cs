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
    public partial class IncidentPage : PageTemplate
    {
        public IncidentPage()
        {
            InitializeComponent();
        }

        async void Create_New_Incident_Clicked(object sender, EventArgs e)
        {
            Frame bt = (Frame)sender;
            await FadeOutIn(bt);
            await App.Navigation.PushAsync(new Pages.maintenance.IncidentsTypes());
        }

        async void Event_Container_Clicked(object sender, EventArgs e)
        {
            Frame bt = (Frame)sender;
            await FadeOutIn(bt);
            await App.Navigation.PushAsync(new Pages.maintenance.EventContainer());
        }

    }
}