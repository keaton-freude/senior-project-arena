using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arena.Sprites
{
    public class TankProjectile
    {
        /* Projectile has Position, Direction and this particular Projectile has a radius
         * that defines its collision
         */

        public float radius;
        public Vector2 position;
        public Vector2 velocity;
        private int projectile_id = 0;
        private int smoke_id = 0;
        public bool exploded = false;
        private RectangleOverlay overlay;
        //public static int projectile_id = 0;
        public bool finished = false;
        private float time = 0.0f;
        public bool counting = false;

        public Rectangle GetCollisionRect()
        {
            return new Rectangle((int)position.X-5, (int)position.Y-4, (int)radius, (int)radius);
        }

        public Vector2 CollisionVector()
        {
            return new Vector2(position.X - 5, position.Y - 4);
        }

        public TankProjectile()
        {
        }

        public TankProjectile(Vector2 pos, Vector2 velocity)
        {
            radius = 1f;
            position = pos;
            this.velocity = velocity;

            this.projectile_id = ArenaParticleEngine.ParticleEngine.Instance.LoadFromFile("FireBall", Arena.Screens.TankShooters.content);
            ArenaParticleEngine.ParticleEngine.Instance.systems[projectile_id].effects[0].Generating = true;
            this.smoke_id = ArenaParticleEngine.ParticleEngine.Instance.LoadFromFile("SmokeTrail", Arena.Screens.TankShooters.content);
            ArenaParticleEngine.ParticleEngine.Instance.systems[smoke_id].effects[0].Generating = true;
            overlay = new RectangleOverlay(Rectangle.Empty, Color.White, Arena.Screens.TankShooters.gDevice);

        }

        public void HardRemove()
        {
            ArenaParticleEngine.ParticleEngine.Instance.systems.Remove(smoke_id);
            ArenaParticleEngine.ParticleEngine.Instance.systems.Remove(projectile_id);
        }

        public void Remove()
        {
            ArenaParticleEngine.ParticleEngine.Instance.systems[smoke_id].effects[0].Generating = false;
            ArenaParticleEngine.ParticleEngine.Instance.systems[projectile_id].effects[0].Generating = false;
            exploded = false;
        }

        private bool already_exploded = false;

        public void Update(float dt)
        {
            position += velocity * dt;
            ArenaParticleEngine.ParticleEngine.Instance.systems[projectile_id].effects[0].Update(dt);
            ArenaParticleEngine.ParticleEngine.Instance.systems[smoke_id].effects[0].Update(dt);

            overlay.dummyRectangle = GetCollisionRect();

            foreach (RectangleOverlay rect in Arena.Screens.TankShooters.map.collision_mask.debug_rectangle_overlays)
            {
                if (rect.dummyRectangle.Intersects(GetCollisionRect()) && !already_exploded)
                {
                    exploded = true;
                    already_exploded = true;
                }
            }

            if (counting)
                time += dt;
            if (time > 2.0f)
                finished = true;
        }



        public void Draw(SpriteBatch spriteBatch)
        {
            


            ArenaParticleEngine.ParticleEngine.Instance.systems[projectile_id].effects[0].Emitter.Location = position;
            ////ArenaParticleEngine.ParticleEngine.Instance.systems["FireBall"].effects[0].Emitter.LastLocation = position;


            ///* Keep smoke trail a bit back */
            ArenaParticleEngine.ParticleEngine.Instance.systems[smoke_id].effects[0].Emitter.Location = position - (velocity * (3f/60f));

            //overlay.Draw(spriteBatch);
        }
    }
}
