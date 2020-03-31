using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComosWebSDK.UI
{
    public class UICheckBox: UIBase
    {
        public bool Value { get; set; }
        
        public bool ReadOnly { get; set; } = false;
    }
}
