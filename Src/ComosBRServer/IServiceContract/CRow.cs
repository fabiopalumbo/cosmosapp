using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBRServiceContracts
{
    public class CRow
    {
        private CCell[] cells = null;
  

        public CCell[] Values { get { return cells; } set { cells = value; }}
        public string SystemFullName { get; set; }
        public string Description { get; set; }
        public string SystemUid { get; set; }
        public bool IsQuery { get; set; }
        public bool ReadOnly { get; set; }

        //v2

        public CCell[] Items { get { return cells; } set { cells = value; } }
        public string UID { get; set; }
    }
}
