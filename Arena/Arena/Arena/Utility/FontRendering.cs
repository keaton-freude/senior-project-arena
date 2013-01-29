using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arena.Utility
{
    public class FontRendering
    {
        public static void DrawOutlinedText(SpriteBatch sb, string text,
            Color backColor, Color frontColor, float scale, float rotation,
            Vector2 position, SpriteFont font, int thickness)
        {
            Vector2 origin = Vector2.Zero;
            sb.Begin();
            sb.DrawString(font, text, position + new Vector2(thickness * scale, thickness * scale),
                backColor, rotation, origin, scale, SpriteEffects.None, 1f);
            sb.DrawString(font, text, position + new Vector2(-thickness * scale, -thickness * scale),
                backColor, rotation, origin, scale, SpriteEffects.None, 1f);
            sb.DrawString(font, text, position + new Vector2(-thickness * scale, thickness * scale),
                backColor, rotation, origin, scale, SpriteEffects.None, 1f);
            sb.DrawString(font, text, position + new Vector2(thickness * scale, -thickness * scale),
                backColor, rotation, origin, scale, SpriteEffects.None, 1f);

            sb.DrawString(font, text, position, frontColor, rotation, origin, scale, SpriteEffects.None,
                1f);
            sb.End();
        }
    }
}
