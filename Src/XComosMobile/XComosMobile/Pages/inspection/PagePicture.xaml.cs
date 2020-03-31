using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace XComosMobile.Pages.inspection
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PagePicture : PageTemplate
    {
        public PagePicture(List<controls.ImageHelper> images, List<string> audios)
        {
            InitializeComponent();            
            int row = 0;
            int column = 0;

            //30, GridUnitType.Star
            decimal helper = (audios.Count + images.Count) / 3;
            decimal rowNumber = Math.Ceiling(helper);

            //if ((audios.Count + images.Count) % 3 > 0)
            //    rowNumber++;

            for (int i = 0; i < rowNumber; i++)
            {
                RowDefinition c1 = new RowDefinition();
                c1.Height = GridLength.Auto;
                mainGrid.RowDefinitions.Add(c1);
            }

            //foreach (var item in images)
            for (int i = 0; i < images.Count; i++)
            {
                controls.ImageHelper item = images[i];
                View imageView = controls.MidiaControl.CreateMediaItemForImage(item.ImageSource, item.Path, item.Date, Application.Current.MainPage.Width/3, Application.Current.MainPage.Width / 3);

                mainGrid.Children.Add(imageView);

                Grid.SetRow(imageView, row);
                Grid.SetColumn(imageView, column);

                column = ControlColumns(column);
                if(column == 0)
                {
                    row++;
                }
            }

            //foreach (var item in audios)
            for (int i = 0; i < audios.Count; i++)
            {
                string item = audios[i];
                View audioView = controls.MidiaControl.CreateMediaItemForAudio(item, Application.Current.MainPage.Width / 3, Application.Current.MainPage.Width / 3);

                mainGrid.Children.Add(audioView);

                Grid.SetRow(audioView, row);
                Grid.SetColumn(audioView, column);

                column = ControlColumns(column);
                if (column == 0)
                {
                    row++;
                }
            }
        }

        private int ControlColumns(int i)
        {
            //return (i < 3 ? i++ : 0);
            if (i < 2)
            {
                return (i + 1);
            }
            else
            {
                return 0;
            }
        }
    }

}