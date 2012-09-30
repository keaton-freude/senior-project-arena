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
        protected Texture2D _texture;
        protected Rectangle? _src_rectangle;
        protected float _scale;
        protected float _rotation;

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
            if (_src_rectangle == null)
                _src_rectangle = new Rectangle(0, 0, tex.Width, tex.Height);
            else
                _src_rectangle = src_rectangle;
            _scale = scale;
            _rotation = 0.0f;
            Position = position;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, _src_rectangle, Color.White, _rotation, new Vector2(_src_rectangle.Value.Width / 2, _src_rectangle.Value.Height / 2), _scale, SpriteEffects.None, 0.0f);
        }

        public virtual void Update(GameTime gameTime)
        {
            /* Override this to provide functionality */
        }
    }
}
