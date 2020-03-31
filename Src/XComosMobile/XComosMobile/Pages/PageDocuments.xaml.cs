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
    public partial class PageDocuments : PageTemplate
    {

        private Services.XDatabase db { get { return Services.XServices.Instance.GetService<Services.XDatabase>(); } }
        private ViewModels.ProjectData ProjectData { get { return Services.XServices.Instance.GetService<ViewModels.ProjectData>(); } }

        public PageDocuments()
        {
            InitializeComponent();
            GetAllDocuments();
        }

        private async void ListViewObjects_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            //ComosWebSDK.Data.CObject cached = (ComosWebSDK.Data.CObject)e.Item;
            Services.IPlatformSystem plataform = Services.XServices.Instance.GetService<Services.IPlatformSystem>();
            ComosWebSDK.Data.CDocument doc = e.Item as ComosWebSDK.Data.CDocument;

            await plataform.OpenFile(doc.FileName, doc.MimeType);

        }
        private void GetAllDocuments()
        {

            List<ComosWebSDK.Data.CDocument> objects = null;

            Services.IDatabase db = Services.XServices.Instance.GetService<Services.IDatabase>();
            ViewModels.ProjectData projectdata = Services.XServices.Instance.GetService<ViewModels.ProjectData>();

            objects = db.GetAllDocuments(projectdata.SelectedProject.UID, projectdata.SelectedLayer.UID);

            listViewDocuments.ItemsSource = objects;

        }

        private async void btUpload_Clicked(object sender, EventArgs e)
        {
            Button bt = (Button)sender;

            ComosWebSDK.Data.CDocument doc = (ComosWebSDK.Data.CDocument)bt.BindingContext;

            this.ShowSpinner(Services.TranslateExtension.TranslateText("saving"));
            bool sucess = await UploadDocument(doc.FileName, doc.UID, doc.Name);
            this.HideSpinner();

            if (sucess)
            {
                ShowToast(Services.TranslateExtension.TranslateText("upload_sucess"));
            }
            else
            {
                ShowToast(Services.TranslateExtension.TranslateText("upload_failed"));
            }

        }

        public static async Task<bool> UploadDocument(string path, string uidowner, string name)
        {

            var m_Http = Services.XServices.Instance.GetService<HttpClient>();
            var plataform = Services.XServices.Instance.GetService<Services.IPlatformSystem>();
            var db = Services.XServices.Instance.GetService<Services.IDatabase>();
            var projectData = Services.XServices.Instance.GetService<ViewModels.ProjectData>();

            ComosWebSDK.brcomosweb.ComosBRWeb m_comosbrweb = new ComosWebSDK.brcomosweb.ComosBRWeb(m_Http);

            string servername = db.ReadSetting("ServerNameBR", "http://siemens.southcentralus.cloudapp.azure.com:5109/Service.svc");
            System.IO.Stream file = null;

            file = plataform.PathToStream(path);

            string ext = System.IO.Path.GetExtension(path).Replace(".", "");


            return await m_comosbrweb.AddRedLineAndAdditionalDocuments(servername, projectData.SelectedProject.UID,
               projectData.SelectedLayer.UID, uidowner, "",
               name, projectData.User, file, ext);


        }

        private void btDelete_Clicked(object sender, EventArgs e)
        {

            Button bt = (Button)sender;

            ComosWebSDK.Data.CDocument doc = (ComosWebSDK.Data.CDocument)bt.BindingContext;
            Services.IDatabase db = Services.XServices.Instance.GetService<Services.IDatabase>();
            Services.IPlatformSystem plataform = Services.XServices.Instance.GetService<Services.IPlatformSystem>();

            db.DeleteDocument(doc.FileName);
            plataform.DeleteFile(doc.FileName);
            GetAllDocuments();
            //this.ShowSpinner(Services.TranslateExtension.TranslateText("saving"));
            //await UploadDocument(doc.FileName, doc.UID, doc.Name);
            //this.HideSpinner();

        }
    }
}