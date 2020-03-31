using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comos.IO
{
    public enum WriteSpecError: int
    {
        None = 0,
        SpecHasInheritance = 1,
        UnsupportedLinkInfo = 2,
        ValueIsNotValid = 3,
        SyncError = 4,
        DateTimeConversionError = 5,
    }
}
