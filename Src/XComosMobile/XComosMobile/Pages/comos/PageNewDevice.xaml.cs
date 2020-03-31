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
using System.Net.Http;
using System.IO;
using XLabs.Forms.Behaviors;
using XComosMobile.QR_Scanner;
using ComosWebSDK.UI;
using Rg.Plugins.Popup.Services;
using XComosMobile.PopUps;
using XComosMobile.Pages.controls;
using IBRServiceContracts;

namespace XComosMobile.Pages.comos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageNewDevice : PageTemplate, AttributesUpdater
    {
        public static bool IsCamera;
        //public static StackLayout panel;
        public static controls.MidiaControl midiacontrol;

        IComosWeb m_ComosWeb { get { return Services.XServices.Instance.GetService<ComosWebSDK.IComosWeb>(); } }
        Dictionary<string, IBRServiceContracts.CWriteValue> Values = new Dictionary<string, IBRServiceContracts.CWriteValue>();

        private static string tempuid = "";
        private static string uidowner = "";
        private static string uidcdev = "";

        int targetIndex;
        Button equipmentVerified;
        string verifiedSystemUid = "";

        static string ownerpic = "";
        static string ownername = "";
        static string servername = "";

        static string user = "";
        static string project = "";
        static string workinglayer = "";
        static string language = "";

        ViewModels.ProjectData ProjectData = Services.XServices.Instance.GetService<ViewModels.ProjectData>();
        Services.XDatabase db = Services.XServices.Instance.GetService<Services.XDatabase>();


        CCachedDevice cacheddevice = null;
        public string Description { get; set; }

        public PageNewDevice(ComosWebSDK.UI.UICachedScreen screen, string uid, string pic, string owner, CCachedDevice cached = null)
        {
            InitializeComponent();

            midiacontrol = midiaControl;
            cacheddevice = cached;
            verifiedSystemUid = "";

            txtDescription.BindingContext = this;

            this.ShowHome = false;
            this.ShowFavoriteButton = false;
            this.ShowAttributeView = false;
            this.Title = owner;

            uidowner = uid;
            uidcdev = screen.UID;
            ownername = owner;
            user = ProjectData.User;
            project = ProjectData.SelectedProject.UID;
            workinglayer = ProjectData.SelectedLayer.UID;
            language = ProjectData.SelectedLanguage.LCID;
            servername = db.ReadSetting("ServerNameBR", "http://siemens.southcentralus.cloudapp.azure.com:5109/Service.svc");

            //NavigationBarControl.OnSaved += NavigationBarControl_OnSaved;            

            UpdateAttributesUI(screen.JSONContent);

            DescLabel.WidthRequest = (XComosMobile.App.WidthPixels / 3);

            if (cached != null)
            {
                tempuid = cached.TempUID;
                this.Description = cached.Description;
                OnPropertyChanged("Description");
            }
            else
            {
                tempuid = UniqueIDGenerator.GenerateID();
            }
            string name = "";
            if (MainObject != null)
                name = MainObject.Name;
            midiaControl.StartComponent(tempuid, name);
        }

        private async Task<bool> CacheDevice()
        {
            IBRServiceContracts.CWriteValueCollection collection = new IBRServiceContracts.CWriteValueCollection()
            {
                Values = Values.Values.ToArray(),
            };

            db.CacheDeviceToCreate(project, workinglayer, uidowner, uidcdev, collection, tempuid, ownername, ownerpic, this.Description);

            this.ShowToast(Services.TranslateExtension.TranslateText("item_cached"));
            await this.Navigation.PopAsync(true);
            return true;
        }

        private void Button_Clicked_Photo(object sender, EventArgs e)
        {

            var platform = Services.XServices.Instance.GetService<Services.IPlatformSystem>();
            IsCamera = true;
            platform.CameraMedia("PageNewDevice");

        }

        bool recording = false;
        string recordPath = "";
        private void StartRecording()
        {
            var platform = Services.XServices.Instance.GetService<Services.IPlatformSystem>();
            recordPath = platform.StartRecordingAudio();
            recording = (recordPath != "");

        }

        private void StopRecording()
        {
            if (recording)
            {
                var db = Services.XServices.Instance.GetService<Services.IDatabase>();
                var platform = Services.XServices.Instance.GetService<Services.IPlatformSystem>();
                var projectData = Services.XServices.Instance.GetService<ViewModels.ProjectData>();
                platform.StopRecordingAudio();
                platform.ShowToast(Services.TranslateExtension.TranslateText("saving"));

                // save sound path
                db.CacheSoundFilePath(recordPath, tempuid, projectData.SelectedProject.UID, projectData.SelectedLayer.UID);
                midiaControl.IncludeAudio(recordPath);

                recordPath = "";
            }
            recording = false;
        }


        public static void SaveImage(string path)
        {

            midiacontrol.SaveImage(path);
        }
        public static void Cameraimage(byte[] imagesource, string path, string date)
        {

            midiacontrol.Cameraimage(imagesource, path, date);

            /*
            var viewcell = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                Padding = new Thickness(5, 5),
                BackgroundColor = Color.Transparent,
            };
            
            FFImageLoading.Forms.CachedImage img = new FFImageLoading.Forms.CachedImage
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.Transparent,
                Source = ImageSource.FromStream(() => new System.IO.MemoryStream(imagesource)),
                WidthRequest = 90,
                HeightRequest = 90,                
        };
  
            //img.Transformations.Add(new FFImageLoading.Transformations.CircleTransformation());
            
            viewcell.Children.Add(img);                   

            TapGestureRecognizer gesture = new TapGestureRecognizer();
            gesture.Tapped += (object sender, EventArgs e) => 
                            {                                
                                var platform = Services.XServices.Instance.GetService<Services.IPlatformSystem>();
                                platform.ShowPicture(path);
                                //Button_Upload_Photo(path, date);
                            };

            viewcell.GestureRecognizers.Add(gesture);
            
            panel.Children.Add(viewcell);                       
            */


        }

        public static async Task Button_Upload_Photo(string path, string name)
        {

            var m_Http = Services.XServices.Instance.GetService<HttpClient>();
            var plataform = Services.XServices.Instance.GetService<Services.IPlatformSystem>();

            ComosWebSDK.brcomosweb.ComosBRWeb m_comosbrweb = new ComosWebSDK.brcomosweb.ComosBRWeb(m_Http);

            //Stream photo = plataform.PathToStream(path);

            try
            {
                var photo = await plataform.DownScaleImage(path);
                var result = await m_comosbrweb.AddPictureToComosObject(servername, project, workinglayer, uidowner, "", name, user, photo);
            }
            catch (Exception ex)
            {
                plataform.ShowToast(ex.Message);
            }
        }

        private async void Button_Clicked_Create(object sender, EventArgs e)
        {

            var plataform = Services.XServices.Instance.GetService<Services.IPlatformSystem>();

            if (plataform.IsOnline)
            {
                await SaveDeviceToComos();
            }
            else
            {
                await CacheDevice();
            }

        }

        private async Task<bool> SaveDeviceToComos()
        {


            IBRServiceContracts.CWriteValueCollection collection = new IBRServiceContracts.CWriteValueCollection()
            {
                Values = Values.Values.ToArray(),
            };

            if (uidowner.Equals("DUMMY_INCIDENT") && collection.Values.Any(value => value.NewValue.Equals("")))
            {
                this.ShowToast("Se deben completar todos los campos para guardar el evento");
                return false;
            }
            else if (!uidowner.Equals("DUMMY_INCIDENT") && collection.Values.SkipWhile(value => value.NestedName.Equals("Z10T00002.Z001A")).Any(value => value.NewValue.Equals("")))
            {
                this.ShowToast("Se deben completar todos los campos para guardar el evento");
                return false;
            }
            else
            {
                this.ShowSpinner(Services.TranslateExtension.TranslateText("creating_item"));

                var m_Http = Services.XServices.Instance.GetService<HttpClient>();
                ComosWebSDK.brcomosweb.ComosBRWeb m_comosbrweb = new ComosWebSDK.brcomosweb.ComosBRWeb(m_Http);

                try
                {

                    if (Description == "")
                        Description = Services.TranslateExtension.TranslateText("new_incident");

                    TResult<string> result;

                    if(uidowner.Equals("DUMMY_INCIDENT") && equipmentVerified.Text.Equals(Constants.OkIcon))
                    {
                        result = await m_comosbrweb.CreateComosDevice(collection, servername, project, workinglayer, language, verifiedSystemUid, uidcdev, user, Description);
                    }
                    else if(uidowner.Equals("DUMMY_INCIDENT") && !equipmentVerified.Text.Equals(Constants.OkIcon))
                    {
                        var result2 = await App.Current.MainPage.DisplayAlert("Guardar", "Desea guardar el aviso aunque el equipo donde se guardará no exista?", "Si", "No");
                        if (result2)
                        {
                            result = await m_comosbrweb.CreateComosDevice(collection, servername, project, workinglayer, language, uidowner, uidcdev, user, Description);
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        result = await m_comosbrweb.CreateComosDevice(collection, servername, project, workinglayer, language, uidowner, uidcdev, user, Description);
                    }

                    this.HideSpinner();


                    try
                    {
                        if (!result.Status)
                        {
                            this.ShowToast("Error en la conexión con COMOS. Contacte al administrador. Referencia: COMOS.IO");
                        }
                        else
                        {
                            if (result.data != "")
                            {
                                db.DeleteCachedDevice(tempuid);
                                this.ShowToast(Services.TranslateExtension.TranslateText("item_created"));

                                this.ShowSpinner(Services.TranslateExtension.TranslateText("saving"));
                                await midiacontrol.UploadAllPicturesFromObject(tempuid, result.data);
                                await midiacontrol.UploadAllAudiosFromObject(tempuid, result.data);
                                this.HideSpinner();
                            }
                            else
                            {
                                this.ShowToast("Error en la conexión con COMOS. Contacte al administrador. Referencia: COMOS.IO");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        this.ShowToast(ex.Message);
                        await CacheDevice();
                    }
                    finally
                    {
                        await this.Navigation.PopAsync(true);
                    }

                }
                catch (Exception ex2)
                {
                    await App.Current.MainPage.DisplayAlert("Error", ex2.Message, "OK");
                    this.HideSpinner();
                }

                return true;
            }

        }

        private void NavigationBarControl_OnSaved(object sender, EventArgs e)
        {

            //foreach (string item in changeditems.Keys)
            //{
            //    string value = "";
            //    ComosWebSDK.UI.UIBase spec = changeditems[item];

            //    value = spec.CachedValue;
            //    cachedb.WriteCacheSpecValue(MainObject.UID, item, MainObject.ProjectUID, MainObject.OverlayUID, value);
            //}
        }

        public async Task UpdateAttributesUI(string html)
        {
            var rejectionFrame = "Rechazo";
            var UltimoUsuarioFrame = "Última modificación";
            StackLayout stack = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Vertical,
            };

            var attributes = CHtmlParser.ParseAttributesForUI(html);
            var uiParser = new UI.HTMLToUIParser();
            uiParser.MainObject = this.MainObject;
            uiParser.CachedDevice = this.cacheddevice;

            foreach (var attribute in attributes)
            {

                if ((attribute.Text != rejectionFrame) && ((attribute.Text != UltimoUsuarioFrame)))
                {
                    var elm = uiParser.UpdateAttributesUI(attribute);
                    if (elm != null)
                    {
                        stack.Children.Add(elm);
                        //this.MobileTabFrame.Content = ((Frame)elm).Content;
                    }
                }
            }

            stack.Padding = new Thickness(0, 0, 0, 0);
            stack.HorizontalOptions = LayoutOptions.FillAndExpand;
            stack.VerticalOptions = LayoutOptions.FillAndExpand;

            this.Values = uiParser.Values;
            this.m_Panel.Content = stack;

            addEquipmentButton();

        }


        private void addEquipmentButton()
        {

            var targetAttribute = "Z10T00002.Z001A"; //Object ID
            var targetFound = false;
            var targetIndex = 0;
            targetIndex = 0;

            //check if attribute Z10T00002.Z001A (Object ID) exists. If found, save index
            var attributes = this.Values.Values.ToArray();
            foreach (var attribute in attributes)
            {
                if (!targetFound) { targetIndex = targetIndex + 1; }
                if (attribute.NestedName == targetAttribute)
                {
                    targetFound = true;
                }
            }

            this.targetIndex = targetIndex;

            if (targetFound)    //if I found Z10T00002.Z001A, add QR button (only if it isn't an incident of a task)
            {
                StackLayout m_panelContent = (StackLayout)this.m_Panel.Content;
                //StackLayout m_panelContent = (StackLayout)this.MobileTabFrame.Content;
                var listm_panelChildren = m_panelContent.Children;
                Frame myFrame = (Frame)listm_panelChildren[0];
                var fcontent = myFrame.Content;
                StackLayout fStacklayout = (StackLayout)fcontent;
                var listChildren = fStacklayout.Children;
                StackLayout fStacklayout2 = (StackLayout)listChildren[targetIndex];  //busco el indice de la lista que se que es el object ID

                if (uidowner.Equals("DUMMY_INCIDENT")) // It isn't an incident of a task
                {
                    CustomEntry equipmentNameEntry = (CustomEntry)fStacklayout2.Children[1];
                    equipmentNameEntry.IsEnabled = false;
                    equipmentNameEntry.BackgroundColor = Color.LightGray;
                    var grid = new Grid();

                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(43) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(43) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(43) });

                    grid.Children.Add(fStacklayout2, 0, 0);

                    Button addEquipmentButton = new Button
                    {
                        Text = Constants.RightArrowIcon,
                        Padding = 0,
                        FontSize = 25,
                        FontFamily = Device.RuntimePlatform == Device.Android ? "fontawesome.ttf#fontawesome" : null // set only for Android
                    };

                    Button equipmentVerifiedButton = new Button
                    {
                        Text = Constants.CancelIcon,
                        Padding = 0,
                        FontSize = 20,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center,
                        WidthRequest = 30,
                        HeightRequest = 30,
                        CornerRadius = 30,
                        BackgroundColor = Color.Red,
                        FontFamily = Device.RuntimePlatform == Device.Android ? "fontawesome.ttf#fontawesome" : null // set only for Android
                    };

                    addEquipmentButton.Clicked += AddEquipment_Clicked;
                    equipmentVerifiedButton.Clicked += EquipmentVerified_Clicked;

                    grid.Children.Add(addEquipmentButton, 1, 0);
                    grid.Children.Add(equipmentVerifiedButton, 2, 0);

                    listChildren.Insert(targetIndex, grid);
                    equipmentVerified = equipmentVerifiedButton;
                }
                else // It's an incident of a task
                {
                    listChildren.RemoveAt(targetIndex);
                }
            }
        }

        private void EquipmentVerified_Clicked(object sender, EventArgs e)
        {
            if (equipmentVerified.Text.Equals(Constants.OkIcon))
                ShowToast("Equipo verificado");
            else
                ShowToast("Equipo no verificado");
        }

        public void updateEquipmentData(string name, bool isVerified, string verifiedSystemUid)
        {
            StackLayout m_panelContent = (StackLayout)this.m_Panel.Content;
            var listm_panelChildren = m_panelContent.Children;
            Frame myFrame = (Frame)listm_panelChildren[0];
            var fcontent = myFrame.Content;
            StackLayout fStacklayout = (StackLayout)fcontent;
            var listChildren = fStacklayout.Children;
            Grid grid = (Grid)listChildren[targetIndex];
            StackLayout gStackLayout = (StackLayout)grid.Children[0];

            CustomEntry equipmentNameEntry = (CustomEntry)gStackLayout.Children[1];
            equipmentNameEntry.Text = name;

            if(isVerified)
            {
                equipmentVerified.Text = Constants.OkIcon;
                equipmentVerified.BackgroundColor = Color.Green;
            }
            else
            {
                equipmentVerified.Text = Constants.CancelIcon;
                equipmentVerified.BackgroundColor = Color.Red;
            }
            this.verifiedSystemUid = verifiedSystemUid;
        }

        private async void AddEquipment_Clicked(object sender, EventArgs e)
        {
            equipmentVerified.Text = Constants.CancelIcon;
            equipmentVerified.BackgroundColor = Color.Red;
            await PopupNavigation.Instance.PushAsync(new AddEquipment(this));
        }

        private void btRecord_Pressed(object sender, EventArgs e)
        {
            StartRecording();
        }

        private void btRecord_Cancel(object sender, EventArgs e)
        {
            var platform = Services.XServices.Instance.GetService<Services.IPlatformSystem>();
            platform.StopRecordingAudio();
            platform.CancelRecording(recordPath);
            platform.ShowToast(Services.TranslateExtension.TranslateText("cancel"));
        }

        private void btRecord_Released(object sender, EventArgs e)
        {
            StopRecording();
        }


    }
}