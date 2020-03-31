using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ComosWebSDK.Data;
using Newtonsoft.Json.Linq;


namespace ComosWebSDK.brcomosweb
{
    public class ComosBRWeb
    {
        HttpClient m_Http = null;
        //ComosHttp m_Http = null;
        

        public ComosBRWeb(string server,string domain, string user, string pass)
        {
            m_Http = new HttpClient();
            m_Http.Timeout = TimeSpan.FromMinutes(5);
            //GetHttp(server, domain,user,pass);

        }

        public ComosBRWeb(HttpClient http)
        {
            m_Http = http;
           m_Http.Timeout = TimeSpan.FromMinutes(5);
        }

        private async void GetHttp(string server, string domain, string user, string pass)
        {

            //await m_Http.Connect(server, domain, user, pass);

        }
        public async Task<CQuerieResult> SearchDevicesByNameAndDescription(string server,string project, string layer, string lang,string tosearch, string filter = "")
        {
            string url = "";
            
            if(filter == "")
            {
                url = string.Format(server + "/v1/SearchDevicesByNameAndDescription/{0}/{1}/{2}/{3}",
                         WebUtility.UrlEncode(project), WebUtility.UrlEncode(layer), WebUtility.UrlEncode(lang), WebUtility.UrlEncode(tosearch).Replace("+","%20"));
            }
            else
            {
                url = string.Format(server + "/v1/SearchDevicesByNameAndDescriptionWithFilter/{0}/{1}/{2}/{3}/{4}",
                         WebUtility.UrlEncode(project), WebUtility.UrlEncode(layer), WebUtility.UrlEncode(lang), WebUtility.UrlEncode(tosearch).Replace("+", "%20"), WebUtility.UrlEncode(filter));
            }            
            
            var response = await m_Http.GetAsync(url);
            
            var output = await response.Content.ReadAsStringAsync();
            IBRServiceContracts.TResult<CQuerieResult> result =
                    Newtonsoft.Json.JsonConvert.DeserializeObject<
                        IBRServiceContracts.TResult<CQuerieResult>>(output);

            if (result.Status)
            {
                return result.data;
            }
            else
            {
                return null;
            }

        }

        public async Task<CQuerieResult> SearchDocumentsByNameAndDescription(string server, string project, string layer, string lang, string tosearch)
        {
            string url = "";
            
                url = string.Format(server + "/v1/SearchDocumentsByNameAndDescription/{0}/{1}/{2}/{3}",
                         WebUtility.UrlEncode(project), WebUtility.UrlEncode(layer), WebUtility.UrlEncode(lang), WebUtility.UrlEncode(tosearch).Replace("+", "%20"));            

            var response = await m_Http.GetAsync(url);

            var output = await response.Content.ReadAsStringAsync();
            IBRServiceContracts.TResult<CQuerieResult> result =
                    Newtonsoft.Json.JsonConvert.DeserializeObject<
                        IBRServiceContracts.TResult<CQuerieResult>>(output);
            
            if (result.Status)
            {
                return result.data;
            }
            else
            {
                return null;
            }

        }

        public async Task<CQuerieResult> SearchAllByNameAndDescription(string server, string project, string layer, string lang, string tosearch)
        {
            string url = "";
            
                url = string.Format(server + "/v1/SearchAllByNameAndDescription/{0}/{1}/{2}/{3}",
                         WebUtility.UrlEncode(project), WebUtility.UrlEncode(layer), WebUtility.UrlEncode(lang), WebUtility.UrlEncode(tosearch).Replace("+", "%20"));

            var response = await m_Http.GetAsync(url);

            var output = await response.Content.ReadAsStringAsync();
            IBRServiceContracts.TResult<CQuerieResult> result =
                    Newtonsoft.Json.JsonConvert.DeserializeObject<
                        IBRServiceContracts.TResult<CQuerieResult>>(output);

            if (result.Status)
            {
                return result.data;
            }
            else
            {
                return null;
            }

        }

        public async Task<IBRServiceContracts.TResult<string>> CreateComosDevice(IBRServiceContracts.CWriteValueCollection collection, string server, string project, string layer, string lang, string owner, string cdev, string user, string desc)
        {

            string output = Newtonsoft.Json.JsonConvert.SerializeObject(collection);

            StringContent content = new StringContent(output, Encoding.UTF8, "application/json");
            string url = string.Format(server + "/v1/CreateComosDeviceByWebUID/{0}/{1}/{2}/{3}/{4}/{5}/{6}",
                 WebUtility.UrlEncode(project), WebUtility.UrlEncode(layer), lang, WebUtility.UrlEncode(owner), WebUtility.UrlEncode(cdev),user,desc);

            var response = await m_Http.PostAsync(url, content);

            output = await response.Content.ReadAsStringAsync();
            var result =Newtonsoft.Json.JsonConvert.DeserializeObject<IBRServiceContracts.TResult<string>>(output);
          
            return result;

        }

