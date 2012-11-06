using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Arena.Screens
{
    public class TargetShooting : GameScreen
    {
        ContentManager content;
        SpriteBatch spriteBatch;

        Texture2D targ_texture;
        Vector2 targ_center = new Vector2(300, 300);

        bool targ_destroyed = false;
        KeyboardState prevKeyboardState;
        MouseState prevMouseState;

        public TargetShooting(List<PlayerIndex> _player_indexes)
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            
            
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            spriteBatch = new SpriteBatch(ScreenManager.GraphicsDevice);
            targ_texture = content.Load<Texture2D>(@"Target");
        }

        public override void UnloadContent()
        {
            content.Unload();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);


            if (IsActive)
            {
                if (Mouse.GetState().LeftButton == ButtonState.Released && prevMouseState.LeftButton == ButtonState.Pressed)
                {
                    if (Collision.CollisionDetection.CircleCircleCollision(new Vector2(Mouse.GetState().X, Mouse.GetState().Y),
                        1.0f, targ_center, 64.0f))
                    {
                        targ_destroyed = true;
                    }
                }
            }

            prevKeyboardState = Keyboard.GetState();
            prevMouseState = Mouse.GetState();
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice device = ScreenManager.GraphicsDevice;
            device.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            if (!targ_destroyed)
                spriteBatch.Draw(targ_texture, new Vector2(targ_center.X - (128 / 2), targ_center.Y - (128 / 2)), Color.Green);
            spriteBatch.End();

            
        }

        public override void HandleInput(InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
            GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected &&
                                       input.GamePadWasConnected[playerIndex];

            if (input.IsPauseGame(ControllingPlayer) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(null), ControllingPlayer);
            }
        }
    }
}
