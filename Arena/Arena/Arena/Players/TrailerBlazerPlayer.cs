using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Arena.Sprites;
using Microsoft.Xna.Framework.Graphics;

namespace Arena.Players
{
    public class TrailerBlazerPlayer: Player
    {
        public TrailBlazerPaddle paddle;
        public int score;
        SpriteFont font;

        public TrailerBlazerPlayer(PlayerIndex pi): base(pi)
        {
            paddle = new TrailBlazerPaddle(pi);
            font = Screens.TrailBlazer.content.Load<SpriteFont>(@"scorefont");
        }

        public override void Update(GameTime gameTime)
        {
            if (paddle.axis == "X")
            {
                paddle.Position += new Vector2(GamePad.GetState(Player_Index).ThumbSticks.Left.X * 10.0f, 0);
            }
            else
                paddle.Position += new Vector2(0, GamePad.GetState(Player_Index).ThumbSticks.Left.Y * 10.0f);

            if (Player_Index == PlayerIndex.Three)
            {
                if (paddle.axis == "X")
                {
                    paddle.Position += new Vector2(GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.X * 10.0f, 0);
                }
                else
                    paddle.Position += new Vector2(0, GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Y * 10.0f);
            }
            
            base.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            /* draw the score */
            Vector2 PositionOfText = Vector2.Zero;

            if (Player_Index == PlayerIndex.One)
            {
                PositionOfText = new Vector2(630, 695);
            }
            else if (Player_Index == PlayerIndex.Two)
            {
                PositionOfText = new Vector2(630, 0);
            }
            else if (Player_Index == PlayerIndex.Three)
            {
                PositionOfText = new Vector2(0, 720 / 2);
            }
            else if (Player_Index == PlayerIndex.Four)
            {
                PositionOfText = new Vector2(1280 - 25, 720 / 2);
            }

            spriteBatch.DrawString(font, String.Format("{0}", score), PositionOfText, Color.White);

            paddle.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }
    }
}
