using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arena.Utility
{
    public class MathFunctions
    {
        public static float RandomFromRange(float min, float max, Random random)
        {
            return (float)random.NextDouble() * (max - min + 1) + min;
        }
    }
}
