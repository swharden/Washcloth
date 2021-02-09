using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Xml.Linq;

namespace Washcloth_Testing
{
    [TestClass]
    public class XmlComments
    {
        [TestMethod]
        public void Test_XmlDoc_ClassSummary()
        {
            Type type = typeof(TestObjects.Calculator);
            string summary = Washcloth.XmlDoc.GetSummary(type);
            Assert.AreEqual("Advanced mathematical calculations", summary);
        }

        [TestMethod]
        public void Test_XmlDoc_ClassConstructor()
        {
            ConstructorInfo info = typeof(TestObjects.Calculator).GetConstructors().First();
            string summary = Washcloth.XmlDoc.GetSummary(info);
            Assert.AreEqual("Instantiate a calculator", summary);
        }

        [TestMethod]
        public void Test_XmlDoc_MethodSummary()
        {
            MethodInfo info = typeof(TestObjects.Calculator).GetMethod("CircleArea");
            string summary = Washcloth.XmlDoc.GetSummary(info);
            Assert.AreEqual("Calculate area of a circle", summary);
        }

        [TestMethod]
        public void Test_XmlDoc_ParameterSummary()
        {
            MethodInfo mi = typeof(TestObjects.Calculator).GetMethod("CircleArea");
            ParameterInfo firstParameter = mi.GetParameters()[0];
            Assert.AreEqual("radius", firstParameter.Name);

            string summary = Washcloth.XmlDoc.GetSummary(firstParameter);
            Assert.AreEqual("distance from center to an edge", summary);
        }

        [TestMethod]
        public void Test_XmlDoc_Enum()
        {
            Type type = typeof(TestObjects.Shape);
            Assert.AreEqual("2D shape", Washcloth.XmlDoc.GetSummary(type));
        }

        [TestMethod]
        public void Test_XmlDoc_EnumValue()
        {
            // TODO: how do we get summary comments from enum values
            // https://github.com/loxsmoke/DocXml#classes-and-methods
        }
    }
}
