using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Arena.Sprites;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Arena.Players
{
    public class TankShooterPlayer: Player
    {
        Tank tank;

        public List<TankProjectile> projectiles;
        Vector2 TankRightEffectOffset = new Vector2(0, 10);
        Vector2 TankDownEffectOffset = new Vector2(0, 10);
        private int explosion_id;

        public TankShooterPlayer(PlayerIndex player_index): base(player_index)
        {
            projectiles = new List<TankProjectile>();
            tank = new Tank(Arena.Screens.TankShooters.content.Load<Texture2D>(@"TankShooters/1/TankBody"), Arena.Screens.TankShooters.content.Load<Texture2D>(@"TankShooters/1/TankCannon"), Arena.Screens.TankShooters.content.Load<Texture2D>(@"TankShooters/1/TankBodyRight"));
            explosion_id = ArenaParticleEngine.ParticleEngine.Instance.LoadFromFile("Explosion", Arena.Screens.TankShooters.content);
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 RightThumbStick = GamePad.GetState(PlayerIndex.One).ThumbSticks.Right;
            Vector2 LeftThumbStick = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left;

            if (RightThumbStick.Length() != 0f)
            {
                if (RightThumbStick.X > 0f)
                    tank.cannon_rotation = Utility.MathFunctions.AngleBetweenVectors(Vector2.UnitY, new Vector2(GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.X, GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Y));
                else
                    tank.cannon_rotation = Utility.MathFunctions.AngleBetweenVectors(-Vector2.UnitY, new Vector2(GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.X, GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Y)) - 3.14f;
            }

            if (LeftThumbStick.Y > .1f)
            {
                /* only swap if we're in the left or right position */
                if (tank.tank_rotation == (3.14f / 2f) || tank.tank_rotation == (3.14f + (3.14f / 2f)))
                    tank.tank_rotation = 0f;
                tank.Position.Y += LeftThumbStick.Y * -1;
            }
            else if (LeftThumbStick.Y < -.1f)
            {
                if (tank.tank_rotation == (3.14f / 2f) || tank.tank_rotation == (3.14f + (3.14f / 2f)))
                    tank.tank_rotation = 3.14f;
                tank.Position.Y += LeftThumbStick.Y * -1;
            }
            else if (LeftThumbStick.X > .1f)
            {
                /* only swap if we're in the up or down position */
                if (tank.tank_rotation == 0f || tank.tank_rotation == 3.14f)
                    tank.tank_rotation = 3.14f / 2f;
                tank.Position.X += LeftThumbStick.X;
            }
            else if (LeftThumbStick.X < -.1f)
            {
                if (tank.tank_rotation == 0f || tank.tank_rotation == 3.14f)
                    tank.tank_rotation = 3.14f + (3.14f / 2);
                tank.Position.X += LeftThumbStick.X;
            }

            if (Keyboard.GetState().IsKeyUp(Keys.T) && PrevKeyboardState.IsKeyDown(Keys.T))
                projectiles.Add(new TankProjectile(tank.Position + tank.tank_offset, Vector2.Transform(-Vector2.UnitY, Matrix.CreateRotationZ(tank.cannon_rotation)) * 650));


            List<TankProjectile> to_remove = new List<TankProjectile>();

            foreach (TankProjectile ta in projectiles)
            {
                ta.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

                if (ta.exploded)
                {
                    to_remove.Add(ta);
                    ta.Remove();
                    ArenaParticleEngine.ParticleEngine.Instance.systems[explosion_id].effects[0].Emitter.Location = ta.position;
                    ((ArenaParticleEngine.OneShotParticleEffect)ArenaParticleEngine.ParticleEngine.Instance.systems[explosion_id].effects[0]).Fire();
                }

            }

            foreach (TankProjectile ta in to_remove)
            {
                if (ta.finished)
                {
                    ta.HardRemove();
                    projectiles.Remove(ta);
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            foreach (TankProjectile ta in projectiles)
            {
                ta.Draw(spriteBatch);

            }
            spriteBatch.End();

            spriteBatch.Begin();
            tank.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(spriteBatch);
        }
    }
}
