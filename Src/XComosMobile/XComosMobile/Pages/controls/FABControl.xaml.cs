using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XComosMobile.Pages.controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [ContentProperty("AFButtons")]
    public partial class FABControl : ContentView  
    {
        public  ObservableCollection<AFButton> AFButtons { get; private set; }
        
        public FABControl()
        {

            InitializeComponent();            

            AFButtons = new ObservableCollection<AFButton>();

            AFButtons.CollectionChanged +=
                 new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Children_CollectionChanged);

            //for (int i = 0; i < 4; i++)
            //{
            //    AFButton bt = new AFButton();
            //    bt.Text = "\uf071";
            //    bt.Style = (Style)App.Current.Resources["ScreenButtonItem"];
            //    AddAFButtonToFAB(bt);
            //}

        }

        void Children_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    foreach (AFButton elem in e.NewItems)
                    {
                        mainStack.Children.Add(elem);            
                    }

                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:

                    foreach (AFButton elem in e.OldItems)
                    {                        
                        mainStack.Children.Remove(elem);            
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    break;
            }
        }

        public async void ExpandRetract()
        {
            if (!mainStack.IsVisible)
            {
                btExpand.Text = "\uf054";
                await mainStack.FadeTo(1, 300, Easing.Linear);

            }
            else
            {
                btExpand.Text = "\uf141";
                await mainStack.FadeTo(0, 300, Easing.Linear);
            }

            mainStack.IsVisible = !mainStack.IsVisible;
        }
    
        private void btExpand_Clicked(object sender, EventArgs e)
        {

            ExpandRetract();

        }
    }
}