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
            spriteBatch = new SpriteBatch(ScreenManager.GraphicsDevice);
            foreach (PlayerIndex pi in PlayerIndexes)
            {
                players.Add(new TankShooterPlayer(pi));
            }
           
            gDevice = ScreenManager.GraphicsDevice;
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

            ArenaParticleEngine.ParticleEngine.Instance.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            foreach (TankShooterPlayer player in players)
            {
                player.Update(gameTime);
            }

            prevKeyboardState = Keyboard.GetState();
            prevMouseState = Mouse.GetState();
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice device = ScreenManager.GraphicsDevice;
            device.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            map.Draw(spriteBatch);
            spriteBatch.End();

            foreach (TankShooterPlayer player in players)
            {
                player.Draw(spriteBatch);
            }

            ArenaParticleEngine.ParticleEngine.Instance.Draw(spriteBatch);
        }
    }
}
