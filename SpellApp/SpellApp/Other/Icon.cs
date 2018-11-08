using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpellApp
{
    class Icon
    {
        private Texture2D texture;
        private Color finalColor;
        private Rectangle frame;
        protected Vector2 location = Vector2.Zero;

        public Icon(
            Texture2D Texture,
            Vector2 Location,
            Rectangle Frame)
        {
            texture = Texture;
            location = Location;
            frame = Frame;
        }

        public Color FinalColor
        {
            get { return finalColor; }
            set { finalColor = value; }
        }

        public Vector2 Center
        {
            get
            {
                return location +
                    new Vector2(frame.Width / 2, frame.Height / 2);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                texture,
                Center,
                new Rectangle(0, 0, 32, 32),
                Color.White);

            spriteBatch.Draw(
                texture,
                Center,
                frame,
                finalColor);
        }
    }
}