        public async Task<bool> WriteComosValues(IBRServiceContracts.CWriteValueCollection collection,string servername, string user,string project,string workinglayer, string language)
        {
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(collection);
            StringContent content = new StringContent(output, Encoding.UTF8, "application/json");
            string url = string.Format(servername + "/v1/WriteComosValues/{0}/{1}/{2}/{3}",
                user, WebUtility.UrlEncode(project), WebUtility.UrlEncode(workinglayer), language);

            var response = await m_Http.PostAsync(url, content); // Acá hay un error
            if (response.StatusCode != HttpStatusCode.OK)
            {                
                return false;
            }

            output = await response.Content.ReadAsStringAsync();
            IBRServiceContracts.TResult<IBRServiceContracts.CWriteValueResult[]> result =
                    Newtonsoft.Json.JsonConvert.DeserializeObject<
                        IBRServiceContracts.TResult<IBRServiceContracts.CWriteValueResult[]>>(output);
            
            return result.Status;

        }

        public async Task<bool> AddPictureToComosObject(string server, string project, string layer,  string owner, string description, string filename, string user, Stream photo)
        {
            MultipartFormDataContent content = new MultipartFormDataContent();
            content.Headers.Add("user", user);
            content.Headers.Add("projectname", project);
            content.Headers.Add("layer", layer);
            content.Headers.Add("owner", owner);
            content.Headers.Add("filename", filename);
            content.Headers.Add("description", description);
           // content.

            var streamContent = new StreamContent(photo);
            streamContent.Headers.ContentLength = photo.Length;
            streamContent.Headers.ContentDisposition = System.Net.Http.Headers.ContentDispositionHeaderValue.Parse("form-data");
            streamContent.Headers.ContentDisposition.Parameters.Add(new System.Net.Http.Headers.NameValueHeaderValue("name", "contentFile"));
            streamContent.Headers.ContentDisposition.Parameters.Add(new System.Net.Http.Headers.NameValueHeaderValue("filename", "\"" + filename + "\""));

            content.Add(streamContent);
            
            //content.Add(new StreamContent(photo), "\"File\"", $"\"{filename}\"");

            var response = await m_Http.PostAsync(server + "/UploadFile2/"+ filename.Replace(":","_") , content);

            photo.Dispose();

            return response.IsSuccessStatusCode;
        }


        public async Task<bool> AddFileToComosObject(string server, string project, string layer, string owner, string description, string filename, string user, Stream file, string extension)
        {
            MultipartFormDataContent content = new MultipartFormDataContent();
            content.Headers.Add("user", user);
            content.Headers.Add("projectname", project);
            content.Headers.Add("layer", layer);
            content.Headers.Add("owner", owner);
            content.Headers.Add("filename", filename);
            content.Headers.Add("description", description);
            content.Headers.Add("extension", extension);
            

            var streamContent = new StreamContent(file);
            streamContent.Headers.ContentLength = file.Length;
            streamContent.Headers.ContentDisposition = System.Net.Http.Headers.ContentDispositionHeaderValue.Parse("form-data");
            streamContent.Headers.ContentDisposition.Parameters.Add(new System.Net.Http.Headers.NameValueHeaderValue("name", "contentFile"));
            streamContent.Headers.ContentDisposition.Parameters.Add(new System.Net.Http.Headers.NameValueHeaderValue("filename", "\"" + filename + "\""));

            content.Add(streamContent);

            try
            {
                var response = await m_Http.PostAsync(server + "/UploadFile3/" + filename.Replace(":", "_"), content);
                file.Dispose();
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {

                return false;
            }

            
        }

        public async Task<bool> AddRedLineAndAdditionalDocuments(string server, string project, string layer, string owner, string description, string filename, string user, Stream file, string extension)
        {
            MultipartFormDataContent content = new MultipartFormDataContent();
            content.Headers.Add("user", user);
            content.Headers.Add("projectname", project);
            content.Headers.Add("layer", layer);
            content.Headers.Add("owner", owner);
            content.Headers.Add("filename", filename);
            content.Headers.Add("description", description);
            content.Headers.Add("extension", extension);


            var streamContent = new StreamContent(file);
            streamContent.Headers.ContentLength = file.Length;
            streamContent.Headers.ContentDisposition = System.Net.Http.Headers.ContentDispositionHeaderValue.Parse("form-data");
            streamContent.Headers.ContentDisposition.Parameters.Add(new System.Net.Http.Headers.NameValueHeaderValue("name", "contentFile"));
            streamContent.Headers.ContentDisposition.Parameters.Add(new System.Net.Http.Headers.NameValueHeaderValue("filename", "\"" + filename + "\""));

            content.Add(streamContent);

            try
            {
                var response = await m_Http.PostAsync(server + "/UploadRedLinesAddiotionalFiles/" + filename.Replace(":", "_"), content);
                file.Dispose();
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }


        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }

    
}
