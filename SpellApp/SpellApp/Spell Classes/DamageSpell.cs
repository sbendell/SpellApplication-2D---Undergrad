using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpellApp
{
    class DamageSpell : Spell
    {
        private float spellPowerMultiplier;
        public double spellPower;

        public DamageSpell(
            Vector2 location,
            Texture2D texture,
            Rectangle initialFrame,
            string SpellName,
            double SpellPower,
            float sppwmlt)
            :base (location,
                 texture,
                 initialFrame,
                 SpellName)
        {
            spellPowerMultiplier = sppwmlt;
            spellPower = SpellPower;
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
        public double Damage(double spellPower)
        {
            return (spellPower * spellPowerMultiplier);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
