using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XComosMobile.Services
{
    public interface IPlatformSystem
    {
        void SetCurrentThemme(Theming.Themes theme);
        void UseLogin(string domain, string password, string username);
        bool IsOnline { get; set; }
        bool IsDeviceOnline { get; }
        string GetPersonalFolderPath();
        Task<bool> OpenFile(string filename, string contenttype = null);

        void DeleteFile(string filename);

        bool FileExists(string filename);

        void ShowToast(string message);

        void ShowProgressMessage(string message, bool spinner);

        void HideProgressMessage();

        void IncrementProgress(int delta);

        double GetScreenWidthPixels { get; }

        double GetScreenHeightPixels { get; }

        double GetScreenDensity { get; }

        void GalleryMedia();

        void CameraMedia(string cameraform);

        byte[] PathToByte(string path);

        void ShowPicture(string imageURL);

        Stream PathToStream(string path);

        Task<string> SaveAndOpenDocument(string filename, System.IO.Stream stream, string mimetype, bool open);
        void SendNotification(string title, string text, bool sound = false, bool progress = false, int total = 0, int partial = 0, int percentMax = 100);

        string AppVersionName { get; }

        Task<Stream> DownScaleImage(string path);

        string StartRecordingAudio();

        bool StopRecordingAudio();

        void CancelRecording(string path);

        void PlayAudio(string path);

        void StopAudio();

        void startworker(string[] uids);

        
    }
}
