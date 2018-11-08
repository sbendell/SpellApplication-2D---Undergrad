using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpellApp
{
    class LocationSpell : DamageSpell
    {
        private int remainingduration;
        private float hitTimer = 6;
        public bool HasHit = false;
        public int spellID;

        public LocationSpell(
            Vector2 location,
            Texture2D texture,
            Rectangle initialFrame,
            string SpellName,
            double SpellPower,
            float sppwmlt,
            int duration,
            int SpellID)
            :base(location,
                 texture,
                 initialFrame,
                 SpellName,
                 SpellPower,
                 sppwmlt)
        {
            remainingduration = duration;
            spellID = SpellID;
        }

        public bool IsActive
        {
            get
            {
                return (remainingduration > 0);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            remainingduration--;
            hitTimer--;
            if (HasHit == false)
            {
                if (hitTimer > 0)
                {
                    HasHit = false;
                }
                else
                {
                    HasHit = true;
                }
            }
        }
    }
}
