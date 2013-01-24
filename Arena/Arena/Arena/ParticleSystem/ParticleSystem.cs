using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArenaParticleEngine
{
    public class ParticleSystem
    {
        public List<ParticleEffect> effects;
        public string name;

        public ParticleSystem()
        {
            effects = new List<ParticleEffect>();
        }

        public void Update(float dt)
        {
            foreach (ParticleEffect effect in effects)
            {
                effect.Update(dt);
            }
            if (effects.Count != 0)
            {
                /* EDITOR ONLY NONSENSE */
                //MainForm.maxParticleCount.Text = "Max Particles: " + effects[0].Emitter.MaxParticles.ToString();
                //MainForm.particleCountLabel.Text = "Active Particles: " + effects[0].particles.Count.ToString();
                /* Can be replaced with XNA-specific debug hooks */
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (ParticleEffect effect in effects)
            {
                effect.Draw(spriteBatch);
            }
        }

        
    }
}
