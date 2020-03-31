using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ComosWebSDK.Data;
using ComosWebSDK;
using XComosMobile.Pages.comos;
using System.Net.Http;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using XComosMobile.Pages.maintenance;

namespace XComosMobile.PopUps
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchMaterialQuantity
    {
        bool flagdevolucion = false;
        StockInfo materialStock;
        string myTipoAlmacenamiento;
        string uom;
        CObject wp;
        ViewModels.ProjectData ProjectData = Services.XServices.Instance.GetService<ViewModels.ProjectData>();
        Services.XDatabase db = Services.XServices.Instance.GetService<Services.XDatabase>();
        Dictionary<string, IBRServiceContracts.CWriteValue> Values = new Dictionary<string, IBRServiceContracts.CWriteValue>();

        ComosWebSDK.IComosWeb m_ComosWeb
        {
            get
            {
                return Services.XServices.Instance.GetService<ComosWebSDK.IComosWeb>();
            }
        }

        public SearchMaterialQuantity(CObject workpackage, Material material)
        {
            InitializeComponent();
            nroDeMaterial.Text = material.itemLargo;
            this.wp = workpackage;
            cancel.FontFamily = Device.RuntimePlatform == Device.Android ? "FontAwesome" : null;
            cancel.Text = Constants.CancelIcon;
            comprar.Toggled += Comprar_Toggled;
            myTipoAlmacenamiento = material.TipoAlmacenamiento;
        }

        public SearchMaterialQuantity(CObject workpackage, Material material, StockInfo myStockInfo)
        {
            InitializeComponent();
            nroDeMaterial.Text = material.itemLargo;
            this.wp = workpackage;
            cancel.FontFamily = Device.RuntimePlatform == Device.Android ? "FontAwesome" : null;
            cancel.Text = Constants.CancelIcon;
            comprar.Toggled += Comprar_Toggled;
            materialStock = myStockInfo;
            myTipoAlmacenamiento = material.TipoAlmacenamiento;
            uom = material.UOM;
        }

        private async void OnClose(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }

        private async Task<string> GetOTCodeFromCurrentWP()
        {
            try
            {
                var objects = await m_ComosWeb.GetNavigatorNodes_Children(ProjectData.SelectedLayer.Database, ProjectData.SelectedProject.UID, ProjectData.SelectedLayer.UID, ProjectData.SelectedLanguage.LCID, wp.UID, "U");
                CObject[] devices = objects.ToArray();

                foreach (var device in devices)
                {
                    if (device.Description.Equals(Constants.WP_OTCode_Description))
                    {
                        return device.Name;
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Error al obtener numero de OT: " + ex.Message, Services.TranslateExtension.TranslateText("OK"));
                return "";
            }
        }

        private async void OnMaterialRequest(object sender, EventArgs e)
        {
            int intNumber;
            var m_Platform = Services.XServices.Instance.GetService<Services.IPlatformSystem>();

            if (m_Platform.IsDeviceOnline && m_Platform.IsOnline)
            {
                if (string.IsNullOrWhiteSpace(qty.Text) || qty.Text.Equals("0") || qty.Text.Equals("-"))
                {
                    this.ShowToast("La cantidad no puede estar vacía ni ser cero");
                }
                else if ((uom.Equals("U", StringComparison.OrdinalIgnoreCase)
                                || uom.Equals("Unit", StringComparison.OrdinalIgnoreCase)
                                || uom.Equals("Unidades", StringComparison.OrdinalIgnoreCase)
                                || uom.Equals("UN", StringComparison.OrdinalIgnoreCase))
                                && !int.TryParse(qty.Text, out intNumber))
                {
                    this.ShowToast("Como la UoM es Unidades, solo se pueden insertar enteros");
                }
                else
                {
                    var res = await App.Current.MainPage.DisplayAlert("Confirmacion", "Desea confirmar la solicitud?", "Si", "No");

                    if (res)
                    {
                        try
                        {
                            action.IsEnabled = false;
                            this.ShowSpinner("Guardando transacción...");

                            string otCode = await GetOTCodeFromCurrentWP();

                            //Si se va a pedir, chequeo si hay stock disponible (el pedido debe ser menor o igual al disponible)
                            if ((comprar.IsToggled == false) && (materialStock.qdisponible < Convert.ToDouble(qty.Text)))
                            {
                                this.ShowToast("No hay stock suficiente para hacer el pedido");
                            }
                            else
                            {
                                //Reglas para poder pedir o comprar
                                //pedir: P o U
                                //comprar solo P
                                //campo tipo de almacenamiento
                                bool compraAccepted = (comprar.IsToggled == true) && (myTipoAlmacenamiento == "P");
                                bool pedidoAccepted = (comprar.IsToggled == false) && (myTipoAlmacenamiento == "P" || myTipoAlmacenamiento == "U");

                                if (otCode == "")
                                {
                                    this.ShowToast("No se pudo encontrar el código de la OT");
                                }
                                else
                                {
                                    var client = new HttpClient();
                                    client.BaseAddress = new Uri(Constants.URL_MaterialRequest);

                                    var json = JsonConvert.SerializeObject(new
                                    {
                                        OT = otCode,
                                        ItemLargo = nroDeMaterial.Text,
                                        Cantidad = qty.Text,
                                        Usuario = ProjectData.User.ToUpper(),
                                        Marca = comprar.IsToggled ? "1" : "0"
                                    });

                                    var plataform = Services.XServices.Instance.GetService<Services.IPlatformSystem>();

                                    if (plataform.IsOnline)
                                    {

                                        var content = new StringContent(json, Encoding.UTF8, "application/json");
                                        HttpResponseMessage response = await client.PostAsync(Constants.Endpoints_MaterialRequest, content);

                                        if (response.StatusCode == System.Net.HttpStatusCode.OK || response.StatusCode == System.Net.HttpStatusCode.Accepted || response.StatusCode == System.Net.HttpStatusCode.Created)
                                        {
                                            saveTransactionToComos(nroDeMaterial.Text, qty.Text, comprar.IsToggled);
                                            await PopupNavigation.Instance.PopAsync();
                                        }
                                        else
                                        {
                                            this.ShowToast("No se pudo enviar el pedido a JDE por el ERROR: " + response.StatusCode + " - " + response.ReasonPhrase);
                                        }
                                    }
                                    else
                                    {
                                        this.ShowToast("Se debe estar online para hacer el pedido");
                                    }
                                }
                            }


                            action.IsEnabled = true;
                        }
                        catch (Exception ex)
                        {
                            this.ShowToast($"Error al procesar la transacción con JDE. Error {ex.Message}");
                        }
                        finally
                        {
                            this.HideSpinner();
                        }
                    }
                    else
                        await PopupNavigation.Instance.PopAsync();
                }
            }
            else
            {
                this.ShowToast("Debe tener conexion para poder hacer un pedido");
            }

        }

        private async void saveTransactionToComos(string itemLargo, string cantidad, bool aComprar)
        {
            try
            {
                if (Double.Parse(cantidad) > 0)
                {
                    if (aComprar)
                    {
                        CheckValue("Y01.E03", "Compra", "");
                        CheckValue("Y01.E05", "", ""); // Pedido
                        CheckValue("Y01.E06", "", ""); // Entregado
                        CheckValue("Y01.E07", cantidad, ""); // Comprado
                    }
                    else
                    {
                        CheckValue("Y01.E03", "Pedido", "");
                        CheckValue("Y01.E05", cantidad, ""); // Pedido
                        CheckValue("Y01.E06", "", ""); // Entregado
                        CheckValue("Y01.E07", "", ""); // Comprado
                    }
                    CheckValue("Y01.E08", ProjectData.User, ""); // User
                }
                else
                {
                    CheckValue("Y01.E03", "Entrega", "");
                    CheckValue("Y01.E05", cantidad, ""); // Pedido
                    CheckValue("Y01.E06", "", ""); // Entregado
                    CheckValue("Y01.E07", "", ""); // Comprado
                    CheckValue("Y01.E08", ProjectData.User, ""); // User
                }

                CheckValue("Y01.E01", DateTime.Now.ToString("d-MM-yyyy HH:mm:ss"), ""); // Time of day
                CheckValue("Y01.E04", itemLargo, ""); // Material Name

                IBRServiceContracts.CWriteValueCollection collection = new IBRServiceContracts.CWriteValueCollection()
                {
                    Values = Values.Values.ToArray(),
                };

                this.ShowSpinner("Guardando transacción...");

                var m_Http = Services.XServices.Instance.GetService<HttpClient>();
                ComosWebSDK.brcomosweb.ComosBRWeb m_comosbrweb = new ComosWebSDK.brcomosweb.ComosBRWeb(m_Http);

                var result = await m_comosbrweb.CreateComosDevice(
                        collection, db.ReadSetting("ServerNameBR", ""), ProjectData.SelectedProject.UID,
                        ProjectData.SelectedLayer.UID, ProjectData.SelectedLanguage.LCID, wp.UID,
                        "U:13:A4A9QUA65S:I", ProjectData.User, "History Object (WP Transactions)");

                if (!result.Status)
                {
                    this.ShowToast("Error. No se guardó la transacción en COMOS. Contacte al administrador. Referencia: COMOS.IO");
                }
                else
                {
                    if (result.data != "")
                    {
                        this.ShowToast("Transacción guardada");
                        returnToWorkpackagePage();
                    }
                    else
                    {
                        this.ShowToast("Error. No se pudo guardar la transacción correctamente. Contacte al administrador. Referencia: COMOS.IO");
                    }
                }
            }
            catch (Exception ex2)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex2.Message, "OK");
            }
            finally
            {
                this.HideSpinner();
            }
        }

        private async void returnToWorkpackagePage()
        {
            try
            {
                IComosWeb m_ComosWeb = Services.XServices.Instance.GetService<IComosWeb>();
                ViewModels.ProjectData projectdata = Services.XServices.Instance.GetService<ViewModels.ProjectData>();

                var Specifications = await m_ComosWeb.GetObjectSpecification(projectdata.SelectedDB.Key, projectdata.SelectedProject.UID, projectdata.SelectedLayer.UID, projectdata.SelectedLanguage.LCID, wp.UID);

                foreach (var item in Specifications)
                {
                    if (item.Name.Equals(Constants.MobileTabName))
                    {
                        //Get OT Number
                        string myOTNumber = "";
                        var objects = await m_ComosWeb.GetNavigatorNodes_Children(projectdata.SelectedLayer.Database, projectdata.SelectedProject.UID, projectdata.SelectedLayer.UID, projectdata.SelectedLanguage.LCID, this.wp.UID, "U");
                        CObject[] devices = objects.ToArray();
                        foreach (var device in devices)
                        {
                            if (device.Description.Equals(Constants.WP_OTCode_Description))
                            {
                                myOTNumber = device.Name;
                            }
                        }
                        await App.Navigation.PushAsync(new PageAttributes(projectdata.SelectedDB.Key, projectdata.SelectedLanguage.LCID, item, wp, "wp", myOTNumber));
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Error al obtener atributos: " + ex.Message, Services.TranslateExtension.TranslateText("OK"));
            }
        }

        private void Comprar_Toggled(object sender, ToggledEventArgs e)
        {
            var checkbox = (Switch)sender;
            if (checkbox.IsToggled)
            {
                action.Text = "Comprar";
            }
            else if (checkbox.IsVisible)
            {
                action.Text = "Pedir";
            }
        }

        private void OnQuantityChanged(object sender, TextChangedEventArgs e)
        {
            var entry = (Entry)sender;

            if (entry.Text != null)
            {
                if (entry.Text.Contains("-"))
                {
                    comprar.IsVisible = false;
                    comprarLabel.IsVisible = false;
                    action.Text = "Devolver";
                    flagdevolucion = true;
                }
                else if (comprar.IsToggled)
                {
                    comprar.IsVisible = true;
                    comprarLabel.IsVisible = true;
                    action.Text = "Comprar";
                }
                else
                {
                    comprar.IsVisible = true;
                    comprarLabel.IsVisible = true;
                    action.Text = "Pedir";
                }
            }
            else
            {
                comprar.IsVisible = true;
                comprarLabel.IsVisible = true;
                if (comprar.IsToggled)
                {
                    action.Text = "Comprar";
                }
                else
                {
                    action.Text = "Pedir";
                }
            }
        }

        public void CheckValue(string nestedname, string newvalue, string oldvalue, string description = "")
        {
            if (newvalue == null)
                newvalue = "";

            if (oldvalue == null)
                oldvalue = "";

            IBRServiceContracts.CWriteValue value = null;
            if (!Values.TryGetValue(nestedname, out value))
            {
                value = new IBRServiceContracts.CWriteValue()
                {
                    NestedName = nestedname,
                    NewValue = newvalue,
                    OldValue = oldvalue,
                    Description = description,
                    WebSystemUID = "",
                };
                Values.Add(nestedname, value);
            }
            else
            {
                value.NewValue = newvalue;
                value.NestedName = nestedname;
                value.OldValue = oldvalue;
                value.Description = description;
                value.WebSystemUID = "";
            }
        }

        public void ShowSpinner(string message)
        {
            var platform = Services.XServices.Instance.GetService<Services.IPlatformSystem>();
            platform.ShowProgressMessage(message, true);
        }


        public void HideSpinner()
        {
            var platform = Services.XServices.Instance.GetService<Services.IPlatformSystem>();
            platform.HideProgressMessage();
        }

        public void ShowToast(string message)
        {
            var platform = Services.XServices.Instance.GetService<Services.IPlatformSystem>();
            platform.ShowToast(message);
        }

    }

}