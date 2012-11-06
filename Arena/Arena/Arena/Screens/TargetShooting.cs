using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Arena.Players;
using Arena.TargetShooting;

namespace Arena.Screens
{
    public class TargetShooting : GameScreen
    {
        ContentManager content;
        SpriteBatch spriteBatch;

        TargetShootingPlayer player;

        KeyboardState prevKeyboardState;
        MouseState prevMouseState;

        TargetManager target_manager;

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

            player = new TargetShootingPlayer(PlayerIndex.One, content);
            target_manager = new TargetManager(content);
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
                player.Update(gameTime, target_manager.Targets);
                target_manager.Update(gameTime);
            }

            prevKeyboardState = Keyboard.GetState();
            prevMouseState = Mouse.GetState();
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice device = ScreenManager.GraphicsDevice;
            device.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            target_manager.Draw(spriteBatch);
            player.Draw(spriteBatch);


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
