using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Arena.Sprites
{
    public class TrailBlazerPaddle: Sprite
    {
        public string axis = "None";
        public Color ballColor = Color.White;

        

        public TrailBlazerPaddle(PlayerIndex playerIndex): base(Screens.TrailBlazer.content.Load<Texture2D>(@"paddle"), new Rectangle(0, 0, 92, 10), Vector2.Zero, 1.0f)
        {
            /* If Player 1 then RED and bottom goal */
            if (playerIndex == PlayerIndex.One)
            {
                axis = "X"; //locked to Y axis, moves along X
                ballColor = Color.Red;
                Position = new Vector2(580, 680);
            }
            else if (playerIndex == PlayerIndex.Two)
            {
                axis = "X";
                ballColor = Color.Green;
                Position = new Vector2(380, 40);
            }
            else if (playerIndex == PlayerIndex.Three)
            {
                axis = "Y";
                this._texture = Screens.TrailBlazer.content.Load<Texture2D>(@"paddle2");
                _src_rectangle = new Rectangle(0, 0, 10, 92);
                ballColor = Color.Yellow;
                Position = new Vector2(40, 300);
            }
            else if (playerIndex == PlayerIndex.Four)
            {
                axis = "Y";
                this._texture = Screens.TrailBlazer.content.Load<Texture2D>(@"paddle2");
                _src_rectangle = new Rectangle(0, 0, 10, 92);
                ballColor = Color.Blue;
                Position = new Vector2(1240, 300);
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, _src_rectangle, ballColor, _rotation, Vector2.Zero, _scale, SpriteEffects.None, 0.0f);
        }
    }
}
