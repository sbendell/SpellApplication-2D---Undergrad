using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpellApp
{
    class TextManager
    {
        PlayerManager playerManager;
        EnemyManager enemyManager;

        public List<MovingText> damageText = new List<MovingText>();

        SpriteFont font;

        Rectangle screenRect;

        Text playerHealth;
        Text playerSpellPower;
        Text playerSpeed;
        Text playerHPregen;
        Text playerLevel;

        Text bossHealth;
        Text bossSpellPower;

        public TextManager(
            SpriteFont Font,
            Rectangle screenBounds,
            PlayerManager PlayerManager,
            EnemyManager EnemyManager)
        {
            font = Font;
            screenRect = screenBounds;
            playerManager = PlayerManager;
            enemyManager = EnemyManager;

            playerHealth = new Text(
                font,
                new Vector2(3, 3),
                "Left");

            playerSpellPower = new Text(
                font,
                new Vector2(3, 18),
                "Left");

            playerSpeed = new Text(
                font,
                new Vector2(3, 33),
                "Left");

            playerHPregen = new Text(
                font,
                new Vector2(3, 48),
                "Left");

            playerLevel = new Text(
                font,
                new Vector2(3, 63),
                "Left");

            bossHealth = new Text(
                font,
                new Vector2(screenRect.Width - 3, 3),
                "Right");

            bossSpellPower = new Text(
                font,
                new Vector2(screenRect.Width - 3, 18),
                "Right");
        }

        public void CreateText(Vector2 creationLocation, string damage)
        {
            MovingText text = new MovingText(
                font,
                creationLocation,
                "Center",
                damage);

            damageText.Add(text);
        }

        public void Update(GameTime gameTime)
        {
            for (int x = damageText.Count - 1; x >= 0; x--)
            {
                if (damageText[x].IsActive)
                {
                    damageText[x].Update(gameTime);
                }
                else
                {
                    damageText.RemoveAt(x);
                }
            }

            playerHealth.TextValue = ("Health: " + ((int)playerManager.currentHealth).ToString() + "/" + ((int)playerManager.maxHealth).ToString());
            playerSpellPower.TextValue = ("Spellpower: " + ((int)playerManager.spellpower).ToString());
            playerSpeed.TextValue = ("Speed: " + (playerManager.playerSpeed / 200).ToString());
            playerHPregen.TextValue = ("HP/s: " + ((int)playerManager.regen).ToString());
            playerLevel.TextValue = ("Level: " + playerManager.playerLevel);

            bossHealth.TextValue = ("Boss HP: " + ((int)enemyManager.bossHealth).ToString() + "/" + ((int)enemyManager.bossMaxHealth).ToString());
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(Text text in damageText)
            {
                text.Draw(spriteBatch);
            }

            playerHealth.Draw(spriteBatch);
            playerSpellPower.Draw(spriteBatch);
            playerSpeed.Draw(spriteBatch);
            playerHPregen.Draw(spriteBatch);
            playerLevel.Draw(spriteBatch);

            bossHealth.Draw(spriteBatch);
            bossSpellPower.Draw(spriteBatch);
        }
    }
}
