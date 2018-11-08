using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpellApp
{
    class IconManager
    {
        PlayerManager playerManager;

        SpriteFont font;
        int HotbarstartX = 222;

        Icon fireballIcon;
        Icon lightningBoltIcon;
        Icon lightningLineIcon;
        Icon ArcaneExplosionIcon;
        Icon ArcanePowerIcon;

        public IconManager(
            SpriteFont Font,
            Texture2D Texture,
            PlayerManager PlayerManager)
        {
            fireballIcon = new Icon(
                Texture,
                IconLocation(1),
                IconRectangle(1));

            lightningBoltIcon = new Icon(
                Texture,
                IconLocation(2),
                IconRectangle(2));

            lightningLineIcon = new Icon(
                Texture,
                IconLocation(3),
                IconRectangle(3));

            ArcaneExplosionIcon = new Icon(
                Texture,
                IconLocation(4),
                IconRectangle(4));

            ArcanePowerIcon = new Icon(
                Texture,
                IconLocation(5),
                IconRectangle(5));

            font = Font;
            playerManager = PlayerManager;
        }

        private Rectangle IconRectangle(int IconNumber)
        {
            return new Rectangle((32 * IconNumber), 0, 32, 32);
        }

        private Vector2 IconLocation(int IconNumber)
        {
            return new Vector2(HotbarstartX + (34 * (IconNumber - 1)), 715);
        }

        public void Update(GameTime gameTime)
        {
            fireballIcon.FinalColor = Color.Lerp(
                Color.Red,
                Color.White,
                playerManager.getCooldown("Fireball"));
            lightningBoltIcon.FinalColor = Color.Lerp(
                Color.Blue,
                Color.White,
                playerManager.getCooldown("LightningBolt"));
            lightningLineIcon.FinalColor = Color.Lerp(
                Color.Blue,
                Color.White,
                playerManager.getCooldown("LightningLine"));
            ArcaneExplosionIcon.FinalColor = Color.Lerp(
                Color.Purple,
                Color.White,
                playerManager.getCooldown("ArcaneExplosion"));
            ArcanePowerIcon.FinalColor = Color.Lerp(
                Color.Gold,
                Color.White,
                playerManager.getCooldown("ArcanePower"));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            fireballIcon.Draw(spriteBatch);
            if (playerManager.lightningBoltUnlocked)
            {
                lightningBoltIcon.Draw(spriteBatch);
            }
            if (playerManager.lightningLineUnlocked)
            {
                lightningLineIcon.Draw(spriteBatch);
            }
            if (playerManager.arcaneExplosionUnlocked)
            {
                ArcaneExplosionIcon.Draw(spriteBatch);
            }
            if (playerManager.arcanePowerUnlocked)
            {
                ArcanePowerIcon.Draw(spriteBatch);
            }
        }
    }
}
