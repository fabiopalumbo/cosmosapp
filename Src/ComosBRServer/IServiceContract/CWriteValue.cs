using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBRServiceContracts
{
    public class CWriteValue
    {
        public string WebSystemUID { get; set; }
        public string NestedName { get; set; }
        public string NewValue { get; set; }
        public string OldValue { get; set; }
    }
}
