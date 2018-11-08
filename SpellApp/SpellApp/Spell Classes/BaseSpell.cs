using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpellApp
{
    abstract class BaseSpell
    {
        public Texture2D Texture;

        protected string spellName;
        protected Vector2 location = Vector2.Zero;
        protected Vector2 velocity = Vector2.Zero;

        public abstract void AddFrame(Rectangle frameRectangle);
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
