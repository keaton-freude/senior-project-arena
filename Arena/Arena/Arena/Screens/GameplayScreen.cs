#region File Description
//-----------------------------------------------------------------------------
// GameplayScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Arena.UI;
#endregion

namespace GameStateManagement
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    class GameplayScreen : GameScreen
    {
        #region Fields

        ContentManager content;
        SpriteFont gameFont;

        SpriteBatch spriteBatch;
        private List<GameIcon> _game_icons = new List<GameIcon>();
        private int _current_game_index = 0;
        float pauseAlpha;

        private List<PlayerIndex> Player_IDS = new List<PlayerIndex>();

        KeyboardState prevKeyboardState;
        GamePadState prevGamepadState;
        private float _thumb_stick_threshold = .3f;
        private List<PlayerIndex> _player_indexes = new List<PlayerIndex>();

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            Player_IDS.Add(PlayerIndex.One); //We shall assume that there is always a Player 1...
            _player_indexes.Add(PlayerIndex.One);
            _player_indexes.Add(PlayerIndex.Two);
            _player_indexes.Add(PlayerIndex.Three);
            _player_indexes.Add(PlayerIndex.Four);

            
        }

        private string PlayerStateString = "";

        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            spriteBatch = new SpriteBatch(ScreenManager.GraphicsDevice);
            gameFont = content.Load<SpriteFont>("gamefont");

            //Create our list of game icons to be displayed and chosen from
            _game_icons.Add(new GameIcon(content.Load<Texture2D>(@"GameIcons\RunNJumpIconNoBorder"), 
                content.Load<Texture2D>(@"GameIcons\border"), 1, new Rectangle(45, 25, 375, 250)));
            _game_icons.Add(new GameIcon(content.Load<Texture2D>(@"GameIcons\BattleTanksIconNoBorder"),
                content.Load<Texture2D>(@"GameIcons\border"), 2, new Rectangle(440, 25, 375, 250)));
            _game_icons.Add(new GameIcon(content.Load<Texture2D>(@"GameIcons\SpaceShootersIconNoBorder"),
                content.Load<Texture2D>(@"GameIcons\border"), 3, new Rectangle(840, 25, 375, 250)));
            _game_icons.Add(new GameIcon(content.Load<Texture2D>(@"GameIcons\HiSpeedGoCartsIconNoBorder"),
                content.Load<Texture2D>(@"GameIcons\border"), 4, new Rectangle(45, 300, 375, 250)));
            _game_icons.Add(new GameIcon(content.Load<Texture2D>(@"GameIcons\PlaceHolderIconNoBorder"),
                content.Load<Texture2D>(@"GameIcons\border"), 5, new Rectangle(440, 300, 375, 250)));
            _game_icons.Add(new GameIcon(content.Load<Texture2D>(@"GameIcons\PlaceHolderIconNoBorder"),
                content.Load<Texture2D>(@"GameIcons\border"), 6, new Rectangle(840, 300, 375, 250)));
            _game_icons[_current_game_index].BorderEnabled = true;

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();
        }


        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void UnloadContent()
        {

            content.Unload();
        }


        #endregion

        #region Update and Draw

        private int mod(int x, int m)
        {
            return (x % m + m) % m;
        }


        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

            if (IsActive)
            {
                foreach (PlayerIndex PI in _player_indexes)
                {
                    if (GamePad.GetState(PI).IsConnected && GamePad.GetState(PI).IsButtonDown(Buttons.Start))
                    {
                        if (!Player_IDS.Contains(PI))
                        {
                            Player_IDS.Add(PI);
                        }
                    }
                }


                foreach (GameIcon gi in _game_icons)
                    gi.Update(gameTime);
                if (prevKeyboardState.IsKeyDown(Keys.Right) && Keyboard.GetState().IsKeyUp(Keys.Right) ||
                    prevGamepadState.ThumbSticks.Left.X < _thumb_stick_threshold && GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X >= .3f)
                {
                    _game_icons[_current_game_index].BorderEnabled = false;
                    _current_game_index = mod(++_current_game_index, _game_icons.Count);
                    _game_icons[_current_game_index].BorderEnabled = true;
                }
                else if (prevKeyboardState.IsKeyDown(Keys.Left) && Keyboard.GetState().IsKeyUp(Keys.Left) ||
                    prevGamepadState.ThumbSticks.Left.X > -_thumb_stick_threshold && GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X <= -.3f)
                {
                    _game_icons[_current_game_index].BorderEnabled = false;
                    _current_game_index = mod(--_current_game_index, _game_icons.Count);
                    _game_icons[_current_game_index].BorderEnabled = true;
                }
                else if (prevKeyboardState.IsKeyDown(Keys.Up) && Keyboard.GetState().IsKeyUp(Keys.Up) ||
                    prevGamepadState.ThumbSticks.Left.Y < _thumb_stick_threshold && GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y >= .3f)
                {
                    _game_icons[_current_game_index].BorderEnabled = false;
                    _current_game_index = mod(_current_game_index - 3, _game_icons.Count);
                    _game_icons[_current_game_index].BorderEnabled = true;
                }
                else if (prevKeyboardState.IsKeyDown(Keys.Down) && Keyboard.GetState().IsKeyUp(Keys.Down) ||
                    prevGamepadState.ThumbSticks.Left.Y > -_thumb_stick_threshold && GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y <= -.3f)
                {
                    _game_icons[_current_game_index].BorderEnabled = false;
                    _current_game_index = mod(--_current_game_index + 4, _game_icons.Count);
                    _game_icons[_current_game_index].BorderEnabled = true;
                }
                else if (prevKeyboardState.IsKeyDown(Keys.Enter) && Keyboard.GetState().IsKeyUp(Keys.Enter) ||
                    prevGamepadState.IsButtonDown(Buttons.A) && GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A))
                {
                    switch (_game_icons[_current_game_index].GameID)
                    {
                        case 1:
                            //Load new background screen and our new game screen.
                            //For now we only have a gamescreen
                            LoadingScreen.Load(ScreenManager, true, PlayerIndex.One, new Arena.Screens.RunNJumpGameScreen(Player_IDS));
                            break;
                        case 2:
                            LoadingScreen.Load(ScreenManager, true, PlayerIndex.One, new Arena.Screens.TargetShooting(Player_IDS));
                            break;
                        case 3:
                            //load game three
                            break;
                        case 4:
                            //load game four
                            break;
                        case 5:
                            //load game five
                            break;
                        case 6:
                            //game game six
                            break;
                        default:
                            //how did this happen?
                            break;
                    }
                }
                
            }

            PlayerStateString = "Players Connected: ";

            foreach (PlayerIndex PI in Player_IDS)
            {
                PlayerStateString += "Player: " + PI.ToString() + ";";
            }

            prevKeyboardState = Keyboard.GetState();
            prevGamepadState = GamePad.GetState(PlayerIndex.One);
        }


        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
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


        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            //ScreenManager.GraphicsDevice.Clear(Color.Black);
            //SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            
            foreach (GameIcon gi in _game_icons)
                gi.Draw(spriteBatch);

            spriteBatch.DrawString(gameFont, PlayerStateString, new Vector2(100, 620), Color.White);
            spriteBatch.End();

            FadeScreen();
        }

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
    }
}
