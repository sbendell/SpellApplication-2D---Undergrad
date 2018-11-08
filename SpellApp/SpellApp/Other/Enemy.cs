using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpellApp
{
    class Enemy : Sprite
    {
        public List<int> hitBySpellID = new List<int>();

        private int enemyHealth;
        private int startHealth;
        private int expWorth;

        public Enemy(
            Vector2 location,
            Texture2D texture,
            Rectangle initialFrame,
            Vector2 velocity,
            int EnemyHealth,
            int ExpWorth)
            :base (location, texture, initialFrame, velocity)
        {
            enemyHealth = EnemyHealth;
            startHealth = EnemyHealth;
            expWorth = ExpWorth;
        }

        public int EnemyHealth
        {
            get { return enemyHealth; }
            set {
                if (enemyHealth <= 0)
                {
                    enemyHealth = 0;
                }
                else
                {
                    enemyHealth = value;
                }
            }
        }

        public int ExpWorth
        {
            get { return expWorth; }
        }

        public bool IsAlive
        {
            get
            {
                if (enemyHealth > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private float HealthBarFloat
        {
            get { return ((float)enemyHealth / (float)startHealth); }
        }

        private int healthBarRectWidth(float healthBarFloat)
        {
            return (int)(40 * healthBarFloat);
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
            base.Draw(spriteBatch);

            spriteBatch.Draw(
                Texture,
                new Rectangle((int)location.X, (int)Top.Y -10, healthBarRectWidth(HealthBarFloat), 5),
                new Rectangle(0, 900, 2, 2),
                Color.White);
        }
    }
}
