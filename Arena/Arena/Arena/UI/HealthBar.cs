using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arena.UI
{
    public class HealthBar
    {
        public Texture2D outline_texture;
        public Texture2D inner_texture;
        public int MaxHP;
        public int CurrentHP;

        public Vector2 Location;
        public float Scale = 1.0f;

        public HealthBar(int Max, int Start)
        {
            MaxHP = Max;
            CurrentHP = Start;

            outline_texture = Screens.TankShooters.content.Load<Texture2D>(@"HealthBarOuter");
            inner_texture = Screens.TankShooters.content.Load<Texture2D>(@"HealthBarInner");
        }

        private float percent = 1.0f;

        public void Update()
        {
            percent = (float)CurrentHP / (float)MaxHP;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            /* Draw the Outline, then draw Inner */
            spriteBatch.Draw(outline_texture, new Rectangle((int)Location.X, (int)Location.Y, (int)(outline_texture.Width * Scale), (int)(outline_texture.Height * Scale)), Color.White);

            /* Inner loses width as HP is lost */
            spriteBatch.Draw(inner_texture, new Rectangle((int)Location.X, (int)Location.Y, (int)(inner_texture.Width * Scale * percent), (int)(inner_texture.Height * Scale)), Color.White); 
        }
    }
}
