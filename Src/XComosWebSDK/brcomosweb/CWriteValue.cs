using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBRServiceContracts
{
    public class CWriteValue
    {

        private string newValue = "";
        private string oldValue = "";
        public string WebSystemUID { get; set; }
        public string NestedName { get; set; }
        public string NewValue
        {
            get
            {
                return newValue.Replace(@"\", @"\\").Replace(@"/",@"\/").Replace('"','\"');
            }
            set { newValue = value; }
        }
        public string OldValue
        {
            get
            {
                return oldValue.Replace(@"\", @"\\").Replace(@"/", @"\/").Replace('"', '\"');
            }
            set { oldValue = value; }
        }
        public string Description { get; set; }


    }
}
