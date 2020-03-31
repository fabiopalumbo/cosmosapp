using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBRServiceContracts
{
    public class CColumn
    {
        private string description = "";
        public string Name { get; set; }

        public string Description { get { return description; } set { description = value; } }

        public int ColumnIndex { get; set; }

        public bool ShowIcon { get; set; }

        public enum DataType
        {
            qcBoolean,
            qcDate,
            qcDouble,
            qcLong,
            qcString,
            qcVariant,
        };

        public DataType ValueType { get; set; }

        public bool Visible { get; set; }

        //v2

        public string DisplayDescription { get { return description; } set { description = value; } }
    }
}
