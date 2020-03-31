using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XComosMobile.Pages.comos;

namespace XComosMobile.Pages.maintenance
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CachedIncidentsPage : PageTemplate
    {
        private string IncidentTypeUID;

        private bool isAnIncidentOfATask;
        private string taskUid;
        private string taskPic;
        private string taskOwner;

        private Services.XDatabase db { get { return Services.XServices.Instance.GetService<Services.XDatabase>(); } }
        private ViewModels.ProjectData ProjectData { get { return Services.XServices.Instance.GetService<ViewModels.ProjectData>(); } }

        public CachedIncidentsPage(String IncidentTypeUID)
        {
            InitializeComponent();
            this.IncidentTypeUID = IncidentTypeUID;
            isAnIncidentOfATask = false;
        }

        public CachedIncidentsPage(String IncidentTypeUID, string taskUid, string taskPic, string taskOwner)
        {
            InitializeComponent();
            this.IncidentTypeUID = IncidentTypeUID;
            isAnIncidentOfATask = true;
            this.taskUid = taskUid;
            this.taskPic = taskPic;
            this.taskOwner = taskOwner;
        }


        private void GetIncidents()
        {
            ListViewObjects.ItemsSource = db.GetCachedDevices().Where(p => p.ProjectUID == ProjectData.SelectedProject.UID && p.OverlayUID == ProjectData.SelectedLayer.UID && p.CDevUID == IncidentTypeUID);
        }

        private async void Button_Clicked_New_Incident(object sender, EventArgs e)
        {
            /*bool answer = false;

            if (CheckOnline())
            {
                answer = await DisplayAlert(Services.TranslateExtension.TranslateText("create_incident"), Services.TranslateExtension.TranslateText("inform_tag_reference"), Services.TranslateExtension.TranslateText("yes"), Services.TranslateExtension.TranslateText("no"));
            }            
            
            if (answer)
            {
                //show search screen
                await this.Navigation.PushAsync(new IncidentsPage(IncidentTypeUID));
            }
            else
            {*/
            // create an event underneath a dummy node   
            ComosWebSDK.UI.UICachedScreen screen = db.GetCachedScreen(IncidentTypeUID);

            if (isAnIncidentOfATask)
            {
                await this.Navigation.PushAsync(new PageNewDevice(screen, taskUid, taskPic, taskOwner));
            }
            else
            {
                await this.Navigation.PushAsync(new PageNewDevice(screen, "DUMMY_INCIDENT", "DUMMY", Services.TranslateExtension.TranslateText("new_incident")));
            }
            //}            
        }

        private async void ListViewObjects_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ComosWebSDK.Data.CCachedDevice cached = (ComosWebSDK.Data.CCachedDevice)e.Item;
            ComosWebSDK.UI.UICachedScreen screen = db.GetCachedScreen(cached.CDevUID);

            await this.Navigation.PushAsync(new PageNewDevice(screen, cached.OwnerUID, cached.OwnerPicture, cached.OwnerName, cached));
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (CheckOnline())
            {
                await UploadIncidents();
            }
            else
            {
                ShowToast(Services.TranslateExtension.TranslateText("offline_upload"));
            }
        }

        private async Task UploadIncidents()
        {

            string language = ProjectData.SelectedLanguage.LCID;
            string servername = db.ReadSetting("ServerNameBR", "http://siemens.southcentralus.cloudapp.azure.com:5109/Service.svc");
            string user = db.ReadSetting("UserName", "");

            var m_Http = Services.XServices.Instance.GetService<HttpClient>();
            ComosWebSDK.brcomosweb.ComosBRWeb m_comosbrweb = new ComosWebSDK.brcomosweb.ComosBRWeb(m_Http);

            this.ShowSpinner(Services.TranslateExtension.TranslateText("creating_incidents"));

            foreach (ComosWebSDK.Data.CCachedDevice item in ListViewObjects.ItemsSource)
            {
                try
                {


                    var result = await m_comosbrweb.CreateComosDevice(item.ValueCollection,
                                                                    servername,
                                                                    item.ProjectUID,
                                                                    item.OverlayUID,
                                                                    language,
                                                                    item.OwnerUID,
                                                                    item.CDevUID,
                                                                    user,
                                                                    item.Description
                                                                    );

                    if (!result.Status)
                    {
                        this.ShowToast("Error en la conexión con COMOS. Contacte al administrador. Referencia: COMOS.IO");
                    }
                    else
                    {
                        if (result.data != "")
                        {
                            db.DeleteCachedDevice(item.TempUID);
                            Pages.controls.MidiaControl midiacontrol = new controls.MidiaControl();
                            await midiacontrol.UploadAllPicturesFromObject(item.TempUID, result.data);
                        }
                        else
                        {
                            this.ShowToast("Error en la conexión con COMOS. Contacte al administrador. Referencia: COMOS.IO");
                        }
                    }


                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                }


            }
            GetIncidents();
            this.HideSpinner();

            this.ShowToast(Services.TranslateExtension.TranslateText("incident_created"));

        }
        private async void btDelete_Clicked(object sender, EventArgs e)
        {

            Button bt = (Button)sender;
            Frame frm = ((Frame)((Grid)bt.Parent).Parent);
            await frm.FadeTo(0, 300, Easing.Linear);

            ComosWebSDK.Data.CCachedDevice cached = (ComosWebSDK.Data.CCachedDevice)bt.BindingContext;
            db.DeleteCachedDevice(cached.TempUID);
            GetIncidents();

        }

        private void PageTemplate_Appearing(object sender, EventArgs e)
        {
            GetIncidents();
        }
    }
}