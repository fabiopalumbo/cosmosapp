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
    public partial class TaskCell : ViewCell
    {

        private const string EXPANDED = "\uf107";
        private const string NORMAL = "\uf106";
        private bool isUpdating;

        public TaskCell()
        {
            InitializeComponent();


            this.CompleteTask.Toggled += CompleteTask_Toggled;
            this.Appearing += TaskCell_Appearing;
        }

        private async void TaskCell_Appearing(object sender, EventArgs e)
        {

            try
            {

                var db = Services.XServices.Instance.GetService<Services.IDatabase>();
                ViewModel task = (ViewModel)this.BindingContext;
                labelNew.IsVisible = db.IsTaskNew(task.Row.Items[0].UID);

                CompleteTask.IsVisible = false;
                int? index = task.Columns.ToList().FindIndex(column => column.DisplayDescription.Equals("MP Padre"));
                if (index != null)
                {
                    if (task.Items[(int)index].Text.Equals("@30|M60|A10|A10|A50|A10|Z15")) // Es una task de un mantenimiento preventivo
                    {
                        CompleteTask.IsVisible = true;
                    }
                }

                if (!mainFrame.AnimationIsRunning("FadeTo"))
                {
                    await mainFrame.FadeTo(0, 1, Easing.Linear);
                }

                mainFrame.IsVisible = true;

                if (!mainFrame.AnimationIsRunning("FadeTo"))
                {
                    await mainFrame.FadeTo(1, 300, Easing.Linear);
                }
            }
            catch (Exception)
            {

            }

        }

        #region show by bindable property

        public static readonly BindableProperty ShowByIndexProperty =
            BindableProperty.Create(nameof(ShowByIndex), typeof(int), typeof(TaskCell), 0);
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
            if (this.BindingContext is ViewModel)
            {
                UpdateContent();
            }
            base.OnBindingContextChanged();
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            if (propertyName == ShowByIndexProperty.PropertyName)
            {
                if (this.BindingContext is ViewModel)
                {
                    UpdateContent();
                }
            }
            base.OnPropertyChanged(propertyName);
        }

        protected void UpdateContent()
        {
            isUpdating = true;
            ViewModel row = (ViewModel)this.BindingContext;
            int index = this.ShowByIndex;
            this.TaskTitle.Text = row.Items[index].Text;
            this.TaskDescription.Text = row.Items[1].Text;
            this.CompleteTask.IsToggled = row.Items[4].Text.Equals("1") ? true : false;

            Converters.IsQueryCachedValue iscacheconverter = new Converters.IsQueryCachedValue();

            for (int i = 0; i < row.Items.Length; i++)
            {
                this.GridTask.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
            }

            for (int i = 0; i < row.Items.Length; i++)
            {
                // skip the task description!
                if (i != 1)
                {
                    this.GridTask.Children.Add(new Label()
                    { Text = row.Columns[i].DisplayDescription, FontAttributes = FontAttributes.Bold }, 0, i);


                    if ((row.Items[i].Text == "1" && row.Items[i].NumericValue == 1) || (row.Items[i].Text == "0" && row.Items[i].NumericValue == 0))
                    {
                        string afcode = "";
                        switch (row.Items[i].NumericValue)
                        {
                            case 1:
                                afcode = "\uf00c";
                                break;
                            case 0:
                                afcode = "\uf00d";
                                break;
                        }

                        controls.AFLabel value = new controls.AFLabel() { Text = afcode };

                        Binding bindinguiddev = new Binding("Items[" + i + "].IsCachedValue");
                        bindinguiddev.Converter = iscacheconverter;
                        value.SetBinding(Label.TextColorProperty, bindinguiddev);
                        this.GridTask.Children.Add(value, 1, i);
                    }
                    else
                    {
                        Label value = new Label()
                        { Text = row.Items[i].Text };

                        Binding bindinguiddev = new Binding("Items[" + i + "].IsCachedValue");
                        bindinguiddev.Converter = iscacheconverter;
                        value.SetBinding(Label.TextColorProperty, bindinguiddev);

                        // last item aways a memo field
                        if (i == row.Items.Length - 1)
                        {
                            this.GridTask.Children.Add(value, 0, i + 1);
                            Grid.SetColumnSpan(value, 2);
                        }
                        else
                        {
                            this.GridTask.Children.Add(value, 1, i);
                        }



                    }


                }
            }
            isUpdating = false;
        }

        private void CompleteTask_Toggled(object sender, ToggledEventArgs e)
        {
            if (!isUpdating)
            {
                Switch completeTaskSwitch = (Switch)sender;
                Services.XDatabase db = Services.XServices.Instance.GetService<Services.XDatabase>();
                ViewModels.ProjectData ProjectData = Services.XServices.Instance.GetService<ViewModels.ProjectData>();
                ViewModel task = (ViewModel)this.BindingContext;

                string value = task.Items[4].Text.Equals("0") ? "1" : "0";

                db.WriteCacheSpecValue(task.Items[0].UID, "Z10T00002.Z10A00048", ProjectData.SelectedProject.UID,
                    ProjectData.SelectedLayer.UID, value, task.Columns[4].DisplayDescription, task.Items[0].Text, task.Items[1].Text);

                if (value.Equals("1"))
                    db.WriteCacheSpecValue(task.Items[0].UID, "Z10T00002.Z10A00047", ProjectData.SelectedProject.UID,
                    ProjectData.SelectedLayer.UID, "100%", task.Columns[6].DisplayDescription, task.Items[0].Text, task.Items[1].Text);
            }

        }

        /*
        private async void StackLayout_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            StackLayout viewcell = (StackLayout)sender;            
            await viewcell.ScaleTo(1.2, 50, Easing.Linear);
            await viewcell.ScaleTo(1, 50, Easing.Linear);
        }
        */
        protected async void OnTaskTitleTapped(object sender, EventArgs args)
        {
            if (TaskInfo.IsVisible)
            {
                btExpand.Text = EXPANDED;
                await TaskInfo.FadeTo(0, 300, Easing.Linear);
                TaskInfo.IsVisible = !TaskInfo.IsVisible;

            }
            else
            {
                btExpand.Text = NORMAL;
                TaskInfo.IsVisible = !TaskInfo.IsVisible;
                TaskInfo.FadeTo(1, 750, Easing.Linear);

            }
        }
    }
}