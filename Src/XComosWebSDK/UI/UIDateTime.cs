using System;
using System.Collections.Generic;
using System.Text;

namespace ComosWebSDK.UI
{
    public class UIDateTime : UIBase
    {
        public string Value { get; set; }
        public string ValueType { get; set; }

        public string ValueForComos { get; set; }

        public bool ReadOnly { get; set; } = false;
    }
}
