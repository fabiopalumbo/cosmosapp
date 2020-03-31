using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComosWebSDK.UI
{
    public class UIOptions: UIBase
    {
        // Dictionary<description,comos value>  
        public Dictionary<string, string> Options { get; set; }
        public string Value { get; set; }
    	public bool ReadOnly { get; set; } = false;
    }
}
