using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Arena.Sprites;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Arena.Utility;

namespace Arena.Players
{
    public class TankShooterPlayer: Player
    {
        public Tank tank;

        public List<TankProjectile> projectiles;
        Vector2 TankRightEffectOffset = new Vector2(0, 10);
        Vector2 TankDownEffectOffset = new Vector2(0, 10);
        private int explosion_id;

        public bool dead = false;

        float time = 0.0f;
        int explosion_count = 0;
        int max_explosions = 30;

        public TankShooterPlayer(PlayerIndex player_index): base(player_index)
        {
            projectiles = new List<TankProjectile>();
            tank = new Tank(Arena.Screens.TankShooters.content.Load<Texture2D>(@"TankShooters/" + player_index.ToString() + "/TankBody"), Arena.Screens.TankShooters.content.Load<Texture2D>(@"TankShooters/" + player_index.ToString() + "/TankCannon"), Arena.Screens.TankShooters.content.Load<Texture2D>(@"TankShooters/" + player_index.ToString() + "/TankBodyRight"));
            explosion_id = ArenaParticleEngine.ParticleEngine.Instance.LoadFromFile("Explosion", Arena.Screens.TankShooters.content);
        
            /* Tank's starting place is determined based on which player index it is. Player 1 is top left corner, Player2 is Top Right Corner, Player3 is BottomLeft Corner, Player4 is BottomRight*/
            if (player_index == PlayerIndex.One)
            {
                tank.Position = new Vector2(80, 140);
                /* Also, Player 1 starts facing down */
                tank.tank_rotation = 3.14f;
                tank.cannon_rotation = 3.14f;
            }
            else if (player_index == PlayerIndex.Two)
            {
                tank.Position = new Vector2(1180, 140);
                /* Player 2 has the same starting direction as Player 1 */
                tank.tank_rotation = 3.14f;
                tank.cannon_rotation = 3.14f;
            }
            else if (player_index == PlayerIndex.Three)
            {

            }
        
        }

        public void Update(GameTime gameTime, bool done)
        {
            if (!done)
            {
                Vector2 RightThumbStick = GamePad.GetState(Player_Index).ThumbSticks.Right;
                Vector2 LeftThumbStick = GamePad.GetState(Player_Index).ThumbSticks.Left;
                tank.Update(gameTime);
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
                    projectiles.Add(new TankProjectile(tank.Position + tank.tank_offset, Vector2.Transform(-Vector2.UnitY, Matrix.CreateRotationZ(tank.cannon_rotation)) * 650, Player_Index));


                List<TankProjectile> to_remove = new List<TankProjectile>();

                foreach (TankProjectile ta in projectiles)
                {
                    ta.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

                    /* Check collision against walls */
                    foreach (RectangleOverlay rect in Arena.Screens.TankShooters.map.collision_mask.debug_rectangle_overlays)
                    {
                        if (rect.dummyRectangle.Intersects(ta.GetCollisionRect()) && !ta.already_exploded)
                        {
                            ta.exploded = true;
                            ta.already_exploded = true;
                        }
                    }

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
            }

            if (tank.health.CurrentHP <= 0)
            {
                dead = true;
                /* He's dead Jim!*/
                /* Create a bunch of explosions and remove */
            }
            time += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (dead && time > .05f && !remove)
            {
                time -= .1f;
                explosion_count += 1;
                /* Create explosion here! */
                ArenaParticleEngine.ParticleEngine.Instance.systems[explosion_id].effects[0].Emitter.Location = Utility.MathFunctions.PointWithinRecetangle(tank.CollisionRect);
                ((ArenaParticleEngine.OneShotParticleEffect)ArenaParticleEngine.ParticleEngine.Instance.systems[explosion_id].effects[0]).Fire();

                if (explosion_count > max_explosions)
                {
                    remove = true;
                }
            }

            base.Update(gameTime);
        }

        public bool remove = false;

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
