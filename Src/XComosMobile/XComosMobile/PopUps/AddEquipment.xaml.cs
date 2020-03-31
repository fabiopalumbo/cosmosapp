using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XComosMobile.Pages.comos;
using XComosMobile.QR_Scanner;

namespace XComosMobile.PopUps
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddEquipment
    {
        ExistenceVerification existenceVerification;
        AttributesUpdater attributesUpdater;
        public AddEquipment(AttributesUpdater attributesUpdater)
        {
            InitializeComponent();
            cancel.FontFamily = Device.RuntimePlatform == Device.Android ? "FontAwesome" : null;
            qrScan.FontFamily = Device.RuntimePlatform == Device.Android ? "FontAwesome" : null;
            cancel.Text = Constants.CancelIcon;
            qrScan.Text = Constants.QRIcon;

            action.Text = "Verificar existencia";

            addTapGestureRecognizers();

            this.attributesUpdater = attributesUpdater;
        }

        private void addTapGestureRecognizers()
        {
            insertAgain.TextDecorations = TextDecorations.Underline;
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) => {
                action.Text = "Verificar existencia";
                descripcionLayout.IsVisible = false;
                descripcion.Text = "";
                codigoDeEquipo.Text = "";
                descripcion.IsEnabled = true;
                codigoDeEquipo.IsEnabled = true;
                qrScan.IsVisible = true;
                insertAgain.IsVisible = false;
                noVerificationNeeded.IsVisible = true;
            };
            insertAgain.GestureRecognizers.Add(tapGestureRecognizer);

            noVerificationNeeded.TextDecorations = TextDecorations.Underline;
            var tapGestureRecognizer2 = new TapGestureRecognizer();
            tapGestureRecognizer2.Tapped += (s, e) => {
                attributesUpdater.updateEquipmentData(codigoDeEquipo.Text, false, "");
                PopupNavigation.Instance.PopAsync();
            };
            noVerificationNeeded.GestureRecognizers.Add(tapGestureRecognizer2);
        }

        private async void OnScanClicked(object sender, EventArgs e)
        {
            action.Text = "Verificar existencia";
            descripcionLayout.IsVisible = false;
            try
            {
                var scanner = DependencyService.Get<IQrScanningService>();
                var result = await scanner.ScanAsync();
                if (result != null)
                {
                    codigoDeEquipo.Text = result.Trim();
                    ShowToast("Lectura del QR exitosa");
                }
            }
            catch (Exception)
            {
                await App.Current.MainPage.DisplayAlert(Services.TranslateExtension.TranslateText("QR_error"), Services.TranslateExtension.TranslateText("QR_error_message"), Services.TranslateExtension.TranslateText("OK"));
            }
        }

        private async void OnActionClicked(object sender, EventArgs e)
        {
            if(action.Text.Equals("Verificar existencia"))
            {
                try
                {
                    this.ShowSpinner("Verificando");

                    var result = await VerifyEquipmentExistence();
                    if (result)
                    {
                        if (existenceVerification.exists)
                        {
                            this.ShowToast("Verificacion exitosa");
                            action.Text = "OK";
                            descripcion.Text = existenceVerification.description;
                            descripcionLayout.IsVisible = true;
                            descripcion.IsEnabled = false;
                            codigoDeEquipo.IsEnabled = false;
                            qrScan.IsVisible = false;
                            insertAgain.IsVisible = true;
                            descripcionGrid.ColumnDefinitions[0].Width = codigoDeEquipoLabel.Width;
                            noVerificationNeeded.IsVisible = false;
                            //descripcionGrid.ColumnDefinitions[1].Width = descripcionLayout.Width;
                            //descripcion.WidthRequest = descripcionLayout.Width;
                        }
                        else
                        {
                            this.ShowToast($"El equipo de codigo {codigoDeEquipo.Text} no existe");
                        }
                    }
                    else
                    {
                        this.ShowToast("Error al obtener el JSON de verificacion");
                    }

                }
                catch (Exception ex)
                {
                    this.ShowToast($"Error en la comunicación con el servidor. {ex.Message}");
                }
                finally
                {
                    this.HideSpinner();
                }
            }
            else // Ok Button
            {
                attributesUpdater.updateEquipmentData(codigoDeEquipo.Text, true, existenceVerification.systemuid);
                await PopupNavigation.Instance.PopAsync();
            }
        }

        private async Task<bool> VerifyEquipmentExistence()
        {
            var client = new HttpClient();

            client.BaseAddress = new Uri(Constants.URL_VerifyExistence);

            HttpResponseMessage response = await client.GetAsync($"{Constants.Endpoints_ExistenceVerification}?name={codigoDeEquipo}");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                existenceVerification = parseJson(result);
            }
            else
            {
                return false;
            }
            return true;
        }

        private ExistenceVerification parseJson(string json)
        {
            ExistenceVerification existence = new ExistenceVerification();
            try
            {
                existence = JsonConvert.DeserializeObject<ExistenceVerification>(json);
            }
            catch
            {
                this.ShowToast("No se pudo leer el JSON de respuesta de verificacion de equipo");
            }
            return existence;
        }

        private async void OnClose(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
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

    public class ExistenceVerification
    {
        public string description { get; set; }
        public bool exists { get; set; }
        public string name { get; set; }
        public string systemuid { get; set; }
    }
}