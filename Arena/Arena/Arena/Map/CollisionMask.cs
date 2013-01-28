using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arena.Map
{
    public class CollisionMask
    {
        /* Contains a list of rectangles that define collisions in a map */
        public List<Rectangle> rectangles;
        public List<RectangleOverlay> debug_rectangle_overlays;

        public CollisionMask()
        {
            rectangles = new List<Rectangle>();
            /* these are used to display collision masks in game for debugging */
            debug_rectangle_overlays = new List<RectangleOverlay>();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            /* only draw if you want debug overlays */

            foreach (RectangleOverlay rect in debug_rectangle_overlays)
            {
                rect.Draw(spriteBatch);
            }
        }
    }
}
