using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arena.Utility
{
    public class MathFunctions
    {
        public static float RandomFromRange(float min, float max, Random random)
        {
            return (float)random.NextDouble() * (max - min + 1) + min;
        }

        public static float AngleBetweenVectors(Vector2 v1, Vector2 v2)
        {
            return (float)Math.Acos((v1.X * v2.X + v1.Y * v2.Y) / (float)(Math.Sqrt(Math.Pow(v1.X, 2) + Math.Pow(v1.Y, 2)) * (float)Math.Sqrt(Math.Pow(v2.X, 2) + Math.Pow(v2.Y, 2))));
        }

        public static Vector2 PointWithinRecetangle(Rectangle rect)
        {
            /* returns a random point that rect contains */
            Random random = new Random();
            int left = (int)Arena.Utility.MathFunctions.RandomFromRange(rect.Left, rect.Left + rect.Width, random);
            int top = (int)Arena.Utility.MathFunctions.RandomFromRange(rect.Top, rect.Top + rect.Height, random);

            return new Vector2(left, top);
        }
    }
}
