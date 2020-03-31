using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ComosWebSDK.Data;
using XComosMobile.Pages.comos;
using System.Net.Http;
using Newtonsoft.Json;

namespace XComosMobile.Pages.maintenance
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchMaterial : PageTemplate
    {
        bool isCalledFromWorkpackage;
        CObject workpackage;
        List<Material> foundMaterialsList;
        string NroDeItem;
        string Field1;
        string Field2;
        string Field3;
        string Familia;
        string Subfamilia;
        string Paniol;

        public SearchMaterial(string nroDeItem, string field1, string field2, string field3, string familia, string subfamilia, string paniol)
        {
            InitializeComponent();
            this.BindingContext = this;
            this.isCalledFromWorkpackage = false;
            this.NroDeItem = nroDeItem;
            this.Field1 = field1;
            this.Field2 = field2;
            this.Field3 = field3;
            this.Familia = familia;
            this.Subfamilia = subfamilia;
            this.Paniol = paniol;
            this.Appearing += SearchMaterialPage_AppearingAsync;
        }

        public SearchMaterial(CObject workpackage, string nroDeItem, string field1, string field2, string field3, string familia, string subfamilia, string paniol)
        {
            InitializeComponent();
            this.BindingContext = this;
            this.isCalledFromWorkpackage = true;
            this.workpackage = workpackage;
            this.NroDeItem = nroDeItem;
            this.Field1 = field1;
            this.Field2 = field2;
            this.Field3 = field3;
            this.Familia = familia;
            this.Subfamilia = subfamilia;
            this.Paniol = paniol;
            this.Appearing += SearchMaterialPage_AppearingAsync;
        }

        private List<Material> parseJsonToMaterials(string materialsJson)
        {
            //JArray myItems = JArray.Parse(materialsJson);
            //JArray myItem = JArray.Parse(myItems.ToString());
            List<Material> foundMaterials = new List<Material>();
            try
            {
                foundMaterials = JsonConvert.DeserializeObject<List<Material>>(materialsJson);
            }
            catch
            {
                this.ShowToast("No se pudo leer el JSON de respuesta de búsqueda de materiales");
            }
            //var myMaterial = JsonConvert.DeserializeObject(materialsJson);
            //foundMaterials.Add(myMaterial);
            return foundMaterials;
        }

        private async void SearchMaterialPage_AppearingAsync(object sender, EventArgs e)
        {
            try
            {
                this.ShowSpinner("Cargando ítems");

                var result = await FindMaterials();
                if (result)
                {
                    if (foundMaterialsList.Count != 0)
                    {
                        ListViewFoundMaterials.ItemsSource = foundMaterialsList;
                    }
                    else
                    {
                        this.ShowToast("No se encontraron ítems");
                    }
                }
                else
                {
                    this.ShowToast("Error al obtener el JSON de ítems");
                }

            }
            catch (Exception ex)
            {
                if(ex.InnerException != null)
                    await App.Current.MainPage.DisplayAlert("Error", $"Error en la comunicación con JDE. {ex.Message}. Inner Exception: {ex.InnerException.Message}", Services.TranslateExtension.TranslateText("OK"));
                else
                    await App.Current.MainPage.DisplayAlert("Error", $"Error en la comunicación con JDE. {ex.Message}", Services.TranslateExtension.TranslateText("OK"));
            }
            finally
            {
                this.HideSpinner();
            }
        }

        private async Task<bool> FindMaterials()
        {
            var client = new HttpClient();

            client.BaseAddress = new Uri(Constants.URL_FindMaterials);

            var json = JsonConvert.SerializeObject(new
            {
                nroDeItem = NroDeItem,
                campo1 = Field1,
                campo2 = Field2,
                campo3 = Field3,
                familia = Familia,
                subfamilia = Subfamilia,
                panol = Paniol
            });

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(Constants.Endpoints_FindMaterials, content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                foundMaterialsList = parseJsonToMaterials(result);
            }
            else
            {
                return false;
            }
            return true;
        }

        private async void Material_Clicked(object sender, EventArgs e)
        {
            try
            {
                Frame frame = (Frame)sender;
                var fcontent = frame.Content;
                var myStacklayout = fcontent.GetType();
                if (myStacklayout == typeof(StackLayout))
                {
                    StackLayout fStacklayout = (StackLayout)fcontent;
                    var listChildren = fStacklayout.Children;
                    Grid reqGroup = (Grid)listChildren[0];
                    Material material = (Material)reqGroup.Children[0].BindingContext;

                    if (isCalledFromWorkpackage)
                    {
                        await App.Navigation.PushAsync(new FoundMaterial(workpackage, material, Paniol));
                    }
                    else
                    {
                        await App.Navigation.PushAsync(new FoundMaterial(material, Paniol));
                    }
                }
            }
            catch (Exception ex)
            {
                this.ShowToast($"Error al seleccionar el material. {ex.Message}");
            }

        }
    }


    public class Material
    {
        public string itemLargo
        {
            get;
            set;
        }
        public string DescripcionAmpliada
        {
            get;
            set;
        }
        public string TipoAlmacenamiento
        {
            get;
            set;
        }
        public string UOM
        {
            get;
            set;
        }
        public string CantidadPuntoOrden
        {
            get;
            set;
        }
        public string PuntoReorden
        {
            get;
            set;
        }

    }

}