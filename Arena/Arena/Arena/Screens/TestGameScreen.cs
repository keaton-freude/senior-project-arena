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

        public TestGameScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            
        }
        List<RectangleOverlay> rectangles;
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            spriteBatch = new SpriteBatch(ScreenManager.GraphicsDevice);
            ScreenManager.Game.ResetElapsedTime();
            rectangles = new List<RectangleOverlay>();
            rectangles.Add(new RectangleOverlay(new Rectangle(0, 200, 50, 120), Color.Green, ScreenManager.GraphicsDevice));
            rectangles.Add(new RectangleOverlay(new Rectangle(0, 420, 50, 120), Color.Green, ScreenManager.GraphicsDevice));
            rectangles.Add(new RectangleOverlay(new Rectangle(200, 690, 380, 50), Color.Green, ScreenManager.GraphicsDevice));
            rectangles.Add(new RectangleOverlay(new Rectangle(700, 690, 380, 50), Color.Green, ScreenManager.GraphicsDevice));
            rectangles.Add(new RectangleOverlay(new Rectangle(1230, 420, 50, 120), Color.Green, ScreenManager.GraphicsDevice));
            rectangles.Add(new RectangleOverlay(new Rectangle(1230, 200, 50, 120), Color.Green, ScreenManager.GraphicsDevice));
            rectangles.Add(new RectangleOverlay(new Rectangle(200, 0, 380, 50), Color.Green, ScreenManager.GraphicsDevice));
            rectangles.Add(new RectangleOverlay(new Rectangle(700, 0, 380, 50), Color.Green, ScreenManager.GraphicsDevice));
            triangle_lib = new Utility.TriangleOverlay(ScreenManager.GraphicsDevice);
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


        Arena.Utility.TriangleOverlay triangle_lib;

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice device = ScreenManager.GraphicsDevice;

            device.Clear(Color.Black);
            spriteBatch.Begin();
            triangle_lib.Triangle(new Vector2(50, 50), new Vector2(200, 50), new Vector2(50, 200), 1, Color.Green, spriteBatch);
            triangle_lib.Triangle(new Vector2(50, 540), new Vector2(200, 690), new Vector2(50, 690), 1, Color.Green, spriteBatch);
            triangle_lib.Triangle(new Vector2(1080, 690), new Vector2(1230, 690), new Vector2(1230, 690-150), 1, Color.Green, spriteBatch);
            triangle_lib.Triangle(new Vector2(1230, 200), new Vector2(1230, 50), new Vector2(1080, 50), 1, Color.Green, spriteBatch);
            foreach (RectangleOverlay rect in rectangles)
            {
                rect.Draw(spriteBatch);
            }
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
