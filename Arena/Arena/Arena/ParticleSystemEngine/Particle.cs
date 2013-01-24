using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArenaParticleEngine
{
    public class Particle
    {
        #region Newtonian Properties
        public Vector2 Location
        {
            get;
            set;
        }

        public Vector2 Velocity
        {
            get;
            set;
        }

        public Vector2 Acceleration
        {
            get;
            set;
        }
        #endregion

        #region Render Properties
        public Color Color
        {
            get;
            set;
        }

        public Color StartColor
        {
            get;
            set;
        }

        public Color EndColor
        {
            get;
            set;
        }

        public float StartAlpha
        {
            get;
            set;
        }

        public float EndAlpha
        {
            get;
            set;
        }

        public float Rotation
        {
            get;
            set;
        }

        public float RotationSpeed
        {
            get;
            set;
        }

        public float Scale
        {
            get;
            set;
        }

        public float ScaleStart
        {
            get;
            set;
        }

        public float ScaleEnd
        {
            get;
            set;
        }

        public Texture2D Texture
        {
            get;
            set;
        }

        public Vector2 Origin
        {
            get;
            set;
        }
        /* Used to identify itself */
        public String Name
        {
            get;
            set;
        }

        public String TextureName
        {
            get;
            set;
        }

        public override string ToString()
        {
            return Name;
        }
        #endregion

        #region Life-time Specifics

        public float MinLife
        {
            get;
            set;
        }

        public float MaxLife
        {
            get;
            set;
        }

        public float Life
        {
            get;
            set;
        }

        public float StartingLife
        {
            get;
            set;
        }

        #endregion

        #region Init
        public Particle()
        {
            /* All pieces of Particle should be set by their property */
        }
        #endregion

        #region Draw/Update
        public void Draw(SpriteBatch spriteBatch)
        {
            float alpha = MathHelper.Lerp(StartAlpha, EndAlpha, 1.0f - (Life / StartingLife));

            //float alpha = 1.0f;
            spriteBatch.Draw(Texture, Location, null, Color * alpha, Rotation, Origin, Scale, SpriteEffects.None, 1.0f);
        }

        public void Update(float dt)
        {
            Life -= dt;
            Velocity += Acceleration * dt;
            Location += Velocity * dt;
            Rotation += RotationSpeed * dt;
       

            Color = Color.Lerp(StartColor, EndColor, 1.0f - (Life / StartingLife));
            Scale = MathHelper.Lerp(ScaleStart, ScaleEnd, 1.0f - (Life / StartingLife));
        }
        #endregion
    }
}
