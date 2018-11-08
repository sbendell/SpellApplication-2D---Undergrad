using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpellApp
{
    class Spell : BaseSpell
    {
        protected List<Rectangle> frames = new List<Rectangle>();
        public int frameWidth;
        public int frameHeight;
        private int currentFrame;
        private float frameTime = 0.1f;
        private float timeForCurrentFrame = 0.0f;
        public int CollisionRadius;

        private float rotation = 0.0f;
        private float scale = 1.0f;

        public Spell(
            Vector2 location,
            Texture2D texture,
            Rectangle initialFrame,
            string SpellName)
        {
            this.location = location;
            Texture = texture;

            frames.Add(initialFrame);
            frameWidth = initialFrame.Width;
            frameHeight = initialFrame.Height;

            spellName = SpellName;
        }

        public Vector2 Location
        {
            get { return location; }
            set { location = value; }
        }

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value % MathHelper.TwoPi; }
        }

        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        public int Frame
        {
            get { return currentFrame; }
            set
            {
                currentFrame = (int)MathHelper.Clamp(value, 0,
                frames.Count - 1);
            }
        }

        public float FrameTime
        {
            get { return frameTime; }
            set { frameTime = MathHelper.Max(0, value); }
        }

        public Rectangle Source
        {
            get { return frames[currentFrame]; }
        }

        public Vector2 Center
        {
            get
            {
                return location +
                    new Vector2(frameWidth / 2, frameHeight / 2);
            }
        }

        public override void AddFrame(Rectangle frameRectangle)
        {
            frames.Add(frameRectangle);
        }

        public override void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            timeForCurrentFrame += elapsed;

            if (timeForCurrentFrame >= FrameTime)
            {
                currentFrame = (currentFrame + 1) % (frames.Count);
                timeForCurrentFrame = 0.0f;
            }

            location += (velocity * elapsed);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                Texture,
                Center,
                Source,
                Color.White,
                rotation,
                new Vector2(frameWidth / 2, frameHeight / 2),
                scale,
                SpriteEffects.None,
                0.0f);
        }
    }
}
