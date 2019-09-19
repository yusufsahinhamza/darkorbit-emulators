using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Ow.Utils
{
    public class Maths
    {
        public static int GetPercentage(int a, int b)
        {
            return a * b / 100;
        }

        public static double GetDoublePercentage(double a, double b)
        {
            return a * b / 100;
        }

        public static double Sqr(double pValue)
        {
            return Math.Pow(pValue, 2);
        }

        public static double Hypotenuse(double pX, double pY)
        {
            return Math.Sqrt(Sqr(pX) + Sqr(pY));
        }
    }
}
