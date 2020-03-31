using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;

namespace XComosMobile.Pages.controls
{
    public class CustomWebView : WebView
    {

        public async Task<bool> RequestPermissions()
        {
            var statusCamera = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            var statusMicrophone = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Microphone);
            return statusCamera == PermissionStatus.Granted && statusMicrophone == PermissionStatus.Granted;
        }
    }
}
