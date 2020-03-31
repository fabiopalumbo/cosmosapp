using System;
using System.Collections.Generic;
using System.Text;

namespace XComosMobile.Pages.comos
{
    public interface AttributesUpdater
    {
        void updateEquipmentData(string name, bool isVerified, string verifiedSystemUid);
    }
}
