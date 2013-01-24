using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arena.Screens;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Arena.Screens
{
    class TestGameScreen : GameScreen
    {
        ContentManager content;
        SpriteBatch spriteBatch;

        KeyboardState prevKeyboardState;

        Texture2D player_texture;
        Vector2 player_position = new Vector2(100, 400 - 64);
        Vector2 jump_direction = new Vector2(0, -1);
        Texture2D ground_texture;
        Vector2 gravity = new Vector2(0, .5f);
        Vector2 velocity = new Vector2(0, 0);

        int GroundY = 400;


        public TestGameScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            spriteBatch = new SpriteBatch(ScreenManager.GraphicsDevice);
            ground_texture = content.Load<Texture2D>(@"MapTiles\GrassTile");
            player_texture = content.Load<Texture2D>(@"blue_ninja_new");
            ScreenManager.Game.ResetElapsedTime();
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
                if (Keyboard.GetState().IsKeyDown(Keys.Space) && prevKeyboardState.IsKeyUp(Keys.Space))
                {

                }
                if (prevKeyboardState.IsKeyDown(Keys.Space) && Keyboard.GetState().IsKeyUp(Keys.Space))
                {

                }


            }

            prevKeyboardState = Keyboard.GetState();
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice device = ScreenManager.GraphicsDevice;

            device.Clear(Color.Black);
            spriteBatch.Begin();

            for (int i = 0; i < 10; ++i)
            {
                spriteBatch.Draw(ground_texture, new Vector2(i * 64, GroundY), Color.White);
            }

            spriteBatch.Draw(player_texture, player_position, new Rectangle(0, 0, 64, 64), Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);

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
