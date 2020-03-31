using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBRServiceContracts
{
    public class CSearchResult
    {
        public string SystemUID { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public string BodyResult { get; set; }

        public string ComosResult { get; set; }

        public string Revision { get; set; }

        public string Version { get; set; }

        public string Project { get; set; }

        public string Overlay { get; set; }

    }
}
