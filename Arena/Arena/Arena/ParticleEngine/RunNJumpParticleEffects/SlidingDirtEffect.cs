using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arena.ParticleEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arena.ParticleEngine.RunNJumpParticleEffects
{
    public class SlidingDirtEffect: ParticleEffect
    {
        public SlidingDirtEffect(List<Texture2D> textures, Vector2 location) : base(textures)
        {
            EmitterLocation = location;
        }

        public override Particle GenerateNewParticle()
        {
            //This generates a new Dirt particle
            Texture2D texture = textures[random.Next(textures.Count)];
            Vector2 position = EmitterLocation;
            Vector2 velocity = new Vector2((float)(random.NextDouble() - 1.0f) * 6.0f, ((float)random.NextDouble() - 1.0f) * 1.0f);
            float angle = 0f;
            float angularVelocity = 0.1f * (float)(random.NextDouble() * 2 - 1);

            Color color = Color.SaddleBrown * .4f;
            float size = .75f;
            int ttl = 20 + random.Next(30);

            return new Particle(texture, position, velocity, angle, angularVelocity, color, size, ttl);

        }

        public override void Update(GameTime gameTime)
        {
            if (Generating)
            {
                int total = 6;

                for (int i = 0; i < total; ++i)
                {
                    particles.Add(GenerateNewParticle());
                }
            }

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
