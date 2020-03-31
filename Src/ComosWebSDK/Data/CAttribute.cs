using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComosWebSDK.Data
{
    public class CAttribute
    {
        public CAttribute()
        {
            x = int.MinValue;
            y = int.MinValue;
            width = int.MinValue;
            height = int.MinValue;
        }
        public string NestedName { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        public string Unit { get; set; }
        // If has units, the options list contain all valid units and Unit property contains selected unit.
        public bool HasUnits { get; set; }
        // an option has a key = comos description and a value = comos value
        public Dictionary<string, string> Options { get; set; }

        public bool IsBoolean { get; set; }

        public bool IsOptions {
            get
            {
                return !this.HasUnits && this.Options != null;
            }
        }

        public int x { get; set; }
        public int y { get; set; }
        public int width { get; set; }
        public int height { get; set; }

        public bool ReadOnly { get; set; } = false;
    }
}
