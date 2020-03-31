using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComosWebSDK.Data
{
    public class CObject
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string UID { get; set; }
        public string ProjectUID { get; set; }
        public string OverlayUID { get; set; }
        public string SystemFullName { get; set; }
        public string ClassType { get; set; }
        public string Picture { get; set; }
        public bool IsClientPicture { get; set; }

        public IBRServiceContracts.CWriteValueCollection Values { get; set; }


        public bool IsQuery()
        {
            if (this.ClassType == null)
                return false;
            if (this.ClassType == "J1" || // engineering
                    this.ClassType == "J2" || // Base objects
                    this.ClassType == "J3" || // Documents
                    this.ClassType == "J4" || // Attributes
                    this.ClassType == "J:" || // Other
                    this.ClassType == "J")
                return true;
            return false;
        }
    }
}
