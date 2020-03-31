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
using System.Net;
using ComosWebSDK.UI;
using XComosMobile.Pages.maintenance;
using XComosMobile.Pages.controls;
using Rg.Plugins.Popup.Services;
using XComosMobile.PopUps;

namespace XComosMobile.Pages.comos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageAttributes : PageTemplate, AttributesUpdater
    {
        ViewModels.ProjectData ProjectData = Services.XServices.Instance.GetService<ViewModels.ProjectData>();
        IComosWeb m_ComosWeb = Services.XServices.Instance.GetService<ComosWebSDK.IComosWeb>();
        Services.XDatabase cachedb = Services.XServices.Instance.GetService<Services.XDatabase>();
        Dictionary<string, IBRServiceContracts.CWriteValue> Values = new Dictionary<string, IBRServiceContracts.CWriteValue>();

        public static controls.MidiaControl midiacontrol;
        public bool belongsToAnEquipment = false;
        public bool isALoogBook = false;

        int targetIndex;
        Button equipmentVerified;
        string verifiedSystemUid = "";

        public string Database { get; set; }
        public string Language { get; set; }

        public bool isWorkpackage;
        public bool isPEMActivity = false;
        public bool reloadWorkPackageGrid = false;

        //Parte elemento y modo para las actividades PEM. Listas de string
        private List<String> FailureTreeFullLabels = new List<string>();
        private List<String> FailureTreeDescriptions = new List<string>();
        private List<String> partes = new List<string>();
        private List<String> partesLabel = new List<string>();
        private List<String> elementos = new List<string>();
        private List<String> elementosLabel = new List<string>();
        private List<int> elementosIndex = new List<int>();
        private List<String> modos = new List<string>();
        private List<String> modosLabel = new List<string>();

        XComosMobile.Pages.controls.CustomPicker elementPicker;
        XComosMobile.Pages.controls.CustomPicker modePicker;

        private string newicon = comos.Constants.PlusIcon;
        public string NewDevIcon
        {
            get
            {
                return newicon;
            }
            set
            {
                newicon = value;
                OnPropertyChanged("NewDevIcon");
            }
        }
        private bool viewsave = true;
        public bool ViewSave
        {
            get
            {
                return viewsave;
            }
            set
            {
                viewsave = value;
                OnPropertyChanged("ViewSave");
            }
        }

        private bool viewdev = false;
        public bool ViewNewDev
        {
            get
            {
                return viewdev;
            }
            set
            {
                viewdev = value;
                OnPropertyChanged("ViewNewDev");
            }
        }

        private void btNew_Clicked(object sender, EventArgs e)
        {
            OnNewClicked(sender, new EventArgs());
        }

        public event EventHandler OnNew;
        protected virtual void OnNewClicked(object sender, EventArgs e)
        {
            EventHandler handler = OnNew;
            if (handler != null)
            {
                handler(this.MainObject, e);
            }
        }

        public PageAttributes(string database, string language, CSpecification spec, CObject obj)
        {
            InitializeComponent();
            InitializeAttributes(database, language, spec, obj);
            this.btAddNewJDEMaterial.IsVisible = false;
            this.btTasks.IsVisible = false;
            this.isWorkpackage = false;
            this.isPEMActivity = false;
            Device.BeginInvokeOnMainThread(async () => { await UpdateAttributesUI(spec, obj, language); });
        }

        public PageAttributes(string database, string language, CSpecification spec, CObject obj, string wp, string otNumber)
        {
            InitializeComponent();
            InitializeAttributes(database, language, spec, obj);
            this.isWorkpackage = true;
            this.btTasks.IsVisible = true;
            this.isPEMActivity = false;
            Device.BeginInvokeOnMainThread(async () => { await UpdateAttributesUI(spec, obj, language); });
            if (otNumber != "") { this.Title = otNumber; }
        }

        public PageAttributes()
        {
            InitializeComponent();
            this.isPEMActivity = false;
            this.btTasks.IsVisible = false;
            this.btAddNewJDEMaterial.IsVisible = false;
        }

        public void InitializeAttributes(string database, string language, CSpecification spec, CObject obj)
        {
            this.btNew.BindingContext = this;

            midiacontrol = midiaControl;

            if (obj.ProjectUID == null)
                obj.ProjectUID = spec.ProjectUID;

            if (obj.OverlayUID == null)
                obj.OverlayUID = spec.OverlayUID;


            this.Icon = obj.Picture;
            this.Title = obj.Name;
            this.MainObject = obj;
            this.BindingContext = this;
            this.Database = database;
            this.Language = language;

            this.ButtonPlayPause.IsVisible = false;
            //this.ShowHome = true;
            //this.ShowAttributeView = true;
            this.CheckFavorite();
            NavigationBarControl.OnSaved += NavigationBarControl_OnSaved;

            midiaControl.StartComponent(MainObject.UID, MainObject.Name);
        }

        public async Task Init(string database, string language, CSpecification spec, CObject obj)
        {

            this.Icon = obj.Picture;
            this.Title = obj.Name;
            this.MainObject = obj;
            this.BindingContext = this;
            this.Database = database;
            this.Language = language;
            this.CheckFavorite();

            NavigationBarControl.OnSaved += NavigationBarControl_OnSaved;
            await UpdateAttributesUI(spec, obj, language, false);
        }

        private void CacheAttributes()
        {
            foreach (var item in Values.Values)
            {
                string value = item.NewValue;
                cachedb.WriteCacheSpecValue(MainObject.UID, item.NestedName, MainObject.ProjectUID, MainObject.OverlayUID, value, item.Description, MainObject.Name, MainObject.Description);
            }

            this.ShowToast(Services.TranslateExtension.TranslateText("cached_attributes"));
        }

        private void NavigationBarControl_OnSaved(object sender, EventArgs e)
        {
            CacheAttributes();
        }

        private ComosWebSDK.brcomosweb.ComosBRWeb GetBRComosWeb()
        {

            var m_Http = Services.XServices.Instance.GetService<HttpClient>();
            ComosWebSDK.brcomosweb.ComosBRWeb m_comosbrweb = new ComosWebSDK.brcomosweb.ComosBRWeb(m_Http);

            return m_comosbrweb;
        }

        public async void OnSaveClicked(object sender, EventArgs e)
        {
            bool cancelBackButtonAction = false;
            var plataform = Services.XServices.Instance.GetService<Services.IPlatformSystem>();

            if (plataform.IsOnline)
            {
                //If is Parte,elemento y Modo (PEM) activity, Z10T00002.Z001E, Z10T00002.Z002E and Z10T00002.Z003E attributes must be in changed values, and they must have a value different from "". Else, cancel save! (prevent sending blank failure tree)
                bool rejectSave = false;
                if (this.isPEMActivity)
                {
                    var attributePEMindex = 0;
                    IBRServiceContracts.CWriteValueCollection collection = new IBRServiceContracts.CWriteValueCollection()
                    {
                        Values = Values.Values.ToArray(),
                    };

                    KeyValuePair<string, IBRServiceContracts.CWriteValue> changedValues = new KeyValuePair<string, IBRServiceContracts.CWriteValue>();
                    for (int i = 0; i < Values.Count; i++)
                    {
                        changedValues = Values.ElementAt(i);
                        if (changedValues.Key == "Z10T00002.Z001E" || changedValues.Key == "Z10T00002.Z002E" || changedValues.Key == "Z10T00002.Z003E")
                        {
                            attributePEMindex++;
                            if (changedValues.Value.ToString() == "")
                            {
                                rejectSave = true;
                            }
                        }
                    }
                    if (attributePEMindex != 3) { rejectSave = true; }  //if I didnt find three attributes, then they might not be completed

                }

                if (!rejectSave)
                {
                    await SaveValuesToComos();
                    this.ShowSpinner("Guardando media...");

                    await midiacontrol.UploadAllPicturesFromObject(MainObject.UID);
                    await midiacontrol.UploadAllAudiosFromObject(MainObject.UID);

                }
                else
                {
                    ShowToast("Se debe completar el árbol de fallas para poder guardar la actividad");
                    cancelBackButtonAction = true;
                }

                this.HideSpinner();
            }
            else
            {
                CacheAttributes();
            }

            if (!cancelBackButtonAction)
            {
                this.reloadWorkPackageGrid = true;
                DoBackButtonPressed();
            }

        }

        public async void OnAddNewJDEMaterialClick(object sender, EventArgs e)
        {
            await App.Navigation.PushAsync(new PageSearchMaterial(MainObject));
        }

        private async Task<bool> SaveValuesToComos()
        {
            string user = ProjectData.User;
            string project = ProjectData.SelectedProject.UID;
            string workinglayer = ProjectData.SelectedLayer.UID;
            string language = ProjectData.SelectedLanguage.LCID;

            IBRServiceContracts.CWriteValueCollection collection = new IBRServiceContracts.CWriteValueCollection()
            {
                Values = Values.Values.ToArray(),
            };

            var db = Services.XServices.Instance.GetService<Services.XDatabase>();
            string servername = db.ReadSetting("ServerNameBR", "http://siemens.southcentralus.cloudapp.azure.com:5109/Service.svc");

            this.ShowSpinner(Services.TranslateExtension.TranslateText("saving"));
            var m_comosbrweb = GetBRComosWeb();

            try
            {
                var result = await m_comosbrweb.WriteComosValues(collection, servername, user, project, workinglayer, language);

                this.HideSpinner();

                if (result)
                {
                    if (MainObject != null)
                        db.ClearAllSpecsFromOwner(MainObject.UID, project, workinglayer);
                    ShowToast(Services.TranslateExtension.TranslateText("done"));
                    return true;
                }
                else
                {
                    ShowToast(Services.TranslateExtension.TranslateText("save_failed"));
                    return false;
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                return false;
            }
        }

        UIBase[] m_UIAttributes = null;
        internal async Task<List<string>> DownloadDocuments()
        {
            List<string> list = new List<string>();
            await DownloadDocuments(m_UIAttributes, list);
            return list;
        }

        private async Task DownloadDocuments(UIBase[] attributes, List<string> list)
        {
            foreach (var attribute in attributes)
            {
                var uiframe = attribute as ComosWebSDK.UI.UIFrame;
                if (uiframe != null)
                {
                    await DownloadDocuments(uiframe.Children, list);
                    continue;
                }

                var uiquery = attribute as ComosWebSDK.UI.UIQuery;
                if (uiquery != null && uiquery.Result != null)
                {
                    //uiquery.Result //
                    // @ToDo: Execute download code
                    foreach (var row in uiquery.Result.Rows)
                    {
                        if (row.Items[0].UID != null)
                            list.Add(row.Items[0].UID);
                    }
                    continue;
                }
            }
        }

        private async void btTasks_Clicked(object sender, EventArgs e)
        {
            await App.Navigation.PushAsync(new Pages.maintenance.TasksPage(MainObject.UID));
        }

        protected override bool OnBackButtonPressed()
        {
            if (isWorkpackage)
            {
                this.Navigation.PushAsync(new WorkPackages(!this.reloadWorkPackageGrid));
                return true;
            }
            else
            {
                base.OnBackButtonPressed();
                return false;
            }
        }

        #region Using grid layout

        public async Task UpdateAttributesUI(CSpecification spec, CObject obj, string language, bool render = true)
        {
            try
            {
                if (render)
                    ShowSpinner(Services.TranslateExtension.TranslateText("loading"));
                //ShowStickSpinner("Loading values...");

                StackLayout stack = new StackLayout()
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    Orientation = StackOrientation.Vertical,
                };

                string html;
                try
                {
                    html = await m_ComosWeb.GetObjectSpecificationAsHtml(
                                this.Database, spec.ProjectUID, spec.OverlayUID, language, obj.UID, spec.Name);
                }
                catch (TaskCanceledException) { return; } // If there is a Logout Request
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Error al cargar atributos: " + ex.Message, Services.TranslateExtension.TranslateText("OK"));
                    return;
                }

                if (html == null)
                {
                    ShowToast("Error al cargar la información. Intente nuevamente.");
                    this.HideSpinner();
                    return;
                }


                m_UIAttributes = CHtmlParser.ParseAttributesForUI(html);
                await ExecuteQueriesInAttributes(m_UIAttributes);

                //just download the jsons
                if (!render)
                {
                    ShowToast("Error al cargar la información. Intente nuevamente.");
                    this.HideSpinner();
                    return;
                }

                var uiParser = new UI.HTMLToUIParser(this);
                uiParser.MainObject = this.MainObject;

                foreach (var attribute in m_UIAttributes)
                {
                    var elm = uiParser.UpdateAttributesUI(attribute);      //convert attribute to xamarin element
                    processDisplayElement(attribute);                      //parse xamarin element to extract info in selected attributes
                    if (elm != null && isAttributeVisible(attribute))       //if elm is not null and is a visible attribute, then add to display stack
                    {
                        View myNewElement = postProcessDisplayElement(elm, attribute);

                        if(isALoogBook)
                        {
                            Frame myNewElementFrame = (Frame)myNewElement;
                            StackLayout myNewElementStackLayout = (StackLayout)myNewElementFrame.Children[0];
                            
                            for (int i = 0; i < myNewElementStackLayout.Children.Count; i++)
                            {
                                var element = myNewElementStackLayout.Children[i];
                                if (element.GetType() == typeof(StackLayout))
                                {
                                    StackLayout elementStackLayout = (StackLayout)element;
                                    foreach (var elementChild in elementStackLayout.Children)
                                    {
                                        VisualElement visualElement = (VisualElement)elementChild;
                                        visualElement.IsEnabled = false;
                                    }
                                }
                            }
                        }

                        stack.Children.Add(myNewElement);
                    }
                }

                stack.Padding = new Thickness(0, 0, 0, 0);
                stack.HorizontalOptions = LayoutOptions.FillAndExpand;
                stack.VerticalOptions = LayoutOptions.FillAndExpand;

                stack.Children.Add(new StackLayout() { HeightRequest = 65 });

                if (isALoogBook)
                {
                    this.ViewSave = false;
                    floatingMenu.IsVisible = false;
                }
                else
                {
                    this.ViewSave = (uiParser.EditableValues.Count > 0);
                    floatingMenu.IsVisible = (uiParser.EditableValues.Count > 0);
                }

                this.Values = uiParser.Values;
                this.m_Panel.Content = stack;

                //if (this.IsActivity){//this.SetupForActivity();}

                if (this.AllowPictures)
                    this.SetupForPictures();

                this.HideSpinner();
                //this.HideStickSpinner();
                addEquipmentButton();
                
                if (belongsToAnEquipment)
                {
                    equipmentVerified.Text = Constants.OkIcon;
                    equipmentVerified.BackgroundColor = Color.Green;
                }
                else if(equipmentVerified != null)
                {
                    equipmentVerified.Text = Constants.CancelIcon;
                    equipmentVerified.BackgroundColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                ShowToast($"Error al cargar atributos: {ex.Message}");
            }

            finally
            {
                this.HideSpinner();
            }

        }

        private void addEquipmentButton()
        {

            var targetAttribute = "Z10T00002.Z001A"; //Object ID
            var targetFound = false;
            var targetIndex = 0;
            targetIndex = 0;

            //check if attribute Z10T00002.Z001A (Object ID) exists. If found, save index
            var firstFrame = (UIFrame)m_UIAttributes[0];
            var attributes = firstFrame.Children;
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

            if (isVerified)
            {
                equipmentVerified.Text = Constants.OkIcon;
                equipmentVerified.BackgroundColor = Color.Green;
                belongsToAnEquipment = true;
            }
            else
            {
                equipmentVerified.Text = Constants.CancelIcon;
                equipmentVerified.BackgroundColor = Color.Red;
                belongsToAnEquipment = false;
            }
            this.verifiedSystemUid = verifiedSystemUid;
        }

        private async void AddEquipment_Clicked(object sender, EventArgs e)
        {
            equipmentVerified.Text = Constants.CancelIcon;
            equipmentVerified.BackgroundColor = Color.Red;
            belongsToAnEquipment = false;
            await PopupNavigation.Instance.PushAsync(new AddEquipment(this));
        }

        //Funcion para extraer información de atributos
        private void processDisplayElement(UIBase myAttribute)
        {
            if (myAttribute.ContainsNestedName("Z10T00002.Z001Q"))      //Si el atributo es una query de Arbol de Fallos, parseo los datos
            {
                this.isPEMActivity = true;
                UIFrame queryFrame = (UIFrame)myAttribute;
                UIQuery arbolFallasQuery = (UIQuery)queryFrame.Children[0];
                CRow[] myRows = arbolFallasQuery.Result.Rows;
                foreach (CRow line in myRows)
                {
                    this.FailureTreeFullLabels.Add(line.Items.ElementAt(0).Text);       //la columna cero es el fullLabel
                    this.FailureTreeDescriptions.Add(line.Items.ElementAt(1).Text);   //la columna uno es la descripción
                }

                parseFailureTreeInfo();
            }
        }

        //BlackList para visualizar atributos. Los atributos que están acá no se visualizan en la pantalla
        private bool isAttributeVisible(UIBase myAttribute)
        {
            if (myAttribute.ContainsNestedName("Z10T00002.Z001Q")) { return false; }
            return true;
        }

        //Funcion para modificar atributos en pantalla
        private View postProcessDisplayElement(View myElement, UIBase myAttribute)
        {
            if (myAttribute.ContainsNestedName("Z10T00002.Z001E"))      //Si llegó el atributo Z001E, entonces interferir con la lista de selección de Z001E, Z002E, Z003E por las listas de partes, elementos y modos
            {
                var i = 0;
                Frame myFrame = (Frame)myElement;
                StackLayout myStack = (StackLayout)myFrame.Content;
                if (myStack.Children.Count > 3)
                {
                    //obtengo picker de parte
                    StackLayout myAttributePart = (StackLayout)myStack.Children[1];
                    XComosMobile.Pages.controls.CustomPicker myCOMOSPickerPart = (XComosMobile.Pages.controls.CustomPicker)myAttributePart.Children[1];
                    //Si ya había una partes cargadas de antes (porque vinieron desde el Platform), las borro
                    int limit = myCOMOSPickerPart.Items.Count - 1;
                    for (i = 0; i < limit; i++)
                    {
                        myCOMOSPickerPart.Items.RemoveAt(1);
                    }
                    //lleno lista de partes
                    for (i = 0; i < this.partes.Count; i++)
                    {
                        myCOMOSPickerPart.Items.Add(this.partes.ElementAt(i));
                    }
                    myCOMOSPickerPart.SelectedIndexChanged += fillElements;

                    //obtengo picker de elemento
                    StackLayout myAttributeElement = (StackLayout)myStack.Children[2];
                    XComosMobile.Pages.controls.CustomPicker myCOMOSPickerElement = (XComosMobile.Pages.controls.CustomPicker)myAttributeElement.Children[1];

                    //Si ya había una elementos cargados de antes (porque vinieron desde el Platform), los borro
                    limit = myCOMOSPickerElement.Items.Count - 1;
                    for (i = 0; i < limit; i++)
                    {
                        myCOMOSPickerElement.Items.RemoveAt(1);
                    }

                    myCOMOSPickerElement.SelectedIndexChanged += fillModes;

                    //obtengo picker de modo
                    StackLayout myAttributeMode = (StackLayout)myStack.Children[3];
                    XComosMobile.Pages.controls.CustomPicker myCOMOSPickerMode = (XComosMobile.Pages.controls.CustomPicker)myAttributeMode.Children[1];

                    //Si ya había modos cargados de antes (porque vinieron desde el Platform), los borro
                    limit = myCOMOSPickerMode.Items.Count - 1;
                    for (i = 0; i < limit; i++)
                    {
                        myCOMOSPickerMode.Items.RemoveAt(1);
                    }

                    this.elementPicker = myCOMOSPickerElement;
                    this.modePicker = myCOMOSPickerMode;

                    return myElement;
                }
            }


            if (myAttribute.ContainsNestedName("Z10T00002.Z020E"))
            {
                //Atributo Usuario de última Modificacion
                Frame myFrame = (Frame)myElement;
                StackLayout myStack = (StackLayout)myFrame.Content;
                StackLayout myAttributeUser = (StackLayout)myStack.Children[1];
                XComosMobile.Pages.controls.CustomEntry myCOMOSEntryUser = (XComosMobile.Pages.controls.CustomEntry)myAttributeUser.Children[1];
                //Label myUser = (Label)myAttributeUser.Children[1];
                //myUser.Text = ProjectData.User;
                myCOMOSEntryUser.Text = ProjectData.User;
            }

            return myElement;
        }

        //funcion que llena los elementos posibles a partir de una parte seleccionada
        //funcion que llena los elementos posibles a partir de una parte seleccionada
        private void fillElements(object sender, EventArgs e)
        {
            int i = 0;
            //Si hay más de 1 item, removerlos (conservar el "null" del principio)
            if (this.elementPicker.Items.Count > 1)
            {
                int limit = this.elementPicker.Items.Count - 1;
                for (i = 0; i < limit; i++)
                {
                    this.elementPicker.Items.RemoveAt(1);
                    if (this.elementosIndex.Count > 0) { this.elementosIndex.RemoveAt(0); }

                }
            }

            //leer la parte seleccionada
            Picker myPartPicker = (Picker)sender;
            int mySelectedIndex = myPartPicker.SelectedIndex - 1;    //-1 porque el primer elemento es el null que ya incluye automaticamente el HTML parser
            if (mySelectedIndex >= 0)
            {
                string mySelectedPartLabel = this.partesLabel.ElementAt(mySelectedIndex);
                //este label debe ser substring de un elemento
                i = 0;
                foreach (string myElementLabel in this.elementosLabel)
                {
                    //si el label de la parte esta contenido en el label del elemento, entonces se debe añadir al picker
                    if (myElementLabel.Contains(mySelectedPartLabel))
                    {
                        this.elementPicker.Items.Add(this.elementos.ElementAt(i));
                        this.elementosIndex.Add(i);
                    }
                    i++;    //index counter
                }
            }

        }

        //funcion que llena los modos posibles a partir de un elemento seleccionado
        private void fillModes(object sender, EventArgs e)
        {
            int i = 0;
            //Si hay más de 1 item, removerlos (conservar el "null" del principio)
            if (this.modePicker.Items.Count > 1)
            {
                int limit = this.modePicker.Items.Count - 1;
                for (i = 0; i < limit; i++)
                {
                    this.modePicker.Items.RemoveAt(1);
                }
            }
            //leer el elemento seleccionado
            Picker myElementPicker = (Picker)sender;
            if (myElementPicker.SelectedIndex > 0)
            {
                int mySelectedIndex = myElementPicker.SelectedIndex - 1;    //-1 porque el primer elemento es el null que ya incluye automaticamente el HTML parser
                int elementListIndex = this.elementosIndex.ElementAt(mySelectedIndex);
                string mySelectedElementLabel = this.elementosLabel.ElementAt(elementListIndex);
                //este label debe ser substring de un elemento
                i = 0;
                foreach (string myModeLabel in this.modosLabel)
                {
                    //si el label de la parte esta contenido en el label del elemento, entonces se debe añadir al picker
                    if (myModeLabel.Contains(mySelectedElementLabel))
                    {
                        this.modePicker.Items.Add(this.modos.ElementAt(i));
                    }
                    i++;    //index counter
                }
            }

        }


        private async Task ExecuteQueriesInAttributes(UIBase[] attributes)
        {
            foreach (var attribute in attributes)
            {
                var uiframe = attribute as ComosWebSDK.UI.UIFrame;
                if (uiframe != null)
                {
                    await ExecuteQueriesInAttributes(uiframe.Children);
                    continue;
                }

                var uiquery = attribute as ComosWebSDK.UI.UIQuery;
                if (uiquery != null)
                {
                    try
                    {
                        uiquery.Result = await m_ComosWeb.GetqueriesResult(
                                     ProjectData.SelectedDB.Key,
                                     ProjectData.SelectedProject.ProjectUID,
                                     ProjectData.SelectedLayer.UID,
                                     ProjectData.SelectedLanguage.LCID,
                                     uiquery.QueryUID,
                                     uiquery.Owner);
                    }
                    catch (TaskCanceledException) { return; } // If there is a Logout Request
                    catch (Exception ex)
                    {
                        await App.Current.MainPage.DisplayAlert("Error", "Error al cargar query: " + ex.Message, Services.TranslateExtension.TranslateText("OK"));
                        return;
                    }

                    continue;
                }
            }
        }

        #endregion

        #region Play/Pause tasks timing

        public bool IsActivity { get; internal set; }

        private void SetupForActivity()
        {
            // Check if activity has the attribute "Z10T00002.Z10A00061" that is fixed to Actual duration value for start stop
            bool showstartplay = false;
            foreach (var ui in m_UIAttributes)
            {
                if (ui.ContainsNestedName("Z10T00002.Z10A00061"))
                {
                    showstartplay = true;
                    DateTime started = cachedb.GetStartDate(this.MainObject.UID, "Z10T00002.Z10A00061", this.MainObject.ProjectUID, this.MainObject.OverlayUID);
                    if (started != DateTime.MaxValue)
                    {
                        // Has been started
                        ButtonPlayPause.Text = "\uf04c"; // Pause icon
                    }
                    break;
                }
            }

            this.ButtonPlayPause.IsVisible = showstartplay;
        }

        private void parseFailureTreeInfo()
        {
            var i = 0;
            string treeRoot = ""; var rootLength = 0;
            //recorro los fullLabels y parseo el árbol
            for (i = 0; i < this.FailureTreeFullLabels.Count; i++)
            {
                if (i == 0) { treeRoot = this.FailureTreeFullLabels.ElementAt(i); rootLength = treeRoot.Length; }
                else
                {
                    string subString = this.FailureTreeFullLabels.ElementAt(i).Substring(rootLength + 1);  //+1 porque luego comienza con un | que quiero descartar
                    string[] splitter = subString.Split('|');   //separo el string en cada | (como los fullLabels de COMOS)

                    //Si tiene 1 elemento, es una parte
                    if (splitter.Count() == 1)
                    {
                        this.partes.Add(this.FailureTreeDescriptions.ElementAt(i).Replace('/', '-'));
                        this.partesLabel.Add(this.FailureTreeFullLabels.ElementAt(i));
                    }
                    //Si tiene 2 elementos, es un elemento
                    if (splitter.Count() == 2)
                    {
                        this.elementos.Add(this.FailureTreeDescriptions.ElementAt(i).Replace('/', '-'));
                        this.elementosLabel.Add(this.FailureTreeFullLabels.ElementAt(i));
                    }
                    //Si tiene 3 elementos, es un modo
                    if (splitter.Count() == 3)
                    {
                        this.modos.Add(this.FailureTreeDescriptions.ElementAt(i).Replace('/', '-'));
                        this.modosLabel.Add(this.FailureTreeFullLabels.ElementAt(i));
                    }

                }
            }
        }



        public async void OnPlayPauseClick(object sender, EventArgs e)
        {
            //Services.XDatabase db = Services.XServices.Instance.GetService<Services.XDatabase>();
            //ViewModels.ProjectData projectdata = Services.XServices.Instance.GetService<ViewModels.ProjectData>();
            //IBRServiceContracts.CWriteValueCollection totalvalues = new IBRServiceContracts.CWriteValueCollection();
            if (ButtonPlayPause.Text == "\uf04b")
            {
                // User Started the task

                ButtonPlayPause.Text = "\uf04c"; // Pause icon
                cachedb.SetStartDate(this.MainObject.UID, "Z10T00002.Z10A00061", this.MainObject.ProjectUID, this.MainObject.OverlayUID, DateTime.Now);
            }
            else
            {
                // user Paused the task.
                ButtonPlayPause.Text = "\uf04b"; // Play icon
                DateTime started = cachedb.GetStartDate(this.MainObject.UID, "Z10T00002.Z10A00061", this.MainObject.ProjectUID, this.MainObject.OverlayUID);
                if (started == DateTime.MaxValue)
                {
                    // something went wrong when saving start date
                    return;
                }

                View view = GetItemForNestedName("Z10T00002.Z10A00061");
                Entry entry = view as Entry;
                if (entry != null)
                {
                    double current = double.Parse(entry.Text);
                    current = Math.Round((current + DateTime.Now.Subtract(started).TotalHours), 3);
                    entry.Text = current.ToString();
                    cachedb.DeleteStartDate(this.MainObject.UID, "Z10T00002.Z10A00061", this.MainObject.ProjectUID, this.MainObject.OverlayUID);
                    await SaveActivityActualTime();
                }
            }
        }

        private View GetItemForNestedName(string nestedname)
        {
            var layout = this.m_Panel.Content as StackLayout;
            return GetItemForNestedName(nestedname, layout);
        }

        private View GetItemForNestedName(string nestedname, StackLayout owner)
        {
            foreach (var child in owner.Children)
            {
                if (child is StackLayout)
                {
                    var item = GetItemForNestedName(nestedname, (StackLayout)child);
                    if (item != null)
                        return item;
                }
                else if (child is Frame)
                {
                    var layout = ((Frame)child).Content as StackLayout;
                    if (layout != null)
                    {
                        var item = GetItemForNestedName(nestedname, layout);
                        if (item != null)
                            return item;
                    }
                }
                else
                {
                    UIBase uibase = child.BindingContext as UIBase;
                    if (uibase != null && uibase.NestedName == nestedname)
                    {
                        return child;
                    }
                }
            }
            return null;
        }

        private async Task<bool> SaveActivityActualTime()
        {
            string user = ProjectData.User;
            string project = ProjectData.SelectedProject.UID;
            string workinglayer = ProjectData.SelectedLayer.UID;
            string language = ProjectData.SelectedLanguage.LCID;

            IBRServiceContracts.CWriteValueCollection collection = new IBRServiceContracts.CWriteValueCollection();
            collection.Values = new IBRServiceContracts.CWriteValue[1];
            collection.Values[0] = Values["Z10T00002.Z10A00061"];

            var db = Services.XServices.Instance.GetService<Services.XDatabase>();
            string servername = db.ReadSetting("ServerNameBR", "http://siemens.southcentralus.cloudapp.azure.com:5109/Service.svc");

            this.ShowSpinner(Services.TranslateExtension.TranslateText("saving"));
            var m_comosbrweb = GetBRComosWeb();

            try
            {
                var result = await m_comosbrweb.WriteComosValues(collection, servername, user, project, workinglayer, language);

                this.HideSpinner();

                if (result)
                {
                    if (MainObject != null)
                        db.ClearSpecFromCache(MainObject.UID, "Z10T00002.Z10A00061", project, workinglayer);
                    ShowToast(Services.TranslateExtension.TranslateText("done"));
                    return true;

                }
                else
                {
                    ShowToast(Services.TranslateExtension.TranslateText("save_failed"));
                    return false;
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                return false;
            }
        }

        #endregion

        #region Upload Picture support
        private static PageAttributes PageInstance = null;
        protected override void OnAppearing()
        {
            PageAttributes.PageInstance = this;
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            // PageAttributes.PageInstance = null;
            base.OnDisappearing();
        }

        public bool AllowPictures { get; internal set; }

        private void SetupForPictures()
        {
            this.ButtonTakePicture.IsVisible = AllowPictures;
        }

        bool IsCamera = false;
        private void Button_Clicked_Photo(object sender, EventArgs e)
        {

            try
            {
                var platform = Services.XServices.Instance.GetService<Services.IPlatformSystem>();
                IsCamera = true;
                platform.CameraMedia("PageAttributes");
            }
            catch (Exception ex)
            {
                ShowToast($"Error multimedia: {ex.Message}");
            }

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

                if (platform.StopRecordingAudio())
                {
                    platform.ShowToast(Services.TranslateExtension.TranslateText("saving"));
                    // save sound path
                    db.CacheSoundFilePath(recordPath, MainObject.UID, projectData.SelectedProject.UID, projectData.SelectedLayer.UID);
                    midiaControl.IncludeAudio(recordPath);
                }

                recordPath = "";
            }
            recording = false;
        }



        public static void SaveImage(string path)
        {
            midiacontrol.SaveImage(path);

            //string date = System.DateTime.Now.ToString();
            //var db = Services.XServices.Instance.GetService<Services.IDatabase>();
            //var projectData = Services.XServices.Instance.GetService<ViewModels.ProjectData>();

            //db.CacheDevicePicture(projectData.SelectedProject.UID, projectData.SelectedLayer.UID, PageAttributes.PageInstance.MainObject.UID, path, date);
        }
        public static void Cameraimage(byte[] imagesource, string path, string date)
        {
            midiacontrol.Cameraimage(imagesource, path, date);

            //var viewcell = new StackLayout()
            //{
            //    Orientation = StackOrientation.Horizontal,
            //    VerticalOptions = LayoutOptions.FillAndExpand,
            //    HorizontalOptions = LayoutOptions.StartAndExpand,
            //    Padding = new Thickness(5, 5),
            //    BackgroundColor = Color.Transparent,
            //};

            //FFImageLoading.Forms.CachedImage img = new FFImageLoading.Forms.CachedImage
            //{
            //    HorizontalOptions = LayoutOptions.FillAndExpand,
            //    VerticalOptions = LayoutOptions.FillAndExpand,
            //    BackgroundColor = Color.Transparent,
            //    Source = ImageSource.FromStream(() => new System.IO.MemoryStream(imagesource)),
            //    WidthRequest = 70,
            //    HeightRequest = 70,
            //};

            //img.Transformations.Add(new FFImageLoading.Transformations.CircleTransformation());

            //viewcell.Children.Add(img);

            // Upload foto

            //var m_Http = Services.XServices.Instance.GetService<HttpClient>();
            //var plataform = Services.XServices.Instance.GetService<Services.IPlatformSystem>();

            //ComosWebSDK.brcomosweb.ComosBRWeb m_comosbrweb = new ComosWebSDK.brcomosweb.ComosBRWeb(m_Http);

            //Stream photo = plataform.PathToStream(path);

            //var photo = await plataform.DownScaleImage(path);
            //ViewModels.ProjectData ProjectData = Services.XServices.Instance.GetService<ViewModels.ProjectData>();
            //Services.XDatabase db = Services.XServices.Instance.GetService<Services.XDatabase>();
            //string filename = "photo_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") +".jpg";
            //var result = await m_comosbrweb.AddPictureToComosObject(
            //    db.ReadSetting("ServerName", ""), 
            //    PageAttributes.PageInstance.MainObject.ProjectUID, 
            //    PageAttributes.PageInstance.MainObject.OverlayUID, 
            //    PageAttributes.PageInstance.MainObject.UID, "description", filename, ProjectData.User, photo);


        }

        #endregion
    }
}