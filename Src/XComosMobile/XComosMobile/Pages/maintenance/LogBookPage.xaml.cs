using ComosWebSDK;
using ComosWebSDK.brcomosweb;
using ComosWebSDK.Data;
using ComosWebSDK.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XComosMobile.Pages.comos;
using XComosMobile.Services;

namespace XComosMobile.Pages.maintenance
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LogBookPage : PageTemplate
    {
        private bool wasLogBookFormCorrectlyDownload = false;
        public static ViewModel logBook { get; set; }
        private Services.XDatabase db { get { return Services.XServices.Instance.GetService<Services.XDatabase>(); } }

        public LogBookPage()
        {
            InitializeComponent();
            this.Appearing += LogBookPage_Appearing;
        }

        private async void LogBookPage_Appearing(object sender, EventArgs e)
        {
            wasLogBookFormCorrectlyDownload = false;
            logBook = null;
            wasLogBookFormCorrectlyDownload = await LoadLogBookForm();
        }

        async void Create_New_Log_Book_Entry_Clicked(object sender, EventArgs e)
        {
            if(wasLogBookFormCorrectlyDownload)
            {
                this.ShowSpinner("Cargando...");
                try
                {
                    Types mylogBook = new Types
                    {
                        Name = logBook.Row.Items[2].Text,
                        UID = logBook.Row.UID,
                        OwnerUID = logBook.Row.Items[4].Text,
                    };

                    ComosWebSDK.UI.UICachedScreen screen = db.GetCachedScreen(mylogBook.UID);
                    await this.Navigation.PushAsync(new PageNewDevice(screen, mylogBook.OwnerUID, "DUMMY", "Nuevo Parte Diario"));

                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert(Services.TranslateExtension.TranslateText("error"), "Error al abrir creación de parte diario: " + ex.Message, "OK");
                }
                finally
                {
                    this.HideSpinner();
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert(Services.TranslateExtension.TranslateText("error"), "Error al abrir creación de parte diario", "OK");
            }
        }

        public static async Task<bool> LoadLogBookForm()
        {
            CQuerieResult m_QueryResult;
            try
            {
                ViewModels.ProjectData projectdata = Services.XServices.Instance.GetService<ViewModels.ProjectData>();

                IComosWeb m_ComosWeb = Services.XServices.Instance.GetService<IComosWeb>();
                IDatabase db = Services.XServices.Instance.GetService<IDatabase>();
                
                CObject o = db.GetCObjectByFullName(projectdata.SelectedLayer.UID, Constants.QueryLogBookTypesFullName);

                if (o == null)
                {
                    try
                    {
                        o = await m_ComosWeb.GetObjectBySystemFullName(projectdata.SelectedLayer, Constants.QueryLogBookTypesFullName);
                    }
                    catch (TaskCanceledException) { return false; } // If there is a Logout Request
                    catch (Exception ex)
                    {
                        await App.Current.MainPage.DisplayAlert("Error", "Error al cargar query: " + ex.Message, Services.TranslateExtension.TranslateText("OK"));
                        return false;
                    }
                    db.InsertCObject(o, projectdata.SelectedLayer.UID);
                }

                if (o == null)
                {
                    await App.Current.MainPage.DisplayAlert(Services.TranslateExtension.TranslateText("error"), Services.TranslateExtension.TranslateText("no_logbook_types_found"), "OK");
                    return false;
                }

                try
                {
                    m_QueryResult = await m_ComosWeb.GetqueriesResult(
                            projectdata.SelectedDB.Key, o.ProjectUID, o.OverlayUID, projectdata.SelectedLanguage.LCID, o.UID, null);
                }
                catch (TaskCanceledException) { return false; } // If there is a Logout Request
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Error al cargar query: " + ex.Message, Services.TranslateExtension.TranslateText("OK"));
                    return false;
                }

                if (m_QueryResult != null)
                {
                    Converters.IsQueryCachedValue cachehandler = new Converters.IsQueryCachedValue();
                    cachehandler.GetQueryCachedValues(m_QueryResult);

                    if (m_QueryResult.Rows.Length > 0)
                    {
                        logBook = new ViewModel(m_QueryResult.Columns, m_QueryResult.Rows[0]);
                        return true;
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert(Services.TranslateExtension.TranslateText("error"), Services.TranslateExtension.TranslateText("no_logbook_types_found"), "OK");
                        return false;
                    }
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert(Services.TranslateExtension.TranslateText("error"), Services.TranslateExtension.TranslateText("no_logbook_types_found"), "OK");
                    return false;
                }
            }
            catch (Exception e)
            {
                await App.Current.MainPage.DisplayAlert(Services.TranslateExtension.TranslateText("error"), e.Message, "OK");
                return false;
            }
        }

        async void Log_Book_Clicked(object sender, EventArgs e)
        {
            Frame bt = (Frame)sender;
            await FadeOutIn(bt);
            await App.Navigation.PushAsync(new Pages.maintenance.LogBookViewer());
        }

    }
}