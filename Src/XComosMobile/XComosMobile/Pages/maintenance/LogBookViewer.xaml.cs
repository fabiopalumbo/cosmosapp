using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;

using ComosWebSDK.Data;
using ComosWebSDK.Extensions;
using ComosWebSDK;
using XComosMobile.Pages.comos;
using System.Net.Http;
using XComosMobile.Services;

namespace XComosMobile.Pages.maintenance
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LogBookViewer : DevicesContainer
    {
        public LogBookViewer() : base()
        {
            InitializeComponent();
            this.BindingContext = this;
            ListViewDevices_ = (ListView)ListViewDevices;
            this.Appearing += DevicesContainerPage_Appearing;
        }

        protected override bool IsALogBook() => true;

        protected override string GetBelongsToAnEquipmentIcon(string name)
        {
            return Constants.CancelIcon;
        }

        protected override string GetButtonColor(string name)
        {
            return Color.Red.ToHex();
        }

        protected override string GetExceptionMessageWhenDevicesContainerDontAppear()
        {
            return "Error al obtener parte diario: ";
        }

        protected override string GetExceptionMessageWhenDeviceTappedFailed()
        {
            return "Error al abrir parte diario: ";
        }

        protected override string GetQueryFullName()
        {
            return Constants.QueryLogBookSystemFullName;
        }
    }
}