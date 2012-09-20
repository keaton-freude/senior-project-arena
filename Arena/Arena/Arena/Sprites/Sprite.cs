using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arena.Sprites
{
    /* Sprites have a texture, position and scale */
    /* Sprites have methods to draw and update (override update to add behavior) */
    /* Sprites are NOT animated (override to get that behavior) */
    public class Sprite
    {
        private Texture2D _texture;
        protected Rectangle? _src_rectangle;
        private float _scale;

        public Vector2 Position
        {
            get;
            set;
        }

        public Sprite()
        {
        }

        public Sprite(Texture2D tex, Rectangle? src_rectangle, Vector2 position, float scale)
        {
            _texture = tex;
            _src_rectangle = src_rectangle;
            _scale = scale;
            Position = position;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, _src_rectangle, Color.White, 0.0f, Vector2.Zero, _scale, SpriteEffects.None, 0.0f);
        }

        public virtual void Update(GameTime gameTime)
        {
            /* Override this to provide functionality */
        }
    }
}
