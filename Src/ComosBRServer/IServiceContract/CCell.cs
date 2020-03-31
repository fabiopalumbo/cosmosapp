using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBRServiceContracts
{
    public class CCell
    {
        private string text = "";


        public string Value { get { return text; } set { text = value; } }
        public string Icon { get; set; }
        public string OldValue { get; set; }
        public string SystemUID { get; set; }
        public string NestedName { get; set; }
        public int SystemType { get; set; }
        public string DisplayValue { get; set; }
        public bool IsButton { get; set; }
        public bool IsBoolean { get; set; }
        public string SystemFullName { get; set; }
        public int FieldSize { get; set; }
        public bool IsQuery { get; set; }
        public bool ReadOnly { get; set; }
        public CCell[] StandardValues { get; set; }

        //v2

        public string Text { get { return text; } set { text = value; } }
        public string UID { get; set; }
    }
}
