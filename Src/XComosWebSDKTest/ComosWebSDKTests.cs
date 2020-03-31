using System;
using NUnit.Framework;
using ComosWebSDK;
using ComosWebSDK.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using ComosWebSDK.Extensions;

namespace XComosWebSDKTest
{
	[TestFixture]
	public class ComosWebSDKTests
	{
		string url = "http://siemens.southcentralus.cloudapp.azure.com:5100";
		//string url = "http://localhost:81/api/webview/v1/";

		[Test]
		[Ignore]
		public void ConnectSucess_knownuser()
		{
			Task.Run(() =>
			{
				ComosHttp m_Http = new ComosHttp();
				ComosWeb cw = new ComosWeb(m_Http, url);
				if (cw.Connect().Result == System.Net.HttpStatusCode.OK)
					return;
				Assert.Fail();
			});

		}

		ComosWeb m_ComosWeb = null;
		private ComosWeb GetComosWeb(bool login = false)
		{
			if (m_ComosWeb != null)
				return m_ComosWeb;
			ComosHttp m_Http = new ComosHttp();
			m_ComosWeb = new ComosWeb(m_Http, url);
			if (m_ComosWeb.Connect("comos", "gert.denul", "45erYU&*11").Result != System.Net.HttpStatusCode.OK)
				Assert.Fail();
			if (login == true)
			{
				var task = m_ComosWeb.Login();
				task.Wait();
			}
			return m_ComosWeb;
		}

		[Test]
		[Ignore]
		public void TestVersion()
		{
			Task.Run(() =>
			{
							/*
							ComosWeb cw = GetComosWeb();
							var product = cw.GetProductMetaData();
							Assert.IsTrue(string.Compare(product.DisplayName, "Comos") == 0);
							Assert.IsTrue(string.Compare(product.Version, "Version 2.0.1 | © 2017 Siemens AG") == 0);
							*/
			}).Wait();

		}


		[Test]
		[Ignore]
		public void GetDatabases()
		{
			Task.Run(() =>
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
			}).Wait();
		}

		[Test]
		[Ignore]
		public void Login()
		{
			Task.Run(async () =>
			{
				using (ComosWeb cw = GetComosWeb())
				{
					var task = cw.Login();
					Console.WriteLine(string.Format("heartbeat = {0}", task.Result.HeartBeat));
					Console.WriteLine(string.Format("Id = {0}", task.Result.Id));
					Console.WriteLine(string.Format("UserId = {0}", task.Result.UserId));
					var value = await cw.Logout();
				}
			}).Wait();
		}

		[Test]
		[Ignore]
		public void GetAppLicenses()
		{
			Task.Run(async () =>
			{
				using (ComosWeb cw = GetComosWeb(true))
				{
								/*
								var task = cw.GetAppLicensesComosDashboard();
								Console.WriteLine(string.Format("DashBoard = {0}", task.Result.IsAppUsageLicensed));
								task = cw.GetAppLicensesComosProjects();
								Console.WriteLine(string.Format("Projects = {0}", task.Result.IsAppUsageLicensed));
								task = cw.GetAppLicensesComosTaskManagement();
								Console.WriteLine(string.Format("TaskManagement = {0}", task.Result.IsAppUsageLicensed));
								task = cw.GetAppLicensesManual();
								Console.WriteLine(string.Format("Manual = {0}", task.Result.IsAppUsageLicensed));
								var value = await cw.Logout();
								*/
				}
			}).Wait();
		}

		[Test]
		[Ignore]
		public void GetProjects()
		{
			Task.Run(async () =>
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
			}).Wait();
		}


		[Test]
		[Ignore]
		public void GetLayers()
		{
			Task.Run(async () =>
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
			}).Wait();
		}

		[Test]
		public void GetObjectBySystemFullName()
		{
			Task.Run(async () =>
			{
				using (ComosWeb cw = GetComosWeb(true))
				{
					/*
					var layer = await cw.GetWorkingLayerByUrl("db1/projects/U:2:A3BQHFA8AR:/wl/U:42:A3WXEAM61M:/details");
					var query = await cw.GetObjectBySystemFullName(layer, "@Q|@M|QTours2");
					Console.WriteLine(string.Format("Query = {0}", query.Name));
					var value = await cw.Logout();
					*/
				}

			}).Wait();
		}
	}
}