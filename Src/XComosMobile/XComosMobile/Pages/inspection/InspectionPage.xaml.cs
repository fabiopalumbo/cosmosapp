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
using XComosMobile.Pages.maintenance;
using XComosMobile.Services;

namespace XComosMobile.Pages.inspection
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InspectionPage : PageTemplate
    {
        public InspectionPage()
        {
            InitializeComponent();
            this.BindingContext = this;
            this.NavigationBarControl.OnSearchTextChanged += NavigationBarControl_OnSearchTextChanged;

            ListViewTasks.RefreshCommand = new Command(() => Task.Run(async () => { await LoadTasks(); }));
            ListViewTasks.IsPullToRefreshEnabled = true;


            this.NavigationBarControl.FilterShowClicked += NavigationBarControl_FilterShowClicked;
            this.ShowFilter = true;
            this.Appearing += TasksPage_Appearing;

        }

        private async void NavigationBarControl_FilterShowClicked(object sender, EventArgs e)
        {

            if (stackfilter.IsVisible)
            {
                await stackfilter.FadeTo(0, 300, Easing.Linear);
                stackfilter.IsVisible = !stackfilter.IsVisible;
            }
            else
            {
                await stackfilter.FadeTo(1, 750, Easing.Linear);
                stackfilter.IsVisible = !stackfilter.IsVisible;
            }

            OnPropertyChanged("IsVisible");
        }

        bool first_time = true;
        private async void TasksPage_Appearing(object sender, EventArgs e)
        {
            if (first_time)
            {
                this.ShowSpinner(Services.TranslateExtension.TranslateText("loading_tasks"));

                m_QueryResult = null;
                TasksCollection = null;
                await LoadTasks();
                this.HideSpinner();
                first_time = false;
            }
        }


        private async Task<bool> GetTaskInfoByUID(string UID, bool push = false)
        {

            CheckAndDownloadAditionalContent downloader = new CheckAndDownloadAditionalContent();
            PageAttributes page = await downloader.DownloadDeviceContent(UID, push);

            if (page != null)
            {

                page.ViewNewDev = true;
                page.OnNew += Page_OnNew;

                if (page != null && push)
                {
                    page.NewDevIcon = comos.Constants.IncidentIcon;
                    page.IsActivity = false;
                    page.AllowPictures = true;
                    await this.Navigation.PushAsync(page);
                }

                return true;
            }
            else
            {
                ShowToast(Services.TranslateExtension.TranslateText("task_not_found"));
                return false;
            }
        }

        private void Page_OnNew(object sender, EventArgs e)
        {
            CObject obj = sender as CObject;
            NewIncidentForTask(obj.UID, obj.Name, obj.Picture);
        }

        private async void NewIncidentForTask(string uid, string text, string pic = "")
        {
            Services.XDatabase db = Services.XServices.Instance.GetService<Services.XDatabase>();

            ComosWebSDK.UI.UICachedScreen screen = db.GetCachedScreen(comos.Constants.IncidentCDevUID);
            await this.Navigation.PushAsync(new PageNewDevice(screen, uid, pic, text));

        }


        private async void ListViewTasks_ItemTapped(object sender, EventArgs e)
        {
            try
            {

                StackLayout stack = ((TaskCell)sender).View as StackLayout;

                //await stack.ScaleTo(1.05, 50, Easing.Linear);
                //await stack.ScaleTo(1, 50, Easing.Linear);        
                await stack.FadeTo(0.5);
                await stack.FadeTo(1);
                this.ShowSpinner(Services.TranslateExtension.TranslateText("loading"));
                ViewModel task = ((TaskCell)sender).BindingContext as ViewModel;
                //TaskViewModel task = e.Item as TaskViewModel;
                await GetTaskInfoByUID(task.Row.Items[0].UID, true);
            }
            finally
            {
                this.HideSpinner();
            }
        }

        bool IsFiltered = false;
        private void NavigationBarControl_OnSearchTextChanged(object sender, TextChangedEventArgs e)
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

        private CQuerieResult m_QueryResult;
        public ViewModel[] TasksCollection { get; set; }
        public CColumn[] TasksAttributeNames
        {
            get
            {
                if (m_QueryResult == null)
                    return null;
                return m_QueryResult.Columns;
            }
        }

        private async Task<bool> LoadTasks()
        {
            bool result = false;
            try
            {
                ViewModels.ProjectData projectdata = Services.XServices.Instance.GetService<ViewModels.ProjectData>();

                IComosWeb m_ComosWeb = Services.XServices.Instance.GetService<IComosWeb>();
                IDatabase db = Services.XServices.Instance.GetService<IDatabase>();
                //this.ShowToast($"Finding CObject on DB");
                CObject o = db.GetCObjectByFullName(projectdata.SelectedLayer.UID, Constants.QueryWorkPackagesSystemFullName);

                if (o == null)
                {
                    try
                    {
                        o = await m_ComosWeb.GetObjectBySystemFullName(projectdata.SelectedLayer, Constants.QueryInspectionsSystemFullName);
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
                    await App.Current.MainPage.DisplayAlert(Services.TranslateExtension.TranslateText("error"), Services.TranslateExtension.TranslateText("my_tasks_not_found"), "OK");
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
                        List<ViewModel> tasks = new List<ViewModel>();
                        foreach (var row in m_QueryResult.Rows)
                        {
                            tasks.Add(new ViewModel(m_QueryResult.Columns, row));
                        }
                        this.TasksCollection = tasks.ToArray();

                        OnPropertyChanged("TasksCollection");
                        OnPropertyChanged("TasksAttributeNames");
                        UpdateSortingAndGrouping();
                    }
                    else
                    {
                        ShowToast(Services.TranslateExtension.TranslateText("no_tasks_found"));
                    }
                    result = true;
                }
                else
                {
                    //page not loaded at comosweboffline
                    ShowToast(Services.TranslateExtension.TranslateText("no_tasks_found"));
                }

                ListViewTasks.EndRefresh();
                return result;
            }
            catch (Exception e)
            {
                await App.Current.MainPage.DisplayAlert(Services.TranslateExtension.TranslateText("error"), e.Message, "OK");
                return false;
            }
        }

        private ComosWebSDK.brcomosweb.ComosBRWeb GetBRComosWeb()
        {
            var m_Http = Services.XServices.Instance.GetService<HttpClient>();
            ComosWebSDK.brcomosweb.ComosBRWeb m_comosbrweb = new ComosWebSDK.brcomosweb.ComosBRWeb(m_Http);
            return m_comosbrweb;

        }

        public async void OnUploadClicked(object sender, EventArgs e)
        {

            if (!CheckOnline())
            {
                this.ShowToast(Services.TranslateExtension.TranslateText("uploading_tasks_only_online"));
                return;
            }

            if (TasksCollection == null || TasksCollection.Length == 0)
            {
                this.ShowToast(Services.TranslateExtension.TranslateText("no_tasks_found"));
                return;
            }

            Services.XDatabase db = Services.XServices.Instance.GetService<Services.XDatabase>();
            ViewModels.ProjectData projectdata = Services.XServices.Instance.GetService<ViewModels.ProjectData>();
            IBRServiceContracts.CWriteValueCollection totalvalues = new IBRServiceContracts.CWriteValueCollection();

            this.ShowSpinner(Services.TranslateExtension.TranslateText("saving_tasks"));

            foreach (var item in TasksCollection)
            {
                IBRServiceContracts.CWriteValueCollection values = db.GetAllCachedSpecsFromOwner(item.Row.Items[0].UID, projectdata.SelectedProject.UID, projectdata.SelectedLayer.UID);
                if (totalvalues.Values == null)
                {
                    totalvalues.Values = values.Values;
                }
                else
                {
                    totalvalues.Values = totalvalues.Values.Concat(values.Values).ToArray();
                }

            }


            string user = projectdata.User;
            string project = projectdata.SelectedProject.UID;
            string workinglayer = projectdata.SelectedLayer.UID;
            string language = projectdata.SelectedLanguage.LCID;
            string servername = db.ReadSetting("ServerNameBR", "http://siemens.southcentralus.cloudapp.azure.com:5109/Service.svc");
            var m_comosbrweb = GetBRComosWeb();

            try
            {
                var result = await m_comosbrweb.WriteComosValues(totalvalues, servername, user, project, workinglayer, language);

                if (result)
                {
                    foreach (var item in totalvalues.Values)
                    {
                        db.ClearAllSpecsFromOwner(item.WebSystemUID, project, workinglayer);
                    }

                    m_QueryResult = null;
                    TasksCollection = null;
                    Device.BeginInvokeOnMainThread(async () => { await LoadTasks(); });

                    ShowToast(Services.TranslateExtension.TranslateText("done"));
                }
                else
                {
                    ShowToast(Services.TranslateExtension.TranslateText("save_failed"));
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                return;
            }

            this.HideSpinner();

        }

        public void OnDownloadClicked(object sender, EventArgs e)
        {

            if (!CheckOnline())
            {
                this.ShowToast(Services.TranslateExtension.TranslateText("downloading_tasks_only_online"));
                return;
            }
            var m_comosbrweb = Services.XServices.Instance.GetService<IComosWeb>();
            try
            {
                //this.ShowSpinner(Services.TranslateExtension.TranslateText("downloading_tasks"));
                //m_comosbrweb.Lock();
                List<string> itemList = new List<string>();

                foreach (var item in TasksCollection)
                {
                    //await GetTaskInfoByUID(item.Row.Items[0].UID);
                    itemList.Add(item.Row.Items[0].UID);
                }

                var platform = Services.XServices.Instance.GetService<Services.IPlatformSystem>();
                ShowToast(Services.TranslateExtension.TranslateText("downloading_tasks"));
                platform.startworker(itemList.ToArray());

            }
            finally
            {
                //m_comosbrweb.UnLock();
                //this.HideSpinner();
                //this.SendNotification("COMOS MRO", Services.TranslateExtension.TranslateText("download_tasks_done"));
                //this.ShowToast(Services.TranslateExtension.TranslateText("download_done"));
            }

        }

        public ObservableCollection<Grouping<object, ViewModel>> GroupedTasks { get; set; }
        public bool UseGroupedTasks
        {
            get { return GroupedTasks != null; }
        }

        private void OnSortByIndexChanged(object sender, EventArgs e)
        {
            UpdateSortingAndGrouping();
        }

        private void OnGroupByIndexChanged(object sender, EventArgs e)
        {
            UpdateSortingAndGrouping();
        }

        private void UpdateSortingAndGrouping()
        {
            int indexGroupBy = -1;
            CColumn groupByColumn = PickerGroupBy.SelectedItem as CColumn;
            if (groupByColumn != null)
                indexGroupBy = Array.FindIndex(m_QueryResult.Columns, (c) => { return c.DisplayDescription == groupByColumn.DisplayDescription; });

            int indexSortBy = -1;
            CColumn sortByColumn = PickerSortBy.SelectedItem as CColumn;
            if (sortByColumn != null)
                indexSortBy = Array.FindIndex(m_QueryResult.Columns, (c) => { return c.DisplayDescription == sortByColumn.DisplayDescription; });

            if (groupByColumn != null && sortByColumn != null)
            {
                var sorted = from row in TasksCollection
                             orderby row.Items[indexSortBy].Text
                             group row by row.Items[indexGroupBy].Text
                             into rowgroup
                             select new Grouping<object, ViewModel>(rowgroup.Key, rowgroup);
                //create a new collection of groups 
                this.GroupedTasks = new ObservableCollection<Grouping<object, ViewModel>>(sorted);
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
                             select new Grouping<object, ViewModel>(rowgroup.Key, rowgroup);
                //create a new collection of groups 
                this.GroupedTasks = new ObservableCollection<Grouping<object, ViewModel>>(sorted);
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
    }
}