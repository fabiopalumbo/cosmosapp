using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBRServiceContracts
{
    public class TResult<T>
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public T data { get; set; }
    }
}
