using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComosWebSDK.Data;

namespace XComosMobile.Services
{
    /// <summary>
    /// This class handles all requests of the mobile application
    /// to store data on disk. 
    /// </summary>
    public class XDatabase
    {
        // A generic database (probably using SQLITE but does not have to be)
        // This interface is not aware of any COMOS or application
        // specific classes.
        IDatabase m_DB = null;
        public XDatabase(IDatabase db)
        {
            m_DB = db;
        }

        public string[] GetDocumentFilePath(string uid, string project, string wo)
        {
            return m_DB.GetDocumentFilePath(uid, project, wo);
        }
        public void CacheDocumentFilePath(string path, string contenttype, string uid, string project, string wo,string name, string description, string picture)
        {

            m_DB.CacheDocumentFilePath(path, contenttype, uid,project,wo,name,description, picture);
        }
        public ComosWebSDK.UI.UICachedScreen GetCachedScreen(string uid)
        {
            return m_DB.GetCachedScreen(uid);
        }
        public void CacheScreen(string uid, string jsoncontent)
        {
            m_DB.CacheScreen(uid,jsoncontent);
        }

        public void DeleteCachedDevice(string id)
        {
            m_DB.DeleteCachedDevice(id);
        }

        public List<CCachedDevice> GetCachedDevices()
        {
            List<CCachedDevice> list = m_DB.GetCachedDevices();

            foreach (CCachedDevice ccdev in list)
            {
                ccdev.ValueCollection = Newtonsoft.Json.JsonConvert.DeserializeObject<IBRServiceContracts.CWriteValueCollection>(ccdev.Values);
            }

            return list;

        }
        public void CacheDeviceToCreate(string project, string overlay, string owner, string cdev, IBRServiceContracts.CWriteValueCollection collection, string tempuid, string ownername, string ownerpicture, string description)
        {
            string values = Newtonsoft.Json.JsonConvert.SerializeObject(collection);
            m_DB.CacheDeviceToCreate(project, overlay, owner, cdev, values, tempuid,ownername,ownerpicture, description);

        }

        public List<string> GetFavorites()
        {
            return m_DB.GetFavorites();
        }

        public IBRServiceContracts.CWriteValueCollection GetAllCachedSpecsFromOwner(string uid, string project, string overlay)
        {
            return m_DB.GetAllCachedSpecsFromOwner(uid, project, overlay);
        }

        public void ClearAllSpecsFromOwner(string specowner, string project,string overlay)
        {
            m_DB.ClearAllSpecsFromOwner(specowner, project, overlay);
        }

        public void ClearSpecFromCache(string specowner, string nestedname, string project, string wo)
        {
            m_DB.ClearSpecFromCache(specowner, nestedname, project, wo);
        }

        public void WriteCacheSpecValue(string specowner,string nestedname, string project, string wo, string value, string description = "", string ownername = "", string ownerdesc = "")
        {
            m_DB.WriteCacheSpecValue(specowner,nestedname, project,wo,value,description,ownername, ownerdesc);
        }

        public string ReadCacheSpecValue(string specowner, string nestedname, string project, string wo, string description = "")
        {
            return m_DB.ReadCacheSpecValue(specowner,nestedname, project,wo, description);
        }

        public void SetFavorite(string name, string value)
        {
            m_DB.SetFavorite(name,value);
        }
        public void RemoveFavorite(string name)
        {
            m_DB.RemoveFavorite(name);
        }

        public void ClearSetting(string name)
        {
            m_DB.ClearSetting(name);
        }

        public bool IsFavorite(string name)
        {
            return m_DB.IsFavorite(name);
        }
        public void WriteSetting(string name, string value)
        {
            m_DB.WriteSetting(name, value);
        }

        public string ReadSetting(string name, string defaultvalue)
        {
            return m_DB.ReadSetting(name, defaultvalue);
        }

        public void WriteSetting(string name, bool value)
        {
            string tmp = value.ToString();
            m_DB.WriteSetting(name, tmp);
        }

