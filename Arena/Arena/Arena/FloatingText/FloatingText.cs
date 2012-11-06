using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arena.FloatingText
{
    public class FloatingText
    {
        public String Text
        {
            get;
            set;
        }

        public SpriteFont TextFont
        {
            get;
            set;
        }

        public Color TextColor
        {
            get;
            set;
        }

        public Vector2 Direction
        {
            get;
            set;
        }

        public Vector2 Speed
        {
            get;
            set;
        }

        public Vector2 Position
        {
            get;
            set;
        }

        public float TTL
        {
            get;
            set;
        }

        private float _current_time;
        public Boolean Dirty
        {
            get;
            set;
        }

        public FloatingText()
        {
        }

        public FloatingText(String text, Vector2 Position, Vector2 Direction, Vector2 Speed, float TTL, Color TextColor, SpriteFont Font)
        {
            Text = text;
            this.Position = Position;
            this.Direction = Direction;
            this.Speed = Speed;
            this.TTL = TTL;
            this.TextColor = TextColor;
            Dirty = false;
            TextFont = Font;
        }

        public void Update(GameTime gameTime)
        {
            Position += Direction * Speed;
            _current_time += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_current_time >= TTL)
                Dirty = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(TextFont, Text, Position, TextColor);
        }
    }
}
