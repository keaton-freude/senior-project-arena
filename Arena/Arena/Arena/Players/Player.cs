using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Arena
{
    /* This class provides a basic layer of information and functionality for local-xbox playeres */
    public class Player
    {
        public PlayerIndex Player_Index
        {
            get;
            set;
        }

        public GamePadState PrevGamepadState
        {
            get;
            set;
        }

        public KeyboardState PrevKeyboardState
        {
            get;
            set;
        }

        public Player(PlayerIndex player_index)
        {
            Player_Index = player_index;
        }

        public virtual void Update(GameTime gameTime)
        {
            /* Overload this function provide game-specific player controls */
            
            PrevGamepadState = GamePad.GetState(Player_Index);
            PrevKeyboardState = Keyboard.GetState();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            /* Overload this function provide game-specific rendering */
        }
    }
}
