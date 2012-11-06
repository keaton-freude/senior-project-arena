using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Arena.Sprites
{
    public class TargetShootingCrosshair: Sprite
    {

        public float Radius
        {
            get;
            set;
        }

        public Color CrosshairColor
        {
            get;
            set;
        }

        public Vector2 Center
        {
            get
            {
                return new Vector2(Position.X - (Radius * _scale), Position.Y - (Radius * _scale));
            }
        }

        public TargetShootingCrosshair(Texture2D texture, Rectangle? src_rectangle, Vector2 position, float scale,
            float radius, Color crosshair_color) : base(texture, src_rectangle, position, scale)
        {
            Radius = radius;
            CrosshairColor = crosshair_color;
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, new Vector2(Position.X - (Radius * _scale), Position.Y - (Radius * _scale)), _src_rectangle, CrosshairColor, 0.0f, Vector2.Zero, _scale, SpriteEffects.None, 1.0f);
        }
    }
}
