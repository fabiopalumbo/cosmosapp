using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComosWebSDK.UI
{
    public class UIBase
    {
        public int x { get; set; }
        public int y { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int WidthLabel { get; set; }
        public string NestedName { get; set; }
        public string CachedValue { get; set; }

        public string Text { get; set; }

        public bool IsIn(UIFrame frame)
        {
            return 
                x > frame.x && 
                y > frame.y && 
                x < (frame.x+frame.width) && 
                y < (frame.y + frame.height);
        }

        public virtual bool ContainsNestedName(string nestedname)
        {
           return string.Compare(this.NestedName, nestedname) == 0;
        }
    }
}
