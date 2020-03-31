using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using NUnit.Framework;
using Plt;
using Chemserv;
using Comos.Global;
using ComosProfileMaster;

namespace BRComos.IO.Tests
{
  //  [TestFixture]
    public class ComosVersionTests
    {
       // [Test]
        public void TestWrite()
        {
            // Initialize workset
            IComosDWorkset WorkSet = null;
            WorkSet = new CPLTWorkset() as IComosDWorkset;
            bool error = WorkSet.Init(string.Empty, string.Empty, @"D:\6.ComosMobile\Data\Demo_iDB\Demo_iDB.mdb");
            //Assert.IsTrue(error);
            //Assert.IsTrue(WorkSet.IsInitialized());
            AppGlobal.Workset = WorkSet;

            // Set current user
            var user = GetUserObject("OQS6HW");
            AppGlobal.Workset.SetCurrentUser((object)user);
            user = null;

            // Set project and layer
            var project = (IComosDProject)AppGlobal.Workset.LoadObjectByType(ComosSystemTypes.SystemTypeProject, "A3BQHFA8AR");
            AppGlobal.Workset.SetCurrentProject((object)project);

            // Set working layer
            var layer = (IComosDWorkingOverlay)AppGlobal.Workset.LoadObjectByType(ComosSystemTypes.SystemTypeWorkingOverlay, "A3XPPURWYZ");
            AppGlobal.Workset.GetCurrentProject().CurrentWorkingOverlay = layer;
            layer = null;

            //IComosDLanguage projectDefaultLanguage = GetProjectDefaultLanguage(project, this.ProfileMaster);
            //if (projectDefaultLanguage != null)
            //    project.SetCurrentLanguageForSession((object)projectDefaultLanguage);

            // Get the comos object
            var co = (IComosDDevice)AppGlobal.Workset.LoadObjectByType(ComosSystemTypes.SystemTypeDevice, "A3XPPJCDYZ");
            //co.CalculateLinkedSpecifications();
            // Get the progress spec
            //var spec = (IComosDSpecification)AppGlobal.Workset.LoadObjectByType(ComosSystemTypes.SystemTypeSpecification, "A3XQ9FEUZJ");
            //var spec = (IComosDSpecification)AppGlobal.Workset.LoadObjectByType(ComosSystemTypes.SystemTypeSpecification, "A3XQQRAPL7");
            var spec = co.spec("Z10T00002.Z10A00050");
            var linkinfo = spec.LinkInfo();
            if (linkinfo != null)
            {
                if(linkinfo.LinkType == 6)
                    spec = spec.GetLinkedSpecification();
            }
            co = null;

            // Write the value
            string progress = ((int)((new Random(DateTime.Now.Millisecond)).NextDouble() * 100.0)).ToString();
            spec.value = progress;

            // Save the spec.
            spec.Save();
            System.Diagnostics.Debug.WriteLine(spec.SystemFullName());
            spec = null;

            // Save the project
            project.SaveAll();
            project = null;
            // Exit.
            AppGlobal.Workset.ReleaseAllObjects();
            //AppGlobal.Workset.Terminate();

            // Read the value back and it should be the same
            
            // Get the comos object
            co = (IComosDDevice)AppGlobal.Workset.LoadObjectByType(ComosSystemTypes.SystemTypeDevice, "A3XPPJA6YZ");
            // Get the progress spec
            spec = (IComosDSpecification)AppGlobal.Workset.LoadObjectByType(ComosSystemTypes.SystemTypeSpecification, "A3XQQRAPL7");
            //spec = co.spec("Y00T00236.FR001");
            // read the value
            string comosprogress = spec.value;
            //Assert.IsTrue(string.Compare(progress, comosprogress)==0);

            project = null;
            AppGlobal.Workset.ReleaseAllObjects();
            AppGlobal.Workset.DisposeAllModules();
            AppGlobal.Workset.Terminate();
        }

        public IComosDUser GetUserObject(string username)
        {
            IComosDCollection collection = AppGlobal.Workset.GetAllUsers();
            int size = collection.Count();
            for (int i = 0; i < size; i++)
            {
                IComosDUser user = (IComosDUser)collection.Item(i + 1);
                if (string.Compare(user.Name, username) == 0)
                {
                    return user;
                }
            }
            return null;
        }

    }
}
