using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComosWebSDK;
using ComosWebSDK.Data;
using System.Threading;
using ComosWebSDK.Extensions;
using System.Net;
using System.Net.Http;

namespace ComosWebSDKTest
{
    [TestClass]
    public class ComosBRWebTests
    {

        string url = "http://siemens.southcentralus.cloudapp.azure.com";

        [TestMethod]
        public async Task TestComosWebBRService()
        {
            var clock = new System.Diagnostics.Stopwatch();

            string urltosearch1 = "http://siemens.southcentralus.cloudapp.azure.com/BRMobile/v1/SearchDevicesByNameAndDescription/U%3A2%3AA3BQHFA8AR%3A/U%3A42%3AA3Y4157Q7Q%3A/1046/areia";
            string urltosearch2 = "http://siemens.southcentralus.cloudapp.azure.com/BRMobile/v1/SearchDevicesByNameAndDescription/U%3A2%3AA3BQHFA8AR%3A/U%3A42%3AA3Y4157Q7Q%3A/1046/complexo";
            string urltosearch3 = "http://siemens.southcentralus.cloudapp.azure.com/BRMobile/v1/SearchDevicesByNameAndDescription/U%3A2%3AA3BQHFA8AR%3A/U%3A42%3AA3Y4157Q7Q%3A/1046/bay";
            string urltosearch4 = "http://siemens.southcentralus.cloudapp.azure.com/BRMobile/v1/SearchDevicesByNameAndDescription/U%3A2%3AA3BQHFA8AR%3A/U%3A42%3AA3Y4157Q7Q%3A/1046/linha";

            //ComosWeb cw = GetComosWeb(true);

            //clock.Start();

            /*
            Task[] tasks = new Task[] { cw.ComosHttp.GetAsync(urltosearch1), cw.ComosHttp.GetAsync(urltosearch2),
                                        cw.ComosHttp.GetAsync(urltosearch3),cw.ComosHttp.GetAsync(urltosearch4),
                                        cw.ComosHttp.GetAsync(urltosearch1), cw.ComosHttp.GetAsync(urltosearch2),
                                        cw.ComosHttp.GetAsync(urltosearch3),cw.ComosHttp.GetAsync(urltosearch4),
                                        cw.ComosHttp.GetAsync(urltosearch1), cw.ComosHttp.GetAsync(urltosearch2),
                                        cw.ComosHttp.GetAsync(urltosearch3),cw.ComosHttp.GetAsync(urltosearch4),
                                        cw.ComosHttp.GetAsync(urltosearch1), cw.ComosHttp.GetAsync(urltosearch2),
                                        cw.ComosHttp.GetAsync(urltosearch3),cw.ComosHttp.GetAsync(urltosearch4),
                                        cw.ComosHttp.GetAsync(urltosearch1), cw.ComosHttp.GetAsync(urltosearch2),
                                        cw.ComosHttp.GetAsync(urltosearch3),cw.ComosHttp.GetAsync(urltosearch4),
                                        };            

            await Task.WhenAll(tasks);
            */

            //await cw.ComosHttp.GetAsync(urltosearch2);

            // System.Diagnostics.Debug.WriteLine("TIMER::" + clock.Elapsed.TotalSeconds);

            int count = 10;

            ComosWeb[] cw_user = new ComosWeb[count];
            for (int i = 0; i < count; i++)
                cw_user[i] = GetComosWeb(true);

            clock.Start();


            Task[] tasks = new Task[count];
            for (int i = 0; i < count; i++)
                tasks[i] = cw_user[i].ComosHttp.GetAsync(urltosearch1);
            await Task.WhenAll(tasks);

            //await cw.ComosHttp.GetAsync(urltosearch2);

            System.Diagnostics.Debug.WriteLine("TIMER::" + clock.Elapsed.TotalSeconds);

        }

        ComosWeb m_ComosWeb = null;
        private ComosWeb GetComosWeb(bool login = false)
        {
            if (m_ComosWeb != null)
                return m_ComosWeb;
            ComosHttp m_Http = new ComosHttp();
            m_ComosWeb = new ComosWeb(m_Http, url);
            if (m_ComosWeb.Connect("comos", "comos.web", "C0mos2017").Result != System.Net.HttpStatusCode.OK)
                Assert.Fail();
            if (login == true)
            {
                var task = m_ComosWeb.Login();
                task.Wait();
            }
            return m_ComosWeb;
        }

        #region property argument support
        public TestContext TestContext { get; set; }
        private string GetProperty(string name)
        {
            return TestContext.Properties
                .Cast<KeyValuePair<string, object>>()
                .Where(_ => string.Compare(_.Key,name) == 0)
                .Select(_ => _.Value as string).First();
        }
        #endregion

        [TestMethod]        
        [TestProperty("url", "http://siemens.southcentralus.cloudapp.azure.com/BRMobile/UploadFile/teste.jpg")]
        [TestProperty("filename", @"C:\Temp\20170112_083025.jpg")]
        //[TestProperty("filename", @"C:\Temp\image.jpg")]
        public async Task TestUploadImage()
        {
            var customurl = GetProperty("url");
            var filename = GetProperty("filename");

            ComosWeb comos = GetComosWeb(true);
            
            var m_HttpHandler = new HttpClientHandler() { UseCookies = true, AllowAutoRedirect = true };
            m_HttpHandler.Credentials = new NetworkCredential()
            {
                Domain = "comos",
                Password = "C0mos2017",
                UserName = "caio.barreto"
            };
            var m_Http = new HttpClient(m_HttpHandler);            
            
            //var m_Http = comos.ComosHttp;

            var content = new StreamContent(System.IO.File.OpenRead(filename));
            content.Headers.Add("Content-Type","application/octet-stream");
            content.Headers.Add("user", "caio.barreto");
            content.Headers.Add("projectname", "iDB_P01");
            content.Headers.Add("layer", "12");
            content.Headers.Add("owner", "U:8:A3NYCBBQC9:I");
            
            var response = await m_Http.PostAsync(customurl,content);
            Assert.IsTrue(response.IsSuccessStatusCode);
        }
    }
}
