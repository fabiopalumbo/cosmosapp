using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ComosWebSDK.Data;
using Mono.Data.Sqlite;
using XComosMobile.Services;

namespace XComosMobile.Droid
{
	public class Database : IDatabase, IDisposable
	{
		SqliteConnection m_DB = null;

		IPlatformSystem m_PlatformSystem = null;

		private const string DB_VERSION = "15";

		#region Database

		public Database()
		{
		}

		public void Dispose()
		{
			this.Close();
		}

		public void Close()
		{
			if (m_DB != null)
			{
				m_DB.Close();
				m_DB.Dispose();
				m_DB = null;
			}
		}

		public IDatabase Open(string filename)
		{
			try
			{
				m_PlatformSystem = XServices.Instance.GetService<IPlatformSystem>();
				string folder = m_PlatformSystem.GetPersonalFolderPath();
				string dbname = System.IO.Path.Combine(folder, filename);
				//m_PlatformSystem.DeleteFile(dbname);
				m_DB = new SqliteConnection("Data Source=" + dbname);
				m_DB.Open();
				if (!m_PlatformSystem.FileExists(dbname))
					throw new Exception("Failed to create database!");

				CreateTablesIfNotExists();
				CheckDBVersion();
				CreateTablesIfNotExists();
				ClearCObjectData();
			}
			catch (Exception e)
			{
				throw e;
			}
			return this;
		}

		private void DropTables(string tablename)
		{
			lock (m_DB)
			{
				SqliteCommand command = m_DB.CreateCommand();
				command.CommandText = "DROP TABLE IF EXISTS " + tablename;
				command.ExecuteNonQuery();
			}
		}

