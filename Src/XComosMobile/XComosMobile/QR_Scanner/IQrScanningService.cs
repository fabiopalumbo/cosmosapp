using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XComosMobile.QR_Scanner
{
    public interface IQrScanningService
    {
        Task<string> ScanAsync();
    }
}
