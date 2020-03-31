using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.ComponentModel;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;

namespace XComosMobile.UWP.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageProgressDialog : PopupPage
    {
        public PageProgressDialog()
        {
            //InitializeComponent();
        }

        public string WaitingMessage { set {

                //m_WaitingMessage.Text = value;

            } }

        public bool IsWaiting
        {
            set
            {

               // m_IsWaiting.IsRunning = value;

            }
        }
    }
}