using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Arena.Screens;
using Microsoft.Xna.Framework.Input;
using GameStateManagement;
using PE = ArenaParticleEngine;
using Arena.Utility;
using Arena.BallManager;
using Arena.Players;

namespace Arena.Screens
{
    public class TrailBlazer: GameScreen
    {
        /* u mad? */
        public static ContentManager content;
        SpriteBatch spriteBatch;
        Texture2D _game_background;
        public static GraphicsDevice gDevice;
       

        KeyboardState prevKeyboardState;
        MouseState prevMouseState;

        List<PlayerIndex> PlayerIndexes;
        public static List<TrailerBlazerPlayer> _players;

        BallManager.BallManager ball_manager;
        public TrailBlazer(List<PlayerIndex> _player_indexes)
        {

            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            PlayerIndexes = _player_indexes;
            
        }

        public override void LoadContent()
        {
            gDevice = ScreenManager.GraphicsDevice;
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            spriteBatch = new SpriteBatch(ScreenManager.GraphicsDevice);

            ball_manager = new BallManager.BallManager(5.5f);

            //ball = new Sprites.TBBall((float)Math.PI / 2.0f, new Vector2(1280/2, 720 / 2));
            _players = new List<TrailerBlazerPlayer>();
            foreach (PlayerIndex pi in PlayerIndexes)
            {
                _players.Add(new TrailerBlazerPlayer(pi));
            }


            _players.Add(new TrailerBlazerPlayer(PlayerIndex.Two));

            _game_background = content.Load<Texture2D>(@"map");
            //_game_background = content.Load<Texture2D>(@"BackgroundTargetShooting");
        }

        public override void UnloadContent()
        {
            content.Unload();
            ParticleEngine.ParticleEngine.GetInstance().effects.Clear();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            TimeKeeper.Instance.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            ball_manager.Update(gameTime);

            PE.ParticleEngine.Instance.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            foreach (TrailerBlazerPlayer player in _players)
            {
                player.Update(gameTime);
            }
            prevKeyboardState = Keyboard.GetState();
            prevMouseState = Mouse.GetState();
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice device = ScreenManager.GraphicsDevice;

            device.Clear(Color.Black);
            spriteBatch.Begin();
            spriteBatch.Draw(_game_background, Vector2.Zero, Color.White);
            foreach (TrailerBlazerPlayer player in _players)
            {
                player.Draw(spriteBatch);
            }
            ball_manager.Draw(spriteBatch);
            spriteBatch.End();
            PE.ParticleEngine.Instance.Draw(spriteBatch);            
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
