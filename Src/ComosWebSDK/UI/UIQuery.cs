using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComosWebSDK.UI
{
    public class UIQuery: UIBase
    {
        public string Owner { get; set; }
        public string QueryUID { get; set; }

        public Data.CQuerieResult Result { get; set; }
    }
}
