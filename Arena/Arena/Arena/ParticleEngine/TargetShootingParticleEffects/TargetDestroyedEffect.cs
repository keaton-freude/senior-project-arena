using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arena.ParticleEngine.TargetShootingParticleEffects
{
    public class TargetDestroyedEffect: ParticleEffect
    {
        public TargetDestroyedEffect(List<Texture2D> textures)
            : base(textures)
        {
        }

        public void DestroyTarget(Vector2 target_center, float target_radius, Color color, float scale)
        {
            //generate pieces of the target (random pieces) in a circle
            //around where the target was
            int n = 20;

            for (int x = 0; x < n; x++)
            {
                particles.Add(GenerateNewParticle(x, n, target_radius, target_center.X, target_center.Y, color, scale));
            }

            n = 15;

            for (int x = 0; x < n; x++)
            {
                particles.Add(GenerateNewParticle(x, n, target_radius * .67f, target_center.X, target_center.Y, color, scale));
            }

            n = 15;

            for (int x = 0; x < n; x++)
            {
                particles.Add(GenerateNewParticle(x, n, target_radius / 2.0f, target_center.X, target_center.Y, color, scale));
            }


            n = 10;

            for (int x = 0; x < n; x++)
            {
                particles.Add(GenerateNewParticle(x, n, target_radius / 4.0f, target_center.X, target_center.Y, color, scale));
            }

            n = 5;

            for (int x = 0; x < n; x++)
            {
                particles.Add(GenerateNewParticle(x, n, target_radius / 8.0f, target_center.X, target_center.Y, color, scale));
            }

        }

        public Particle GenerateNewParticle(int x, int n, float r, float off_x, float off_y, Color color, float scale)
        {
            Texture2D texture = textures[random.Next(textures.Count)];
            float xp = (float)Math.Cos(Math.PI * 2 / n * x) * r;
            float yp = (float)Math.Sin(Math.PI * 2 / n * x) * r;
            Vector2 position = new Vector2(xp + off_x, yp + off_y);

            Vector2 velocity = new Vector2(xp, yp);

            velocity.Normalize();

            float angle = 0f;
            float angularVelocity = 0.1f;

            //scale -= .5f;

            //float size = MathHelper.Clamp(scale, 0.1f, 1.0f);
            float size = scale;
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
