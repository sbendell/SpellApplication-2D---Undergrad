using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpellApp
{
    class BaseSprite
    {
        protected Texture2D texture;
        public Vector2 location;
        private Rectangle initialFrame;

        public BaseSprite(
            Texture2D Texture,
            Vector2 Location,
            Rectangle InitialFrame)
        {
            texture = Texture;
            location = Location;
            initialFrame = InitialFrame;
        }

        public Rectangle CollisionRect
        {
            get
            {
                return new Rectangle((int)location.X,
                    (int)location.Y,
                    initialFrame.Width,
                    initialFrame.Height);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                texture,
                location,
                initialFrame,
                Color.White);
        }
    }
}
