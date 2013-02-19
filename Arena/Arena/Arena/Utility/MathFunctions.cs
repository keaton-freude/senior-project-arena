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

        public static void CircleRectangleReflection(ref Vector2 circle_velocity, int direction)
        {
            /* Then, we swap the y-values of circle_velocity */
            if (direction == 0)
            {
                circle_velocity.Y *= -1;
            }
            else if (direction == 1)
            {
                circle_velocity.X *= -1;
            }
        }

        public static void RectRectPaddleReflection(ref Vector2 circle_velocity, Rectangle r1, Rectangle r2, int modifier)
        {
            //r1 = paddle, r2 = ball

            float percent = 0.0f;
            if (modifier == 0)
            {
                int val1 = 0;
                if (r2.Left < r1.Left)
                    val1 = r1.Left;
                else
                    val1 = r2.Left;

                percent = (float)(val1 - r2.Left) / (float)r2.Width;
            }
            else
            {
                int val1 = 0;
                if (r2.Top < r1.Top)
                    val1 = r1.Top;
                else
                    val1 = r2.Top;
                percent = (float)(val1 - r2.Top) / (float)r2.Height;
            }



                //if (percent > .90f) //Special case for bounce to right 45 degrees
                //{
                //    if (circle_velocity.X < 0)
                //    {
                //        circle_velocity.X *= -1;
                //        circle_velocity.Y *= -1;
                //    }
                //    else
                //        circle_velocity.Y *= -1;
                //}
                //else if (percent < .1f) //Special case for bounce to left 45 degrees
                //{
                //    if (circle_velocity.X > 0)
                //    {
                //        circle_velocity.X *= -1;
                //        circle_velocity.Y *= -1;
                //    }
                //    else
                //        circle_velocity.Y *= -1;
                //}
                //else
                //{
                //    /* bounce by some amount */
                //}
                if (modifier == 0)
                {
                    float x_value = MathHelper.Lerp(-.75f, .75f, percent);
                    circle_velocity.X = x_value;
                    circle_velocity.Y *= -1;
                    circle_velocity.Normalize();
                }
                else
                {
                    float y_value = MathHelper.Lerp(-.75f, .75f, percent);
                    circle_velocity.Y = y_value;
                    circle_velocity.X *= -1;
                    circle_velocity.Normalize();
                }
        }   

        public static void TriangleRectangleReflection(ref Vector2 circle_velocity, int direction)
        {
            
            circle_velocity.X *= -1;
            circle_velocity.Y *= -1;

            /* and then swap the the values */

            float temp = circle_velocity.X;
            circle_velocity.Y = circle_velocity.X;
            circle_velocity.X = temp;
        }
    }
}
