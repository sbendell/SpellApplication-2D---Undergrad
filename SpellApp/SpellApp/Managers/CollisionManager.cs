using Microsoft.Xna.Framework;

namespace SpellApp
{
    class CollisionManager
    {
        PlayerManager playerManager;
        SpellManager spellManager;
        TextManager textManager;
        LevelManager levelManager;

        Vector2 offScreen = new Vector2(-500, -500);

        public CollisionManager(
            PlayerManager PlayerManager,
            EnemyManager EnemyManager,
            SpellManager SpellManager,
            TextManager TextManager,
            LevelManager LM)
        {
            playerManager = PlayerManager;
            spellManager = SpellManager;
            textManager = TextManager;
            levelManager = LM;
        }

        public void DetectSpellToEnemies()
        {
            foreach (DamageSpell spell in spellManager.projSpells)
            {
                if (levelManager.enemyManager.bossSprite.IsCircleColliding(spell.Center, spell.CollisionRadius))
                {
                    textManager.CreateText(levelManager.enemyManager.bossSprite.Top,
                        (((int)spell.Damage(spell.spellPower)).ToString()));
                    levelManager.enemyManager.bossHealth -= (int)spell.Damage(spell.spellPower);
                    spell.Location = offScreen;
                }

                foreach (Enemy enemy in levelManager.enemyManager.enemies)
                {
                    if (enemy.IsCircleColliding(spell.Center, spell.CollisionRadius))
                    {
                        textManager.CreateText(enemy.Top,
                            (((int)spell.Damage(spell.spellPower)).ToString()));
                        enemy.EnemyHealth -= (int)spell.Damage(spell.spellPower);
                        spell.Location = offScreen;
                    }
                }
            }
            foreach (LocationSpell spell in spellManager.locationSpells)
            {
                if (levelManager.enemyManager.bossSprite.CollisionRectangle.Intersects(spell.CollisionRectangle))
                {
                    if (spell.HasHit == false)
                    {
                        textManager.CreateText(levelManager.enemyManager.bossSprite.Top,
                            (((int)spell.Damage(spell.spellPower)).ToString()));
                        levelManager.enemyManager.bossHealth -= (int)spell.Damage(spell.spellPower);
                        spell.HasHit = true;
                    }
                }

                foreach (Enemy enemy in levelManager.enemyManager.enemies)
                {
                    if (enemy.CollisionRectangle.Intersects(spell.CollisionRectangle))
                    {
                        if (enemy.hitBySpellID.Contains(spell.spellID) == false)
                        {
                            textManager.CreateText(enemy.Top,
                            (((int)spell.Damage(spell.spellPower)).ToString()));
                            enemy.EnemyHealth -= (int)spell.Damage(spell.spellPower);
                            enemy.hitBySpellID.Add(spell.spellID);
                        }
                    }
                }
            }
            foreach (LineSpell spell in spellManager.lineSpells)
            {
                if (levelManager.enemyManager.bossSprite.IsCircleColliding(spell.Top, 30) ||
                    levelManager.enemyManager.bossSprite.IsCircleColliding(spell.Center, 30) ||
                    levelManager.enemyManager.bossSprite.IsCircleColliding(spell.Bottom, 30))
                {
                    if (spell.HasHit == false)
                    {
                        textManager.CreateText(levelManager.enemyManager.bossSprite.Top,
                            (((int)spell.Damage(spell.spellPower)).ToString()));
                        levelManager.enemyManager.bossHealth -= (int)spell.Damage(spell.spellPower);
                        spell.HasHit = true;
                    }
                }
                foreach (Enemy enemy in levelManager.enemyManager.enemies)
                {
                    if (enemy.IsCircleColliding(spell.Top, 30) ||
                    enemy.IsCircleColliding(spell.Center, 30) ||
                    enemy.IsCircleColliding(spell.Bottom, 30))
                    {
                        if (enemy.hitBySpellID.Contains(spell.spellID) == false)
                        {
                            textManager.CreateText(enemy.Top,
                            (((int)spell.Damage(spell.spellPower)).ToString()));
                            enemy.EnemyHealth -= (int)spell.Damage(spell.spellPower);
                            enemy.hitBySpellID.Add(spell.spellID);
                        }
                    }
                }
            }
        }

        public void DetectEnemyToPlayer()
        {
            for (int x = levelManager.enemyManager.enemies.Count - 1; x >= 0; x--)
            {
                if (levelManager.enemyManager.enemies[x].IsCircleColliding(playerManager.playerSprite.Center, playerManager.playerSprite.CollisionRadius))
                {
                    playerManager.OutOfCombat = false;
                    playerManager.OutOfCombatTimer = 0;
                    playerManager.currentHealth -= (40 * levelManager.currentLevel);
                    levelManager.enemyManager.enemies.RemoveAt(x);
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            DetectSpellToEnemies();
            DetectEnemyToPlayer();
        }
    }
}
