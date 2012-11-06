using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arena.ParticleEngine.TargetShootingParticleEffects
{
    class ShotFiredEffect: ParticleEffect
    {
        /* ShotFiredEffect is responsible for generating and managing
         * all shot-fired response particle effects
         * it contains a method to generate a clump of particles at
         * a given circle location and manages all of those particles
         * removing them when needed
         */

        public ShotFiredEffect(List<Texture2D> textures) : base(textures)
        {
            /* Nothing needed here */
        }

        public void GenerateShotFired(Vector2 circle_center, float radius)
        {
            //40 points around the circle yo
            int n = 40;

            for (int x = 0; x < n; x++)
            {
                particles.Add(GenerateNewParticle(x, n, radius, circle_center.X, circle_center.Y));
            }

            n = 16;

            for (int x = 0; x < n; x++)
            {
                particles.Add(GenerateNewParticle(x, n, radius / 2.0f, circle_center.X, circle_center.Y));
            }


            n = 6;

            for (int x = 0; x < n; x++)
            {
                particles.Add(GenerateNewParticle(x, n, radius / 4.0f, circle_center.X, circle_center.Y));
            }
        }

        public Particle GenerateNewParticle(int x, int n, float r, float off_x, float off_y)
        {
            Texture2D texture = textures[random.Next(textures.Count)];
            float xp = (float)Math.Cos(Math.PI * 2 / n * x) * r;
            float yp = (float)Math.Sin(Math.PI * 2 / n * x) * r;
            Vector2 position = new Vector2(xp + off_x, yp + off_y);

            Vector2 velocity = Vector2.Zero;

            float angle = 0f;
            float angularVelocity = 0.0f;

            Color color = Color.White;
            float size = .1f;
            int ttl = 15;
            return new Particle(texture, position, velocity, angle, angularVelocity, color, size, ttl);


            //return null;
        }

        public override void Update(GameTime gameTime)
        {
            for (int particle = 0; particle < particles.Count; particle++)
            {
                particles[particle].Update();
                if (particles[particle].TTL <= 0)
                {
                    particles.RemoveAt(particle);
                    particle--;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int index = 0; index < particles.Count; index++)
            {
                particles[index].Draw(spriteBatch);
            }
        }
    }
}
