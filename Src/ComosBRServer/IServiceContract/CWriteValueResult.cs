using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBRServiceContracts
{
    public class CWriteValueResult
    {
        public string ErrorMsg { get; set; }
        public string SystemUID { get; set; }
        public string NestedName { get; set; }
        public string OldValue { get; set; }
    }
}
