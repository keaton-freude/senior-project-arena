using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArenaParticleEngine
{
    public class ContinuousParticleEffect : ParticleEffect
    {
        public ContinuousParticleEffect(List<Texture2D> textures, ParticleEmitter emitter) :
            base(textures, emitter)
        {
        }

        public override void Update(float dt)
        {
            for (int particle = 0; particle < particles.Count; particle++)
            {
                particles[particle].Update(dt);
                if (particles[particle].Life <= 0)
                {
                    particles.RemoveAt(particle);
                }
            }

            if (Generating)
            {
                /* Generate new Particles for this frame */
                /* This is more of a "wind-up" function, shouldn't be called once
                 * we're at max capacity */

                int particle_budget = Emitter.MaxParticles - particles.Count;
                int particles_this_update = 0;

                if (particle_budget - Emitter.ParticlesPerUpdate < 0)
                {
                    particles_this_update = Emitter.ParticlesPerUpdate - (Emitter.ParticlesPerUpdate - particle_budget);
                }
                else
                    particles_this_update = Emitter.ParticlesPerUpdate;


                for (int i = 0; i < Emitter.ParticlesPerUpdate; ++i)
                {
                    /* How many particles CAN we add? */
                    if (particles.Count < Emitter.MaxParticles)
                        particles.Add(base.GenerateNewParticle(0, 0, i+1, particles_this_update));
                    else
                        break;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            /* Can we change blendingState during a spriteBatch.Begin block? if so, then we don't
             * need to call begin and end on every effect here
             * Also: How much of a performance hit is there in having more than 1 Begin/End pair?
             * 
             * Post on Shawn's blog about changing blending-mode while in the middle of a Begin operation
             * http://blogs.msdn.com/b/shawnhar/archive/2007/01/02/spritebatch-and-custom-blend-modes.aspx
             * However, it appears that when changing the blend-mode, it will apply to all sprites in the batch
             * so, not a solution but a decent place to start...
             */

            if (BlendingState == "Alpha")
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            else if (BlendingState == "Additive")
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            for (int index = 0; index < particles.Count; index++)
            {
                particles[index].Draw(spriteBatch);
            }
            spriteBatch.End();
        }
    }
}
