using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arena.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Arena.Players
{
    public class TargetShootingPlayer: Player
    {
        public TargetShootingCrosshair Crosshair
        {
            get;
            set;
        }

        public TargetShootingPlayer(PlayerIndex player_index, ContentManager content) :
            base(player_index)
        {
            Color color = Color.White;
            switch (player_index)
            {
                case PlayerIndex.One:
                    color = Color.Red;
                    break;
                case PlayerIndex.Two:
                    color = Color.Blue;
                    break;
                case PlayerIndex.Three:
                    color = Color.Green;
                    break;
                case PlayerIndex.Four:
                    color = Color.Orange;
                    break;
            }
            Crosshair = new TargetShootingCrosshair(content.Load<Texture2D>(@"Crosshairs/circle-5"),
                null, Vector2.Zero, .4f, 128.0f, color);
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime, List<TargetShootingTarget> targets)
        {
            Crosshair.Position = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

            //check player input (A firing action)

            if (PrevMouseState.LeftButton == ButtonState.Pressed && Mouse.GetState().LeftButton == ButtonState.Released)
            {
                //Shot fired!
                foreach (TargetShootingTarget target in targets)
                {
                //Check collision
                    if (Collision.CollisionDetection.CircleCircleCollision(Crosshair.Center, Crosshair.Radius * Crosshair.Scale,
                        target.Center, target.Radius * target.Scale))
                    {
                        target.Destroyed = true;
                    }
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            Crosshair.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }
    }
}
