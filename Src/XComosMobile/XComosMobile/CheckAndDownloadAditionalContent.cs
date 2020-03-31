using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ComosWebSDK;
using ComosWebSDK.Data;
using XComosMobile.Pages.comos;
using XComosMobile.Pages.maintenance;

namespace XComosMobile
{
    public class CheckAndDownloadAditionalContent : IncidentsTypes
    {

        List<string> UIDsToDownload = new List<string>();

        public CheckAndDownloadAditionalContent()
        {
            CheckAndDownloadAditionalContent_Appearing();
        }

        private async void CheckAndDownloadAditionalContent_Appearing()
        {
            var result = await LoadIncidentTypes();
            if (result != false)
            {
                for (int i = 0; i < IncidentTypesCollection.Length; i++)
                {
                    UIDsToDownload.Add(IncidentTypesCollection[i].Row.UID.ToString());
                }
            }

            var result2 = await LogBookPage.LoadLogBookForm();
            if (result2 != false && LogBookPage.logBook != null)
            {
                UIDsToDownload.Add(LogBookPage.logBook.Row.UID.ToString());
            }
            CheckContent();
        }

        private async void CheckContent()
        {
            var db = Services.XServices.Instance.GetService<Services.XDatabase>();
            IComosWeb m_ComosWeb = Services.XServices.Instance.GetService<IComosWeb>();
            ViewModels.ProjectData ProjectData = Services.XServices.Instance.GetService<ViewModels.ProjectData>();


            foreach (string uid in UIDsToDownload)
            {

                //if (db.GetCachedScreen(uid) == null)
                if (1 == 1)
                {
                    List<CSpecification> specs = null;
                    try
                    {
                        specs = await m_ComosWeb.GetObjectSpecification(
                                        ProjectData.SelectedDB.Key, ProjectData.SelectedProject.UID, ProjectData.SelectedLayer.UID, ProjectData.SelectedLanguage.LCID, uid);
                    }
                    catch (TaskCanceledException) { return; } // If there is a Logout Request
                    catch (Exception e)
                    {
                        await App.Current.MainPage.DisplayAlert("Error", "Error al obtener atributos: " + e.Message, Services.TranslateExtension.TranslateText("OK"));
                        return;
                    }

                    if (specs != null)
                    {

                        foreach (var item in specs)
                        {
                            //cache mobile
                            //if (item.Name.Equals("Z10T00002"))
                            if (item.Name.Equals(Pages.comos.Constants.MobileTabName))
                            {
                                string html;
                                try
                                {
                                    html = await m_ComosWeb.GetObjectSpecificationAsHtml(ProjectData.SelectedDB.Key, ProjectData.SelectedProject.UID,
                                    ProjectData.SelectedLayer.UID, ProjectData.SelectedLanguage.LCID, uid, item.Name);
                                }
                                catch (TaskCanceledException) { return; } // If there is a Logout Request
                                catch (Exception ex)
                                {
                                    await App.Current.MainPage.DisplayAlert("Error", "Error al cargar atributos: " + ex.Message, Services.TranslateExtension.TranslateText("OK"));
                                    return;
                                }

                                db.CacheScreen(uid, html);
                            }

                        }
                    }

                }

            }

        }

        public async Task<PageAttributes> DownloadDeviceContent(string UID, bool push, bool onlymobile = true, bool recursive = true)
        {
            PageAttributes mobilepage = null;
            bool download = false;
            IComosWeb m_ComosWeb = Services.XServices.Instance.GetService<IComosWeb>();
            ViewModels.ProjectData projectdata = Services.XServices.Instance.GetService<ViewModels.ProjectData>();
            Services.XDatabase db = Services.XServices.Instance.GetService<Services.XDatabase>();


            List<CSpecification> Specifications;
            try
            {
                Specifications = await m_ComosWeb.GetObjectSpecification(
                                                                        projectdata.SelectedDB.Key,
                                                                        projectdata.SelectedProject.UID,
                                                                        projectdata.SelectedLayer.UID,
                                                                        projectdata.SelectedLanguage.LCID,
                                                                        UID);
            }
            catch (TaskCanceledException) { return null; } // If there is a Logout Request
            catch (Exception e)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Error al obtener atributos: " + e.Message, Services.TranslateExtension.TranslateText("OK"));
                return null;
            }

            if (Specifications == null || Specifications.Count == 0)
                return null;

