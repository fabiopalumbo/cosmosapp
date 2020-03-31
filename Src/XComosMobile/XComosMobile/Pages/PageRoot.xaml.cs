using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XComosMobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageRoot : MasterDetailPage
    {
        public PageRoot()
        {
            InitializeComponent();
            MasterBehavior = MasterBehavior.Popover;
        }
    }
}