using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arena.ParticleEngine
{
    public class ParticleEngine
    {
        public Dictionary<String, ParticleEffect> effects;
        private static ParticleEngine _instance;

        public static ParticleEngine GetInstance()
        {
            if (_instance == null)
                _instance = new ParticleEngine();
            return _instance;
        }

        private ParticleEngine()
        {
            effects = new Dictionary<String, ParticleEffect>();
        }

        public void AddParticleEffect(ParticleEffect effect)
        {
        }

        public void Update(GameTime gameTime)
        {
            foreach (ParticleEffect effect in effects.Values)
            {
                effect.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (ParticleEffect effect in effects.Values)
            {
                effect.Draw(spriteBatch);
            }
        }
    }
}
