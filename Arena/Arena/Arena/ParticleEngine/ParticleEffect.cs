using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arena.ParticleEngine
{
    public abstract class ParticleEffect
    {
        protected Random random; //Most particle effects need an element of randomness
        public Vector2 EmitterLocation { get; set; } //The location from which new particles spawn
        protected List<Particle> particles; //The current list of particles in effect
        protected List<Texture2D> textures; //The list of textures this effect can use

        public bool Generating { get; set; } //Are we generating or not? (pause/unpaused)

        public ParticleEffect(List<Texture2D> textures)
        {
            random = new Random();
            particles = new List<Particle>();
            this.textures = textures;
        }

        public virtual Particle GenerateNewParticle()
        {
            /* Overload this to create custom particles */
            /* Use as appropriate (may not even need it!) */

            return null;
        }

        public virtual void Update(GameTime gameTime)
        {
            /* This class should never be used */
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            /* Don't use this class */
        }
    }
}
