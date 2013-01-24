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
        protected Color[] _color_data;

        /* Certain sub-classes might treat their BoundingRectangles differently
         * such as offsetting, or class-specific logic.
         * OVerride this property to receive that benefit
         */
        public virtual Rectangle BoundingRectangle
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, 
                (int)(_src_rectangle.Value.Width * _scale), (int)(_src_rectangle.Value.Height * _scale)); }
        }

        public float Rotation
        {
            get
            {
                return _rotation;
            }
            set
            {
                _rotation = value;
            }
        }

        public Color[] ColorData
        {
            get
            {
                return _color_data;
            }
            set
            {
                _color_data = value;
            }
        }

        public Vector2 Position
        {
            get;
            set;
        }

        public float Scale
        {
            get
            {
                return _scale;
            }

            set
            {
                _scale = value;
            }
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
            _color_data =
                new Color[tex.Width * tex.Height];
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, _src_rectangle, Color.White, _rotation, Vector2.Zero, _scale, SpriteEffects.None, 0.0f);
        }

        public virtual void Update(GameTime gameTime)
        {
            /* Override this to provide functionality */
        }
    }
}
