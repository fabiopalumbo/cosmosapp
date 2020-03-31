using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComosWebSDK.Data
{
    public class CAttributes
    {
        Dictionary<string, CAttribute> m_Attributes = new Dictionary<string, CAttribute>();

        public CAttribute this[string NestedName]
        {
            get
            {
                CAttribute attribute = null;
                m_Attributes.TryGetValue(NestedName, out attribute);
                return attribute;
            }
            internal set {
                m_Attributes[NestedName] = value;
            }
        }

        public IEnumerable<CAttribute> Items
        {
            get
            {
                return m_Attributes.Values;
            }
        }
        public IEnumerable<CAttribute> ItemsSorted
        {
            get
            {
                return m_Attributes.Values.OrderBy(o => o.y);
            }
        }

    }
}
