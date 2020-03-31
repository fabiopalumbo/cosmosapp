using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBRServiceContracts
{
    public class CComosObject
    {

        public string Name;
        public string SystemUid;
        public string Description;
        public string ControlType;
        public string Value;
        public string OwnerUID;
        public string NestedName;
        public string UIDLinkObject;
        public int SystemType;
        public List<CComosObject> Objects = new List<CComosObject>();

    }
}