		private void CheckDBVersion()
		{

			List<string> tables = new List<string>();
			tables.Add("DeviceCache");
			tables.Add("SpecCache");
			tables.Add("DocumentCache");
			tables.Add("PhotoCache");
			tables.Add("SoundCache");
			tables.Add("ScreenCache");
			tables.Add("TasksCache");
			tables.Add("CObjectCache");

			try
			{

				string version = ReadSetting("DB_VERSION", "0");

				if (version == DB_VERSION)
				{
					return;
				}
				else
				{

					WriteSetting("DB_VERSION", DB_VERSION);

					foreach (var item in tables)
					{
						DropTables(item);
					}

					m_DB.Close();
					m_DB.Open();

				}
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		private void CreateTablesIfNotExists()
		{
			lock (m_DB)
			{
				// Settings Table
				var command = m_DB.CreateCommand();
				command.CommandText = "CREATE TABLE IF NOT EXISTS Settings (name text PRIMARY KEY, content text, date TIMESTAMP default CURRENT_TIMESTAMP)";
				var result = command.ExecuteNonQuery();
				if (result != 0 && result != -1)
				{
					throw new Exception("Failed creating table SETTINGS");
				}


				// Url Cache Table
				command.CommandText = "CREATE TABLE IF NOT EXISTS Url (name text PRIMARY KEY, content text, date TIMESTAMP default CURRENT_TIMESTAMP)";
				result = command.ExecuteNonQuery();
				if (result != 0 && result != -1)
				{
					throw new Exception("Failed creating table URL");
				}


				// Favorites table
				command.CommandText = "CREATE TABLE IF NOT EXISTS Favorites (name text PRIMARY KEY, content text, date TIMESTAMP default CURRENT_TIMESTAMP)";
				result = command.ExecuteNonQuery();
				if (result != 0 && result != -1)
				{
					throw new Exception("Failed creating table FAVORITES");
				}

				// Tasks Table

				command.CommandText = "CREATE TABLE IF NOT EXISTS TasksCache (uid text PRIMARY KEY)";
				result = command.ExecuteNonQuery();
				if (result != 0 && result != -1)
				{
					throw new Exception("Failed creating table TasksCache");
				}

				//// SpecCache Table         
				//command.CommandText = "DROP TABLE IF EXISTS SpecCache";
				//result = command.ExecuteNonQuery();

				command.CommandText = "CREATE TABLE IF NOT EXISTS SpecCache (specowner text , nestedname text , description text, value text, project text, overlay text, ownername text, ownerdesc text, date TIMESTAMP default CURRENT_TIMESTAMP, PRIMARY KEY(specowner,nestedname,description))";
				result = command.ExecuteNonQuery();
				if (result != 0 && result != -1)
				{
					throw new Exception("Failed creating table SpecCache");
				}

				// DeviceCache Table                

				//command.CommandText = "DROP TABLE IF EXISTS DeviceCache";
				//result = command.ExecuteNonQuery();

				command.CommandText = "CREATE TABLE IF NOT EXISTS DeviceCache (tempuid text PRIMARY KEY, owner text , specs text , cdev text, project text, overlay text, ownername text,ownerpicture text, description text, date TIMESTAMP default CURRENT_TIMESTAMP)";
				result = command.ExecuteNonQuery();
				if (result != 0 && result != -1)
				{
					throw new Exception("Failed creating table DeviceCache");
				}

				command.CommandText = "CREATE TABLE IF NOT EXISTS Activities (specowner text , nestedname text , datestarted text, project text, overlay text, date TIMESTAMP default CURRENT_TIMESTAMP, PRIMARY KEY(specowner,nestedname))";
				result = command.ExecuteNonQuery();
				if (result != 0 && result != -1)
				{
					throw new Exception("Failed creating table Activities");
				}

				// ScreenCache Table                
				command.CommandText = "CREATE TABLE IF NOT EXISTS ScreenCache (uid text PRIMARY KEY, content text , date TIMESTAMP default CURRENT_TIMESTAMP)";
				result = command.ExecuteNonQuery();
				if (result != 0 && result != -1)
				{
					throw new Exception("Failed creating table ScreenCache");
				}

				// ScreenCache Table                
				command.CommandText = "CREATE TABLE IF NOT EXISTS CObjectCache (uid text, name text, description text, fullname text, layer text, projectuid text, overlayuid text, classtype text, picture text, isclientpicture text, PRIMARY KEY(uid,layer))";
				result = command.ExecuteNonQuery();
				if (result != 0 && result != -1)
				{
					throw new Exception("Failed creating table CObjectCache");
				}

				// DocumentCache Table                                
				//command.CommandText = "SELECT * FROM DocumentCache";
				//var reader = command.ExecuteReader();
				//while (reader.Read())
				//{
				//    for(int i = 0; i < reader.FieldCount; i++)
				//        System.Diagnostics.Debug.Write(reader.GetString(i)+"\t");
				//    System.Diagnostics.Debug.WriteLine("--------------------------------------------------");
				//}
				//reader.Close();


				//command.CommandText = "DROP TABLE IF EXISTS PhotoCache";
				//result = command.ExecuteNonQuery();


				command.CommandText = "CREATE TABLE IF NOT EXISTS DocumentCache (uid text PRIMARY KEY, path text, doctype text , date TIMESTAMP default CURRENT_TIMESTAMP, project text, overlay text, name text, description text, picture text)";
				result = command.ExecuteNonQuery();
				if (result != 0 && result != -1)
				{
					throw new Exception("Failed creating table DocumentCache");
				}
				
				command.CommandText = "CREATE TABLE IF NOT EXISTS PhotoCache (path text PRIMARY KEY, devuid text, photo_name text , project text, overlay text, sent text, owner_name text)";
				result = command.ExecuteNonQuery();
				if (result != 0 && result != -1)
				{
					throw new Exception("Failed creating table PhotoCache");
				}

				command.CommandText = "CREATE TABLE IF NOT EXISTS SoundCache (path text PRIMARY KEY, devuid text, audio_name text , project text, overlay text, sent text, owner_name text)";
				result = command.ExecuteNonQuery();
				if (result != 0 && result != -1)
				{
					throw new Exception("Failed creating table SoundCache");
				}

			}
		}

		#endregion

		#region Specifications

		public IBRServiceContracts.CWriteValueCollection GetAllCachedSpecsFromOwner(string uid, string project, string overlay)
		{

			List<IBRServiceContracts.CWriteValue> list = new List<IBRServiceContracts.CWriteValue>();
			IBRServiceContracts.CWriteValueCollection values = new IBRServiceContracts.CWriteValueCollection();

			try
			{
				lock (m_DB)
				{
					SqliteCommand command = m_DB.CreateCommand();
					command.CommandText = string.Format("SELECT nestedname, value, description FROM SpecCache where (specowner = '{0}' and project = '{1}' and overlay = '{2}')", uid, project, overlay);

					var reader = command.ExecuteReader();
					while (reader.Read())
					{

						IBRServiceContracts.CWriteValue obj = new IBRServiceContracts.CWriteValue
						{
							NestedName = reader.GetString(0),
							WebSystemUID = uid,
							NewValue = reader.GetString(1),
							Description = reader.GetString(2)
						};

						list.Add(obj);

					}

				}
			}
			catch (Exception ex)
			{
				//throw ex;
			}

			values.Values = list.ToArray();

			return values;
		}

		public void ClearSpecFromCache(string specowner, string nestedname, string project, string wo)
		{
			try
			{
				lock (m_DB)
				{
					using (SqliteCommand command = m_DB.CreateCommand())
					{
						string query = string.Format(
							"DELETE from SpecCache where (specowner = '{0}' and nestedname = '{1}' and project = '{2}' and overlay = '{3}')",
							specowner, nestedname, project, wo);

						//VALUES  ('{0}','{1}','{2}','{3}','{4}','{5}')", specowner, nestedname, value, project, wo, description);
						command.CommandText = query;
						var result = command.ExecuteNonQuery();
						if (result != 1)
						{
							throw new Exception("Failed WriteCacheSpecValue!!");
						}
					}
				}
			}
			catch (Exception e)
			{
				//throw e;
			}
		}

		public void ClearAllSpecsFromOwner(string uid, string project, string overlay)
		{
			try
			{
				lock (m_DB)
				{
					using (SqliteCommand command = m_DB.CreateCommand())
					{
						string query = string.Format(
							"DELETE from SpecCache where (specowner = '{0}' and project = '{1}' and overlay = '{2}')", uid, project, overlay);

						command.CommandText = query;
						var result = command.ExecuteNonQuery();
					}
				}
			}
			catch (Exception e)
			{
				//throw e;
			}
		}

		public void WriteCacheSpecValue(string specowner, string nestedname, string project, string wo, string value, string description = "", string ownername = "", string ownerdesc = "")
		{

			try
			{
				lock (m_DB)
				{
					using (SqliteCommand command = m_DB.CreateCommand())
					{
						string query = string.Format(
							"INSERT or REPLACE INTO SpecCache (specowner,nestedname, value, project,overlay, description, ownername, ownerdesc ) VALUES  ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')", specowner, nestedname, value, project, wo, description, ownername, ownerdesc);
						command.CommandText = query;
						var result = command.ExecuteNonQuery();
						if (result != 1)
						{
							throw new Exception("Failed WriteCacheSpecValue!!");
						}
					}
				}
			}
			catch (Exception e)
			{
				throw e;
			}

		}

		public string ReadCacheSpecValue(string specowner, string nestedname, string project, string wo, string description = "")
		{

			try
			{
				lock (m_DB)
				{
					using (SqliteCommand command = m_DB.CreateCommand())
					{
						if (description == "")
						{
							command.CommandText = string.Format(
								"SELECT value FROM SpecCache WHERE specowner = '{0}' and nestedname = '{1}' and project = '{2}' and overlay = '{3}'", specowner, nestedname, project, wo);
						}
						else
						{
							command.CommandText = string.Format(
								"SELECT value FROM SpecCache WHERE specowner = '{0}' and description = '{1}' and project = '{2}' and overlay = '{3}'", specowner, description, project, wo);
						}


						object ovalue = command.ExecuteScalar();
						if (ovalue == null)
							return "";
						string value = ovalue as string;
						if (string.IsNullOrEmpty(value))
							return "";
						return value;
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		#endregion

		#region Screen

		public ComosWebSDK.UI.UICachedScreen GetCachedScreen(string systemuid)
		{

			try
			{
				lock (m_DB)
				{
					using (SqliteCommand command = m_DB.CreateCommand())
					{
						command.CommandText = string.Format(
							"SELECT content FROM ScreenCache WHERE uid = '{0}'", systemuid);
						object ovalue = command.ExecuteScalar();
						if (ovalue == null)
							return null;
						string value = ovalue as string;
						if (string.IsNullOrEmpty(value))
							return null;
						return new ComosWebSDK.UI.UICachedScreen { UID = systemuid, JSONContent = value.Replace(@"\@", "'") };
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void CacheScreen(string systemuid, string jsoncontent)
		{
			//ScreenCache

			try
			{
				lock (m_DB)
				{
					using (SqliteCommand command = m_DB.CreateCommand())
					{
						string query = string.Format(
							"INSERT or REPLACE INTO ScreenCache (uid,content) VALUES  ('{0}','{1}')", systemuid, jsoncontent.Replace("'", @"\@"));
						command.CommandText = query;
						var result = command.ExecuteNonQuery();
						if (result != 1)
						{
							throw new Exception("Failed CacheScreen!!");
						}
					}
				}
			}
			catch (Exception e)
			{
				//throw e;
			}

		}

		#endregion

		#region Device

		public void DeleteCachedDevice(string id)
		{
			try
			{
				lock (m_DB)
				{
					using (SqliteCommand command = m_DB.CreateCommand())
					{
						command.CommandText = string.Format(
								"DELETE FROM DeviceCache WHERE tempuid = '{0}'", id);
						command.ExecuteScalar();
					}
				}
			}
			catch (Exception ex)
			{
				//throw ex;
			}
		}

		public List<ComosWebSDK.Data.CCachedDevice> GetCachedDevices()
		{

			List<ComosWebSDK.Data.CCachedDevice> list = new List<ComosWebSDK.Data.CCachedDevice>();
			try
			{
				lock (m_DB)
				{
					SqliteCommand command = m_DB.CreateCommand();
					command.CommandText = "SELECT * FROM DeviceCache";

					var reader = command.ExecuteReader();
					while (reader.Read())
					{

						ComosWebSDK.Data.CCachedDevice obj = new ComosWebSDK.Data.CCachedDevice
						{
							OwnerUID = reader.GetString(1),
							ProjectUID = reader.GetString(4),
							OverlayUID = reader.GetString(5),
							OwnerName = reader.GetString(6),
							OwnerPicture = reader.GetString(7),
							Description = reader.GetString(8),
							CDevUID = reader.GetString(3),
							Values = reader.GetString(2),
							TempUID = reader.GetString(0)

						};

						list.Add(obj);

					}

				}
			}
			catch (Exception ex)
			{
				//throw ex;
			}
			return list;

		}

		public void CacheDeviceToCreate(string project, string overlay, string owner, string cdev, string values, string tempuid, string ownername, string ownerpicture, string description)
		{
			try
			{
				lock (m_DB)
				{
					using (SqliteCommand command = m_DB.CreateCommand())
					{


						string query = string.Format(
								"INSERT or REPLACE INTO DeviceCache (tempuid,owner, specs, cdev, project,overlay,ownername,ownerpicture,description ) VALUES  ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')", tempuid, owner, values, cdev, project, overlay, ownername, ownerpicture, description);
						command.CommandText = query;
						var result = command.ExecuteNonQuery();
						if (result != 1)
						{
							throw new Exception("Failed CacheDeviceToCreate!!");
						}
					}
				}
			}
			catch (Exception e)
			{
				//throw e;
			}


		}

		public List<ComosWebSDK.Data.CObject> GetDevicesWithCachedAttributes(string project, string wo)
		{
			List<ComosWebSDK.Data.CObject> list = new List<ComosWebSDK.Data.CObject>();

			try
			{
				lock (m_DB)
				{
					using (SqliteCommand command = m_DB.CreateCommand())
					{
						command.CommandText = string.Format(
							"SELECT distinct ownername, ownerdesc,specowner FROM SpecCache WHERE project = '{0}' and overlay = '{1}'", project, wo);
						var reader = command.ExecuteReader();
						while (reader.Read())
						{
							string ownername = reader.GetString(0);
							string ownerdesc = reader.GetString(1);
							string owneruid = reader.GetString(2);

							list.Add(new ComosWebSDK.Data.CObject()
							{
								Name = ownername,
								Description = ownerdesc,
								UID = owneruid
							});
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return list;
		}

		public List<ComosWebSDK.Data.CObject> GetDevicesWithCachedPictures(string project, string wo)
		{
			List<ComosWebSDK.Data.CObject> list = new List<ComosWebSDK.Data.CObject>();

			try
			{
				lock (m_DB)
				{
					using (SqliteCommand command = m_DB.CreateCommand())
					{
						command.CommandText = string.Format(
							"SELECT distinct devuid, owner_name FROM PhotoCache WHERE project = '{0}' and overlay = '{1}' and sent = '0'", project, wo);
						var reader = command.ExecuteReader();
						while (reader.Read())
						{

							list.Add(new ComosWebSDK.Data.CObject()
							{
								Name = reader.GetString(1),
								Description = "Photo",
								UID = reader.GetString(0)
							});
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return list;
		}

		public void CacheDevicePicture(string project, string overlay, string devuid, string path, string date, string owner)
		{

			try
			{
				lock (m_DB)
				{
					using (SqliteCommand command = m_DB.CreateCommand())
					{
						string query = string.Format(
							"INSERT or REPLACE INTO PhotoCache (photo_name,path, devuid, project,overlay, sent, owner_name ) VALUES  ('{0}','{1}','{2}','{3}', '{4}', '0','{5}')", date, path, devuid, project, overlay, owner);
						command.CommandText = query;
						var result = command.ExecuteNonQuery();
						if (result != 1)
						{
							throw new Exception("Failed CacheDevicePicture!!");
						}
					}
				}
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		#endregion

		#region Favorites

		public List<string> GetFavorites()
		{

			List<string> list = new List<string>();
			try
			{
				lock (m_DB)
				{
					SqliteCommand command = m_DB.CreateCommand();
					command.CommandText = "SELECT content FROM Favorites";

					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						list.Add(reader.GetString(0));
					}

				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return list;
		}

		public void RemoveFavorite(string name)
		{
			try
			{
				lock (m_DB)
				{
					SqliteCommand command = m_DB.CreateCommand();
					command.CommandText = string.Format(
						"DELETE FROM Favorites WHERE name = '{0}'", name);
					command.ExecuteScalar();
				}
			}
			catch (Exception ex)
			{
				//throw ex;
			}
		}

		public void SetFavorite(string name, string value)
		{
			try
			{
				lock (m_DB)
				{
					using (SqliteCommand command = m_DB.CreateCommand())
					{
						string query = string.Format(
							"INSERT or REPLACE INTO Favorites (name, content ) VALUES  ('{0}','{1}')", name, value);
						command.CommandText = query;
						var result = command.ExecuteNonQuery();
						if (result != 1)
						{
							throw new Exception("Failed WriteSettings!!");
						}
					}
				}
			}
			catch (Exception e)
			{
				//throw e;
			}
		}

		public bool IsFavorite(string name)
		{
			try
			{
				lock (m_DB)
				{
					SqliteCommand command = m_DB.CreateCommand();
					command.CommandText = string.Format(
						"SELECT content FROM Favorites WHERE name = '{0}'", name);
					object ovalue = command.ExecuteScalar();
					if (ovalue == null)
					{
						return false;
					}
					else
					{
						return true;
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		#endregion

		#region Setting

		public void ClearSetting(string name)
		{
			try
			{
				lock (m_DB)
				{
					SqliteCommand command = m_DB.CreateCommand();
					command.CommandText = string.Format(
						"DELETE content FROM Settings WHERE name = '{0}'", name);
					command.ExecuteScalar();
				}
			}
			catch (Exception ex)
			{
				//throw ex;
			}
		}

		public void WriteSetting(string name, string value)
		{
			try
			{
				lock (m_DB)
				{
					using (SqliteCommand command = m_DB.CreateCommand())
					{
						string query = string.Format(
							"INSERT or REPLACE INTO Settings (name, content ) VALUES  ('{0}','{1}')", name, value);
						command.CommandText = query;
						var result = command.ExecuteNonQuery();
						if (result != 1)
						{
							throw new Exception("Failed WriteSettings!!");
						}
					}
				}
			}
			catch (Exception e)
			{
				//throw e;
			}
		}

		public string ReadSetting(string name, string defaultvalue)
		{
			try
			{
				lock (m_DB)
				{
					SqliteCommand command = m_DB.CreateCommand();
					command.CommandText = string.Format(
						"SELECT content FROM Settings WHERE name = '{0}'", name);
					object ovalue = command.ExecuteScalar();
					if (ovalue == null)
						return defaultvalue;
					string value = ovalue as string;
					if (string.IsNullOrEmpty(value))
						return defaultvalue;
					return value;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		#endregion

		#region CObject

		public void ClearCObjectData()
		{
			try
			{
				lock (m_DB)
				{
					using (SqliteCommand command = m_DB.CreateCommand())
					{
						string query = $"DELETE FROM CObjectCache";
						command.CommandText = query;
						var result = command.ExecuteNonQuery();
					}
				}
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public void InsertCObject(CObject item, string layer)
		{
			try
			{
				lock (m_DB)
				{
					using (SqliteCommand command = m_DB.CreateCommand())
					{
						string query = 
							$"INSERT or REPLACE INTO CObjectCache (uid, name, description, fullname, layer, projectuid, classtype, picture, isclientpicture, overlayuid) VALUES  ('{item.UID}','{item.Name}','{item.Description}','{item.SystemFullName}', '{layer}','{item.ProjectUID}','{item.ClassType}','{item.Picture}','{item.IsClientPicture}', '{item.OverlayUID}')";
						command.CommandText = query;
						var result = command.ExecuteNonQuery();
						if (result != 1)
						{
							throw new Exception("Failed InsertCObject!!");
						}
					}
				}
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public CObject GetCObjectByFullName(string layer, string fullname)
		{
			CObject result = null;

			try
			{
				lock (m_DB)
				{
					using (SqliteCommand command = m_DB.CreateCommand())
					{
						command.CommandText = $"SELECT uid, name, description, fullname, projectuid, classtype, picture, isclientpicture, overlayuid FROM CObjectCache WHERE fullname = '{fullname}' and layer = '{layer}'";
						var reader = command.ExecuteReader();
						if (reader.Read())
						{
							result = new CObject()
							{
								UID = reader.GetString(0),
								Name = reader.GetString(1),
								Description = reader.GetString(2),
								SystemFullName = reader.GetString(3),
								ProjectUID = reader.GetString(4),
								ClassType = reader.GetString(5),
								Picture = reader.GetString(6),
								IsClientPicture = Convert.ToBoolean(reader.GetString(7)),
								OverlayUID = reader.GetString(8)
							};
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return result;
		}


		#endregion

		#region Url

		public void WriteUrlCache(string url, string response)
		{
			try
			{
				lock (m_DB)
				{
					using (SqliteCommand command = m_DB.CreateCommand())
					{
						string query = string.Format(
							"INSERT or REPLACE INTO Url (name, content ) VALUES  ('{0}','{1}')", url, response.Replace("'", "''"));
						command.CommandText = query;
						var result = command.ExecuteNonQuery();
						if (result != 1)
						{
							throw new Exception("Failed WriteSettings!!");
						}
					}
				}
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public string ReadUrlCache(string url)
		{
			try
			{
				lock (m_DB)
				{
					SqliteCommand command = m_DB.CreateCommand();
					command.CommandText = string.Format(
						"SELECT content FROM Url WHERE name = '{0}'", url);
					object ovalue = command.ExecuteScalar();
					if (ovalue == null)
						return null;
					string value = ovalue as string;
					if (string.IsNullOrEmpty(value))
						return null;
					return value;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void ClearUrlCache()
		{
			try
			{
				lock (m_DB)
				{
					SqliteCommand command = m_DB.CreateCommand();
					command.CommandText = string.Format("DELETE * FROM Url");
					int result = command.ExecuteNonQuery();
					if (result != 0)
						throw new Exception("Failed to clear Url table");
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		#endregion
		
		#region Task

		public void MarkTaskAsRead(string uid)
		{
			try
			{
				lock (m_DB)
				{
					using (SqliteCommand command = m_DB.CreateCommand())
					{
						string query = string.Format(
							"INSERT or REPLACE INTO TasksCache (uid ) VALUES  ('{0}')", uid);
						command.CommandText = query;
						var result = command.ExecuteNonQuery();
						if (result != 1)
						{
							throw new Exception("Failed MarkTaskAsRead!!");
						}
					}
				}
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public bool IsTaskNew(string uid)
		{

			try
			{
				lock (m_DB)
				{
					SqliteCommand command = m_DB.CreateCommand();
					command.CommandText = string.Format(
						"SELECT uid FROM TasksCache WHERE uid = '{0}'", uid);
					object ovalue = command.ExecuteScalar();
					if (ovalue == null)
					{
						return true;
					}
					else
					{
						return false;
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		#endregion

		#region Document

		public string[] GetDocumentFilePath(string uid, string project, string wo)
		{
			try
			{
				lock (m_DB)
				{
					using (SqliteCommand command = m_DB.CreateCommand())
					{
						command.CommandText = string.Format(
							"SELECT path, doctype FROM DocumentCache WHERE uid = '{0}' and project = '{1}' and overlay = '{2}'", uid, project, wo);
						var reader = command.ExecuteReader();
						while (reader.Read())
						{
							string path = reader.GetString(0);
							if (path == null)
								return null;
							string doctype = reader.GetString(1);
							if (string.IsNullOrEmpty(path))
								return null;
							return new[] { path, doctype };
						}
						return null;
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		public void CacheDocumentFilePath(string path, string contenttype, string uid, string project, string wo, string name, string description, string picture)
		{
			try
			{
				lock (m_DB)
				{
					using (SqliteCommand command = m_DB.CreateCommand())
					{
						string query = string.Format(
							"INSERT or REPLACE INTO DocumentCache (uid,path, doctype, project,overlay,name,description,picture ) VALUES  ('{0}','{1}','{2}','{3}', '{4}','{5}','{6}','{7}')", uid, path, contenttype, project, wo, name, description, picture);
						command.CommandText = query;
						var result = command.ExecuteNonQuery();
						if (result != 1)
						{
							throw new Exception("Failed CacheDocumentFilePath!!");
						}
					}
				}
			}
			catch (Exception e)
			{
				//throw e;
			}

		}

		public Dictionary<string, string> GetAudiosFromDevice(string uid, string project, string wo)
		{
			Dictionary<string, string> dic = new Dictionary<string, string>();

			try
			{
				lock (m_DB)
				{
					using (SqliteCommand command = m_DB.CreateCommand())
					{
						command.CommandText = string.Format(
							"SELECT path, audio_name FROM SoundCache WHERE devuid = '{0}' and project = '{1}' and overlay = '{2}'", uid, project, wo);
						var reader = command.ExecuteReader();
						while (reader.Read())
						{
							string path = reader.GetString(0);
							string date = reader.GetString(1);

							if (!string.IsNullOrEmpty(path))
								dic.Add(path, date);
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return dic;

		}

		public Dictionary<string, string> GetPicturesFromDevice(string uid, string project, string wo)
		{
			Dictionary<string, string> dic = new Dictionary<string, string>();

			try
			{
				lock (m_DB)
				{
					using (SqliteCommand command = m_DB.CreateCommand())
					{
						command.CommandText = string.Format(
							"SELECT path, photo_name FROM PhotoCache WHERE devuid = '{0}' and project = '{1}' and overlay = '{2}'", uid, project, wo);
						var reader = command.ExecuteReader();
						while (reader.Read())
						{
							string path = reader.GetString(0);
							string date = reader.GetString(1);

							if (!string.IsNullOrEmpty(path))
								dic.Add(path, date);
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return dic;

		}

		public void CacheSoundFilePath(string path, string uid, string project, string wo)
		{
			try
			{
				lock (m_DB)
				{
					using (SqliteCommand command = m_DB.CreateCommand())
					{
						string query = string.Format(
								"INSERT or REPLACE INTO SoundCache (devuid,path, project,overlay, audio_name ) VALUES  ('{0}','{1}','{2}','{3}','{4}')", uid, path, project, wo, DateTime.Now.ToString("MM-dd-yyyy-HH-mm-ss"));
						command.CommandText = query;
						var result = command.ExecuteNonQuery();
						if (result != 1)
						{
							throw new Exception("Failed CacheSoundFilePath!!");
						}
					}
				}
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public string[] GetSoundFilePath(string uid, string project, string wo)
		{
			try
			{
				lock (m_DB)
				{
					using (SqliteCommand command = m_DB.CreateCommand())
					{
						command.CommandText = string.Format(
								"SELECT path FROM SoundCache WHERE devuid = '{0}' and project = '{1}' and overlay = '{2}'", uid, project, wo);
						var reader = command.ExecuteReader();
						List<string> list = new List<string>();
						while (reader.Read())
						{
							string path = reader.GetString(0);
							list.Add(path);
						}
						return list.ToArray();
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void MarkAudioAsUploaded(string path)
		{

			try
			{
				lock (m_DB)
				{
					using (SqliteCommand command = m_DB.CreateCommand())
					{
						string query = string.Format(
								"UPDATE SoundCache set sent = '1' where path = '{0}'", path);
						command.CommandText = query;
						var result = command.ExecuteNonQuery();
						if (result != 1)
						{
							throw new Exception("Failed MarkAudioAsUploaded!!");
						}
					}
				}
			}
			catch (Exception e)
			{
				throw e;
			}

		}

		public void MarkPictureAsUploaded(string path)
		{

			try
			{
				lock (m_DB)
				{
					using (SqliteCommand command = m_DB.CreateCommand())
					{
						string query = string.Format(
								"UPDATE PhotoCache set sent = '1' where path = '{0}'", path);
						command.CommandText = query;
						var result = command.ExecuteNonQuery();
						if (result != 1)
						{
							throw new Exception("Failed MarkPictureAsUploaded!!");
						}
					}
				}
			}
			catch (Exception e)
			{
				throw e;
			}

		}

		public void DeleteAudio(string path)
		{

			try
			{
				lock (m_DB)
				{
					using (SqliteCommand command = m_DB.CreateCommand())
					{
						string query = string.Format(
								"DELETE FROM SoundCache where path = '{0}'", path);
						command.CommandText = query;
						var result = command.ExecuteNonQuery();
						if (result != 1)
						{
							throw new Exception("Failed DeletePhoto!!");
						}
					}
				}
			}
			catch (Exception e)
			{
				throw e;
			}

		}

		public void DeleteDocument(string path)
		{

			try
			{
				lock (m_DB)
				{
					using (SqliteCommand command = m_DB.CreateCommand())
					{
						string query = string.Format(
								"DELETE FROM DocumentCache where path = '{0}'", path);
						command.CommandText = query;
						var result = command.ExecuteNonQuery();
						if (result != 1)
						{
							throw new Exception("Failed DocumentCache!!");
						}
					}
				}
			}
			catch (Exception e)
			{
				throw e;
			}

		}

		public void DeletePhoto(string path)
		{

			try
			{
				lock (m_DB)
				{
					using (SqliteCommand command = m_DB.CreateCommand())
					{
						string query = string.Format(
								"DELETE FROM PhotoCache where path = '{0}'", path);
						command.CommandText = query;
						var result = command.ExecuteNonQuery();
						if (result != 1)
						{
							throw new Exception("Failed DeletePhoto!!");
						}
					}
				}
			}
			catch (Exception e)
			{
				throw e;
			}

		}

		public bool IsAudioUploaded(string path)
		{

			try
			{
				lock (m_DB)
				{
					SqliteCommand command = m_DB.CreateCommand();
					command.CommandText = string.Format(
							"SELECT sent FROM SoundCache WHERE path = '{0}' and sent = '1'", path);
					object ovalue = command.ExecuteScalar();
					if (ovalue == null)
					{
						return false;
					}
					else
					{
						return true;
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		public bool IsPictureUploaded(string path)
		{

			try
			{
				lock (m_DB)
				{
					SqliteCommand command = m_DB.CreateCommand();
					command.CommandText = string.Format(
							"SELECT sent FROM PhotoCache WHERE path = '{0}' and sent = '1'", path);
					object ovalue = command.ExecuteScalar();
					if (ovalue == null)
					{
						return false;
					}
					else
					{
						return true;
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		public List<ComosWebSDK.Data.CDocument> GetAllDocuments(string project, string wo)
		{
			List<ComosWebSDK.Data.CDocument> list = new List<ComosWebSDK.Data.CDocument>();

			try
			{
				lock (m_DB)
				{

					//uid text PRIMARY KEY, path text, doctype text, date TIMESTAMP default CURRENT_TIMESTAMP, project text, overlay text, name text, description text

					using (SqliteCommand command = m_DB.CreateCommand())
					{
						command.CommandText = string.Format(
								"SELECT uid,path,doctype,name,description, picture FROM DocumentCache WHERE project = '{0}' and overlay = '{1}'", project, wo);
						var reader = command.ExecuteReader();
						while (reader.Read())
						{
							list.Add(new ComosWebSDK.Data.CDocument()
							{
								FileName = reader.GetString(1),
								Name = reader.GetString(3),
								Description = reader.GetString(4),
								UID = reader.GetString(0),
								MimeType = reader.GetString(2),
								Picture = reader.GetString(5)
							});
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return list;
		}

		#endregion

		#region Start/Stop button support

		public void SetStartDate(string specowner, string nestedname, string project, string wo, DateTime value)
		{
			try
			{
				lock (m_DB)
				{
					using (SqliteCommand command = m_DB.CreateCommand())
					{
						string query = string.Format(
								"INSERT or REPLACE INTO Activities (specowner, nestedname, datestarted, project, overlay) VALUES  ('{0}','{1}','{2}','{3}','{4}')", specowner, nestedname, value.ToString("MM/dd/yyyy HH:mm:ss.fff"), project, wo);
						command.CommandText = query;
						var result = command.ExecuteNonQuery();
						if (result != 1)
						{
							throw new Exception("Failed WriteCacheSpecValue!!");
						}
					}
				}
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public DateTime GetStartDate(string specowner, string nestedname, string project, string wo)
		{
			try
			{
				lock (m_DB)
				{
					using (SqliteCommand command = m_DB.CreateCommand())
					{
						command.CommandText = string.Format(
					 "SELECT datestarted FROM Activities WHERE specowner = '{0}' and nestedname = '{1}' and project = '{2}' and overlay = '{3}'", specowner, nestedname, project, wo);

						object ovalue = command.ExecuteScalar();
						if (ovalue == null)
							return DateTime.MaxValue;
						string value = ovalue as string;
						if (string.IsNullOrEmpty(value))
							return DateTime.MaxValue;
						return DateTime.ParseExact(value, "MM/dd/yyyy HH:mm:ss.fff", CultureInfo.InstalledUICulture);
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		public void DeleteStartDate(string specowner, string nestedname, string project, string wo)
		{
			try
			{
				lock (m_DB)
				{
					using (SqliteCommand command = m_DB.CreateCommand())
					{
						string query = string.Format(
								"DELETE from Activities WHERE specowner = '{0}' and nestedname = '{1}' and project = '{2}' and overlay = '{3}'", specowner, nestedname, project, wo);

						command.CommandText = query;
						var result = command.ExecuteNonQuery();
					}
				}
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		#endregion

	}
}