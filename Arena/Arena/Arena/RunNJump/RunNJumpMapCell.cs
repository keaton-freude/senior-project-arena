using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arena
{
    public class RunNJumpMapCell
    {
        private Texture2D _texture;

        public Vector2 Position
        {
            get;
            set;
        }

        public RunNJumpMapCell(Texture2D texture, Vector2 position)
        {
            _texture = texture;
            Position = position;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, Color.White);
        }
    }
}
