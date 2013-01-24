using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArenaParticleEngine
{
    public class OneShotParticleEffect: ParticleEffect
    {
        /* This class provides a friendy way of creating one shot particles */

        public String EffectType
        {
            get;
            set;
        }

        /* If EffectType == Circle, then we use this */
        public float CircleRadius
        {
            get;
            set;
        }

        //public int NumberOfParticles
        //{
        //    get;
        //    set;
        //}

        public Int32 Iterations
        {
            get;
            set;
        }



        public OneShotParticleEffect(List<Texture2D> textures, ParticleEmitter emitter) :
            base(textures, emitter)
        {

        }

        public void Fire()
        {
            /* Create the particle Effect according to the rules given */
            /* Hard coding to a circle for testing. TODO: Make generic */
            int n = Emitter.MaxParticles;

            for (int x = 0; x < n; ++x)
            {
                float xp = (float)Math.Cos(Math.PI * 2 / n * x) * CircleRadius;
                float yp = (float)Math.Sin(Math.PI * 2 / n * x) * CircleRadius;
                particles.Add(base.GenerateNewParticle(xp, yp, 0, 1));
            }

        }

        //public Particle GenerateNewParticle(float offset_x, float offset_y)
        //{
        //    return null;
        //    //Texture2D texture = null;
        //    //int ParticleID = -1; 
        //    ///* if we're doing random texture polling */
        //    //if (TexturePolling == "Random")
        //    //    ParticleID = random.Next(textures.Count);
        //    //else if (TexturePolling == "RoundRobin")
        //    //    ParticleID = CurrentTextureIndex++ % textures.Count;


        //    //Particle BaseParticle = MasterParticles[ParticleID];


        //    //Vector2 position = new Vector2(Emitter.Location.X + offset_x, Emitter.Location.Y + offset_y);

        //    //Vector2 emitter_offset = Vector2.Lerp(Emitter.OffsetMin, Emitter.OffsetMax, (float)random.NextDouble());
        //    //position += emitter_offset;

        //    ///* We will build the rest of this particle off of our base particle */

        //    //Particle ParticleToReturn = new Particle();
        //    //ParticleToReturn.Texture = textures[ParticleID];
        //    //ParticleToReturn.Location = position;

        //    //float spd_x = RandomFromRange(Emitter.MinParticleSpeed.X, Emitter.MaxParticleSpeed.X, random);
        //    //float spd_y = RandomFromRange(Emitter.MinParticleSpeed.Y, Emitter.MaxParticleSpeed.Y, random);
        //    //Vector2 velocity = Vector2.Transform(Emitter.Direction, Matrix.CreateRotationZ(MathHelper.Lerp(Emitter.MinAngleRot, Emitter.MaxAngleRot, (float)random.NextDouble())));

        //    //velocity *= new Vector2(spd_x, spd_y);

        //    ///* Now we need to transform the velocity by a random angle specified in the emitter */


        //    ///* Same thing for acceleration */
        //    //Vector2 acceleration = Vector2.Zero;

        //    //acceleration = Vector2.Lerp(Emitter.MinimumAcceleration, Emitter.MaximumAcceleration, (float)random.NextDouble());
            


        //    //float angle = BaseParticle.Rotation;
        //    //float angularVelocity = BaseParticle.RotationSpeed;

        //    //string Name = BaseParticle.Name;

        //    //Color startColor = BaseParticle.StartColor;
        //    //Color endColor = BaseParticle.EndColor;


        //    //float alpha = BaseParticle.StartAlpha;

        //    //float life = MathHelper.Lerp(BaseParticle.MinLife, BaseParticle.MaxLife, (float)random.NextDouble());

        //    //float scale = BaseParticle.Scale;

        //    //ParticleToReturn.Acceleration = acceleration;
        //    //ParticleToReturn.StartAlpha = alpha;
        //    //ParticleToReturn.StartColor = startColor;
        //    //ParticleToReturn.EndColor = endColor;
        //    //ParticleToReturn.Name = Name;
        //    //ParticleToReturn.Origin = Vector2.Zero;
        //    //ParticleToReturn.Rotation = angle;
        //    //ParticleToReturn.RotationSpeed = angularVelocity;
        //    //ParticleToReturn.Scale = scale;
        //    //ParticleToReturn.Life = life;
        //    //ParticleToReturn.StartingLife = life;
        //    //ParticleToReturn.Velocity = velocity;
        //    //ParticleToReturn.ScaleStart = BaseParticle.ScaleStart;
        //    //ParticleToReturn.ScaleEnd = BaseParticle.ScaleEnd;

        //    //return ParticleToReturn;
        //}

        public override void Update(float dt)
        {
            for (int particle = 0; particle < particles.Count; particle++)
            {
                particles[particle].Update(dt);
                if (particles[particle].Life <= 0)
                {
                    //particles[particles.IndexOf(particles[particle])] = GenerateNewParticle(1, 1, 1, EmitterLocation.X, EmitterLocation.Y);
                    particles.RemoveAt(particle);
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
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
