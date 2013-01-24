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

        RectangleOverlay obstacle_rect;
        public bool OffScreen
        {
            get;
            private set;
        }

        public override Rectangle BoundingRectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, (int)(_texture.Width * _scale), (int)(_texture.Height * _scale));
            }
        }

        public Matrix TranslationC
        {
            get
            {
                return Matrix.CreateTranslation(new Vector3(Position, 0.0f));
            }
        }

        public float Speed
        {
            get;
            set;
        }

        public RunNJumpObstacle(Texture2D texture, Rectangle? src_rectangle, Vector2 position, float scale, GraphicsDevice gDevice)
            : base(texture, src_rectangle, position, scale)
        {
            Velocity = new Vector2(-1, 0);
            Speed = 800.0f;
            obstacle_rect = new RectangleOverlay(BoundingRectangle, Color.Green, gDevice);
            ColorData =
                new Color[texture.Width * texture.Height];
            texture.GetData(ColorData);
        }

        public override void Update(GameTime gameTime)
        {
            //All map obstacles move to the left of the screen

            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds * Speed;
            //Position += Velocity * Speed;
            obstacle_rect.dummyRectangle = BoundingRectangle;
            if (BoundingRectangle.Right < 0)
            {
                //mark for deletion
                OffScreen = true;
            }

            

            //base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //obstacle_rect.Draw(spriteBatch);
            //spriteBatch.DrawString(Screens.RunNJumpGameScreen.font, String.Format("X:{0}, Y:{1}, W:{2}, H:{3}", Position.X, Position.Y, _texture.Width, _texture.Height), new Vector2(Position.X, Position.Y - 85), Color.White);

            spriteBatch.Draw(this._texture, BoundingRectangle, Color.White);
            //spriteBatch.Draw(this._texture, Position, _src_rectangle, Color.White, _rotation, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
            //base.Draw(spriteBatch);
        }
    }
}
