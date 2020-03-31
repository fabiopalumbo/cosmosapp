using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComosWebSDK.Data;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XComosMobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageFavorites : PageTemplate
    {

        public ViewModels.Navigator Navigator { get; set; }
        public PageFavorites(CWorkingLayer layer, string language)
        {
            InitializeComponent();

            this.Navigator = new ViewModels.Navigator(layer, language);
            this.Navigator.Nav = this.NavigationBarControl;
            this.BindingContext = this.Navigator;

            GetFavoritesData();

        }

        private CQuerieResult GetFavoritesData()
        {
            CQuerieResult q = new CQuerieResult();
            var db = Services.XServices.Instance.GetService<Services.XDatabase>();
            List<string> favorites = db.GetFavorites();
            CColumn[] columns = new CColumn[2];
            CRow[] rows = new CRow[favorites.Count];

            columns[0] = new CColumn() { DisplayDescription = "Name" };
            columns[1] = new CColumn() { DisplayDescription = "Description" };
            //columns[2] = new CColumn() { DisplayDescription = "Pic" };

            q.Columns = columns;
            q.Rows = rows;

            int i = 0;

            foreach (var item in favorites)
            {
                CRow row = new CRow();
                CObject output = Newtonsoft.Json.JsonConvert.DeserializeObject<CObject>(item);
                m_Query.SetMainObject(output);
                row.Items = new CCell[2];

                row.Items[0] = new CCell() { Text = output.Name, UID = output.UID, Picture = output.Picture };
                row.Items[1] = new CCell() { Text = output.Description, UID = output.UID, Picture = output.Picture };
                //row.Items[2] = new CCell() { Text = output.Picture, UID = output.UID, Picture = output.Picture };

                row.UID = output.UID;
                rows[i] = row;

                i++;
            }

            m_Query.SetNavigator(this.Navigator.Database);
            m_Query.Query = q;
            m_Query.ShowListQuery();

            return q;


        }
    }
}