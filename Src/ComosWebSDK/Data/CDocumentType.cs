using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComosWebSDK.Data
{
    public class CDocumentType
    {
    
        public string Name { get; set; }
        public string Description { get; set; }
        public string SystemUID { get; set; }
        public int SystemType { get; set; }
        public string FullLabel { get; set; }
        public string UID { get; set; }
        public string Picture { get; set; }
        public bool IsClientPicture { get; set; }
    }
}
