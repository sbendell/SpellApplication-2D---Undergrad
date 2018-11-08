using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpellApp
{
    class Text
    {
        public SpriteFont font;
        private string textValue = "";
        private Vector2 location = Vector2.Zero;
        string AlignTo;
        Vector2 origin;

        public Text(
            SpriteFont Font,
            Vector2 startLocation,
            string alignTo)
        {
            font = Font;
            location = startLocation;
            AlignTo = alignTo;
        }

        public Vector2 Location
        {
            get { return location; }
            set { location = value; }
        }

        public string TextValue
        {
            get { return textValue; }
            set { textValue = value; }
        }

        private void DrawText(SpriteBatch spriteBatch, string text, Color backColor, Color frontColor, float scale, float rotation, Vector2 position)
        {
            if (AlignTo == "Left")
            {
                origin = Vector2.Zero;
            }
            if (AlignTo == "Center")
            {
                origin = new Vector2(font.MeasureString(text).X / 2, 0);
            }
            if (AlignTo == "Right")
            {
                origin = new Vector2(font.MeasureString(text).X, 0);
            }

            spriteBatch.DrawString(font,
                text,
                position + new Vector2(1 * scale, 1 * scale),
                backColor,
                rotation,
                origin,
                scale,
                SpriteEffects.None,
                1f);

            spriteBatch.DrawString(font,
                text, 
                position + new Vector2(-1 * scale, -1 * scale),
                backColor,
                rotation,
                origin,
                scale,
                SpriteEffects.None,
                1f);

            spriteBatch.DrawString(font,
                text,
                position + new Vector2(-1 * scale, 1 * scale),
                backColor,
                rotation,
                origin,
                scale,
                SpriteEffects.None,
                1f);

            spriteBatch.DrawString(font,
                text, position + new Vector2(1 * scale, -1 * scale),
                backColor,
                rotation,
                origin,
                scale,
                SpriteEffects.None,
                1f);

            spriteBatch.DrawString(font,
                text,
                position,
                frontColor,
                rotation,
                origin,
                scale,
                SpriteEffects.None,
                1f);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawText(spriteBatch,
                textValue,
                Color.Black,
                Color.White,
                1f,
                0.0f,
                location);
        }
    }
}
