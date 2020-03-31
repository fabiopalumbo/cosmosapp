using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComosWebSDK.UI
{
    public class UIEdit: UIBase
    {
        public string Value { get; set; }        
        public string ValueType { get; set; }

        public bool HasUnit { get { return this.Unit != null; } }
        public string Unit { get; set; }

        public bool ReadOnly { get; set; } = false;
    }
}
