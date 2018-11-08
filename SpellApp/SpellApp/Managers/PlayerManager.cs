using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace SpellApp
{
    class PlayerManager
    {
        SpellManager spellManager;

        public Sprite playerSprite;
        SpriteFont healthFont;

        public bool EndOfLevel = false;

        private int intelligence;
        private int agility;
        private int stamina;
        private int strength;

        private Rectangle playerAreaLimit;
        public float playerSpeed;
        public bool playerAlive = true;
        public int playerLevel = 1;
        public int playerExperience;
        public int experienceForNextLevel;

        private float fireBallTimer;
        private float lightningBoltTimer;
        private float lightningLineTimer;
        private float arcaneExplosionTimer;

        public bool lightningBoltUnlocked = false;
        public bool lightningLineUnlocked = false;
        public bool arcaneExplosionUnlocked = false;
        public bool arcanePowerUnlocked = false;

        public double spellpower;
        public int maxHealth;
        public int currentHealth = 5000;

        public bool OutOfCombat = true;
        public float OutOfCombatTimer = 15f;
        private float OutOfCombatInterval = 10f;
        public double regen;
        private float healingInterval = 1.0f;
        private float healingTimer;

        public PlayerManager(
            Texture2D playerTexture,
            SpriteFont HealthFont,
            Rectangle playerInitialFrame,
            int frameCount,
            Rectangle screenBounds,
            SpellManager SpellManager)
        {
            playerSprite = new Sprite(
                new Vector2(100, 100),
                playerTexture,
                playerInitialFrame,
                Vector2.Zero);

            playerAreaLimit = new Rectangle(
                0,
                0,
                screenBounds.Width,
                screenBounds.Height);

            for (int x = 1; x < frameCount; x++)
            {
                playerSprite.AddFrame(
                    new Rectangle(
                        playerInitialFrame.X + (playerInitialFrame.Width * x),
                        playerInitialFrame.Y,
                        playerInitialFrame.Width,
                        playerInitialFrame.Height));
            }
            spellManager = SpellManager;
            healthFont = HealthFont;
            playerSprite.CollisionRadius = 20;
        }

        private float Cooldown(int intelligence, float cooldown)
        {
            if (intelligence < 50)
            {
                return (cooldown * (1 - (0.01f * intelligence)));
            }
            else
            {
                return (cooldown * 0.5f);
            }
        }

        public float getCooldown(string spell)
        {
            if (spell == "Fireball")
            {
                return fireBallTimer;
            }
            if (spell == "LightningBolt")
            {
                return lightningBoltTimer;
            }
            if (spell == "LightningLine")
            {
                return lightningLineTimer;
            }
            if (spell == "ArcaneExplosion")
            {
                return arcaneExplosionTimer;
            }
            if (spell == "ArcanePower")
            {
                return spellManager.ArcanePower.cooldown;
            }
            else
            {
                return 0;
            }
        }

        private void HandleKeyboardInput(KeyboardState keyState, MouseState mouseState)
        {
            if (keyState.IsKeyDown(Keys.D1))
            {
                if (fireBallTimer <= 0)
                {
                    spellManager.CastSpell(new Vector2(playerSprite.Location.X, playerSprite.Location.Y),
                        new Vector2(mouseState.X - (spellManager.fireballInitialFrame.Width / 2), mouseState.Y - (spellManager.fireballInitialFrame.Height / 2)),
                        "Fireball",
                        spellpower);
                    fireBallTimer = Cooldown(intelligence, 0.6f);
                    OutOfCombatTimer = 0;
                    SoundManager.PlayerCastFireball();
                }
            }

            if (keyState.IsKeyDown(Keys.D2))
            {
                if (lightningBoltUnlocked)
                {
                    if (lightningBoltTimer <= 0)
                    {
                        spellManager.CastSpell(new Vector2(playerSprite.Location.X, playerSprite.Location.Y),
                            new Vector2(mouseState.X - (spellManager.lightningBoltInitialFrame.Width / 2), mouseState.Y - (spellManager.lightningBoltInitialFrame.Height / 2)),
                            "LightningBolt",
                            spellpower);
                        lightningBoltTimer = Cooldown(intelligence, 3f);
                        OutOfCombatTimer = 0;
                        SoundManager.PlayerCastLightningBall();
                    }
                }
            }

            if (keyState.IsKeyDown(Keys.D3))
            {
                if (lightningLineUnlocked)
                {
                    if (lightningLineTimer <= 0)
                    {
                        spellManager.CastSpell(new Vector2(playerSprite.Location.X, playerSprite.Location.Y - 48),
                            new Vector2(mouseState.X - (spellManager.lightningLineInitialFrame.Width / 2), mouseState.Y - (spellManager.lightningLineInitialFrame.Height / 2)),
                            "LightningLine",
                            spellpower);
                        lightningLineTimer = Cooldown(intelligence, 3f);
                        OutOfCombatTimer = 0;
                        SoundManager.PlayerCastLightningLine();
                    }
                }
            }

            if (keyState.IsKeyDown(Keys.D4))
            {
                if (arcaneExplosionUnlocked)
                {
                    if (arcaneExplosionTimer <= 0)
                    {
                        spellManager.CastSpell(new Vector2(playerSprite.Location.X, playerSprite.Location.Y - 48),
                            new Vector2(mouseState.X - (spellManager.arcaneExplosionInitialFrame.Width / 2), mouseState.Y - (spellManager.arcaneExplosionInitialFrame.Height / 2)),
                            "ArcaneExplosion",
                            spellpower);
                        arcaneExplosionTimer = Cooldown(intelligence, 6f);
                        OutOfCombatTimer = 0;
                    }
                }
            }

            if (keyState.IsKeyDown(Keys.D5))
            {
                if (arcanePowerUnlocked)
                {
                    if (spellManager.ArcanePower.cooldown <= 0)
                    {
                        spellManager.ArcanePower.IsActive = true;
                    }
                }
            }

            if (keyState.IsKeyDown(Keys.A))
            {
                playerSprite.Velocity += new Vector2(-1, 0);
                playerSprite.direction = "Left";
            }

            if (keyState.IsKeyDown(Keys.D))
            {
                playerSprite.Velocity += new Vector2(1, 0);
                playerSprite.direction = "Right";
            }

            if (keyState.IsKeyDown(Keys.W))
            {
                playerSprite.Velocity += new Vector2(0, -1);
                playerSprite.direction = "Up";
            }

            if (keyState.IsKeyDown(Keys.S))
            {
                playerSprite.Velocity += new Vector2(0, 1);
                playerSprite.direction = "Down";
            }
        }

        private void imposeMovementLimits()
        {
            Vector2 location = playerSprite.Location;

            if (location.X < playerAreaLimit.X)
                location.X = playerAreaLimit.X;

            if (location.X >
                (playerAreaLimit.Right - playerSprite.Source.Width))
                location.X =
                    (playerAreaLimit.Right - playerSprite.Source.Width);

            if (location.Y < playerAreaLimit.Y)
                location.Y = playerAreaLimit.Y;

            if (location.Y >
                (playerAreaLimit.Bottom - playerSprite.Source.Height))
                location.Y =
                    (playerAreaLimit.Bottom - playerSprite.Source.Height);

            playerSprite.Location = location;
        }

        private void HealToFull(GameTime gameTime)
        {
            if (OutOfCombatTimer < OutOfCombatInterval)
            {
                OutOfCombat = false;
                OutOfCombatTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                OutOfCombat = true;
            }
            if (currentHealth < maxHealth)
            {
                if (OutOfCombat)
                {
                    currentHealth += 3;
                }
                else
                {
                    healingTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (healingTimer > healingInterval)
                    {
                        currentHealth += (int)regen;
                        healingTimer = 0f;
                    }
                }
            }
            else
            {
                currentHealth = maxHealth;
            }
        }

        private void HandleTimers(GameTime gameTime)
        {
            fireBallTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            lightningBoltTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            lightningLineTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            arcaneExplosionTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        private void ManageStats()
        {
            intelligence = playerLevel * 7;
            stamina = playerLevel * 5;
            agility = playerLevel * 4;
            strength = playerLevel * 3;

            spellpower = ((intelligence * 3) * spellManager.ArcanePower.Buff());
            maxHealth = (stamina * 10);
            playerSpeed = (100 + agility) * 2f;
            regen = (strength * 0.3f);
        }

        private void CalculateLevelAndExp()
        {
            if (playerExperience >= experienceForNextLevel)
            {
                playerLevel += 1;
                playerExperience = 0;
                currentHealth = maxHealth;
            }
            experienceForNextLevel = (int)(100 * Math.Pow(1.5, playerLevel));
        }

        private void UnlockSpells()
        {
            if (playerLevel >= 2)
            {
                lightningBoltUnlocked = true;
            }
            if (playerLevel >= 3)
            {
                lightningLineUnlocked = true;
            }
            if (playerLevel >= 5)
            {
                arcaneExplosionUnlocked = true;
            }
            if (playerLevel >= 8)
            {
                arcanePowerUnlocked = true;
            }
        }

        public void Update(GameTime gameTime)
        {
            HandleTimers(gameTime);
            playerSprite.Velocity = Vector2.Zero;
            playerSprite.Velocity.Normalize();
            HealToFull(gameTime);
            ManageStats();
            CalculateLevelAndExp();
            UnlockSpells();

            spellpower = 10000;

            if (EndOfLevel == false)
            {
                imposeMovementLimits();
                HandleKeyboardInput(Keyboard.GetState(), Mouse.GetState());
            }

            playerSprite.Velocity *= playerSpeed;
            playerSprite.Update(gameTime);

            spellManager.ArcanePower.Location = (playerSprite.Location - new Vector2(16, 12));
            spellManager.ArcanePower.MaxCooldown = Cooldown(intelligence, 60000f);
            spellManager.ArcanePower.Update(gameTime);

            spellManager.Regenerate.Location = (playerSprite.Location - new Vector2(16, 12));
            spellManager.Regenerate.Update(gameTime);

            if (currentHealth >= 0)
            {
                playerAlive = true;
            }
            else
            {
                playerAlive = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            playerSprite.Draw(spriteBatch);

            if (spellManager.ArcanePower.IsActive)
            {
                spellManager.ArcanePower.Draw(spriteBatch);
            }
            if (OutOfCombat)
            {
                spellManager.Regenerate.Draw(spriteBatch);
            }
        }
    }
}
