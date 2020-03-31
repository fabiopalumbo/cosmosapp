using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ComosWebSDK;
using ComosWebSDK.Data;

namespace XComosMobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageProjects : PageTemplate, INotifyPropertyChanged
    {
        #region

        ViewModels.ProjectData m_ProjectData = null;
        bool appeared = false;
        ComosWebSDK.IComosWeb web;

        public PageProjects(ComosWebSDK.IComosWeb web)
        {
            InitializeComponent();
            this.ShowHamburger = false;
            this.ShowBackButton = true;
            this.web = web;
            m_ProjectData = new ViewModels.ProjectData();

            this.BindingContext = m_ProjectData;

        }



        private void BtnOpenPicker_OnClicked(object sender, EventArgs e)
        {
            PickerDatabase.Focus();
        }

        bool m_LastSessionLoaded = false;
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            ShowSpinner(Services.TranslateExtension.TranslateText("loading"));
            //ShowStickSpinner("Loading...");
            (Application.Current.MainPage as PageRoot).IsGestureEnabled = false;

            if (!m_LastSessionLoaded) // Avoid loading data again when user comes from back button.
            {
                await m_ProjectData.LoadLastSession();

                if (m_ProjectData.SelectedLayer != null)
                {
                    await BuildSelectedLayerStructure(m_ProjectData.SelectedLayer);
                    OnClickedOpenProject(null, null);
                }
            }
            else
            {

            }
            HideSpinner();
            //HideStickSpinner();
            m_LastSessionLoaded = true;

            //await OnDatabaseSelectedAutomatically();
            //await OnProjectSelectedAutomatically();
        }
        public override void OnNavigateBackAsync()
        {
            Services.XServices.Instance.RemoveService<ViewModels.ProjectData>();
            base.OnNavigateBackAsync();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        async void OnClickedOpenProject(object sender, EventArgs e)
        {

            var projectdata = (ViewModels.ProjectData)BindingContext;
            var m_Client = Services.XServices.Instance.GetService<ComosWebSDK.IComosWeb>();
            m_Client.StartHeartBeat();
            Device.StartTimer(TimeSpan.FromSeconds(
                    m_Client.HeartBeat),
                    m_Client.DoHeartBeat);


            Services.XDatabase db = Services.XServices.Instance.GetService<Services.XDatabase>();
            CLanguage lang = null;
            lang = db.ReadSetting("Language", lang);

            if (lang != null)
            {
                projectdata.SelectedLanguage = lang;
            }

            // Language of comos web is buggy, can not use this.
            // We will use by default the project default user language.

            if (projectdata.SelectedLanguage == null)
            {
                try
                {
                    CObject clanguage = await m_Client.GetDefaultLanguage(projectdata.SelectedDB.Key, projectdata.SelectedProject.UID, projectdata.SelectedLayer.UID);
                    projectdata.OverideSelectedLanguage(new CLanguage()
                    {
                        Key = null,
                        LCID = clanguage.Name,
                        Value = clanguage.Description
                    });
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Error al obtener idiomas: " + ex.Message, Services.TranslateExtension.TranslateText("OK"));
                    return;
                }

            }

            //projectdata.SelectedLanguage = 

            projectdata.SaveLastSession();
            // Init the image converter based on the database to use.
            object tmp = null;
            if (App.Current.Resources.TryGetValue("PictureToImage", out tmp))
            {
                Converters.PictureRefToImage c = tmp as Converters.PictureRefToImage;
                if (c != null)
                    c.InitConverter(projectdata.SelectedLayer.Database);
            }

            Services.XServices.Instance.AddService<ViewModels.ProjectData>(projectdata);

            await App.Navigation.PushAsync(new Pages.PageSolutions(web));

        }

        public async void OnDatabaseSelected(object sender, EventArgs e)
        {

            var projectdata = (ViewModels.ProjectData)BindingContext;

            var database = PickerDatabase.SelectedItem as CDatabase;

            this.ShowSpinner(Services.TranslateExtension.TranslateText("Cargando Base de Datos"));

            await projectdata.OpenDatabase(database);
            PickerProject.SelectedItem = null;
            projectdata.Layers = new CWorkingLayer[0];
            //ListViewLayers.ItemsSource
            StackBreadcrumb.Children.Clear();

            this.HideSpinner();
        }

        public async void OnProjectSelected(object sender, EventArgs e)
        {
            var projectdata = (ViewModels.ProjectData)BindingContext;

            if (appeared)
            {
                StackBreadcrumb.Children.Clear();
            }
            else
            {
                appeared = true;
            }

            this.ShowSpinner(Services.TranslateExtension.TranslateText("loading"));
            await projectdata.SelectProject(PickerProject.SelectedItem as CProject);
            this.HideSpinner();

        }

        private async Task BuildSelectedLayerStructure(CWorkingLayer layer)
        {
            var projectdata = (ViewModels.ProjectData)BindingContext;
            projectdata.Layers = layer.Layers;

            if (layer.OwnerLayers != null)
            {
                StackBreadcrumb.Children.Clear();
                foreach (var item in layer.OwnerLayers)
                {
                    var btnl = new Button()
                    {
                        Text = item.Name,
                        FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Button)),
                        CommandParameter = item,
                    };
                    btnl.Clicked += Btn_Clicked;
                    StackBreadcrumb.Children.Add(btnl);
                }

                var btn = new Button()
                {
                    Text = layer.Name,
                    FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Button)),
                    CommandParameter = layer,
                };
                btn.Clicked += Btn_Clicked;
                StackBreadcrumb.Children.Add(btn);
            }


        }
        public async void OnLayerSelected(object sender, EventArgs e)
        {
            var SelectedLayer = ListViewLayers.SelectedItem as CWorkingLayer;
            var projectdata = (ViewModels.ProjectData)BindingContext;
            await projectdata.SelectLayer(SelectedLayer);
            var btn = new Button()
            {
                Text = SelectedLayer.Name,
                FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Button)),
                CommandParameter = SelectedLayer,
            };
            btn.Clicked += Btn_Clicked;
            SelectedLayer.OwnerLayers = GetOwnerLayers();
            StackBreadcrumb.Children.Add(btn);

        }

        private CWorkingLayer[] GetOwnerLayers()
        {
            CWorkingLayer[] layers = new CWorkingLayer[StackBreadcrumb.Children.Count()];
            int i = 0;
            foreach (var item in StackBreadcrumb.Children)
            {
                Button btn = item as Button;
                layers[i] = btn.CommandParameter as CWorkingLayer;
                layers[i].OwnerLayers = null;
                layers[i].Layers = null;
                i++;
            }
            return layers;
        }

        private async void Btn_Clicked(object sender, EventArgs e)
        {
            // Async void !! This is bad, cause when exception happens app will not be able to catch it.
            var projectdata = (ViewModels.ProjectData)BindingContext;
            var btn = StackBreadcrumb.Children.Last();
            while (btn != sender)
            {
                StackBreadcrumb.Children.Remove(btn);
                btn = StackBreadcrumb.Children.Last();
            }

            await projectdata.SelectLayer((CWorkingLayer)((Button)btn).CommandParameter);
        }

        protected override bool OnBackButtonPressed()
        {
            try
            {
                App.ResetAppAsync(web).GetAwaiter();
            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert(Services.TranslateExtension.TranslateText("logout"), "Error al cerrar sesion: " + ex.Message, Services.TranslateExtension.TranslateText("OK")).GetAwaiter();
            }
            return true;
        }



        //Automatic selection of DB, project and Working Layers

        public async Task OnDatabaseSelectedAutomatically()
        {

            var projectdata = (ViewModels.ProjectData)BindingContext;

            CDatabase database = null;

            for (int i = 0; i < PickerDatabase.Items.Count; i++)
            {
                string myDBName = PickerDatabase.Items.ElementAt(i).ToString();
                if (myDBName == comos.Constants.defaultDB)
                {
                    PickerDatabase.SelectedIndex = i;
                    database = PickerDatabase.SelectedItem as CDatabase;
                    break;
                }
            }
            //var database = PickerDatabase.SelectedItem as CDatabase;

            this.ShowSpinner(Services.TranslateExtension.TranslateText("Cargando Base de Datos..."));

            if (database != null)
            {
                await projectdata.OpenDatabase(database);
                PickerProject.SelectedItem = null;
                projectdata.Layers = new CWorkingLayer[0];
                //ListViewLayers.ItemsSource
                StackBreadcrumb.Children.Clear();
            }

            this.HideSpinner();
        }

        //Autoselect project
        public async Task OnProjectSelectedAutomatically()
        {
            var projectdata = (ViewModels.ProjectData)BindingContext;

            if (appeared)
            {
                StackBreadcrumb.Children.Clear();
            }
            else
            {
                appeared = true;
            }

            this.ShowSpinner(Services.TranslateExtension.TranslateText("Cargando proyecto..."));

            CProject myProject = null;

            for (int i = 0; i < PickerProject.Items.Count; i++)
            {
                string myProjectName = PickerProject.Items.ElementAt(i).ToString();
                if (myProjectName == comos.Constants.defaultProject)
                {
                    PickerProject.SelectedIndex = i;
                    myProject = PickerProject.SelectedItem as CProject;
                    break;
                }
            }

            if (myProject != null) { await projectdata.SelectProject(myProject as CProject); }

            this.HideSpinner();

            //await OnLayerSelectedAutomatically(0);

        }

        //Autoselect Layers
        async Task OnLayerSelectedAutomatically(int layerIndex)
        {
            //get layer name
            string[] myLayers = comos.Constants.defaultWorkingLayer.Split('|');
            string myLayer = myLayers[layerIndex];

            var SelectedLayer = ListViewLayers.SelectedItem as CWorkingLayer;
            var projectdata = (ViewModels.ProjectData)BindingContext;
            await projectdata.SelectLayer(SelectedLayer);
            var btn = new Button()
            {
                Text = SelectedLayer.Name,
                FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Button)),
                CommandParameter = SelectedLayer,
            };
            btn.Clicked += Btn_Clicked;
            SelectedLayer.OwnerLayers = GetOwnerLayers();
            StackBreadcrumb.Children.Add(btn);

        }

        #endregion
    }
}