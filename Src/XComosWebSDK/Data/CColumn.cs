using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComosWebSDK.Data
{
    public class CColumn
    {
        public int Id { get; set; }
        public int Alignment { get; set; }
        public int Width { get; set; }
        public bool Visible { get; set; }
        public string DisplayDescription { get; set; }
        public bool Numeric { get; set; }
        public bool WithPicture { get; set; }
        public bool WrapText { get; set; }
        public bool IsDate { get; set; }
        
    }
}
