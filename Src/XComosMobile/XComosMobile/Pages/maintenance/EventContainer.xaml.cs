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
    public partial class EventContainer : DevicesContainer
    {
        public EventContainer() : base()
        {
            InitializeComponent();
            this.BindingContext = this;
            ListViewDevices_ = (ListView)ListViewDevices;
            this.Appearing += DevicesContainerPage_Appearing;
        }

        private bool IsNotInComosEventBag(string fatherName)
        {
            return (!fatherName.Equals("A90") && !fatherName.Equals("A100"));
        }

        protected override string GetBelongsToAnEquipmentIcon(string name)
        {
            return IsNotInComosEventBag(name) ? Constants.OkIcon : Constants.CancelIcon;
        }

        protected override string GetButtonColor(string name)
        {
            return IsNotInComosEventBag(name) ? Color.Green.ToHex() : Color.Red.ToHex();
        }

        protected override string GetExceptionMessageWhenDevicesContainerDontAppear()
        {
            return "Error al obtener avisos: ";
        }

        protected override string GetExceptionMessageWhenDeviceTappedFailed()
        {
            return "Error al abrir aviso: ";
        }

        protected override string GetQueryFullName()
        {
            return Constants.QueryEventContainerFullName;
        }
    }

}