        public bool ReadSetting(string name, bool defaultvalue)
        {
            string tmp = m_DB.ReadSetting(name, null);
            if (tmp == null)
                return defaultvalue;

            bool result = defaultvalue;
            if (bool.TryParse(tmp, out result))
            {
                return result;
            }
            else
            {
                // Do not ignore this exception.
                // There is a mix somewher between saved types.
                throw new Exception(string.Format("ReadSettings : {0} is not a boolean value", name));
            }
        }
        readonly char Seperator = '^';
        public void WriteSetting(string name, ComosWebSDK.Data.CDatabase db)
        {
            String tmp = db.Key + Seperator + db.Value;
            m_DB.WriteSetting(name, tmp);
        }
        public CDatabase ReadSetting(string name, CDatabase db)
        {
            string tmp = m_DB.ReadSetting(name, null);
            if (tmp == null)
                return db;
            String[] tokens = tmp.Split(Seperator);
            return new CDatabase()
            {
                Key = tokens[0],
                Value = tokens[1],
            };
        }
        public void WriteSetting(string name, CLanguage language)
        {
            String tmp = language.Key + Seperator + language.LCID + Seperator + language.Value;
            m_DB.WriteSetting(name, tmp);
        }
        public CLanguage ReadSetting(string name, CLanguage language)
        {
            string tmp = m_DB.ReadSetting(name, null);
            if (tmp == null)
                return language;
            String[] tokens = tmp.Split(Seperator);
            if (tokens.Length == 3)
            {            
                return new CLanguage()
                {
                    Key = tokens[0],
                    LCID = tokens[1],
                    Value = tokens[2],
                };
            }
            else
            {
                return null;
            }
        }
        public void WriteSetting(string name, CProject project)
        {
            StringBuilder tmp = new StringBuilder();
            tmp.Append(project.Description); tmp.Append(Seperator);
            tmp.Append(project.IsClientPicture.ToString()); tmp.Append(Seperator);
            tmp.Append(project.Name); tmp.Append(Seperator);
            tmp.Append(project.ProjectUID); tmp.Append(Seperator);
            tmp.Append(project.Picture); tmp.Append(Seperator);
            tmp.Append(project.UID); 
            m_DB.WriteSetting(name, tmp.ToString());
        }

        public CProject ReadSetting(string name, CProject project)
        {
            string tmp = m_DB.ReadSetting(name, null);
            if (tmp == null)
                return project;
            String[] tokens = tmp.Split(Seperator);
            return new CProject()
            {
                Description = tokens[0],
                IsClientPicture = bool.Parse(tokens[1]),
                Name = tokens[2],
                ProjectUID = tokens[3],
                Picture = tokens[4],
                UID = tokens[5],
            };
        }

        public void WriteSetting(string name, CWorkingLayer layer)
        {
            StringBuilder tmp = new StringBuilder();
            tmp.Append(layer.Database); tmp.Append(Seperator);
            tmp.Append(layer.Description); tmp.Append(Seperator);
            tmp.Append(layer.IsClientPicture); tmp.Append(Seperator);
            tmp.Append(layer.Name); tmp.Append(Seperator);
            tmp.Append(layer.OverlayUID); tmp.Append(Seperator);
            tmp.Append(layer.Picture); tmp.Append(Seperator);
            tmp.Append(layer.ProjectUID); tmp.Append(Seperator);            
            tmp.Append(layer.UID); tmp.Append(Seperator);
            tmp.Append(Newtonsoft.Json.JsonConvert.SerializeObject(layer.Layers)); tmp.Append(Seperator);

            tmp.Append(Newtonsoft.Json.JsonConvert.SerializeObject(layer.OwnerLayers));

            m_DB.WriteSetting(name, tmp.ToString());
        }
        public CWorkingLayer ReadSetting(string name, CWorkingLayer layer)
        {
            string tmp = m_DB.ReadSetting(name, null);
            if (tmp == null)
                return layer;
            String[] tokens = tmp.Split(Seperator);
            CWorkingLayer wo = new CWorkingLayer()
            {
                Database = tokens[0],
                Description = tokens[1],
                IsClientPicture = bool.Parse(tokens[2]),
                Name = tokens[3],
                OverlayUID = tokens[4],
                Picture = tokens[5],
                ProjectUID = tokens[6],
                UID = tokens[7],                           
            };

            try
            {
                wo.Layers = Newtonsoft.Json.JsonConvert.DeserializeObject<CWorkingLayer[]>(tokens[8]);
                wo.OwnerLayers = Newtonsoft.Json.JsonConvert.DeserializeObject<CWorkingLayer[]>(tokens[9]);
            }
            catch (Exception)
            {                
            }

            return wo;
        }

        #region Play/Stop button support

        public void SetStartDate(string specowner, string nestedname, string project, string wo, DateTime value)
        {
            m_DB.SetStartDate(specowner, nestedname, project, wo, value);
        }

        public DateTime GetStartDate(string specowner, string nestedname, string project, string wo)
        {
            return m_DB.GetStartDate(specowner, nestedname, project, wo);
        }

        internal void DeleteStartDate(string specowner, string nestedname, string project, string wo)
        {
            m_DB.DeleteStartDate(specowner, nestedname, project, wo);
        }

        #endregion
    }
}
