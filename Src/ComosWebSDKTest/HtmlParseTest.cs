using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ComosWebSDK;
using ComosWebSDK.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using ComosWebSDK.Extensions;

namespace ComosWebSDKTest
{
    [TestClass]
    public class HtmlParseTest
    {
        [TestMethod]
        public void ParseExample1_CheckBox()
        {
            CAttributes attributes = CHtmlParser.ParseAttributes(HtmlExamples.HtmlExample1);
            Assert.IsTrue(attributes["Y00T00250.Y00A00726"].Value == "False");
            Assert.IsTrue(attributes["Y00T00250.Y00A00726"].Description == "Canceled");

            Assert.IsTrue(attributes["Y00T00250.Y00A02847"].Value == "True");
            Assert.IsTrue(attributes["Y00T00250.Y00A02847"].Description == "Activate automatic notification");
        }

        [TestMethod]
        public void ParseExample1_Text()
        {
            CAttributes attributes = CHtmlParser.ParseAttributes(HtmlExamples.HtmlExample1);
            Assert.IsTrue(attributes["Y00T00250.Y00A02883"].Value == "LIANA.SANTOS");
            Assert.IsTrue(attributes["Y00T00250.Y00A02883"].Description == "Engineer");
        }

        [TestMethod]
        public void ParseExample1_Selection()
        {
            CAttributes attributes = CHtmlParser.ParseAttributes(HtmlExamples.HtmlExample1);
            CAttribute attribute = attributes["Y00T00250.Y00A01088"];

            Assert.IsTrue(attribute.Description == "Status");
            Assert.IsTrue(string.Compare(@"For information", attribute.Value) == 0);
            Assert.IsTrue(attribute.Options.Where(x => x.Key == "").Count() == 1);
            Assert.IsTrue(attribute.Options.Where(x => x.Key == "N/A").Count() == 1);
            Assert.IsTrue(attribute.Options.Where(x => x.Key == "New").Count() == 1);
            Assert.IsTrue(attribute.Options.Where(x => x.Key == "For information").Count() == 1);
            Assert.IsTrue(attribute.Options.Where(x => x.Key == "Confirmed").Count() == 1);
            Assert.IsTrue(attribute.Options.Where(x => x.Key == "Suspended").Count() == 1);
            Assert.IsTrue(attribute.Options.Where(x => x.Key == "Completed").Count() == 1);
            Assert.IsTrue(attribute.Options.Where(x => x.Key == "Delegated").Count() == 1);


            Assert.IsTrue(attribute.Options.Where(x => x.Value == "").Count() == 1);
            Assert.IsTrue(attribute.Options.Where(x => x.Value == "N/A").Count() == 1);
            Assert.IsTrue(attribute.Options.Where(x => x.Value == "New").Count() == 1);
            Assert.IsTrue(attribute.Options.Where(x => x.Value == "For information").Count() == 1);
            Assert.IsTrue(attribute.Options.Where(x => x.Value == "Confirmed").Count() == 1);
            Assert.IsTrue(attribute.Options.Where(x => x.Value == "Suspended").Count() == 1);
            Assert.IsTrue(attribute.Options.Where(x => x.Value == "Completed").Count() == 1);
            Assert.IsTrue(attribute.Options.Where(x => x.Value == "Delegated").Count() == 1);
        }

        [TestMethod]
        public void ParseExample2_TextWithUnits()
        {
            CAttributes attributes = CHtmlParser.ParseAttributes(HtmlExamples.HtmlExample2);
            CAttribute attribute = attributes["Y00T00235.Y00A02875"];

            Assert.IsTrue(attribute.Description == "Units/time");
            Assert.IsTrue(attribute.Value == "");
            Assert.IsTrue(attribute.Options.Where(x => x.Key == "ppm").Count() == 1);
            Assert.IsTrue(attribute.Options.Where(x => x.Key == "1/1").Count() == 1);
            Assert.IsTrue(attribute.Options.Where(x => x.Key == "%").Count() == 1);
            Assert.IsTrue(attribute.Options.Where(x => x.Key == "‰").Count() == 1);

            Assert.IsTrue(attribute.Options["ppm"] == "F25.AA100");
            Assert.IsTrue(attribute.Options["1/1"] == "F25.AA110");
            Assert.IsTrue(attribute.Options["%"] == "F25.AA120");
            Assert.IsTrue(attribute.Options["‰"] == "F25.AA130");

            Assert.IsTrue(attribute.Unit == "%");
        }

        [TestMethod]
        public void Parse_HtmlExample_OptionsDuplicate()
        {
            CAttributes attributes = CHtmlParser.ParseAttributes(HtmlExamples.HtmlExampleOptionsDuplicate);
            CAttribute attribute = attributes["Y00T00001.Y00A03066"];

            Assert.IsTrue(attribute.Description == "Object class");
            Assert.IsTrue(attribute.Value == "Boundary stream");

            // Process stream is defined 2 times in this example of coos web.
            // Make sure it exists only once.
            Assert.IsTrue(attribute.Options.Where(x => x.Key == "Process stream").Count() == 1);
            Assert.IsTrue(attribute.Options["Process stream"] == "Process stream");
        }

        [TestMethod]
        public void Parse_HtmlExample_TextArea()
        {
            CAttributes attributes = CHtmlParser.ParseAttributes(HtmlExamples.HtmlExample_TextArea);
            CAttribute attribute = attributes["Y00T00156.Y00A00171"];

            Assert.IsTrue(attribute.Description == "Binding norms and standards");
            Assert.IsTrue(attribute.Value == "");
        }

        [TestMethod]
        public void Parse_HtmlExampleUI()
        {
            var attributes = CHtmlParser.ParseAttributesForUI(HtmlExamples.HtmlExample1);
            Assert.IsTrue(attributes.Count() == 3);
            attributes = CHtmlParser.ParseAttributesForUI(HtmlExamples.HtmlExample2);
            Assert.IsTrue(attributes.Count() == 1);
            attributes = CHtmlParser.ParseAttributesForUI(HtmlExamples.HtmlExampleOptionsDuplicate);
            Assert.IsTrue(attributes.Count() == 1);
            attributes = CHtmlParser.ParseAttributesForUI(HtmlExamples.HtmlExample_TextArea);
            Assert.IsTrue(attributes.Count() == 3);
            attributes = CHtmlParser.ParseAttributesForUI(HtmlExamples.HtmlExample3);
            Assert.IsTrue(attributes.Count() == 2);
        }
    }
}
