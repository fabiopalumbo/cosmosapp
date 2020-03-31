using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ComosWebSDK.Data;
using ComosWebSDK.Extensions;
using ComosWebSDK;

namespace XComosMobile.Pages.maintenance
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    //public partial class Tasks : PageTemplate
    public partial class Tasks : PageTemplate
    {

        public Tasks()
        {
            InitializeComponent();
            LoadTasksQuery();
            this.ShowSearch = true;
            this.ShowRefresh = true;

        }

        private async void LoadTasksQuery()
        {
            m_Query.nav = this.NavigationBarControl;

            ViewModels.ProjectData projectdata = Services.XServices.Instance.GetService<ViewModels.ProjectData>();
            IComosWeb m_ComosWeb = Services.XServices.Instance.GetService<IComosWeb>();
            CObject o = await m_ComosWeb.GetObjectBySystemFullName(projectdata.SelectedLayer, "@Q|@M|MyTasks");

            //ToolbarItems.Add(new ToolbarItem("Lista", "", async () =>
            //{
            //    m_Query.ViewMode = controls.QueryControl.QueryViewMode.Accordion;
            //    await m_Query.ShowListQuery();
            //}));

            //ToolbarItems.Add(new ToolbarItem("Tabela", "", async () =>
            //{
            //    m_Query.ViewMode = controls.QueryControl.QueryViewMode.List;
            //    await m_Query.ShowTableQuery();
            //}));


            m_Query.StartQuery(projectdata.SelectedDB.Key, o);

            this.NavigationBarControl.OnFiltered += m_Query.Nav_OnFiltered;
        }
    }
}