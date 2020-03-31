using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ComosWebSDK.Data;

namespace XComosMobile.Pages.maintenance
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WorkPackageCell : ViewCell
    {
        public WorkPackageCell()
        {
            InitializeComponent();
        }

        #region show by bindable property

        public static readonly BindableProperty ShowByIndexProperty =
            BindableProperty.Create(nameof(ShowByIndex), typeof(int), typeof(WorkPackageCell), 0);
        public int ShowByIndex
        {
            get
            {
                int index = (int)GetValue(ShowByIndexProperty);
                if (index < 0)
                    return 0;
                return index;
            }
            set { SetValue(ShowByIndexProperty, value); }
        }

        #endregion

        protected override void OnBindingContextChanged()
        {
            if (this.BindingContext is WorkPackageViewModel)
            {
                UpdateContent();
            }
            base.OnBindingContextChanged();
        }

        public Color BackColor
        {
            get
            {
                Converters.TextToBoolean c = new Converters.TextToBoolean();
                WorkPackageViewModel vm = (WorkPackageViewModel)this.BindingContext;

                string finish = vm.Items[4].Text;
                string cancel = vm.Items[5].Text;

                bool isfinish = (bool)c.Convert(finish, null, null, null);
                bool iscancel = (bool)c.Convert(cancel, null, null, null);

                if (isfinish)
                    return Color.Green;
                if (iscancel)
                    return Color.Red;

                return Color.Yellow;
            }

        }

        protected override void OnPropertyChanged(string propertyName)
        {
            if (propertyName == ShowByIndexProperty.PropertyName)
            {
                if (this.BindingContext is WorkPackageViewModel)
                {
                    UpdateContent();
                }
            }
            base.OnPropertyChanged(propertyName);
        }

        protected void UpdateContent()
        {
            Converters.IsQueryCachedValue iscacheconverter = new Converters.IsQueryCachedValue();

            WorkPackageViewModel row = (WorkPackageViewModel)this.BindingContext;
            int index = this.ShowByIndex;

            if (index == 0)
            {
                this.TaskTitle.Text = row.Items[0].Text + " | " + row.Items[2].Text + " | " + row.Items[3].Text;
            }
            else
            {
                this.TaskTitle.Text = row.Items[index].Text;
            }

            if (row.Items.Length >= 6)
                this.TaskDescription.Text = row.Items[1].Text + " | " + row.Items[4].Text + " | " + row.Items[5].Text;

            for (int i = 0; i < row.Items.Length; i++)
            {
                this.GridTask.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
            }

            for (int i = 0; i < row.Items.Length; i++)
            {
                this.GridTask.Children.Add(new Label()
                { Text = row.Columns[i].DisplayDescription, }, 0, i);

                Label value = new Label()
                { Text = row.Items[i].Text };

                Binding bindinguiddev = new Binding("Items[" + i + "].IsCachedValue");
                bindinguiddev.Converter = iscacheconverter;
                value.SetBinding(Label.TextColorProperty, bindinguiddev);

                this.GridTask.Children.Add(value, 1, i);

            }
        }

        protected async void OnTaskTitleTapped(object sender, EventArgs args)
        {
            if (TaskInfo.IsVisible)
            {
                //TaskInfo.TranslateTo(0, -60, 300, Easing.Linear);
                //await TaskInfo.ScaleTo(0, 300, Easing.Linear);
                await TaskInfo.FadeTo(0, 300, Easing.Linear);
                TaskInfo.IsVisible = !TaskInfo.IsVisible;
            }
            else
            {
                TaskInfo.IsVisible = !TaskInfo.IsVisible;
                //TaskInfo.TranslateTo(0, 60, 300, Easing.Linear);
                //TaskInfo.ScaleTo(1, 300, Easing.Linear);
                TaskInfo.FadeTo(1, 750, Easing.Linear);
            }
        }
    }
}