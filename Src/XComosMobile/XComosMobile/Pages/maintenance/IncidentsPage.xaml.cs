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
using ModernHttpClient;
using XComosMobile.Pages.comos;

namespace XComosMobile.Pages.maintenance
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IncidentsPage : PageTemplate
    {

        private const string SystemFullNameIncidentObjectsRoot = "@30|M60|A50";
        public ViewModels.Navigator Navigator { get; set; }

        ComosWebSDK.brcomosweb.ComosBRWeb m_comosbrweb = null;
        ViewModels.ProjectData m_ProjectData = Services.XServices.Instance.GetService<ViewModels.ProjectData>();
        private string IncidentCDevUID;

        public IncidentsPage(String IncidentTypeUID)
        {
            InitializeComponent();

            this.Navigator = new ViewModels.Navigator(m_ProjectData.SelectedLayer, m_ProjectData.SelectedLanguage.LCID);
            this.Navigator.Nav = this.NavigationBarControl;
            this.BindingContext = this.Navigator;
            this.IncidentCDevUID = IncidentTypeUID;
            this.Connect();
        }

        private void Connect()
        {

            var m_Http = Services.XServices.Instance.GetService<HttpClient>();
            m_comosbrweb = new ComosWebSDK.brcomosweb.ComosBRWeb(m_Http);

        }

        private async void searchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            string tosearch = searchBar.Text;
            var db = Services.XServices.Instance.GetService<Services.XDatabase>();
            string servername = db.ReadSetting("ServerNameBR", "http://siemens.southcentralus.cloudapp.azure.com:5109/Service.svc");


            if (tosearch != "")
            {

                this.ShowSpinner(Services.TranslateExtension.TranslateText("searching"));

                try
                {
                    CQuerieResult result = await m_comosbrweb.SearchDevicesByNameAndDescription(servername, m_ProjectData.SelectedProject.UID,
                                                                                           m_ProjectData.SelectedLayer.UID,
                                                                                           m_ProjectData.SelectedLanguage.LCID,
                                                                                           tosearch, SystemFullNameIncidentObjectsRoot);

                    /*
                    query.Query = result;
                    await query.ShowListQuery();
                    query.SetNavigator(this.Navigator.Database);
                    query.FilterVisible = true;
                    query.OnCellTaped += Query_OnCellTaped;
                    */

                    if (result == null)
                    {
                        this.ShowToast("Error al buscar la query. Intente nuevamente");
                        return;
                    }

                    m_List.ItemsSource = QueryResultToResultItem(result);

                    string message = Services.TranslateExtension.TranslateText("results_found");
                    message = message.Replace("@", result.Rows.Count().ToString());

                    this.ShowToast(message);

                }
                catch (Exception ex)
                {

                    this.ShowToast(Services.TranslateExtension.TranslateText("search_error") + ex.Message);
                }
                finally
                {
                    this.HideSpinner();
                }

            }
        }

        private async void Query_OnCellTaped(object sender, EventArgs e)
        {
            StackLayout viewcell = (StackLayout)sender;
            ResultItem item = (ResultItem)viewcell.BindingContext;

            await viewcell.ScaleTo(1.2, 50, Easing.Linear);
            await viewcell.ScaleTo(1, 50, Easing.Linear);

            Services.XDatabase db = Services.XServices.Instance.GetService<Services.XDatabase>();
            ComosWebSDK.UI.UICachedScreen screen = db.GetCachedScreen(IncidentCDevUID);
            await this.Navigation.PushAsync(new PageNewDevice(screen, item.UID, "", item.Name));

        }

        private List<ResultItem> QueryResultToResultItem(CQuerieResult query)
        {
            List<ResultItem> list = new List<ResultItem>();
            for (int i = 0; i < query.Rows.Length; i++)
            {
                list.Add(new ResultItem { Name = query.Rows[i].Items[0].Text, UID = query.Rows[i].Items[0].UID, Description = query.Rows[i].Items[1].Text });
            }
            return list;
        }


        public class ResultItem
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string UID { get; set; }

        }

    }
}