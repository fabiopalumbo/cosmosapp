using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XLabs.Forms.Behaviors;

namespace XComosMobile.Pages.controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MidiaControl : ContentView
    {

        public static StackLayout MidiaPanel { get; set; }

        private static string tempuid = "";
        private static string name = "";
        private static List<ImageHelper> images = null;
        private static List<string> audios = null;


        public MidiaControl()
        {
            InitializeComponent();
            MidiaPanel = m_Photo_Panel;
            
            m_Photo_Panel.ChildAdded += M_Photo_Panel_ChildAdded;
            m_Photo_Panel.ChildRemoved += M_Photo_Panel_ChildRemoved;

        }

        private void M_Photo_Panel_ChildRemoved(object sender, ElementEventArgs e)
        {
            if (m_Photo_Panel.Children.Count == 0)
            {
                this.IsVisible = false;
                mainStack.IsVisible = false;
            }
        }

        private void M_Photo_Panel_ChildAdded(object sender, ElementEventArgs e)
        {
            if (m_Photo_Panel.Children.Count > 0)
            {
                this.IsVisible = true;
                mainStack.IsVisible = true;
            }
        }

        public void StartComponent(string uid, string owner)
        {
            tempuid = uid;
            name = owner;
            images = this.LoadImages();
            audios = this.LoadAudios();


            TapGestureRecognizer gesture = new TapGestureRecognizer();
            gesture.Tapped += async (object sender, EventArgs e) =>
            {                
                await App.Navigation.PushAsync(new Pages.inspection.PagePicture(images,audios));
            };

            lblMidia.GestureRecognizers.Add(gesture);
        }

        private List<ImageHelper> LoadImages()
        {
            var db = Services.XServices.Instance.GetService<Services.IDatabase>();
            var projectData = Services.XServices.Instance.GetService<ViewModels.ProjectData>();
            var platform = Services.XServices.Instance.GetService<Services.IPlatformSystem>();
            List<ImageHelper> imageList = new List<ImageHelper>();

            Dictionary<string, string> dic = db.GetPicturesFromDevice(tempuid, projectData.SelectedProject.UID, projectData.SelectedLayer.UID);

            foreach (var item in dic)
            {
                              
                byte[] source = platform.PathToByte(item.Key);
                if (source.Length > 1)
                {
                    Cameraimage(source, item.Key, item.Value, false);
                    imageList.Add(new ImageHelper() { ImageSource = source, Path = item.Key, Date = item.Value });
                }
            }

            return imageList;
        }

        private List<string> LoadAudios()
        {
            var db = Services.XServices.Instance.GetService<Services.IDatabase>();
            var projectData = Services.XServices.Instance.GetService<ViewModels.ProjectData>();
            var audios = db.GetSoundFilePath(tempuid, projectData.SelectedProject.UID, projectData.SelectedLayer.UID);
            List<string> audioList = new List<string>();

            foreach (var item in audios)
            {
                IncludeAudio(item, false);
                audioList.Add(item);
            }
            return audioList;
        }

        public static View CreateMediaItemForAudio(string path, double width = 90, double height = 90)
        {

            var platform = Services.XServices.Instance.GetService<Services.IPlatformSystem>();

            //audios.Add(path);

            var viewcell = new CustomStackLayout()
            {
                Orientation = StackOrientation.Horizontal,                
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,                                
                Padding = new Thickness(5, 5),
                IsAudio = true,
                CustomData = path,
                BackgroundColor = Color.Transparent,
            };

            AFButton img = new AFButton
            {
                Text = "\uf028",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = (Color)Application.Current.Resources["UIFabButton"],
                TextColor = (Color)Application.Current.Resources["UIFabButtonText"],
                WidthRequest = width,
                HeightRequest = height                
            };
      

            viewcell.LongClick += Viewcell_LongClick;

            viewcell.Children.Add(img);

            //TapGestureRecognizer gesture = new TapGestureRecognizer();
            //gesture.Tapped += (object sender, EventArgs e) =>
            //{
            //    platform.PlayAudio(path);
            //};

            //viewcell.GestureRecognizers.Add(gesture);

            img.Clicked += (object sender, EventArgs e) =>
            {
                //AFButton bt = sender as AFButton;
                //if (!play)
                //{
                    //bt.Text = "\uf04d";
                    platform.PlayAudio(path);
                //    play = true;
                //}
                //else
                //{
                //    //bt.Text = "\uf028";
                //    platform.StopAudio();
                //    play = false;
                //}
                
           };

            return viewcell;

        }
        public static View CreateMediaItemForImage(byte[] imagesource, string path, string date, double width = 90, double height = 90)
        {

            //images.Add(new ImageHelper() { ImageSource = imagesource, Path = path, Date = date });

            var viewcell = new CustomStackLayout()
            //var viewcell = new CustomFrame()
            {                
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,
                Padding = new Thickness(5, 5),
                BackgroundColor = Color.Transparent,
                IsPicture = true,
                CustomData = path
            };

            FFImageLoading.Forms.CachedImage img = new FFImageLoading.Forms.CachedImage
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.Transparent,
                Source = ImageSource.FromStream(() => new System.IO.MemoryStream(imagesource)),
                WidthRequest = width,
                HeightRequest = height,                
            };            

            viewcell.LongClick += Viewcell_LongClick;
            //viewcell.Content = img;
            //img.Transformations.Add(new FFImageLoading.Transformations.CircleTransformation());

            viewcell.Children.Add(img);

            TapGestureRecognizer gesture = new TapGestureRecognizer();
            gesture.Tapped += (object sender, EventArgs e) =>
            {
                var platform = Services.XServices.Instance.GetService<Services.IPlatformSystem>();
                platform.ShowPicture(path);
            };

            viewcell.GestureRecognizers.Add(gesture);

            return viewcell;

        }

        private static async void Viewcell_LongClick(object sender, EventArgs e)
        {
            CustomStackLayout data = sender as CustomStackLayout;
            string path = (string)data.CustomData;
            Page pg = new Page();
            var platform = Services.XServices.Instance.GetService<Services.IPlatformSystem>();
           

            var action = await pg.DisplayActionSheet(Services.TranslateExtension.TranslateText("delete_question"), Services.TranslateExtension.TranslateText("cancel"), Services.TranslateExtension.TranslateText("delete"));

            if (action != Services.TranslateExtension.TranslateText("cancel"))
            {
                
                if (action == Services.TranslateExtension.TranslateText("delete"))
                {
                    platform.DeleteFile(path);
                    var db = Services.XServices.Instance.GetService<Services.IDatabase>();
                    if (data.IsPicture)
                    {
                        db.DeletePhoto(path);
                    }
                    else if (data.IsAudio)
                    {
                        db.DeleteAudio(path);
                    }                    

                    MidiaPanel.Children.Remove(data);
                    MidiaPanel.ForceLayout();
                }
                    
            }

        }

        public void IncludeAudio(string path, bool include = true)
        {
            if (include)
                audios.Add(path);

            m_Photo_Panel.Children.Add(CreateMediaItemForAudio(path));
        }

        public void Cameraimage(byte[] imagesource, string path, string date, bool include = true)
        {
            if (include)
            images.Add(new ImageHelper() { ImageSource = imagesource, Path = path, Date = date });

            m_Photo_Panel.Children.Add(CreateMediaItemForImage(imagesource,path,date));

        }

        public void SaveImage(string path)
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            var db = Services.XServices.Instance.GetService<Services.IDatabase>();
            var projectData = Services.XServices.Instance.GetService<ViewModels.ProjectData>();

            db.CacheDevicePicture(projectData.SelectedProject.UID, projectData.SelectedLayer.UID, tempuid, path, date,name);
        }

        public async Task UploadAllAudiosFromObject(string uidowner, string uidNewOwner = "")
        {

            var db = Services.XServices.Instance.GetService<Services.IDatabase>();
            var projectData = Services.XServices.Instance.GetService<ViewModels.ProjectData>();
            var platform = Services.XServices.Instance.GetService<Services.IPlatformSystem>();

            Dictionary<string, string> dic = db.GetAudiosFromDevice(uidowner, projectData.SelectedProject.UID, projectData.SelectedLayer.UID);

            if (uidNewOwner == "")
                uidNewOwner = uidowner;

            foreach (var item in dic)
            {
                string source = item.Key;

                if (!db.IsAudioUploaded(source))
                {
                    bool uploaded = await Upload_Audio(source, item.Value, uidNewOwner);
                   if (uploaded)
                     db.MarkAudioAsUploaded(source);
                }
            }

            platform.ShowToast(Services.TranslateExtension.TranslateText("done"));


        }

        public async Task UploadAllPicturesFromObject(string uidowner, string uidNewOwner = "")
        {

            var db = Services.XServices.Instance.GetService<Services.IDatabase>();
            var projectData = Services.XServices.Instance.GetService<ViewModels.ProjectData>();
            var platform = Services.XServices.Instance.GetService<Services.IPlatformSystem>();            

            Dictionary<string, string> dic = db.GetPicturesFromDevice(uidowner, projectData.SelectedProject.UID, projectData.SelectedLayer.UID);

            if (uidNewOwner == "")
                uidNewOwner = uidowner;

            foreach (var item in dic)
            {
                string source = item.Key;

                if (!db.IsPictureUploaded(source))
                {
                    bool uploaded = await Upload_Photo(source, item.Value, uidNewOwner);
                    if (uploaded)
                        db.MarkPictureAsUploaded(source);
                }               
            }

            platform.ShowToast(Services.TranslateExtension.TranslateText("done"));
        
        }

        public static async Task<bool> Upload_Audio(string path, string name, string uidowner)
        {

            var m_Http = Services.XServices.Instance.GetService<HttpClient>();
            var plataform = Services.XServices.Instance.GetService<Services.IPlatformSystem>();
            var db = Services.XServices.Instance.GetService<Services.IDatabase>();
            var projectData = Services.XServices.Instance.GetService<ViewModels.ProjectData>();

            ComosWebSDK.brcomosweb.ComosBRWeb m_comosbrweb = new ComosWebSDK.brcomosweb.ComosBRWeb(m_Http);

            string servername = db.ReadSetting("ServerNameBR", "http://siemens.southcentralus.cloudapp.azure.com:5109/Service.svc");
            System.IO.Stream file = null;

            file = plataform.PathToStream(path);
            
            return await m_comosbrweb.AddFileToComosObject(servername, projectData.SelectedProject.UID,
               projectData.SelectedLayer.UID, uidowner, "",
               name, projectData.User, file,"mp4");

        
        }
    public static async Task<bool> Upload_Photo(string path, string name, string uidowner, bool downscale = true)
        {

            var m_Http = Services.XServices.Instance.GetService<HttpClient>();
            var plataform = Services.XServices.Instance.GetService<Services.IPlatformSystem>();
            var db = Services.XServices.Instance.GetService<Services.IDatabase>();
            var projectData = Services.XServices.Instance.GetService<ViewModels.ProjectData>();

            ComosWebSDK.brcomosweb.ComosBRWeb m_comosbrweb = new ComosWebSDK.brcomosweb.ComosBRWeb(m_Http);

            string servername = db.ReadSetting("ServerNameBR", "http://siemens.southcentralus.cloudapp.azure.com:5109/Service.svc");
            System.IO.Stream photo = null;
            try
            {
                if (plataform.FileExists(path))
                {
                    if (downscale)                    
                        photo = await plataform.DownScaleImage(path);                 
                    else                    
                        photo = plataform.PathToStream(path);                    

                    return await m_comosbrweb.AddPictureToComosObject(servername, projectData.SelectedProject.UID,
                       projectData.SelectedLayer.UID, uidowner, "",
                       name, projectData.User, photo);
                }
            }
            catch (Exception ex)
            {
                plataform.ShowToast(ex.Message);
            }

            return false;
      
        }

    }

    public class ImageHelper
    {
        public byte[] ImageSource { get; set; }
        public string Path { get; set; }
        public string Date { get; set; }
    }
}