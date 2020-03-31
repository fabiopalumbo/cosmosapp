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
using XComosMobile.Pages.maintenance;

namespace XComosMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public abstract partial class DevicesContainer : PageTemplate
    {
        private bool noDeviceWasClicked;
        private bool first_time = true;
        private CQuerieResult m_QueryResult;
        private List<DeviceInfo> deviceContainerList;

        protected View ListViewDevices_;
        protected ViewModel[] DeviceContainerCollection { get; set; }
        private Services.XDatabase db { get { return Services.XServices.Instance.GetService<Services.XDatabase>(); } }
        private ViewModels.ProjectData ProjectData { get { return Services.XServices.Instance.GetService<ViewModels.ProjectData>(); } }

        public DevicesContainer()
        {
            InitializeComponent();
            this.BindingContext = this;
            deviceContainerList = new List<DeviceInfo>();
        }

        #region Private Methods
        protected async void DevicesContainerPage_Appearing(object sender, EventArgs e)
        {
            try
            {
                noDeviceWasClicked = true;
                if (first_time)
                {
                    this.ShowSpinner("Cargando...");

                    m_QueryResult = null;
                    DeviceContainerCollection = null;
                    var result = await LoadDevices();
                    if (result != false)
                    {
                        for (int i = 0; i < DeviceContainerCollection.Length; i++)
                        {
                            deviceContainerList.Add(new DeviceInfo
                            {
                                Name = DeviceContainerCollection[i].Row.Items[1].Text + " | " + DeviceContainerCollection[i].Row.Items[3].Text + " | " + DeviceContainerCollection[i].Row.Items[4].Text,    // descripcion | owner | descripcion de equipo
                                UID = DeviceContainerCollection[i].Row.Items[1].UID,
                                AditionalInfo = DeviceContainerCollection[i].Row.Items[0].Text + " | " + DeviceContainerCollection[i].Row.Items[2].Text,    // label | ingeniero
                                belongsToAnEquipment = GetBelongsToAnEquipmentIcon(DeviceContainerCollection[i].Row.Items[3].Text),
                                buttonColor = GetButtonColor(DeviceContainerCollection[i].Row.Items[3].Text),
                            });
                        }
                    }
                    ((ListView)ListViewDevices_).ItemsSource = deviceContainerList;
                    this.HideSpinner();
                    first_time = false;
                }
            }
            catch (Exception ex)
            {
                ShowToast(GetExceptionMessageWhenDevicesContainerDontAppear() + ex.Message);
            }
        }

        protected async void ListViewDevices_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                if (noDeviceWasClicked)
                {
                    noDeviceWasClicked = false;
                    DeviceInfo device = (DeviceInfo)e.Item;
                    CheckAndDownloadAditionalContent downloader = new CheckAndDownloadAditionalContent();
                    PageAttributes page = await downloader.DownloadDeviceContent(device.UID, true);
                    page.belongsToAnEquipment = device.belongsToAnEquipment.Equals(Constants.OkIcon) ? true : false;
                    page.isALoogBook = IsALogBook();
                    if (page != null)
                    {
                        await this.Navigation.PushAsync(page);
                    }
                    else
                    {
                        ShowToast("No hay resultados");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowToast(GetExceptionMessageWhenDeviceTappedFailed() + ex.Message);
            }
        }

        private async Task<bool> LoadDevices()
        {
            bool result = false;
            try
            {
                ViewModels.ProjectData projectdata = Services.XServices.Instance.GetService<ViewModels.ProjectData>();

                IComosWeb m_ComosWeb = Services.XServices.Instance.GetService<IComosWeb>();
                IDatabase db = Services.XServices.Instance.GetService<IDatabase>();
                
                CObject o = db.GetCObjectByFullName(projectdata.SelectedLayer.UID, GetQueryFullName());

                if (o == null)
                {
                    try
                    {
                        o = await m_ComosWeb.GetObjectBySystemFullName(projectdata.SelectedLayer, GetQueryFullName());
                    }
                    catch (Exception ex)
                    {
                        await App.Current.MainPage.DisplayAlert("Error", "Error al cargar query: " + ex.Message, Services.TranslateExtension.TranslateText("OK"));
                        return false;
                    }
                    db.InsertCObject(o, projectdata.SelectedLayer.UID);
                }

                if (o == null)
                {
                    await App.Current.MainPage.DisplayAlert(Services.TranslateExtension.TranslateText("error"), Services.TranslateExtension.TranslateText("event_container_not_found"), "OK");
                    return false;
                }

                try
                {
                    m_QueryResult = await m_ComosWeb.GetqueriesResult(
                            projectdata.SelectedDB.Key, o.ProjectUID, o.OverlayUID, projectdata.SelectedLanguage.LCID, o.UID, null);
                }
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
                        List<ViewModel> unassignedEventsList = new List<ViewModel>();
                        foreach (var row in m_QueryResult.Rows)
                        {
                            unassignedEventsList.Add(new ViewModel(m_QueryResult.Columns, row));
                        }

                        this.DeviceContainerCollection = unassignedEventsList.ToArray();

                        OnPropertyChanged("EventContainerCollection");
                        /*OnPropertyChanged("TasksAttributeNames");
			            UpdateSortingAndGrouping();*/
                        result = true;
                    }
                    else
                    {
                        ShowToast("No hay resultados");
                        result = false;
                    }

                }
                else
                {
                    //page not loaded at comosweboffline
                    ShowToast("No hay resultados");
                }

                ((ListView)ListViewDevices_).EndRefresh();
            }
            catch (Exception e)
            {
                await App.Current.MainPage.DisplayAlert(Services.TranslateExtension.TranslateText("error"), e.Message,
                    "OK");
                return false;
            }

            return result;

        }
        #endregion

        #region Protected Methods

        protected virtual bool IsALogBook() => false;

        protected abstract string GetBelongsToAnEquipmentIcon(string name);

        protected abstract string GetButtonColor(string name);

        protected abstract string GetExceptionMessageWhenDevicesContainerDontAppear();

        protected abstract string GetExceptionMessageWhenDeviceTappedFailed();

        protected abstract string GetQueryFullName();
        #endregion
    }

    public class DeviceInfo
    {
        public string Name { get; set; }

        public string UID { get; set; }

        public string AditionalInfo { get; set; }

        public string belongsToAnEquipment { get; set; }

        public string buttonColor { get; set; }
    }

}