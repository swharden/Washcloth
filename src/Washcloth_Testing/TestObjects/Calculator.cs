using System;
using System.Collections.Generic;
using System.Text;

namespace Washcloth_Testing.TestObjects
{
    /// <summary>
    /// Advanced mathematical calculations
    /// </summary>
    public class Calculator
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

        /// <summary>
        /// Instantiate a calculator
        /// </summary>
        /// <param name="useFloatingMath">use floating point math</param>
        public Calculator(bool useFloatingMath = true)
        {

        }
    }
}
