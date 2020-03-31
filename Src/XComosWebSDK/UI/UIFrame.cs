using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComosWebSDK.UI
{
    public class UIFrame: UIBase
    {
        public int TabIndex { get; set; }

        List<UIBase> m_Children = null;
        public UIBase[] Children
        {
            get
            {
                return (m_Children == null) ? null : m_Children.ToArray();
            }
        }

        public void AddChild(UIBase b)
        {
            if (m_Children == null)
                m_Children = new List<UIBase>();
            m_Children.Add(b);
        }

        public override bool ContainsNestedName(string nestedname)
        {
            foreach (var child in Children)
            {
                if (child is UIFrame)
                {
                    if (((UIFrame)child).ContainsNestedName(nestedname))
                        return true;
                }
                else if (string.Compare(child.NestedName,nestedname) == 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
