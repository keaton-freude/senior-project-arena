using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arena.Sprites
{
    public class Tank: Sprite
    {
        public Texture2D tank_body_texture;
        public Texture2D tank_body_texture_right;
        public Texture2D tank_cannon_texture;

        public int tank_dust_effect = -1;

        public Texture2D tank_body_to_draw;

        public float scale = .65f;

        public Vector2 cannon_offset_up = new Vector2(21, 40);

        public float cannon_rotation = 0.0f;

        public float tank_rotation = 0.0f;

        public RectangleOverlay debug_overlay;

        public Vector2 tank_offset = new Vector2(21, 40);

        public Vector2 Position;

        public SpriteEffects Orientation;

        public UI.HealthBar health;

        public Rectangle CollisionRect
        {
            get
            {
                Rectangle toReturn = Rectangle.Empty;
                if (tank_rotation == 0.0 || tank_rotation == 3.14f)
                    toReturn = new Rectangle((int)Position.X, (int)Position.Y+4, (int)(tank_body_texture.Width * scale), (int)(tank_body_texture.Height * scale) - 8);
                else if (tank_rotation == 3.14f / 2.0f || true)
                {
                    toReturn = new Rectangle((int)Position.X - 15, (int)Position.Y + 19, (int)(tank_body_texture.Height * scale) - 8, (int)(tank_body_texture.Width * scale));
                }
                return toReturn;
            }
        }


        public Tank()
        {
        }

        public Tank(Texture2D tbt, Texture2D tct, Texture2D tbtr)
        {
            health = new UI.HealthBar(100, 100);
            health.Scale = .6f;
            tank_body_texture = tbt;
            tank_cannon_texture = tct;
            tank_body_texture_right = tbtr;
            tank_body_to_draw = tank_body_texture;

            Position = new Vector2(300, 400);
            Orientation = SpriteEffects.None;

            debug_overlay = new RectangleOverlay(CollisionRect, Color.Green, Arena.Screens.TankShooters.gDevice);

            tank_dust_effect = ArenaParticleEngine.ParticleEngine.Instance.LoadFromFile("TankDustEffect", Arena.Screens.TankShooters.content);
        }

        public override void Update(GameTime gameTime)
        {
            health.Location = new Vector2(CollisionRect.X, CollisionRect.Y - 11);
            health.Update();
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            debug_overlay.dummyRectangle = CollisionRect;
            spriteBatch.Draw(tank_body_texture, Position + tank_offset, null, Color.White, tank_rotation, new Vector2(33, 70), scale, Orientation, 0f);
            spriteBatch.Draw(tank_cannon_texture, Position + cannon_offset_up, null, Color.White, cannon_rotation, new Vector2(18, 71), scale, SpriteEffects.None, 0f);
            debug_overlay.Draw(spriteBatch);
            health.Draw(spriteBatch);
        }
    }
}
