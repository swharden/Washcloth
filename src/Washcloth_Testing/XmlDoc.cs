using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

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
    public class XmlDoc
    {
        [TestMethod]
        public void Test_HasComments_MethodInfo()
        {
            foreach (MethodInfo mi in typeof(MathClass).GetMethods())
            {
                Console.WriteLine(mi);
                if (mi.Name == "CircleArea")
                {
                    string xml = Washcloth.XmlDoc.GetText(mi);
                    Console.WriteLine(xml);
                    Assert.IsNotNull(xml);
                }
            }
        }

        [TestMethod]
        public void Test_HasComments_FromType()
        {
            Type type = typeof(MathClass);
            string xml = Washcloth.XmlDoc.GetText(type);
            Console.WriteLine(xml);
            Assert.IsNotNull(xml);
        }

        //[TestMethod]
        public void Test_MathComments_FromMethod()
        {
            // WARNING: THIS DOES NOT WORK
            MethodInfo powerMethod = typeof(Math).GetMethod("Pow");
            string xml = Washcloth.XmlDoc.GetText(powerMethod);
            Console.WriteLine(xml);
            Assert.IsNotNull(xml);
        }

        [TestMethod]
        public void Test_GetSummary_Method()
        {
            MethodInfo mi = typeof(MathClass).GetMethod("CircleArea");
            Assert.AreEqual("Calculate area of a circle", Washcloth.XmlDoc.GetSummary(mi));
        }

        [TestMethod]
        public void Test_GetSignature_Method()
        {
            MethodInfo mi = typeof(MathClass).GetMethod("CircleArea");
            string knownSignature = "public double CircleArea(double radius = 123)";
            string testSignature = Washcloth.XmlDoc.GetSignature(mi);
            Assert.AreEqual(knownSignature, testSignature);
        }

        [TestMethod]
        public void Test_GetSignature_Class()
        {
            Type type = typeof(MathClass);
            string knownSignature = "Washcloth_Testing.MathClass";
            string testSignature = Washcloth.XmlDoc.GetSignature(type);
            Assert.AreEqual(knownSignature, testSignature);
        }
    }
}
