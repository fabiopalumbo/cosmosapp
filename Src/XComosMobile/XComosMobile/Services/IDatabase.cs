using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComosWebSDK.Data;

namespace XComosMobile.Services
{
	public interface IDatabase : IDisposable
	{
		void Close();
		IDatabase Open(string filename);
		List<string> GetFavorites();

		void MarkTaskAsRead(string uid);
		bool IsTaskNew(string uid);
		void ClearSetting(string name);
		void WriteSetting(string name, string value);
		string ReadSetting(string name, string defaultvalue);
		bool IsFavorite(string name);
		void SetFavorite(string name, string value);
		void RemoveFavorite(string name);
		void WriteUrlCache(string url, string response);
		string ReadUrlCache(string url);
		void ClearUrlCache();
		void ClearAllSpecsFromOwner(string uid, string project, string overlay);
		IBRServiceContracts.CWriteValueCollection GetAllCachedSpecsFromOwner(string uid, string project, string overlay);
		void ClearSpecFromCache(string specowner, string nestedname, string project, string wo);
		void WriteCacheSpecValue(string specowner, string nestedname, string project, string wo, string value, string description = "", string ownername = "", string ownerdesc = "");
		string ReadCacheSpecValue(string specowner, string nestedname, string project, string wo, string description = "");
		void CacheDeviceToCreate(string project, string overlay, string owner, string cdev, string values, string tempuid, string ownername, string ownerpicture, string description);
		void DeleteCachedDevice(string id);

		void DeleteDocument(string path);
		void CacheScreen(string systemuid, string jsoncontent);
		List<ComosWebSDK.Data.CCachedDevice> GetCachedDevices();
		ComosWebSDK.UI.UICachedScreen GetCachedScreen(string systemuid);
		void CacheDocumentFilePath(string path, string contenttype, string uid, string project, string wo, string name, string description, string picture);
		string[] GetDocumentFilePath(string uid, string project, string wo);
		void CacheDevicePicture(string project, string layer, string devuid, string path, string date, string owner);
		Dictionary<string, string> GetPicturesFromDevice(string uid, string project, string wo);

		Dictionary<string, string> GetAudiosFromDevice(string uid, string project, string wo);

		List<ComosWebSDK.Data.CObject> GetDevicesWithCachedAttributes(string project, string wo);

		List<ComosWebSDK.Data.CDocument> GetAllDocuments(string project, string wo);
		void CacheSoundFilePath(string path, string uid, string project, string wo);
		string[] GetSoundFilePath(string uid, string project, string wo);
		bool IsPictureUploaded(string path);
		void MarkPictureAsUploaded(string path);

		void MarkAudioAsUploaded(string path);

		List<ComosWebSDK.Data.CObject> GetDevicesWithCachedPictures(string project, string wo);

		void DeletePhoto(string path);

		void DeleteAudio(string path);

		bool IsAudioUploaded(string path);

		#region Start/Stop button support

		void SetStartDate(string specowner, string nestedname, string project, string wo, DateTime value);
		DateTime GetStartDate(string specowner, string nestedname, string project, string wo);
		void DeleteStartDate(string specowner, string nestedname, string project, string wo);

		#endregion

		void InsertCObject(CObject item, string layer);

		CObject GetCObjectByFullName(string layer, string fullname);
	}
}