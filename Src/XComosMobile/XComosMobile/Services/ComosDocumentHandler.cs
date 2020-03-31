using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using XComosMobile.ViewModels;

namespace XComosMobile.Services
{
    public static class ComosDocumentHandler
    {
        public static async void DownloadDocument(ProjectData projectdata, string uid)
        {
            //var m_Http = XComosMobile.Services.XServices.Instance.GetService<HttpClient>();

            ////var http = m_ComosWeb.GetComosHttpClient();            
            //string project = projectdata.SelectedProject.UID;
            //string workinglayer = projectdata.SelectedLayer.UID;
            //string language = projectdata.SelectedLanguage.LCID;

            //var db = Services.XServices.Instance.GetService<Services.XDatabase>();
            //string servername = db.ReadSetting("ServerName", "http://siemens.southcentralus.cloudapp.azure.com");

            //uid = "A3XXX6021Z";
            //project = "iDB_P01";
            //workinglayer = "52";

            //string url = string.Format(servername + "/BRMobile/Download/{0}/{1}/{2}/{3}",
            //    uid, WebUtility.UrlEncode(project), WebUtility.UrlEncode(workinglayer), "false");

            //var response = await m_Http.GetAsync(url);

            //System.IO.Stream st = response.Content.ReadAsStreamAsync().Result;


            /*

                        FileStream fileStream = File.Create(fileFullPath, (int)stream.Length);
            // Initialize the bytes array with the stream length and then fill it with data
            byte[] bytesInStream = new byte[stream.Length];
            stream.Read(bytesInStream, 0, bytesInStream.Length);    
            // Use write method to write to the file specified above
            fileStream.Write(bytesInStream, 0, bytesInStream.Length);

            */


            /*
            var platform = Services.XServices.Instance.GetService<Services.IPlatformSystem>();
            if (response.StatusCode != HttpStatusCode.OK)
            {                
                return false;
            }

            output = await response.Content.ReadAsStringAsync();
            
            IBRServiceContracts.TResult<IBRServiceContracts.CWriteValueResult[]> result =
                    Newtonsoft.Json.JsonConvert.DeserializeObject<
                        IBRServiceContracts.TResult<IBRServiceContracts.CWriteValueResult[]>>(output);
            if (!result.Status)
            {                
                return false;
            }
            */

            //return true;

            Xamarin.Forms.WebView v = new Xamarin.Forms.WebView();
            v.Source = "http://siemens.southcentralus.cloudapp.azure.com/api/webview/v1/dbs/db1/projects/U:2:A3BQHFA8AR:/documents/U:29:A3XXUPEM1Z:?overlayid=U:42:A3Y4157Q7Q:";

            v.Navigated += V_Navigated;

        }

        private static void V_Navigated(object sender, Xamarin.Forms.WebNavigatedEventArgs e)
        {
            
        }
    }
}
