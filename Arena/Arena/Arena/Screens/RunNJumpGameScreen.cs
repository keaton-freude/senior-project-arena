using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Arena.Sprites;
using Arena.ParticleEngine;

namespace Arena.Screens
{
    class RunNJumpGameScreen : GameScreen
    {
        ContentManager content;
        SpriteBatch spriteBatch;
        float pauseAlpha; 
        KeyboardState prevKeyboardState;
        ParticleEngine.ParticleEngine particleEngine;

        Texture2D test_alpha;

        RunNJumpNinja test_sprite;
        Texture2D _game_background;

        RunNJumpObstacle test_obstacle;

        Texture2D test;

        RunNJumpMap _game_map;


        public RunNJumpGameScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            
        }

        

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            spriteBatch = new SpriteBatch(ScreenManager.GraphicsDevice);
            test = content.Load<Texture2D>(@"blackbox");

            //List<Texture2D> dirt_textures = new List<Texture2D>();
            //dirt_textures.Add(ScreenManager.Game.Content.Load<Texture2D>(@"ParticleTextures\Dirt2"));
            //particleEngine = new ParticleEngine.ParticleEngine(dirt_textures, new Vector2(400, 240));
            test_alpha = content.Load<Texture2D>(@"obstacle_alpha");
            test_obstacle = new RunNJumpObstacle(content.Load<Texture2D>(@"obstacle_alpha"), null, new Vector2(300, 300), 2.0f);
            test_sprite = new RunNJumpNinja(content.Load<Texture2D>(@"blue_ninja"), new Rectangle(0, 0, 64, 64), new Vector2(100, 336), 2.0f, 0, 4, .08f, new Point(64, 64), ScreenManager.Game.Content.Load<Texture2D>(@"ParticleTextures\Dirt3"));
            _game_map = new RunNJumpMap(content.Load<Texture2D>(@"MapTiles\DirtTile"), content.Load<Texture2D>(@"MapTiles\GrassTile"), content.Load<Texture2D>(@"obstacle_alpha"));
            _game_background = content.Load<Texture2D>(@"sky1");
            ScreenManager.Game.ResetElapsedTime();
        }

        public override void UnloadContent()
        {
            content.Unload();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

            if (IsActive)
            {
                //Update Code Here
                test_sprite.Update(gameTime);
                _game_map.Update(gameTime);

                float speed = 3.0f;

                if (Keyboard.GetState().IsKeyDown(Keys.Space) && test_sprite.Sliding == false && test_sprite.JumpCooldown == 0.0f)
                    test_sprite.Jumping = true;
                if (Keyboard.GetState().IsKeyDown(Keys.C))
                    test_sprite.Sliding = true;
                if (Keyboard.GetState().IsKeyUp(Keys.C))
                    test_sprite.Sliding = false;
                if (Keyboard.GetState().IsKeyDown(Keys.Left) && test_sprite.Sliding == false)
                    test_sprite.Position += new Vector2(-1, 0) * speed;
                if (Keyboard.GetState().IsKeyDown(Keys.Right) && test_sprite.Sliding == false)
                    test_sprite.Position += new Vector2(1, 0) * speed;

                test_obstacle.Update(gameTime);
                //particleEngine.EmitterLocation = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
                //particleEngine.Update();
            }

            prevKeyboardState = Keyboard.GetState();
        }

        public override void Draw(GameTime gameTime)
        {
            //ScreenManager.GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin();
            spriteBatch.Draw(_game_background, new Vector2(0, -150), null, Color.White, 0.0f, Vector2.Zero, new Vector2(.75f, .5f), SpriteEffects.None, 1.0f);
            test_sprite.Draw(spriteBatch);
            _game_map.Draw(spriteBatch);
            test_obstacle.Draw(spriteBatch);
            //particleEngine.Draw(spriteBatch);
            spriteBatch.End();
        }

        #region Fade Screen Logic
        private void FadeScreen()
        {
            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }
        #endregion

        #region Handles pausing and exit screen logic
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
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
        }
        #endregion
    }
}
