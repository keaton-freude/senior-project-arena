using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arena.Utility
{
    class TriangleOverlay
    {
        private Texture2D pixel;

        public TriangleOverlay(GraphicsDevice gDevice)
        {
            pixel = new Texture2D(gDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
        }

        public void SetPixel(Vector2 pos, int weight, Color color, SpriteBatch spriteBatch)
        {
            SetPixel((int)pos.X, (int)pos.Y, weight, color, spriteBatch);
        }

        public void SetPixel(int x, int y, int weight, Color color, SpriteBatch spriteBatch)
        {
            int b = weight / 2;
            spriteBatch.Draw(pixel, new Rectangle(x - b, y - b, weight, weight), color);
        }

        public void SetPixel(float x, float y, float weight, Color color, SpriteBatch spriteBatch)
        {
            SetPixel((int)x, (int)y, (int)weight, color, spriteBatch);
        }

        public void Line(Vector2 from, Vector2 to, int weight, Color color, SpriteBatch spriteBatch)
        {
            Line((int)from.X, (int)from.Y, (int)to.X, (int)to.Y, weight, color, spriteBatch);
        }
        public void Line(int x1, int y1, int x2, int y2, int weight, Color color, SpriteBatch spriteBatch)
        {

            // Digital Differential Analyser (DDA) Algorithm
            int dx = Math.Abs(x2 - x1);
            int dy = Math.Abs(y2 - y1);
            int steps = dy;
            if (dx > dy) steps = dx;
            float sx = (float)(x2 - x1) / (float)steps;
            float sy = (float)(y2 - y1) / (float)steps;

            float x = x1;
            float y = y1;
            SetPixel(x1, y1, weight, color, spriteBatch);
            for (int i = 0; i < steps; i++)
            {
                x += sx;
                y += sy;
                SetPixel((int)x, (int)y, weight, color, spriteBatch);
            }
        }
        public void Line(float x1, float y1, float x2, float y2, float weight, Color color, SpriteBatch spriteBatch)
        {
            Line(new Vector2(x1, y1), new Vector2(x2, y2), (int)weight, color, spriteBatch);
        }

        public void Triangle(Vector2 p1, Vector2 p2, Vector2 p3, int weight, Color color, SpriteBatch spriteBatch)
        {
            Line(p1, p2, weight, color, spriteBatch);
            Line(p1, p3, weight, color, spriteBatch);
            Line(p2, p3, weight, color, spriteBatch);
        }
        public void Triangle(int x1, int y1, int x2, int y2, int x3, int y3, int weight, Color color, SpriteBatch spriteBatch)
        {
            // Draw three lines
            Line(x1, y1, x2, y2, weight, color, spriteBatch);
            Line(x1, y1, x3, y3, weight, color, spriteBatch);
            Line(x2, y2, x3, y3, weight, color, spriteBatch);
        }
        public void Triangle(float x1, float y1, float x2, float y2, float x3, float y3, float weight, Color color, SpriteBatch spriteBatch)
        {
            Triangle((int)x1, (int)y1, (int)x2, (int)y2, (int)x3, (int)y3, (int)weight, color, spriteBatch);
        }
    }
}
