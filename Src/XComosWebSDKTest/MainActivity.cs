using System.Reflection;
using Android.App;
using Android.OS;
using Xamarin.Android.NUnitLite;
using System.Net;

namespace XComosWebSDKTest
{
    [Activity(Label = "XComosWebSDKTest", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : TestSuiteActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            // tests can be inside the main assembly
            AddTest(Assembly.GetExecutingAssembly());
            // or in any reference assemblies
            // AddTest (typeof (Your.Library.TestClass).Assembly);

            // Once you called base.OnCreate(), you cannot add more assemblies.
            ServicePointManager.ServerCertificateValidationCallback += (o, certificate, chain, errors) => true;

            base.OnCreate(bundle);
        }
    }
}

