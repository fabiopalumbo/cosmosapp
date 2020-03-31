using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComosWebSDK.Data
{
    public class CCachedDevice
    {

        public string TempUID { get; set; }
        public string Description { get; set; }
        public string Values { get; set; }
        public string OwnerName { get; set; }
        public string OwnerPicture { get; set; }
        public string OwnerUID { get; set; }
        public string ProjectUID { get; set; }
        public string OverlayUID { get; set; }
        public string CDevUID { get; set; }
        public string Type { get; set; }
        public IBRServiceContracts.CWriteValueCollection ValueCollection { get; set; }

    }
}
