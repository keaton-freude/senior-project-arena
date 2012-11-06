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

        Texture2D _game_background;

        //TargetShootingPlayer player;

        KeyboardState prevKeyboardState;
        MouseState prevMouseState;

        SpriteFont PlayerScoreFont;

        TargetManager target_manager;

        List<PlayerIndex> PlayerIndexes;
        List<TargetShootingPlayer> _players;

        public TargetShooting(List<PlayerIndex> _player_indexes)
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

            //player = new TargetShootingPlayer(PlayerIndex.One, content);
            _players = new List<TargetShootingPlayer>();
            foreach (PlayerIndex PI in PlayerIndexes)
            {
                _players.Add(new TargetShootingPlayer(PI, content));
            }
            
            
            target_manager = new TargetManager(content);
            PlayerScoreFont = content.Load<SpriteFont>(@"FloatingTextFont");
            List<Texture2D> ShotsFiredTextures = new List<Texture2D>();
            List<Texture2D> TargetDestroyedTextures = new List<Texture2D>();
            TargetDestroyedTextures.Add(content.Load<Texture2D>(@"ParticleTextures/Target1"));
            TargetDestroyedTextures.Add(content.Load<Texture2D>(@"ParticleTextures/Target2"));
            TargetDestroyedTextures.Add(content.Load<Texture2D>(@"ParticleTextures/Target3"));
            TargetDestroyedTextures.Add(content.Load<Texture2D>(@"ParticleTextures/Target4"));
            FloatingText.FloatingTextEngine.GetInstance().TextFont = content.Load<SpriteFont>(@"FloatingTextFont");
            ShotsFiredTextures.Add(content.Load<Texture2D>(@"ParticleTextures/Target1"));
            ParticleEngine.ParticleEngine.GetInstance().effects.Add("ShotsFiredEffect", new ParticleEngine.TargetShootingParticleEffects.ShotFiredEffect(ShotsFiredTextures));
            ParticleEngine.ParticleEngine.GetInstance().effects.Add("TargetDestroyedEffect", new ParticleEngine.TargetShootingParticleEffects.TargetDestroyedEffect(TargetDestroyedTextures));
            _game_background = content.Load<Texture2D>(@"BackgroundTargetShooting");
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
                //player.Update(gameTime, target_manager.Targets);
                foreach (TargetShootingPlayer player in _players)
                {
                    player.Update(gameTime, target_manager.Targets);
                }
                target_manager.Update(gameTime);
            }
            ParticleEngine.ParticleEngine.GetInstance().Update(gameTime);
            FloatingText.FloatingTextEngine.GetInstance().Update(gameTime);
            prevKeyboardState = Keyboard.GetState();
            prevMouseState = Mouse.GetState();
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice device = ScreenManager.GraphicsDevice;
            device.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(_game_background, Vector2.Zero, Color.White);
            target_manager.Draw(spriteBatch);
            ParticleEngine.ParticleEngine.GetInstance().Draw(spriteBatch);

            //player.Draw(spriteBatch);

            foreach (TargetShootingPlayer player in _players)
            {
                player.Draw(spriteBatch);
            }
            FloatingText.FloatingTextEngine.GetInstance().Draw(spriteBatch);
            //Draw the player's scores on the top of the screen

            for (int i = 0; i < _players.Count; ++i)
            {
                Utility.FontRendering.DrawOutlinedText(spriteBatch, String.Format("Player {0}: {1}", i + 1, _players[i].Score), Color.Black, _players[i].Crosshair.CrosshairColor, 1.5f, 0.0f, new Vector2(i * 250, 25), PlayerScoreFont, 1);
                
                //spriteBatch.DrawString(PlayerScoreFont, String.Format("Player {0}: {1}", i + 1, _players[i].Score), new Vector2(i * 250, 0), _players[i].Crosshair.CrosshairColor);
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
