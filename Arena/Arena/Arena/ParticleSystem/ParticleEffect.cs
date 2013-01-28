using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ArenaParticleEngine
{
    public abstract class ParticleEffect
    {
        protected Random random;
        public ParticleEmitter Emitter { get; set; }
        public List<Particle> particles; /* Particles currently in effect */
        protected List<Texture2D> textures; /* Textures we can draw on */
        public List<Particle> MasterParticles;

        public bool Generating { get; set; } /* who knows if needed */

        public ParticleEffect(List<Texture2D> textures, ParticleEmitter emitter)
        {
            random = new Random();
            particles = new List<Particle>();
            this.textures = textures;
            MasterParticles = new List<Particle>();
            this.Emitter = emitter;
        }

        public float RandomFromRange(float min, float max, Random random)
        {
            return (float)random.NextDouble() * (max - min + 1) + min;
        }

        public Int32 CurrentTextureIndex
        {
            get;
            set;
        }

        public String TexturePolling
        {
            get;
            set;
        }

        /* Currently supported: Additive, Alpha */
        public String BlendingState
        {
            get;
            set;
        }

        public Particle GenerateNewParticle(float offset_x, float offset_y, int p_count, int total_p)
        {
            float percent = (float)p_count / (float)total_p;

            int ParticleID = -1;
            /* if we're doing random texture polling */
            if (TexturePolling == "Random")
                ParticleID = random.Next(textures.Count);
            else if (TexturePolling == "RoundRobin")
                ParticleID = CurrentTextureIndex++ % textures.Count;


            Particle BaseParticle = MasterParticles[ParticleID];

            Vector2 base_position = Vector2.Lerp(Emitter.LastLocation, Emitter.Location, 1.0f);


            Vector2 position = new Vector2(base_position.X + offset_x, base_position.Y + offset_y);

            


            //Vector2 emitter_offset = Vector2.Lerp(Emitter.OffsetMin, Emitter.OffsetMax, (float)random.NextDouble());

            Vector2 emitter_offset = new Vector2(RandomFromRange(Emitter.OffsetMin.X, Emitter.OffsetMax.X, random), RandomFromRange(Emitter.OffsetMin.Y, Emitter.OffsetMax.Y, random));

            position += emitter_offset;

            /* We will build the rest of this particle off of our base particle */

            Particle ParticleToReturn = new Particle();
            ParticleToReturn.Texture = textures[ParticleID];
            //ParticleToReturn.Origin = new Vector2(ParticleToReturn.Texture.Width / 2, ParticleToReturn.Texture.Height / 2);
            ParticleToReturn.Location = position;

            Vector2 velocity = new Vector2(RandomFromRange(Emitter.MinVelocity.X, Emitter.MaxVelocity.X, random), RandomFromRange(Emitter.MinVelocity.Y, Emitter.MaxVelocity.Y, random));


            /* Same thing for acceleration */
            Vector2 acceleration = Vector2.Zero;

            //acceleration = Vector2.Lerp(Emitter.MinimumAcceleration, Emitter.MaximumAcceleration, (float)random.NextDouble());

            acceleration = new Vector2(RandomFromRange(Emitter.MinimumAcceleration.X, Emitter.MaximumAcceleration.X, random), RandomFromRange(Emitter.MinimumAcceleration.Y, Emitter.MaximumAcceleration.Y, random));


            float angle = BaseParticle.Rotation;
            float angularVelocity = BaseParticle.RotationSpeed;

            string Name = BaseParticle.Name;

            Color startColor = BaseParticle.StartColor;
            Color endColor = BaseParticle.EndColor;


            float alpha = BaseParticle.StartAlpha;

            float life = MathHelper.Lerp(BaseParticle.MinLife, BaseParticle.MaxLife, (float)random.NextDouble());

            float scale = BaseParticle.Scale;

            ParticleToReturn.Acceleration = acceleration;
            ParticleToReturn.StartAlpha = alpha;
            ParticleToReturn.EndAlpha = BaseParticle.EndAlpha;
            ParticleToReturn.StartColor = startColor;
            ParticleToReturn.EndColor = endColor;
            ParticleToReturn.Name = Name;
            ParticleToReturn.Origin = Vector2.Zero;
            ParticleToReturn.Rotation = angle;
            ParticleToReturn.RotationSpeed = angularVelocity;
            ParticleToReturn.Scale = scale;
            ParticleToReturn.Life = life;
            ParticleToReturn.StartingLife = life;
            ParticleToReturn.Velocity = velocity;
            ParticleToReturn.ScaleStart = BaseParticle.ScaleStart;
            ParticleToReturn.ScaleEnd = BaseParticle.ScaleEnd;

            return ParticleToReturn;
        }

        public virtual void Update(float dt)
        {
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}
