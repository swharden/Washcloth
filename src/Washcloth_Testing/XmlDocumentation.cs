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
    public class XmlDocumentation
    {
        [TestMethod]
        public void Test_HasComments_MethodInfo()
        {
            var docs = new Washcloth.XmlDocumentation("docFile.xml");

            foreach (MethodInfo mi in typeof(MathClass).GetMethods())
            {
                Console.WriteLine(mi);
                if (mi.Name == "CircleArea")
                {
                    string xml = docs.GetXml(mi);
                    Console.WriteLine(xml);
                    Assert.IsNotNull(xml);
                }
            }
        }

        [TestMethod]
        public void Test_HasComments_FromType()
        {
            var docs = new Washcloth.XmlDocumentation("docFile.xml");

            Type type = typeof(MathClass);
            string xml = docs.GetXml(type);
            Console.WriteLine(xml);
            Assert.IsNotNull(xml);
        }

        //[TestMethod]
        public void Test_MathComments_FromMethod()
        {
            var docs = new Washcloth.XmlDocumentation("docFile.xml");

            // WARNING: THIS DOES NOT WORK
            MethodInfo powerMethod = typeof(Math).GetMethod("Pow");
            string xml = docs.GetXml(powerMethod);
            Console.WriteLine(xml);
            Assert.IsNotNull(xml);
        }
    }
}
