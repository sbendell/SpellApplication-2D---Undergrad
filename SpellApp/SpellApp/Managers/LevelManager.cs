using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpellApp
{
    class LevelManager
    {
        PlayerManager playerManager;
        public EnemyManager enemyManager;

        public int currentLevel;
        Rectangle clientBounds;

        public LevelManager(int startLevel,
            Texture2D texture,
            SpriteFont font,
            Rectangle ClientBounds,
            PlayerManager PM,
            LevelManager LM)
        {
            currentLevel = startLevel;
            clientBounds = ClientBounds;
            playerManager = PM;

            enemyManager = new EnemyManager(
                texture,
                font,
                new Rectangle(0, 0, 96, 96),
                16,
                ClientBounds,
                playerManager,
                LM);
        }

        public void ChangeLevel(int bossHp, int bossMaxHp)
        {
            if (playerManager.playerSprite.Location.X < clientBounds.Width + 1000 && playerManager.EndOfLevel)
            {
                playerManager.playerSprite.Location += new Vector2(3, 0);
            }
            if (playerManager.playerSprite.Location.X > clientBounds.Width)
            {
                playerManager.EndOfLevel = false;
                playerManager.playerSprite.Location = Vector2.Zero;
                currentLevel++;
                playerManager.currentHealth = playerManager.maxHealth;
            }
        }
    }
}
