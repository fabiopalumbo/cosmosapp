using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ComosWebSDK;
using ComosWebSDK.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using ComosWebSDK.Extensions;

namespace ComosWebSDKTest
{
    [TestClass]
    public class ComosWebSDKTests
    {
        string url = "http://siemens.southcentralus.cloudapp.azure.com:5100";
        //string url = "http://localhost:81";

        [TestMethod]
        public void testm()
        {
            string newValue = @"10""";
            newValue.Replace(@"\", @"\\").Replace(@"/", @"\/").Replace('"', '\"');
            
        }


        [TestMethod]
        [Ignore]
        public void ConnectSucess_knownuser()
        {
            ComosHttp m_Http = new ComosHttp();
            ComosWeb cw = new ComosWeb(m_Http, url);
            if (cw.Connect().Result == System.Net.HttpStatusCode.OK)
                return;
            Assert.Fail();
        }

        ComosWeb m_ComosWeb = null;
        private ComosWeb GetComosWeb(bool login = false)
        {
            if (m_ComosWeb != null)
                return m_ComosWeb;
            ComosHttp m_Http = new ComosHttp();
            m_ComosWeb = new ComosWeb(m_Http, url);
            if (m_ComosWeb.Connect("comos","gert.denul","45erYU&*11").Result != System.Net.HttpStatusCode.OK)
                Assert.Fail();
            if (login == true)
            {
                var task = m_ComosWeb.Login();
                task.Wait();
            }
            return m_ComosWeb;
        }

        [TestMethod]
        [Ignore]
        public void TestVersion()
        {
            ComosWeb cw = GetComosWeb();
            var product = cw.GetProductMetaData();
            Assert.IsTrue(string.Compare(product.DisplayName, "Comos") == 0);
            Assert.IsTrue(string.Compare(product.Version, "Version 2.0.1 | © 2017 Siemens AG") == 0);
        }

        [TestMethod]
        [Ignore]
        public void GetDatabases()
        {
            ComosWeb cw = GetComosWeb();
            var task = cw.GetDatabases();
            Console.WriteLine("Databases");
            foreach (var databases in task.Result.Databases)
            {
                Console.WriteLine("\t" + databases.Key + " " + databases.Name);
            }
            Console.WriteLine("Languages");
            foreach (var languages in task.Result.Languages)
            {
                Console.WriteLine("\t" + languages.Key + " " + languages.LCID + " " + languages.Value);
            }
        }

        [TestMethod]
        [Ignore]
        public async Task Login()
        {
            using (ComosWeb cw = GetComosWeb())
            {
                var task = cw.Login();
                Console.WriteLine(string.Format("heartbeat = {0}", task.Result.HeartBeat));
                Console.WriteLine(string.Format("Id = {0}", task.Result.Id));
                Console.WriteLine(string.Format("UserId = {0}", task.Result.UserId));
                var value = await cw.Logout();
            }
        }

        [TestMethod]
        [Ignore]
        public async Task GetAppLicenses()
        {
            using (ComosWeb cw = GetComosWeb(true))
            {
                var task = cw.GetAppLicensesComosDashboard();
                Console.WriteLine(string.Format("DashBoard = {0}", task.Result.IsAppUsageLicensed));
                task = cw.GetAppLicensesComosProjects();
                Console.WriteLine(string.Format("Projects = {0}", task.Result.IsAppUsageLicensed));
                task = cw.GetAppLicensesComosTaskManagement();
                Console.WriteLine(string.Format("TaskManagement = {0}", task.Result.IsAppUsageLicensed));
                task = cw.GetAppLicensesManual();
                Console.WriteLine(string.Format("Manual = {0}", task.Result.IsAppUsageLicensed));
                var value = await cw.Logout();
            }
        }

        [TestMethod]
        [Ignore]
        public async Task GetProjects()
        {
            using (ComosWeb cw = GetComosWeb(true))
            {
                var tdb = cw.GetDatabases();
                var db = tdb.Result.Databases.FirstOrDefault();
                Assert.IsTrue(db != null);
                var tproj = cw.GetProjects(db);
                foreach (var project in tproj.Result)
                {
                    Console.WriteLine(string.Format("ClassType = {0}", project.ClassType));
                    Console.WriteLine(string.Format("Description = {0}", project.Description));
                    Console.WriteLine(string.Format("IsClientPicture = {0}", project.IsClientPicture));
                    Console.WriteLine(string.Format("Name = {0}", project.Name));
                    Console.WriteLine(string.Format("OverlayUID = {0}", project.OverlayUID));
                    Console.WriteLine(string.Format("Picture = {0}", project.Picture));
                    Console.WriteLine(string.Format("ProjectUID = {0}", project.ProjectUID));
                    Console.WriteLine(string.Format("SystemFullName = {0}", project.SystemFullName));
                    Console.WriteLine(string.Format("UID = {0}", project.UID));
                }
                var value = await cw.Logout();
            }
        }

        [TestMethod]
        [Ignore]
        public async Task GetLayers()
        {
            using (ComosWeb cw = GetComosWeb(true))
            {
                var tdb = cw.GetDatabases();
                var db = tdb.Result.Databases.FirstOrDefault();
                Assert.IsTrue(db != null);
                var tproj = cw.GetProjects(db);
                CProject project = null;
                foreach (var proj in tproj.Result)
                {
                    if (proj.Name == "iDB_P01")
                    {
                        project = proj;
                        break;
                    }
                }

                var layers = cw.GetWorkingOverlays(project);
                foreach (var layer in layers.Result)
                {
                    Console.WriteLine(string.Format("ClassType = {0}", layer.ClassType));
                    Console.WriteLine(string.Format("Description = {0}", layer.Description));
                    Console.WriteLine(string.Format("IsClientPicture = {0}", layer.IsClientPicture));
                    Console.WriteLine(string.Format("Name = {0}", layer.Name));
                    Console.WriteLine(string.Format("OverlayUID = {0}", layer.OverlayUID));
                    Console.WriteLine(string.Format("Picture = {0}", layer.Picture));
                    Console.WriteLine(string.Format("ProjectUID = {0}", layer.ProjectUID));
                    Console.WriteLine(string.Format("SystemFullName = {0}", layer.SystemFullName));
                    Console.WriteLine(string.Format("UID = {0}", layer.UID));
                }
                var value = await cw.Logout();
            }
        }

        [TestMethod]
        [Ignore]
        public async Task GetObjectBySystemFullName()
        {
            using (ComosWeb cw = GetComosWeb(true))
            {
                var layer = await cw.GetWorkingLayerByUrl("db1/projects/U:2:A3BQHFA8AR:/wl/U:42:A3WXEAM61M:/details");
                var query = await cw.GetObjectBySystemFullName(layer, "@Q|@M|QTours2");
                Console.WriteLine(string.Format("Query = {0}", query.Name));
                var value = await cw.Logout();
            }
        }

        [TestMethod]
        //[Ignore]
        public async Task GetObjectSpecifications()
        {
            using (ComosWeb cw = GetComosWeb(true))
            {
                var layer = await cw.GetWorkingLayerByUrl("db1/projects/U:2:A3BQHFA8AR:/wl/U:42:A3WXEAM61M:/details");
                var o = await cw.GetObjectBySystemFullName(layer, "A10|A90|A30|A30| » 6/8/2017 09:08:35");
                var specs = await cw.GetObjectSpecification(layer, "1033", o.UID);

                foreach (var spec in specs)
                {
                    var html = await cw.GetObjectSpecificationAsHtml(layer, "1033", o.UID,spec.Name);
                    System.IO.File.WriteAllText("attributes." + spec.Description + ".txt",html);
                }

                var value = await cw.Logout();
            }
        }

        [TestMethod]
        [Ignore]
        public async Task GetObjectWithSpecialUrlChars()
        {
            using (ComosWeb cw = GetComosWeb(true))
            {
                var layer = await cw.GetWorkingLayerByUrl("db1/projects/U:2:A3BQHFA8AR:/wl/U:42:A3WXEAM61M:/details");
                var o = await cw.GetObjectBySystemFullName(layer, "A10|A10|=RO.SEC#7|Sec#7|A20");
                Assert.IsTrue(o != null);
                var value = await cw.Logout();
            }
        }
    }
}
