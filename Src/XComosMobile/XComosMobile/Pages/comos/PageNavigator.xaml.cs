using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ComosWebSDK.Data;

namespace XComosMobile.Pages.comos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageNavigator : PageTemplate
    {

        public PageNavigator(CWorkingLayer layer, string language, string navmode = "U")
        {
            InitializeComponent();
            this.Navigator = new ViewModels.Navigator(layer, language);
            this.Navigator.NavigationMode = navmode;
            NavigationBarControl.IsVisible = false;
            this.Navigator.Nav = this.NavigationBarControl;
            this.BindingContext = this.Navigator;
            this.NavigationBarControl.OnFiltered += SearchControl_OnFiltered;
            ListViewObjects.ItemTapped += ListViewObjects_ItemTapped;
            this.ShowHome = false;
            this.ShowSearch = true;
            this.ShowRefresh = false;

        }

        private void ListViewObjects_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (DetailsClicked)
            {
                DetailsClicked = false;
                return; // When user clicks button in viewcell, the OnClick in view cell is also triggered.
            }
            var obj = ListViewObjects.SelectedItem as CObject;

            NavigateToObject(obj);

            ListViewObjects.SelectedItem = null;
        }

        private void SearchControl_OnFiltered(object sender, EventArgs e)
        {
            ListViewObjects.ItemsSource = this.NavigationBarControl.FilteredList;
        }

        public async void NavigateToObject(CObject obj)
        {
            if (obj != null)
                await this.Navigator.NavigateToChild(obj);

            BtnMoveUp.IsVisible = true;
        }

        public ViewModels.Navigator Navigator { get; set; }

        bool DetailsClicked = false;
        public async Task OnChildSelected(object sender, EventArgs e)
        {
            //if (DetailsClicked)
            //{
            //    DetailsClicked = false;
            //    return; // When user clicks button in viewcell, the OnClick in view cell is also triggered.
            //}
            //var obj = ListViewObjects.SelectedItem as CObject;

            //NavigateToObject(obj);            
        }

        public async void OnClickedMoveUp(object sender, EventArgs e)
        {
            await this.Navigator.MoveUp();

            if (this.Navigator.Parent == null)
                BtnMoveUp.IsVisible = false;
            BtnMoveUp.IsVisible = this.Navigator.IsNotRoot;
        }

        public async void OnDetailsClicked(object sender, EventArgs e)
        {
            DetailsClicked = true;
            Button btn = (Button)sender;
            CObject o = (CObject)btn.CommandParameter;
            if (o.IsQuery())
            {
                PageQueries page = new PageQueries(this.Navigator.Database, o);
                //await this.Navigation.PushAsync(page);
                await ((NavigationPage)((PageRoot)Application.Current.MainPage).Detail).Navigation.PushAsync(page);
            }
            else
            {
                PageSpecifications page = new PageSpecifications(this.Navigator.Database, o, this.Navigator.Language);

                await ((NavigationPage)((PageRoot)Application.Current.MainPage).Detail).Navigation.PushAsync(page);
                //await this.Navigation.PushAsync(page);
            }
        }

        protected override bool OnBackButtonPressed()
        {
            if (this.Navigator.IsNotRoot)
            {
                Task.Run(async () => { await this.Navigator.MoveUp(); });
                BtnMoveUp.IsVisible = this.Navigator.IsNotRoot;
                return true;
            }
            return false;
        }


    }
}