using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Reflection;

namespace XComosMobile.Pages.controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NavigationBarControl : ContentView
    {
        List<FilterItem> list = new List<FilterItem>();
        public bool IsSearchVisible { get; set; }


        public string SearchBarText { get { return searchBar.Text; }}
        public List<object> FilteredList { get; set; }

        public event EventHandler OnSaved;

        protected virtual void Saved(object sender, EventArgs e)
        {
            EventHandler handler = OnSaved;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event EventHandler OnFiltered;


        protected virtual void Filtered(object sender, EventArgs e)
        {
            EventHandler handler = OnFiltered;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public List<FilterItem> FullList
        {
            get { return list; }
            set { list = value; }
        }

        public NavigationBarControl()
        {
            InitializeComponent();
                                   
        }
        

         public List<object> FilterList(string pattern) 
        {
            List<FilterItem> filter = list.Where(x => x.FilterValue.ToUpper().Contains(pattern.ToUpper())).ToList();
            List<object> objlist = new List<object>();

            foreach (var item in filter)
            {
                objlist.Add(item.FilterObject);
            }

            return objlist;
            //return list.Where(x => x.GetType().GetRuntimeProperty(propertyName).ToString().StartsWith(pattern)).ToList();

        }

        private void AFButton_Clicked(object sender, EventArgs e)
        {
            CheckVisible();
        }

        private void searchBar_Unfocused(object sender, FocusEventArgs e)
        {
            CheckVisible();

            //searchBar.Text = "";
        }

        private void CheckVisible()
        {
            searchBar.IsVisible = mainStack.IsVisible;
            mainStack.IsVisible = (!mainStack.IsVisible);
            
        }

        public event EventHandler<TextChangedEventArgs> OnSearchTextChanged;
        public event EventHandler<EventArgs> SearchBarSearchClicked;
        public event EventHandler<EventArgs> FilterShowClicked;

        private void showFilter_ShowFilterButtonPressed(object sender, EventArgs e)
        {            
            if (FilterShowClicked != null)
                FilterShowClicked(sender, e);
        }
        private void searchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            // buscar devices com searchBar.Text
            if (SearchBarSearchClicked !=null)      
            SearchBarSearchClicked(sender, e);
        }
        private void searchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (OnSearchTextChanged != null)
            {
                OnSearchTextChanged(sender, e);
                return;
            }

            if (SearchBarSearchClicked != null)
            {                
                return;
            }

            if (searchBar.Text != "")
            {
                FilteredList = FilterList(searchBar.Text);                
            }
            else
            {
                List<object> newlist = new List<object>();
                foreach (var item in FullList)
                {
                    newlist.Add(item.FilterObject);
                }
                FilteredList = newlist;                    
            }

            Filtered(this, new EventArgs());
        }

        public class FilterItem
        {

            public object FilterObject { get; set; }
            public string FilterValue { get; set; }

        }

        private void btSave_Clicked(object sender, EventArgs e)
        {
            Saved(this, new EventArgs());
        }

        private void btHome_Clicked(object sender, EventArgs e)
        {
            PageSolutions old = null;
            foreach (var item in this.Navigation.NavigationStack)
            {
                if(item is PageSolutions)
                {
                    old = (PageSolutions)item;
                    break;
                }
            }

            this.Navigation.RemovePage(old);
            var web = Services.XServices.Instance.GetService<ComosWebSDK.IComosWeb>();
            this.Navigation.PushAsync(new PageSolutions(web));

        }

        private void btWOInfo_Clicked(object sender, EventArgs e)
        {

            ViewModels.ProjectData ProjectData = Services.XServices.Instance.GetService<ViewModels.ProjectData>();
            var platform = Services.XServices.Instance.GetService<Services.IPlatformSystem>();

            platform.ShowToast(ProjectData.SelectedProject.Name + "   " + ProjectData.SelectedProject.Description + System.Environment.NewLine +
                               ProjectData.SelectedLayer.Name + "   " + ProjectData.SelectedLayer.Description);

        }
    }
}