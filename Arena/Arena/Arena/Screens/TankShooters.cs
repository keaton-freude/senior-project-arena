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
using Arena.Map;
using Arena.Sprites;

namespace Arena.Screens
{
    public class TankShooters: GameScreen
    {
        public static ContentManager content;
        SpriteBatch spriteBatch;

        KeyboardState prevKeyboardState;
        MouseState prevMouseState;
        public static GraphicsDevice gDevice;

        public static Arena.Map.Map map;

        private int explosion_id = -1;

        private SpriteFont UIFont;

        public List<TankShooterPlayer> players;

        List<PlayerIndex> PlayerIndexes;

        public TankShooters(List<PlayerIndex> _player_indexes)
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            PlayerIndexes = _player_indexes;
            players = new List<TankShooterPlayer>();


        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            gDevice = ScreenManager.GraphicsDevice;
            spriteBatch = new SpriteBatch(ScreenManager.GraphicsDevice);
            UIFont = content.Load<SpriteFont>(@"gamefont");
            foreach (PlayerIndex pi in PlayerIndexes)
            {
                players.Add(new TankShooterPlayer(pi));
            }
           // players.Add(new TankShooterPlayer(PlayerIndex.Two));

            /* Projectiles have all moved (may have exploded against a wall or whatever)
             * Check each projectile against a player */
            /* We will go player by player, and projectile by projectile checking collisions */



           

            List<Texture2D> textures = new List<Texture2D>();

            for(int i = 1; i < 47; ++i)
            {
                textures.Add(content.Load<Texture2D>(@"TankShooters/MapTiles/Desert" + i.ToString()));
            }
            map = new Arena.Map.Map("TankMap", textures, ScreenManager.GraphicsDevice);
        }

        public override void UnloadContent()
        {
            content.Unload();
            ParticleEngine.ParticleEngine.GetInstance().effects.Clear();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);
            if (!done)
            {
                foreach (TankShooterPlayer player in players)
                {
                    foreach (TankProjectile projectile in player.projectiles)
                    {
                        foreach (TankShooterPlayer innerplayer in players)
                        {
                            if (innerplayer.dead == false)
                            {
                                if (projectile.GetCollisionRect().Intersects(innerplayer.tank.CollisionRect) && !projectile.already_exploded && projectile.owner != innerplayer.Player_Index)
                                {
                                    /* Hit! */
                                    /* Create explosion */
                                    projectile.exploded = true;
                                    projectile.already_exploded = true;

                                    /* Lower health of hit player */
                                    innerplayer.tank.health.CurrentHP = innerplayer.tank.health.CurrentHP - 10;
                                }
                            }
                        }
                    }
                }

                /* lastly, check every projectile against every other, destroy both if they intersect */



                ArenaParticleEngine.ParticleEngine.Instance.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

                int tanks_alive = 0;

                foreach (TankShooterPlayer player in players)
                {
                    player.Update(gameTime, false);
                    if (player.dead == false)
                        tanks_alive++;

                }


                if (tanks_alive == 1)
                {
                    /* We have a winner, stop all updates and print winner */
                    foreach (TankShooterPlayer player in players)
                    {
                        if (player.dead == false)
                        {
                            /* This can be the only one not dead */
                            winner = player.Player_Index;
                            done = true;
                        }
                    }
                }
            }

            if (done)
            {
                ArenaParticleEngine.ParticleEngine.Instance.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
                foreach (TankShooterPlayer player in players)
                {
                    player.Update(gameTime, true);
                    //if (player.dead == false)
                        //tanks_alive++;

                }
            }

            prevKeyboardState = Keyboard.GetState();
            prevMouseState = Mouse.GetState();
        }

        private PlayerIndex winner = PlayerIndex.One;
        private bool done = false;

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice device = ScreenManager.GraphicsDevice;
            device.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            map.Draw(spriteBatch);
            spriteBatch.End();

            foreach (TankShooterPlayer player in players)
            {
                if (player.dead == false)
                    player.Draw(spriteBatch);
            }

            ArenaParticleEngine.ParticleEngine.Instance.Draw(spriteBatch);

            if (done)
            {
                Utility.FontRendering.DrawOutlinedText(spriteBatch, "Winner: Player " + winner.ToString(), Color.Black, Color.White, 2.0f, 0.0f, new Vector2(200, 200), UIFont, 1);
                ArenaParticleEngine.ParticleEngine.Instance.Draw(spriteBatch);
            }
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
