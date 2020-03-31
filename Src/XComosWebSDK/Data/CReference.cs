using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComosWebSDK.Data
{
    class CReference
    {
        public string Description { get; set; }
        public string Type { get; set; }
        public string Action { get; set; }
        public string Parameter { get; set; }
        public string Parameter2 { get; set; }
        public List<CReference> Children { get; set; }
        public bool IsChecked { get; set; }
        public bool IsDisabled { get; set; }
        public string HelpId { get; set; }
        public string Picture { get; set; }
        public bool IsClientPicture { get; set; }
    }
}
