using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IBRServiceContracts
{
	//comment
    public class CQueryResult
    {
        public DateTime Date { get; set;  }

        public List<CColumn> Columns { get; set;  }

        public List<CRow> Rows { get; set; }

        public int RowCount { get; set; }

        public string QueryName { get; set; }


    }
}
