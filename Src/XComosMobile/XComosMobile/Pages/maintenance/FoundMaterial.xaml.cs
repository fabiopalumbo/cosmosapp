using System;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Xaml;
using ComosWebSDK.Data;
using XComosMobile.Pages.comos;
using System.Net.Http;
using Newtonsoft.Json;
using XComosMobile.PopUps;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace XComosMobile.Pages.maintenance
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FoundMaterial : PageTemplate
    {
        Material material;
        StockInfo stockInfo;
        CObject workpackage;
        string paniol;
        bool noErrorsOccurred;

        public FoundMaterial(Material material, string paniol)
        {
            InitializeComponent();
            this.BindingContext = this;
            this.btAddNewJDEMaterial.IsVisible = false;
            this.material = material;
            this.paniol = paniol;
            this.noErrorsOccurred = true;
            this.Appearing += FoundMaterialPage_Appearing;
        }

        public FoundMaterial(CObject workpackage, Material material, string paniol)
        {
            InitializeComponent();
            this.BindingContext = this;
            this.workpackage = workpackage;
            this.material = material;
            this.paniol = paniol;
            this.noErrorsOccurred = true;
            this.Appearing += FoundMaterialPage_Appearing;
        }

        private async void FoundMaterialPage_Appearing(object sender, EventArgs e)
        {
            btAddNewJDEMaterial.BackgroundColor = (Color)App.Current.Resources["UIFabButton"];

            try
            {
                itemLargo.Text = material.itemLargo;
                descripcionAmpliada.Text = material.DescripcionAmpliada;
                tipoDeAlmacenamiento.Text = material.TipoAlmacenamiento;
                UoM.Text = material.UOM;
                puntosDeOrden.Text = material.CantidadPuntoOrden;
                puntosDeReorden.Text = material.PuntoReorden;

                this.ShowSpinner("Cargando información de stock...");
                var result = await CheckStockInformation();
                if (result)
                {
                    if (stockInfo != null)
                    {
                        ubicacionPrimaria.Text = stockInfo.ubicacionPrimaria;
                        ubicacionSecundaria.Text = stockInfo.ubicacionSecundaria;
                        cantidadFisica.Text = stockInfo.qfisica.ToString();
                        cantidadDisponible.Text = stockInfo.qdisponible.ToString();
                        noErrorsOccurred = true;
                    }
                    else
                    {
                        this.ShowToast("No se encontró información de stock");
                        noErrorsOccurred = false;
                        btAddNewJDEMaterial.BackgroundColor = Color.Gray;
                    }
                }
                else
                {
                    this.ShowToast("Error al obtener el JSON de stock");
                    noErrorsOccurred = false;
                    btAddNewJDEMaterial.BackgroundColor = Color.Gray;
                }
            }
            catch (Exception ex)
            {
                this.ShowToast($"Error al obtener el JSON de stock: {ex.Message}");
                noErrorsOccurred = false;
                btAddNewJDEMaterial.BackgroundColor = Color.Gray;
            }
            finally
            {
                this.HideSpinner();
            }
        }

        private StockInfo parseJsonToStockMaterial(string json)
        {
            StockInfo stockInfo_ = new StockInfo();
            try
            {
                stockInfo_ = JsonConvert.DeserializeObject<StockInfo>(json);
                noErrorsOccurred = true;
            }
            catch
            {
                this.ShowToast("No se pudo leer el JSON de respuesta de stock");
                noErrorsOccurred = false;
                btAddNewJDEMaterial.BackgroundColor = Color.Gray;
            }
            return stockInfo_;
        }

        public async Task<bool> CheckStockInformation()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(Constants.URL_StockInformation);

            var json = JsonConvert.SerializeObject(new
            {
                itemlargo = material.itemLargo,
                Sucursal = paniol
            });

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(Constants.Endpoints_StockInformation, content);

            if (response.StatusCode == System.Net.HttpStatusCode.OK || response.StatusCode == System.Net.HttpStatusCode.Accepted || response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                var result = await response.Content.ReadAsStringAsync();
                stockInfo = parseJsonToStockMaterial(result);
            }
            else
            {
                return false;
            }
            return true;
        }

        public async void OnAddNewJDEMaterialClick(object sender, EventArgs e)
        {
            if (noErrorsOccurred)
                await PopupNavigation.Instance.PushAsync(new SearchMaterialQuantity(workpackage, material, stockInfo));
            else
                this.ShowToast("No se puede pedir un material porque falta la info de stock");
        }
    }

    public class StockInfo
    {
        public string itemLargo
        {
            get;
            set;
        }
        public string ubicacionPrimaria
        {
            get;
            set;
        }
        public string ubicacionSecundaria
        {
            get;
            set;
        }
        public double qfisica
        {
            get;
            set;
        }
        public double qdisponible
        {
            get;
            set;
        }

    }

}