            foreach (var item in Specifications)
            {
                //TO DO DOWNLOAD EVERYTHING???

                if (item.Name.Equals(Constants.MobileTabName) || !onlymobile)
                {
                    download = true;
                }

                if (download)
                {
                    CSystemObject sysobj;
                    try
                    {
                        sysobj = await m_ComosWeb.GetObject(projectdata.SelectedLayer, UID, projectdata.SelectedLanguage.LCID);
                    }
                    catch (TaskCanceledException) { return null; } // If there is a Logout Request
                    catch (Exception ex)
                    {
                        await App.Current.MainPage.DisplayAlert("Error", "Error al cargar objetos de Comos Web: " + ex.Message, Services.TranslateExtension.TranslateText("OK"));
                        return null;
                    }

                    if (sysobj == null)
                        return null;

                    CObject o = new CObject()
                    {
                        ClassType = sysobj.SystemType,
                        Description = sysobj.Description,
                        IsClientPicture = sysobj.IsClientPicture,
                        Name = sysobj.Name,
                        UID = sysobj.UID,
                        OverlayUID = projectdata.SelectedLayer.UID,
                        Picture = sysobj.Picture,
                        ProjectUID = projectdata.SelectedProject.UID,
                        SystemFullName = sysobj.Name,
                    };

                    //check if its download only

                    PageAttributes page = null;
                    if (!push)
                    {
                        page = new PageAttributes();
                        await page.Init(projectdata.SelectedDB.Key, projectdata.SelectedLanguage.LCID, item, o);
                    }
                    else
                        page = new PageAttributes(projectdata.SelectedDB.Key, projectdata.SelectedLanguage.LCID, item, o);

                    //download all documents if not pushing to page
                    if (!push)
                    {
                        List<string> uids_docs = await page.DownloadDocuments();

                        foreach (var doc in uids_docs)
                        {
                            // string[] filename_mime = await DownloadDocument(doc, false);
                            CDocument filename_mime = await DownloadDocument(doc, false);
                            if (filename_mime != null && filename_mime.FileName != null)
                            {
                                db.CacheDocumentFilePath(filename_mime.FileName, filename_mime.MimeType, doc, projectdata.SelectedProject.UID, projectdata.SelectedLayer.UID, filename_mime.Name, filename_mime.Description, filename_mime.Picture);
                            }
                            else
                            {
                                if (recursive)
                                    await DownloadDeviceContent(doc, false, true, false);
                            }

                        }
                    }

                    if (item.Name.Equals(Constants.MobileTabName))
                    {
                        mobilepage = page;
                    }

                    download = false;
                }
            }

            return mobilepage;
        }

        public async Task<CDocument> DownloadDocument(string UID, bool open)
        {
            IComosWeb m_ComosWeb = Services.XServices.Instance.GetService<ComosWebSDK.IComosWeb>();
            ViewModels.ProjectData ProjectData = Services.XServices.Instance.GetService<ViewModels.ProjectData>();

            // Need to Lock to wait big files (on sleep will logout comosweb)
            m_ComosWeb.Lock();
            CSystemObject o;

            try
            {
                o = await m_ComosWeb.GetObject(
                                                ProjectData.SelectedDB.Key,
                                                ProjectData.SelectedProject.UID,
                                                ProjectData.SelectedLayer.UID,
                                                UID, ProjectData.SelectedLanguage.LCID);
            }
            catch (TaskCanceledException) { return await Task.FromResult<CDocument>(null); } // If there is a Logout Request
            catch (Exception e)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Error al descargar documento: " + e.Message, Services.TranslateExtension.TranslateText("OK"));
                return await Task.FromResult<CDocument>(null);
            }

            if (o == null)
            {
                return await Task.FromResult<CDocument>(null);
            }
            if (o.DocumentType != null)
            {
                bool exportpdf = string.Compare(o.DocumentType.Name, "ComosReport", StringComparison.CurrentCultureIgnoreCase) == 0;
                if (!exportpdf)
                {
                    exportpdf = string.Compare(o.DocumentType.Name, "ComosIReport", StringComparison.CurrentCultureIgnoreCase) == 0;
                }
                // Is a comos report.
                CDocument result = await Task.Run<CDocument>(async () =>
                {
                    HttpResponseMessage response;
                    try
                    {
                        response = await m_ComosWeb.ComosWeb.GetDocumentStream(
                                                    ProjectData.SelectedDB.Key,
                                                    ProjectData.SelectedProject.UID,
                                                    ProjectData.SelectedLayer.UID,
                                                    UID, exportpdf);
                    }
                    catch (TaskCanceledException) { return null; } // If there is a Logout Request
                    catch (Exception ex)
                    {
                        await App.Current.MainPage.DisplayAlert("Error", "Error al cargar documento: " + ex.Message, Services.TranslateExtension.TranslateText("OK"));
                        return null;
                    }

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string filename = response.Content.Headers.ContentDisposition.FileName;
                        System.Diagnostics.Debug.WriteLine(response.Content.Headers.ContentDisposition.DispositionType);
                        filename = filename.Trim(new char[] { '"' });
                        filename = filename.Replace("/", "-");
                        var stream = await response.Content.ReadAsStreamAsync();
                        var platform = Services.XServices.Instance.GetService<Services.IPlatformSystem>();
                        string filename_saved = await platform.SaveAndOpenDocument(filename, stream, response.Content.Headers.ContentType.MediaType, open);
                        return new CDocument()
                        {
                            Description = o.Description,
                            Name = o.Name,
                            UID = o.UID,
                            FileName = filename_saved,
                            Picture = o.Picture,
                            MimeType = response.Content.Headers.ContentType.MediaType
                        };
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        return await Task.FromResult<CDocument>(null);
                    }
                    else
                    {

                        var platform = Services.XServices.Instance.GetService<Services.IPlatformSystem>();
                        platform.ShowToast("Error:" + response.ReasonPhrase);
                        return await Task.FromResult<CDocument>(null);
                        //throw new Exception(response.ReasonPhrase);
                    }
                });

                return result;
            }

            m_ComosWeb.UnLock();

            return await Task.FromResult<CDocument>(null);
        }
    }
}
