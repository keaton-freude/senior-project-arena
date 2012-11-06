using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Arena.FloatingText
{
    public class FloatingTextEngine
    {
        private static FloatingTextEngine _instance;
        public ContentManager _content;

        List<FloatingText> Texts;

        public static FloatingTextEngine GetInstance()
        {
            if (_instance == null)
                _instance = new FloatingTextEngine();
            return _instance;
        }

        public SpriteFont TextFont
        {
            get;
            set;
        }

        private FloatingTextEngine()
        {
            Texts = new List<FloatingText>();
        }

        public void AddText(String text, Vector2 Position, Vector2 Direction, Vector2 Speed, float TTL, Color TextColor)
        {
            if (TextFont != null)
                Texts.Add(new FloatingText(text, Position, Direction, Speed, TTL, TextColor, TextFont));
        }

        public void Update(GameTime gameTime)
        {
            List<FloatingText> text_to_remove = new List<FloatingText>();

            foreach (FloatingText FT in Texts)
            {
                FT.Update(gameTime);

                if (FT.Dirty)
                    text_to_remove.Add(FT);
            }

            foreach (FloatingText FT in text_to_remove)
                Texts.Remove(FT);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (FloatingText FT in Texts)
                FT.Draw(spriteBatch);
        }
    }
}
