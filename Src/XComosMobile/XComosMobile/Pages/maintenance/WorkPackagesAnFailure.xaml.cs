using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComosWebSDK;
using ComosWebSDK.Data;
using ComosWebSDK.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using System.Collections.ObjectModel;
using XComosMobile.Pages.comos;
using XComosMobile.Services;

namespace XComosMobile.Pages.maintenance
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WorkPackagesAnFailure : PageTemplate
    {

        public WorkPackagesAnFailure(bool keepElements = false)
        {
            InitializeComponent();
            this.BindingContext = this;
            this.NavigationBarControl.OnSearchTextChanged += NavigationBarControl_OnSearchTextChanged;
            this.ListViewTasks.ItemTapped += ListViewTasks_ItemTapped;

            ListViewTasks.RefreshCommand = new Command(() => { LoadWorkPackages(); });
            ListViewTasks.IsPullToRefreshEnabled = true;

            this.NavigationBarControl.FilterShowClicked += NavigationBarControl_FilterShowClicked;
            this.ShowFilter = true;

            //this.Appearing += TasksPage_Appearing;
            //stackfilter.FadeTo(1, 750, Easing.Linear);

            this.Appearing += TasksPage_Appearing;
            this.keepElements = keepElements;

            /*
			this.ShowSpinner("Carregando Workpackages...");            
			Device.BeginInvokeOnMainThread(async () => { await LoadWorkPackages(); });
			this.HideSpinner();
			*/

        }

        private bool keepElements = false;
        bool first_time = true;

        private async void TasksPage_Appearing(object sender, EventArgs e)
        {
            if (first_time)
            {
                this.ShowSpinner(Services.TranslateExtension.TranslateText("loading_failure_analysis"));
                if (!this.keepElements) m_QueryResult = null;

                TasksCollection = null;
                await LoadWorkPackages();
                this.HideSpinner();
                first_time = false;
            }
        }
        private async void NavigationBarControl_FilterShowClicked(object sender, EventArgs e)
        {
            try
            {
                if (stackfilter.IsVisible)
                {
                    await stackfilter.FadeTo(0, 300, Easing.Linear);
                    stackfilter.IsVisible = !stackfilter.IsVisible;
                }
                else
                {
                    stackfilter.FadeTo(1, 750, Easing.Linear);
                    stackfilter.IsVisible = !stackfilter.IsVisible;
                }
                OnPropertyChanged("IsVisible");
            }
            catch (Exception ex)
            {
                this.ShowToast($"Error al mostrar los filtros: {ex.Message}");
            }

        }

        public async void OnDownloadClicked(object sender, EventArgs e)
        {
            try
            {
                this.ShowSpinner(Services.TranslateExtension.TranslateText("downloading_packages"));

                foreach (var item in TasksCollection)
                {
                    await GetTaskInfoByUID(item.Row.Items[0].UID);
                }

            }
            finally
            {
                this.HideSpinner();
                this.ShowToast(Services.TranslateExtension.TranslateText("download_done"));
            }

        }

        private async Task<bool> GetTaskInfoByUID(string UID, bool push = false)
        {

            IComosWeb m_ComosWeb = Services.XServices.Instance.GetService<IComosWeb>();
            ViewModels.ProjectData projectdata = Services.XServices.Instance.GetService<ViewModels.ProjectData>();

            List<CSpecification> Specifications;
            try
            {
                Specifications = await m_ComosWeb.GetObjectSpecification(
                                                                    projectdata.SelectedDB.Key,
                                                                    projectdata.SelectedProject.UID,
                                                                    projectdata.SelectedLayer.UID,
                                                                    projectdata.SelectedLanguage.LCID,
                                                                    UID);
            }
            catch (Exception e)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Error al obtener atributos: " + e.Message, Services.TranslateExtension.TranslateText("OK"));
                return false;
            }

            foreach (var item in Specifications)
            {
                if (item.Name.Equals(Constants.MobileTabName))
                {
                    CSystemObject sysobj;
                    try
                    {
                        sysobj = await m_ComosWeb.GetObject(projectdata.SelectedLayer, UID, projectdata.SelectedLanguage.LCID);
                    }
                    catch (Exception ex)
                    {
                        await App.Current.MainPage.DisplayAlert("Error", "Error al cargar OT´s: " + ex.Message, Services.TranslateExtension.TranslateText("OK"));
                        return false;
                    }

                    if (sysobj == null)
                        return false;

                    CObject o = new CObject()
                    {
                        ClassType = sysobj.SystemType,
                        Description = sysobj.Description,
                        IsClientPicture = sysobj.IsClientPicture,
                        Name = sysobj.Name,
                        UID = sysobj.UID,
                        OverlayUID = projectdata.SelectedLayer.UID,
                        Picture = sysobj.Picture,
                        ProjectUID = projectdata.SelectedProject.ProjectUID,
                        SystemFullName = sysobj.Name,
                    };

                    //Get OT Number
                    string otNumber = "";
                    var objects = await m_ComosWeb.GetNavigatorNodes_Children(projectdata.SelectedLayer.Database, projectdata.SelectedProject.UID, projectdata.SelectedLayer.UID, projectdata.SelectedLanguage.LCID, sysobj.UID, "U");
                    CObject[] devices = objects.ToArray();
                    foreach (var device in devices)
                    {
                        if (device.Description.Equals(Constants.WP_OTCode_Description))
                        {
                            otNumber = device.Name;
                        }
                    }

                    PageAttributes page = new PageAttributes(projectdata.SelectedDB.Key, projectdata.SelectedLanguage.LCID, item, o, "wp", otNumber);
                    //WorkPackageDetail page = new WorkPackageDetail(item,UID);
                    if (push)
                        await this.Navigation.PushAsync(page);

                    return true;
                }
            }

            this.ShowToast(Services.TranslateExtension.TranslateText("mobile_tab_not_found"));

            return false;
        }



        private async void ListViewTasks_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                this.ShowSpinner(Services.TranslateExtension.TranslateText("loading"));

                WorkPackageViewModel wp = e.Item as WorkPackageViewModel;
                await GetTaskInfoByUID(wp.Row.Items[0].UID, true);

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", $"No se puede abrir la orden de trabajo. Puede deberse a una mala conexion a la red", "Ok");
            }
            finally
            {
                this.HideSpinner();
            }
        }

        bool IsFiltered = false;
        private void NavigationBarControl_OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (e.NewTextValue.Length < 2 && !IsFiltered) // avoid filtering with first character perfomormance improvemnt.
                    return;
                string filter = e.NewTextValue;

                if (string.IsNullOrEmpty(filter))
                {
                    // Reset the filter
                    foreach (var t in TasksCollection)
                    {
                        t.IsVisible = true;
                    }
                    IsFiltered = false;
                }
                else
                {
                    int titleindex = PickerShowBy.SelectedIndex;
                    if (titleindex < 0)
                        titleindex = 0;
                    foreach (var t in TasksCollection)
                    {
                        t.IsVisible = t.Items[titleindex].Text.ToUpper().Contains(filter.ToUpper());
                    }
                    IsFiltered = true;
                }
            }
            catch (Exception ex)
            {
                this.ShowToast($"Error al editar campo de búsqueda: {ex.Message}");
            }

        }

        private static CQuerieResult m_QueryResult;
        public WorkPackageViewModel[] TasksCollection { get; set; }
        public CColumn[] TasksAttributeNames
        {
            get
            {
                if (m_QueryResult == null)
                    return null;
                return m_QueryResult.Columns;
            }
        }

        private async Task<bool> LoadWorkPackages()
        {
            bool result = true;
            try
            {
                ViewModels.ProjectData projectdata = Services.XServices.Instance.GetService<ViewModels.ProjectData>();

                IComosWeb m_ComosWeb = Services.XServices.Instance.GetService<IComosWeb>();
                IDatabase db = Services.XServices.Instance.GetService<IDatabase>();
                //this.ShowToast($"Finding CObject on DB");
                CObject o = db.GetCObjectByFullName(projectdata.SelectedLayer.UID, Constants.QueryWorkPackagesAnFailureSystemFullName);

                if (o == null)
                {
                    try
                    {
                        o = await m_ComosWeb.GetObjectBySystemFullName(projectdata.SelectedLayer, Constants.QueryWorkPackagesAnFailureSystemFullName);
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
                    await App.Current.MainPage.DisplayAlert(Services.TranslateExtension.TranslateText("error"), Services.TranslateExtension.TranslateText("my_wp_not_found"), "OK");
                    return false;
                }

                if (!keepElements)
                {
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
                }
                else
                {
                    keepElements = false;
                }

                if (m_QueryResult != null)
                {
                    if (m_QueryResult.Rows.Count() > 0)
                    {

                        List<WorkPackageViewModel> tasks = new List<WorkPackageViewModel>();
                        foreach (var row in m_QueryResult.Rows)
                        {
                            tasks.Add(new WorkPackageViewModel(m_QueryResult.Columns, row));
                        }

                        this.TasksCollection = tasks.ToArray();

                        OnPropertyChanged("TasksCollection");
                        OnPropertyChanged("TasksAttributeNames");
                        UpdateSortingAndGrouping();
                        OnPropertyChanged("SelectedIndex");
                    }
                    else
                    {
                        ShowToast(Services.TranslateExtension.TranslateText("no_wp_found"));
                    }

                    ListViewTasks.EndRefresh();
                }
                else
                {
                    //page not loaded at comosweboffline
                    ShowToast(Services.TranslateExtension.TranslateText("no_wp_found"));
                    ListViewTasks.EndRefresh();
                }
            }
            catch (Exception e)
            {
                await App.Current.MainPage.DisplayAlert(Services.TranslateExtension.TranslateText("error"), e.Message, "OK");
                return false;
            }
            return result;
        }

        public ObservableCollection<Grouping<object, WorkPackageViewModel>> GroupedTasks { get; set; }
        public bool UseGroupedTasks
        {
            get { return GroupedTasks != null; }
        }

        private async void OnDeleteFilters(object sender, EventArgs e)
        {
            try
            {
                PickerShowBy.SelectedItem = null;
                PickerSortBy.SelectedItem = null;
                PickerGroupBy.SelectedItem = null;
                UpdateSortingAndGrouping();
            }
            catch (Exception ex)
            {
                this.ShowToast($"Error al borrar filtro: {ex.Message}");
            }

        }

        private async void OnApplyFilters(object sender, EventArgs e)
        {
            try
            {
                UpdateSortingAndGrouping();
            }
            catch (Exception ex)
            {
                this.ShowToast($"Error al aplicar filtro: {ex.Message}");
            }
        }

        private void UpdateSortingAndGrouping()
        {
            int indexGroupBy = -1;
            CColumn groupByColumn = PickerGroupBy.SelectedItem as CColumn;
            if (groupByColumn != null)
                indexGroupBy = Array.FindIndex(m_QueryResult.Columns, (c) => { return c == groupByColumn; });

            int indexSortBy = -1;
            CColumn sortByColumn = PickerSortBy.SelectedItem as CColumn;
            if (sortByColumn != null)
                indexSortBy = Array.FindIndex(m_QueryResult.Columns, (c) => { return c == sortByColumn; });

            if (groupByColumn != null && sortByColumn != null)
            {
                var sorted = from row in TasksCollection
                             orderby row.Items[indexSortBy].Text
                             group row by row.Items[indexGroupBy].Text
                                         into rowgroup
                             select new Grouping<object, WorkPackageViewModel>(rowgroup.Key, rowgroup);
                //create a new collection of groups 
                this.GroupedTasks = new ObservableCollection<Grouping<object, WorkPackageViewModel>>(sorted);
                OnPropertyChanged("GroupedTasks");
                OnPropertyChanged("UseGroupedTasks");
                ListViewTasks.ItemsSource = this.GroupedTasks;
                ListViewTasks.IsGroupingEnabled = true;
            }
            else if (groupByColumn != null)
            {
                var sorted = from row in TasksCollection
                             group row by row.Items[indexGroupBy].Text
                                         into rowgroup
                             select new Grouping<object, WorkPackageViewModel>(rowgroup.Key, rowgroup);
                //create a new collection of groups 
                this.GroupedTasks = new ObservableCollection<Grouping<object, WorkPackageViewModel>>(sorted);
                OnPropertyChanged("GroupedTasks");
                OnPropertyChanged("UseGroupedTasks");
                ListViewTasks.ItemsSource = this.GroupedTasks;
                ListViewTasks.IsGroupingEnabled = true;
            }
            else if (sortByColumn != null)
            {
                var sorted = from row in TasksCollection
                             orderby row.Items[indexSortBy].Text
                             select row;
                this.GroupedTasks = null;
                ListViewTasks.ItemsSource = sorted;
                ListViewTasks.IsGroupingEnabled = false;
                OnPropertyChanged("GroupedTasks");
                OnPropertyChanged("UseGroupedTasks");
            }
            else
            {
                ListViewTasks.ItemsSource = this.TasksCollection;
                ListViewTasks.IsGroupingEnabled = false;
                this.GroupedTasks = null;
                OnPropertyChanged("GroupedTasks");
                OnPropertyChanged("UseGroupedTasks");
            }
        }

        protected override bool OnBackButtonPressed()
        {
            var web = Services.XServices.Instance.GetService<ComosWebSDK.IComosWeb>();
            this.Navigation.PushAsync(new PageSolutions(web));
            return true;
        }
    }
}