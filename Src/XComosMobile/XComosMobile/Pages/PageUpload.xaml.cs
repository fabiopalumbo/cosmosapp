using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XComosMobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageUpload : PageTemplate
    {
        private Services.XDatabase db { get { return Services.XServices.Instance.GetService<Services.XDatabase>(); } }
        private ViewModels.ProjectData ProjectData { get { return Services.XServices.Instance.GetService<ViewModels.ProjectData>(); } }
        public PageUpload()
        {
            InitializeComponent();


        }

        private async void ListViewObjects_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ComosWebSDK.Data.CObject cached = (ComosWebSDK.Data.CObject)e.Item;
            ViewModels.ProjectData projectdata = Services.XServices.Instance.GetService<ViewModels.ProjectData>();

            comos.PageSpecifications page = new comos.PageSpecifications(projectdata.SelectedDB.Key, cached, projectdata.SelectedLanguage.LCID);
            await this.Navigation.PushAsync(page);

        }
        private void GetAllObjectsWithCachedAttributes()
        {

            List<ComosWebSDK.Data.CObject> objects = null;

            Services.IDatabase db = Services.XServices.Instance.GetService<Services.IDatabase>();
            ViewModels.ProjectData projectdata = Services.XServices.Instance.GetService<ViewModels.ProjectData>();

            objects = db.GetDevicesWithCachedAttributes(projectdata.SelectedProject.UID, projectdata.SelectedLayer.UID);
            List<ComosWebSDK.Data.CObject> pictures = db.GetDevicesWithCachedPictures(projectdata.SelectedProject.UID, projectdata.SelectedLayer.UID);

            foreach (var item in pictures)
            {
                if (objects.Select(x => x.UID == item.UID).Count() == 0)
                    objects.Add(item);
            }


            listViewUploads.ItemsSource = objects;

        }

        private async void btUpload_Clicked(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
            //Frame frm = ((Frame)((Grid)bt.Parent).Parent);
            //  await bt.FadeTo(0, 300, Easing.Linear);

            ComosWebSDK.Data.CObject cached = (ComosWebSDK.Data.CObject)bt.BindingContext;
            var plataform = Services.XServices.Instance.GetService<Services.IPlatformSystem>();

            if (plataform.IsOnline)
                UploadItem(cached);
            else
                await App.Current.MainPage.DisplayAlert(Services.TranslateExtension.TranslateText("error"), "Debe tener conexion para realizar esta operacion", "OK");
        }

        private async void btDelete_Clicked(object sender, EventArgs e)
        {

            Button bt = (Button)sender;
            //Frame frm = ((Frame)((Grid)bt.Parent).Parent);
            // await bt.FadeTo(0, 300, Easing.Linear);

            ComosWebSDK.Data.CObject cached = (ComosWebSDK.Data.CObject)bt.BindingContext;
            string project = ProjectData.SelectedProject.UID;
            string workinglayer = ProjectData.SelectedLayer.UID;

            //db.DeleteCachedDevice(cached.TempUID);
            // todo delete all modifications of device in cache!!
            db.ClearAllSpecsFromOwner(cached.UID, project, workinglayer);
            GetAllObjectsWithCachedAttributes();

        }

        private void PageTemplate_Appearing(object sender, EventArgs e)
        {
            GetAllObjectsWithCachedAttributes();
        }

        private async void UploadItem(ComosWebSDK.Data.CObject item)
        {
            Pages.controls.MidiaControl media = new controls.MidiaControl();

            this.ShowSpinner(Services.TranslateExtension.TranslateText("saving"));

            var result = await SaveValuesToComos(item);
            if (result)
            {
                await media.UploadAllPicturesFromObject(item.UID);

            }

            GetAllObjectsWithCachedAttributes();

            this.HideSpinner();
        }
        private async Task UpdloadAll()
        {

            List<ComosWebSDK.Data.CObject> objects = (List<ComosWebSDK.Data.CObject>)listViewUploads.ItemsSource;
            Pages.controls.MidiaControl media = new controls.MidiaControl();

            this.ShowSpinner(Services.TranslateExtension.TranslateText("saving"));
            foreach (var item in objects)
            {
                var result = await SaveValuesToComos(item);
                if (result)
                {
                    await media.UploadAllPicturesFromObject(item.UID);

                }
            }

            GetAllObjectsWithCachedAttributes();

            this.HideSpinner();

            ShowToast(Services.TranslateExtension.TranslateText("done"));


        }

        private async Task<bool> SaveValuesToComos(ComosWebSDK.Data.CObject cacheobj)
        {
            string user = ProjectData.User;
            string project = ProjectData.SelectedProject.UID;
            string workinglayer = ProjectData.SelectedLayer.UID;
            string language = ProjectData.SelectedLanguage.LCID;

            var m_Http = Services.XServices.Instance.GetService<HttpClient>();
            ComosWebSDK.brcomosweb.ComosBRWeb m_comosbrweb = new ComosWebSDK.brcomosweb.ComosBRWeb(m_Http);


            var db = Services.XServices.Instance.GetService<Services.IDatabase>();
            IBRServiceContracts.CWriteValueCollection collection = db.GetAllCachedSpecsFromOwner(cacheobj.UID, project, workinglayer);

            //string servername = db.ReadSetting("ServerName", "");

            string servername = db.ReadSetting("ServerNameBR", "http://siemens.southcentralus.cloudapp.azure.com:5109/Service.svc");



            var result = true;

            if (collection.Values.Count() > 0)
            {
                this.ShowSpinner(Services.TranslateExtension.TranslateText("saving"));
                try
                {
                    result = await m_comosbrweb.WriteComosValues(collection, servername, user, project, workinglayer, language);
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                    return false;
                }              

                this.HideSpinner();
            }



            if (result)
            {
                if (cacheobj != null)
                    db.ClearAllSpecsFromOwner(cacheobj.UID, project, workinglayer);
                //ShowToast(Services.TranslateExtension.TranslateText("done"));
                return true;

            }
            else
            {
                //ShowToast(Services.TranslateExtension.TranslateText("save_failed"));
                return false;
            }

        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var plataform = Services.XServices.Instance.GetService<Services.IPlatformSystem>();

            if (plataform.IsOnline)
            {

                var answer = await App.Current.MainPage.DisplayAlert(Services.TranslateExtension.TranslateText("upload"), Services.TranslateExtension.TranslateText("save_question"), Services.TranslateExtension.TranslateText("yes"), Services.TranslateExtension.TranslateText("no"));
                if (answer)
                    await UpdloadAll();
            }
            else
            {
                await App.Current.MainPage.DisplayAlert(Services.TranslateExtension.TranslateText("error"), "Debe tener conexion para realizar esta operacion", "OK");
            }
        }
    }
}