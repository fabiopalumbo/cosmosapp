using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ComosWebSDK.Data;
using ComosWebSDK.Extensions;
using System.Collections;

namespace XComosMobile.Pages.comos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageQueries : PageTemplate
    {
        public PageQueries(string navigator, CObject o)
        {
            InitializeComponent();

            m_Query.nav = this.NavigationBarControl;
            this.ShowSearch = true;
            this.ShowRefresh = true;

            m_Query.StartQuery(navigator, o);

            this.NavigationBarControl.OnFiltered += m_Query.Nav_OnFiltered;
        }
    }
}