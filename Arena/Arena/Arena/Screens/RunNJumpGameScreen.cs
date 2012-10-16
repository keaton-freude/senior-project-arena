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
        ParticleEngine.ParticleEngine particleEngine;

        Players.RunNJumpPlayer player;

        bool colliding = false;

        RectangleOverlay rect;
        
        String test_sprite_rect = "";

        public int Counter = 0;

        Texture2D test_alpha;
        List<RunNJumpPlayer> _players = new List<RunNJumpPlayer>();
        RunNJumpNinja test_sprite;
        Texture2D _game_background;
        List<PlayerIndex> PlayerIndexes = new List<PlayerIndex>();

        public string GameState = "Pregame";

        RunNJumpObstacle test_obstacle;
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

            test_sprite = new RunNJumpNinja(content.Load<Texture2D>(@"blue_ninja_new"), new Rectangle(0, 0, 64, 64), new Vector2(100, _game_map.GroundY - (64 * 2.0f)), 2.0f, 0, 4, .08f, new Point(64, 64), ScreenManager.Game.Content.Load<Texture2D>(@"ParticleTextures\Dirt3"), ScreenManager.GraphicsDevice);
            foreach (PlayerIndex PI in PlayerIndexes)
            {
                _players.Add(new RunNJumpPlayer(PI, content.Load<Texture2D>(@"blue_ninja_new"),
                    _game_map.GroundY - (int)(64 * 2.0f), ScreenManager.Game.Content.Load<Texture2D>(@"ParticleTextures\Dirt3"), ScreenManager.GraphicsDevice));
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
                    test_sprite.Update(gameTime);
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
                                players_to_remove.Add(player);
                            }
                        }

                        foreach (RunNJumpPlayer p in players_to_remove)
                            _players.Remove(p);

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

                if (elapsed_time >= 10.0f && GameState == "Pregame")
                {
                    GameState = "Playing";
                    _game_map.StartSpawning();
                }


            }

            prevKeyboardState = Keyboard.GetState();
        }

        public override void Draw(GameTime gameTime)
        {
            if (GameState != "Done")
            {

                spriteBatch.Begin();
                spriteBatch.Draw(_game_background, new Vector2(0, -150), null, Color.White, 0.0f, Vector2.Zero, new Vector2(.75f, .5f), SpriteEffects.None, 1.0f);
                foreach (RunNJumpPlayer player in _players)
                {
                    player.Draw(spriteBatch);
                }
                _game_map.Draw(spriteBatch);

                if (colliding)
                    spriteBatch.DrawString(font, "Colliding", new Vector2(10, 10), Color.White);
                else
                    spriteBatch.DrawString(font, "Not Colliding", new Vector2(10, 10), Color.White);

                if (GameState == "Pregame")
                {
                    spriteBatch.DrawString(gameFont, String.Format("{0} . . . ", 10 - (int)elapsed_time), new Vector2(300, 300), Color.Green);
                }
                else
                    spriteBatch.DrawString(gameFont, GameState, new Vector2(100, 100), Color.Yellow);



                spriteBatch.DrawString(gameFont, Counter.ToString(), new Vector2(300, 300), Color.Yellow);

                spriteBatch.DrawString(font, test_sprite_rect, new Vector2(10, 40), Color.White);
                rect.Draw(spriteBatch);
                spriteBatch.End();
            }
            else
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(gameFont, WinString, new Vector2(200, 300), Color.Yellow, 0.0f, Vector2.Zero, 2.0f, SpriteEffects.None, 1.0f);
                spriteBatch.End();
            }
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
