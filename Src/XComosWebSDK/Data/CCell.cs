using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComosWebSDK.Data
{
    public class CCell
    {
        public string UID { get; set; }
        public string Text { get; set; }
        public double NumericValue { get; set; }
        public string Picture { get; set; }
        public bool IsClientPicture { get; set; }
        public bool IsCachedValue { get; set; }

    }
}
