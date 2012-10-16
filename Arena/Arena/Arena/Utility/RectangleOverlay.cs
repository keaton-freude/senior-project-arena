using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arena
{
    public class RectangleOverlay
    {
        Texture2D dummyTexture;
        public Rectangle dummyRectangle;
        Color Colori;

        public RectangleOverlay()
        {
        }

        public RectangleOverlay(Rectangle rect, Color colori, GraphicsDevice gDevice)
        {
            dummyTexture = new Texture2D(gDevice, 1, 1);
            dummyTexture.SetData(new Color[] { Color.White });
            dummyRectangle = rect;
            Colori = colori;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(dummyTexture, dummyRectangle, Colori);

            spriteBatch.Draw(dummyTexture, new Rectangle(dummyRectangle.Left, dummyRectangle.Top, dummyRectangle.Width, 1), Colori);
            spriteBatch.Draw(dummyTexture, new Rectangle(dummyRectangle.Left, dummyRectangle.Bottom, dummyRectangle.Width, 1), Colori);
            spriteBatch.Draw(dummyTexture, new Rectangle(dummyRectangle.Left, dummyRectangle.Top, 1, dummyRectangle.Height), Colori);
            spriteBatch.Draw(dummyTexture, new Rectangle(dummyRectangle.Right, dummyRectangle.Top, 1, dummyRectangle.Height + 1), Colori);
        }
    }
}
