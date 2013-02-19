using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace Arena.Sprites
{
    public class TBBall: Sprite
    {
        public enum OWNER
        {
            NONE,
            PLAYERONE,
            PLAYERTWO,
            PLAYERTHREE,
            PLAYERFOUR
        }

        /* Ball has an owner, to begin with, there is no owner */
        public OWNER owner;
        public float speed = 255.0f;
        public float speed_delta = 0.0f;
        public Vector2 direction;
        public int particle_effect_id;
        public static Random r = new Random();

        public Rectangle GetCollisionRect()
        {
            return new Rectangle((int)Position.X-8, (int)Position.Y-6, 20, 20);
        }

        public RectangleOverlay overlay;
        

        /// <summary>
        /// Creates a TBBall at starting_offset, given some direction given by angle
        /// </summary>
        /// <param name="angle">Determines direction ball is fired off at, 0 == straight up, 180degrees = straight down, etc</param>
        /// <param name="starting_offset">Initial position of ball</param>
        public TBBall(float angle, Vector2 starting_offset)
        {
            /* given some angle, we will generate our position as a rotation
             * from the starting location */
            float temp_radius = 1.0f;
            Position = starting_offset;
            owner = OWNER.NONE;
            overlay = new RectangleOverlay(new Rectangle((int)Position.X, (int)Position.Y, (int)(128.0f * .3), (int)(128.0F * .3)), Color.Green, Arena.Screens.TrailBlazer.gDevice);
            /* maybe we can choose random between -1 and 1 for both x and y, then normalize? */
            //float x = Utility.MathFunctions.RandomFromRange(-1.0f, 1.0f, r);
            //float y = Utility.MathFunctions.RandomFromRange(-1.0f, 1.0f, r);

            //direction = new Vector2(x, y);
            //direction.Normalize();

            var angl = r.NextDouble() * Math.PI * 2.0f;
            direction = new Vector2((float)Math.Cos(angl), (float)Math.Sin(angl));

            direction.Normalize();

            particle_effect_id = ArenaParticleEngine.ParticleEngine.Instance.LoadFromFile("BallOfLight", Screens.TrailBlazer.content);

            /* One "glitch" from the particle engine is the first update will be wonky due to Interpolation
             * between LastLocation and Location, so we will fix that by setting both Location and LastLocation to starting_offset
             * difference is negligble, but let's be thorough */
            
            ArenaParticleEngine.ParticleEngine.Instance.systems[particle_effect_id].effects[0].Emitter.Location = starting_offset;
            ArenaParticleEngine.ParticleEngine.Instance.systems[particle_effect_id].effects[0].Emitter.LastLocation = starting_offset;
            /* Finally, we need to start generating particles asap */
            ArenaParticleEngine.ParticleEngine.Instance.systems[particle_effect_id].effects[0].Generating = true;
        }

        public void Destroy()
        {
            /* remove entry from particle engine */
            ArenaParticleEngine.ParticleEngine.Instance.systems.Remove(particle_effect_id);
        }

        public TBBall(Vector2 direction_override, Vector2 start)
        {
            Position = start;
            direction = direction_override;
            owner = OWNER.NONE;
            overlay = new RectangleOverlay(new Rectangle((int)Position.X, (int)Position.Y, (int)(128.0f * .3), (int)(128.0F * .3)), Color.Green, Arena.Screens.TrailBlazer.gDevice);

            particle_effect_id = ArenaParticleEngine.ParticleEngine.Instance.LoadFromFile("BallOfLight", Screens.TrailBlazer.content);
            ArenaParticleEngine.ParticleEngine.Instance.systems[particle_effect_id].effects[0].Emitter.Location = start;
            ArenaParticleEngine.ParticleEngine.Instance.systems[particle_effect_id].effects[0].Emitter.LastLocation = start;
            /* Finally, we need to start generating particles asap */
            ArenaParticleEngine.ParticleEngine.Instance.systems[particle_effect_id].effects[0].Generating = true;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            /* We need to change the color of the ball based on current owner */
            
            switch (owner)
            {
                case OWNER.NONE:
                    /* Ball should be white */
                    ArenaParticleEngine.ParticleEngine.Instance.systems[particle_effect_id].effects[0].MasterParticles[0].StartColor = Color.White;
                    ArenaParticleEngine.ParticleEngine.Instance.systems[particle_effect_id].effects[0].MasterParticles[0].EndColor = Color.White;
                    break;
                case OWNER.PLAYERONE:
                    ArenaParticleEngine.ParticleEngine.Instance.systems[particle_effect_id].effects[0].MasterParticles[0].StartColor = Color.Red;
                    ArenaParticleEngine.ParticleEngine.Instance.systems[particle_effect_id].effects[0].MasterParticles[0].EndColor = Color.Red;
                    break;
                case OWNER.PLAYERTWO:
                    ArenaParticleEngine.ParticleEngine.Instance.systems[particle_effect_id].effects[0].MasterParticles[0].StartColor = Color.Green;
                    ArenaParticleEngine.ParticleEngine.Instance.systems[particle_effect_id].effects[0].MasterParticles[0].EndColor = Color.Green;
                    break;
                case OWNER.PLAYERTHREE:
                    ArenaParticleEngine.ParticleEngine.Instance.systems[particle_effect_id].effects[0].MasterParticles[0].StartColor = Color.Yellow;
                    ArenaParticleEngine.ParticleEngine.Instance.systems[particle_effect_id].effects[0].MasterParticles[0].EndColor = Color.Yellow;
                    break;
                case OWNER.PLAYERFOUR:
                    ArenaParticleEngine.ParticleEngine.Instance.systems[particle_effect_id].effects[0].MasterParticles[0].StartColor = Color.Blue;
                    ArenaParticleEngine.ParticleEngine.Instance.systems[particle_effect_id].effects[0].MasterParticles[0].EndColor = Color.Blue;
                    break;
            }
            overlay.Draw(spriteBatch);
            //base.Draw(spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            speed += speed_delta * dt;

            MathHelper.Clamp(speed, 0.0f, 800.0f);

            /* Lets move our ball according to direction */
            Position += direction * speed * dt;
            overlay.dummyRectangle = GetCollisionRect();
            /* To update, we need to set the location of our particle effect */
            /* the ParticleEngine.Instance.Update() method will handle the rest */
            ArenaParticleEngine.ParticleEngine.Instance.systems[particle_effect_id].effects[0].Emitter.Location = Position;
            base.Update(gameTime);
        }
    }
}
