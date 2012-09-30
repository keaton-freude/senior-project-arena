using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arena.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arena
{
    public class RunNJumpObstacle: Sprite
    {
        public Vector2 Velocity
        {
            get;
            set;
        }

        public bool OffScreen
        {
            get;
            private set;
        }

        public Rectangle BoundingRectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, (int)(_texture.Width * _scale), (int)(_texture.Height * _scale));
            }
        }

        public float Speed
        {
            get;
            set;
        }

        public RunNJumpObstacle(Texture2D texture, Rectangle? src_rectangle, Vector2 position, float scale)
            : base(texture, src_rectangle, position, scale)
        {
            Velocity = new Vector2(-1, 0);
            Speed = 1000.0f;
        }

        public override void Update(GameTime gameTime)
        {
            //All map obstacles move to the left of the screen

            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds * Speed;
            //Position += Velocity * Speed;

            if (BoundingRectangle.Right < 0)
            {
                //mark for deletion
                OffScreen = true;
            }

            

            //base.Update(gameTime);
        }
    }
}
