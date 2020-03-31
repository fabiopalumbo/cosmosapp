using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBRServiceContracts
{
    class CComosActivity
    {

    }

    public class ComosTab
    {
        public IBRServiceContracts.CCell[] Attributes { get; set; }
        public IBRServiceContracts.CQueryResult[] Queries { get; set; }
        public string Description { get; set; }

    }

    public class ComosActivity
    {
        public ComosTab[] Tabs { get; set; }
        public string Header { get; set; }
        public string SystemFullName { get; set; }
        public int SystemType { get; set; }
    }

}
