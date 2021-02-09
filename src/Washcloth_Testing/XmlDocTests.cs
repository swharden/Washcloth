using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Xml.Linq;
using Washcloth;

namespace Washcloth_Testing
{
    /// <summary>
    /// Advanced mathematical calculations
    /// </summary>
    public class MathClass
    {
        /// <summary>
        /// Calculate area of a circle
        /// </summary>
        /// <param name="radius">distance from center to an edge</param>
        /// <returns>area in original units squared</returns>
        public double CircleArea(double radius = 123)
        {
            return Math.PI * radius * radius;
        }
    }

    [TestClass]
    public class XmlDocTests
    {
        [TestMethod]
        public void Test_Member_FromMethodInfo()
        {
            foreach (MethodInfo mi in typeof(MathClass).GetMethods())
            {
                if (mi.Name == "CircleArea")
                {
                    var methodXml = XmlDoc.GetMember(mi);
                    Console.WriteLine(methodXml);
                    Assert.IsNotNull(methodXml);

                    foreach(ParameterInfo p in mi.GetParameters())
                    {
                        var paramXml = XmlDoc.GetParam(p);
                        Console.WriteLine(p);
                        Console.WriteLine(paramXml);
                        Assert.IsNotNull(paramXml);
                    }
                }
            }
        }

        [TestMethod]
        public void Test_Member_FromType()
        {
            Type type = typeof(MathClass);
            var xml = Washcloth.XmlDoc.GetMember(type);
            Console.WriteLine(xml);
            Assert.IsNotNull(xml);
        }

        //[TestMethod]
        public void Test_MathComments_FromMethod()
        {
            // WARNING: THIS DOES NOT WORK
            MethodInfo powerMethod = typeof(Math).GetMethod("Pow");
            XElement xml = XmlDoc.GetMember(powerMethod);
            Console.WriteLine(xml);
            Assert.IsNotNull(xml);
        }

        [TestMethod]
        public void Test_GetSummary_Method()
        {
            MethodInfo mi = typeof(MathClass).GetMethod("CircleArea");
            Assert.AreEqual("Calculate area of a circle", XmlDoc.GetSummary(mi));
        }

        [TestMethod]
        public void Test_GetSignature_Method()
        {
            MethodInfo mi = typeof(MathClass).GetMethod("CircleArea");
            string knownSignature = "public double CircleArea(double radius = 123)";
            string testSignature = XmlDoc.GetSignature(mi);
            Assert.AreEqual(knownSignature, testSignature);
        }

        [TestMethod]
        public void Test_GetSignature_Class()
        {
            Type type = typeof(MathClass);
            string knownSignature = "Washcloth_Testing.MathClass";
            string testSignature = XmlDoc.GetSignature(type);
            Assert.AreEqual(knownSignature, testSignature);
        }
    }
}
