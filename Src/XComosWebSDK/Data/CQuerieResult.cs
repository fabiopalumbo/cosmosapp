using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComosWebSDK.Data
{
    public class CQuerieResult
    {
        public string UID { get; set; }
        public CColumn[] Columns { get; set; }
        public CRow[] Rows { get; set; }

    }
}
