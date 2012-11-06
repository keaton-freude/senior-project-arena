using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arena.Sprites
{
    public class TargetShootingTarget: Sprite
    {
        public Vector2 Direction
        {
            get;
            set;
        }

        public Vector2 Speed
        {
            get;
            set;
        }

        public bool Destroyed
        {
            get;
            set;
        }

        public Vector2 Center
        {
            get
            {
                return new Vector2(Position.X + (_scale * Radius), Position.Y + (_scale * Radius));
            }
        }

        public float Radius
        {
            get;
            set;
        }

        public TargetShootingTarget(Texture2D tex, Rectangle? src_rectangle, Vector2 position, float scale, Vector2 direction, Vector2 speed, float radius)
            :base(tex, src_rectangle, position, scale)
        {
            Radius = radius;
            Direction = direction;
            Speed = speed;
        }

        public override void Update(GameTime gameTime)
        {
            Position += Direction * Speed;

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
