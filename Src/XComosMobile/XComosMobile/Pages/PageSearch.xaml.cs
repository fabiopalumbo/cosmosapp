using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ComosWebSDK.Data;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XComosMobile.Pages.comos;

namespace XComosMobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageSearch : PageTemplate
    {
        ComosWebSDK.IComosWeb m_ComosWeb = Services.XServices.Instance.GetService<ComosWebSDK.IComosWeb>();
        ViewModels.ProjectData m_ProjectData = Services.XServices.Instance.GetService<ViewModels.ProjectData>();
        Services.IPlatformSystem plataform = Services.XServices.Instance.GetService<Services.IPlatformSystem>();
        Services.XDatabase cachedb = Services.XServices.Instance.GetService<Services.XDatabase>();

        Converters.IsComosDocumentUIDToBool docconverter = new Converters.IsComosDocumentUIDToBool();
        Converters.IsComosDeviceUIDToBool devconverter = new Converters.IsComosDeviceUIDToBool();

        public PageSearch()
        {
            try
            {
              InitializeComponent();

              if ((Device.RuntimePlatform == Device.Android) && (searchBarPageSearch.Height < 5))
              {
                //Fixes an android bug where the search bar would be hidden
                searchBarPageSearch.HeightRequest = 40.0;
              }
            }
            catch (Exception ex)
            {
              ShowToast("Error: " + ex.Message);
            }

        }

        private void searchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            //m_Query.FilterVisible = true;
            SearchForItems();
        }

        private async void SearchForItems()
        {

            string tosearch = searchBarPageSearch.Text;

            if (tosearch != null && tosearch != "" && tosearch.Length > 3)
            {
                var m_Http = Services.XServices.Instance.GetService<HttpClient>();
                ComosWebSDK.brcomosweb.ComosBRWeb m_comosbrweb = new ComosWebSDK.brcomosweb.ComosBRWeb(m_Http);
                ViewModels.ProjectData m_ProjectData = Services.XServices.Instance.GetService<ViewModels.ProjectData>();
                var db = Services.XServices.Instance.GetService<Services.XDatabase>();
                string servername = db.ReadSetting("ServerNameBR", "http://siemens.southcentralus.cloudapp.azure.com:5109/Service.svc");

                this.ShowSpinner(Services.TranslateExtension.TranslateText("searching"));

                try
                {

                    CQuerieResult result = null;
                    if (toggletags.IsToggled && !toggledocs.IsToggled)
                    {
                        result = await m_comosbrweb.SearchDevicesByNameAndDescription(servername, m_ProjectData.SelectedProject.UID,
                                                                                                                                                             m_ProjectData.SelectedLayer.UID,
                                                                                                                                                             m_ProjectData.SelectedLanguage.LCID,
                                                                                                                                                             tosearch);

                    }
                    else if (!toggletags.IsToggled && toggledocs.IsToggled)
                    {
                        result = await m_comosbrweb.SearchDocumentsByNameAndDescription(servername, m_ProjectData.SelectedProject.UID,
                                                                                                                                                             m_ProjectData.SelectedLayer.UID,
                                                                                                                                                             m_ProjectData.SelectedLanguage.LCID,
                                                                                                                                                             tosearch);
                    }
                    else if (toggletags.IsToggled && toggledocs.IsToggled)
                    {
                        result = await m_comosbrweb.SearchAllByNameAndDescription(servername, m_ProjectData.SelectedProject.UID,
                                                                                                                                                             m_ProjectData.SelectedLayer.UID,
                                                                                                                                                             m_ProjectData.SelectedLanguage.LCID,
                                                                                                                                                             tosearch);
                    }

                    else if (!toggletags.IsToggled && !toggledocs.IsToggled)
                    {
                        // do nothing
                        result = new CQuerieResult { Rows = new CRow[1], Columns = new CColumn[1] };
                    }

                    if (result == null)
                    {
                        this.ShowToast("Error. Intente nuevamente");
                        return;
                    }

                    m_List.ItemsSource = QueryResultToResultItem(result);


                    //m_Query.Query = result;
                    //await m_Query.ShowListQuery();
                    //m_Query.SetNavigator(m_ProjectData.SelectedDB.Key);

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

        private List<ResultItem> QueryResultToResultItem(CQuerieResult query)
        {
            List<ResultItem> list = new List<ResultItem>();

            for (int i = 0; i < query.Rows.Length; i++)
            {
                list.Add(new ResultItem { DisplayName = query.Rows[i].Items[0].Text, DisplayDescription = query.Rows[i].Items[1].Text, UID = query.Rows[i].Items[0].UID });
            }
            return list;
        }

        private async void m_List_ItemTapped(object sender, EventArgs e)
        {

            StackLayout viewcell = (StackLayout)sender;
            ResultItem item = (ResultItem)viewcell.BindingContext;

            await viewcell.ScaleTo(1.2, 50, Easing.Linear);
            await viewcell.ScaleTo(1, 50, Easing.Linear);


            if ((bool)docconverter.Convert(item.UID, null, null, null))
            {
                if (plataform.IsOnline)
                {
                    plataform.ShowProgressMessage(Services.TranslateExtension.TranslateText("downloading_documents"), true);
                    CheckAndDownloadAditionalContent downloader = new CheckAndDownloadAditionalContent();
                    CDocument filename_andtype = await downloader.DownloadDocument(item.UID, true);

                    if (filename_andtype != null && filename_andtype.FileName != "")
                        cachedb.CacheDocumentFilePath(filename_andtype.FileName, filename_andtype.MimeType, item.UID, m_ProjectData.SelectedProject.UID, m_ProjectData.SelectedLayer.UID, filename_andtype.Name, filename_andtype.Description, filename_andtype.Picture);
                    if (filename_andtype == null)
                    {
                        plataform.ShowToast(Services.TranslateExtension.TranslateText("document_not_found"));
                    }

                    plataform.HideProgressMessage();
                }
            }
            else if ((bool)devconverter.Convert(item.UID, null, null, null))
            {
                CSystemObject sysobj;
                try
                {
                    sysobj = await m_ComosWeb.GetObject(m_ProjectData.SelectedLayer, item.UID, m_ProjectData.SelectedLanguage.LCID);
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Error al buscar: " + ex.Message, Services.TranslateExtension.TranslateText("OK"));
                    return;
                }

                if (sysobj == null)
                    return;

                CObject o = new CObject()
                {
                    ClassType = sysobj.SystemType,
                    Description = sysobj.Description,
                    IsClientPicture = sysobj.IsClientPicture,
                    Name = sysobj.Name,
                    UID = sysobj.UID,
                    OverlayUID = m_ProjectData.SelectedLayer.UID,
                    Picture = sysobj.Picture,
                    ProjectUID = m_ProjectData.SelectedProject.ProjectUID,
                    SystemFullName = sysobj.Name,
                };
                PageSpecifications page = new PageSpecifications(m_ProjectData.SelectedDB.Key, o, m_ProjectData.SelectedLanguage.LCID);
                await this.Navigation.PushAsync(page);
            }


        }

        public class ResultItem
        {
            private const string DOCUMENT = "\uf15b";
            private const string DEVICE = "\uf1b2";
            public string DisplayDescription { get; set; }
            public string DisplayName { get; set; }
            public string UID { get; set; }
            public string DisplayIcon
            {
                get
                {
                    if (this.UID != null && this.UID.Contains(":8:"))
                    {
                        return DEVICE;
                    }
                    else if (this.UID != null && this.UID.Contains(":29:"))
                    {
                        return DOCUMENT;
                    }
                    return "";
                }
            }
        }
    }
}