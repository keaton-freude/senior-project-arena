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
using Arena.Players;

namespace Arena.Screens
{
    class RunNJumpGameScreen : GameScreen
    {
        ContentManager content;
        SpriteBatch spriteBatch;
        float pauseAlpha; 
        KeyboardState prevKeyboardState;


        RectangleOverlay rect;

        int frame_count = 0;
        
        //String test_sprite_rect = "";

        private const bool DEBUG_MODE = true;

        public int Counter = 0;
        float frame_elapsed = 0.0f;
        Texture2D test_alpha;
        List<RunNJumpPlayer> _players = new List<RunNJumpPlayer>();

        Texture2D _game_background;
        List<PlayerIndex> PlayerIndexes = new List<PlayerIndex>();

        public string GameState = "Pregame";

        public static SpriteFont font;

        public string WinString = "";

        SpriteFont gameFont;

        float elapsed_time = 0.0f;
        Texture2D test;

        public RunNJumpMap _game_map;


        public RunNJumpGameScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            
        }

        public RunNJumpGameScreen(List<PlayerIndex> _player_indexes)
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            PlayerIndexes = _player_indexes;
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            int ninja_slide_effect_name = ArenaParticleEngine.ParticleEngine.Instance.LoadFromFile("SlidingDirt", content);
            spriteBatch = new SpriteBatch(ScreenManager.GraphicsDevice);
            test = content.Load<Texture2D>(@"blackbox");
            rect = new RectangleOverlay(new Rectangle(10, 10, 100, 100), Color.Green, ScreenManager.GraphicsDevice);
            //List<Texture2D> dirt_textures = new List<Texture2D>();
            //dirt_textures.Add(ScreenManager.Game.Content.Load<Texture2D>(@"ParticleTextures\Dirt2"));
            //particleEngine = new ParticleEngine.ParticleEngine(dirt_textures, new Vector2(400, 240));
            test_alpha = content.Load<Texture2D>(@"obstacle_alpha");
            //test_obstacle = new RunNJumpObstacle(content.Load<Texture2D>(@"obstacle_alpha"), null, new Vector2(300, 300), 2.0f);
            _game_map = new RunNJumpMap(content.Load<Texture2D>(@"MapTiles\DirtTile"), content.Load<Texture2D>(@"MapTiles\GrassTile"), content.Load<Texture2D>(@"obstacle_alpha"), ScreenManager.GraphicsDevice);
            _game_background = content.Load<Texture2D>(@"sky1");
            font = content.Load<SpriteFont>(@"SpriteFont1");
            gameFont = content.Load<SpriteFont>(@"GameStateFont");
            //player = new Players.RunNJumpPlayer(PlayerIndex.One, ScreenManager.Game.Content.Load<Texture2D>(@"blue_ninja_new"),
            //    _game_map.GroundY - (int)(64 * 2.0f), ScreenManager.Game.Content.Load<Texture2D>(@"ParticleTextures\Dirt3"), ScreenManager.GraphicsDevice);

            //test_sprite = new RunNJumpNinja(content.Load<Texture2D>(@"blue_ninja_new"), new Rectangle(0, 0, 64, 64), new Vector2(100, _game_map.GroundY - (64 * 2.0f)), 2.0f, 0, 4, .08f, new Point(64, 64), ScreenManager.Game.Content.Load<Texture2D>(@"ParticleTextures\Dirt3"), ScreenManager.GraphicsDevice);
            foreach (PlayerIndex PI in PlayerIndexes)
            {
                _players.Add(new RunNJumpPlayer(PI, content.Load<Texture2D>(@"blue_ninja_new"),
                    _game_map.GroundY - (int)(64 * 2.0f), ninja_slide_effect_name, ScreenManager.GraphicsDevice));
            }
            ScreenManager.Game.ResetElapsedTime();
            
        }

        public override void UnloadContent()
        {
            _game_map.CleanUp();
            content.Unload();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            if (GameState != "Done")
            {

                if (coveredByOtherScreen)
                    pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
                else
                    pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);
                elapsed_time += (float)gameTime.ElapsedGameTime.TotalSeconds;
                Counter++;
                List<RunNJumpPlayer> players_to_remove = new List<RunNJumpPlayer>();
                if (IsActive)
                {
                    //Update Code Here
                    //test_sprite.Update(gameTime);
                    if (GameState == "Playing")
                    {
                        _game_map.Update(gameTime);
                        //player.Update(gameTime, _game_map);

                        foreach (RunNJumpPlayer player in _players)
                        {
                            player.Update(gameTime, _game_map, GameState);

                            if (player.Collided)
                            {
                                //remove this player
                                if (!DEBUG_MODE)
                                    players_to_remove.Add(player);
                            }
                        }

                        foreach (RunNJumpPlayer p in players_to_remove)
                            _players.Remove(p);

                        //NO WIN CONDITION IN DEBUG MODE
                        if (!DEBUG_MODE)
                        {
                            if (_players.Count == 1)
                            {
                                //win statement
                                //determine winner
                                WinString = "Winner: Player " + _players[0].Player_Index.ToString();
                                GameState = "Done";
                            }
                            else if (_players.Count == 0)
                            {
                                //Draw
                                WinString = "Draw!";
                                GameState = "Done";
                            }
                        }
                    }
                }

                if (elapsed_time >= 5.0f && GameState == "Pregame")
                {
                    GameState = "Playing";
                    _game_map.StartSpawning();
                }


            }
            ArenaParticleEngine.ParticleEngine.Instance.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            //ParticleEngine.ParticleEngine.GetInstance().Update(gameTime);

            prevKeyboardState = Keyboard.GetState();
        }
        float fps = 0.0f;
        public override void Draw(GameTime gameTime)
        {
            frame_elapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
            frame_count++;
            if (frame_elapsed >= 1.0f)
            {
                fps = frame_count;
                frame_elapsed -= 1.0f;
                frame_count = 0;
            }

            if (GameState != "Done")
            {

                spriteBatch.Begin();
                spriteBatch.Draw(_game_background, new Vector2(0, -150), null, Color.White, 0.0f, Vector2.Zero, new Vector2(.75f, .5f), SpriteEffects.None, 1.0f);

                foreach (RunNJumpPlayer player in _players)
                {
                    player.Draw(spriteBatch);
                }
                _game_map.Draw(spriteBatch);

                if (GameState == "Pregame")
                {
                    spriteBatch.DrawString(gameFont, String.Format("{0} . . . ", Math.Round(5.0f - elapsed_time, 2).ToString()), new Vector2(300, 300), Color.Green, 0.0f, Vector2.Zero, MathHelper.Lerp(1.0f, 1.5f, (elapsed_time / 5.0f)), SpriteEffects.None, 1.0f);
                }
                else
                    spriteBatch.DrawString(gameFont, GameState, new Vector2(100, 100), Color.Yellow);

                spriteBatch.DrawString(gameFont, "FPS: " + fps.ToString(), new Vector2(0, 0), Color.Red);

                //spriteBatch.DrawString(font, test_sprite_rect, new Vector2(10, 40), Color.White);

                spriteBatch.End();
            }
            else
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(gameFont, WinString, new Vector2(200, 300), Color.Yellow, 0.0f, Vector2.Zero, 2.0f, SpriteEffects.None, 1.0f);
                spriteBatch.End();
            }

            ArenaParticleEngine.ParticleEngine.Instance.Draw(spriteBatch);

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
                _game_map.StopSpawning();
                //GameState = "Paused";
                ScreenManager.AddScreen(new PauseMenuScreen(this), ControllingPlayer);
            }

            
        }

        
        #endregion
    }
}
