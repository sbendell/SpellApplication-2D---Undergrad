using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpellApp
{
    class EnemyManager
    {
        PlayerManager playerManager;
        LevelManager levelManager;

        public List<Enemy> enemies = new List<Enemy>();
        public string[] currentEnemy = { "RedGhost", "skeleton" };

        public Sprite bossSprite;
        SpriteFont healthFont;
        float enemySpeed = 200f;
        Rectangle bossAreaLimit;

        Texture2D enemyTexture;
        Rectangle enemyInitialFrame;
        float spawnTimer;
        Random rand = new Random();

        public int bossHealth = 5000;
        public int bossMaxHealth = 5000;
        private int direction = 0;

        public readonly Rectangle redGhostInitialFrame = new Rectangle(0, 101, 25, 35);
        public readonly Rectangle skeletonInitialFrame = new Rectangle(0, 141, 36, 48);

        public EnemyManager(
            Texture2D bossTexture,
            SpriteFont HealthFont,
            Rectangle bossInitialFrame,
            int frameCount,
            Rectangle screenBounds,
            PlayerManager PlayerManager,
            LevelManager LM)
        {
            bossSprite = new Sprite(
                new Vector2(400, 400),
                bossTexture,
                bossInitialFrame,
                Vector2.Zero);

            bossAreaLimit = new Rectangle(200, 100, 400, 500);

            for (int x = 1; x < frameCount; x++)
            {
                bossSprite.AddFrame(
                    new Rectangle(
                        bossInitialFrame.X + (bossInitialFrame.Width * x),
                        bossInitialFrame.Y,
                        bossInitialFrame.Width,
                        bossInitialFrame.Height));
            }
            healthFont = HealthFont;
            playerManager = PlayerManager;
            levelManager = LM;
            enemyTexture = bossTexture;
            enemyInitialFrame = bossInitialFrame;
            bossSprite.CollisionRadius = 40;
        }

        public int CalculateExperienceWorth()
        {
            return levelManager.currentLevel * 25;
        }

        public void MoveToPlayer(Vector2 playerLocation)
        {
            foreach(Enemy enemy in enemies)
            {
                Vector2 velocity = playerLocation - enemy.Location;

                enemy.Velocity = Movement.Direction(velocity) * enemySpeed;
            }
        }

        private Vector2 chooseSpawnLocation()
        {
            int x = rand.Next(0, 4);

            if (x == 0)
            {
                return new Vector2(
                        0, rand.Next(0, 768));
            }
            if (x == 1)
            {
                return new Vector2(
                        768, rand.Next(0, 768));
            }
            if (x == 2)
            {
                return new Vector2(
                        rand.Next(0, 768), 0);
            }
            if (x == 3)
            {
                return new Vector2(
                        rand.Next(0, 768), 768);
            }
            else
            {
                return Vector2.Zero;
            }
        }

        private void imposeMovementLimits()
        {
            Vector2 location = bossSprite.Location;

            if (location.X < bossAreaLimit.X)
            {
                location.X = bossAreaLimit.X;
                direction = rand.Next(0, 8);
            }

            if (location.X >
                (bossAreaLimit.Right - bossSprite.Source.Width))
            {
                location.X =
                    (bossAreaLimit.Right - bossSprite.Source.Width);
                direction = rand.Next(0, 8);
            }

            if (location.Y < bossAreaLimit.Y)
            {
                location.Y = bossAreaLimit.Y;
                direction = rand.Next(0, 8);
            }

            if (location.Y >
                (bossAreaLimit.Bottom - bossSprite.Source.Height))
            {
                location.Y =
                    (bossAreaLimit.Bottom - bossSprite.Source.Height);
                direction = rand.Next(0, 8);
            }

            bossSprite.Location = location;
        }

        public void AddEnemy(string enemy)
        {
            if (enemy == "RedGhost")
            {
                Enemy thisRedGhost = new Enemy(
                    Vector2.Zero,
                    enemyTexture,
                    redGhostInitialFrame,
                    Vector2.Zero,
                    500,
                    CalculateExperienceWorth());

                for (int x = 1; x < 10; x++)
                {
                    thisRedGhost.AddFrame(
                        new Rectangle(
                            redGhostInitialFrame.X + (redGhostInitialFrame.Width * x),
                            redGhostInitialFrame.Y,
                            redGhostInitialFrame.Width,
                            redGhostInitialFrame.Height));
                }
                thisRedGhost.Location = chooseSpawnLocation();
                thisRedGhost.CollisionRadius = 15;
                enemies.Add(thisRedGhost);
            }
            if (enemy == "skeleton")
            {
                Enemy thisSkeleton = new Enemy(
                    Vector2.Zero,
                    enemyTexture,
                    skeletonInitialFrame,
                    Vector2.Zero,
                    500,
                    CalculateExperienceWorth());

                for (int x = 1; x < 10; x++)
                {
                    thisSkeleton.AddFrame(
                        new Rectangle(
                            skeletonInitialFrame.X + (skeletonInitialFrame.Width * x),
                            skeletonInitialFrame.Y,
                            skeletonInitialFrame.Width,
                            skeletonInitialFrame.Height));
                }
                thisSkeleton.Location = chooseSpawnLocation();
                thisSkeleton.CollisionRadius = 15;
                enemies.Add(thisSkeleton);
            }
        }

        private Vector2 randomDirection()
        {
            if (direction == 0)
            {
                return new Vector2(0, 1);
            }
            else if (direction == 1)
            {
                return new Vector2(0, -1);
            }
            else if (direction == 2)
            {
                return new Vector2(1, 0);
            }
            else if (direction == 3)
            {
                return new Vector2(-1, 0);
            }
            else if (direction == 4)
            {
                return new Vector2(1, 1);
            }
            else if (direction == 5)
            {
                return new Vector2(-1, 1);
            }
            else if (direction == 6)
            {
                return new Vector2(1, -1);
            }
            else
            {
                return new Vector2(-1, -1);
            }
        }

        public void Update(GameTime gameTime)
        {
            bossSprite.Velocity = Vector2.Zero;
            bossSprite.Velocity += randomDirection();
            bossSprite.Velocity *= 160f;
            imposeMovementLimits();
            bossSprite.Update(gameTime);

            if (playerManager.EndOfLevel == false)
            {
                spawnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (spawnTimer > 3f)
            {
                int enemyNumber = rand.Next(0, 2);
                string enemyString = currentEnemy[enemyNumber];

                AddEnemy(enemyString);
                spawnTimer = 0f;
            }

            if (bossHealth <= 0)
            {
                playerManager.EndOfLevel = true;
                levelManager.ChangeLevel(bossHealth, bossMaxHealth);
            }

            for(int x = enemies.Count - 1; x >= 0; x--)
            {
                if (enemies[x].IsAlive)
                {
                    enemies[x].Update(gameTime);
                }
                else
                {
                    playerManager.playerExperience += enemies[x].ExpWorth;
                    enemies.RemoveAt(x);
                }
            }
            MoveToPlayer(playerManager.playerSprite.Location);

            bossMaxHealth = levelManager.currentLevel * 5000;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (bossHealth >= 0)
            {
                bossSprite.Draw(spriteBatch);
            }
            foreach(Enemy enemy in enemies)
            {
                enemy.Draw(spriteBatch);
            }
        }
    }
}
