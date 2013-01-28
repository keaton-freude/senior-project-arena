using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arena.Map
{


    public class MapCell
    {
        public int X
        {
            get;
            set;
        }

        public int Y
        {
            get;
            set;
        }

        public Texture2D texture
        {
            get;
            set;
        }

        public MapCell()
        {
        }

        public MapCell(Texture2D texture, int x, int y)
        {
            this.texture = texture;
            this.X = x;
            this.Y = y;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Vector2(X, Y), Color.White);   
        }
    }
}
