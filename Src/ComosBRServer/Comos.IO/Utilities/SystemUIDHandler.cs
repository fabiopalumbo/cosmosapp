using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRComos.IO.Utilities
{
    public static class SystemUIDHandler
    {

        // need to know whats the first letter

        public static bool GetComosSystemUID(string websystemuid, out string classname, out int systemtype, out string systemuid)
        {
            try
            {
                string[] tokens = websystemuid.Split(':');
                classname = tokens[3];
                systemuid = tokens[2];
                return int.TryParse(tokens[1], out systemtype);
            }
            catch (Exception ex)
            {
                Log.WriteLog("GetComosSystemUID::" + websystemuid + "::" + ex.Message);

                classname = "";
                systemuid = "";
                systemtype = 0;
                return false;
            }
            
        }

        public static string GetComosWebSystemUID(Plt.IComosBaseObject obj)
        {

            string systemuid = obj.SystemUID();
            string systemtype = obj.SystemType().ToString();
            string startletter = "U";
            string classname = "";
            if (obj is Plt.IComosDDevice)
            {
                classname = ((Plt.IComosDDevice)obj).Class;
            }

            return startletter + ":" + systemtype + ":" + systemuid + ":" + classname;
        }

    }
}
