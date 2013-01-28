using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arena.Sprites
{
    class Tank: Sprite
    {
        public Texture2D tank_body_texture;
        public Texture2D tank_body_texture_right;
        public Texture2D tank_cannon_texture;

        public Texture2D tank_body_to_draw;

        public float scale = .65f;

        public Vector2 cannon_offset_up = new Vector2(21, 40);

        public float cannon_rotation = 0.0f;

        public float tank_rotation = 0.0f;

        public Vector2 tank_offset = new Vector2(21, 40);

        public Vector2 Position;

        public SpriteEffects Orientation;

        public Tank()
        {
        }

        public Tank(Texture2D tbt, Texture2D tct, Texture2D tbtr)
        {
            tank_body_texture = tbt;
            tank_cannon_texture = tct;
            tank_body_texture_right = tbtr;
            tank_body_to_draw = tank_body_texture;

            Position = new Vector2(300, 400);
            Orientation = SpriteEffects.None;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tank_body_texture, Position + tank_offset, null, Color.White, tank_rotation, new Vector2(33, 70), scale, Orientation, 0f);
            spriteBatch.Draw(tank_cannon_texture, Position + cannon_offset_up, null, Color.White, cannon_rotation, new Vector2(18, 71), scale, SpriteEffects.None, 0f);
        }
    }
}
