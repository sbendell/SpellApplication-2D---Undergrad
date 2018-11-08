using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpellApp
{
    class Sprite
    {
        protected Texture2D Texture;

        public string direction = "Down";
        public bool flipHorizontally = false;

        protected List<Rectangle> frames = new List<Rectangle>();
        private int frameWidth = 0;
        private int frameHeight = 0;
        public int currentFrame;
        private float frameTime = 0.1f;
        public float timeForCurrentFrame = 0.0f;
        public int CollisionRadius;

        protected Vector2 location = Vector2.Zero;
        protected Vector2 velocity = Vector2.Zero;

        public Sprite(
            Vector2 location,
            Texture2D texture,
            Rectangle initialFrame,
            Vector2 velocity)
        {
            this.location = location;
            Texture = texture;
            this.velocity = velocity;

            frames.Add(initialFrame);
            frameWidth = initialFrame.Width;
            frameHeight = initialFrame.Height;
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

        public Vector2 Top
        {
            get
            {
                return location +
                    new Vector2(frameWidth / 2, 0);
            }
        }

        public Rectangle CollisionRectangle
        {
            get
            {
                return new Rectangle(
                    (int)Location.X,
                    (int)Location.Y,
                    frameWidth,
                    frameHeight);
            }
        }

        public bool IsCircleColliding(Vector2 otherCenter, float otherRadius)
        {
            if (Vector2.Distance(Center, otherCenter) <
                (CollisionRadius + otherRadius))
                return true;
            else
                return false;
        }

        public SpriteEffects Flip(bool flip)
        {
            if (flip)
            {
                return SpriteEffects.FlipHorizontally;
            }
            else
            {
                return SpriteEffects.None;
            }
        }

        public void AddFrame(Rectangle frameRectangle)
        {
            frames.Add(frameRectangle);
        }

        public virtual void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            timeForCurrentFrame += elapsed;

            if (timeForCurrentFrame >= FrameTime)
            {
                timeForCurrentFrame = 0.0f;
                currentFrame = (currentFrame + 1) % (frames.Count);

                if (direction == "Down")
                {
                    if (currentFrame > 3)
                    {
                        currentFrame = 0;
                    }
                }
                else if (direction == "Left")
                {
                    if (currentFrame < 4 || currentFrame > 7)
                    {
                        currentFrame = 4;
                    }
                }
                else if (direction == "Right")
                {
                    if (currentFrame < 8 || currentFrame > 11)
                    {
                        currentFrame = 8;
                    }
                }
                else if (direction == "Up")
                {
                    if (currentFrame < 12 || currentFrame > 15)
                    {
                        currentFrame = 12;
                    }
                }
            }

            location += (velocity * elapsed);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                Texture,
                Center,
                Source,
                Color.White,
                0.0f,
                new Vector2(frameWidth / 2, frameHeight / 2),
                1.0f,
                Flip(flipHorizontally),
                0.0f);
        }
    }
}
