using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


using ComosWebSDK;
using ComosWebSDK.Data;
using ComosWebSDK.Extensions;

namespace XComosMobile.Pages.comos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageSpecifications : PageTemplate
    {
        IComosWeb m_ComosWeb
        {
            get
            {
                return Services.XServices.Instance.GetService<IComosWeb>();
            }
        }
        string m_Navigator;
        CObject obj;
        string m_Language;
        public PageSpecifications(string navigator, CObject o, string language)
        {
            this.MainObject = o;
            this.m_Language = language;
            this.BindingContext = this;
            this.ShowAttributeView = false;

            //            InitializeComponent();            

            //this.CheckFavorite();
            m_Navigator = navigator;
            obj = o;
            Device.BeginInvokeOnMainThread(async () => { await UpdateDetails(); });
            //StartToolBar();
        }

        public string Datababase { get => m_Navigator; }
        public List<CSpecification> Specifications { get; set; }

        public async Task UpdateDetails()
        {
            this.ShowSpinner(Services.TranslateExtension.TranslateText("loading"));

            ViewModels.ProjectData projectdata = Services.XServices.Instance.GetService<ViewModels.ProjectData>();

            try
            {
                this.Specifications = await m_ComosWeb.GetObjectSpecification(
                                        m_Navigator, projectdata.SelectedProject.UID, projectdata.SelectedLayer.UID, m_Language, obj.UID);
            }
            catch (Exception e)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Error al obtener atributos: " + e.Message, Services.TranslateExtension.TranslateText("OK"));
                return;
            }

            OnPropertyChanged("Specifications");
            this.HideSpinner();

            if (this.Specifications == null || this.Specifications.Count == 0)
            {
                await App.Current.MainPage.DisplayAlert("", Services.TranslateExtension.TranslateText("no_attributes"), "OK");
                this.Navigation.RemovePage(this);
                return;
            }


            foreach (var item in this.Specifications)
            {
                if (item.Name.Equals(Constants.MobileTabName))
                {
                    PageAttributes page = new PageAttributes(this.m_Navigator, m_Language, item, obj);
                    this.Navigation.RemovePage(this);
                    await this.Navigation.PushAsync(page);
                }
            }

            InitializeComponent();

        }

        public async void OnSpecificationSelected(object sender, EventArgs e)
        {
            CSpecification spec = (CSpecification)ListViewObjects.SelectedItem;
            PageAttributes page = new PageAttributes(this.m_Navigator, m_Language, spec, obj);
            await this.Navigation.PushAsync(page);
        }

    }
}