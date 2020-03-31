using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComosWebSDK.Data;
using Xamarin.Forms;

namespace XComosMobile.Converters
{
    public class IsQueryCachedValue : Xamarin.Forms.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool iscache = (bool)value;

            if (iscache)
            {
                return (Color)App.Current.Resources["spec-only-cache"];
            }
            else
            {
                return (Color)App.Current.Resources["UITextColor"];
                
            }
          
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        
        public void GetQueryCachedValues(CQuerieResult query)
        {
            Services.IDatabase cachedb = Services.XServices.Instance.GetService<Services.IDatabase>();
            ViewModels.ProjectData ProjectData = Services.XServices.Instance.GetService<ViewModels.ProjectData>();

        
                foreach (var row in query.Rows)
                {

                    for (int i = 0; i < row.Items.Length; i++)
                    {
                       string uid = row.Items[0].UID;
                        CCell item = row.Items[i];

                        string cachedvalue = cachedb.ReadCacheSpecValue(uid, "", ProjectData.SelectedProject.ProjectUID, ProjectData.SelectedLayer.UID, query.Columns[i].DisplayDescription);
                        if (cachedvalue != "")
                        {
                            int result = 0;
                            item.Text = cachedvalue;

                            if (int.TryParse(cachedvalue, out result))
                                item.NumericValue = result;

                            item.IsCachedValue = true;
                        }
                            
                    }
                

                }

        }
    }
}
