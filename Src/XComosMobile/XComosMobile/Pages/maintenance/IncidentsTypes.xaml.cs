using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;

using ComosWebSDK.Data;
using ComosWebSDK.Extensions;
using ComosWebSDK;
using XComosMobile.Pages.comos;
using System.Net.Http;
using XComosMobile.Services;

namespace XComosMobile.Pages.maintenance
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IncidentsTypes : PageTemplate
    {
        private CQuerieResult m_QueryResult;
        public ViewModel[] IncidentTypesCollection { get; set; }
        List<Types> incidentTypesList;

        private bool isAnIncidentOfATask;
        private string taskUid;
        private string taskPic;
        private string taskOwner;

        private Services.XDatabase db { get { return Services.XServices.Instance.GetService<Services.XDatabase>(); } }

        // If the user clicks the Page Solutions' button "Incidents"
        public IncidentsTypes()
        {
            InitializeComponent();
            this.BindingContext = this;
            incidentTypesList = new List<Types>();
            isAnIncidentOfATask = false;
            this.Appearing += IncidentsPage_Appearing;
        }

        // If it is an incident for a task
        public IncidentsTypes(string taskUid, string taskPic, string taskOwner)
        {
            InitializeComponent();
            this.BindingContext = this;
            incidentTypesList = new List<Types>();
            isAnIncidentOfATask = true;
            this.taskUid = taskUid;
            this.taskPic = taskPic;
            this.taskOwner = taskOwner;
            this.Appearing += IncidentsPage_Appearing;
        }

        bool first_time = true;
        private async void IncidentsPage_Appearing(object sender, EventArgs e)
        {
            if (first_time)
            {
                this.ShowSpinner(Services.TranslateExtension.TranslateText("loading_incident_types"));

                m_QueryResult = null;
                IncidentTypesCollection = null;
                var result = await LoadIncidentTypes();
                if (result != false)
                {
                    for (int i = 0; i < IncidentTypesCollection.Length; i++)
                    {
                        incidentTypesList.Add(new Types
                        {
                            Name = IncidentTypesCollection[i].Row.Items[2].Text,
                            UID = IncidentTypesCollection[i].Row.UID,
                        });
                    }
                }
                ListViewIncidents.ItemsSource = incidentTypesList;
                this.HideSpinner();
                first_time = false;
            }
        }

        private async void IncidentType_Clicked(object sender, EventArgs e)
        {
            this.ShowSpinner("Cargando...");
            try
            {
                Frame incident = (Frame)sender;
                var fcontent = incident.Content;
                var myStacklayout = fcontent.GetType();
                if (myStacklayout == typeof(StackLayout))
                {
                    StackLayout fStacklayout = (StackLayout)fcontent;
                    var listChildren = fStacklayout.Children;
                    Grid reqGroup = (Grid)listChildren[0];     //The only element inside the stackLayout is a Grid
                    Label incidentLabel = reqGroup.FindByName<Label>("IncidentName");
                    var incidentText = incidentLabel.Text;

                    Types myIncident = incidentTypesList.Find(incidentType => incidentType.Name == incidentText);

                    ComosWebSDK.UI.UICachedScreen screen = db.GetCachedScreen(myIncident.UID);

                    if (isAnIncidentOfATask)
                    {
                        await this.Navigation.PushAsync(new PageNewDevice(screen, taskUid, taskPic, taskOwner));
                    }
                    else
                    {
                        await this.Navigation.PushAsync(new PageNewDevice(screen, "DUMMY_INCIDENT", "DUMMY", Services.TranslateExtension.TranslateText("new_incident")));
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert(Services.TranslateExtension.TranslateText("error"), "Error al abrir creación de aviso: " + ex.Message, "OK");               
            }
            finally
            {
                this.HideSpinner();
            }          
        }

        public async Task<bool> LoadIncidentTypes()
        {
            bool result = false;
            try
            {
                ViewModels.ProjectData projectdata = Services.XServices.Instance.GetService<ViewModels.ProjectData>();

                IComosWeb m_ComosWeb = Services.XServices.Instance.GetService<IComosWeb>();
                IDatabase db = Services.XServices.Instance.GetService<IDatabase>();
                //this.ShowToast($"Finding CObject on DB");
                CObject o = db.GetCObjectByFullName(projectdata.SelectedLayer.UID, Constants.QueryIncidentTypesFullName);

                if (o == null)
                {
                    try
                    {
                        o = await m_ComosWeb.GetObjectBySystemFullName(projectdata.SelectedLayer, Constants.QueryIncidentTypesFullName);
                    }
                    catch (TaskCanceledException) { return false; } // If there is a Logout Request
                    catch (Exception ex)
                    {
                        await App.Current.MainPage.DisplayAlert("Error", "Error al cargar query: " + ex.Message, Services.TranslateExtension.TranslateText("OK"));
                        return false;
                    }
                    db.InsertCObject(o, projectdata.SelectedLayer.UID);
                }

                if (o == null)
                {
                    await App.Current.MainPage.DisplayAlert(Services.TranslateExtension.TranslateText("error"), Services.TranslateExtension.TranslateText("my_incidents_not_found"), "OK");
                    return false;
                }

                try
                {
                    m_QueryResult = await m_ComosWeb.GetqueriesResult(
                            projectdata.SelectedDB.Key, o.ProjectUID, o.OverlayUID, projectdata.SelectedLanguage.LCID, o.UID, null);
                }
                catch (TaskCanceledException) { return false; } // If there is a Logout Request
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Error al cargar query: " + ex.Message, Services.TranslateExtension.TranslateText("OK"));
                    return false;
                }

                if (m_QueryResult != null)
                {
                    Converters.IsQueryCachedValue cachehandler = new Converters.IsQueryCachedValue();
                    cachehandler.GetQueryCachedValues(m_QueryResult);

                    if (m_QueryResult.Rows.Length > 0)
                    {
                        List<ViewModel> incidentTypes = new List<ViewModel>();
                        foreach (var row in m_QueryResult.Rows)
                        {
                            incidentTypes.Add(new ViewModel(m_QueryResult.Columns, row));
                        }
                        this.IncidentTypesCollection = incidentTypes.ToArray();

                        OnPropertyChanged("IncidentTypesCollection");
                        /*OnPropertyChanged("TasksAttributeNames");
                        UpdateSortingAndGrouping();*/
                    }
                    else
                    {
                        ShowToast(Services.TranslateExtension.TranslateText("no_incidents_types_found"));
                    }

                    result = true;
                }
                else
                {
                    //page not loaded at comosweboffline
                    ShowToast(Services.TranslateExtension.TranslateText("no_incidents_types_found"));
                }

                ListViewIncidents.EndRefresh();
            }
            catch (Exception e)
            {
                await App.Current.MainPage.DisplayAlert(Services.TranslateExtension.TranslateText("error"), e.Message, "OK");
                return false;
            }

            return result;
        }
    }

    public class Types
    {
        public string Name
        {
            get;
            set;
        }
        public string UID
        {
            get;
            set;
        }
        public string OwnerUID
        {
            get;
            set;
        }
    }

}