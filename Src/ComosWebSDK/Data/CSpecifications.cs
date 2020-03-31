using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComosWebSDK.Data
{
    public class CSpecification
    {
        public string NestedName { get; set; }
        public string CachedValue { get; set; }
        public bool IsEditable { get; set; }
        public bool IsReauthenticationRequired { get; set; }
        public string ObjectUID { get; set; }
        public string ProjectName { get; set; }
        public string OverlayName { get; set; }
        public object children { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UID { get; set; }
        public string ProjectUID { get; set; }
        public string OverlayUID { get; set; }
        public string SystemFullName { get; set; }
        public string ClassType { get; set; }
        public string Picture { get; set; }
        public bool IsClientPicture { get; set; }
    }
}
